using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hiro
{
    public int NextCard;

    //SlotSystem
    public List<MeshRenderer> SlotMesh = new List<MeshRenderer>();
    public List<CardBase> Slot = new List<CardBase>();
    public List<MeshRenderer> BackSlotMesh = new List<MeshRenderer>();
    public List<CardBase> BackSlot = new List<CardBase>();


    //ColodSystem
    public List<CardBase> CardColod = new List<CardBase>();
    public List<int> CardHandFull = new List<int>();

    public List<CardBase> CardHand = new List<CardBase>();

    public List<CardBase> PlayColod = new List<CardBase>();

    //StatHiroSystem
    public int ManaMax = 10;
    public int Mana;
    public int ManaCurent;
    public int Team;

    public int Hp;
    public int ShotHiro;

    public TMP_Text Ui;
}
