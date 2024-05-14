using DG.Tweening;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public UnityEvent<Card> BeginDragEvent;
    [HideInInspector] public UnityEvent<Card> EndDragEvent;
    [HideInInspector] public UnityEvent<Card> PointerEnterEvent;
    [HideInInspector] public UnityEvent<Card> PointerExitEvent;
    [HideInInspector] public UnityEvent<Card> PointerUpEvent;
    [HideInInspector] public UnityEvent<Card> PointerDownEvent;

    public float moveSpeedLimit;

    private Vector3 targetPosition;
    

    [Header("Card bools")]
    private bool isSelected = false;

    [Header("DOTweenVariables")]
    [SerializeField] private float returnDuration;

    private Vector3 offset;
    private bool isDragging = false;
    private bool isHovering = false;

    public bool IsSelected { get => isSelected; set => isSelected = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public bool IsHovering { get => isHovering; set => isHovering = value; }

    void Update()
    {
        Debug.Log(isHovering);

        if (isDragging)
        {
            TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
            Vector2 direction = ((Vector2)TargetPosition - (Vector2)transform.position).normalized;
            Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, (Vector2)TargetPosition, 0.1f);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - (Vector2)transform.position;

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        isDragging = false;
        ReturnToCardSlot();

    }

    public void ReturnToCardSlot()
    {
        if (!IsSelected)
        {
            transform.DOMove(transform.parent.position, returnDuration)
                .SetEase(Ease.OutBack);
        }
        else
        {
            transform.DOMove(transform.parent.position + Vector3.up * 0.5f, returnDuration)
                .SetEase(Ease.OutBack);
        }
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        IsHovering = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        IsHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging) return;


        if (!IsSelected)
        {
            IsSelected = true;
        }
        else
        {
            IsSelected = false;
        }

        if (IsSelected)
        {
            transform.DOLocalMove(Vector3.up * 0.5f, 0.1f);
        }
        else
        {
            transform.DOLocalMove(Vector3.zero, 0.1f);
        }
    }

    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

}
