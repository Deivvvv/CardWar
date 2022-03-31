using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameSetting", order = 1)]

public class GameSetting : ScriptableObject
{
    public ActionLibrary Library;

    public Sprite[] Icon;
    public string[] StatName;
    public int[] SellCount;
    public string[] NameIcon;

    public string origPath = $"/Resources/Hiro";

    public Material[] TargetColor;
    public GameObject OrigHiro;

    [Space(10)]
    public int StatSize = 4;
    public int RuleSize = 5;

    [Space(10)]
    public int SlotSize = 5;
    public int StartHandSize = 5;
    // public Color[] SelectColor; ??
}
