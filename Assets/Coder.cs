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
        static string subMood = "HeadInfo";
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
                    SetSubMood("HeadInfo");
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
                case ("Move"):
                    EditMove(com[1]);
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
                        case ("Rule"):
                            EditRule(com[1]);
                            break;
                    }
                    break;

                case ("Switch"):
                    Switch(com[1]);
                    break;
                case ("SetSwitch"):
                    SetSwitch(com[1]);
                    break;
                case ("SetMood"):
                    SetSubMood(com[1]);
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
                            core.head[keyA].Rule.Add("Void");

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
                            //core.head[keyA].Index.RemoveAt(keyB);
                            //core.head[keyA].Rule.RemoveAt(keyB);
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

        static RuleForm ReturnCoreOrig(string str)
        {
            string[] com = str.Split('?');
            RuleForm core = null; 
            switch (com[0])
            {
                case ("ResultPlus"):
                    core = mainRule.Trigger[int.Parse(com[1])].PlusAction[int.Parse(com[2])].ResultCore[int.Parse(com[2])];
                    break;
                case ("ResultMinus"):
                    core = mainRule.Trigger[int.Parse(com[1])].MinusAction[int.Parse(com[2])].ResultCore[int.Parse(com[3])];
                    break;
                case ("Plus"):
                    core = mainRule.Trigger[int.Parse(com[1])].PlusAction[int.Parse(com[2])].Core[int.Parse(com[2])];
                    break;
                case ("Minus"):
                    core = mainRule.Trigger[int.Parse(com[1])].MinusAction[int.Parse(com[2])].Core[int.Parse(com[3])];
                    break;
                case ("Action"):
                    core = mainRule.Trigger[int.Parse(com[1])].Action[int.Parse(com[2])].Core[int.Parse(com[3])];
                    break;
            }

            return core;
        }

        static void SetSubMood(string str)
        {
            subMood = str;
            TextRule("HeadInfo");
        }
        static void EditRule(string str)
        {
            int a, b,c;
            string[] com = str.Split('_');
            if("RuleList" == com[0])
            {
                switch (com[1])
                {
                    case ("Return"):
                        ReturnRuleList(com[2], com[3]));
                        break;
                    case ("Set"):
                        RuleForm core = null;

                        a = int.Parse(com[2]);
                        com = com[3].Split('*');
                        switch (com[0])
                        {
                            case ("NeedRuleHead"):
                                b = mainRule.NeedRule.FindIndex(x => x.Head == a);
                                if (b != -1)
                                    return;
                                mainRule.NeedRule.Add(new SubIntLite(a));
                                break;
                            case ("NeedRule"):
                                b = int.Parse(com[1]);
                                mainRule.NeedRule[b].Num = AddListText(mainRule.NeedRule[b].Num, a, true);
                                break;
                            case ("EnemyRuleHead"):
                                b = mainRule.EnemyRule.FindIndex(x => x.Head == a);
                                if (b != -1)
                                    return;
                                mainRule.EnemyRule.Add(new SubIntLite(a));
                                break;
                            case ("EnemyRule"):
                                b = int.Parse(com[1]);
                                mainRule.EnemyRule[b].Num = AddListText(mainRule.EnemyRule[b].Num, a, true);
                                break;

                            case ("TaypId"):
                                core = ReturnCoreOrig(com[1]);
                                core.TaypId = a;
                                break;
                            case ("Tayp"):
                                com = com[1].Split('?'); 
                                int d = int.Parse(com[1]), f = int.Parse(com[2])
                                if (com[0] == "Result")
                                {
                                    core = mainRule.Trigger[d].Action[f].ResultCore;
                                    core.Tayp = a;
                                    TextRule("HeadInfo");
                                    return;
                                }

                                int e = int.Parse(com[3]);
                                bool plus = (com[0] == "Plus");
                                core = (plus) ? mainRule.Trigger[d].PlusAction[f].Core[e]: mainRule.Trigger[d].MinusAction[f].Core[e];
                               
                                if (a == keyStat && core.Tayp != a)
                                {
                                    if (plus)
                                        mainRule.Trigger[d].PlusAction[f].ResultCore.Add(new RuleForm(keyStat));
                                    else
                                        mainRule.Trigger[d].MinusAction[f].ResultCore.Add(new RuleForm(keyStat));
                                }
                                else if (a != keyStat && core.Tayp == keyStat)
                                {
                                    int h = 0;
                                    for (int i = 0; i < e; i++)
                                        if (plus)
                                        {
                                            if (mainRule.Trigger[d].PlusAction[f].Core[i].Tayp == keyStat)
                                                h++;
                                        }
                                        else if (mainRule.Trigger[d].MinusAction[f].Core[i].Tayp == keyStat)
                                            h++;

                                    if (plus)
                                        mainRule.Trigger[d].PlusAction[f].ResultCore.RemoveAt(h);
                                    else
                                        mainRule.Trigger[d].MinusAction[f].ResultCore.RemoveAt(h);
                                }
                                
                                core.Tayp = a;
                                break;
                            case ("Card"):
                                core = ReturnCoreOrig(com[1]);
                                core.Card = a;
                                break;
                            case ("Trigger"):
                                mainRule.Trigger.Add(new TriggerAction(a));
                                break;
                        }
                        break;
                    case ("Remove"):
                        a = int.Parse(com[3]);
                        switch (com[2])
                        {
                            case ("NeedRuleHead"):
                                mainBase.NeedRule.RemoveAt(a);
                                break;
                            case ("NeedRule"):
                                b = int.Parse(com[4]);
                                mainBase.NeedRule[b].Num.RemoveAt(a);
                                break;
                            case ("EnemyRuleHead"):
                                mainBase.EnemyRule.RemoveAt(a);
                                break;
                            case ("EnemyRule"):
                                b = int.Parse(com[4]);
                                mainBase.EnemyRule[b].Num.RemoveAt(a);
                                break;
                            case ("Trigger"):
                                mainRule.Trigger.RemoveAt(a);
                                break;
                        }

                        break;
                }
            }



            a = int.Parse(com[2]);
            switch (com[0])
            {
                case ("Add"):
                    switch (com[1])
                    {
                        case ("Plus"):
                            mainRule.Trigger[a].PlusAction.Add(new IfAction());
                            break;
                        case ("Minus"):
                            mainRule.Trigger[a].MinusAction.Add(new IfAction());
                            break;
                        case ("Action"):
                            mainRule.Trigger[a].Action.Add(new RuleAction());
                            break;
                    }
                    break;
                case ("Remove"):
                    b = int.Parse(com[3]);
                    switch (com[1])
                    {
                        case ("Plus"):
                            mainRule.Trigger[a].PlusAction.RemoveAt(b);
                            break;
                        case ("Minus"):
                            mainRule.Trigger[a].MinusAction.RemoveAt(b);
                            break;
                        case ("Action"):
                            mainRule.Trigger[a].Action.RemoveAt(b);
                            break;
                    }
                    break;
            }
            TextRule("HeadInfo");

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
                    str += AddLink("SetSwitch|Antipod_-1", "Null") + "\n";
                    for (int i = 0; i < core.bD[keyStat].Base.Count; i++)
                        if(keyB != i)
                            str += AddLink($"SetSwitch|Antipod_{i}", core.bD[keyStat].Base[i].Name) + "\n";
                    
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
                    string key = $"{keyA}-{com[1]}";
                    a = int.Parse(com[1]);
                    if (a == -1)
                        if (mainBase.Sub.Antipod == -1)
                            return;

                    AddEdit(key);
                    mainBase1 = ReturnMainBase(key);
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
                    nameTT.text = core.head[keyA].Rule[keyB];
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
        static int IfPlus(int a,int size, bool mood)
        {

            if (mood)
            {
                if (a > 0)
                    return a - 1;
            }
            else if (a < size-1)
                return a + 1;

            return a;
        }

        static RuleForm EditCore(RuleForm form, string text, bool mood)
        {
            switch (text) 
            {
                case ("Mod"):
                    if (mood)
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
                    if (mood)
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
            bool mood = (com[1] == "-");

            com = com[0].Split('_');
            if (com.Length > 1)
            {
                a = int.Parse(com[0]);
                b = int.Parse(com[1]);
            }
            switch (com[0])
            {
                case ("Cost"):
                    if (mood)
                        mainRule.Cost--;
                    else
                        mainRule.Cost++;
                    break;

                case ("Team"):
                    if (mood)
                        mainRule.Trigger[a].Team--;
                    else
                        mainRule.Trigger[a].Team++;
                    //mainRule.Trigger[a].Team = IfPlus(mainRule.Trigger[a].Team, core.frame.PlayerString.Length, mood);
                    break;

                case ("ActionPlus"):
                    if (mood)
                        mainRule.Trigger[a].PlusAction[b].Point--;
                    else
                        mainRule.Trigger[a].PlusAction[b].Point++;
                    break;
                case ("ActionMinus"):
                    if (mood)
                        mainRule.Trigger[a].MinusAction[b].Point--;
                    else
                        mainRule.Trigger[a].MinusAction[b].Point++;
                    break;
                case ("ActionMin"):
                    if (mood)
                        mainRule.Trigger[a].Action[b].Min--;
                    else
                        mainRule.Trigger[a].Action[b].Min++;

                    if (mainRule.Trigger[a].Action[b].Min > mainRule.Trigger[a].Action[b].Max)
                        mainRule.Trigger[a].Action[b].Max = mainRule.Trigger[a].Action[b].Min;
                    break;
                case ("ActionMax"):
                    if (com[3])
                        mainRule.Trigger[a].Action[b].Max--;
                    else
                        mainRule.Trigger[a].Action[b].Max++;

                    if (mainRule.Trigger[a].Action[b].Min < mainRule.Trigger[a].Action[b].Max)
                        mainRule.Trigger[a].Action[b].Min = mainRule.Trigger[a].Action[b].Max;
                    break;


                case ("ActionPlusResult"):
                    c = int.Parse(com[2]);
                    mainRule.Trigger[a].PlusAction[b].Result[c] = IfPlus(mainRule.Trigger[a].PlusAction[b].Result[c], core.frame.EqualString.Length, mood);
                    break;
                case ("ActionMinusResult"):
                    c = int.Parse(com[2]);
                    mainRule.Trigger[a].MinusAction[b].Result[c] = IfPlus(mainRule.Trigger[a].MinusAction[b].Result[c], core.frame.EqualString.Length, mood);
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

                    form = EditCore(form, com[4], mood);

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
        static void EditMove(string str)
        {

            int a = 0, b = 0, c = 0;
            string[] com = str.Split('*');
            bool mood = (com[1] == "-");
            com = str.Split('_');

            a = int.Parse(com[1]);
            b = int.Parse(com[2]);
            if (com.Length == 4)
            {
                c = int.Parse(com[3]);
                //RuleForm core = null;
                switch (com[0])
                {
                    case ("Plus"):
                        if (mood)
                            mainRule.Trigger[a].PlusAction[b].Core.Insert(c - 1, mainRule.Trigger[a].PlusAction[b].Core[c]);
                        else
                            mainRule.Trigger[a].PlusAction[b].Core.Insert(c + 1, mainRule.Trigger[a].PlusAction[b].Core[c]);
                        break;
                    case ("Minus"):
                        if (mood)
                            mainRule.Trigger[a].MinusAction[b].Core.Insert(c - 1, mainRule.Trigger[a].MinusAction[b].Core[c]);
                        else
                            mainRule.Trigger[a].MinusAction[b].Core.Insert(c + 1, mainRule.Trigger[a].MinusAction[b].Core[c]);
                        break;
                    default:
                        if (mood)
                            mainRule.Trigger[a].Action[b].Core.Insert(c - 1, mainRule.Trigger[a].Action[b].Core[c]);
                        else
                            mainRule.Trigger[a].Action[b].Core.Insert(c + 1, mainRule.Trigger[a].Action[b].Core[c]);
                        break;
                }
            }
            else
                switch (com[0])
                {
                    case ("Plus"):
                        if (mood)
                            mainRule.Trigger[a].PlusAction.Insert(c - 1, mainRule.Trigger[a].PlusAction[b]);
                        else
                            mainRule.Trigger[a].PlusAction.Insert(c + 1, mainRule.Trigger[a].PlusAction[b]);
                        break;
                    case ("Minus"):
                        if (mood)
                            mainRule.Trigger[a].MinusAction.Insert(c - 1, mainRule.Trigger[a].MinusAction[b]);
                        else
                            mainRule.Trigger[a].MinusAction.Insert(c + 1, mainRule.Trigger[a].MinusAction[b]);
                        break;
                    default:
                        if (mood)
                            mainRule.Trigger[a].Action.Insert(c - 1, mainRule.Trigger[a].Action[b]);
                        else
                            mainRule.Trigger[a].Action.Insert(c + 1, mainRule.Trigger[a].Action[b]);
                        break;
                }

        }

        static string TextEditInt(string path, string cost)  {  return AddLink($"Int|{path}*-", "<<") + $" ({cost}) " + AddLink($"Int|{path}*+", ">>");  }

        static string TextMove(string path, int a, int size) 
        { 
            string str = "";
            if(a >0)
                str += AddLink($"Move|{path}*-", "/\\/\\");
            
            if (a < size)
                str += AddLink($"Move|{path}*+", "\/\/");

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
                        str += AddLink("Switch|Antipod", (mainBase.Sub.Antipod == -1) ? "Antipod: Null" : "Antipod: "+ core.bD[keyStat].Base[mainBase.Sub.Antipod].Name) + "\n";

                        str += "\n������ AntiStat ��� �������";
                        str += WebText(mainBase.Sub.AntiStat, "AntiStat"); 

                        str += "\n������ DefStat ��� �������";
                        str += WebText(mainBase.Sub.DefStat, "DefStat");

                    }
                    for (int i = 0; i < mainBase.Text.Count; i++)
                    {
                        str += $"\n������ {core.bD[keyA].Key[i]} ��� �������";
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
            int a, b,c;
            string[] com = str.Split('_');

            switch (com[0])
            {
                case ("MenuHead"):
                    str = "";
                    for (int i = 0; i < core.head.Count; i++)
                        str += AddLink($"Open|Menu_{i}", core.head[i].Name + IfLook(core.bD[keyTag].Base[i].Look) + $"({core.head[i].Index.Count})") + "\n";

                    TT[0].text = str;
                    break;

                case ("Menu"):
                    a = int.Parse(com[1]);
                    str = AddLink("Open|MenuHead", "Back") + "\n";
                    str += AddLink($"Sys|New_{a}", "New Rule " + core.head[a].Name);
                    str += "\n";

                    for (int i = 0; i < core.head[a].Index.Count; i++)
                    {
                        str += "\n  " + AddLink($"Key|{a}-{i}", $"({core.head[a].Index[i]})"+ core.head[a].Rule[i]);
                    }

                    TT[0].text = str;
                    break;


                case ("HeadInfo"):
                    str = AddLink($"Sys|Save", "Save");
                    str += "    ";
                    str += AddLink($"Sys|Load", "Load");
                    str += "    ";
                    str += AddLink($"Sys|Clear", "Clear");
                    str += "    ";
                    str += AddLink($"Sys|Del", "Delite");
                    str += "\n\n";
                    str += AddLink("GetIO|RuleName", $"({core.head[keyA].Index[keyB]})������� -- { core.head[keyA].Rule[keyB]}");
                    // str += AddLink("Edit_Cost", $"\n����: { mainBase.Cost}");
                    str += $"\n����: " + TextEditInt("Cost", "" + mainRule.Cost);
                    str += $"\nTag {core.bD[keyTag].Base[mainRule.Tag].Name}";
                    // str += AddLink("Tag_Tag", $"\nTag { mainBase.Tag}");


                    str += "\n��������� ��������";
                    str += AddLink("Edit|RuleList_Return_NeedRuleHead", $"\n������� ���������");
                    for (int i = 0; i < mainRule.NeedRule.Count; i++)
                    {
                        str += AddLink("Edit|RuleList_Remove_NeedRuleHead", $"\n������� ��������� {core.head[mainRule.NeedRule[i].Head].Name}");
                        str += AddLink($"Edit|RuleList_Return_NeedRule_{i}", $"\n������� ����� � {core.head[mainRule.NeedRule[i].Head].Name}");
                        for (int i1 = 0; i1 < mainRule.NeedRule[i].Num.Count; i1++)
                            str += AddLink($"Edit|RuleList_Remove_NeedRule_{i}_{i1}", $"\n    ��������� ����� � {core.head[mainRule.NeedRule[i].Head].Rule[mainRule.NeedRule[i].Num[i1]]}");

                    }
                    str += "\n";

                    str += "\n����������� ��������";
                    str += AddLink("Edit|RuleList_Return_EnemyRuleHead", $"\n������� ���������");
                    for (int i = 0; i < mainRule.EnemyRule.Count; i++)
                    {
                        str += AddLink("Edit|RuleList_Remove_EnemyRuleHead", $"\n������� ��������� {core.head[mainRule.EnemyRule[i].Head].Name}");
                        str += AddLink($"Edit|RuleList_Return_EnemyRule_{i}", $"\n������� ����� � {core.head[mainRule.EnemyRule[i].Head].Name}");
                        for (int i1 = 0; i1 < mainRule.EnemyRule[i].Num.Count; i1++)
                            str += AddLink($"Edit|RuleList_Remove_EnemyRule_{i}_{i1}", $"\n    ��������� ����� � {core.head[mainRule.EnemyRule[i].Head].Rule[mainRule.EnemyRule[i].Num[i1]]}");

                    }
                    str += "\n";


                    str += "\n\nTriggers\n";
                    for (int i = 0; i < mainRule.Trigger.Count; i++)
                        str += AddLink($"SubMood|Trigger_{i}", $"\n    {NameTrigger(i)}") + "   " + AddLink($"Edit|RuleList_Remove_Trigger_{i}", "-Remove");
                    str += "\n";
                    str += AddLink($"Edit|RuleList_Return_Trigger", "-Add");
                    TT[1].text = str;
                    break;
                case ("Info"):
                    a = subMood;
                    //b = mainRule.Trigger[a].Trigger;
                    str = AddLink($"SetMood|HeadInfo", $"Back");
                    //if (core.frame.Trigger[b].Extend.Length > 1)
                    //    str += AddLink($"Edit|TriggerExtend_Open_{a}", $"����� -- " + core.frame.Trigger[b].Extend[mainRule.Trigger[a].TriggerExtend]); 

                    str += AddLink($"SetSwitch|CountMod_{a}", $"������� � ��������� -- " + ((mainRule.Trigger[a].CountMod) ? "Yes" : "No"));
                    str += AddLink($"SetSwitch|CountModExtend_{a}", $"������� � �������� -- " + ((mainRule.Trigger[a].CountModExtend) ? "Yes" : "No"));


                    str += $"\n ������� ���� " + AddLink($"Edit|RuleList_Return_Plan_{a}", $"������� ���� " + ((mainRule.Trigger[a].Plan > -1) ? core.bD[keyPlan].Base[mainRule.Trigger[a].Plan].Name : "AllPlan"));

                    if (mainRule.Trigger[a].Team > 0)
                        str += AddLink($"Int|Team*-", "<<");

                    str += $" ({core.frame.PlayerString[mainRule.Trigger[a].Team]}) ";

                    if (mainRule.Trigger[a].Team < core.frame.EqualString.Length)
                        str += AddLink($"Int|Team*+", ">>");

                    //str += $"\n ������� ����� " + TextEditInt($"Team", core.frame.PlayerString[mainRule.Trigger[a].Team]);


                    str += "\n ������������� �������";
                    str += AddLink($"Edit|Add_Plus_{a}", $"\n������� �������");
                    for (int i = 0; i < mainRule.Trigger[a].PlusAction.Count; i++)
                    {
                        str += AddLink($"Edit|Remove_Plus_{a}", $"\n������� �������");
                        str += "\n";
                        str += IfActionCase(a, i, "Plus");
                    }


                    str += "\n����������� �������";
                    str += AddLink($"Edit|Add_Minus_{a}", $"\n������� ����������");
                    for (int i = 0; i < mainRule.Trigger[a].MinusAction.Count; i++)
                    {
                        str += AddLink($"Edit|Remove_Minus_{a}", $"\n������� ����������");
                        str += "\n";
                        str += IfActionCase(a, i, "Minus");
                    }



                    str += "\n��������";
                    str += AddLink($"Edit|Add_Minus_{a}", $"\n������� ��������");
                    for (int i = 0; i < mainRule.Trigger[a].Action.Count; i++)
                    {
                        str += AddLink($"Edit|Remove_Action_{a}", $"\n������� ��������");
                        str += "\n";
                        str += TextMove($"Plus_{a}_{i}", i, mainRule.Trigger[a].Action.Count);
                        str += "\n";
                        str += ActionRule(a, i);
                    }
                    //str += AddLink($"Edit|TargetPalyer_{a}", $"\n ������� ����� " + core.frame.PlayerString[mainRule.Trigger[a].Team]);


                    //ReturnCoreInfo    ReturnCore

                    //str += "\n ������������� �������";
                    //str += AddLink($"Edit|Action_Plus_{a}", $"\n������� �������");
                    //for (int i = 0; i < mainRule.Trigger[a].PlusAction.Count; i++)
                    //    str += AddLink($"Edit|NeedRule_Edit_{i}", $"\n{ReturnCore(mainRule.Trigger[a].PlusAction[i])}";
                    //str += AddLink($"Edit|NeedRule_Add", $"\n{ReturnCore(mainRule.Trigger[a].PlusAction[i])}";



                    //str += AddLink($"Edit|NeedRule_Edit_{i}", $"\n    ��������� ����� � {ReturnCoreInfo(mainRule.Trigger[a].PlusAction[i])}");
                    //str += AddLink($"Edit|NeedRule_Remove_{i}", $"\n    ��������� ����� � {ReturnRuleHead(mainRule.NeedRule[i])}");

                    //str += "\n����������� �������";
                    //str += AddLink("Edit|EnemyRule_Add", $"\n������� �����");
                    //for (int i = 0; i < mainRule.EnemyRule.Count; i++)
                    //    str += AddLink($"Edit|EnemyRule_Redact_{i}", $"\n    ��������� ����� � {ReturnMainRule(mainRule.EnemyRule[i]).Name}");

                    //str += "\n��������";
                    //for (int i = 0; i < mainRule.Trigger.Count; i++)
                    //    str += AddLink($"SubMood|Trigger_{i}", $"\n    {NameTrigger(i)}") + "   " + AddLink($"Edit|Trigger_Remove_{i}", "-Remove");
                    //str += AddLink($"Edit|Trigger_Add", "-Add");
                    TT[1].text = str;
                    break;
                    /*
                     ��������� �������� �� �������, ��� �������� ��� ������� ������ �� ������, ��� ��������� ��������� ������

                     */



            }
        }
        static string IfActionCase(int a, int b, string plus)
        {
            int c = 0;
            IfAction ifAction = (plus == "Plus") ? mainRule.Trigger[a].PlusAction[b] : mainRule.Trigger[a].MinusAction[b];
            string str = "",key = plus+$"?{a}?{b}?";
            str += $"\n({b}) Point";
            str += TextEditInt($"Action{plus}_{a}_{b}", "" + ifAction.Point);
            str += " ";
            str += TextMove($"{plus}_{a}_{b}",b, (plus == "Plus") ? mainRule.Trigger[a].PlusAction.Count : mainRule.Trigger[a].MinusAction.Count);
            for (int i = 0; i < ifAction.Core.Count; i++)
            {
                str += "\n";
                str += TextMove($"{plus}_{a}_{b}_{i}", i, (plus == "Plus") ? mainRule.Trigger[a].PlusAction.Count : mainRule.Trigger[a].MinusAction.Count);

                bool use = (ifAction.Core[i].Tayp == keyStat);
                str += "\n";

                str += ReturnCore(ifAction.Core[i],key+i);

                str += " ";
                if (ifAction.Result[i] > 0)
                    str += AddLink($"Int|Action{plus}Result_{a}_{b}*-", "<<");

                str += $" ({core.frame.EqualString[ifAction.Result[i]]}) ";
                if (use)
                {
                    if (ifAction.Result[i] < core.frame.EqualString.Length)
                        str += AddLink($"Int|Action{plus}Result_{a}_{b}*+", ">>");
                }
                else
                    if (ifAction.Result[i] < 1)
                    str += AddLink($"Int|Action{plus}Result_{a}_{b}*+", ">>");


                str += " ";

                if (use)
                {
                    str += ReturnCore(ifAction.ResultCore[i], "Result"+key + c, true);
                    c++;
                }

                str += AddLink($"Edit|Remove_Core_{plus}_{a}_{b}_{i}", "-- Remove");
            }
            str += AddLink($"Edit|Add_Core_{plus}_{a}_{b}", "-- Add");
            return str;
        }
        static string ActionRule(int a, int b)
        {
            RuleAction action = mainRule.Trigger[a].Action[b];
            string key = $"Action?{a}?{b}?";
            string str = "";
            str += $"\n({b}) Point";
            str += TextEditInt($"ActionMin_{a}_{b}", "" + action.Min);
            str += "    ";
            str += TextEditInt($"ActionMax_{a}_{b}", "" + action.Max);

            // str += AddLink($"Edit|Remove_Core_{plus}_{a}_{b}_{i}", "-- Remove");

            str += "\nTeam ";

            if (action.Team > 0)
                str += AddLink($"Int|ActionTeam*-", "<<");

            str += $" ({core.frame.PlayerString[action.Team]}) ";

            if (action.Team < core.frame.EqualString.Length)
                str += AddLink($"Int|ActionTeam*+", ">>");

            str += "\n";

            str += "\nAction ";
            str += AddLink($"Edit|RuleList_Return_Action_{a}_{b}", core.frame.Action[action.Action].Name);
            if (core.frame.Action[a].Extend.Count > 1)
            {
                str += "\nActionExtend ";
                str += AddLink($"Edit|RuleList_Return_Action_{a}_{b}", core.frame.Action[action.Action].Extend[action.ActionExtend]);
            }
            //�������������� ����� ���� � ����� ��������� ����� �����
            // str +=  Action;
            //  str += ActionExtend;

            // for (int i = 0; i < (action.Core.Count / 2); i++)
            // {
            //     str += "\n";

            //     str += ReturnCore(action.Core[i * 2],key +i, true);

            //     str += " / ";

            //     str += ReturnCore(action.Core[i * 2 + 1], key + (i+1), true);

            //     //str += AddLink($"Edit|Remove_Core_Action_{a}_{b}_{i}", "-- Remove");
            // }
            // //str += AddLink($"Edit|Add_Core_Action_{a}_{b}", "-- Add");

            // str += "\n";
            //// Debug.Log("!");//��� ����� �������� ������� � ���������� 
            // str += ReturnCore(action.ResultCore, key, true);//������������ �� ���� � �������

            return str;
        }

        static string NameTrigger(int i)
        {
            string str = "Trigger";
            if(mainRule.Trigger[i].Plan != -1)
                str += "    " + core.bD[keyPlan].Base[ mainRule.Trigger[i].Plan].Name ;
            else
                str += "    AllPlan" ;
            str += "    " + core.frame.Trigger[mainRule.Trigger[i].Trigger] ;
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
        static void ReturnRuleList(string text, string path)
        {
            string key = $"Edit|RuleList_Set_";//{text}_+path
            string str =  AddLink("ClearIO", "Back") ;
            path = text + "*" + path;
            if(text == "NeedRuleHead" || text == "EnemyRuleHead")
            {
                for (int i = 0; i < core.head.Count; i++)
                    if (core.head[i].Index.Count > 0)
                        str += $"\n" + AddLink($"{key}{i}_{text}", $"Set { core.head[i].Name}");
            }
            else if (text == "NeedRule" || text == "EnemyRule")
            {
                string[] com = path.Split('|');
                int a = int.Parse(com[0]);
                int b = 0;
                List<int> numbs = new List<int>();


                for (int i = 0; i < core.head[a].Index.Count; i++)
                    numbs.Add(core.head[a].Index[i]);

                b = mainRule.EnemyRule.FindIndex(x => x.Head == a);
                if (b == -1)
                    for (int i = 0; i < mainRule.EnemyRule[b].Num.Count; i++)
                        numbs.Remove(mainRule.EnemyRule[b].Num[i]);


                b = mainRule.NeedRule.FindIndex(x => x.Head == a);
                if (b == -1)
                    for (int i = 0; i < mainRule.NeedRule[b].Num.Count; i++)
                        numbs.Remove(mainRule.NeedRule[b].Num[i]);



                for (int i = 0; i < numbs.Count; i++)
                        str += $"\n" + AddLink($"{key}{i}_{text}*{a}", $"Set { core.head[a].Rule[numbs[i]}");

            }
            else
                switch (text)
                {
                    case ("Trigger"):
                        for (int i = 0; i < core.frame.Trigger.Length; i++)
                            str += $"\n" + AddLink($"{key}{i}", $"Add {core.frame.Trigger[i]}");// " Add|-1 null";
                        break;

                    case ("Tayp"):
                        str += $"\n" + AddLink($"{key}-1_{path}", $"Set Null");
                        for (int i = 0; i < core.bD.Count; i++)//if(i != key(������ ��������) ��� ���������� ������������ ������ �������
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.bD[i].Name}");
                        break;
                    case ("TaypId"):
                        string[] com = path.Split('|');
                        int a = int.Parse(com[0]);
                        if (a == keyStat)
                            for (int i = 0; i < core.frame.SysStat.Length; i++)
                                str += $"\n" + AddLink($"{key}-{i}_{com[1]}", $"Set {core.frame.SysStat[i]}");

                        for (int i = 0; i < core.bD[a].Base.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{com[1]}", $"Set { core.bD[a].Base[i].Name}");
                        break;
                    case ("Card"):
                        for (int i = 0; i < core.frame.CardString.Length; i++)
                            str += $"\n" + AddLink($"{key}-{i + 1}_{path}", $"Set { core.frame.CardString[i]}");

                        //core.frame.CardString[-(coreForm.Card + 1)]
                        for (int i = 0; i < core.bD[keyAssociation].Base.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.bD[keyAssociation].Base[i].Name}");

                        break;
                    //case ("TaregtPlayer"):
                    //    for (int i = 0; i < core.frame.PlayerString.Length; i++)
                    //        str += $"\n" + AddLink($"Edit|TaregtPlayer_{i}_{path}", $"Set { core.frame.PlayerString[i]}");
                    //    break;
                    case ("Forse"):
                        for (int i = 0; i < core.frame.ForseTayp.Length; i++)
                            str += $"\n" + AddLink($"{key}_{i}_{path}", $"Set { core.frame.ForseTayp[i]}");
                        break;
                    case ("Action"):
                        //str += $"\n Add|-1 null";
                        
                        for (int i = 0; i < core.frame.Action.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.frame.Action[i].Name}");
                        break;
                    case ("ActionExtend"):
                        com = path.Split('|');
                        a = int.Parse(com[0]);
                        //str += $"\n Add|-1 null";
                        
                        for (int i = 0; i < core.frame.Action[a].Extend.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.frame.Action[a].Extend[i].Count}");
                        break;
                    case ("Plan"):
                        //ForseTayp
                        str += $"\n Add|-1 AllPlans";
                        for (int i = 0; i < core.bD[keyPlan].Base.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.bD[keyPlan].Base[i].Name}");
                        break;
                }
            TT[0].text =str;
        }

        static string ReturnCore(RuleForm coreForm , string path, bool result = false)
        {

            string str = "", text ="";
            if (coreForm.Card < 0)
                text = $"\n{core.frame.CardString[-(coreForm.Card +1)]}";
            else
                text = $"\n{core.bD[keyAssociation].Base[coreForm.Card].Name}";

            
            str += AddLink($"Edit|RuleList_Return_Card_{path}" , text);

            if (!result)
            {
                if (coreForm.Tayp != -1)
                    text = $" {core.bD[coreForm.Tayp].Name}";
                else
                    text = $" Null";

                str += $" " + AddLink($"Edit|RuleList_Return_Tayp_{path}", text);
            }

            if (coreForm.Tayp > -1)
            {
                if (coreForm.Tayp == keyStat && coreForm.TaypId < 0)
                    text = core.frame.SysStat[-coreForm.TaypId];
                else
                    text = $" {core.bD[coreForm.Tayp].Base[coreForm.TaypId].Name}";


                str += $" " + AddLink($"Edit|RuleList_Return_TaypId_{coreForm.Tayp}|{path}", text);

                str += TextEditInt("Mod_" + path, "" + coreForm.Mod);
                str += TextEditInt("Num_" + path, "" + coreForm.Num);
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
            //mainRule.Name = "Void";

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
     ����������� �������� 
    ����
    ��������(���� ����������)
    ���� ����
    ����� ������� ����� ����������
     
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
       // public string Name;//��������
       // public string Info = "Void";//��������
        public int Tag; //��������

        public int Cost;//����

        public List<TriggerAction> Trigger;// = new List<TriggerAction>();

        public List<SubIntLite> NeedRule;// = new List<string>();
        public List<SubIntLite> EnemyRule;//= new List<string>();
    }

    public class TriggerAction
    {
        TriggerAction(int a)
        {
            Trigger = a;
        }

        public int Plan =-1;
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

        public List<RuleForm> Core = new List<RuleForm>();
        public List<RuleForm> ResultCore = new List<RuleForm>();
        public List<int> Result = new List<int>();
    }

    public class RuleForm
    {
        RuleForm(int a =-1 )
        {
            Tayp == a;
        }

        public int Card = -1;//��� ���������� �������� ������������ ������������ ��������, � ��������� ������ ������������ ��������� "Null";//0-null 1-card1 2-card2
        public int Tayp = -1; 
        public int TaypId = 0;
        public int Mod = 1;
        public int Num = 0;
    }

    public class RuleAction
    {
        public string Name = "Action";
        public int Action;
        public int ActionExtend;

        public int Min;
        public int Max;

        public int Team;

        //public int RuleTag;//�������� ����� �������� 
        //public int Rule;
        public List<RuleForm> Core = new List<RuleForm>();
        public RuleForm ResultCore = new RuleForm();

        public int ForseMood;

    }

    public class SubIntLite
    {
        public int Head;
        public List<int> Num = new List<int>();
        SubIntLite(int a)
        {
            Head = a;
        }
    }

    #endregion
}
