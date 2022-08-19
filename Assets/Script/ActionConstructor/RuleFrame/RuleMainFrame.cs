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
    public string[] SysStat = { "Skip","MainStat","Mana","Speed"};
    //public string[] StatTayp;
    [HideInInspector] public List<string> Tayp;

    [Space(10)]
    [HideInInspector] public string[] Trigger;
    [HideInInspector] public List<SubAction> Action;

    [Space(10)]
    //public List<string> KeyWord;
    public List<string> KeyWordStatus;


    [Space(10)]
   // public List<string> ClassCard;
   // public List<string> CardTayp;

    [Space(10)]
    [SerializeField] private Color[] colors;
    [HideInInspector] public string[] ColorsStr;


    /*
     Triggers
    action - ������� ������ ��� ������ �������� �� ���� ��������

     
     comand
    transf plan - ���������� ������� �� ������ ����� � ������
    StstAdd - �����������/��������� ����
    StatusAdd - �������� ������, ���� ��� ���
    StatusRemove - ������� ������, ���� �� ����
    attak - ��������� �������� �����


     */
    public void Convert()
    {
        {
            string[] com = { "Guild", "Legion", "Stat", "Tag", "Plan", "Association", "CivilGroup", "Status", "CardTayp", "CardClass", "Race","StatGroup" };
            Tayp = new List<string>(com);
        }

        {
            //������� ������ �������� , ���-�� �������� �������� // ������� �� ������ ���� // ����-�� ��������� �� ������ ����
            string[] com = { "NextTurn", "Action", "AnotherAction", "Transf", "AnotherTransf", "Destroy", "Equip", "AnotherDestroy", "AnotherEquip" ,"PreAction", "PostAction", "EndAction", "Target", "Usebel", "NoTarget","SecondAction" };
            Trigger = com;
        }



        Action = new List<SubAction>();
        Action.Add(new SubAction("Attack", "Meele_Shot"));
        Action.Add(new SubAction("Stat", "Add_Clear_MainStat_Replace"));//�������� ���� - ������� ���� - �������� �������� ���� - �������� ��������
        // Action.Add(new SubAction("Karma", "Clear_Add_Remove", "PG_PS_PL_PT_NG_NS_NL_NT"));//karma_{global}_{all}_Stat   .. ����������� ���� ������
       // Action.Add(new SubAction("Switch", "Guild_Ligion_Social_Race"));
       // Action.Add(new SubAction("Effect", "Add_Remove_Replace", "Eternal_NoEternal"));
        Action.Add(new SubAction("Rule", "Use_Add_Delite"));
        Action.Add(new SubAction("Status", "Add_Remove_Replace"));
        Action.Add(new SubAction("Transf", " "));
        /*
           karma
          global = ����� ���������� �� ���� ����
          turn
          local

          positiv
          negativ
          all

           */




        ColorsStr = new string[colors.Length];
        for (int i = 0; i < ColorsStr.Length; i++)
            ColorsStr[i] = ColorUtility.ToHtmlStringRGB(colors[i]);
    }

    public List<string> SetKey(int a)
    {
        switch (Tayp[a]) 
        {
            case ("Guild"):
                {
                    string[] com1 = { "Legion","Tag","Stat"};
                    return new List<string>(com1);
                }
                break;
            case ("Legion"):
                {
                    string[] com1 = { "Guild", "Tag", "CivilGroup", "Stat" };
                    return new List<string>(com1);
                }
                break;
            case ("Stat"):
                {
                    string[] com1 = { "Legion", "Guild", "Tag", "CivilGroup", "Race", "StatGroup" };
                    return new List<string>(com1);
                }
                break;
            case ("Tag"):
                {
                    string[] com1 = { "Legion", "Guild", "CivilGroup", "Race", "Stat" };//mainStat = stat=0
                    return new List<string>(com1);
                }
                break;
            case ("Plan"):
                {
                    string[] com1 = { "CivilGroup" };
                    return new List<string>(com1);
                }
                break;
            case ("Association"):
                //{
                //    string[] com1 = { "Legion", "Tag" };
                //    return new List<string>(com1);
                //}
                break;
            case ("CivilGroup"):
                {
                    string[] com1 = { "Legion", "Tag", "CardTayp" , "Stat","Plan", "Race" };
                    return new List<string>(com1);
                }
                break;
            case ("Status"):
                //{
                //    string[] com1 = new string[0];// { };
                //    return new List<string>(com1);
                //}
                break;
            case ("CardTayp"):
                {
                    string[] com1 = { "CivilGroup" };
                    return new List<string>(com1);
                }
                break;
            case ("CardClass"):
                //{
                //    string[] com1 = { "Legion", "Tag" };
                //    return new List<string>(com1);
                //}
                break;
            case ("Race"):
                {
                    string[] com1 = { "CivilGroup", "Tag", "CivilGroup" };
                    return new List<string>(com1);
                }
                break;
            case ("StatGroup"):
                {
                    string[] com1 = { "Stat" };
                    return new List<string>(com1);
                }
                break;
                
        }
        return new List<string>();
    }
}
/*
 �������
    ����� ������ ��� ��������
    ����� �������� ��� ��������

�������
    ����� ������ ��� ��������
    ����� ��� ����� ��� ��������
    ����� ������� ��� ��������

��� ������
    ����� ������ ��� ��������
    ����� �������� ��� ��������

����
    ����� ������ ��� ��������
    ����� ��� ����� ��� ��������
    

 
 */

public class SubAction
{
    public SubAction(string name, string str, string ext =" ")
    {
        Name = name;
        Extend = str.Split('_');
        Dop = ext.Split('_');
    }
    public string Name;
    public string[] Extend;
    public string[] Dop;
}

//[System.Serializable]
//public class SubTayp
//{
//    public string Name;
//    public string[] Key;// = new List<string>();
//    public bool[] Hide;
//}
//[System.Serializable]
//public class FrameExtend
//{
//    public string Name = "Null";
//    public string[] Guild;

//}
