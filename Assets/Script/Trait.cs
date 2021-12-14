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


}
