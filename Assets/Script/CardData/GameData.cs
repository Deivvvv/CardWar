using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.IO;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public List<int> BlackList;
    public List<string> AllCard;
    public Fraction[] AllFraction;

    public Sprite[] Icon;
    public string[] StatName;
    public int[] SellCount;
    public string[] NameIcon;
}

