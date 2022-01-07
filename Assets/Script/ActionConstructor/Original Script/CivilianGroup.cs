using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CivilianGroup", menuName = "ScriptableObjects/CivilianGroup", order = 1)]
public class CivilianGroup : ScriptableObject
{
    public List<Legion> Legions;
   // public string Legion;//легион в котором состоят

    public string Name;//имя группы - дворянство

    public List<Titul> Tituls;

    public List<StringCase> Сonstant;
}
[System.Serializable]
public class Titul
{
    //титул - название карты при указанном значении очко прогресии
    //очки прогресси для получения титула
    public string Name;
    public int Cost;

    public List<string> Effect;
}

[System.Serializable]
public class StringCase
{
    //параметр конвертируемый в очки титула
    //модификатор конвертации в очки, прим (0.1f)
    public Constant Name;
    public float Cost =1;
}