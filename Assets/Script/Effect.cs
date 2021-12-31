using UnityEngine;

[CreateAssetMenu(fileName = "ActionLibrary", menuName = "ScriptableObjects/ActionLibrary", order = 1)]
public class Effect : ScriptableObject
{
    public string Name;
    public int Size;
    public RealCard TargetHiro;
}
