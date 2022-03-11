using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Guild", menuName = "ScriptableObjects/Guild", order = 1)]
public class Guild : ScriptableObject
{
    public string Name;
    public Sprite Icon;

    public List<Race> Races;
    public List<Legion> Legions;
    public string[] Effect;

    public string[] GuildRule;
}
