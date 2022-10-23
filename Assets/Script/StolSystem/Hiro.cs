using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//[System.Serializable]
public class Hiro
{
    public int NextCard;

    //SlotSystem
    public List<MeshRenderer> SlotMesh = new List<MeshRenderer>();
    public List<CardCase> Slot = new List<CardCase>();
    public List<MeshRenderer> BackSlotMesh = new List<MeshRenderer>();
    public List<CardCase> BackSlot = new List<CardCase>();


    //ColodSystem
    public List<CardCase> CardColod = new List<CardCase>();
    public List<int> CardHandFull = new List<int>();

    public List<int> CardHand = new List<int>();

    public List<int> PlayColod = new List<int>();

    //StatHiroSystem
    public int ManaMax = 10;
    public int Mana =1;
    public int ManaCurent =1;
    public int Team;

    public int Hp =15;
    public int ShotHiro;

    public TMP_Text Ui;
    public Transform UiStol;
}
