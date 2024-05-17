using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public Card selectedCard;
    public GameObject selectedCardVisual;

    public GameObject cardSlotPrefab;

    public List<Card> cards;

    public int cardAmount;

    private void Start()
    {
        for (int i = 0; i < cardAmount; i++)
        {
            Instantiate(cardSlotPrefab, transform);  
        }



        cards = GetComponentsInChildren<Card>().ToList();

        int cardCount = 0;

        foreach (Card card in cards)
        {
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(EndDrag);

            card.name = cardCount.ToString();

            cardCount++;

        }
    }

    private void Update()
    {
        if (selectedCard != null)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (selectedCard.transform.position.x > cards[i].transform.position.x)
                {
                    if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                    {

                        ChangeCardPosition(i);

                        break;
                    }
                }
                
                if (selectedCard.transform.position.x < cards[i].transform.position.x)
                {
                    if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                    {

                        ChangeCardPosition(i);

                        break;
                    }
                }
                
            }
        }


    }
    private void ChangeCardPosition(int i)
    {
        Transform selectedCardParent = selectedCard.transform.parent;
        Transform selectedCardVisualParent = selectedCardVisual.transform.parent;
        Transform otherCardParent = cards[i].transform.parent;

        cards[i].CardVisual.transform.SetParent(selectedCardParent);
        cards[i].transform.SetParent(selectedCardParent);



        if (!cards[i].IsSelected)
        {
            cards[i].transform.DOMove(selectedCardParent.position, 0.2f);
        }
        else
        {
            cards[i].transform.DOMove(selectedCardParent.position + Vector3.up * 0.5f, 0.2f);
        }

        selectedCardVisual.transform.SetParent(otherCardParent);
        selectedCard.transform.SetParent(otherCardParent);



    }

    private void BeginDrag(Card card, GameObject gameObject)
    {
        selectedCard = card;
        selectedCardVisual = gameObject;

    }
    private void EndDrag(Card card, GameObject gameObject)
    {
        selectedCard = null;
        selectedCardVisual = null;
    }

}
