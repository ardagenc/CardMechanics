using DG.Tweening;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private Card card;

    [SerializeField] private Transform cardVisual;

    Vector3 movementDelta;
    Vector3 rotationDelta;


    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationAmount;

    [Header("Tilt")]
    [SerializeField] private float tiltAmount;
    [SerializeField] private float tiltSpeed;

    [Header("Tweeners")]
    Tweener tiltTween;
    Tweener defaultRotationTween;


    private void Awake()
    {
        card.PointerEnterEvent.AddListener(PointerEnter);
        card.PointerExitEvent.AddListener(PointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
    }
    void Update()
    {        
        CardTilt();
        CardRotation();


    }

    private void CardRotation()
    {
        if (!card.IsDragging) return;

        Vector3 movement = card.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = (card.IsDragging ? movementDelta : movement) * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -40, 40));
    }
    private void CardTilt()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);       
        Vector3 rotationValue = (mousePosition - transform.position).normalized;
        rotationValue.z = 0;

        if (card.IsHovering)
        {
            Vector3 tilt = new Vector3(rotationValue.y * tiltAmount, rotationValue.x * -tiltAmount, cardVisual.eulerAngles.z);

            float tiltx = Mathf.LerpAngle(cardVisual.eulerAngles.x, tilt.x, tiltSpeed * Time.deltaTime);
            float tilty = Mathf.LerpAngle(cardVisual.eulerAngles.y, tilt.y, tiltSpeed * Time.deltaTime);

            cardVisual.eulerAngles = new Vector3(tiltx, tilty, transform.eulerAngles.z);
        }
        else
        {
            float tiltx = Mathf.LerpAngle(cardVisual.eulerAngles.x, 0, tiltSpeed * Time.deltaTime);
            float tilty = Mathf.LerpAngle(cardVisual.eulerAngles.y, 0, tiltSpeed * Time.deltaTime);

            cardVisual.eulerAngles = new Vector3(tiltx, tilty, transform.eulerAngles.z);

        }
    }

    private void BeginDrag(Card card)
    {
        card.IsHovering = false;
    }
    private void EndDrag(Card card)
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void PointerEnter(Card card)
    {
        //cardVisual.DOScale(Vector3.one * 1.2f, 0.1f);
        //cardVisual.DOPunchRotation(Vector3.one * 30f, 0.05f);
    }
    private void PointerExit(Card card)
    {
        //cardVisual.DOScale(Vector3.one, 0.1f);
        //cardVisual.DOPunchRotation(Vector3.one * -30f, 0.05f);
    }


}
