using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionLibrary", menuName = "ScriptableObjects/ActionLibrary", order = 1)]
//[System.Serializable]
public class ActionLibrary : ScriptableObject
{
    public List<Guild> Guilds;

    public List<Legion> Legions;
    public List<CivilianGroup> CivilianGroups;
    public List<Constant> Constants;
    public List<Effect> Effects;
    public List<Trait> Action;

    public List<string> RuleName;
}
