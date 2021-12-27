using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Constant", menuName = "ScriptableObjects/Constant", order = 1)]
public class Constant : ScriptableObject
{
    public string NameLocalization;
    public string Name;

    public Sprite Icon;
    public string IconName;
    public int Cost;

    public string AntiConstant;//свойства с отрицанием
    public string TwinConstant;//своиства исколючения, в одной карте они не могут быть установленны
}

public class CivilianGroup 
{
    public string Name;//имя группы - дворянство

    public List<string> Titul;
    public List<int> TitulCost;

    public List<string> Сonstant;
    public List<int> СonstantModifer;
}

/*
 Civilian Group
public string

 
 */