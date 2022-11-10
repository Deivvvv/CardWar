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
     //   static List<string> edit;// = new List<string>();
        static int keyA =0, keyB =-1;
        //static string mood;
        static int subMood = -1;
        static MainBase mainBase;
        static HeadRule mainRule;
        static CardCase mainCard;
        static CoreSys core;
        public static void SetCore(CoreSys coreSys) {if(coreSys != null)core = coreSys; }
        public static CoreSys GetCore()  { return core; }

        static RedactorUi ui;
        public static void SetTT(RedactorUi _ui)  { ui = _ui; }

        static int KeyAConverter()
        {
            if (keyA < 0)
                return -keyA - 1;
            return keyA;
        }

        static void SetKey(string str)
        {
            // if (keyA < 0)
            //     TextRule("MenuHead");
            //else
            //    TextBD("MenuHead");

            if ( keyB !=-1)
                if(keyA <0)
                    Saver.SaveRule(mainRule, KeyAConverter(), keyB);
                else
                    Saver.SaveBD(keyA, keyB);
            
            string[] com = str.Split('.');
            keyA = int.Parse(com[0]);
            keyB = int.Parse(com[1]);

            if (keyA < 0)
            {
                mainRule = Saver.LoadRule(KeyAConverter(), core.head[KeyAConverter()].Index[keyB]);
                //TextRule("MenuHead");
                TextRule($"Menu_{keyA}");
                SetSubMood(-1);
            }
            else
            {
                mainBase = core.bD[keyA].Base[keyB];
                TextBD("Info");
                //TextBD("MenuHead");
                TextBD($"Menu_{keyA}");
            }
        }

        public static int ReturnIndex(string str) { return core.frame.Tayp.FindIndex(x => x == str); }

        #endregion

        static void SwitchBD()
        {
            if (keyA > -1)
            {
                Saver.SaveBD(keyA, keyB);
                keyA = -1;
                TextRule("MenuHead");
            }
            else
            {
                Saver.SaveRule(mainRule, KeyAConverter(), keyB);
                keyA = 0;
                TextBD("MenuHead");
            }
            keyB = -1;
            ui.TT[1].text = "";
        }

        #region Sort
        static HideLibrary hide;
        /*
        public static List<int> SortList(List<int> index, int mood)
        {
            bool use;
            List<int> newIndex = new List<int>();
            CardCase baseCard = null;
            for (int i = 0; i < index.Count; i++)
            {
                use = true;
                baseCard = Saver.Load(index[i]);
                switch (mood)
                {
                    case (0):
                        for (int i1 = 0; i1 < 4 && use; i1++)
                            switch (i1)
                            {
                                case (0):
                                    use = hide[core.keyGuild].Select[baseCard.Guild];
                                    break;
                                case (1):
                                    use = hide[core.keyLegion].Select[baseCard.Legion];
                                    break;
                                case (2):
                                    use = hide[core.keySocial].Select[baseCard.CivilianGroups];
                                    break;
                                case (3):
                                    use = hide[core.keyRace].Select[baseCard.Races];
                                    break;
                            }
                        break;
                    case (1):
                        for (int i2 = 0; i2 < baseCard.Trait.Count && use; i2++)
                            use = hide[keyTag].Select[baseCard.Trait[i2].Head];
                        break;
                    case (2):
                        for (int i2 = 0; i2 < baseCard.Stat.Count && use; i2++)
                            use = hide[keyStat].Select[baseCard.Stat[i2][0]];
                        break;
                }
              


                if (use)
                    newIndex.Add(index[i]);
            }

            return newIndex;
        }

        public static string ReadSort(List<int> index)
        {
            //Action<bool> vis = add =>
            //{
            //    string str1="";
            //    List<int> useList = (sortIndex.Count > 0) ? sortIndex : listIndex;

            //    num = NewPage(add, num, bodys.Count, useList.Count);

            //    lastIndex = new List<int>();
            //    for (int i = num * bodys.Count; i < useList.Count; i++)
            //    {
            //        listIndex.Add(useList[i]);
            //    }
            //    return str1;
            //};
            List<int> indexLegion = new List<int>();
            List<int> indexCivilian = new List<int>();
            List<int> indexCivilian = new List<int>();

            string str = "";
            str += $"{core.bd[keyGuild].Name} - ({hide[keyGuild].Select.Count})";
            for (int i = 0; i < hide[keyGuild].Select.Count; i++)
            {
                str += "/n" + ((hide[keyGuild].Select[i]) ? "+" : "-") + core.bd[keyGuild].Base[i].Name;
            }


            for (int i = 0; i < hide[keyLegion].Select.Count; i++)
            {
                str += "/n" + ((hide[keyLegion].Select[i]) ? "+" : "-") + core.bd[keyLegion].Base[i].Name;
            }

            str += "\nSort card";
            return str;
        }
        */
        #endregion
        #region Coder
        public static void Read(string str)
        {
            Debug.Log(str);
            string[] com = str.Split('|');
            switch (com[0])
            {
                case ("Linker"):
                    com = com[1].Split('_');
                    Linker(com[0], int.Parse(com[1]), int.Parse(com[2]));
                    break;
                case ("LinkerMain"):
                    com = com[1].Split('_');
                    LinkerMain(com[0], int.Parse(com[1]));
                    break;
                case ("LinkerRead"):
                    com = com[1].Split('_');
                    LinkerRead(com[0], int.Parse(com[1]), int.Parse(com[2]), com[3] == "1");
                    break;

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
                    if (keyA < 0)
                        TextRule(com[1]);
                    else
                        TextBD(com[1]);
                    break;
                case ("Edit"):
                    if (keyA < 0)
                        EditRule(com[1]);
                    else
                        StatEdit(com[1]);
                    break;

                case ("Switch"):
                    Switch(com[1]);
                    break;
                case ("SetSwitch"):
                    SetSwitch(com[1]);
                    break;
                case ("SetMood"):
                    SetSubMood(int.Parse(com[1]));
                    break;
            }
        }
        static void StatEdit(string str)
        {
            string[] com = str.Split('_');
            bool add = (com[0] == "Add");
            int a = int.Parse(com[1]);

            switch (com[2])
            {
                case ("MainRace"):
                    if(mainBase.Race.MainRace != -1)
                        core.bD[core.keyRace].Base[mainBase.Race.MainRace].Race.UseRace.Add(keyB);
                    else
                        core.bD[core.keyRace].Base[a].Race.UseRace.Remove(keyB);
                    mainBase.Race.MainRace = a;
                    break;
                case ("MainStat"):
                    mainBase.Race.MainStat = a;
                    break;

                case ("Cost"):
                    if (add)
                        mainBase.Cost++;
                    else
                        mainBase.Cost--;
                    break;
                case ("DefStat"):
                    if (add)
                        mainBase.Sub.DefStat.Add(a);
                    else
                        mainBase.Sub.DefStat.RemoveAt(a);
                    break;
                case ("AntiStat"):
                    if (add)
                        mainBase.Sub.AntiStat.Add(a);
                    else
                        mainBase.Sub.AntiStat.RemoveAt(a);
                    break;
                case ("GroupStatMain"):
                    if (add)
                    {
                        mainBase.Group.MainSize++;
                        if(mainBase.Group.MainSize ==0)
                            mainBase.Group.MainSize++;

                    }
                    else
                    {
                        mainBase.Group.MainSize--;
                        if (mainBase.Group.MainSize == 0)
                            mainBase.Group.MainSize--;
                    }
                    
                    break;
                case ("GroupStat"):
                    if (add)
                    {
                        mainBase.Group.Stat.Add(a);
                        mainBase.Group.Size.Add(1);
                    }
                    else
                    {
                        mainBase.Group.Stat.RemoveAt(a);
                        mainBase.Group.Size.RemoveAt(a);
                    }
                    break;
                case ("GroupStatSize"):
                    if (add)
                    {
                        mainBase.Group.Size[a]++;
                        if (mainBase.Group.Size[a] == 0)
                            mainBase.Group.Size[a]++;

                    }
                    else
                    {
                        mainBase.Group.Size[a]--;
                        if (mainBase.Group.Size[a] == 0)
                            mainBase.Group.Size[a]--;
                    }
                    break;
            }
            ClearIO();
        }
        static void Sys(string str)
        {
            int a, b;

            string[] com = str.Split('_');
            if (keyA > -1)
                switch (com[0])
                {
                    case ("New"):
                        Saver.SaveBD(keyA, keyB);
                        keyA = int.Parse(com[1]);
                        keyB = core.bD[keyA].Base.Count;
                        core.bD[keyA].Base.Add(NewMainBase(keyA));
                        mainBase = core.bD[keyA].Base[keyB];
                        Saver.SaveBD(keyA, keyB);
                        if (keyA == core.keyTag)
                            Saver.RuleAdd();

                        GetIO("Base");
                        return;

                        break;
                    case ("Save"):
                        Saver.SaveBD(keyA, keyB);
                        break;
                    case ("AllReLoad"):
                        Saver.BackUpAllLoad("BD");
                        break;
                    case ("PreReLoad"):
                        MainBase localMainBase = core.bD[keyA].Base[keyB];
                        // for (int i = 0; i < edit.Count; i++)
                        //  {
                        //     com = edit[i].Split('-');
                        //     a = int.Parse(com[0]);
                        //     b = int.Parse(com[1]);

                        Saver.LoadBD(keyA, keyB);
                        // }
                        break;
                }
            else
            {
                int key = KeyAConverter();
                switch (com[0])
                {
                    case ("AllReLoad"):
                        Saver.BackUpAllLoad("Rule");
                        break;
                    case ("New"):
                        keyA = int.Parse(com[1]);
                       // Debug.Log(core.head.Count);
                      //  Debug.Log(core.head[keyA].Index.Count);

                        key = KeyAConverter();
                        keyB = core.head[key].Index.Count;
                        if (keyB == 0)
                            core.head[key].Index.Add(0);
                        else
                            core.head[key].Index.Add(core.head[key].Index[keyB - 1] + 1);
                        NewMainRule(key);
                        core.head[key].Rule.Add("Void");
                        core.head[key].Cost.Add(0);

                        Saver.SaveRuleMain(key);
                        Saver.SaveRule(mainRule, key, core.head[key].Index[keyB]);
                        if(keyA >-1)
                            keyA = -key - 1;
                        subMood = -1;

                        GetIO("RuleName");
                        return;

                        break;
                    case ("Save"):
                        if (keyB > -1)
                            Saver.SaveRule(mainRule, key, core.head[key].Index[keyB]);
                        break;
                    case ("Load"):
                        mainRule = Saver.LoadRule(key, core.head[key].Index[keyB]);
                        break;
                    case ("Clear"):
                        NewMainRule(mainRule.Tag);
                        break;
                    case ("Del"):
                        ui.TT[1].text = AddLink("ClearIO", "NO") + "      " + AddLink($"Sys|Delite", "YES");
                        return;
                        break;
                    case ("Delite"):
                        Saver.DeliteRule(key, core.head[key].Index[keyB]);
                        //core.head[keyA].Index.RemoveAt(keyB);
                        //core.head[keyA].Rule.RemoveAt(keyB);
                        if (core.head[key].Index.Count == 0)
                        {
                            TextRule($"Menu_{keyA}");
                            ui.TT[1].text = "";
                            return;
                        }
                        else
                            keyB = 0;
                        //mainRule = NewMainRule();
                        break;
                }
            }
            ClearIO();
        }

        static RuleForm ReturnCoreOrig(string str)
        {
            string[] com = str.Split('?');
            RuleForm core = null;
            Debug.Log(str);
            switch (com[0])
            {
                case ("ResultPlus"):
                    core = mainRule.Trigger[subMood].PlusAction[int.Parse(com[1])].ResultCore[int.Parse(com[2])];
                    break;
                case ("ResultMinus"):
                    core = mainRule.Trigger[subMood].MinusAction[int.Parse(com[1])].ResultCore[int.Parse(com[2])];
                    break;
                case ("Plus"):
                    core = mainRule.Trigger[subMood].PlusAction[int.Parse(com[1])].Core[int.Parse(com[2])];
                    break;
                case ("Minus"):
                    core = mainRule.Trigger[subMood].MinusAction[int.Parse(com[1])].Core[int.Parse(com[2])];
                    break;
                case ("ResultAction"):
                    core = mainRule.Trigger[subMood].Action[int.Parse(com[1])].ResultCore;
                    break;
                case ("Action"):
                    core = mainRule.Trigger[subMood].Action[int.Parse(com[1])].Core[int.Parse(com[2])];
                    break;
            }

            return core;
        }

        static void SetSubMood(int a)
        {
            subMood = a;
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
                        Debug.Log(str);
                        ReturnRuleList(com[2], com[3]);
                        break;
                    case ("Set"):
                        RuleForm coreForm = null;
                        Debug.Log(str);
                        a = int.Parse(com[2]);
                        com = com[3].Split('*');
                        switch (com[0])
                        {
                            case ("Forse"):
                                b = int.Parse(com[1]);
                                mainRule.Trigger[subMood].Action[b].ForseMood = a;
                                break;

                            case ("Trigger"):
                                mainRule.Trigger[subMood].Trigger = a;
                                break;
                            case ("Action"):
                                b = int.Parse(com[1]);
                                mainRule.Trigger[subMood].Action[b].Action = a;
                                mainRule.Trigger[subMood].Action[b].ActionExtend = 0;
                                NewForm(mainRule.Trigger[subMood].Action[b]);
                                break;

                            case ("ActionExtend"):
                                b = int.Parse(com[1]);
                                mainRule.Trigger[subMood].Action[b].ActionExtend = a;
                                NewForm(mainRule.Trigger[subMood].Action[b]);
                                break;

                            case ("Plan"):
                                mainRule.Trigger[subMood].Plan = a;
                                break;

                            case ("TaypId"):
                                Debug.Log($"!{a}");
                                coreForm = ReturnCoreOrig(com[1]);
                                coreForm.TaypId = a;
                                break;
                            case ("Tayp"):
                                com = com[1].Split('?');
                                int f = int.Parse(com[1]);
                                if (com[0] == "Result")
                                {
                                    coreForm = mainRule.Trigger[subMood].Action[f].ResultCore;
                                    coreForm.Tayp = a;
                                    TextRule("HeadInfo");
                                    return;
                                }

                                int e = int.Parse(com[2]);
                                bool plus = (com[0] == "Plus");
                                coreForm = (plus) ? mainRule.Trigger[subMood].PlusAction[f].Core[e]: mainRule.Trigger[subMood].MinusAction[f].Core[e];
                               
                                if (a == core.keyStat && coreForm.Tayp != a)
                                {
                                    if (plus)
                                        mainRule.Trigger[subMood].PlusAction[f].ResultCore.Add(new RuleForm(core.keyStat));
                                    else
                                        mainRule.Trigger[subMood].MinusAction[f].ResultCore.Add(new RuleForm(core.keyStat));
                                }
                                else if (a != core.keyStat && coreForm.Tayp == core.keyStat)
                                {
                                    int h = 0;
                                    for (int i = 0; i < e; i++)
                                        if (plus)
                                        {
                                            if (mainRule.Trigger[subMood].PlusAction[f].Core[i].Tayp == core.keyStat)
                                                h++;
                                        }
                                        else if (mainRule.Trigger[subMood].MinusAction[f].Core[i].Tayp == core.keyStat)
                                            h++;

                                    if (plus)
                                        mainRule.Trigger[subMood].PlusAction[f].ResultCore.RemoveAt(h);
                                    else
                                        mainRule.Trigger[subMood].MinusAction[f].ResultCore.RemoveAt(h);
                                }
                                
                                coreForm.Tayp = a;
                                break;
                            case ("Card"):
                                coreForm = ReturnCoreOrig(com[1]);
                                coreForm.Card = a;
                                break;
                        }

                        ClearIO();
                        break;
                    case ("Remove"):
                        a = int.Parse(com[3]);
                        mainRule.Trigger.RemoveAt(a);

                        TextRule("HeadInfo");
                        break;
                    case ("Add"):
                        mainRule.Trigger.Add(new TriggerAction());

                        TextRule("HeadInfo");
                        break;
                }
                return;
            }


            Debug.Log(str);
            if (com[1] == "Core")
            {
                a = int.Parse(com[3]); 
                switch (com[0])
                {
                    case ("Add"):
                        switch (com[2])
                        {
                            case ("Plus"):
                                mainRule.Trigger[subMood].PlusAction[a].Core.Add(new RuleForm());
                                mainRule.Trigger[subMood].PlusAction[a].Result.Add(0);
                                break;
                            case ("Minus"):
                                mainRule.Trigger[subMood].MinusAction[a].Core.Add(new RuleForm());
                                mainRule.Trigger[subMood].MinusAction[a].Result.Add(0);
                                break;
                        }
                        break;
                    case ("Remove"):
                        b = int.Parse(com[4]);
                        switch (com[2])
                        {
                            case ("Plus"):
                                mainRule.Trigger[subMood].PlusAction[a].Core.RemoveAt(b);
                                mainRule.Trigger[subMood].PlusAction[a].Result.RemoveAt(b);
                                break;
                            case ("Minus"):
                                mainRule.Trigger[subMood].MinusAction[a].Core.RemoveAt(b);
                                mainRule.Trigger[subMood].MinusAction[a].Result.RemoveAt(b);
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (com[0])
                {
                    case ("Add"):
                        switch (com[1])
                        {
                            case ("Plus"):
                                a = mainRule.Trigger[subMood].PlusAction.Count;
                                mainRule.Trigger[subMood].PlusAction.Add(new IfAction());
                                mainRule.Trigger[subMood].PlusAction[a].Core.Add(new RuleForm());
                                mainRule.Trigger[subMood].PlusAction[a].Result.Add(0);
                                break;
                            case ("Minus"):
                                a = mainRule.Trigger[subMood].MinusAction.Count;
                                mainRule.Trigger[subMood].MinusAction.Add(new IfAction());
                                mainRule.Trigger[subMood].MinusAction[a].Core.Add(new RuleForm());
                                mainRule.Trigger[subMood].MinusAction[a].Result.Add(0);
                                break;
                            case ("Action"):
                                a = mainRule.Trigger[subMood].Action.Count;
                                mainRule.Trigger[subMood].Action.Add(new RuleAction());
                                mainRule.Trigger[subMood].Action[a] = NewForm(mainRule.Trigger[subMood].Action[a]);
                                break;
                        }
                        break;
                    case ("Remove"):
                        a = int.Parse(com[2]);
                        switch (com[1])
                        {
                            case ("Plus"):
                                mainRule.Trigger[subMood].PlusAction.RemoveAt(a);
                                break;
                            case ("Minus"):
                                mainRule.Trigger[subMood].MinusAction.RemoveAt(a);
                                break;
                            case ("Action"):
                                mainRule.Trigger[subMood].Action.RemoveAt(a);
                                break;
                        }
                        break;
                }
            }
            TextRule("HeadInfo");

        }
       
        static void Switch(string str)
        {
            Debug.Log(str);
            int a;
            string[] com = str.Split('_');

            str = AddLink("ClearIO", "Back") + "\n\n";

            switch (com[0])
            {
                case ("BD"):
                    SwitchBD();
                    return;
                    break;
                case ("Antipod"):
                    str += AddLink("SetSwitch|Antipod_-1", "Null") + "\n";
                    for (int i = 0; i < core.bD[core.keyStat].Base.Count; i++)
                        if(keyB != i)
                            str += AddLink($"SetSwitch|Antipod_{i}", core.bD[core.keyStat].Base[i].Name) + "\n";
                    
                    break;
                case ("Color"):
                    str += SwitchColor();
                    break;
                case ("Icon"):
                    str += SwitchIcon();
                    break;
            }
            ui.TT[0].text = str;


        }
        static void SetSwitch(string str)
        {
            int a, b;
            MainBase mainBase1 = null;
            string[] com = str.Split('_');
           // AddEdit($"{keyA}-{keyB}");
            switch (com[0])
            {

                case ("Antipod"):
                    a = int.Parse(com[1]);
                    if (a == -1)
                        if (mainBase.Sub.Antipod == -1)
                            return;

                    if (a != -1)
                    {
                        mainBase1 = core.bD[keyA].Base[a];
                        if (mainBase1.Sub.Antipod != -1)
                        {
                            core.bD[keyA].Base[mainBase1.Sub.Antipod].Sub.Antipod = -1;
                            Saver.SaveBD(keyA, mainBase1.Sub.Antipod);

                        }
                        mainBase1.Sub.Antipod = keyB;
                        Saver.SaveBD(keyA, a);
                    }
                    else
                    {
                        if (mainBase.Sub.Antipod != -1)
                        {
                            mainBase1 =  core.bD[keyA].Base[mainBase.Sub.Antipod];
                            mainBase1.Sub.Antipod = -1;
                            Saver.SaveBD(keyA, mainBase.Sub.Antipod);
                        }
                    }


                    mainBase.Sub.Antipod = a;
                    //Saver.SaveBD(keyA, keyB);

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
                case ("Visible"):
                    if(keyA <0)
                        mainRule.Visible = !mainRule.Visible;
                    else
                        mainBase.Visible = !mainBase.Visible;
                    break;

                case ("VisibleCard"):
                        mainRule.VisibleCard = !mainRule.VisibleCard;
                    
                    break;
                case ("Regen"):
                   // mainBase.Sub.Regen = !mainBase.Sub.Regen;
                    break;
                case ("Hide"):
                    a = int.Parse(com[1]);
                    b = int.Parse(com[2]);
                    //core.bD[a].Hide[b] = !core.bD[a].Hide[b];
                    break;
                case ("CountMod"):
                    mainRule.Trigger[subMood].CountMod = !mainRule.Trigger[subMood].CountMod;
                    break;
                case ("CountModExtend"):
                    mainRule.Trigger[subMood].CountModExtend = !mainRule.Trigger[subMood].CountModExtend;
                    break;
                default:
                    Debug.Log(com[0]);
                    break;
            }
            ClearIO();
        }

        #endregion

        #region IO
        static void GetIO(string str)
        {
            //string[] com = str.Split('_');

            ui.NameTT.Select();
            // GetIOText();
            ui.TT[0].text = "";
            switch (str)
            {
                case ("HeadBase"):
                    ui.NameTT.text = core.bD[keyA].Name;
                    break;
                case ("HeadInfo"):
                    ui.NameTT.text = core.bD[keyA].Info;
                    break;
                case ("Base"):
                    ui.NameTT.text = mainBase.Name;
                    break;
                case ("Info"):
                    ui.NameTT.text = mainBase.Info;
                    break;

                case ("RuleName"):
                    ui.NameTT.text = core.head[KeyAConverter()].Rule[keyB];
                    break;
            }
           // nameTT.gameObject.active = true;
            GetIOText(str);
            //Debug.Log(com[0]);
        }

        static void SetIO(string str)
        {
            switch (str)
            {
                case ("HeadBase"):
                    core.bD[keyA].Name = ui.NameTT.text;
                    Saver.SaveBDMain(keyA);
                    break;
                case ("HeadInfo"):
                    core.bD[keyA].Info = ui.NameTT.text;
                    Saver.SaveBDMain(keyA);
                    break;
                case ("Base"):
                    mainBase.Name = ui.NameTT.text;
                    break;
                case ("Info"):
                    mainBase.Info = ui.NameTT.text;
                    break;

                case ("RuleName"):
                    core.head[KeyAConverter()].Rule[keyB] = ui.NameTT.text;
                    Saver.SaveRuleMain(KeyAConverter());
                    break;
            }

            ClearIO();
        }

        static void ClearIO()
        {
            if (keyB == -1)
                return;
            if (keyA < 0)
            {
                TextRule($"Menu_{keyA}");
                TextRule("HeadInfo");
            }
            else
            {
                TextBD($"Menu_{keyA}");
                TextBD("Info");
            }
            //nameTT.gameObject.active = false;
        }

        static void GetIOText(string str)
        {
            ui.TT[1].text = AddLink("ClearIO", "Back") + "      " + AddLink($"SetIO|{str}", "OK") + "\n\n";
        }
        #endregion

        #region Text
        static string WebText(int a)
        {
            string str = "";
            for (int i = 0; i < core.bD[a].Base.Count; i++)
            {
                str += "\n  " + AddLink($"Key|{a}.{i}", core.bD[a].Base[i].Name + IfLook(core.bD[a].Base[i].Look)) ;
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
            for (int i = 0; i < core.bD[core.keyStat].Base.Count; i++)
            {
                str += "\n  " + AddLink($"Edit|{mood}_{i}_1", core.bD[core.keyStat].Base[i].Name + IfLook(core.bD[core.keyStat].Base[i].Look) + "-Add");
            }

            return str;
        }
        static string WebText(List<int> list, int a, string mood)
        {
            int b;
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                b = list[i];
                str += "\n  " + AddLink($"Key|{a}-{b}", core.bD[a].Base[b].Name) + IfLook(core.bD[a].Base[b].Look);
                str += "     " + AddLink($"Edit|Remove_{b}_{mood}", "-Remove", core.frame.ColorsStr[1]);
            }
            str += "\n  " +AddLink($"Open|Add_{a}_{mood}", "-Add");
            

            return str;
        }
        static string WebText(List<int> list, string mood)
        {
            int b;
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                b = list[i];
                str += "\n  " + AddLink($"Key|{core.keyStat}.{b}", core.bD[core.keyStat].Base[b].Name) + IfLook(core.bD[core.keyStat].Base[b].Look);
                str += "     " + AddLink($"Edit|{mood}_{b}_0", "-Remove", core.frame.ColorsStr[1]);
            }
            str += "\n  " + AddLink($"Open|NewInfo_{mood}", "-Add");


            return str;
        }

        static string IfLook(bool use)  { return(use) ? "[]" : "[ ]"; }
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
            Debug.Log(str);
            int b=0, c=0;
            string[] com = str.Split('*');
            bool mood = (com[1] == "-"); 
            com = com[0].Split('_');

            if (com.Length == 2)
                b = int.Parse(com[1]);

            switch (com[0])
            {
                case ("SizePlan"):
                    if (mood)
                        mainBase.Plan.Size--;
                    else
                        mainBase.Plan.Size++;
                    break;
                case ("Cost"):

                    int key = KeyAConverter();
                    if (mood)
                        core.head[key].Cost[keyB]--;
                    else
                        core.head[key].Cost[keyB]++;
                    Saver.SaveRuleMain(key);
                    break;

                case ("Team"):
                    if (mood)
                        mainRule.Trigger[subMood].Team--;
                    else
                        mainRule.Trigger[subMood].Team++;
                    //mainRule.Trigger[a].Team = IfPlus(mainRule.Trigger[a].Team, core.frame.PlayerString.Length, mood);
                    break;

                case ("ActionPlus"):
                    if (mood)
                        mainRule.Trigger[subMood].PlusAction[b].Point--;
                    else
                        mainRule.Trigger[subMood].PlusAction[b].Point++;
                    break;
                case ("ActionMinus"):
                    if (mood)
                        mainRule.Trigger[subMood].MinusAction[b].Point--;
                    else
                        mainRule.Trigger[subMood].MinusAction[b].Point++;
                    break;
                case ("ActionMin"):
                    if (mood)
                        mainRule.Trigger[subMood].Action[b].Min--;
                    else
                        mainRule.Trigger[subMood].Action[b].Min++;

                    if (mainRule.Trigger[subMood].Action[b].Min > mainRule.Trigger[subMood].Action[b].Max)
                        mainRule.Trigger[subMood].Action[b].Max = mainRule.Trigger[subMood].Action[b].Min;
                    break;
                case ("ActionMax"):
                    if (mood)
                        mainRule.Trigger[subMood].Action[b].Max--;
                    else
                        mainRule.Trigger[subMood].Action[b].Max++;

                    if (mainRule.Trigger[subMood].Action[b].Min > mainRule.Trigger[subMood].Action[b].Max)
                        mainRule.Trigger[subMood].Action[b].Min = mainRule.Trigger[subMood].Action[b].Max;
                    break;

                case ("ActionPrioritet"):
                    if (mood)
                        mainRule.Trigger[subMood].Action[b].Prioritet--;
                    else
                        mainRule.Trigger[subMood].Action[b].Prioritet++;

                    break;


                case ("ActionPlusResult"):
                    c = int.Parse(com[2]);
                    mainRule.Trigger[subMood].PlusAction[b].Result[c] = IfPlus(mainRule.Trigger[subMood].PlusAction[b].Result[c], core.frame.EqualString.Length, mood);
                    break;
                case ("ActionMinusResult"):
                    c = int.Parse(com[2]);
                    mainRule.Trigger[subMood].MinusAction[b].Result[c] = IfPlus(mainRule.Trigger[subMood].MinusAction[b].Result[c], core.frame.EqualString.Length, mood);
                    break;

                case ("ActionTeam"):
                    mainRule.Trigger[subMood].Action[b].Team = IfPlus(mainRule.Trigger[subMood].Action[b].Team, core.frame.PlayerString.Length, mood);
                    break;
                case ("ActionForse"):
                    mainRule.Trigger[subMood].Action[b].ForseMood = IfPlus(mainRule.Trigger[subMood].Action[b].ForseMood, core.frame.ForseTayp.Length, mood);
                    break;




                default:
                    com = com[0].Split('?');
                    b = int.Parse(com[2]);

                    c = int.Parse(com[3]);
                    RuleForm form = null;
                    switch (com[1])
                    {
                        case ("ResultPlus"):
                            form = mainRule.Trigger[subMood].PlusAction[b].ResultCore[c];
                            break;
                        case ("ResultMinus"):
                            form = mainRule.Trigger[subMood].MinusAction[b].ResultCore[c];
                            break;
                        case ("Plus"):
                            form = mainRule.Trigger[subMood].PlusAction[b].Core[c]; 
                            break;
                        case ("Minus"):
                            form = mainRule.Trigger[subMood].MinusAction[b].Core[c];
                            break;
                        case ("ResultAction"):
                            form = mainRule.Trigger[subMood].Action[b].ResultCore;
                            break;
                        default:
                            form = mainRule.Trigger[subMood].Action[b].Core[c];
                            break;
                    }

                    //form = 
                        EditCore(form, com[0], mood);

                    //switch (com[3])
                    //{
                    //    case ("Plus"):
                    //        mainRule.Trigger[subMood].PlusAction[b].Core[c] = form;
                    //        break;
                    //    case ("Minus"):
                    //        mainRule.Trigger[subMood].MinusAction[b].Core[c] = form;
                    //        break;
                    //    default:
                    //        mainRule.Trigger[subMood].Action[b].Core[c] = form;
                    //        break;
                    //}
                    break;
            }
            ClearIO();
        }
        static void EditMove(string str)
        {

            Debug.Log(str);
            int b = 0, c = 0;
            string[] com = str.Split('*');
            bool minus = (com[1] == "-");
            com = com[0].Split('_');

            // a = int.Parse(com[1]);
            b = int.Parse(com[1]);
            if (com.Length == 3)
            {
                c = int.Parse(com[2]);
                int d = (minus) ? c - 1: c + 1;
                
                IfAction ifAction = (com[0] == "Plus") ? mainRule.Trigger[subMood].PlusAction[b] : mainRule.Trigger[subMood].MinusAction[b];

                RuleForm form = ifAction.Core[c];
                ifAction.Core[c] = ifAction.Core[d];
                ifAction.Core[d] = form;

                b = ifAction.Result[c];
                ifAction.Result[c] = ifAction.Result[d];
                ifAction.Result[d] = b;

                if (ifAction.Core[d].Tayp == ifAction.Core[c].Tayp && ifAction.Core[d].Tayp == core.keyStat)
                {
                    int a1 = 0, a2 = 0;
                    for (int i = 0; i < c; i++)
                        if (ifAction.Core[i].Tayp == core.keyStat)
                            a1++;

                    a2 = (minus) ? a1 - 1 : a1 + 1;

                    form = ifAction.ResultCore[a1];
                    ifAction.ResultCore[a1] = ifAction.Core[a2];
                    ifAction.ResultCore[a2] = form;

                }
            }
            else
            {
                int d = (minus) ? b -1: b + 1;
                IfAction ifAction = null;
                switch (com[0])
                {
                    case ("Plus"):
                        ifAction = mainRule.Trigger[subMood].PlusAction[b];
                        mainRule.Trigger[subMood].PlusAction[b] = mainRule.Trigger[subMood].PlusAction[d];
                        mainRule.Trigger[subMood].PlusAction[d] = ifAction;
                        break;
                    case ("Minus"):
                        ifAction = mainRule.Trigger[subMood].MinusAction[b];
                        mainRule.Trigger[subMood].MinusAction[b] = mainRule.Trigger[subMood].MinusAction[d];
                        mainRule.Trigger[subMood].MinusAction[d] = ifAction;
                        break;
                    default:
                        RuleAction action = mainRule.Trigger[subMood].Action[b];
                        mainRule.Trigger[subMood].Action[b] = mainRule.Trigger[subMood].Action[d];
                        mainRule.Trigger[subMood].Action[d] = action;
                        break;
                }
            }
            ClearIO();
        }

        static string TextEditInt(string path, string cost)  {  return AddLink($"Int|{path}*-", "<<") + $" ({cost}) " + AddLink($"Int|{path}*+", ">>");  }

        static string TextMove(string path, int a, int size) 
        { 
            string str = "";
            if(a >0)
                str += AddLink($"Move|{path}*-", " /\\/\\");


            if (a < size -1)
                str += AddLink($"Move|{path}*+", " \\/\\/");

            return str;
        }

        static void TextBD(string str)
        {
            int a, b;
            string[] com = str.Split('_');
            switch (com[0])
            {
                case ("MenuHead"):
                    //str = "";//AddLink("Open|Menu", "Open SysMenu") + "\n"; ;
                    str = AddLink("Switch|BD", "SwitchBD") + "\n";
                    for (int i = 0; i < core.bD.Count; i++)
                        str += AddLink($"Open|Menu_{i}", core.bD[i].Name + $"({core.bD[i].Base.Count})") + "\n";

                    ui.TT[0].text = str;
                    break;

                case ("Menu"):
                    a = int.Parse(com[1]);
                   // bool look = (0 <= core.frame.Tayp[a].Key.FindIndex(x => x == "Look"));
                    str = AddLink("Open|MenuHead", "Back") + "\n";
                    str += AddLink($"Sys|New_{a}", "New " + core.bD[a].Name);
                    str += "\n";
                    str += WebText(a);
                    ui.TT[0].text = str;
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
                    ui.TT[0].text = str; 
                    break;

                case ("Info"):
                    str = HeadBDInfo();
                    if (keyA == core.keyStat)
                    {
                        //str += AddLink("SetSwitch|Regen", "Regen " + ((mainBase.Sub.Regen) ? "Yes" : "No")) + "\n";

                        str += AddLink("Switch|Icon", $"Icon <sprite index={mainBase.Sub.Image}>") + "\n";
                        str += AddLink("Switch|Antipod", (mainBase.Sub.Antipod == -1) ? "Antipod: Null" : "Antipod: " + core.bD[core.keyStat].Base[mainBase.Sub.Antipod].Name) + "\n";

                        str += "\nСписок AntiStat для доступа";
                        str += WebText(mainBase.Sub.AntiStat, core.keyStat, "AntiStat");

                        str += "\nСписок DefStat для доступа";
                        str += WebText(mainBase.Sub.DefStat, core.keyStat, "DefStat");

                    }
                    else if (keyA == core.keyRace)
                    {
                        //a = int.Parse(com[1]);
                        str += "\nРасса родитель: ";
                        if(mainBase.Race.MainRace == -1)
                            str += AddLink($"Open|Add_{keyA}_MainRace", "SetMainRace");
                        else
                        {
                            str += AddLink($"Key|{keyA}.{mainBase.Race.MainRace}", core.bD[core.keyRace].Base[mainBase.Race.MainRace].Name);
                            str += "   ";
                            str += AddLink($"Edit|Remove_{keyA}_MainRace", "Clear");

                        }
                        str += "\nГлавный стат: ";
                        str +=  AddLink($"Open|Add_{core.keyRace}_MainStat", $"-Switch {core.bD[core.keyStat].Base[mainBase.Race.MainStat].Name}");
                    }
                    else if (keyA == core.keyStatGroup)
                    {
                        str += "\nСтат группа: ";
                        str += "\nДелитель группы" + AddLink($"Edit|Remove_0_GroupStatMain", $"<<")+$" ({mainBase.Group.MainSize}) " +AddLink($"Edit|Add_0_GroupStatMain", $">>");
                        for (int i = 0; i < mainBase.Group.Stat.Count; i++)
                        {
                            str += "\n" + AddLink($"Key|{core.keyStat}.{mainBase.Group.Stat[i]}", core.bD[core.keyStat].Base[mainBase.Group.Stat[i]].Name);
                            str += "   ";
                            str += AddLink($"Edit|Remove_{i}_GroupStatSize", $"<<") + $" ({mainBase.Group.Size[i]}) " + AddLink($"Edit|Add_{i}_GroupStatSize", $">>");
                            str += "   ";
                            str += AddLink($"Edit|Remove_{i}_GroupStat", "Clear");

                        }
                        str += "\n" + AddLink($"Open|Add_{core.keyStat}_GroupStat", $"Add Stat");
                    }
                    else if(keyA == core.keyPlan)
                    {

                        str += "\nРазмер плана: ";
                        str += TextEditInt("SizePlan", "" + mainBase.Plan.Size);
                    }
                    str += AccsesText(mainBase.accses);
                    /*
                    for (int i = 0; i < mainBase.Text.Count; i++)
                    {
                        str += $"\nСписок {core.bD[keyA].Key[i]} для доступа ";
                        str += AddLink($"SetSwitch|Hide_{keyA}_{i}", (core.bD[keyA].Hide[i]) ? "Close" : $"Open ({mainBase.Text[i].Text.Count})") + "\n";
                        if (core.bD[keyA].Hide[i])
                        {
                            str += WebText(mainBase.Text[i].Text, i);
                        }
                    }*/

                    ui.TT[1].text = str;
                    break;
                default:
                    a = int.Parse(com[1]);
                    str = AddLink("ClearIO", "Back") + "\n\n";
                    List<int> mainlist = new List<int>();
                    for (int i = 0; i < core.bD[a].Base.Count; i++)
                        mainlist.Add(i);

                    if(a == core.keyStatGroup)
                    {
                        for (int i = 0; i < mainBase.Group.Stat.Count; i++)
                            mainlist.Remove(mainBase.Group.Stat[i]);
                    }
                    else if (a == core.keyStat)
                    {
                        mainlist.Remove(mainBase.Sub.Antipod);
                            
                        for (int i = 0; i < mainBase.Sub.AntiStat.Count; i++)
                            mainlist.Remove(mainBase.Sub.AntiStat[i]);

                        for (int i = 0; i < mainBase.Sub.DefStat.Count; i++)
                            mainlist.Remove(mainBase.Sub.DefStat[i]);
                        
                    }
                    else if(a == core.keyRace)
                    {
                        if (com[2] == "MainStat")
                        {
                            mainlist = new List<int>();
                            for (int i = 0; i < core.bD[core.keyStat].Base.Count; i++)
                                mainlist.Add(i);

                            for (int i = 0; i < mainlist.Count; i++)
                                str += "\n" + AddLink($"Edit|{com[0]}_{mainlist[i]}_{com[2]}", $"Add {core.bD[core.keyStat].Base[mainlist[i]].Name}");

                            ui.TT[0].text = str;
                            return;
                        }
                        else
                        {
                            BD bd = core.bD[a];
                            List<int> localList = new List<int>();
                            for (int i = 0; i < mainBase.Race.UseRace.Count; i++)
                                localList.Add(mainBase.Race.UseRace[i]);

                            List<int> oldId;
                            for (int i = 0; i < localList.Count; i++)
                            {
                                oldId = bd.Base[localList[i]].Race.UseRace;

                                for (int j = 0; j < oldId.Count; j++)
                                    if (!bd.Base[oldId[j]].Look)
                                        localList.Add(oldId[j]);
                            }



                            localList.Add(keyB);
                            for (int i = 0; i < localList.Count; i++)
                                mainlist.Remove(localList[i]);
                        }
                      
                    }
                    else
                    {
                        mainlist.RemoveAt(keyB);
                    }

                    for (int i = 0; i < mainlist.Count; i++)
                        str += "\n"+ AddLink($"Edit|{com[0]}_{mainlist[i]}_{com[2]}",$"Add {core.bD[a].Base[mainlist[i]].Name}");

                    ui.TT[0].text = str;
                    break;
            }
            //return str;
        }
        public static string AccsesText(Accses accses, bool full = true)
        {
            string sub(Accses accses, string mood, bool full)
            {
                string str = "\n";
                int a;
                List<SubInt> list = null;
                switch (mood)
                {
                    case ("Like"):
                        list = accses.Like;
                        str += "Разрешения";
                        break;
                    case ("Need"):
                        list = accses.Need;
                        str += "требования";
                        break;
                    case ("DisLike"):
                        list = accses.DisLike;
                        str += "Запреты";
                        break;
                    case ("Use"):
                        list = accses.Use;
                        str += "Упоминается";
                        break;
                    case ("Mark"):
                        list = accses.Mark;
                        str += "Теги";
                        break;
                }

                str += $" механики ";
                if(full)
                    str += AddLink($"LinkerMain|{mood}_1", $"\nСоздать раздел");
                for (int i = 0; i < list.Count; i++)
                    if(list[i].Head < 0)
                    {
                        int b = -list[i].Head - 1;
                        if (full)
                        {
                            str += AddLink($"LinkerRead|{mood}_{list[i].Head}_-1_0", $"\nУдалить заголовок {core.head[b].Name}");

                            str += AddLink($"Linker|{mood}_{list[i].Head}_1", $"\nСоздать связь в {core.head[b].Name}");
                            for (int i1 = 0; i1 < list[i].Num.Count; i1++)
                            {
                                //for (int i2 = 0; i2 < core.head[b].Index.Count; i2++)
                                //{
                                //    Debug.Log(core.head[b].Index[i2]);
                                //}

                                a = core.head[b].Index.FindIndex(x => x == list[i].Num[i1].Head);
                                if(a != -1)
                                {
                                    str += AddLink($"LinkerRead|{mood}_{list[i].Head}_{list[i].Num[i1].Head}_0", $"\n   Разорвать связь с");
                                    str += AddLink($"Key|{list[i].Head}.{a}",$"    {core.head[b].Rule[a]}");
                                }
                                else
                                    str += AddLink($"LinkerRead|{mood}_{list[i].Head}_{list[i].Num[i1].Head}_0", $"\n    Разорвать связь с {b} | {list[i].Num[i1].Head} !!!!!");
                            }
                        }
                        else
                        {
                            str += $"\nЗаголовок {core.head[b].Name}";
                            for (int i1 = 0; i1 < list[i].Num.Count; i1++)
                            {
                                a = core.head[b].Index.FindIndex(x => x == list[i].Num[i1].Head);
                                if (a != 0)
                                    str += AddLink($"Key|{b}.{a}", core.head[b].Rule[a]);
                                else
                                    str += $"\n  {b} |{list[i].Num[i1].Head} !!!!!";
                            }
                        }

                    }
                    else
                    {
                        if (full)
                        {
                            str += AddLink($"LinkerRead|{mood}_{list[i].Head}_-1_0", $"\nУдалить заголовок {core.bD[list[i].Head].Name}");

                            str += AddLink($"Linker|{mood}_{list[i].Head}_1", $"\nСоздать связь в {core.bD[list[i].Head].Name}");
                            for (int i1 = 0; i1 < list[i].Num.Count; i1++)
                            {
                                str += AddLink($"LinkerRead|{mood}_{list[i].Head}_{list[i].Num[i1].Head}_0", $"\n    Разорвать связь с ");

                                str += $"\n " + AddLink($"Key|{list[i].Head}-{list[i].Num[i1].Head}", core.bD[list[i].Head].Base[list[i].Num[i1].Head].Name);
                            }
                            
                        }
                        else
                        {
                            str += $"\nЗаголовок {core.bD[list[i].Head].Name}";
                            for (int i1 = 0; i1 < list[i].Num.Count; i1++)
                                str += $"\n "+ AddLink($"Key|{list[i].Head}.{list[i].Num[i1].Head}", core.bD[list[i].Head].Base[list[i].Num[i1].Head].Name);
                        }
                    }
                str += "\n";

                return str;
            }

            string str1 = "";

            str1 += sub(accses, "Like", full);
            str1 += sub(accses, "Need", full);
            str1 += sub(accses, "DisLike", full);
            str1 += sub(accses, "Use", false);
            str1 += sub(accses, "Mark", full);

            return str1;
        }

        static void TextRule(string str)
        {
            int a, b,c;
            string[] com = str.Split('_');

            switch (com[0])
            {
                case ("MenuHead"):
                    str = AddLink("Switch|BD", "SwitchBD") + "\n";
                    for (int i = 0; i < core.head.Count; i++)
                        str += AddLink($"Open|Menu_{-i-1}", core.bD[core.keyTag].Base[i].Name + IfLook(core.bD[core.keyTag].Base[i].Look) + $"({core.head[i].Index.Count})") + "\n";

                    ui.TT[0].text = str;
                    break;

                case ("Menu"):
                    a = int.Parse(com[1]);
                    b = -a - 1;
                    str = AddLink("Open|MenuHead", "Back") + "\n";
                    str += AddLink($"Sys|New_{a}", "New Rule " + core.head[b].Name);
                    str += "\n";

                    for (int i = 0; i < core.head[b].Index.Count; i++)
                    {
                        str += "\n  " + AddLink($"Key|{a}.{i}", $"({core.head[b].Index[i]})"+ core.head[b].Rule[i]);
                    }

                    ui.TT[0].text = str;
                    break;


                    
                case ("HeadInfo"):
                    int key = KeyAConverter();
                    if (subMood == -1)
                    {
                        str = AddLink($"Sys|Save", "Save");
                        str += "    ";
                        str += AddLink($"Sys|Load", "Load");
                        str += "    ";
                        str += AddLink($"Sys|Clear", "Clear");
                        str += "    ";
                        str += AddLink($"Sys|Del", "Delite");
                        str += "\n\n";
                        str += AddLink("GetIO|RuleName", $"({core.head[key].Index[keyB]})Правило -- { core.head[key].Rule[keyB]}");
                        // str += AddLink("Edit_Cost", $"\nЦена: { mainBase.Cost}");
                        str += $"\nЦена: " + TextEditInt("Cost", "" + core.head[key].Cost[keyB]);
                        str += $"\nTag {core.bD[core.keyTag].Base[mainRule.Tag].Name}";
                        // str += AddLink("Tag_Tag", $"\nTag { mainBase.Tag}");

                        str += "\n" + AddLink("SetSwitch|Visible", (mainRule.Visible) ? "Visible" : "InVisible");
                        str += "\n" + AddLink("SetSwitch|VisibleCard", (mainRule.VisibleCard) ? "Visible" : "InVisible");


                        str += "\nТребуемые механики";
                        str += AccsesText(mainRule.accses);
                        str += "\n";


                        str += "\n\nTriggers";
                        for (int i = 0; i < mainRule.Trigger.Count; i++)
                        {
                            string str1 = "Trigger";
                            if (mainRule.Trigger[i].Plan != -1)
                                str1 += "    " + core.bD[core.keyPlan].Base[mainRule.Trigger[i].Plan].Name;
                            else
                                str1 += "    AllPlan";
                            str1 += "    " + core.frame.Trigger[mainRule.Trigger[i].Trigger];
                            str += AddLink($"SetMood|{i}", $"\n    {str1}") + "   " + AddLink($"Edit|RuleList_Remove_Trigger_{i}", "-Remove");
                        }
                        str += "\n";
                        str += AddLink($"Edit|RuleList_Add_Trigger", "-Add");
                    }
                    else
                    {
                        //b = mainRule.Trigger[a].Trigger;
                        str = AddLink($"SetMood|-1", $"Back");
                        str += "\n" + AddLink($"Edit|RuleList_Return_Trigger_{subMood}", $"Trigger -- " + core.frame.Trigger[mainRule.Trigger[subMood].Trigger]);
                        //if (core.frame.Trigger[b].Extend.Length > 1)
                        //    str += AddLink($"Edit|TriggerExtend_Open_{a}", $"Режим -- " + core.frame.Trigger[b].Extend[mainRule.Trigger[a].TriggerExtend]); 

                        str += "\n"+ AddLink($"SetSwitch|CountMod", $"Правило с счетчиком -- " + ((mainRule.Trigger[subMood].CountMod) ? "Yes" : "No"));
                        str += "\n" + AddLink($"SetSwitch|CountModExtend", $"Правило с разницей -- " + ((mainRule.Trigger[subMood].CountModExtend) ? "Yes" : "No"));


                        str += "\n" + AddLink($"Edit|RuleList_Return_Plan_{subMood}", $"Текущий план " + ((mainRule.Trigger[subMood].Plan > -1) ? core.bD[core.keyPlan].Base[mainRule.Trigger[subMood].Plan].Name : "AllPlan"));
                        
                        str += "\nTeam";
                        if (mainRule.Trigger[subMood].Team > 0)
                            str += AddLink($"Int|Team*-", "<<");

                        str += $" ({core.frame.PlayerString[mainRule.Trigger[subMood].Team]}) ";

                        //Debug.Log(mainRule.Trigger[subMood].Team);
                       // Debug.Log(core.frame.EqualString.Length);
                        if (mainRule.Trigger[subMood].Team < core.frame.PlayerString.Length-1)
                            str += AddLink($"Int|Team*+", ">>");

                        //str += $"\n Целевой игрок " + TextEditInt($"Team", core.frame.PlayerString[mainRule.Trigger[a].Team]);


                        str += "\n";
                        str += "\n Положительные условия";
                        str += AddLink($"Edit|Add_Plus", $"\nСоздать условие");
                        for (int i = 0; i < mainRule.Trigger[subMood].PlusAction.Count; i++)
                        {
                            str += IfActionCase( i, "Plus");
                            str += AddLink($"Edit|Remove_Plus_{i}", $"\nУдалить условие");
                            str += "\n";
                        }
                        str += "\n";


                        str += "\nИсключающие условия";
                        str += AddLink($"Edit|Add_Minus", $"\nСоздать исключение");
                        for (int i = 0; i < mainRule.Trigger[subMood].MinusAction.Count; i++)
                        {
                            str += IfActionCase( i, "Minus");
                            str += AddLink($"Edit|Remove_Minus_{i}", $"\nУдалить исключение");
                            str += "\n";
                        }
                        str += "\n";



                        str += "\nДействия";
                        str += AddLink($"Edit|Add_Action", $"\nСоздать действие");
                        for (int i = 0; i < mainRule.Trigger[subMood].Action.Count; i++)
                        {
                            str += "\n";
                            str += TextMove($"Action_{i}", i, mainRule.Trigger[subMood].Action.Count);
                            str += AddLink($"Edit|Remove_Action_{i}", $"\nУдалить действие");
                            str += "\n";
                            str += ActionRule( i);
                        }
                    }
                    ui.TT[1].text = str;
                    break;
                    /*
                     системная привязка на выборку, для действия она завзана только на статах, для остальных свободный доступ

                     */



            }
        }
        static string IfActionCase(int b, string plus)
        {
            int c = 0;
            IfAction ifAction = (plus == "Plus") ? mainRule.Trigger[subMood].PlusAction[b] : mainRule.Trigger[subMood].MinusAction[b];
            string str = "",key = plus+$"?{b}?";
            str += $"\n({b})";
            str += TextMove($"{plus}_{b}", b, (plus == "Plus") ? mainRule.Trigger[subMood].PlusAction.Count : mainRule.Trigger[subMood].MinusAction.Count);

            str +=$" Point";
            str += TextEditInt($"Action{plus}_{b}", "" + ifAction.Point);
            for (int i = 0; i < ifAction.Core.Count; i++)
            {
                str += "\n";
                str += TextMove($"{plus}_{b}_{i}", i, ifAction.Core.Count);

                bool use = (ifAction.Core[i].Tayp == core.keyStat);
                //str += "\n";
                Debug.Log(key + i);
                str += ReturnCore(ifAction.Core[i],key+i);

                str += " ";
                if (ifAction.Result[i] > 0)
                    str += AddLink($"Int|Action{plus}Result_{b}_{i}*-", "<<");

                str += $" ({core.frame.EqualString[ifAction.Result[i]]}) ";
                if (use)
                {
                    if (ifAction.Result[i] < core.frame.EqualString.Length-1)
                        str += AddLink($"Int|Action{plus}Result_{b}_{i}*+", ">>");
                }
                else
                    if (ifAction.Result[i] < 1)
                    str += AddLink($"Int|Action{plus}Result_{b}_{i}*+", ">>");


                str += " ";

                if (use)
                {
                    str += ReturnCore(ifAction.ResultCore[i], "Result"+key + c, true);
                    c++;
                }

                if (i != 0)
                    str += AddLink($"Edit|Remove_Core_{plus}_{b}_{i}", "-- Remove");
            }
            str += AddLink($"Edit|Add_Core_{plus}_{b}", "-- Add");
            return str;
        }
        static string ActionRule( int b)
        {
            RuleAction action = mainRule.Trigger[subMood].Action[b];
            string key = $"Action?{b}?";
            string str = "";
            str += $"\n({b}) Prioritet ";
            str += TextEditInt($"ActionPrioritet_{b}", "" + action.Prioritet);
            str += $"\n({b}) Point";
            str += TextEditInt($"ActionMin_{b}", "" + action.Min);
            str += "    ";
            str += TextEditInt($"ActionMax_{b}", "" + action.Max);

            // str += AddLink($"Edit|Remove_Core_{plus}_{a}_{b}_{i}", "-- Remove");

            str += "\nTeam ";

            if (action.Team > 0)
                str += AddLink($"Int|ActionTeam_{b}*-", "<<");

            str += $" ({core.frame.PlayerString[action.Team]}) ";

            if (action.Team < core.frame.PlayerString.Length-1)
                str += AddLink($"Int|ActionTeam_{b}*+", ">>");

            str += "\n";

            str += "\nAction ";
            str += AddLink($"Edit|RuleList_Return_Action_{b}", core.frame.Action[action.Action].Name);
            if (core.frame.Action[subMood].Extend.Length > 1)
            {
                str += " ";
                str += AddLink($"Edit|RuleList_Return_ActionExtend_{b}*{action.Action}", core.frame.Action[action.Action].Extend[action.ActionExtend]);
            }

            str += "\n" + ReadForm(action, key);
            //редактирование через лист и затем обработка через форму
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
            //// Debug.Log("!");//тут стоит вариатор условии в сотвествии 
            // str += ReturnCore(action.ResultCore, key, true);//используямая бд идет с шаблона

            return str;
        }
        static RuleAction NewForm(RuleAction action)
        {
            int a = 1;
            switch (core.frame.Action[action.Action].Name)
            {
                case ("Attack"):
                    a = 2;
                    break;
                case ("Stat"):
                    a = 2;
                    break;
                case ("Rule"):
                    a = 2;
                    break;
                case ("Effect"):
                    a = 5;
                    break;
                case ("Transf"):
                    a = 0;
                    break;
                    //case ("Rule"):
                    //    action.ResultCore.TaypId = keyStat; 
                    //    break;
            }

            action.Core = new List<RuleForm>(new RuleForm[a]);
            for (int i = 0; i < a; i++)
                action.Core[i] = new RuleForm(core.keyStat);

            switch (core.frame.Action[action.Action].Name)
            {
                case ("Rule"):
                    action.ResultCore = new RuleForm(0);
                    action.Core[2].Tayp = core.keyPlan;
                    Debug.Log(action.Core[2].Tayp);
                    break;
                case ("Transf"):
                    action.ResultCore = new RuleForm(core.keyPlan);
                    break;
                default:
                    action.ResultCore = new RuleForm(core.keyStat);
                    action.ForseMood = 0;
                    break;
            }
            return action;
        }
        static string ReadForm( RuleAction action, string path)
        {
            string result = "Result" + path + "0";
            string str = "";
            switch (action.Core.Count)
            {
                case (6):
                    str += "\nПриоритет " + ReturnCore(action.Core[0], path + 0, true) + "/" + ReturnCore(action.Core[1], path + 1, true);
                    str += "\nПродолжительность " + ReturnCore(action.Core[2], path + 2, true) + "/" + ReturnCore(action.Core[3], path + 3, true);
                    str += "\nСила " + ReturnCore(action.Core[4], path + 4, true) + "/" + ReturnCore(action.Core[5], path + 5, true);
                    break;

                case (2):
                    str += "\nОснова " + ReturnCore(action.Core[0], path + 0, true);
                    str += "\nДелитель " + ReturnCore(action.Core[1], path + 1, true);
                    break;

                case (3):
                    str += "\nОснова " + ReturnCore(action.Core[0], path + 0, true);
                    str += "\nДелитель " + ReturnCore(action.Core[1], path + 1, true);
                    str += "\nНовый план " + ReturnCore(action.Core[2], path + 1, true);
                    break;

                case (1):
                    str += "\nНовый план " + ReturnCore(action.Core[0], path + 1, true);
                    break;

            }
            int a = -1;
            if (core.frame.Action[action.Action].Name == "Rule")
                a = action.ActionExtend;

            str += "\nРезультат " + ReturnCore(action.ResultCore, result, true,a);

            if(action.ResultCore.Tayp == core.keyStat)
                str += "\nForse " + AddLink($"Edit|RuleList_Return_Forse_{path}", $"{core.frame.ForseTayp[action.ForseMood]}");

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


            str += AddLink($"GetIO|HeadBase", "Название раздела :" + core.bD[keyA].Name);

            string link = $"GetIO|HeadInfo";
            str +=(core.bD[keyA].Info == "Void") ? AddLink(link, "[!]", core.frame.ColorsStr[1]) : AddLink(link, "[?]", core.frame.ColorsStr[0]);

            str += $"\n";
            str += $"\n{keyB}- ";

            str += AddLink("GetIO|Base", "Имя " + mainBase.Name, mainBase.Color);
            link = $"GetIO|Info";
            str += (mainBase.Info == "Void") ? AddLink(link, "[!]", core.frame.ColorsStr[1]) : AddLink(link, "[?]", core.frame.ColorsStr[0]);

            str += "\n" + AddLink("Switch|Color", $"Цвет = {mainBase.Color}", mainBase.Color);

            str += "\n" + $"Цена: " + AddLink("Edit|Remove_0_Cost", " << ") + $"{mainBase.Cost}" + AddLink("Edit|Add_0_Cost", " >> ");
            str += "\n" + AddLink("SetSwitch|Look", "Уровень доступа: "+((mainBase.Look) ? "Требует разрешения" : "Общедоступный") + IfLook(mainBase.Look)) + "\n";
            str += "\n" + AddLink("SetSwitch|Visible", "Видимость в вики: " + ((mainBase.Visible) ? "Виден" : "Невиден"));
            

            return str;
        }
        static void LinkerRead( string moodData, int a, int b, bool add)
        {
            Accses localAccses;
            int localKey;
            if (keyA >-1)
            {
                localAccses = mainBase.accses;
                localKey = keyA;
            }
            else
            {
                localAccses = mainRule.accses;
                localKey = -keyA - 1;
            }

            Debug.Log(core.keyMark);
            localAccses.Edit(moodData, a, b, add);
            if (moodData != "Mark")
                if (b != -1)
                {
                    if (a < 0)
                    {
                        Debug.Log(core.keyMark);
                        HeadRule localRule = Saver.LoadRule(-a - 1, b);
                        localRule.accses.Edit("Use", localKey, keyB, add);
                        Saver.SaveRule(localRule, -a - 1, b);
                    }
                    else
                    {
                        core.bD[a].Base[b].accses.Edit("Use", localKey, keyB, add);
                        Saver.SaveBD(a, b);
                    }
                }

            //Saver.SaveBD(keyA, keyB);
            ClearIO();
        }
        static void LinkerMain(string mood, int add)
        {
            string str = AddLink("ClearIO", "Back");
            str += $"\n Система связи доступов в БД";

            if (mood == "Mark")
            {
                for (int i = 0; i < core.bD[core.keyMark].Base.Count; i++)
                    str += $"\n" + AddLink($"LinkerRead|{mood}_{core.keyMark}_{i}_{add}", $"Set { core.bD[core.keyMark].Base[i].Name}");

            }
            else
                for (int i = 0; i < core.bD.Count; i++)
                    str += $"\n" + AddLink($"Linker|{mood}_{i}_{add}", $"Open { core.bD[i].Name}");

            ui.TT[0].text = str;
        }
        static void Linker(string moodData, int a, int add)
        {
            Accses localAccses;
            if (keyA > -1)
                localAccses = mainBase.accses;
            else
                localAccses = mainRule.accses;

            List<int> nums = new List<int>();
            if (a < 0)
            {
                for (int i = 0; i < core.head[-a - 1].Index.Count; i++)
                    nums.Add(core.head[-a - 1].Index[i]);
            }
            else if (a == core.keyTag)
            {
                for (int i = 0; i < core.bD[a].Base.Count; i++)
                    if (core.head[i].Index.Count > 0)
                        nums.Add(-i-1);
            }
            else
                for (int i = 0; i < core.bD[a].Base.Count; i++)
                    nums.Add(i);

            if (a != core.keyTag) 
            {
                int b = 0;
                if (moodData == "Need")
                {
                    b = localAccses.Find(localAccses.Like, a, false);
                    if (b != -1)
                        for (int i = 0; i < localAccses.Like[b].Num.Count; i++)
                            nums.Remove(localAccses.Like[b].Num[i].Head);
                }


                if (moodData == "Like")
                {
                    b = localAccses.Find(localAccses.Need, a, false);
                    if (b != -1)
                        for (int i = 0; i < localAccses.Need[b].Num.Count; i++)
                            nums.Remove(localAccses.Need[b].Num[i].Head);
                }

                b = localAccses.Find(localAccses.DisLike, a, false);
                if (b != -1)
                    for (int i = 0; i < localAccses.DisLike[b].Num.Count; i++)
                        nums.Remove(localAccses.DisLike[b].Num[i].Head);
            }


            string str = AddLink("ClearIO", "Back");
            if(a < 0)
            {
                for (int i = 0; i < nums.Count; i++)
                    str += $"\n" + AddLink($"LinkerRead|{moodData}_{a}_{nums[i]}_{add}", $"Set { core.head[-a-1].Rule[nums[i]]}");
            }
            else if(a == core.keyTag)
            {
                for (int i = 0; i < nums.Count; i++)
                    str += $"\n" + AddLink($"LinkerRead|{moodData}_{nums[i]}_-1_{add}", $"Set { core.bD[a].Base[-nums[i]-1].Name}");
            }
            else
            {
                for (int i = 0; i < nums.Count; i++)
                    str += $"\n" + AddLink($"LinkerRead|{moodData}_{a}_{nums[i]}_{add}", $"Set { core.bD[a].Base[nums[i]].Name}");
            }

            ui.TT[0].text = str;
        }

        static void ReturnRuleList(string text, string path)
        {
            string key = $"Edit|RuleList_Set_";//{text}_+path
            string str =  AddLink("ClearIO", "Back") ;
            
                if(text =="Rule")
                    path = "TaypId*" + path;
                else if (text == "RuleTag")
                    path = "Tayp*" + path;
                else if (text == "Forse")
                {
                    string[] com = path.Split('?');
                    path = text + "*" + com[1];
                }
                else
                    path = text + "*" + path;

                switch (text)
                {
                    case ("Trigger"):
                        for (int i = 0; i < core.frame.Trigger.Length; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Add {core.frame.Trigger[i]}");// " Add|-1 null";
                        break;

                    case ("Tayp"):
                        str += $"\n" + AddLink($"{key}-1_{path}", $"Set Null");
                        for (int i = 0; i < core.bD.Count; i++)//if(i != key(нужное значение) для исключения определенных блоков выборки
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.bD[i].Name}");
                        break;
                    case ("TaypId"):
                        Debug.Log(path);
                        Debug.Log(core.keyStat);
                        string[] com = path.Split('?');
                        int a = int.Parse(com[3]);
                        if (a == core.keyStat)
                            for (int i = 0; i < core.frame.SysStat.Length; i++)
                                str += $"\n" + AddLink($"{key}-{i+1}_{path}", $"Set {core.frame.SysStat[i]}");

                       // if (a > -1)
                            for (int i = 0; i < core.bD[a].Base.Count; i++)
                                str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.bD[a].Base[i].Name}");
                      //  else
                        //    for (int i = 0; i < core.head[-a-1].Rule.Count; i++)
                         //       str += $"\n" + AddLink($"{key}{i}_{com[2]}", $"Set { core.head[-a].Rule[i]}");
                        break;
                    case ("Card"):
                        for (int i = 0; i < core.frame.CardString.Length; i++)
                            str += $"\n" + AddLink($"{key}-{i + 1}_{path}", $"Set { core.frame.CardString[i]}");

                        //core.frame.CardString[-(coreForm.Card + 1)]
                      //  for (int i = 0; i < core.bD[keyAssociation].Base.Count; i++)
                      //      str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.bD[keyAssociation].Base[i].Name}");
                        break;
                    //case ("TaregtPlayer"):
                    //    for (int i = 0; i < core.frame.PlayerString.Length; i++)
                    //        str += $"\n" + AddLink($"Edit|TaregtPlayer_{i}_{path}", $"Set { core.frame.PlayerString[i]}");
                    //    break;
                    case ("Forse"):
                        for (int i = 0; i < core.frame.ForseTayp.Length; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.frame.ForseTayp[i]}");
                        break;
                    case ("Action"):
                        //str += $"\n Add|-1 null";

                        for (int i = 0; i < core.frame.Action.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.frame.Action[i].Name}");
                        break;
                    case ("ActionExtend"):
                        Debug.Log(path);
                        com = path.Split('*');
                        a = int.Parse(com[2]);
                        //str += $"\n Add|-1 null";

                        for (int i = 0; i < core.frame.Action[a].Extend.Length; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.frame.Action[a].Extend[i]}");
                        break;
                    case ("Plan"):
                        //ForseTayp
                        str += $"\n Add|-1 AllPlans";
                        for (int i = 0; i < core.bD[core.keyPlan].Base.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.bD[core.keyPlan].Base[i].Name}");
                        break;
                    case ("RuleTag"):
                        for (int i = 0; i < core.head.Count; i++)
                            if (core.head[i].Rule.Count > 0)
                                str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.head[i].Name} ({core.head[i].Rule.Count})");
                        break;
                    case ("Rule"):
                        Debug.Log(path);
                        com = path.Split('?');
                        a = int.Parse(com[3]);
                        for (int i = 0; i < core.head[a].Rule.Count; i++)
                            str += $"\n" + AddLink($"{key}{i}_{path}", $"Set { core.head[a].Rule[i]}");
                        break;
                }
            ui.TT[0].text =str;
        }


        static string ReturnCore(RuleForm coreForm , string path, bool result = false, int rule = -1)
        {

            string str = "", text ="";
            if (coreForm.Card < 0)
                text = $"{core.frame.CardString[-(coreForm.Card +1)]}";
            //else
                //text = $"{core.bD[keyAssociation].Base[coreForm.Card].Name}";

            
            str += AddLink($"Edit|RuleList_Return_Card_{path}" , text);

            if (rule != -1)
            {
                str += $" " + AddLink($"Edit|RuleList_Return_RuleTag_{path}", $"{core.head[coreForm.Tayp].Name}");
                str += $" " + AddLink($"Edit|RuleList_Return_Rule_{path}?{coreForm.Tayp}", core.head[coreForm.Tayp].Rule[coreForm.TaypId]);

                if (rule == 0)
                {
                    str += TextEditInt("Mod?" + path, "" + coreForm.Mod);
                    str += TextEditInt("Num?" + path, "" + coreForm.Num);
                }
            }
            else
            {
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
                    if (coreForm.Tayp == core.keyStat && coreForm.TaypId < 0)
                        text = core.frame.SysStat[-coreForm.TaypId-1];
                    else
                        text = $" {core.bD[coreForm.Tayp].Base[coreForm.TaypId].Name}";


                    str += $" " + AddLink($"Edit|RuleList_Return_TaypId_{path}?{coreForm.Tayp}", text);
                    if (coreForm.Tayp == core.keyStat)
                    {
                        str += TextEditInt("Mod?" + path, "" + coreForm.Mod);
                        str += TextEditInt("Num?" + path, "" + coreForm.Num);
                    }
                }
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
            for (int i = 1; i < 160; i++)
                str += "\n" + AddLinkIcon(i);

            return str;

        }

        static string AddLinkIcon(int i)
        {
            return $"<link=SetSwitch|Icon_{i}><sprite index={i}></link>";
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
        static MainBase NewMainBase(int a)
        {
            MainBase mainBase = new MainBase();
            if(a== core.keyStat)
            {
                mainBase.Sub = new MainBaseSubInt();
                mainBase.Sub.AntiStat = new List<int>();
                mainBase.Sub.DefStat = new List<int>();
            }
            else if (a == core.keyRace)
            {
                mainBase.Race = new MainBaseSubRace();
                //mainBase.Race.MainRace
            }
            else if(a == core.keyStatGroup)
            {
                mainBase.Group = new MainBaseStatGroup();
            }
            else if(a == core.keyPlan)
            {
                mainBase.Plan = new MainBaseStatPlan();
            }

            mainBase.accses = new Accses();
            //for (int i = 0; i < core.bD[keyA].Key.Count; i++)
            //    mainBase.Text.Add(new SubText());
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

        static void NewMainRule(int a)
        {
            mainRule = new HeadRule();
            mainRule.Tag = a;
            //mainRule.Name = "Void";

            mainRule.Trigger = new List<TriggerAction>();

            mainRule.accses = new Accses();
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
    }

    public class MainBase
    {
        //public int SysName = " ";
        //public string Tayp;
        public string Name = "Void";
        public string Color = "ffff00";
        public string Info = "Void";
        public int Cost;
        public bool Visible;

        public Accses accses = new Accses();
        public MainBaseSubInt Sub;
        public MainBaseSubRace Race;
        public MainBaseStatGroup Group;
        public MainBaseStatPlan Plan;

        public bool Look= true;

    }
    public class MainBaseSubRace
    {
        public int MainStat = 0;
        public int MainRace = -1;
        public List<int> UseRace = new List<int>();
    }
    public class MainBaseSubInt
    {
        //public bool Regen;
        public int Image = 0;
        public int Antipod = -1;
        public List<int> AntiStat = new List<int>();
        public List<int> DefStat = new List<int>();
    }
    public class MainBaseStatGroup
    {
        public int MainSize =1;
        public List<int> Stat = new List<int>();
        public List<int> Size = new List<int>();
    }
    public class MainBaseStatPlan
    {
        public int Size;
    }

    public class Accses
    {
        public List<SubInt> Like = new List<SubInt>();
        public List<SubInt> Need = new List<SubInt>();
        public List<SubInt> DisLike = new List<SubInt>();
        public List<SubInt> Use = new List<SubInt>();
        public List<SubInt> Mark = new List<SubInt>();
        public Accses() {  }
        public Accses(Accses accses)
        {
            for (int i = 0; i < accses.Like.Count; i++)
            {
                Like.Add(new SubInt(accses.Like[i].Head));
                for (int j = 0; j < accses.Like[i].Num.Count; j++)
                    Like[i].Num.Add(new SubInt(accses.Like[i].Num[j].Head));
            }

            for (int i = 0; i < accses.Need.Count; i++)
            {
                Need.Add(new SubInt(accses.Need[i].Head));
                for (int j = 0; j < accses.Need[i].Num.Count; j++)
                    Need[i].Num.Add(new SubInt(accses.Need[i].Num[j].Head));
            }
            for (int i = 0; i < accses.DisLike.Count; i++)
            {
                DisLike.Add(new SubInt(accses.DisLike[i].Head));
                for (int j = 0; j < accses.DisLike[i].Num.Count; j++)
                    DisLike[i].Num.Add(new SubInt(accses.DisLike[i].Num[j].Head));
            }
        }
        public Accses(string str)
        {
            SubInt subInt = new SubInt(str, 4);
            Like = subInt.Num[0].Num;
            //Debug.Log(Like.Count);
            Need = subInt.Num[1].Num;
            DisLike = subInt.Num[2].Num;
            Use = subInt.Num[3].Num;
            Mark = subInt.Num[4].Num;
        }

        public void Edit(string mood,int a,int b, bool add)
        {
            List<SubInt> list = null;
            switch (mood)
            {
                case ("Like"):
                    list = Like;
                    break;
                case ("Need"):
                    list = Need;
                    break;
                case ("DisLike"):
                    list = DisLike;
                    break;
                case ("Use"):
                    list = Use;
                    break;
                case ("Mark"):
                    list = Mark;
                    break;
            }

            a = Find(list, a, add);
            if (a != -1)
                if (b < 0)
                {
                    if(!add)
                        list.RemoveAt(a);
                }
                else
                {
                    list[a].Edit(b, add);
                   // if (mood == "Use")
                    //    if (list[a].Num.Count == 0)
                    //        list.RemoveAt(a);
                }
        }

        public string Zip()
        {
            SubInt subInt = new SubInt(0);
            for(int i=0;i<5;i++)
                subInt.Num.Add(new SubInt(0));

            subInt.Num[0].Num = Like;
            subInt.Num[1].Num = Need;
            subInt.Num[2].Num = DisLike;
            subInt.Num[3].Num = Use;
            subInt.Num[4].Num = Mark;

            return subInt.Zip(3);
        }

        public void ClearList()
        {
            void clire(List<SubInt> list, bool use = false)
            {
                for (int i = 0; i < list.Count; i++)
                    if (list[i].Head >= 0)
                    {
                        if (list[i].Num.Count == 0)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                    else if (use)
                    {
                        if (list[i].Num.Count == 0)
                        {
                            int a = Find(Need, list[i].Head, false);
                            if (a != -1)
                                if (Need[a].Num.Count == 0)
                                    Need.RemoveAt(a);

                            a = Find(Like, list[i].Head, false);
                            if (a != -1)
                                if (Like[a].Num.Count == 0)
                                    Like.RemoveAt(a);
                        }
                    }
            }
            clire(Like);
            clire(Need);
            clire(DisLike, true);
        }

        void Reset()
        {
            Like = new List<SubInt>();
            Need = new List<SubInt>();
            DisLike = new List<SubInt>();
        }
        List<int> intGuild, intLegion, intRace, intCivilian, intCardTayp, intCardClass, intStat, intRule;
        //List<SubInt> intRule;

        public List<int> ReturnAccses(string mood)
        {
            switch (mood)
            {
                case ("Guild"):
                    return intGuild;
                    break;
                case ("Legion"):
                    return intLegion;
                    break;
                case ("Race"):
                    return intRace;
                    break;
                case ("Civilian"):
                    return intCivilian;
                    break;
                case ("CardTayp"):
                    return intCardTayp;
                    break;
                case ("CardClass"):
                    return intCardClass;
                    break;
                case ("Stat"):
                    return intStat;
                    break;
               
            }
            return null;
        }

        public void AccsesComplite()
        {
            CoreSys core = DeCoder.GetCore();

            intGuild = new List<int>();
            intLegion = new List<int>();
            intRace = new List<int>();
            intCivilian = new List<int>();

            intCardTayp = new List<int>();
            intCardClass = new List<int>();
            intStat = new List<int>();
            intRule = new List<int>();

            void CountData(List<int> list, int i)
            {
                for (int j = 0; j < DisLike[i].Num.Count; j++)
                    list.Add(DisLike[i].Num[j].Head);
            }
            for (int i = 0; i < DisLike.Count; i++)
            {
                if (DisLike[i].Head < 0)  {  intRule.Add(i);  continue;  }

                if (DisLike[i].Head == core.keyStat) { CountData(intStat, i); continue; }


                if (DisLike[i].Head == core.keyRace) { CountData(intRace, i); continue; }
                if (DisLike[i].Head == core.keyLegion) { CountData(intLegion, i); continue; }
                if (DisLike[i].Head == core.keyCivilian) { CountData(intCivilian, i); continue; }

                if (DisLike[i].Head == core.keyCardTayp) { CountData(intCardTayp, i); continue; }
                if (DisLike[i].Head == core.keyCardClass) { CountData(intCardClass, i); continue; }

                if (DisLike[i].Head == core.keyGuild) { CountData(intGuild, i); continue; }
            }
        }
        public bool AccsesCard(CardCase card)
        {
            for (int i = 0; i < intGuild.Count; i++)
                if (card.Guild == intGuild[i])
                    return false;

            for (int i = 0; i < intLegion.Count; i++)
                if (card.Legion == intLegion[i])
                    return false;

            for (int i = 0; i < intRace.Count; i++)
                if (card.Race == intRace[i])
                    return false;

            for (int i = 0; i < intCivilian.Count; i++)
                if (card.Civilian == intCivilian[i])
                    return false;


            for (int i = 0; i < intCardTayp.Count; i++)
                if (card.CardTayp == intCardTayp[i])
                    return false;

            for (int i = 0; i < intCardClass.Count; i++)
                if (card.CardClass == intCardClass[i])
                    return false;


            for(int j=0;j< card.Stat.Count;j++)
                for (int i = 0; i < intStat.Count; i++)
                    if (card.Stat[j].Get("Stat") == intStat[i])
                        return false;


            for (int i = 0; i < intRule.Count; i++)
                for (int j = 0; j < card.Trait.Count; j++)
                    if (card.Trait[j].Head == DisLike[intRule[i]].Head)
                        if (DisLike[intRule[i]].Num.Count == 0)
                            return false;
                        else
                            for (int k = 0; k < DisLike[intRule[i]].Num.Count; k++)
                                for (int h = 0; h < card.Trait[j].Num.Count; h++)
                                    if (card.Trait[j].Num[h].Head == DisLike[intRule[i]].Num[k].Head)
                                        return false;


            return true;
        }
        public int SplitCard( CardCase card, Accses coreAccses)
        { 
            /*
         Принадлежность карты
Гильдия или наёмники
Легион
Раса
Соц группа
Тип карты
Раздел механик
Статы
         */
            void AddRace(int a, CoreSys core)
            {
                if(a != -1)
                {
                    Split(core.bD[core.keyRace].Base[a].accses);
                    AddRace(core.bD[core.keyRace].Base[a].Race.MainRace, core);
                }
            }

            CoreSys core = DeCoder.GetCore();
            Reset();
            int s = -1,a;
            //stat
            for(int i = 0; i < card.Stat.Count; i++)
                Split(core.bD[core.keyStat].Base[card.Stat[i].Get("Stat")].accses);

            //tag and trait
            for (int i = 0; i < card.Trait.Count; i++)
            {
                a = -card.Trait[i].Head - 1;
                Split(core.bD[core.keyStat].Base[a].accses);
                for(int j = 0; j < core.head[a].Rule.Count;j++)
                {
                    Split(Saver.LoadRuleAccses(a, j));
                }
            }

            if(coreAccses == null)
            {
                Split(core.bD[core.keyCardTayp].Base[card.CardTayp].accses);
                Split(core.bD[core.keyCivilian].Base[card.Civilian].accses);

                Split(core.bD[core.keyRace].Base[card.Race].accses);
                AddRace(core.bD[core.keyRace].Base[card.Race].Race.MainRace, core);


                Split(core.bD[core.keyLegion].Base[card.Legion].accses);
                Split(core.bD[core.keyGuild].Base[card.Guild].accses);
                Split(core.bD[core.keyCardClass].Base[card.CardClass].accses);
            }
            else
                Split(coreAccses);

            //stat->trait->legion->race->guild->civilian
            //>> keyStat >> keyTag + keyRule >> keyLegion >>keyRace >> keyGuild >> keySocial >> keyCardTayp >> keyCardClass


            return s;
        }
        void Error(string str)
        {
            Debug.Log($"ERRoR:{str} Need Creator Accses Fall");
        }
        public void SplitLite(Accses accses)
        {
            int a;
            for (int i = 0; i < accses.DisLike.Count; i++)
                if (accses.DisLike[i].Num.Count == 0)
                {
                    a = Find(DisLike, accses.DisLike[i].Head, false);
                    if (a != -1)
                    {
                        if (DisLike[a].Num.Count != 0)
                            DisLike[a].Num = new List<SubInt>();
                    }
                    else
                        Find(DisLike, accses.DisLike[i].Head);

                }
                else
                {
                    int c = 0;
                    if (accses.DisLike[i].Head < 0)
                    {
                        c = -accses.DisLike[i].Head - 1;
                        c = DeCoder.GetCore().head[c].Rule.Count;
                    }
                    else
                        c = DeCoder.GetCore().bD[accses.DisLike[i].Head].Base.Count;

                    a = Find(DisLike, accses.DisLike[i].Head);
                    if (a != -1)
                        for (int j = 0; j < accses.DisLike[i].Num.Count; j++)
                        {
                            DisLike[a].Find(accses.DisLike[i].Num[j].Head);
                            if (c != 0)
                                if (DisLike[a].Num.Count == c)
                                    DisLike[a].Num = new List<SubInt>();
                        }

                }
        }

        public int Split(Accses accses)
        {
            CoreSys core = DeCoder.GetCore();
            int a, b;
            //блок блокировки доступа
            for(int i = 0; i < accses.DisLike.Count; i++)
            {
                if (accses.DisLike[i].Num.Count == 0)
                {

                    a = Find(Like, accses.DisLike[i].Head, false);
                    if (a != -1)
                        Like.RemoveAt(a);

                    a = Find(DisLike, accses.DisLike[i].Head, false);
                    if (a != -1)
                    {
                        if (DisLike[a].Num.Count != 0)
                            DisLike[a].Num = new List<SubInt>();
                    }
                    else
                        Find(DisLike, accses.DisLike[i].Head);

                }
                else
                {
                    a = Find(Need, accses.DisLike[i].Head, false);
                    if (a != -1)
                        for (int j = 0; j < accses.DisLike[i].Num.Count;j++)
                        {
                            b = Need[a].Find(accses.DisLike[i].Num[j].Head, false);
                            if(b != -1)
                            {
                                string str = $"(Нарушение достпа необходимо сбросить предыдущий слой требующий этот доступ)Вы пытаеть блокировать доступ к требуему экземпляру раздела{accses.DisLike[i].Head} {accses.DisLike[i].Head}";

                                //for (int j = 0; j < Need[a].Num[b].Num.Count; j++) 
                                //{
                                //    str += $"\nраздел {core.bD[Need[a].Num[b].Num[j].Head].Name}";
                                //    for (int k = 0; k < Need[a].Num[b].Num[j].Num.Count; k++)
                                //    {
                                //        str += $"\n     элемент {core.bD[Need[a].Num[b].Num[j].Num[k].Head].Name}";
                                //    } 
                                //}

                                    Error(str);
                                return accses.DisLike[i].Head;
                            }
                        }


                    int c = 0;
                    if (accses.DisLike[i].Head <0)
                    {
                        c = -accses.DisLike[i].Head - 1;
                        c = core.head[c].Rule.Count;
                    }
                    else
                        c = core.bD[accses.DisLike[i].Head].Base.Count;

                    a = Find(DisLike, accses.DisLike[i].Head);
                    if (a != -1)
                        for (int j = 0; j < accses.DisLike[i].Num.Count; j++)
                        {
                            DisLike[a].Find(accses.DisLike[i].Num[j].Head);
                            if(c !=0)
                                if(DisLike[a].Num.Count == c)
                                    DisLike[a].Num = new List<SubInt>();
                        }



                    a = Find(Like, accses.DisLike[i].Head, false);
                    if (a != -1)
                        if (Like[a].Num.Count == 0)
                        {
                             if(DisLike[a].Num.Count ==0)
                                Like.RemoveAt(a);
                        }
                        else
                            for (int j = 0; j < accses.DisLike[i].Num.Count; j++)
                            {
                                b = Like[a].Find(accses.DisLike[i].Num[j].Head, false);
                                if (b != -1)
                                    Like[a].Num.RemoveAt(b);
                                if (Like[a].Num.Count == 0)
                                {
                                    Like.RemoveAt(a);
                                    break;
                                }
                            }
                }


            }


            for (int i = 0; i < accses.Need.Count; i++)
            {
                if (accses.Need[i].Num.Count == 0)
                {
                    Find(Need,accses.Need[i].Head);
                }
                else
                {
                    a = Find(Need,accses.Need[i].Head);
                    for (int j = 0; j < accses.Need[i].Num.Count; j++)
                        Need[a].Find(accses.Need[i].Num[j].Head);
                }
            }

            for (int i = 0; i < accses.Like.Count; i++)
            {
                if (accses.Like[i].Num.Count == 0)
                {
                    a = Find(Like,accses.Like[i].Head);
                    if(Like[a].Num.Count >0)
                        Like[a].Num = new List<SubInt>();
                }
                else
                {
                    int c = 0;
                    if (accses.Like[i].Head < 0)
                    {
                        c = -accses.Like[i].Head - 1;
                        c = core.head[c].Rule.Count;
                    }
                    else
                        c = core.bD[accses.Like[i].Head].Base.Count;

                    b = Find(DisLike,accses.Like[i].Head, false);
                    a = Find(Like,accses.Like[i].Head);
                    for (int j = 0; j < accses.Like[i].Num.Count; j++)
                    {
                        Like[a].Find(accses.Like[i].Num[j].Head);
                        if (Like[a].Num.Count == c)
                            Like[a].Num = new List<SubInt>();

                        if(b!= -1)
                        {
                            int d = DisLike[b].Find(accses.Like[i].Num[j].Head, false);
                            if (d != -1)
                                DisLike[b].Num.RemoveAt(d);
                        }

                    }

                    if (Like[a].Num.Count ==0)
                        if (DisLike[b].Num.Count == 0)
                            DisLike.RemoveAt(b);
                }
            }


            return -1;
        }
        public int Find(List<SubInt> list,int a, bool rewrite = true)
        {
            int i = list.FindIndex(x => x.Head == a);
            if(rewrite)
                if (i == -1)
                {
                    list.Add(new SubInt(a));
                    i = list.Count - 1;
                }
            return i;
        }
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
        public List<int> Index = new List<int>();
        public List<int> Cost = new List<int>();
        //public int Cost;//Цена
        public int LastIndex = 0;
    }

    public class HeadRule
    {
       // public string Name;//Название
       // public string Info = "Void";//Описание
        public int Tag; //Описание

        public bool Visible;
        public bool VisibleCard =true;
        //public int Cost;//Цена

        public List<TriggerAction> Trigger;// = new List<TriggerAction>();

        public Accses accses;
      //  public List<SubIntLite> NeedRule;// = new List<string>();
      //  public List<SubIntLite> EnemyRule;//= new List<string>();
    }

    public class TriggerAction
    {

        public int Plan =-1;
        public int Trigger;
        //public int TriggerExtend;

        public bool CountMod;
        public bool CountModExtend;
        // public int Id;
        public int Team;


        public List<IfAction> PlusAction = new List<IfAction>();
        public List<IfAction> MinusAction = new List<IfAction>();

        public List<RuleAction> Action = new List<RuleAction>();
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
        public RuleForm(int a =-1 )
        {
            Tayp = a;
        }

        public int Card = -1;//для конкретных значений использеются отрицательны значения, в противном случае используется асоциация "Null";//0-null 1-card1 2-card2
        public int Tayp; 
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
        public int Prioritet;

        public int Team;

        //public int RuleTag;//Работают через действие 
        //public int Rule;
        public List<RuleForm> Core = new List<RuleForm>();
        public RuleForm ResultCore = new RuleForm();
        public int ForseMood;

    }
   

    #endregion
}
