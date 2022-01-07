using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleMainFrame", menuName = "ScriptableObjects/RuleMainFrame", order = 1)]
public class RuleMainFrame : ScriptableObject
{
    public string[] BoolString;
    public string[] EqualString;

    public string[] TurnString;
    public string[] PlayerString;

    [Space(10)]
    public string[] Trigger;
    public List<RuleBaseFrame> AllTriggers;//цели для тригеров

    //public string[] RuleColor;
    //public string[] Color;
    // public string[] AllTriggersTayp;
    /*

 "Creature"
 "Creatures"
 "AllCreatures"
 Head
 TargetCreature"
 "Stol"
 "UseCard"



  */
}

[System.Serializable]
public class RuleFarmeMacroCase
{
    //public bool Use;
    public string Text;
    public int Id;
    public RuleFarmeCase[] Form;
}

[System.Serializable]
public class RuleFarmeCase
{
    public string Text = " {Rule}";//текст
   // public int Num;//номер
    public string Rule;//указывающие слово

}
