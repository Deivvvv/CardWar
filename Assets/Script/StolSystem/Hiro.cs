using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiro
{
    public int NextCard;
    public List<Slot> Slots;
    public List<RealCard> Army;


    public List<CardBase> CardColod;
    public List<int> CardHand;

    public int ManaMax = 10;
    public int Mana;
    public int ManaCurent;
    public int Team;
}
public class Slot
{
    public RealCard[] Position;
}
