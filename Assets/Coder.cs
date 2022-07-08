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
        static List<string> edit;// = new List<string>();
        static int keyA, keyB, keyStat;

        static string mood;
        static MainBase mainBase;
        //public static void SetMood(string str) { mood = str; }

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
        static void SetKey(string str)
        {
            string[] com = str.Split('-');
            keyA = int.Parse(com[0]);
            keyB = int.Parse(com[1]);

            switch (mood)
            {
                case ("BD"):
                    mainBase = core.bD[keyA].Base[keyB];
                    TextBD("Info");
                    break;
            }
        }

        public static int ReturnIndex(string str) { return core.frame.Tayp.FindIndex(x => x == str); }

        static void AddEdit(string str)
        {
            edit.Add(str);
        }
        static void ClearEdit()
        {
            edit = new List<string>();
        }
        #endregion
        /*
         * Sys - раздел системных комманд
         * Open - открывает определенный раздел проводника 
         * Switch - переключает раздел на другой из содержимого статической библиотеки
         * Link - сформировать ссылку для текста
         * Re - взять текст из поля
         */

        public static void Starter(string str)
        {
            mood = str;
            ResetKey();
            ClearEdit();
            keyStat = ReturnIndex("Stat");
            //Debug.Log(mood);
            
            switch (mood)
            {
                case ("BD"):
                    TextBD("MenuHead");
                    break;
            }
        }

        #region Coder
        public static void Read(string str)
        {
            Debug.Log(str);
            string[] com = str.Split('|');
            switch (com[0])
            {
                case ("Key"):
                    SetKey(com[1]);
                    break;
                case ("Sys"):
                    Sys(com[1]);
                    break;


                case ("GetIO"):
                    GetIO(com[1]);
                    break;
                case ("SetIO"):
                    SetIO(com[1]);
                    break;
                case ("ClearIO"):
                    ClearIO();
                    break;
                case ("Open"):
                    switch (mood)
                    {
                        case ("BD"):
                            TextBD(com[1]);
                            break;
                    }
                    break;
                case ("Edit"):
                    switch (mood)
                    {
                        case ("BD"):
                            EditBDCase(com[1]);
                            break;
                    }
                    break;

                case ("Switch"):
                    Switch(com[1]);
                    break;
                case ("SetSwitch"):
                    SetSwitch(com[1]);
                    break;
            }
        }

        static void Sys(string str)
        {
            int a, b;

            string[] com = str.Split('_');
            switch (mood)
            {
                case ("BD"):
                    switch (com[0])
                    {
                        case ("New"):
                            keyA = int.Parse(com[1]);
                            keyB = core.bD[keyA].Base.Count;
                            core.bD[keyA].Base.Add(NewMainBase());
                            mainBase = core.bD[keyA].Base[keyB];
                            Saver.SaveBD(keyA, keyB);
                            //ClearIO();
                            //return;
                            break;
                        case ("Save"):
                            for(int i = 0; i < edit.Count; i++)
                            {
                                com = edit[i].Split('-');
                                a =int.Parse(com[0]);
                                b = int.Parse(com[1]);

                                Saver.SaveBD(a, b);
                            }
                            ClearEdit();
                            break;
                        case ("Load"):
                            for (int i = 0; i < edit.Count; i++)
                            {
                                com = edit[i].Split('-');
                                a = int.Parse(com[0]);
                                b = int.Parse(com[1]);

                                Saver.LoadBD(a, b);
                            }
                            ClearEdit();
                            break;
                        case ("Clear"):
                            core.bD[keyA].Base[keyB] = NewMainBase(); 
                            AddEdit($"{keyA}-{keyB}");
                            //ClearIO();
                            break;
                    }
                    ClearIO();
                    break;
            }

        }

        static List<int> AddListText(List<int> list, int b, bool add)
        {
            int a;
            if (add)
            {
                if (list.Count == 0)
                    list = new List<int>();
                a = list.FindIndex(x => x == b);
                if (a == -1)
                    list.Add(b);
            }
            else if (list.Count > 0)
            {
                a = list.FindIndex(x => x == b);
                Debug.Log(a);
                if (a == -1)
                    list.RemoveAt(a);

            }

            return list;
        }
        static MainBase ReturnMainBase(string str)
        {
            string[] com = str.Split('-');
            return core.bD[int.Parse(com[0])].Base[int.Parse(com[1])];
        }

        static void EditBDCase(string str)
        {
            AddEdit($"{keyA}-{keyB}");
            int a, b,c;
            string[] com = str.Split('_');
            bool add = (com[2] == "1");
            switch (com[0])
            {
                case ("AntiStat"):
                    mainBase.Sub.AntiStat = AddListText(mainBase.Sub.AntiStat, int.Parse(com[1]), add);
                    break;
                case ("DefStat"):
                    mainBase.Sub.DefStat = AddListText(mainBase.Sub.DefStat, int.Parse(com[1]), add);
                    break;
                default:
                    a = int.Parse(com[0]);
                    b = int.Parse(com[1]);


                    mainBase.Text[a].Text= AddListText(mainBase.Text[a].Text, b, add);

                    a = core.bD[keyA].KeyId[a];

                    AddEdit($"{a}-{b}");

                    c = core.bD[a].Key.FindIndex(x => x == core.frame.Tayp[keyA]);
                    core.bD[a].Base[b].Text[c].Text = AddListText(core.bD[a].Base[b].Text[c].Text, keyB, add);
                    break;
            }



            ClearIO();
        }
        static void Switch(string str)
        {
            int a;
            string[] com = str.Split('_');

            str = AddLink("ClearIO", "Back") + "\n\n";

            switch (com[0])
            {

                case ("Antipod"):
                    //a = ReturnIndex("Stat");
                    str += AddLink("SetSwitch|Antipod_-1", "Null") + "\n";
                    for(int i = 0; i < core.bD[keyStat].Base.Count; i++)
                        str += AddLink($"SetSwitch|Antipod_{keyStat}-{i}", core.bD[keyStat].Base[i].Name) + "\n";
                    //mainBase.Antipod = bool.Parse(root.Element("Antipod").Value);
                    break;
                case ("Color"):
                    str += SwitchColor();
                    break;
                case ("Icon"):
                    str += SwitchIcon();
                    break;
            }
            TT[0].text = str;


        }
        static void SetSwitch(string str)
        {
            int a, b;
            MainBase mainBase1 = null;
            string[] com = str.Split('_');
            AddEdit($"{keyA}-{keyB}");
            switch (com[0])
            {

                case ("Antipod"):
                    a = int.Parse(com[1]);
                    if (a == -1)
                        if (mainBase.Sub.Antipod == -1)
                            return;

                    AddEdit(com[1]);
                    mainBase1 = ReturnMainBase(com[1]);
                    if (mainBase1.Sub.Antipod != -1)
                    {

                        AddEdit($"{keyStat}-{mainBase1.Sub.Antipod}");
                        core.bD[keyStat].Base[mainBase1.Sub.Antipod].Sub.Antipod = -1;

                    }

                    mainBase.Sub.Antipod = a;
                    mainBase1.Sub.Antipod = keyB;

                    break;
                case ("Color"):
                    mainBase.Color = com[1];
                    break;
                case ("Icon"):
                    mainBase.Sub.Image = int.Parse(com[1]);
                    break;
                case ("Look"):
                    mainBase.Look = !mainBase.Look;
                    break;
                case ("Regen"):
                    mainBase.Sub.Regen = !mainBase.Sub.Regen;
                    break;
                case ("Hide"):
                    a = int.Parse(com[1]);
                    b = int.Parse(com[2]);
                    core.bD[a].Hide[b] = !core.bD[a].Hide[b];
                    break;
            }
            ClearIO();
        }

        #endregion

        #region IO
        static void GetIO(string str)
        {
            string[] com = str.Split('_');

            Debug.Log(nameTT.text);
            switch (com[0])
            {
                case ("HeadBase"):
                    nameTT.text = core.bD[keyA].Name;
                    break;
                case ("HeadInfo"):
                    nameTT.text = core.bD[keyA].Info;
                    break;
                case ("Base"):
                    nameTT.text = mainBase.Name;
                    break;
                case ("Info"):
                    nameTT.text = mainBase.Info;
                    break;

                case ("CardName"):
                    // cardBase = nameTT.text;
                    break;

                case ("RuleName"):
                    //  cardBase = nameTT.text;
                    break;
            }
            TT[0].text = "";
            Debug.Log(nameTT.text);
            nameTT.gameObject.active = true;
            GetIOText(com[0]);
        }

        static void SetIO(string str)
        {
            switch (str)
            {
                case ("HeadBase"):
                    core.bD[keyA].Name = nameTT.text;
                    Saver.SaveBDMain(keyA);
                    break;
                case ("HeadInfo"):
                    core.bD[keyA].Info = nameTT.text;
                    Saver.SaveBDMain(keyA);
                    break;
                case ("Base"):
                    AddEdit($"{keyA}-{keyB}");
                    mainBase.Name = nameTT.text;
                    break;
                case ("Info"):
                    AddEdit($"{keyA}-{keyB}");
                    mainBase.Info = nameTT.text;
                    break;

                case ("CardName"):
                   // cardBase = nameTT.text;
                    break;

                case ("RuleName"):
                  //  cardBase = nameTT.text;
                    break;
            }

            ClearIO();
        }

        static void ClearIO()
        {
            nameTT.gameObject.active = false;
            switch (mood)
            {
                case ("BD"):
                    TextBD($"Menu_{keyA}");
                    TextBD("Info");
                    break;
            }
        }

        static void GetIOText(string str)
        {
            string str1 = "";
            switch (mood)
            {
                case ("BD"):

                    str1 = AddLink("ClearIO", "Back") +"      " + AddLink($"SetIO|{str}", "OK") +"\n\n";
                    //if (str == "HeadBase" || str == "Base")
                    //{
                    //}
                    TT[1].text = str1;
                    //switch (str)
                    //{
                    //    case ("HeadBase"):
                    //        nameTT.text = core.bD[keyA].Name;
                    //        break;
                    //    case ("HeadInfo"):
                    //        nameTT.text = core.bD[keyA].Info;
                    //        break;
                    //    case ("Base"):
                    //        nameTT.text = mainBase.Name;
                    //        break;
                    //    case ("Info"):
                    //        nameTT.text = mainBase.Info;
                    //        break;
                    //        break;
                    //}
                        
                    break;
            }
        }
        #endregion

        #region Text
        static string WebText(int a)
        {
            string str = "";
            for (int i = 0; i < core.bD[a].Base.Count; i++)
            {
                str += "\n  " + AddLink($"Key|{a}-{i}", core.bD[a].Base[i].Name + IfLook(core.bD[a].Base[i].Look)) ;
            }

            return str;
        }
        static string WebText(int a, int b)
        {
            string str = "";
            for (int i = 0; i < core.bD[b].Base.Count; i++)
            {
                str += "\n  " + AddLink($"Edit|{a}_{i}_1", core.bD[b].Base[i].Name + IfLook(core.bD[b].Base[i].Look) + "-Add") ;
            }

            return str;
        }
        static string WebText( string mood)
        {
            string str = "";
            for (int i = 0; i < core.bD[keyStat].Base.Count; i++)
            {
                str += "\n  " + AddLink($"Edit|{mood}_{i}_1", core.bD[keyStat].Base[i].Name + IfLook(core.bD[keyStat].Base[i].Look) + "-Add");
            }

            return str;
        }

        static string WebText(List<int> list, int a)
        {
            int c = core.bD[keyA].KeyId[a];
            int b;
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                b = list[i];
                str += "\n  " + AddLink($"Key|{c}-{b}", core.bD[c].Base[b].Name) + IfLook(core.bD[c].Base[b].Look);
                str += "     " + AddLink($"Edit|{a}_{b}_0", "-Remove", core.frame.ColorsStr[1]);
            }
            str += "\n  " +AddLink($"Open|NewInfo_{a}_{c}", "-Add");
            

            return str;
        }
        static string WebText(List<int> list, string mood)
        {
            int b;
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                b = list[i];
                str += "\n  " + AddLink($"Key|{keyStat}-{b}", core.bD[keyStat].Base[b].Name) + IfLook(core.bD[keyStat].Base[b].Look);
                str += "     " + AddLink($"Edit|{mood}_{b}_0", "-Remove", core.frame.ColorsStr[1]);
            }
            str += "\n  " + AddLink($"Open|NewInfo_{mood}", "-Add");


            return str;
        }

        static string IfLook(bool use)
        {
            return(use) ? "[]" : "[ ]";
        }

        static void TextBD(string str)
        {
            int a, b;
            string[] com = str.Split('_');
            switch (com[0])
            {
                case ("MenuHead"):
                    str = "";//AddLink("Open|Menu", "Open SysMenu") + "\n"; ;
                    for (int i = 0; i < core.bD.Count; i++)
                        str += AddLink($"Open|Menu_{i}", core.bD[i].Name + $"({core.bD[i].Base.Count})") + "\n";

                    TT[0].text = str;
                    break;

                case ("Menu"):
                    a = int.Parse(com[1]);
                   // bool look = (0 <= core.frame.Tayp[a].Key.FindIndex(x => x == "Look"));
                    str = AddLink("Open|MenuHead", "Back") + "\n";
                    str += AddLink($"Sys|New_{a}", "New " + core.bD[a].Name);
                    str += "\n";
                    str += WebText(a);
                    TT[0].text = str;
                    break;

                case ("NewInfo"):
                    str = AddLink("ClearIO", "Back") + "\n\n";
                    switch (com[1])
                    {
                        case ("AntiStat"):
                            str += WebText("AntiStat");
                            break;
                        case ("DefStat"):
                            str += WebText("DefStat");
                            break;
                        default:
                            str += WebText(int.Parse(com[1]), int.Parse(com[2]));
                            break;
                    }
                    TT[0].text = str; 
                    break;

                case ("Info"):
                    str = HeadBDInfo();
                    if(keyA == keyStat)
                    {
                        str += AddLink("SetSwitch|Regen", "Regen " + ((mainBase.Sub.Regen) ? "Yes" : "No")) + "\n";

                        str += AddLink("Switch|Icon", $"Icon <index={mainBase.Sub.Image}>") + "\n";
                        str += AddLink("Switch|Antipod", (mainBase.Sub.Antipod == -1) ? "Null" : core.bD[keyStat].Base[mainBase.Sub.Antipod].Name) + "\n";

                        str += "\nСписок AntiStat для доступа";
                        str += WebText(mainBase.Sub.AntiStat, "AntiStat"); 

                        str += "\nСписок DefStat для доступа";
                        str += WebText(mainBase.Sub.DefStat, "DefStat");

                    }
                    for (int i = 0; i < mainBase.Text.Count; i++)
                    {
                        str += $"\nСписок {core.bD[keyA].Key[i]} для доступа";
                        str += AddLink($"SetSwitch|Hide_{keyA}_{i}", (core.bD[keyA].Hide[i]) ? "Close" : $"Open ({mainBase.Text[i].Text.Count})") + "\n";
                        if (core.bD[keyA].Hide[i])
                        {
                            str += WebText(mainBase.Text[i].Text, i);
                        }
                    }

                    TT[1].text = str;
                    break;
            }
            //return str;
        }
        //static string TextBDExtend( int i)
        //{
        //    int a;
        //    string str = AddLink($"SetSwitch|Hide_{keyA}_{i}", (tayp.Hide[i]) ? "Close" : $"Open ({list.Count})") + "\n";
        //    if (core.bD[keyA].Hide[i])
        //    {
        //        str += WebText(core.bD[keyA].Base[keyB].Text[i].Text, i);
        //        str += AddLink($"Edit|{keyA}_{keyB}_{core.bD[keyA].Key[i]}_1", "-Add") + "\n";
        //    }
        //    return str;
        //}


        static string HeadBDInfo()
        {
            string str = "";

            str += AddLink($"Sys|Save", "Save");
            str += "    ";
            str += AddLink($"Sys|Load", "Load");
            str += "    ";
            str += AddLink($"Sys|Clear", "Clear");
            str += "\n\n";


            str += AddLink($"GetIO|HeadBase", "Head name :" + core.bD[keyA].Name);

            string link = $"GetIO|HeadInfo";
            str +=(core.bD[keyA].Info == "Void") ? AddLink(link, "[!]", core.frame.ColorsStr[1]) : AddLink(link, "[?]", core.frame.ColorsStr[0]);

            str += $"\n";
            str += $"\n{keyB}- ";

            str += AddLink("GetIO|Base", "Name " + mainBase.Name, mainBase.Color);
            link = $"GetIO|Info";
            str += (mainBase.Info == "Void") ? AddLink(link, "[!]", core.frame.ColorsStr[1]) : AddLink(link, "[?]", core.frame.ColorsStr[0]);

            str += "\n" + AddLink("Swicth|Color", $"Color = {mainBase.Color}", mainBase.Color);

            str += "\n" + $"Cost {mainBase.Cost}";
            str += "\n" + AddLink("SetSwitch|Look", ((mainBase.Look) ? "Close" : "Open") + IfLook(mainBase.Look)) + "\n";

            return str;
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
            return $"<link=SetSwitch|Icon_{i}><index={i}></link>";
        }
        static string AddLinkColor(int i)
        {
            string str = core.frame.ColorsStr[i];
            return $"<link=SetSwitch|Color_{str}><color=#{str}>{str}</color></link>";
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
        #region BD Extend
        static MainBase NewMainBase()
        {
            MainBase mainBase = new MainBase();
            if(keyA == keyStat)
            {
                mainBase.Sub = new SubInt();
                mainBase.Sub.AntiStat = new List<int>();
                mainBase.Sub.DefStat = new List<int>();
            }

            if ("Race" == core.frame.Tayp[keyA])
            {
                mainBase.Race = new SybRace();
                //mainBase.Race.MainRace
            }

            mainBase.Text = new List<SubText>();
            for (int i = 0; i < core.bD[keyA].Key.Count; i++)
                mainBase.Text.Add(new SubText());
            //mainBase.Guild = new List<string>();
            //mainBase.Legion = new List<string>();
            //mainBase.Civilian = new List<string>();
            //mainBase.Stat = new List<string>();
            //mainBase.AntiStat = new List<string>();
            //mainBase.DefStat = new List<string>();
            //switch (mood)
            //{

                //}
            return mainBase;
        }
        #endregion
        #region Card Extend
        CardBase cardBase;

        #endregion

        #region Rule Extend
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
        public List<string> Key;
        public int[] KeyId;
        public bool[] Hide;
    }

    public class MainBase
    {
        //public int SysName = " ";
        //public string Tayp;
        public string Name = "Void";
        public string Color = "ffff00";
        public string Info = "Void";
        public int Cost;

        //Race
       // public List<string> Key; 
        public List<SubText> Text = new List<SubText>();
        //public List<string> Legion;
        //public List<string> Civilian;
        //public List<string> Stat;
        public SubInt Sub;
        public SybRace Race;

        public bool Look;

    }
    //public class SybCivil
    //{
    //    public int MainStat = 0;
    //    public int MainRace = -1;
    //}
    public class SybRace
    {
        public int MainStat =0;
        public int MainRace =-1;
    }

    public class SubInt
    {
        public bool Regen;
        public int Image = 0;
        public int Antipod = -1;
        public List<int> AntiStat = new List<int>();
        public List<int> DefStat = new List<int>();
    }

    public class SubText
    {
        public List<int> Text = new List<int>();
    }


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
        public string Name = "Void";//Название
        public string Info;//Описание
        public string Tag; //Описание

        public int Cost;//Цена
        public int CostExtend;//цена за доп очки навыков

        public int LevelCap;//Максимальный уровень способности

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
