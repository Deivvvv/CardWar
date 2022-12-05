using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipList", menuName = "ScriptableObjects/EquipList", order = 1)]
public class EquipList : ScriptableObject
{
    public string NameList;
    public List<GameObject> ItemList;
}
