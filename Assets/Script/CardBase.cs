using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class CardBase 
{
    public string Name;

    public Guild Guilds;

    public Race Races;
    public Legion Legions;
    public CivilianGroup CivilianGroups;
    // public List<Constant> Constants;

    public List<Constant> Stat;
    public List<int> StatSize;
    public int Mana;

    //public List<int> Stat;
    public List<string> Trait;
    public List<int> TraitSize;

    // public List<Rule> Rules;


    public Transform Body;

    public byte[] Image;

    public string Class;
}
