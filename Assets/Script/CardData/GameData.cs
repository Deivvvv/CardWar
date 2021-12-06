using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.IO;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public CardBase AllCard;

    public Fraction[] AllFraction;
}


[CreateAssetMenu(fileName = "Fraction", menuName = "ScriptableObjects/Fraction", order = 1)]
[System.Serializable]
public class Fraction : ScriptableObject
{
    public string Frac;
    // public XML

    public TextAsset[] Hiro;
}