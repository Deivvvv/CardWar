using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiro
{
    public int NextCard;
    public List<Slot> Slots;
    public List<RealCard> Army;


    private List<CardBase> CardColod;
    private List<int> CardHand;

    public int ManaMax = 10;
    public int Mana;
    public int ManaCurent;

}
public class Slot
{
    public RealCard[] Position;
}
