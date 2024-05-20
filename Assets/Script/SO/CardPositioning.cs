using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardPositioning")]
public class CardPositioning : ScriptableObject
{
    public AnimationCurve curve;
    public float curveMultiplier;
    public AnimationCurve rotation;
    public float rotationMultiplier;

}
