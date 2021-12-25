using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiro
{
    public int NextCard;
    public Transform[] OrigSlots;
    public Slot[] Slots;
    public List<RealCard> Army;
    public List<RealCard> Provacator;


    public List<CardBase> CardColod;
    public List<int> CardHand;

    public int ManaMax = 10;
    public int Mana;
    public int ManaCurent;
    public int Team;

    public int Hp;
    public int ShotHiro;
}
public class Slot
{
    public RealCard[] Position;
    public MeshRenderer[] Mesh;
}
