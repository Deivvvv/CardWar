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
    public List<int> StatSizeLocal;
    public int Mana;

    //public List<int> Stat;
    public List<string> Trait;
    public List<int> TraitSize;

    // public List<Rule> Rules;


    public Transform Body;

    public byte[] Image;


    //extend
    public string Class;
    public int Iniciativa = 2;
    public bool Provacator;
    public int Team;

    public List<TestEffect> Effect;
    public List<TestEffect> InfinityEffect;

    public List<CardBase> Guard;
    public List<CardBase> Support;
    /*
     publec CardBase Guard;
     */
}

public class TestEffect 
{
    public string Name = "Fly";
    public int Turn;
    public int Power;
    public int Prioritet;
    public string EffectGroup;
    public string Mood;//All-Max-Local
    public CardBase Target;
}

