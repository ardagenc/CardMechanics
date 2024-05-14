using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private Card card;

    [SerializeField] private Transform cardVisual;

    [Header("Rotation")]
    [SerializeField] private float rotationAmount;


    private void Awake()
    {
        card.PointerEnterEvent.AddListener(PointerEnter);
        card.PointerExitEvent.AddListener(PointerExit);
    }
    private void PointerEnter(Card card)
    {
        cardVisual.DOScale(Vector3.one * 1.2f, 0.1f);
        cardVisual.DOPunchRotation(Vector3.one * 30f, 0.05f);
    }
    private void PointerExit(Card arg0)
    {
        cardVisual.DOScale(Vector3.one, 0.1f);
        cardVisual.DOPunchRotation(Vector3.one * -30f, 0.05f);
    }


    void Update()
    {        
        CardTilt();


    }

    private void CardTilt()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion toRotation = Quaternion.Euler((mousePosition.y - cardVisual.position.y) * 30, (mousePosition.x - cardVisual.position.x) * -30, 0);

        Vector3 rotationValue = mousePosition - transform.position;

        if (card.IsHovering)
        {
            cardVisual.DORotate(new Vector3(rotationValue.y, -rotationValue.x, 0) * 40, 0.5f);
            //cardVisual.rotation = Quaternion.Lerp(transform.rotation, toRotation, 100);
        }
        else
        {
            cardVisual.DORotate(Vector3.zero, 0.5f);
            //cardVisual.rotation = Quaternion.Lerp(cardVisual.rotation, Quaternion.Euler(0,0,0), 100);
        }
    }

}
