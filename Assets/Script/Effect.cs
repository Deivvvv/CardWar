using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effect", order = 1)]
public class Effect : ScriptableObject
{
    public string Name = "Fly";
    public int Turn;
    public int Power;
    public int Prioritet;

    public string EffectGroup;
    public string Mood;//All-Max-Local

    public CardBase Target;

    public string Com;
}
