using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public List<int> BlackList;
    public int AllCard;
   // public List<string> AllCard;

    public Fraction[] AllFraction;
}

