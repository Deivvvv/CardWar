using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardBase 
{
    public string Name;

    public Guild Guilds;

    public Race Races;
    public Legion Legions;
    public CivilianGroup CivilianGroups;
    // public List<Constant> Constants;

    public List<Constant> Stat =new List<Constant>();
    public List<int> StatSize = new List<int>();
    public List<int> StatSizeLocal = new List<int>();
    public int Mana;

    //public List<int> Stat;
    public List<HeadSimpleTrigger> Trait = new List<HeadSimpleTrigger>();
    public List<int> TraitSize = new List<int>();

    // public List<Rule> Rules;


    public Transform Body;

    //public Sprite ImageFull;
    [HideInInspector]
    public byte[] Image;


    //extend
    public string Class;
    public int Iniciativa = 2;
    public Hiro MyHiro;
    public string Tayp = "HandCreate";
    public List<string> Tactic = new List<string>();

    public List<Effect> Effect;
    public List<Effect> InfinityEffect;

    public List<CardBase> Guard;
    public List<CardBase> Support;
    /*
     publec CardBase Guard;
     */
    //Status

    public bool Provacator;
    public bool Fly;
}


