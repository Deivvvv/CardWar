using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trait", menuName = "ScriptableObjects/Trait", order = 1)]
public class Trait : ScriptableObject
{
    public string Name;
    public int Count;
    public int MoveCost;
    public string Tayp;
    public string ClassTayp;
    public int strength;


}
/*
 разработка струкрутры
название
стоймость
стоимость в очках действий
группы на которые действует - All, Friend, Enemy, F
action - действие на юнитов 





 */