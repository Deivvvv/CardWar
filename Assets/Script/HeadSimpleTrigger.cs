using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeadSimpleTrigger
{
    public string Name = "Благочестие";//Название
    public string Info;//Описание

    public int Cost;//Цена
    public int CostExtend;//цена за доп очки навыков

    public int LevelCap;//Максимальный уровень способности

    //public int CostMovePoint;

    public bool Player;


    public List<SimpleTrigger> SimpleTriggers = new List<SimpleTrigger>();

    public List<string> NeedRule = new List<string>();
    public List<string> EnemyRule = new List<string>();
}

//[System.Serializable]
public class SimpleTrigger
{
    public int CodName;
    public bool CountMod;
    public bool CountModExtend;
    public string TargetPalyer;
    public string Trigger;
    public List<SimpleIfCore> PlusPrior = new List<SimpleIfCore>();
    public List<SimpleIfCore> MinusPrior = new List<SimpleIfCore>();
    public List<SimpleAction> Action = new List<SimpleAction>();
}
//[System.Serializable]
public class SimpleAction
{
    public string Action;
    public string Player;
    public string ActionFull;
    public int MinPoint;
    public int MaxPoint;

    public string Mood;
    // public int Num;
}
//[System.Serializable]
public class SimpleIfCore
{
    public string Result;
    public int Point;
    public int Prioritet;
    public string Attribute;
    //public List<string> Target;//
}