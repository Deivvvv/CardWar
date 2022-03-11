using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Race", menuName = "ScriptableObjects/Race", order = 1)]
public class Race : ScriptableObject
{
    public string Name;

    public Race MainRace;
    public Constant MainStat;

    public List<Legion> Legions;
    public string[] Effect;

}
