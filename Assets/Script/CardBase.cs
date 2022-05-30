using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardBase
{
    public int Id;
    public string Name;

    public Guild Guilds;

    public Race Races;
    public Legion Legions;
    public CivilianGroup CivilianGroups;
    // public List<Constant> Constants;
    public HeadSimpleTrigger WalkMood;
    public HeadSimpleTrigger ActionMood;
    public HeadSimpleTrigger DefMood;
    public SimpleAction DefAction;

    public List<Constant> Stat =new List<Constant>();
    public List<int> StatSize = new List<int>();
    public List<int> StatSizeLocal = new List<int>();
    public int Mana;

    //public List<int> Stat;
    public List<string> Trait = new List<string>();
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


    public List<Effect> Effect;
    public List<Effect> InfinityEffect;

    public List<CardBase> Guard;
    public List<CardBase> Support;
    /*
     publec CardBase Guard;
     */
    //Status

    public List<int> PlayCard;
    public List<int> PlayAnotherCard;

    public List<int> Die;
    public List<int> AnotherDie;

    public List<int> Action;
    public List<int> AnotherAction;

    public List<int> InHand;
    public List<int> NextTurn;

    public List<int> PreAction;
    public List<int> PostAction;
    public List<int> ActionFlash;

    public List<string> Status;
}


