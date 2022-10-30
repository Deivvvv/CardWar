using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCase : MonoBehaviour
{

    public int Id;
    public string Name;

    public int Guild ,Race ,Legion , Civilian, CardTayp, CardClass;

    // public List<Constant> Constants;
    public int HeadWalkMood;
    public int HeadActionMood;
    public int HeadDefMood;
    public int HeadDefAction;

    public int WalkMood;
    public int ActionMood;
    public int DefMood;
    public int DefAction;

    public List<StatExtend> Stat = new List<StatExtend>();


    //public List<Constant> Stat = new List<Constant>();
    //public List<int> StatSize = new List<int>();
    //public List<int> StatSizeLocal = new List<int>();
    public int Mana;

    //public List<int> Stat;
    public List<SubInt> Trait = new List<SubInt>();

    // public List<Rule> Rules;


    //public Transform Body;

    public Sprite Image;
    //[HideInInspector]
    //public byte[] Image;


    //extend
    //public string Class;
    public int Iniciativa = 2;
    public Hiro MyHiro;


    public List<Effect> Effect;
    public List<Effect> InfinityEffect;

    // public List<int> Guard; //CardCase
    // public List<int> Support;//CardCase
    /*
     publec CardCase Guard;
     */
    //Status
    /*
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
    */

    public List<int> Status;

    public CardCase(int intGuild, int intCardTayp, int intCardClass, int intId)
    {
        Guild = intGuild;
        CardTayp = intCardTayp;
        CardClass = intCardClass;
        Id = intId;
    }
    public CardCase(int intGuild, int intCardTayp, int intCardClass, int intId, int intLegion, int intCivilian, int intRace)
    {
        Guild = intGuild;
        CardTayp = intCardTayp;
        CardClass = intCardClass;
        Id = intId; 
        Civilian = intCivilian;
        Legion = intLegion;
        Race = intRace;
    }
}

public class StatExtend
{
    private int stat, statSize = 1, statSizeLocal = 1, icon;
    StatExtend(int a,int b)
    {
        stat = a;
        icon = b;
    }
    public void Swap(int a, int b)
    {
        stat = a;
        icon = b;
    }

    public void Edit(string mood, int size)
    {
        switch (mood)
        {
            case ("All"):
                statSize += size;
                statSizeLocal += size;
                break;
            case ("Max"):
                statSize += size;
                break;
            case ("Local"):
                if(statSizeLocal +size <= statSize)
                    statSizeLocal += size;
                break;
            case ("LocalForce"):
                statSizeLocal += size;
                break;
        }
    }


    public void Set(string mood, int size)
    {
        switch (mood)
        {
            case ("All"):
                statSize = size;
                statSizeLocal = size;
                break;
            case ("Max"):
                statSize = size;
                break;
            case ("Local"):
                    statSizeLocal = size;
                break;
            case ("LocalForce"):
                statSizeLocal = size;
                break;
        }
    }
    public int Get(string mood)
    {
        int a = 0;
        switch (mood)
        {
            case ("Stat"):
                a = stat;
                break;
            case ("Max"):
                a = statSize;
                break;
            case ("Local"):
                a = statSizeLocal;
                break;
            case ("LocalForce"):
                a = statSizeLocal;
                break;
        }
        return a;
    }

    public string Read(string mood)
    {
        string str = $"<index={icon}>";
        switch (mood)
        {
            case ("All"):
                str += $"{statSize}/{statSizeLocal}";
                break;
            case ("Max"):
                str += $"{statSize}";
                break;
            case ("Local"):
                str += $"{statSizeLocal}";
                break;
            case ("LocalForce"):
                str += $"{statSizeLocal}";
                break;
        }
        return str;
    }
}