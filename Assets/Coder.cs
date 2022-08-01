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
        static int keyA, keyB, keyStat, keyTag, keyPlan, keyAssociation;

        static string mood;
        static string subMood;
        static MainBase mainBase;
        static HeadRule mainRule;
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
                case ("Rule"):
                    mainRule = Saver.LoadRule(keyA, core.head[keyA].Index[keyB]);
                    subMood = "Info";
                    TextRule("HeadInfo");
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
            keyTag = ReturnIndex("Tag");
            keyPlan = ReturnIndex("Plan");
            keyAssociation = ReturnIndex("Association");

            //Debug.Log(mood);

            switch (mood)
            {
                case ("BD"):
                    TextBD("MenuHead");
                    break;
                case ("Rule"):
                    //Saver.LoadAllRule();
                    TextRule("MenuHead");
                   // ClearIO();
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
                case ("Int"):
                    EditInt(com[1]);
                    break;

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
                        case ("Rule"):
                            TextRule(com[1]);
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
                    break;

                case ("Rule"):
                    switch (com[0])
                    {
                        case ("New"):
                            keyA = int.Parse(com[1]);
                            keyB = core.head[keyA].Index.Count;
                            if(keyB == 0)
                                core.head[keyA].Index.Add(0);
                            else
                                core.head[keyA].Index.Add(core.head[keyA].Index[keyB - 1] + 1);
                            NewMainRule();
                            core.head[keyA].Rule.Add(mainRule.Name);

                            Saver.SaveRuleMain( keyA);
                            Saver.SaveRule(mainRule, keyA, core.head[keyA].Index[keyB]);
                            break;
                        case ("Save"):
                            Saver.SaveRule(mainRule, keyA, core.head[keyA].Index[keyB]);
                            break;
                        case ("Load"):
                            mainRule = Saver.LoadRule(keyA, core.head[keyA].Index[keyB]);
                            break;
                        case ("Clear"):
                            NewMainRule();
                            break;
                        case ("Del"):
                            TT[1].text = AddLink("ClearIO", "NO") + "      " + AddLink($"Sys|Delite", "YES");
                            return;
                            break;
                        case ("Delite"):
                            Saver.DeliteRule(keyA, core.head[keyA].Index[keyB]);
                            core.head[keyA].Index.RemoveAt(keyB);
                            core.head[keyA].Rule.RemoveAt(keyB);
                            if (core.head[keyA].Index.Count == 0)
                            {
                                TextRule($"Menu_{keyA}");
                                TT[1].text = "";
                                return;
                            }
                            else
                                keyB = 0;
                            //mainRule = NewMainRule();
                            break;
                    }
                    break;
            }
            ClearIO();

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
                //Debug.Log(a);
                if (a != -1)
                    list.RemoveAt(a);

            }

            return list;
        }
        static MainBase ReturnMainBase(string str)
        {
            string[] com = str.Split('-');
            return core.bD[int.Parse(com[0])].Base[int.Parse(com[1])];
        }
        static HeadRule ReturnMainRule(string str)
        {
            string[] com = str.Split('-');

            return Saver.LoadRule(int.Parse(com[0]), int.Parse(com[1]));
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

           // GetIOText();
            TT[0].text = "";
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
                    nameTT.text = mainRule.Name;
                    break;
            }
            nameTT.gameObject.active = true;
            GetIOText(com[0]);
            Debug.Log(com[0]);
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
                    core.head[keyA].Rule[keyB] = nameTT.text;
                    Saver.SaveRuleMain(keyA);
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
                case ("Rule"):
                    TextRule($"Menu_{keyA}");
                    TextRule("HeadInfo");
                    break;
            }
        }

        static void GetIOText(string str)
        {
            TT[1].text = AddLink("ClearIO", "Back") + "      " + AddLink($"SetIO|{str}", "OK") + "\n\n";
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
        static int IfPlus(int a,int size, string mood)
        {

            if (mood == "-")
            {
                if (a > 0)
                    return a - 1;
            }
            else if (a < size-1)
                return a + 1;

            return a;
        }

        static RuleForm EditCore(RuleForm form,  string mood)
        {
            string[] com = mood.Split('*');
            switch (com[0]) 
            {
                case ("Card"):
                    form.Card = int.Parse(com[1]);
                    break;
                case ("Tayp"):
                    form.Tayp = int.Parse(com[1]);
                    if(form.Tayp !=-1)
                        form.TaypId = int.Parse(com[2]);
                    break;
                case ("Mod"):
                    if (com[1] == "-")
                    {
                        form.Mod--;
                        if (form.Mod ==0)
                            form.Mod--;
                    }
                    else
                    {
                        form.Mod++;
                        if (form.Mod == 0)
                            form.Mod++;
                    }
                    break;
                case ("Num"):
                    if (com[1] == "-")
                        form.Num--;
                    else
                        form.Num++;
                    break;
            }
            return form;
        }

        static RuleForm EditCoreStat(RuleForm form, string mood)
        {
            string[] com = mood.Split('*');
            switch (com[0])
            {
                case ("Card"):
                    form.Card = int.Parse(com[1]);
                    break;
                case ("Tayp"):
                    form.Tayp = int.Parse(com[1]);
                    if (form.Tayp != -1)
                        form.TaypId = int.Parse(com[2]);
                    break;
                case ("Mod"):
                    if (com[1] == "-")
                    {
                        form.Mod--;
                        if (form.Mod == 0)
                            form.Mod--;
                    }
                    else
                    {
                        form.Mod++;
                        if (form.Mod == 0)
                            form.Mod++;
                    }
                    break;
                case ("Num"):
                    if (com[1] == "-")
                        form.Num--;
                    else
                        form.Num++;
                    break;
            }
            return form;
        }

        static void EditInt(string str)
        {
            int a =0, b=0, c=0;
            string[] com = str.Split('*');
            string mood = com[1];

            com = com[0].Split('_');
            if (com.Length > 1)
            {
                a = int.Parse(com[0]);
                b = int.Parse(com[1]);
            }
            switch (com[0])
            {
                case ("Cost"):
                    if (mood == "-")
                        mainRule.Cost--;
                    else
                        mainRule.Cost++;
                    break;

                case ("Team"):
                    mainRule.Trigger[a].Team = IfPlus(mainRule.Trigger[a].Team, core.frame.PlayerString.Length, mood);
                    break;

                case ("ActionPlus"):
                    if (mood == "-")
                        mainRule.Trigger[a].PlusAction[b].Point--;
                    else
                        mainRule.Trigger[a].PlusAction[b].Point++;
                    break;
                case ("ActionMinus"):
                    if (mood == "-")
                        mainRule.Trigger[a].MinusAction[b].Point--;
                    else
                        mainRule.Trigger[a].MinusAction[b].Point++;
                    break;
                case ("ActionMin"):
                    if (mood == "-")
                        mainRule.Trigger[a].Action[b].Min--;
                    else
                        mainRule.Trigger[a].Action[b].Min++;

                    if (mainRule.Trigger[a].Action[b].Min > mainRule.Trigger[a].Action[b].Max)
                        mainRule.Trigger[a].Action[b].Max = mainRule.Trigger[a].Action[b].Min;
                    break;
                case ("ActionMax"):
                    if (com[3] == "-")
                        mainRule.Trigger[a].Action[b].Max--;
                    else
                        mainRule.Trigger[a].Action[b].Max++;

                    if (mainRule.Trigger[a].Action[b].Min < mainRule.Trigger[a].Action[b].Max)
                        mainRule.Trigger[a].Action[b].Min = mainRule.Trigger[a].Action[b].Max;
                    break;


                case ("ActionPlusResult"):
                    mainRule.Trigger[a].PlusAction[b].Point = IfPlus(mainRule.Trigger[a].PlusAction[b].Point, core.frame.EqualString.Length, mood);
                    break;
                case ("ActionMinusResult"):
                    mainRule.Trigger[a].MinusAction[b].Point = IfPlus(mainRule.Trigger[a].MinusAction[b].Point, core.frame.EqualString.Length, mood);
                    break;

                case ("ActionTeam"):
                    mainRule.Trigger[a].Action[b].Team = IfPlus(mainRule.Trigger[a].Action[b].Team, core.frame.PlayerString.Length, mood);
                    break;
                case ("ActionForse"):
                    mainRule.Trigger[a].Action[b].ForseMood = IfPlus(mainRule.Trigger[a].Action[b].ForseMood, core.frame.ForseTayp.Length, mood);
                    break;




                default:
                    c = int.Parse(com[2]);
                    RuleForm form = null;
                    switch (com[3])
                    {
                        case ("Plus"):
                            form = mainRule.Trigger[a].PlusAction[b].Core[c]; 
                            break;
                        case ("Minus"):
                            form = mainRule.Trigger[a].MinusAction[b].Core[c];
                            break;
                        default:
                            form = mainRule.Trigger[a].Action[b].Core[c];
                            break;
                    }
                    if(mood != "-" && mood != "+")
                    {
                        string[] com1 = com[4].Split('(');
                        mood = com1[0];
                        for (int i = 1; i < com1.Length; i++)
                            mood += "*" + com1;
                    }
                    else
                        mood = com[4] + "*" + mood;

                    form = EditCore(form, mood);

                    switch (com[3])
                    {
                        case ("Plus"):
                            mainRule.Trigger[a].PlusAction[b].Core[c] = form;
                            break;
                        case ("Minus"):
                            mainRule.Trigger[a].MinusAction[b].Core[c] = form;
                            break;
                        default:
                            mainRule.Trigger[a].Action[b].Core[c] = form;
                            break;
                    }
                    break;
            }
            ClearIO();
        }

        static string TextEditInt(string path, int cost)
        {
            return AddLink($"Int|{path}*-", "<<") + $" ({cost}) " + AddLink($"Int|{path}*+", ">>");
        }
        static string ActionText(string head, string path, int a, int b)
        {
            
            string str = "";
            if(b != -1)
            {
                switch (head)
                {//Int|ActionPlus_{a}_{b}_{c}
                    case ("ActionPlus"):
                        str += TextEditInt(path,mainRule.Trigger[a].PlusAction[b].Point);
                        break;
                    case ("ActionMinus"):
                        str += TextEditInt(path, mainRule.Trigger[a].MinusAction[b].Point);
                        break;
                    case ("ActionMin"):
                        str += TextEditInt(path, mainRule.Trigger[a].Action[b].Min);
                        break;
                    case ("ActionMax"):
                        str += TextEditInt(path, mainRule.Trigger[a].Action[b].Max);
                        break;
                }

            }

            return str;
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
        static void TextRule(string str)
        {
            int a, b;
            string[] com = str.Split('_');

            switch (com[0])
            {
                case ("MenuHead"):
                    str = "";
                    Debug.Log(core.head.Count);
                    for (int i = 0; i < core.head.Count; i++)
                        str += AddLink($"Open|Menu_{i}", core.head[i].Name + IfLook(core.bD[keyTag].Base[i].Look) + $"({core.head[i].Index.Count})") + "\n";

                    TT[0].text = str;
                    break;

                case ("Menu"):
                    a = int.Parse(com[1]);
                    str = AddLink("Open|MenuHead", "Back") + "\n";
                    str += AddLink($"Sys|New_{a}", "New Rule " + core.head[a].Name);
                    str += "\n";

                    Debug.Log(core.head[a].Index.Count);
                    for (int i = 0; i < core.head[a].Index.Count; i++)
                    {
                        Debug.Log(core.head[a].Rule[i]);
                        str += "\n  " + AddLink($"Key|{a}-{i}", $"({core.head[a].Index[i]})"+ core.head[a].Rule[i]);
                    }
                    //str += WebText(a);
                    TT[0].text = str;
                    break;


                case ("HeadInfo"):
                    com = subMood.Split('_');
                    switch (com.Length)
                    {
                        case (1):
                            str = AddLink($"Sys|Save", "Save");
                            str += "    ";
                            str += AddLink($"Sys|Load", "Load");
                            str += "    ";
                            str += AddLink($"Sys|Clear", "Clear");
                            str += "    ";
                            str += AddLink($"Sys|Del", "Delite");
                            str += "\n\n";
                            str +=  AddLink("GetIO|RuleName", $"({keyB})Правило -- { mainRule.Name}");
                            // str += AddLink("Edit_Cost", $"\nЦена: { mainBase.Cost}");
                            str += $"\nЦена: " + TextEditInt("Cost", mainRule.Cost);
                            str += $"\nTag {core.bD[keyTag].Base[ mainRule.Tag].Name}";
                            // str += AddLink("Tag_Tag", $"\nTag { mainBase.Tag}");


                            str += "\nТребуемые механики";
                            str += AddLink("Edit|NeedRule_Add", $"\nСоздать связь");
                            for (int i = 0; i < mainRule.NeedRule.Count; i++)
                                str += AddLink($"Edit|NeedRule_Remove_{i}", $"\n    Разорвать связь с {ReturnMainRule(mainRule.NeedRule[i]).Name}");

                            str += "\nИсключающие механики";
                            str += AddLink("Edit|EnemyRule_Add", $"\nСоздать связь");
                            for (int i = 0; i < mainRule.EnemyRule.Count; i++)
                                str += AddLink($"Edit|EnemyRule_Remove_{i}", $"\n    Разорвать связь с {ReturnMainRule(mainRule.EnemyRule[i]).Name}");

                            for (int i = 0; i < mainRule.Trigger.Count; i++)
                                str += AddLink($"SubMood|Trigger_{i}", $"\n    {NameTrigger(i)}") +"   " +  AddLink($"Edit|Trigger_Remove_{i}", "-Remove");
                            str += AddLink($"Edit|Trigger_Add", "-Add");
                            break;
                        case (2):

                            /*



        public List<IfAction> PlusAction;
        public List<IfAction> MinusAction;

        public List<RuleAction> Action;
                             
                             */
                            a = int.Parse(com[1]);
                            //b = mainRule.Trigger[a].Trigger;
                            str = "";
                            //if (core.frame.Trigger[b].Extend.Length > 1)
                            //    str += AddLink($"Edit|TriggerExtend_Open_{a}", $"Режим -- " + core.frame.Trigger[b].Extend[mainRule.Trigger[a].TriggerExtend]); 

                            str += AddLink($"SetSwitch|CountMod_{a}", $"Правило с счетчиком -- " + ((mainRule.Trigger[a].CountMod)? "Yes" : "No"));
                            str += AddLink($"SetSwitch|CountModExtend_{a}", $"Правило с разницей -- " + ((mainRule.Trigger[a].CountModExtend) ? "Yes" : "No"));

                            str += AddLink($"Edit|TargetPalyer_{a}", $"\n Целевой игрок " + core.frame.PlayerString[mainRule.Trigger[a].Team]);


                            //Edit|ActionPlus_Up_{A}_{i}
                            //Edit|ActionPlus_Down_{A}_{i}

                            //Edit|ActionPlus_Edit_{A}_{i}

                            //Edit|ActionPlus_EditNew_{A}
                            //Edit|ActionPlus_EditNew_{A}_{i}
                            //Edit|ActionPlus_EditRemove_{A}_{i}
                            //Edit|ActionPlus_Edit_{A}_{i}
                            //Edit|ActionPlus_Edit_{A}_{i}_{i1}_Edit

                            str += "\n Положительные условия";
                            str += AddLink($"Edit|Action_Plus_{a}", $"\nСоздать условие");
                            for (int i = 0; i < mainRule.Trigger[a].PlusAction.Count; i++)
                                str += AddLink($"Edit|NeedRule_Remove_{i}", $"\n    Разорвать связь с {ReturnMainRule(mainRule.NeedRule[i]).Name}");

                            str += "\nИсключающие условия";
                            str += AddLink("Edit|EnemyRule_Add", $"\nСоздать связь");
                            for (int i = 0; i < mainRule.EnemyRule.Count; i++)
                                str += AddLink($"Edit|EnemyRule_Remove_{i}", $"\n    Разорвать связь с {ReturnMainRule(mainRule.EnemyRule[i]).Name}");

                            str += "\nДействия";
                            for (int i = 0; i < mainRule.Trigger.Count; i++)
                                str += AddLink($"SubMood|Trigger_{i}", $"\n    {NameTrigger(i)}") + "   " + AddLink($"Edit|Trigger_Remove_{i}", "-Remove");
                            str += AddLink($"Edit|Trigger_Add", "-Add");
                            break;
                            /*
                             системная привязка на выборку, для действия она завзана только на статах, для остальных свободный доступ
                             
                             */
                    }





                    TT[1].text = str;
                    break;



                //case ("Info"):
                //    str = HeadBDInfo();
                //    if (keyA == keyStat)
                //    {
                //        str += AddLink("SetSwitch|Regen", "Regen " + ((mainBase.Sub.Regen) ? "Yes" : "No")) + "\n";

                //        str += AddLink("Switch|Icon", $"Icon <index={mainBase.Sub.Image}>") + "\n";
                //        str += AddLink("Switch|Antipod", (mainBase.Sub.Antipod == -1) ? "Null" : core.bD[keyStat].Base[mainBase.Sub.Antipod].Name) + "\n";

                //        str += "\nСписок AntiStat для доступа";
                //        str += WebText(mainBase.Sub.AntiStat, "AntiStat");

                //        str += "\nСписок DefStat для доступа";
                //        str += WebText(mainBase.Sub.DefStat, "DefStat");

                //    }
                //    for (int i = 0; i < mainBase.Text.Count; i++)
                //    {
                //        str += $"\nСписок {core.bD[keyA].Key[i]} для доступа";
                //        str += AddLink($"SetSwitch|Hide_{keyA}_{i}", (core.bD[keyA].Hide[i]) ? "Close" : $"Open ({mainBase.Text[i].Text.Count})") + "\n";
                //        if (core.bD[keyA].Hide[i])
                //        {
                //            str += WebText(mainBase.Text[i].Text, i);
                //        }
                //    }

                //    TT[1].text = str;
                //    break;
            }
            //return str;
        }
        static string NameTrigger(int i)
        {
            string str = "Trigger";
            str += "    " + core.bD[keyPlan].Base[ mainRule.Trigger[i].Plan].Name ;
            str += "    " + core.frame.Trigger[mainRule.Trigger[i].Trigger] ;
            //if(core.frame.Trigger[mainRule.Trigger[i].Trigger].Extend.Length >1)
            //    str += core.frame.Trigger[mainRule.Trigger[i].Trigger].Extend[mainRule.Trigger[i].TriggerExtend];
            return str;
        }


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
        static string ReturnRuleList(string path, string text, int a =0)
        {
            string str = "";
            switch (text)
            {
                case ("Trigger"):
                    for (int i = 0; i < core.frame.Trigger.Length; i++)
                        str += $"\n" + AddLink($"Edit|Trigger_Add_{i}", $"Add {core.frame.Trigger[i]}");// " Add|-1 null";
                    break;

                case ("Tayp"):
                    str += $"\n" + AddLink($"Edit|Tayp_-1_{path}", $"Set Null");
                    for (int i = 0; i < core.bD.Count; i++)
                        str += $"\n" + AddLink($"Edit|Tayp_{i}_{path}", $"Set { core.bD[i].Name}");
                    break;
                case ("TaypId"):
                    for (int i = 0; i < core.bD[a].Base.Count; i++)
                        str += $"\n" + AddLink($"Edit|TaypId_{a}_{i}_{path}", $"Set { core.bD[a].Base[i].Name}");
                    break;
                case ("Card"):
                    for(int i = 0;i < core.frame.CardString.Length;i++)
                        str += $"\n" + AddLink($"Edit|Card_-{i + 1}_{path}", $"Set { core.frame.CardString[i]}");

                    //core.frame.CardString[-(coreForm.Card + 1)]
                    for (int i = 0; i < core.bD[keyAssociation].Base.Count; i++)
                        str += $"\n" + AddLink($"Edit|Card_{i}_{path}", $"Set { core.bD[keyAssociation].Base[i].Name}");

                    break;
                case ("TaregtPlayer"):
                    for (int i = 0; i < core.frame.PlayerString.Length; i++)
                        str += $"\n" + AddLink($"Edit|TaregtPlayer_{i}_{path}", $"Set { core.frame.PlayerString[i]}");
                    break;
                case ("Forse"):
                    for (int i = 0; i < core.frame.ForseTayp.Length; i++)
                        str += $"\n" + AddLink($"Edit|Forse_{i}_{path}", $"Set { core.frame.ForseTayp[i]}");
                    break;
                case ("Action"):
                    str += $"\n Add|-1 null";
                    break;
                case ("Plan"):
                    //ForseTayp
                    str += $"\n Add|-1 AllPlans";
                    break;
            }
            return str;
        }

        static string ReturnCore(RuleForm coreForm , string path = " ")
        {
            bool edit = (path != " ");

            string str = "", text ="";
            if (coreForm.Card < 0)
                text = $"\n{core.frame.CardString[-(coreForm.Card +1)]}";
            else
                text = $"\n{core.bD[keyAssociation].Base[coreForm.Card].Name}";

            if(!edit)
                str += $"{text}";
            else
                str += AddLink($"Edit|RuleList_Card_{path}" , text);




            if (coreForm.Tayp != -1)
                text = $" {core.bD[coreForm.Tayp].Name}";
            else
                text = $" Null";

            if (!edit)
                str += $" {text}";
            else
                str += $" " + AddLink($"Edit|RuleList_Tayp_{path}", text);

            if (coreForm.Tayp != -1)
            {
                if (coreForm.Tayp != -1)
                    text = $" {core.bD[coreForm.Tayp].Base[coreForm.TaypId].Name}";
                else
                    text = "Null";
                if (!edit)
                    str += $" {text}";
                else
                    str += $" " + AddLink($"Edit|RuleList_TaypId_{path}", text);
            }

            if (!edit)
            {
                str += $" {coreForm.Mod}";
                str += $" {coreForm.Num}";

            }
            else
            {
                str += TextEditInt(path, coreForm.Mod);
                str += TextEditInt(path, coreForm.Num);
            }



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

        #region Rule Extend
        #region Create

        static void NewMainRule()
        {
            mainRule = new HeadRule();
            mainRule.Tag = keyA;
            mainRule.Name = "Void";

            mainRule.Trigger = new List<TriggerAction>();

            mainRule.NeedRule = new List<string>();
            mainRule.EnemyRule = new List<string>();
        }

        #endregion



        //#region Stol Extend

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
    /*
     специальные комманды 
    мана
    скорость(была инициатива)
    маин стат
    карма текущая общая количество
     
     */
    public class SubRuleHead
    {
        public string Name;
        public List<string> Rule = new List<string>();
        public List<int> Index;// = new List<int>();
        public int LastIndex = 0;
    }

    public class HeadRule
    {
        public string Name;//Название
       // public string Info = "Void";//Описание
        public int Tag; //Описание

        public int Cost;//Цена

        public List<TriggerAction> Trigger;// = new List<TriggerAction>();

        public List<string> NeedRule;// = new List<string>();
        public List<string> EnemyRule;//= new List<string>();
    }

    public class TriggerAction
    {
        public int Plan;
        public int Trigger;
        //public int TriggerExtend;

        public bool CountMod;
        public bool CountModExtend;
        // public int Id;
        public int Team;


        public List<IfAction> PlusAction;
        public List<IfAction> MinusAction;

        public List<RuleAction> Action;
    }

    public class IfAction
    {
        public int Point;

        public List<RuleForm> Core;// = new List<RuleForm>();
        public List<int> Result;// = new List<int>();
    }

    public class RuleForm
    {
        public int Card = -1;//для конкретных значений использеются отрицательны значения, в противном случае используется асоциация "Null";//0-null 1-card1 2-card2
        public int Tayp = -1; 
        public int TaypId = 0;
        public int Mod = 1;
        public int Num = 0;
    }

    public class RuleAction
    {
        public string Name = "Void";
        public int Action;
        public int ActionExtend;

        //public string Name = "Action";
        public int Min;
        public int Max;

        public int Team;

        public int RuleTag;
        public int Rule;
        public List<RuleForm> Core;// = new List<RuleForm>();

        public int ForseMood;

    }

    #endregion
}
