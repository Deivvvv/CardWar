using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Fraction", menuName = "ScriptableObjects/Fraction", order = 1)]
[System.Serializable]
public class Fraction : ScriptableObject
{
    public string Frac;
    public string Path;
    // public XML
    public string[] Card;

    // public TextAsset[] Hiro;
}
