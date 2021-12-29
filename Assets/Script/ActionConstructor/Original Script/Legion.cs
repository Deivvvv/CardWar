using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Legion", menuName = "ScriptableObjects/Legion", order = 1)]
public class Legion : ScriptableObject
{
    public string Name;

    public List<CivilianGroup> CivilianGroups;
    public string[] Effect;
}
