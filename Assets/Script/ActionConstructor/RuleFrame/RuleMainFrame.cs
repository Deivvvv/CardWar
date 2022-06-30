using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleMainFrame", menuName = "ScriptableObjects/RuleMainFrame", order = 1)]
public class RuleMainFrame : ScriptableObject
{
    //public string[] BoolString;
    public string[] ForseTayp;
    public string[] EqualString;

    public string[] PlayerString;
    public string[] CardString;
    public string[] StatTayp;
    public string[] Tayp = { "Guild", "Legion", "Stat", "Tag", "Plan", "Association", "CivilGroup", "Status", "CardTayp", "CardClass" };

    [Space(10)]
    public string[] Trigger;
  //  public FrameExtend[] Tag;
    public string[] Status;
    public string[] Action;

    [Space(10)]
    public List<string> KeyWord;
    public List<string> KeyWordStatus;


    [Space(10)]
    public List<string> ClassCard;
    public List<string> CardTayp;
}

//[System.Serializable]
//public class FrameExtend
//{
//    public string Name = "Null";
//    public string[] Guild;

//}
