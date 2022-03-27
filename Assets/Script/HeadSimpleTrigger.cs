using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSimpleTrigger
{
    public string Name = "�����������";//��������
    public string Info;//��������

    public int Cost;//����
    public int CostExtend;//���� �� ��� ���� �������

    public int LevelCap;//������������ ������� �����������

    //public int CostMovePoint;

    public bool Player;


    public List<SimpleTrigger> SimpleTriggers = new List<SimpleTrigger>();

    public List<string> NeedRule = new List<string>();
    public List<string> EnemyRule = new List<string>();
}

public class SimpleTrigger
{
    public bool CountMod;
    public bool CountModExtend;
    public int TargetPalyer;
    public List<SimpleIfCore> MinusPrior = new List<SimpleIfCore>();
    public List<SimpleIfCore> PlusPrior = new List<SimpleIfCore>();
    public List<SimpleAction> Action;
    //  public List<RuleAction> Action;
}
public class SimpleAction
{
    public string Action;
    public string ActionFull;
    public int MinPoint;
    public int MaxPoint;
}
public class SimpleIfCore
{
    public int Point;
    public int Prioritet;
    public string Attribute;//public List<string> Attribute;//
    //public List<string> Target;//
}