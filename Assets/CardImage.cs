using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    [SerializeField] private CardSkin cardSkin;

    [SerializeField] private Image cardImage;
    void Start()
    {
        cardImage.sprite = cardSkin.cardImage[Random.Range(0, 2)];
    }

}
