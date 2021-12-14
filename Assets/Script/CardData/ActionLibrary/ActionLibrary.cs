using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionLibrary", menuName = "ScriptableObjects/ActionLibrary", order = 1)]
//[System.Serializable]
public class ActionLibrary : ScriptableObject
{
    public List<Trait> Action;
}
