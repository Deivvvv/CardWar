using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;
using TMPro;

using UnityEditor;
using UnityEngine.TextCore;
using UnityEngine.U2D;
using System.Linq;

using Coder;
//using SubSys;




namespace XMLSaver
{
    static class Saver
    {
        //static XElement root = null;
        static string mainPath = Application.dataPath + $"/Resources/";
        static CoreSys core;
        public static void SetCore(CoreSys coreSys) { core = coreSys; }
        static string v = "V1";

        public static void BackUpSave(string mood)
        {
            string path = mainPath + "backUp/";
            switch (mood)
            {
                case ("BD"):
                    break;
                case ("Rule"):
                    break;
            }

        }
        public static void BackUpLoad(string mood)
        {
            string path = mainPath + "backUp/";
            switch (mood)
            {
                case ("BD"):
                    break;
                case ("Rule"):
                    break;
            }

        }
        public static void BackUpAllLoad(string mood)
        {
            switch (mood)
            {
                case ("BD"):
                    break;
                case ("Rule"):
                    break;
            }
        }

        #region Lang
        static string lang;
        public static void LoadDataLang(string str) { lang = str; }
        static string FindLang(string path, int a)
        {
            XElement root = null;
            //string str = $"{path}/{lang}/{a}.L" 

            // Debug.Log($"{path}L/{lang}/{a}.L");
            if (File.Exists($"{path}L/{lang}/{a}.L"))
                root = XDocument.Parse(File.ReadAllText($"{path}/L/{lang}/{a}.L")).Element("root");
            else if (File.Exists($"{path}L/Eng/{a}.L"))
                root = XDocument.Parse(File.ReadAllText($"{path}/L/Eng/{a}.L")).Element("root");

            string str = "";
            if (root != null)
                str = root.Element("I").Value;

            // Debug.Log(str);
            return str;

        }
        static void AddLangFile(string path, string str, int a)
        {
            //Debug.Log(path);
            path = $"{path}L/{lang}/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            XElement root = new XElement("root");

            root.Add(new XElement("I", str));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}{a}.L", saveDoc.ToString());
        }
        static void RemoveWord(string path, int a)
        {
            XElement root = null;
            string str = "";
            string[] com1;
            List<string> com;

            str = FindLang(path, a);
            int f = str.Split('/').Length;
            com1 = Directory.GetFiles($"{path}L/", "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < com1.Length; i++)
            {
                Debug.Log(com1[i]);
                path = $"{com1[i]}/{a}.L";
                if (File.Exists(path))
                {
                    root = XDocument.Parse(File.ReadAllText(path)).Element("root");
                    str = root.Element("I").Value;
                    com = new List<string>(str.Split('/'));
                    if (com.Count >= f)
                    {
                        com.RemoveAt(a);
                        if (com.Count == 0)
                            File.Delete(path);
                        else
                        {
                            str = com[0];
                            for (int i1 = 1; i1 < com.Count; i1++)
                                str += "/" + com[i1];

                            root = new XElement("root");

                            root.Add(new XElement("I", str));

                            XDocument saveDoc = new XDocument(root);
                            File.WriteAllText(path, saveDoc.ToString());
                        }

                    }
                }

            }
        }
        static void RemoveLocal(string path, int a)
        {
            string[] com = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < com.Length; i++)
            {
                path = $"{com[i]}/{a}.L";
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        static string GetWord(string path, int a)
        {
            string str1 = "", str2 = "";
            //TranfWord()
            return str1 + "*" + str2;
        }

        static void TranfWord(string path, string text, int a)
        {
            string[] com = text.Split('*');
            string[] lang = com[0].Split('/');
            com = com[1].Split('/');
            XElement root;
            string str = "";
            for (int i = 0; i < lang.Length; i++)
            {
                str = $"{path}L/{lang[i]}/{a}.L";
                if (File.Exists(path))
                {
                    root = XDocument.Parse(File.ReadAllText(path)).Element("root");
                    str = root.Element("I").Value;
                    str += "/" + com[i];
                }
                else
                    str = com[i];

                root = new XElement("root");

                root.Add(new XElement("I", str));

                XDocument saveDoc = new XDocument(root);
                File.WriteAllText(path, saveDoc.ToString());

            }
        }
        #endregion

        public static void Reload()
        {
            for (int i = 0; i < core.bD.Count; i++)
                for (int j = 0; j < core.bD[i].Base.Count; j++)
                    SaveBD(i, j);

            for (int i = 0; i < core.head.Count; i++)
                for (int j = 0; j < core.head[i].Index.Count; j++)
                {
                    HeadRule rule = LoadRule(i, core.head[i].Index[j]);
                    SaveRule(rule, i, core.head[i].Index[j]);
                }
        }
        #region BD

        static void BDReload(int a)
        {
            core.bD = new List<BD>(new BD[core.frame.Tayp.Count]);
            string path = mainPath + "BD/";
            for (; a < core.frame.Tayp.Count; a++)
                if (!Directory.Exists($"{path}{a}/"))
                {
                    Directory.CreateDirectory($"{path}{a}/");
                    core.bD[a] = new BD();
                    core.bD[a].Name = core.frame.Tayp[a];
                    SaveBDMain(a);
                }
        }

        public static void LoadBDAll()
        {
            mainPath = Application.dataPath + $"/Resources/{v}/";
            int l = core.frame.Tayp.Count;
            List<BD> bD = new List<BD>(new BD[l]);
            string path = mainPath + "BD/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            if (fCount + 1 < l)
                BDReload(fCount);

            string[] com;
            string str = "";
            BD bd = null;
            core.bD = bD;
            for (int i = 0; i < l; i++)
            {
                bd = new BD();
                str = FindLang($"{path}{i}/", -1);
                com = str.Split('/');
                bd.Name = com[0];
                bd.Info = com[1];

                bD[i] = bd;
                // com = Directory.GetFiles($"{path}{i}/", "*.H");
                fCount = Directory.GetFiles($"{path}{i}/", "*.H").Length;
                bd.Base = new List<MainBase>(new MainBase[fCount]);
                for (int i1 = 0; i1 < fCount; i1++)
                    LoadBD(i, i1);


            }


        }
        public static void SaveBDMain(int a)
        {
            string path = mainPath + $"BD/{a}/";
            string str = $"{core.bD[a].Name}/{core.bD[a].Info}";
            //Debug.Log(str);
            AddLangFile(path, str, -1);
        }

        public static void LoadBD(int a, int b)
        {
            string path = mainPath + $"BD/{a}/";
            string[] com;
            string str = "";
            MainBase mainBase = new MainBase();
            XElement root = XDocument.Parse(File.ReadAllText($"{path}{b}.H")).Element("root");

            mainBase.Color = root.Element("Color").Value;
            mainBase.Cost = int.Parse(root.Element("Cost").Value);
            mainBase.Look = bool.Parse(root.Element("Look").Value);
            mainBase.Visible = bool.Parse(root.Element("Visible").Value);

            if (a == core.keyStat)
            {
                mainBase.Sub = new MainBaseSubInt();
                //mainBase.Sub.Regen = bool.Parse(root.Element("Regen").Value);

                mainBase.Sub.Image = int.Parse(root.Element("Image").Value);
                mainBase.Sub.Antipod = int.Parse(root.Element("Antipod").Value);
                str = root.Element("AntiStat").Value;
                if (str != "")
                    mainBase.Sub.AntiStat = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                str = root.Element("DefStat").Value;
                if (str != "")
                    mainBase.Sub.AntiStat = new List<int>(str.Split('/').Select(int.Parse).ToArray());
            }
            else if (a == core.keyRace)
            {
                mainBase.Race = new MainBaseSubRace();
                mainBase.Race.MainStat = int.Parse(root.Element("MainStat").Value);
                mainBase.Race.MainRace = int.Parse(root.Element("MainRace").Value);
                str = root.Element("UseRace").Value;
                if (str != "")
                    mainBase.Race.UseRace = new List<int>(str.Split('/').Select(int.Parse).ToArray());
            }
            else if (a == core.keyStatGroup)
            {
                mainBase.Group = new MainBaseStatGroup();
                mainBase.Group.MainSize = int.Parse(root.Element("MainSize").Value);
                str = root.Element("Stat").Value;
                if (str != "")
                    mainBase.Group.Stat = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                str = root.Element("Size").Value;
                if (str != "")
                    mainBase.Group.Size = new List<int>(str.Split('/').Select(int.Parse).ToArray());
            }
            else if (a == core.keyPlan)
            {
                mainBase.Plan = new MainBaseStatPlan();
                mainBase.Plan.Size = int.Parse(root.Element("Size").Value);
            }


            mainBase.accses = new Accses(root.Element("Accses").Value);

            str = FindLang(path, b);
            if (str != "")
            {
                com = str.Split('/');
                mainBase.Name = com[0];
                mainBase.Info = com[1];
            }

            core.bD[a].Base[b] = mainBase;
        }
        static string ReturnListData(List<bool> list)
        {
            string str = " ";
            if (list.Count > 0)
            {
                str = "" + list[0];
                for (int i = 1; i < list.Count; i++)
                    str += $"/{list[i]}";
            }
            return str;
        }
        static string ReturnListData(List<int> list)
        {
            string str = " ";
            if (list.Count > 0)
            {
                str = "" + list[0];
                for (int i = 1; i < list.Count; i++)
                    str += $"/{list[i]}";
            }
            return str;
        }
        static string ReturnListData(List<string> list)
        {
            string str = " ";
            if (list.Count > 0)
            {
                str = "" + list[0];
                for (int i = 1; i < list.Count; i++)
                    str += $"/{list[i]}";
            }
            return str;
        }

        public static void SaveBD(int a, int b)
        {
            if (b == -1)
                return;

            string[] com = null;
            string str = "";
            string path = mainPath + $"BD/{a}/";
            MainBase mainBase = core.bD[a].Base[b];

            mainBase.accses.ClearList();

            str = $"{mainBase.Name}/{mainBase.Info}";
            AddLangFile(path, str, b);

            XElement root = new XElement("root");

            root.Add(new XElement("Color", mainBase.Color));
            root.Add(new XElement("Cost", mainBase.Cost));
            root.Add(new XElement("Look", mainBase.Look));
            root.Add(new XElement("Visible", mainBase.Visible));

            root.Add(new XElement("Accses", mainBase.accses.Zip()));

            if (a == core.keyStat)
            {
                //root.Add(new XElement("Regen", mainBase.Sub.Regen));
                root.Add(new XElement("Image", mainBase.Sub.Image));

                root.Add(new XElement("Antipod", mainBase.Sub.Antipod));

                root.Add(new XElement("AntiStat", ReturnListData(mainBase.Sub.AntiStat)));
                root.Add(new XElement("DefStat", ReturnListData(mainBase.Sub.DefStat)));
            }
            else if (a == core.keyRace)
            {
                root.Add(new XElement("MainStat", mainBase.Race.MainStat));
                root.Add(new XElement("MainRace", mainBase.Race.MainRace));
                root.Add(new XElement("UseRace", ReturnListData(mainBase.Race.UseRace)));
            }
            else if (a == core.keyStatGroup)
            {
                root.Add(new XElement("MainSize", mainBase.Group.MainSize));

                root.Add(new XElement("Stat", ReturnListData(mainBase.Group.Stat)));
                root.Add(new XElement("Size", ReturnListData(mainBase.Group.Size)));
            }
            else if (a == core.keyPlan)
            {
                root.Add(new XElement("Size", mainBase.Plan.Size));
            }


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"{b}.H", saveDoc.ToString());
        }



        #endregion

        #region Rule
        public static void RuleAdd()
        {
            int a = core.head.Count;
            string path = mainPath + "Rule/";
            //if (!Directory.Exists($"{path}{a}/"))
            // {
            Directory.CreateDirectory($"{path}{a}/");
            core.head.Add(new SubRuleHead());
            SaveRuleMain(a);
            // }
            LoadAllRule();
        }

        public static void LoadAllRule()
        {
            string[] com = null, com1 = null;
            string str = "";
            string path = mainPath + "Rule/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //int f = DeCoder.ReturnIndex("Tag");

            // int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            // if (fCount <= core.bD[f].Base.Count)
            //     RuleReload( fCount, core.bD[f].Base.Count);

            int a = 0, b = 0;
            List<SubRuleHead> head = new List<SubRuleHead>(new SubRuleHead[core.bD[core.keyTag].Base.Count]);
            for (int i = 0; i < head.Count; i++)
            {
                head[i] = new SubRuleHead();
                head[i].Name = core.bD[core.keyTag].Base[i].Name;
                XElement root = XDocument.Parse(File.ReadAllText($"{path}{i}.HR")).Element("root");
                str = root.Element("Id").Value;
                if (str != "")
                    head[i].Index = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                else
                    head[i].Index = new List<int>();
                str = root.Element("Cost").Value;
                if (str != "")
                    head[i].Cost = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                else
                    head[i].Cost = new List<int>();


                b = head[i].Index.Count;
                if (b > 0)
                {
                    head[i].Rule = new List<string>(new string[b]);
                    head[i].LastIndex = head[i].Index[b - 1] + 1;

                    str = FindLang($"{path}", i);
                    com = str.Split('/');

                    a = com.Length;
                    if (a < b)
                    {
                        string oldLang = lang;
                        lang = "Eng";
                        str = FindLang($"{path}{i}", -1);
                        com = str.Split('/');

                        for (int i1 = a; i1 < b; i1++)
                            head[i].Rule[i1] = com[i1];

                        lang = oldLang;
                        str = FindLang($"{path}{i}", -1);
                        com = str.Split('/');
                    }

                    // Debug.Log(com.Length);
                    // Debug.Log(head[i].Rule.Count);
                    for (int i1 = 0; i1 < com.Length; i1++)
                        head[i].Rule[i1] = com[i1];

                    if (com.Length != a)
                        SaveRuleMain(i);
                }
                else
                    head[i].Rule = new List<string>();

            }

            core.head = head;
        }
        public static void SaveRuleMain(int a)
        {
            Debug.Log(a);
            Debug.Log(core.head.Count);
            SubRuleHead head = core.head[a];
            string path = mainPath + "Rule/";
            XElement root = new XElement("root");

            string str2 = " ";
            if (head.Index.Count > 0)
            {
                string str1 = "" + head.Index[0];
                str2 = head.Rule[0];
                string str3 = "" + head.Cost[0];

                for (int i = 1; i < head.Index.Count; i++)
                {
                    str1 += "/" + head.Index[i];
                    str2 += "/" + head.Rule[i];
                    str3 += "/" + head.Cost[i];
                }
                root.Add(new XElement("Id", str1));
                root.Add(new XElement("Cost", str3));
            }
            else
            {
                root.Add(new XElement("Id", " "));
                root.Add(new XElement("Cost", " "));
            }

            AddLangFile(path, str2, a);

            Debug.Log(path);

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"{a}.HR", saveDoc.ToString());

        }

        static RuleForm ReturnCore(string str)
        {
            string[] com = str.Split('*');
            RuleForm core = new RuleForm();

            core.Card = int.Parse(com[0]);
            core.Tayp = int.Parse(com[1]);
            core.TaypId = int.Parse(com[2]);
            core.Mod = int.Parse(com[3]);
            core.Num = int.Parse(com[4]);

            return core;
        }
        static IfAction ReturnIfAction(string str)
        {
            string[] com = str.Split('|');
            IfAction core = new IfAction();
            core.Point = int.Parse(com[0]);

            core.Result = new List<int>(com[1].Split('!').Select(int.Parse).ToArray());


            string[] com1 = com[2].Split('!');
            core.Core = new List<RuleForm>(new RuleForm[com1.Length]);
            for (int i = 0; i < com1.Length; i++)
                core.Core[i] = ReturnCore(com1[i]);

            if (com[3] == " ")
                core.ResultCore = new List<RuleForm>();
            else
            {
                com1 = com[3].Split('!');
                core.ResultCore = new List<RuleForm>(new RuleForm[com1.Length]);
                for (int i = 0; i < com1.Length; i++)
                    core.ResultCore[i] = ReturnCore(com1[i]);
            }

            return core;
        }
        static RuleAction ReturnRuleAction(string str)
        {
            string[] com = str.Split('|');
            RuleAction core = new RuleAction();
            core.Action = int.Parse(com[0]);
            core.ActionExtend = int.Parse(com[1]);

            core.Min = int.Parse(com[2]);
            core.Max = int.Parse(com[3]);

            core.Team = int.Parse(com[4]);

            //core.RuleTag = int.Parse(com[5]);
            //core.Rule = int.Parse(com[6]);

            //core.ForseMood = int.Parse(com[5]);
            if (com[5] == " ")
                core.Core = new List<RuleForm>();
            else
            {
                string[] com1 = com[5].Split('!');
                core.Core = new List<RuleForm>(new RuleForm[com1.Length]);
                for (int i = 0; i < com1.Length; i++)
                    core.Core[i] = ReturnCore(com1[i]);
            }


            core.ResultCore = ReturnCore(com[6]);
            core.Prioritet = int.Parse(com[7]);

            return core;
        }

        public static Accses LoadRuleAccses(int a, int b)
        {
            XElement root = XDocument.Parse(File.ReadAllText(mainPath + $"Rule/{a}/{b}.R")).Element("root");
            return new Accses(root.Element("Accses").Value);

        }
        public static HeadRule LoadRule(int a, int b)
        {
            HeadRule head = new HeadRule();
            head.Tag = a;
            head.Id = b;
            //string path = mainPath + $"Rule/{a}/{b};

            string str = "";
            string[] com, com1;
            XElement root = XDocument.Parse(File.ReadAllText(mainPath + $"Rule/{a}/{b}.R")).Element("root");
            //head.Cost = int.Parse(root.Element("Cost").Value);
            //  str = root.Element("Cost").Value;
            //int c = int.Parse(root.Element("Triggers").Value);

            head.Trigger = new List<TriggerAction>(new TriggerAction[int.Parse(root.Element("Triggers").Value)]);
            for (int i = 0; i < head.Trigger.Count; i++)
            {

                //Debug.Log(root.Element($"Trigger{i}").Value);
                //str = root.Element($"Trigger{i}").Value;
                //com = str.Split('/');

                com = root.Element($"Trigger{i}").Value.Split('/');
                TriggerAction trigger = new TriggerAction();
                //com1 = com[0].Split('|');
                trigger.Plan = int.Parse(com[0]);
                trigger.Trigger = int.Parse(com[1]);

                trigger.CountMod = bool.Parse(com[2]);
                trigger.CountModExtend = bool.Parse(com[3]);

                trigger.Team = int.Parse(com[4]);

                if (com[5] == " ")
                    trigger.PlusAction = new List<IfAction>();
                else
                {
                    com1 = com[5].Split('?');
                    trigger.PlusAction = new List<IfAction>(new IfAction[com1.Length]);
                    for (int i1 = 0; i1 < com1.Length; i1++)
                        trigger.PlusAction[i1] = ReturnIfAction(com1[i1]);
                }


                if (com[6] == " ")
                    trigger.MinusAction = new List<IfAction>();
                else
                {
                    com1 = com[6].Split('?');
                    trigger.MinusAction = new List<IfAction>(new IfAction[com1.Length]);
                    for (int i1 = 0; i1 < com1.Length; i1++)
                        trigger.MinusAction[i1] = ReturnIfAction(com1[i1]);
                }


                if (com[7] == " ")
                    trigger.Action = new List<RuleAction>();
                else
                {
                    com1 = com[7].Split('?');
                    trigger.Action = new List<RuleAction>(new RuleAction[com1.Length]);
                    for (int i1 = 0; i1 < com1.Length; i1++)
                        trigger.Action[i1] = ReturnRuleAction(com1[i1]);
                }



                head.Trigger[i] = trigger;
            }

            head.accses = new Accses(root.Element("Accses").Value);
            head.Visible = bool.Parse(root.Element("Visible").Value);
            head.VisibleCard = bool.Parse(root.Element("VisibleCard").Value);

            com = FindLang(mainPath + $"Rule/{a}/", b).Split('/');
            //head.Name = com[0];
            if (com.Length > 0)
            {
                b = 1;
                for (int i = 0; i < head.Trigger.Count; i++)
                    if (head.Trigger[i].Trigger == 1)
                    {
                        head.Trigger[i].Name = com[b];
                        b++;
                        if (b == com.Length)
                            break;
                    }
            }

            return head;
        }


        static string GetCore(RuleForm core)
        {
            string str = $"{core.Card}*{core.Tayp}*{core.TaypId}*{core.Mod}*{core.Num}";

            return str;
        }
        static string GetIfAction(IfAction core)
        {

            string str = $"{core.Point}|";
            string str1 = $"{core.Result[0]}";
            for (int i = 1; i < core.Result.Count; i++)
                str1 += $"!{core.Result[i]}";
            str += str1;

            str += "|";

            str1 = GetCore(core.Core[0]);
            for (int i = 1; i < core.Core.Count; i++)
                str1 += "!" + GetCore(core.Core[i]);
            str += str1;

            str += "|";
            if (core.ResultCore.Count > 0)
            {
                str1 = GetCore(core.ResultCore[0]);
                for (int i = 1; i < core.ResultCore.Count; i++)
                    str1 += "!" + GetCore(core.ResultCore[i]);
                str += str1;
            }
            else
                str += " ";

            return str;
        }
        static string GetRuleAction(RuleAction core)
        {
            string str = $"{core.Action}|{core.ActionExtend}|{core.Min}|{core.Max}|{core.Team}|";
            if (core.Core.Count > 0)
            {
                str += GetCore(core.Core[0]);
                for (int i = 1; i < core.Core.Count; i++)
                    str += "!" + GetCore(core.Core[i]);
            }
            else
                str += " ";
            str += "|" + GetCore(core.ResultCore);

            str += "|" + core.Prioritet;
            return str;
        }

        static string GetNeedRule(SubInt subInt)
        {
            string str = $"{subInt.Head}_";
            if (subInt.Num.Count == 0)
                str += " ";
            else
            {
                str += $"{subInt.Num[0]}";
                for (int i = 1; i < subInt.Num.Count; i++)
                    str += $"-{subInt.Num[i]}";
            }

            return str;
        }

        public static void SaveRule(HeadRule head, int a, int b)
        {
            if (b == -1)
                return;
            string str, str1, str2 = " ";

            string path = mainPath + $"Rule/{a}/";
            XElement root = new XElement("root");

            head.accses.ClearList();


            //root.Add(new XElement("Cost", head.Cost));
            root.Add(new XElement("Triggers", head.Trigger.Count));
            for (int i = 0; i < head.Trigger.Count; i++)
            {

                TriggerAction trigger = head.Trigger[i];
                str = $"{trigger.Plan}/{trigger.Trigger}/{trigger.CountMod}/{trigger.CountModExtend}/{trigger.Team}";
                if (trigger.PlusAction.Count == 0)
                    str += "/ ";
                else
                {
                    str1 = "/" + GetIfAction(trigger.PlusAction[0]);
                    for (int i1 = 1; i1 < trigger.PlusAction.Count; i1++)
                        str1 += "?" + GetIfAction(trigger.PlusAction[i1]);

                    str += str1;
                }

                if (trigger.MinusAction.Count == 0)
                    str += "/ ";
                else
                {
                    str1 = "/" + GetIfAction(trigger.MinusAction[0]);
                    for (int i1 = 1; i1 < trigger.MinusAction.Count; i1++)
                        str1 += "?" + GetIfAction(trigger.MinusAction[i1]);

                    str += str1;
                }

                if (trigger.Action.Count == 0)
                    str += "/ ";
                else
                {
                    str1 = "/" + GetRuleAction(trigger.Action[0]);
                    for (int i1 = 1; i1 < trigger.Action.Count; i1++)
                        str1 += "?" + GetRuleAction(trigger.Action[i1]);


                    str += str1;
                }


                if (trigger.Trigger == 1)
                {
                    if (str2 == " ")
                        str2 = trigger.Name;
                    else
                        str2 += "/" + trigger.Name;
                }

                root.Add(new XElement("Trigger" + i, str));
            }

            root.Add(new XElement("Accses", head.accses.Zip()));
            root.Add(new XElement("Visible", head.Visible));
            root.Add(new XElement("VisibleCard", head.VisibleCard));


            AddLangFile(path, str2, b);


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"{b}.R", saveDoc.ToString());

        }


        public static void DeliteRule(int a, int b)
        {
            string path = mainPath + $"Rule/";
            RemoveWord(path, a);
            path += $"{a}/";
            RemoveLocal(path, b);
            path += $"{b}.R";
            File.Delete(path);
            b = core.head[a].Index.FindIndex(x => x == b);


            core.head[a].Rule.RemoveAt(b);
            core.head[a].Index.RemoveAt(b);
            core.head[a].Cost.RemoveAt(b);
            if (core.head[a].Index.Count == 0)
                core.head[a].LastIndex = 0;
            SaveRuleMain(a);
        }
        #endregion

        #region card
        /*
        public static void TransfCard(CardCase card,int oldGuild, int oldKey)
        {
            HideLibrary lib = LoadGuild(oldGuild);
            if (card.Guild != oldGuild || card.Key != oldKey)
            {
                int key = card.Key;
                card.Key = oldKey; 
                lib.RemoveCard(card);
                if(card.Guild != oldGuild)
                    SaveGuild(lib);

                DeliteCard(card);
                card.Id = -1;
                card.Key = key;

                if (card.Guild != oldGuild)
                    lib = LoadGuild(card.Guild);
                lib.AddCard(card);
            }
            else 
                lib.SwitchCard(card);
            

            SaveGuild(lib);
            SaveCard(card);
        }*/

        //-1 последняя сесия
        public static SubInt LoadGuildCard(int a)
        {
            if (!File.Exists(mainPath + $"Card/{a}.CD"))
                return new SubInt(0);

            XElement root = XDocument.Parse(File.ReadAllText(mainPath + $"Card/{a}.CD")).Element("root");
            string str = root.Element("Index").Value;
            return new SubInt(str, 5);//lable;
        }
        public static void SaveGuildCard(SubInt sub, int a)
        {
            string path = mainPath + $"Card/{a}.CD";
            Debug.Log(path);
            XElement root = new XElement("root");
            string str = "";
            root.Add(new XElement("Index", sub.Zip(4)));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path, saveDoc.ToString());
        }
        public static HideLibrary LoadGuild(int a)
        {
            HideLibrary lib = new HideLibrary();
            if (!File.Exists(mainPath + $"Card/{a}.G"))
            {
                lib.Index = new List<SubInt>();
                SaveGuild(lib, a);
                return lib;
            }

            HideLibraryCase ReadCase(XElement root, string com)
            {
                string str;
                HideLibraryCase cases = new HideLibraryCase();
                str = root.Element(com + "Use").Value;
                if (str != "")
                    cases.Use = new List<bool>(str.Split('/').Select(bool.Parse).ToArray());

                str = root.Element(com + "Index").Value;
                if (str != "")
                    cases.Index = new List<int>(str.Split('/').Select(int.Parse).ToArray());

                str = root.Element(com + "Size").Value;
                if (str != "")
                    cases.Size = new List<int>(str.Split('/').Select(int.Parse).ToArray());

                return cases;
            }

            XElement root = XDocument.Parse(File.ReadAllText(mainPath + $"Card/{a}.G")).Element("root");

            SubInt sub = LoadGuildCard(a);
            lib.Index = sub.Num;

            lib.Legion = ReadCase(root, "Legion");
            lib.Civilian = ReadCase(root, "Civilian");
            lib.Race = ReadCase(root, "Race");
            lib.Tag = ReadCase(root, "Tag");
            lib.Stat = ReadCase(root, "Stat");

            lib.AllCard = int.Parse(root.Element("AllCard").Value);

            return lib;
        }
        public static void SaveGuildCard(CardCase card)
        {
            HideLibrary lib = LoadGuild(card.Guild);
            lib.SwitchCard(card);
            SaveGuild(lib, card.Guild);
            SaveCard(card);
        }
        public static void SaveGuild(HideLibrary lib, int a)
        {

            void SaveCase(XElement root, HideLibraryCase cases, string com)
            {
                root.Add(new XElement(com + "Use", ReturnListData(cases.Use)));
                root.Add(new XElement(com + "Index", ReturnListData(cases.Index)));
                root.Add(new XElement(com + "Size", ReturnListData(cases.Size)));
            }

            string path = mainPath + $"Card/{a}.G";
            XElement root = new XElement("root");
            if (lib.Index.Count != 0)
            {
                SubInt sub = new SubInt(0);
                sub.Num = lib.Index;
                SaveGuildCard(sub, a);
            }

            SaveCase(root, lib.Legion, "Legion");
            SaveCase(root, lib.Civilian, "Civilian");
            SaveCase(root, lib.Race, "Race");
            SaveCase(root, lib.Tag, "Tag");
            SaveCase(root, lib.Stat, "Stat");

            root.Add(new XElement("AllCard", lib.AllCard));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path, saveDoc.ToString());
        }

        public static void DeliteCard(CardCase card)
        {
            string path = mainPath + $"Card/{card.Guild}/{card.CardTayp}/{card.CardClass}/{card.Id}.c";
            File.Delete(path);
            //path = mainPath + $"Card/{card.Guild}/{card.CardTayp}/{card.CardClass}/L/{lang}/{card.Id}.L";
            //File.Delete(path);
        }
        public static void SaveCard(CardCase card)
        {
            string path = mainPath + $"Card/{card.Guild}/{card.CardTayp}/{card.CardClass}/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            // string path = mainPath + $"Card/{card.Guild}/{card.CardTayp}/{card.CardClass}/{card.Id}.c";
            XElement root = new XElement("root");

            root.Add(new XElement("Name", card.Name));
            root.Add(new XElement("Mana", card.Mana));

            root.Add(new XElement("Legion", card.Legion));
            root.Add(new XElement("Civilian", card.Civilian));
            root.Add(new XElement("Race", card.Race));


            string str = card.Stat[0].Read("Save");
            for (int i = 1; i < card.Stat.Count; i++)
                str += "|" + card.Stat[i].Read("Save");
            root.Add(new XElement("Stat", str));

            {
                SubInt sub = new SubInt(0);
                sub.Num = card.Trait;
                root.Add(new XElement("Trait", sub.Zip(3)));
            }
            // AddLangFile(path, card.Name, card.Id);


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"{card.Id}.c", saveDoc.ToString());
        }
        public static CardCase LoadCard(int intGuild, int intCardTayp, int intCardClass, int intId)
        {
            CardCase card = new CardCase(intGuild, intCardTayp, intCardClass, intId);
            XElement root = XDocument.Parse(File.ReadAllText(mainPath + $"Card/{card.Guild}/{card.CardTayp}/{card.CardClass}/{card.Id}.c")).Element("root");
            card.Name = root.Element("Name").Value;
            card.Mana = int.Parse(root.Element("Mana").Value);

            card.Legion = int.Parse(root.Element("Legion").Value);
            card.Civilian = int.Parse(root.Element("Civilian").Value);
            card.Race = int.Parse(root.Element("Race").Value);

            BD bd = core.bD[core.keyStat];
            string[] com = root.Element("Stat").Value.Split('|');
            for (int i = 0; i < com.Length; i++)
                card.Stat.Add(new StatExtend(com[i], bd));

            {
                SubInt sub = new SubInt(root.Element("Trait").Value, 3);
                card.Trait = sub.Num;
            }

            return card;
        }

        #endregion

        #region Colod
        public static void SaveColodBD(int guild,List<string> names,List<int> index)
        {

            //string path = mainPath + $"Card/{guild}.cMain";
            XElement root = new XElement("root");

            root.Add(new XElement("Names", ReturnListData(names)));

            root.Add(new XElement("Index", ReturnListData(index)));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(mainPath + $"Card/{guild}.cMain", saveDoc.ToString());


        }

        public static void LoadColodBD(int guild, List<int> colodGuild, List<string> colodName)
        {
            if (!File.Exists(mainPath + $"Card/{guild}.cMain"))
            {
               // Gallery.colodGuild = new List<int>();
                //Gallery.colodName = new List<string>();
                return;
            }
                XElement root = XDocument.Parse(File.ReadAllText(mainPath + $"Card/{guild}.cMain")).Element("root");
            int[] comI  = root.Element("Index").Value.Split('/').Select(int.Parse).ToArray();
            for (int i = 0; i < comI.Length; i++)
                colodGuild.Add(comI[i]);

            string[] com  =root.Element("Names").Value.Split('/');
            for (int i = 0; i < com.Length; i++)
                colodName.Add(com[i]);

        }
        public static void SaveColod(int guild, int colod, int cardSum,List<string> id,List<int> size)
        {

            string path = mainPath + $"Card/{guild}/Colod/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            XElement root = new XElement("root");

            root.Add(new XElement("CardSum", cardSum));

            root.Add(new XElement("Id", ReturnListData(id)));

            root.Add(new XElement("Size", ReturnListData(size)));


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"{colod}.co", saveDoc.ToString());
           
        }
        public static void DliteColod(int guild,int colod)
        {
        }
        public static int LoadColod(int guild, int colod, List<CardCase> cardsColod, List<int> size)
        {
            //! Добавить сюда добавление нулевой карты, карты-палатки

            XElement root = XDocument.Parse(File.ReadAllText(mainPath + $"Card/{guild}/Colod/{colod}.co")).Element("root");

            int cardSum = int.Parse(root.Element("CardSum").Value);

            string[] com = root.Element("Id").Value.Split('/');
            for (int i = 0; i < com.Length; i++)
            {
                int[] index = com[i].Split('|').Select(int.Parse).ToArray();

                cardsColod.Add(LoadCard(index[0], index[2], index[1], index[3]));
            }

            int[] ints = root.Element("Size").Value.Split('/').Select(int.Parse).ToArray();
            for (int i = 0; i < ints.Length; i++)
                size.Add(ints[i]);
            return cardSum;
        }
        #endregion
        //#region GameData

        //public static void SaveGameData(GameData gameData)
        //{
        //    if (gameData == null)
        //        return;

        //    SubGameData sub = null;
        //    for (int i = 0; i < gameData.Data.Count; i++)
        //    {
        //        sub = gameData.Data[i];

        //        if (sub.Size > 0)
        //        {
        //            XElement root = new XElement("root");

        //            root.Add(new XElement("Size", sub.Size));
        //            root.Add(new XElement("Key", sub.Key));

        //            XDocument saveDoc = new XDocument(root);
        //            File.WriteAllText(Application.dataPath + "/Resources/Data/" + sub.MasterKey + "Data.xml", saveDoc.ToString());
        //        }
        //    }
        //}


        //public static void LoadGameData(SubGameData sub)
        //{
        //    string path = Application.dataPath + "/Resources/Data/"+ sub.MasterKey + "Data.xml";
        //    Debug.Log(path);
        //    if (!File.Exists(path))
        //        return;
        //    XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");
        //    sub.Key = root.Element("Key").Value;
        //    sub.Size = int.Parse(root.Element("Size").Value);
        //   // Debug.Log(sub.Key);
        //    //Debug.Log(path);
        //    //GameData gameData = new GameData();
        //    //string str;
        //    //string[] com;

        //    ////if (File.Exists($"{path}/Data.xml"))
        //    ////{
        //    //XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
        //    //gameData.Size = int.Parse(root.Element("Size").Value);

        //    //str = root.Element("Data").Value;
        //    //Debug.Log(str);
        //    //com = str.Split('_');
        //    //gameData.Guild = new List<GameDataData>();
        //    //for (int i = 0; i < com.Length; i++)
        //    //    gameData.Guild.Add(LoadGameDataRoot(path, com[i]));

        //    //return gameData;
        //}
        //#endregion
        //#region LoadData

        //public static BD LoadBD(string path)
        //{
        //    BD bd = new BD();
        //    if (File.Exists(path))
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");
        //        bd.Name = root.Element("Name").Value;
        //        bd.Info = root.Element("Info").Value;
        //    }

        //    return bd;
        //}
        //public static void SaveAllBD(List<BD> bD)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/";
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);
        //    for (int i = 0; i < bD.Count; i++)
        //    {
        //        if (!Directory.Exists(path + $"/{i}/"))
        //            Directory.CreateDirectory(path + $"/{i}/");
        //        SaveBD(bD[i], path + $"{i}.{lang}");
        //        //SaveBase(i, 0, new MainBase());
        //    }

        //}
        //public static void SaveBD(BD bD, string path)
        //{
        //    XElement root = new XElement("root");
        //    root.Add(new XElement("Name", bD.Name));
        //    root.Add(new XElement("Info", bD.Info));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());

        //}


        //public static List<BD> LoadAllData()
        //{
        //    List<BD> bD = new List<BD>();
        //    string path = Application.dataPath + $"/Resources/Data/";
        //    string[] tayp = Directory.GetFiles(path, $"*.{lang}");
        //    {
        //        string[] tayp1 = Directory.GetFiles(path, $"*.Eng");
        //        if (tayp.Length != tayp1.Length)
        //            tayp = tayp1;
        //    }

        //    for (int i = 0; i < tayp.Length; i++)
        //    {
        //        bD.Add(LoadBD(tayp[i]));

        //       string[] com = Directory.GetFiles(path + $"/{i}/", "*.H");


        //        for (int i1 = 0; i1 < com.Length; i1++) 
        //        {
        //            bD[i].Base.Add(LoadBase(com[i1], tayp[i])); 
        //        }

        //    }
        //    return bD;

        //}
        //public static MainBase LoadBase(string path, string tayp)
        //{
        //    MainBase mainBase = new MainBase();
        //    XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");

        //   // mainBase.SysName = root.Element("Sys").Value;
        //   // mainBase.Tayp = tayp;
        //    mainBase.ColorName = root.Element("ColorName").Value;

        //    //switch(tayp){}


        //    if (File.Exists(path + "_" + lang))
        //    {
        //        root = XDocument.Parse(File.ReadAllText(path + "_" + lang)).Element("root");
        //    }
        //    else if (File.Exists(path + "_Eng"))
        //    {
        //        root = XDocument.Parse(File.ReadAllText(path + "_Eng")).Element("root");
        //    }
        //    else
        //        return mainBase;

        //    mainBase.Name = root.Element("Name").Value;
        //    mainBase.Info = root.Element("Info").Value;

        //    return mainBase;
        //}

        //public static void SaveBase(int a, int b, MainBase mainBase)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/{a}/{b}.H_{lang}";
        //    XElement root = new XElement("root");
        //    root.Add(new XElement("Name", mainBase.Name));
        //    root.Add(new XElement("Info", mainBase.Info));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());


        //    path = Application.dataPath + $"/Resources/Data/{a}/{b}.H";
        //    root = new XElement("root");
        //    root.Add(new XElement("ColorName", mainBase.ColorName));
        //    //root.Add(new XElement("Info", mainBase.Info));

        //    saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());
        //}
        //#endregion
        //public static void LoadAtlas(SpriteRenderer sr, TMP_SpriteAsset  tmp)
        //{
        //    //string path = Application.dataPath + $"/Resources/Icon/";
        //    //string[] com = Directory.GetFiles(path, "*.png");

        //    //Texture2D[] textures = new Texture2D[1024];
        //    //Texture2D tx;
        //    //for (int i = 0; i < com.Length; i++)
        //    //{
        //    //    tx = new Texture2D(2, 2);
        //    //    byte[] FileData = File.ReadAllBytes(com[i]);
        //    //    tx.LoadImage(FileData);
        //    //    textures[i] = tx;
        //    //}




        //    //tx = new Texture2D(1024, 1024);
        //    //tx.PackTextures(textures, 0, 1024);

        //    ////sr.sprite = Sprite.Create(tx, new Rect(0.0f, 0.0f, tx.width, tx.height), new Vector2(0.5f, 0.5f), 100.0f);

        //    ////tmp.spriteSheet = tx;
        //    ////tmp.UpdateLookupTables();
        //    //path = Application.dataPath + $"/Resources/D.png";

        //    //byte[] bytes = tx.EncodeToPNG();
        //    //File.WriteAllBytes(path, bytes);//.png

        //}
        //#region CardSet
        //public static void SaveCardSets(List<string> list, string guild)
        //{
        //    string path = Application.dataPath + $"/Resources/CardSet/{guild}.xml";


        //    XElement root = new XElement("root");
        //    string str;
        //    if (list.Count == 0)
        //        str = " ";
        //    else
        //    {
        //        str = list[0];
        //        for (int i = 1; i < list.Count; i++)
        //            str += "/" + list[i];
        //    }
        //    root.Add(new XElement("Path", str));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //}
        //public static List<string> LoadCardSets(string guild)
        //{
        //    List<string> colod;

        //    string path = Application.dataPath + $"/Resources/CardSet/{guild}";
        //    if (Directory.Exists(path))
        //        Directory.CreateDirectory(path);

        //    path += ".xml";
        //    if (File.Exists(path))
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");

        //        string str = root.Element("Path").Value;
        //        if (str != " ")
        //            colod = new List<string>(str.Split('/'));
        //        else
        //            colod = new List<string>();
        //    }
        //    else
        //        colod = new List<string>();

        //    return colod;
        //}
        //public static void SaveCardSet(CardSet cardSet, string path)
        //{
        //    path = Application.dataPath + $"/Resources/CardSet/" + path + ".xml";
        //    XElement root = new XElement("root");

        //    string str = " ";
        //    if (cardSet.Path.Count > 0)
        //    {
        //        str = cardSet.Path[0];
        //        for (int i = 1; i < cardSet.Path.Count; i++)
        //            str += "/" + cardSet.Path;
        //    }
        //    root.Add(new XElement("Path", str));


        //    str = " ";
        //    if (cardSet.Size.Count > 0)
        //    {
        //        str = ""+cardSet.Size[0];
        //        for (int i = 1; i < cardSet.Size.Count; i++)
        //            str += "/" + cardSet.Size;
        //    }
        //    root.Add(new XElement("Size", str));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());

        //}
        //public static CardSet LoadCardSet( string name)
        //{
        //    string path = Application.dataPath + $"/Resources/CardSet/" + name+".xml";
        //    CardSet cardSet = new CardSet();
        //    string[] com = name.Split('/');
        //    cardSet.Name = com[1];
        //    cardSet.Path = new List<string>();
        //    cardSet.Size = new List<int>();
        //    if (File.Exists(path))
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");

        //        string str = root.Element("Path").Value;
        //        cardSet.Path = new List<string>(str.Split('/'));

        //        str = root.Element("Size").Value;
        //        com = str.Split('/');
        //        foreach (string str1 in com)
        //            cardSet.Size.Add(int.Parse(str1));

        //    }
        //    return cardSet;
        //}
        //#endregion
        //#region Card

        ////public static void SetData(string path)
        ////{
        ////    root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
        ////    //card = 
        ////}

        //public static Sprite LoadTexture(string FilePath)
        //{
        //    // Load a PNG or JPG file from disk to a Texture2D
        //    // Returns null if load fails
        //    byte[] FileData = File.ReadAllBytes(FilePath+".X");
        //    Texture2D texture = new Texture2D(2, 2);
        //    texture.LoadImage(FileData);

        //    return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        //}


        //public static void Save(CardCase cardBase, string path)
        //{
        //    path = Application.dataPath + $"/Resources/Data/" + path;
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);
        //    path += ""+cardBase.Id;
        //    XElement root = new XElement("root");

        //    root.Add(new XElement("Name", cardBase.Name));

        //    root.Add(new XElement("Guild", cardBase.Guilds.Name));

        //    root.Add(new XElement("Races", cardBase.Races.Name));
        //    root.Add(new XElement("Legions", cardBase.Legions.Name));
        //    root.Add(new XElement("CivilianGroups", cardBase.CivilianGroups.Name));

        //    root.Add(new XElement("Tayp", cardBase.Tayp));
        //    root.Add(new XElement("Mana", cardBase.Mana));


        //    string str1 ="", str2 = "";
        //    for (int i = 0; i < cardBase.Stat.Count; i++)
        //    {
        //        str1 += cardBase.Stat[i].Name;
        //        str2 += cardBase.StatSize[i];

        //        if(i < cardBase.Stat.Count - 1)
        //        {
        //            str1 += "_";
        //            str2 += "_";
        //        }
        //    }
        //    root.Add(new XElement("Stat", str1));
        //    root.Add(new XElement("StatSize", str2));
        //    str1 = "";

        //    for (int i = 0; i < cardBase.Trait.Count; i++)
        //    {
        //        str1 += cardBase.Trait[i];

        //        if (i < cardBase.Trait.Count - 1)
        //            str1 += "_";
        //    }
        //    if (cardBase.Trait.Count == 0)
        //        str1 = " ";

        //    root.Add(new XElement("Trait", str1));


        //    Texture2D itemBGTex = cardBase.Image.texture;
        //    byte[] bytes = itemBGTex.EncodeToPNG();
        //    File.WriteAllBytes(path+".X", bytes);//.png


        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path +".xml", saveDoc.ToString());

        //}



        //public static CardCase Load(string path)
        //{
        //    path = Application.dataPath + $"/Resources/Data/" + path;
        //  //  Debug.Log(path);
        //    CardCase cardBase = new CardCase();
        //    XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");


        //    #region Main
        //    cardBase.Name = root.Element("Name").Value;
        //        //Debug.Log(path);

        //        string data = root.Element("Guild").Value;
        //        int a = gameSetting.Library.Guilds.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.Guilds = gameSetting.Library.Guilds[a];

        //        data = root.Element("Races").Value;
        //        a = cardBase.Guilds.Races.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.Races = cardBase.Guilds.Races[a];

        //        data = root.Element("Legions").Value;
        //        a = cardBase.Guilds.Legions.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.Legions = cardBase.Guilds.Legions[a];

        //        data = root.Element("CivilianGroups").Value;
        //        a = cardBase.Legions.CivilianGroups.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.CivilianGroups = cardBase.Legions.CivilianGroups[a];
        //    #endregion
        //    cardBase.Tayp = root.Element("Tayp").Value;

        //    cardBase.Mana = int.Parse(root.Element("Mana").Value);

        //    data = root.Element("Stat").Value;
        //    string[] com = data.Split('_');
        //    cardBase.Stat = new List<Constant>();
        //    for (int i = 0; i < com.Length; i++)
        //    {
        //        cardBase.Stat.Add( gameSetting.Library.Constants.Find(x => x.Name == com[i]));
        //    }

        //    data = root.Element("StatSize").Value;
        //    com = data.Split('_');
        //    cardBase.StatSize = new List<int>();
        //    cardBase.StatSizeLocal = new List<int>();
        //    for (int i = 0; i < com.Length; i++)
        //    {
        //        cardBase.StatSize.Add(int.Parse(com[i]));
        //        cardBase.StatSizeLocal.Add(0);
        //    }
        //    data = root.Element("Trait").Value;
        //    com = data.Split('_');
        //    if (com[0] != " ")
        //        cardBase.Trait = new List<string>(com);
        //    else
        //        cardBase.Trait = new List<string>();

        //    for(int i =0;i<cardBase.Trait.Count; i++)
        //    {
        //        cardBase.TraitSize.Add(0);
        //    }


        //    cardBase.Image = LoadTexture(path);





        //    return cardBase;
        //}
        //#endregion
        //#region Rule
        //public static void LoadMainRule( ActionLibrary library)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
        //    if (path != "")
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

        //        string str = root.Element("RuleTag").Value;
        //        string[] com = str.Split('_');
        //        List<SubRuleHead> text = new List<SubRuleHead>();
        //        SubRuleHead subRule = null;
        //        for (int i =0; i < com.Length; i++)
        //        {
        //            subRule = new SubRuleHead();

        //            str = root.Element($"RuleName{i}").Value;
        //            string[] com1 = str.Split('_');

        //            subRule.Name = com[i];
        //            subRule.Rule = new List<string>();

        //            foreach( string str1 in com1)
        //                subRule.Rule.Add(str1);


        //            text.Add(subRule);
        //        }

        //        library.Rule = text;
        //    }
        //}
        //public static void SaveMainRule( ActionLibrary library)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
        //    if (path != "")
        //    {
        //        XElement root = new XElement("root");
        //        string str;
        //        string strMain = "";

        //        //root.Add(new XElement($"RuleSize", library.Rule.Count));

        //        for (int i =0;i< library.Rule.Count;i++)
        //        {
        //            strMain += library.Rule[i].Name;
        //            str = "";

        //            for (int i1 = 0; i1 < library.Rule[i].Rule.Count; i1++)
        //            {
        //                str += library.Rule[i].Rule[i1];
        //                if(i1+1 < library.Rule[i].Rule.Count)
        //                    str += "_";

        //            }
        //            root.Add(new XElement($"RuleName{i}", str));

        //            if (i + 1 < library.Rule.Count)
        //                str += "_";
        //        }

        //        root.Add(new XElement($"RuleTag", strMain));


        //        XDocument saveDoc = new XDocument(root);
        //        File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //    }
        //}

        //private static string SaveRuleCore(RuleForm ruleForm)
        //{
        //    string text = "" + ruleForm.Card
        //        + "_" + ruleForm.StatTayp
        //        + "_" + ruleForm.Stat
        //        + "_" + ruleForm.Mod
        //        + "_" + ruleForm.Num;

        //    return text;
        //}
        //private static string SaveRuleCoreSimple(RuleForm ruleForm)
        //{
        //    string text = "" + ruleForm.Card 
        //        + ":" + ruleForm.StatTayp 
        //        + "_" + ruleForm.Stat 
        //        + "_" + ruleForm.Mod 
        //        + "_" + ruleForm.Num;

        //    return text;
        //}
        //private static RuleForm LoadRuleCore(string str)
        //{
        //    RuleForm ruleForm = new RuleForm();

        //    string[] subs = str.Split('_');
        //    ruleForm.Card = subs[0];
        //    ruleForm.StatTayp = subs[1];
        //    ruleForm.Stat = subs[2];
        //    ruleForm.Mod = int.Parse(subs[3]);
        //    ruleForm.Num = int.Parse(subs[4]);

        //    return ruleForm;
        //}

        //private static XElement SaveRuleIfAction(IfAction ifAction, XElement root, string text)
        //{
        //    RuleForm ifCore = null;

        //    root.Add(new XElement($"{text}Result", ifAction.Result));
        //    root.Add(new XElement($"{text}Prioritet", ifAction.Prioritet));
        //    root.Add(new XElement($"{text}Point", ifAction.Point));

        //    root.Add(new XElement($"{text}CoreCount", ifAction.Core.Count));
        //    string str = "";// + b2 = ifAction.Core.; 
        //    for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
        //    {
        //        ifCore = ifAction.Core[i2];
        //        str = SaveRuleCore(ifCore);
        //        root.Add(new XElement($"{text}Core{i2}", str));
        //    }
        //    return root;
        //}
        //private static XElement SaveRuleAction(RuleAction ifAction, XElement root, string text)
        //{
        //    RuleForm ifCore = null;
        //    root.Add(new XElement($"{text}Name", $"{ifAction.Name}"));
        //    root.Add(new XElement($"{text}Point", $"{ifAction.MinPoint}|{ifAction.MaxPoint}"));
        //    root.Add(new XElement($"{text}ActionMood", ifAction.ActionMood));
        //    root.Add(new XElement($"{text}Action", ifAction.Action));
        //   // root.Add(new XElement($"{text}Num", ifAction.Num));
        //    root.Add(new XElement($"{text}ForseMood", ifAction.ForseMood));

        //    root.Add(new XElement($"{text}CoreCount", ifAction.Core.Count));
        //    string str = "";// + b2 = ifAction.Core.; 
        //    for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
        //    {
        //        ifCore = ifAction.Core[i2];
        //        str = SaveRuleCore(ifCore);
        //        root.Add(new XElement($"{text}Core{i2}", str));
        //    }
        //    return root;
        //}

        //private static IfAction LoadRuleIfAction(XElement root, string text)
        //{
        //    IfAction ifAction = new IfAction();

        //    ifAction.Result = int.Parse(root.Element($"{ text}Result").Value);
        //    ifAction.Prioritet = int.Parse(root.Element($"{ text}Prioritet").Value);
        //    ifAction.Point = int.Parse(root.Element($"{ text}Point").Value);

        //    int a = int.Parse(root.Element($"{ text}CoreCount").Value);

        //    for (int i = 0; i < a; i++)
        //    {
        //        ifAction.Core.Add(LoadRuleCore(root.Element($"{text}Core{i}").Value));
        //    }

        //    return ifAction;
        //}
        //private static RuleAction LoadRuleAction(XElement root, string text)
        //{
        //    RuleAction ifAction = new RuleAction();


        //    ifAction.Name = root.Element($"{ text}Name").Value;

        //    string str = root.Element($"{ text}Point").Value;
        //    string[] subs = str.Split('|');

        //    ifAction.MinPoint = int.Parse(subs[0]);
        //    ifAction.MaxPoint = int.Parse(subs[1]);

        //    ifAction.ActionMood = int.Parse(root.Element($"{ text}ActionMood").Value);
        //    ifAction.Action = root.Element($"{ text}Action").Value;
        //   // ifAction.Num = int.Parse(root.Element($"{ text}Num").Value);
        //    ifAction.ForseMood = int.Parse(root.Element($"{ text}ForseMood").Value);

        //    int a = int.Parse(root.Element($"{ text}CoreCount").Value); 

        //    for (int i = 0; i < a; i++)
        //    {
        //        ifAction.Core.Add(LoadRuleCore(root.Element($"{text}Core{i}").Value));
        //    }

        //    return ifAction;

        //}

        //private static string SaveRuleIfActionSimple(IfAction ifAction)
        //{
        //    string text =
        //        "" + frame.EqualString[ifAction.Result]
        //        + "_" + ifAction.Prioritet
        //        + "_" + ifAction.Point + "*";
        //    int b = ifAction.Core.Count;
        //    for (int i = 0; i < b; i++)
        //    {
        //        text += SaveRuleCoreSimple(ifAction.Core[i]);
        //        if (i + 1 != b)
        //            text += "|";
        //    }
        //    return text;
        //}
        //private static string SaveRuleActionSimple(RuleAction ifAction)
        //{
        //    string text =
        //        "" + ifAction.MinPoint
        //        + "_" + ifAction.MaxPoint
        //        + "_" + frame.PlayerString[ifAction.ActionMood]
        //        + "_" + ifAction.Action
        //      //  + "_" + ifAction.Num
        //        + "_" + frame.ForseTayp[ifAction.ForseMood]
        //        + "*";

        //    int b = ifAction.Core.Count;
        //    for (int i = 0; i < b; i++)
        //    {
        //        text += SaveRuleCoreSimple(ifAction.Core[i]);
        //        if (i + 1 != b)
        //            text += "|";
        //    }

        //    return text;
        //}


        //public static void SaveSimpleRule(HeadRule head)
        //{
        //    SimpleTrigger simpleTrigger = new SimpleTrigger();
        //    string path = Application.dataPath + $"/Resources/Data/SimpleRule/{head.Tag}_{head.Name}";
        //    if (path != "")
        //    {
        //        int a = 0;
        //        XElement root = new XElement("root");
        //        string str = "" + head.Name
        //           + "_" + head.Cost
        //           + "_" + head.CostExtend
        //           + "_" + head.LevelCap
        //           + "_" + head.Player;
        //        root.Add(new XElement("Head", str));

        //        str = "";
        //        int b = head.NeedRule.Count;
        //        for (int i = 0; i < b; i++)
        //        {
        //            str += head.NeedRule[i];
        //            if (i + 1 != b)
        //                str += "_";
        //        }
        //        root.Add(new XElement("HeadNeedRule", str));

        //        str = "";
        //        b = head.EnemyRule.Count;
        //        for (int i = 0; i < b; i++)
        //        {
        //            str += head.EnemyRule[i];
        //            if (i + 1 != b)
        //                str += "_";
        //        }
        //        root.Add(new XElement("HeadEnemyRule", str));

        //        str = "";
        //        TriggerAction triggerAction = null;
        //        IfAction ifAction = null;
        //        RuleAction ruleAction = null;
        //        string text = "";
        //        int b1 = 0;
        //        int b2 = 0;
        //        b = head.TriggerActions.Count;

        //        for (int i = 0; i < b; i++)
        //        {
        //            triggerAction = head.TriggerActions[i];
        //            str += frame.PlayerString[triggerAction.TargetPalyer]
        //                +"_" + frame.Trigger[triggerAction.Trigger]
        //                + "_" + triggerAction.CountMod
        //                + "_" + triggerAction.CountModExtend;

        //            text = "";
        //            b1 = triggerAction.PlusAction.Count;
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text += SaveRuleIfActionSimple(triggerAction.PlusAction[i1]);
        //                if (i1 + 1 != b1)
        //                    text += "/";
        //            }
        //            root.Add(new XElement($"Trigger{i}PlusAction", text));

        //            text = "";
        //            b1 = triggerAction.MinusAction.Count;
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text += SaveRuleIfActionSimple(triggerAction.MinusAction[i1]);
        //                if (i1 + 1 != b1)
        //                    text += "/";
        //            }
        //            root.Add(new XElement($"Trigger{i}MinusAction", text));


        //            text = "";
        //            b1 = triggerAction.Action.Count;
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {

        //                if(triggerAction.Action[i1].Action == "ActionFlash")
        //                {   
        //                    a = triggerAction.Action[i1].Core.Count-1;
        //                    triggerAction.Action[i1].Action = "PreAction";
        //                    text += SaveRuleActionSimple(triggerAction.Action[i1]);

        //                    triggerAction.Action[i1].Core[a].Mod = -triggerAction.Action[i1].Core[a].Mod;
        //                    triggerAction.Action[i1].Action = "PostAction";
        //                    text += SaveRuleActionSimple(triggerAction.Action[i1]);

        //                    triggerAction.Action[i1].Core[a].Mod = -triggerAction.Action[i1].Core[a].Mod;
        //                    triggerAction.Action[i1].Action = "ActionFlash";
        //                }
        //                else
        //                    text += SaveRuleActionSimple(triggerAction.Action[i1]);

        //                if (i1 + 1 != b1)
        //                    text += "/";
        //            }
        //            root.Add(new XElement($"Trigger{i}Action", text));


        //            if (i + 1 != b)
        //                str += "|";
        //        }
        //        root.Add(new XElement("TriggerHead", str));
        //        XDocument saveDoc = new XDocument(root);
        //        File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //    }

        //}


        //private static XElement root;
        //public static void SetSimpleRoot(string tag)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/SimpleRule/{tag}";
        //    if (path != "")
        //    {
        //        root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
        //    }
        //}
        //public static string LoadSimpleRule(string mood, int b = 0 )
        //{
        //    string path = "";
        //    switch (mood)
        //    {
        //        case ("Head"):
        //            path = root.Element($"Head").Value;
        //            break;
        //        case ("HeadNeedRule"):
        //            path = root.Element($"HeadNeedRule").Value;
        //            break;
        //        case ("HeadEnemyRule"):
        //            path = root.Element($"HeadEnemyRule").Value;
        //            break;
        //        case ("Trigger"):
        //            path = root.Element($"TriggerHead").Value;
        //            break;
        //        case ("TriggerPartPlus"):
        //            path = $"{root.Element($"Trigger{b}PlusAction").Value}";
        //            break;
        //        case ("TriggerPartMinus"):
        //            path = $"{root.Element($"Trigger{b}MinusAction").Value}";
        //            break;
        //        case ("TriggerPart"):
        //            path = $"{root.Element($"Trigger{b}Action").Value}";
        //            break;
        //    }
        //    return path;
        //}

        //public static void SaveRule(HeadRule head)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/Rule/{head.Tag}_{head.Name}"; ;
        //    if (path != "")
        //    {
        //        XElement root = new XElement("root");

        //        root.Add(new XElement("HeadName", head.Name));
        //        root.Add(new XElement("HeadCost", head.Cost));
        //        root.Add(new XElement("HeadNameText", head.NameText));
        //        root.Add(new XElement("Tag", head.Tag));
        //        root.Add(new XElement("HeadCostExtend", head.CostExtend));
        //        root.Add(new XElement("HeadLevelCap", head.LevelCap));
        //        root.Add(new XElement("HeadPlayer", head.Player));


        //        int b = head.NeedRule.Count;
        //        root.Add(new XElement("HeadNeedRuleCount", b));
        //        for (int i = 0; i < b; i++)
        //        {
        //            root.Add(new XElement($"HeadNeedRule{i}", head.NeedRule[i]));
        //        }

        //        b = head.EnemyRule.Count;
        //        root.Add(new XElement("HeadEnemyRuleCount", b));
        //        for (int i = 0; i < b; i++)
        //        {
        //            root.Add(new XElement($"HeadEnemyRule{i}", head.EnemyRule[i]));
        //        }


        //        TriggerAction triggerAction = null;
        //        IfAction ifAction = null;
        //        RuleAction ruleAction = null;

        //        b = head.TriggerActions.Count;

        //        root.Add(new XElement("TriggerActionCount", b));

        //        string text = "";
        //        int b1 = 0;
        //        int b2 = 0;

        //        for (int i = 0; i < b; i++)
        //        {
        //            triggerAction = head.TriggerActions[i];


        //            //public bool CountMod;
        //            //public bool CountModExtend;
        //            root.Add(new XElement($"TargetPalyer{i}", triggerAction.TargetPalyer));
        //            root.Add(new XElement($"Trigger{i}", triggerAction.Trigger));


        //            root.Add(new XElement($"CountMod{i}", triggerAction.CountMod));
        //            root.Add(new XElement($"CountModExtend{i}", triggerAction.CountModExtend));


        //            b1 = triggerAction.PlusAction.Count;
        //            root.Add(new XElement($"PlusActionCount{i}", b1));
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                ifAction = triggerAction.PlusAction[i1];
        //                text = $"Trigger{i}PlusAction{i1}";
        //                SaveRuleIfAction(ifAction, root, text);

        //            }



        //            b1 = triggerAction.MinusAction.Count;
        //            root.Add(new XElement($"MinusActionCount{i}", b1));
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                ifAction = triggerAction.MinusAction[i1];
        //                text = $"Trigger{i}MinusAction{i1}";
        //                root = SaveRuleIfAction(ifAction, root, text);
        //            }



        //            b1 = triggerAction.Action.Count;
        //            root.Add(new XElement($"ActionCount{i}", b1));
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                ruleAction = triggerAction.Action[i1];
        //                text = $"Trigger{i}Action{i1}";

        //                root = SaveRuleAction(ruleAction, root, text);
        //            }
        //        }


        //        XDocument saveDoc = new XDocument(root);
        //        File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //    }
        //}

        //public static HeadRule LoadRule(string tag)
        //{
        //    HeadRule head = null;
        //    string path = Application.dataPath + $"/Resources/Data/Rule/{tag}";
        //    if (path != "")
        //    {

        //        XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

        //        head = new HeadRule();
        //        head.Name = root.Element($"HeadName").Value;
        //        head.Cost = int.Parse(root.Element($"HeadCost").Value);
        //        head.NameText = root.Element($"HeadNameText").Value;
        //        head.Tag = root.Element($"Tag").Value;

        //        head.CostExtend = int.Parse(root.Element($"HeadCostExtend").Value);
        //        head.LevelCap = int.Parse(root.Element($"HeadLevelCap").Value);
        //        //head.CostMovePoint = int.Parse(root.Element($"CostMovePoint").Value);
        //        head.Player = bool.Parse(root.Element($"HeadPlayer").Value);


        //        int b = int.Parse(root.Element("HeadNeedRuleCount").Value);
        //        for (int i = 0; i < b; i++)
        //        {
        //            head.NeedRule.Add(root.Element($"HeadNeedRule{i}").Value);
        //        }

        //        b = int.Parse(root.Element("HeadEnemyRuleCount").Value);
        //        for (int i = 0; i < b; i++)
        //        {
        //            head.EnemyRule.Add(root.Element($"HeadEnemyRule{i}").Value);
        //        }


        //        TriggerAction triggerAction = null;
        //        IfAction ifAction = null;
        //        RuleAction ruleAction = null;


        //        string text = "";
        //        int b1 = 0;
        //        int b2 = 0;

        //        b = int.Parse(root.Element("TriggerActionCount").Value);
        //        for (int i = 0; i < b; i++)
        //        {
        //            triggerAction = new TriggerAction();

        //            triggerAction.PlusAction = new List<IfAction>();
        //            triggerAction.MinusAction = new List<IfAction>();
        //            triggerAction.Action = new List<RuleAction>();


        //            triggerAction.CountMod = bool.Parse(root.Element($"CountMod{i}").Value);
        //            triggerAction.CountModExtend = bool.Parse(root.Element($"CountModExtend{i}").Value);

        //            triggerAction.TargetPalyer = int.Parse(root.Element($"TargetPalyer{i}").Value);
        //            triggerAction.Trigger = int.Parse(root.Element($"Trigger{i}").Value);

        //            b1 = int.Parse(root.Element($"PlusActionCount{i}").Value);
        //            //Debug.Log(b1);
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text = $"Trigger{i}PlusAction{i1}";
        //                triggerAction.PlusAction.Add(LoadRuleIfAction(root, text));

        //            }

        //            b1 = int.Parse(root.Element($"MinusActionCount{i}").Value);
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text = $"Trigger{i}MinusAction{i1}";
        //                triggerAction.MinusAction.Add(LoadRuleIfAction(root, text));
        //            }

        //            b1 = int.Parse(root.Element($"ActionCount{i}").Value);
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text = $"Trigger{i}Action{i1}";

        //                triggerAction.Action.Add(LoadRuleAction(root, text));
        //            }

        //            head.TriggerActions.Add(triggerAction);
        //        }



        //        // ruleConstructor.SetRule(head);

        //    }
        //    return head;
        //}
        //#endregion
    }
}
