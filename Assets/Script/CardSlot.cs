using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public Card card;

    private float cardPositioningOffsetY;
    [SerializeField]private float cardPositioningRotationZ;

    public float CardPositioningOffsetY { get => cardPositioningOffsetY; set => cardPositioningOffsetY = value; }
    public float CardPositioningRotationZ { get => cardPositioningRotationZ; set => cardPositioningRotationZ = value; }

    private void Awake()
    {
        card.OnHandPositioning += OnHandPositioning;
    }
    private void OnHandPositioning()
    {
        CardPositioningOffsetY = card.OffsetY;
        CardPositioningRotationZ = card.RotationZ;

        Debug.Log(cardPositioningRotationZ);

    }
}
