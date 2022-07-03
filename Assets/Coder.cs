using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//using UnityEngine.CoreModule;
using UnityEngine.UI;
using TMPro;

using XMLSaver;

namespace Coder
{
    class DeCoder
    {
        #region Seters
        static int keyA, keyB;

        static string mood;
        public static void SetMood(string str) { mood = str; }

        static CoreSys core;
        public static void SetCore(CoreSys coreSys) {core = coreSys; }

        static List<TextMeshProUGUI> TT;
        static TMP_InputField nameTT;
        public static void SetTT(List<TextMeshProUGUI> newTT, TMP_InputField newNameTT)
        {
            TT = newTT;
            nameTT = newNameTT;
        }

        static void  ResetKey()
        {
            keyA = 0;
            keyB = 0;
            //keyC = 0;
        }
        #endregion
        /*
         * Sys - раздел системных комманд
         * Open - открывает определенный раздел проводника 
         * Switch - переключает раздел на другой из содержимого статической библиотеки
         * Link - сформировать ссылку дл€ текста
         * Re - вз€ть текст из пол€
         */
        public static void Starter()
        {
            switch (mood)
            {
                case ("BD"):
                    TextBD("Main");
                    //TextBD("Info");
                    break;
            }
        }

        #region Coder
        public static void Read(string str)
        {
            
            
            string[] com = str.Split('|');
            switch (com[0])
            {
                case ("Sys"):
                    //ReadSys(com[1]);
                    break;
                case ("GetIO"):
                    GetIO(com[1]);
                    break;
                case ("SetIO"):
                    SetIO(com[1]);
                    break;
                case ("Open"):
                   // ReadOpen(com[1]);
                    break;
                case ("Switch"):
                    ReadSwitch(com[1]);
                    break;
                case ("Link"):
                    //ReadLink(com[1]);
                    break;
                //case ("Re"):
                //    if (com.Length > 2)
                //        GetLocText(com[1], int.Parse(com[2]));
                //    else
                //        GetLocText(com[1]);
                //    break;

                case ("Color"):
                    str =SwitchColor();
                    break;
                case ("Icon"):
                    str = SwitchIcon();
                    break;
            }
        }

        static void ReadSwitch(string str)
        {
            string[] com = str.Split('_');
            switch (com[0])
            {
                case ("ID"):
                    switch (com[1])
                    {
                        case ("Base"):
                            keyA = int.Parse(com[2]);
                            keyB = int.Parse(com[3]);
                            break;
                    }
                    break;
                case ("Color"):
                    core.bD[keyA].Base[keyB].Color = core.frame.ColorsStr[int.Parse(com[1])];
                    break;
                case ("Icon"):
                   // core.bD[keyA].Base[keyB].Color = core.frame.ColorStr[int.Parse(com[1])];
                    break;
            }


        }

        #endregion

        #region IO
        static void GetIO(string str)
        {
            string[] com = str.Split('_');

            keyA = int.Parse(com[1]);
            keyB = int.Parse(com[2]);

            switch (com[0])
            {
                case ("HeadBase"):
                    nameTT.text = core.bD[keyA].Name;
                    break;
                case ("HeadInfo"):
                    nameTT.text = core.bD[keyA].Info;
                    break;
                case ("Base"):
                    nameTT.text = core.bD[keyA].Base[keyB].Name;
                    break;
                case ("Info"):
                    nameTT.text = core.bD[keyA].Base[keyB].Info;
                    break;

                case ("CardName"):
                    // cardBase = nameTT.text;
                    break;

                case ("RuleName"):
                    //  cardBase = nameTT.text;
                    break;
            }
        }

        static void SetIO(string str)
        {
            switch (str)
            {
                case ("HeadBase"):
                    core.bD[keyA].Name = nameTT.text;
                    break;
                case ("HeadInfo"):
                    core.bD[keyA].Info = nameTT.text;
                    break;
                case ("Base"):
                    core.bD[keyA].Base[keyB].Name = nameTT.text;
                    break;
                case ("Info"):
                    core.bD[keyA].Base[keyB].Info = nameTT.text;
                    break;

                case ("CardName"):
                   // cardBase = nameTT.text;
                    break;

                case ("RuleName"):
                  //  cardBase = nameTT.text;
                    break;
            }
        }

        #endregion

        #region Text
        public static void Text(string str)
        {
            string[] com = str.Split('|');
            switch (com[0])
            {
                case ("BD"):
                    TextBD(com[1]);
                    break;
            }

        }


        static void TextBD(string str)
        {
            string[] com = str.Split('_');
            switch (com[0])
            {
                case ("Menu"):
                    str = AddLink("Sys|Menu", "Open SysMenu") + "\n"; ;
                    for (int i = 0; i < core.frame.Tayp.Count; i++)
                    {
                        str += AddLink($"Open|Text_{i}", core.bD[i].Name + $"({core.bD[i].Base.Count})") + "\n";
                    }
                    TT[0].text = str;
                    break;
                case ("Info"):
                    str = AddLink("Sys|Menu", "Open SysMenu") + "\n"; ;
                    for (int i = 0; i < core.frame.Tayp.Count; i++)
                    {
                        str += AddLink($"Open|Text_{i}", core.bD[i].Name + $"({core.bD[i].Base.Count})") + "\n";
                    }
                    TT[1].text = str;
                    break;
            }
            //return str;
        }
        #endregion



        #region Link
        static string SwitchColor()
        {
            string str = AddLinkColor(0);
            for (int i = 1; i < core.frame.ColorsStr.Length; i++)
                str += "\n" + AddLinkColor(i);

            return str;
        }
        static string SwitchIcon()
        {
            string str = AddLinkIcon(0);
            //for (int i = 1; i < iconSize; i++)
            //    str += "\n" + AddLinkIcon(i);

            return str;

        }

        static string AddLinkIcon(int i)
        {
            return $"<link=Switch|Icon_{i}><index={i}></link>";
        }
        static string AddLinkColor(int i)
        {
            return $"<link=Switch|Color_{i}><color=#{core.frame.ColorsStr[i]}>{core.frame.ColorsStr[i]}</color></link>";
        }

        //string AddLinkTayp(int a,int b)
        //{
        //    return $"<link=Text|Info_{a}_{b}><color={core.bD[a].Base[b].Color}>{core.bD[a].Base[b].Name}</color></link>";
        //}
        //string AddLinkTaypInfo(MainBase main)
        //{
        //    string str;
        //    if(main.Info == "Void")
        //        str = $"<link=InfoLink_{main.SysName}><color={colorsStr[1]}>[!]</color></link>";
        //    else
        //        str = $"<link=InfoLink_{main.SysName}><color={colorsStr[0]}>[?]</color></link>";
        //    return str;
        //}


        static string AddLink(string link, string text, string colors = "ffff00")
        {
            return $"<link={link}><color=#{colors}>{text}</color></link>";
        }

        #endregion


        #region CardRedactor Extend
        CardBase cardBase;

        #endregion

        #region RuleRedactor Extend
        static HeadRule ruleMain;
        #region Create
        static void CreateHeadRule()
        {
            ruleMain = new HeadRule();
        }

        #endregion

        #region Save/Load
        static void RuleConstructorStart()
        {
            ResetKey();

           // ruleHead = Saver.LoadAllRule();
            //if (ruleHead.Count == 0) {

            //    ruleMain = CreateHeadRule();
            //    //ruleHead.Add(new SubRuleHead(core.bD[3].Base[0]))
            //}
            //else
            //{
            //    ruleMain = Saver.LoadRule(0, 0);
            //}
        }
        static void SaveRule()
        {
            //if (ruleHead.Count == 0)
            //    ruleHead.Add(new SubRuleHead(ruleMain.Tag));

            //if(ruleHead[keyA].Name != ruleMain.Tag)
            //{
            //    ruleHead[keyA].Rule.RemoveAt(keyB);
            //    Saver.DeliteRule(keyA, keyB);
            //    Saver.SaveRuleMain(ruleHead[keyA], keyA);

            //    keyA = ruleHead.FindIndex(x => x.Name == ruleMain.Tag);
            //    keyB = ruleHead[keyA].Rule.Count;
            //    ruleHead[keyA].Rule.Add(keyB);
            //}

            //Saver.SaveRuleMain(ruleHead[keyA], keyA);
            //Saver.SaveRule(ruleMain, keyA, keyB);
            //Saver.SaveRuleZip(ruleMain, keyA, keyB);
        }
        static void LoadRule()
        {
            //XMLSaver.LoadRule(tag);
        }
        #endregion

        #endregion

        #region Stol Extend

        #endregion


    }


    #region ClassBD
    public class BD
    {
        public string Name = "Void";
        public string Info = "Void";
        public List<MainBase> Base = new List<MainBase>();
    }

    public class MainBase
    {
        //public int SysName = " ";
        //public string Tayp;
        public string Name = "Void";
        public string Color = "ffff00";
        public string Info = "Void";
        public int Cost;

        /*
         */
    }

    //public class MainGuild { }
    //public class MainLegion { }
    //public class MainRace { }
    //public class MainCivilianGroup { }
    //public class MainTag { }
    //public class MainStat { }
    #endregion
    #region classRule

    public class SubRuleHead
    {
        public string Name;
        public List<string> Rule = new List<string>();
        public List<int> Index = new List<int>();
        public int LastIndex = 0;
    }

    public class HeadRule
    {
        public string Name = "Void";//Ќазвание
        public string Info;//ќписание
        public string Tag; //ќписание

        public int Cost;//÷ена
        public int CostExtend;//цена за доп очки навыков

        public int LevelCap;//ћаксимальный уровень способности

        //public int CostMovePoint;

        public bool Player;


        public List<TriggerAction> TriggerActions = new List<TriggerAction>();

        public List<string> NeedRule = new List<string>();
        public List<string> EnemyRule = new List<string>();
    }

    public class TriggerAction
    {
        public bool CountMod;
        public bool CountModExtend;
        // public int Id;
        public int TargetPalyer;
        public int Trigger;

        public string RootText;
        public string MainText;
        public string PlusText;
        public string MinusText;
        public string ActionText;


        public List<IfAction> PlusAction;
        public List<IfAction> MinusAction;

        public List<RuleAction> Action;
    }

    public class IfAction
    {
        public int Prioritet;
        public int Point;

        public int Result;

        public List<RuleForm> Core = new List<RuleForm>();

        public string Text;
    }

    public class RuleForm
    {
        public string Card = "Null";//0-null 1-card1 2-card2
        public string StatTayp;
        public string Stat;
        public int Mod = 1;
        public int Num;
    }

    public class RuleAction
    {
        public string Name = "Action";
        public int MinPoint;
        public int MaxPoint;

        public int ActionMood;//
        public string Action;//
        public List<RuleForm> Core = new List<RuleForm>();
        //public int Num;

        public int ForseMood;//

        public string RootText;
    }

    #endregion
}
