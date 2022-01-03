using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleBaseFrame", menuName = "ScriptableObjects/RuleBaseFrame", order = 1)]
public class RuleBaseFrame : ScriptableObject
{
    public string Text;
    public string Name;
    public List<RuleFrame> Form;
}
