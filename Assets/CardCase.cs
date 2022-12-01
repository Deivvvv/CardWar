using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coder;

public class CardCase
{

    public int Id;
    public string Name;

    public int Guild ,Race ,Legion , Civilian, CardTayp, CardClass, Plan;

    public List<StatExtend> Stat = new List<StatExtend>();

    public int Mana;
    public int Speed =1; 
    public int ActionPoint;
    public int ActionPointMax =1;
    public List<SubInt> Trait = new List<SubInt>();

    public Sprite Image;
    public int Iniciativa = 2;
    public int Team;


    public List<Effect> Effect;
    public List<Effect> InfinityEffect;

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
    public CardCase(CardCase card)
    {
        Guild = card.Guild;
        CardTayp = card.CardTayp;
        CardClass = card.CardClass;
        Id = card.Id;
        Civilian = card.Civilian;
        Legion = card.Legion;
        Race = card.Race;
        Name = card.Name;
        Team = card.Team;
        for (int i =0;i<card.Stat.Count;i++)
            Stat.Add(new StatExtend(card.Stat[i]));

        for (int i = 0; i < card.Trait.Count; i++)
        {
            Trait.Add(new SubInt(card.Trait[i].Head));
            for (int j = 0; j < card.Trait[i].Num.Count; j++)
                Trait[i].Num.Add(new SubInt(card.Trait[i].Num[j].Head));
        }
    }
}

public class StatExtend
{
    private int stat, statSize = 1, statSizeLocal = 1, icon;
    public StatExtend(StatExtend statExtend)
    {
        stat = statExtend.stat;
        statSize = statExtend.statSize;
        statSizeLocal = statExtend.statSizeLocal;
        icon = statExtend.icon;
    }
    public StatExtend(int a, BD bd)
    {
        stat = a;
        icon = bd.Base[stat].Sub.Image;
    }
    public StatExtend(string str, BD bd )
    {
        string[] com = str.Split('/');
        stat = int.Parse(com[0]);
        statSize = int.Parse(com[1]);
        icon = bd.Base[stat].Sub.Image;
    }
    public void Swap(int a, BD bd, bool revers =true)
    {
        stat = a;
        icon = bd.Base[stat].Sub.Image;
        if (revers)
        {
            statSize = -statSize;
            statSizeLocal = -statSizeLocal;
        }
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
                if (statSizeLocal + size <= statSize)
                    statSizeLocal += size;
                else
                    statSizeLocal = statSize;
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
    public int GetStat()
    {
        return stat;
    }
    public int Get(string mood)
    {
        int a = 0;
        switch (mood)
        {
            case ("All"):
                a = statSize;
                a += statSizeLocal;
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
        string str = $"<sprite index={icon}>";
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
            case ("Save"):
                str = $"{stat}/{statSize}";
                break;
        }
        return str;
    }
}