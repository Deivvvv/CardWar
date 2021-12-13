using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameSetting", order = 1)]

public class GameSetting : ScriptableObject
{
    public ActionLibrary Library;

    public GameData ReservData;
    public GameData GlobalMyData;
    public GameData LocalMyData;

    public GameData ReservPlayerData;
    public GameData GlobalPlayerData;
    public GameData LocalPlayerData;

    public Sprite[] Icon;
    public string[] StatName;
    public int[] SellCount;
    public string[] NameIcon;

    public string origPath = $"/Resources/Hiro";

    public Material[] TargetColor;
    public GameObject OrigHiro;
    // public Color[] SelectColor; ??
}
