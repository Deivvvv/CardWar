using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSet", menuName = "ScriptableObjects/CardSet", order = 1)]
public class CardSet : ScriptableObject
{
    public string Name;
    public List<int> OrigCard;
    public List<int> OrigCount;


   // public int[] Card = new int[40];
}
