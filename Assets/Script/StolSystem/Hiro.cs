using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiro
{
    public int NextCard;
    public Transform[] OrigSlots;
    public Slot[] Slots;


    public List<CardBase> CardColod = new List<CardBase>();
    public List<int> CardHandFull = new List<int>();
    public List<int> CardHand = new List<int>();

    public List<CardBase> PlayColod = new List<CardBase>();

    public int ManaMax = 10;
    public int Mana;
    public int ManaCurent;
    public int Team;

    public int Hp;
    public int ShotHiro;
}
public class Slot
{
    //public RealCard[] Position;
    public MeshRenderer[] Mesh;
}
