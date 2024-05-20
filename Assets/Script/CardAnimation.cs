using DG.Tweening;
using UnityEngine;


public class CardAnimation : MonoBehaviour
{
    Vector3 movementDelta;
    Vector3 rotationDelta;

    [Header("Follow Card")]
    [SerializeField] private float followSpeed;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationAmount;

    [Header("Tilt")]
    [SerializeField] private float autoTiltAmount;
    [SerializeField] private float tiltAmount;
    [SerializeField] private float tiltSpeed;
    private float tiltX;
    private float tiltY;

    [Header("Tweeners")]
    Tweener tiltTween;
    Tweener defaultRotationTween;

    [Header("References")]
    [SerializeField] private Card card;
    [SerializeField] private CardSlot cardSlot;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CardHolder cardHolder;
    [SerializeField] private GameObject cardShadow;
    [SerializeField] private CardPositioning cardPositioning;

    private void Awake()
    {
        cardHolder = GetComponentInParent<CardHolder>();

        card.PointerEnterEvent.AddListener(PointerEnter);
        card.PointerExitEvent.AddListener(PointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
    }

    void Update()
    {
        CardFollowPosition();
        CardAutomaticTilt();
        CardManuelTilt();
        if (card.IsDragging)
        {
            CardRotation();
        }
        else
        {
            CardInitialRotation();
        }
    }

    private void CardFollowPosition()
    {
        transform.position = Vector3.Lerp(transform.position, card.transform.position - Vector3.forward, followSpeed * Time.deltaTime);

    }

    private void CardInitialRotation()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, card.transform.eulerAngles.z);
    }

    private void CardRotation()
    {
        Vector3 movement = transform.position - card.transform.position;
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = (card.IsDragging ? movementDelta : movement) * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));
    }

    private void CardAutomaticTilt()
    {
        if (card.IsHovering) return;

        float sine = Mathf.Sin(Time.fixedTime + card.ParentIndex());
        float cosine = Mathf.Cos(Time.fixedTime + card.ParentIndex());

        transform.eulerAngles = new Vector3(sine * autoTiltAmount, cosine * autoTiltAmount, cardSlot.CardPositioningRotationZ);
    }
    private void CardManuelTilt()
    {
        if (card.IsDragging) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotationValue = (mousePosition - transform.position).normalized;
        rotationValue.z = 0;

        if (card.IsHovering)
        {
            Vector3 tilt = new Vector3(rotationValue.y * tiltAmount * 10, rotationValue.x * -tiltAmount * 10, cardSlot.CardPositioningRotationZ);

            tiltX = Mathf.LerpAngle(transform.eulerAngles.x, tilt.x, tiltSpeed * Time.deltaTime);
            tiltY = Mathf.LerpAngle(transform.eulerAngles.y, tilt.y, tiltSpeed * Time.deltaTime);


        }
        else
        {
            tiltX = Mathf.LerpAngle(transform.eulerAngles.x, 0, tiltSpeed * Time.deltaTime);
            tiltY = Mathf.LerpAngle(transform.eulerAngles.y, 0, tiltSpeed * Time.deltaTime);

        }
            transform.eulerAngles = new Vector3(tiltX, tiltY, cardSlot.CardPositioningRotationZ);
    }

    private void BeginDrag(Card card, GameObject gameObject)
    {        
        gameObject = this.gameObject;
        card.IsHovering = false;

        transform.DOScale(Vector3.one * 1.5f, 0.1f);
        cardShadow.transform.DOLocalMoveY(0f, 0.1f);
    }
    private void EndDrag(Card card, GameObject gameObject)
    {
        gameObject = null;
        transform.eulerAngles = new Vector3(0, 0, 0);

        //reconsider here
        canvas.sortingOrder = 0;
        transform.DOScale(Vector3.one, 0.1f);
        //


        cardShadow.transform.DOLocalMoveY(0.05f, 0.1f);
    }

    private void PointerEnter(Card card)
    {
        for (int i = 0; i < cardHolder.cards.Count; i++)
        {
            if (cardHolder.cards[i].IsDragging) return;            
        }

        canvas.sortingOrder = 1;
        transform.DOScale(Vector3.one * 1.2f, 0.1f);
        transform.DOPunchRotation(Vector3.one * 15f, 0.1f);
    }
    private void PointerExit(Card card)
    {
        for (int i = 0; i < cardHolder.cards.Count; i++)
        {
            if (cardHolder.cards[i].IsDragging) return;
        }

        canvas.sortingOrder = 0;
        transform.DOScale(Vector3.one, 0.1f);
        transform.DOPunchRotation(Vector3.one * -15f, 0.1f);
    }


}
