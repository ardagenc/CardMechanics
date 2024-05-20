using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSkin")]
public class CardSkin : ScriptableObject
{
    public Sprite[] cardImage;
    public Material cardShader;
}
