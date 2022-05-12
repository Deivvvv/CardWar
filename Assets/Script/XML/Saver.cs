using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace Saver
{
    static class XMLSaver
    {
        private static GameSetting gameSetting;
        private static RuleMainFrame frame;

        public static void SetGameSetting(GameSetting _gameSetting)
        {
            gameSetting = _gameSetting;
        }
        public static void SetRuleMainFrame(RuleMainFrame _frame)
        {
            frame = _frame;
        }


        //GameData
        public static void SaveGameData(GameData gameData, string path)
        {

            XElement root = new XElement("root");

            root.Add(new XElement("AllCard", gameData.AllCard));

            int a = gameData.BlackList.Count;
            root.Add(new XElement("BlackList", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("BlackList" + i, gameData.BlackList[i]));
            }

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}.xml", saveDoc.ToString());

        }
        public static void LoadGameData(string path, CardConstructor cardConstructor)
        {

            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                GameData gameData = new GameData();

                gameData.AllCard = int.Parse(root.Element("AllCard").Value);


                int a = int.Parse(root.Element("BlackList").Value);
                gameData.BlackList = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    gameData.BlackList.Add(int.Parse(root.Element($"BlackList{i}").Value));
                }

                cardConstructor.TransfData(gameData, null);

            }

        }

        //CardSet
        public static void SaveCardSet(CardSet cardSet, string path)
        {

            XElement root = new XElement("root");

            root.Add(new XElement("Name", cardSet.Name));

            int a = cardSet.OrigCard.Count;
            root.Add(new XElement("OrigCard", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("OrigCard" + i, cardSet.OrigCard[i]));
            }

            root.Add(new XElement("OrigCount", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("OrigCount" + i, cardSet.OrigCount[i]));
            }

            root.Add(new XElement("AllCard", 40));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}.xml", saveDoc.ToString());

        }
        public static CardSet LoadCardSet( string path)
        {
            CardSet cardSet = new CardSet();
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                cardSet.Name = root.Element("Name").Value;

                cardSet.AllCard = int.Parse(root.Element("AllCard").Value);

                int a = int.Parse(root.Element("OrigCard").Value); 
                cardSet.OrigCard = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardSet.OrigCard.Add(int.Parse(root.Element($"OrigCard{i}").Value));
                }

                cardSet.OrigCount = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardSet.OrigCount.Add(int.Parse(root.Element($"OrigCount{i}").Value));
                }


                //colodConstructor.TransfData(cardSet);

            }
            return cardSet;
        }

        # region Card
        public static void Save(CardBase cardBase, string path)
        {

            XElement root = new XElement("root");

            root.Add(new XElement("Name", cardBase.Name));

            root.Add(new XElement("Guild", cardBase.Guilds.Name));

            root.Add(new XElement("Races", cardBase.Races.Name));
            root.Add(new XElement("Legions", cardBase.Legions.Name));
            root.Add(new XElement("CivilianGroups", cardBase.CivilianGroups.Name));

            root.Add(new XElement("Mana", cardBase.Mana));
            int a = cardBase.Stat.Count;
            root.Add(new XElement("Stat", a));
            for (int i = 0; i < a; i++)
            {
                if(cardBase.Stat[i] != null)
                    root.Add(new XElement("Stat" + i, cardBase.Stat[i].Name));
                else
                    root.Add(new XElement("Stat" + i, " "));

                root.Add(new XElement("StatSize" + i, cardBase.StatSize[i]));
                //root.Add(new XElement("Trait" + i, cardBase.Trait[i]));
            }

            a = cardBase.Trait.Count;
            root.Add(new XElement("Trait", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("Trait" + i, cardBase.Trait[i].Name));
                root.Add(new XElement("TraitSize" + i, cardBase.TraitSize[i]));
            }

            string path1 = "";
            a = cardBase.Image.Length;
            root.Add(new XElement("Image", a));
            for (int i = 0; i < a; i++)
            {
                path1 += $"{cardBase.Image[i]}.";
            }
            root.Add(new XElement("ImageSt", path1));

            //string[] subs = path1.Split('.');
            //byte[] = string[] subs

            /*
             byte[] bytes = Encoding.ASCII.GetBytes(someString);
Вам нужно будет превратить его обратно в строку, подобную этой:

string someString = Encoding.ASCII.GetString(bytes);
             */

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}.xml", saveDoc.ToString());

        }



        public static CardBase Load(string path)
        {
            CardBase cardBase = new CardBase();
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");


                cardBase.Name = root.Element("Name").Value;
                //Debug.Log(path);

                string data = root.Element("Guild").Value;
                int a = gameSetting.Library.Guilds.FindIndex(x => x.Name == data);
                if (a < 0)
                    Debug.Log(data);
                cardBase.Guilds = gameSetting.Library.Guilds[a];

                data = root.Element("Races").Value;
                a = cardBase.Guilds.Races.FindIndex(x => x.Name == data);
                if (a < 0)
                    Debug.Log(data);
                cardBase.Races = cardBase.Guilds.Races[a];

                data = root.Element("Legions").Value;
                a = cardBase.Guilds.Legions.FindIndex(x => x.Name == data);
                if (a < 0)
                    Debug.Log(data);
                cardBase.Legions = cardBase.Guilds.Legions[a];

                data = root.Element("CivilianGroups").Value;
                a = cardBase.Legions.CivilianGroups.FindIndex(x => x.Name == data);
                if (a < 0)
                    Debug.Log(data);
                cardBase.CivilianGroups = cardBase.Legions.CivilianGroups[a];


                cardBase.Mana = int.Parse(root.Element("Mana").Value);


                data = " ";
                a = int.Parse(root.Element("Stat").Value);
                int b = 0;
                cardBase.Stat = new List<Constant>();
                cardBase.StatSize = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Stat.Add(null);
                    cardBase.StatSize.Add(0);
                    //cardBase.StatSize[i];

                    data = root.Element($"Stat{i}").Value;
                    if (data != "")
                    {
                        b = gameSetting.Library.Constants.FindIndex(x => x.Name == data);
                        if (b >= 0)
                        {
                            cardBase.Stat[i] = gameSetting.Library.Constants[b];
                            cardBase.StatSize[i] = int.Parse(root.Element($"StatSize{i}").Value);
                        }
                    }
                    cardBase.StatSizeLocal.Add(cardBase.StatSize[i]);

                    //else
                    //    Debug.Log(data);

                    //  cardBase.Stat.Add( int.Parse(root.Element($"Stat{cardBase.Stat.name}").Value));
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new List<HeadSimpleTrigger>();
                cardBase.TraitSize = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait.Add(null);
                    cardBase.TraitSize.Add(0);

                    data = root.Element($"Trait{i}").Value;
                    //Debug.Log()
                    b = gameSetting.Rule.FindIndex(x => x.Name == data);
                    if (b >= 0)
                    {
                        cardBase.Trait[i] = gameSetting.Rule[b];
                        cardBase.TraitSize[i] = int.Parse(root.Element($"TraitSize{i}").Value);
                    }
                }

                //Load Image
                string path1 = root.Element($"ImageSt").Value;

                string[] subs = path1.Split('.');
                a = int.Parse(root.Element($"Image").Value);
                byte[] bat = new byte[a];
                for (int i = 0; i < a; i++)
                {
                    bat[i] = byte.Parse(subs[i]);
                }
                cardBase.Image = bat;





            }
            return cardBase;
        }
        #endregion



        #region Rule
        public static void LoadMainRule( ActionLibrary library)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                int b = int.Parse(root.Element("RuleCount").Value);

                List<string> text = new List<string>();
                for(int i = 0; i < b; i++)
                {
                    text.Add(root.Element($"RuleName{i}").Value);
                }
               library.RuleName = text;
            }
        }
        public static void SaveMainRule( ActionLibrary library)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
            if (path != "")
            {
                XElement root = new XElement("root");

                int b = library.RuleName.Count;

                root.Add(new XElement("RuleCount", b));

                for(int i = 0; i < b; i++)
                {
                    root.Add(new XElement($"RuleName{i}", library.RuleName[i]));
                }


                XDocument saveDoc = new XDocument(root);
                File.WriteAllText($"{path}.xml", saveDoc.ToString());
            }
        }

        private static string SaveRuleCore(RuleForm ruleForm)
        {
            string text = "" + ruleForm.Card
                + "_" + ruleForm.StatTayp
                + "_" + ruleForm.Stat
                + "_" + ruleForm.Mod
                + "_" + ruleForm.Num;

            return text;
        }
        private static string SaveRuleCoreSimple(RuleForm ruleForm)
        {
            string text = "" + frame.CardString[ruleForm.Card] 
                + ":" + ruleForm.StatTayp 
                + "_" + ruleForm.Stat 
                + "_" + ruleForm.Mod 
                + "_" + ruleForm.Num;

            return text;
        }
        private static RuleForm LoadRuleCore(string str)
        {
            RuleForm ruleForm = new RuleForm();

            string[] subs = str.Split('_');
            ruleForm.Card = int.Parse(subs[0]);
            ruleForm.StatTayp = subs[1];
            ruleForm.Stat = subs[2];
            ruleForm.Mod = int.Parse(subs[3]);
            ruleForm.Num = int.Parse(subs[4]);

            return ruleForm;
        }

        private static XElement SaveRuleIfAction(IfAction ifAction, XElement root, string text)
        {
            RuleForm ifCore = null;
            
            root.Add(new XElement($"{text}Result", ifAction.Result));
            root.Add(new XElement($"{text}Prioritet", ifAction.Prioritet));
            root.Add(new XElement($"{text}Point", ifAction.Point));

            root.Add(new XElement($"{text}CoreCount", ifAction.Core.Count));
            string str = "";// + b2 = ifAction.Core.; 
            for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
            {
                ifCore = ifAction.Core[i2];
                str = SaveRuleCore(ifCore);
                root.Add(new XElement($"{text}Core{i2}", str));
            }
            return root;
        }
        private static XElement SaveRuleAction(RuleAction ifAction, XElement root, string text)
        {
            RuleForm ifCore = null;
            root.Add(new XElement($"{text}Point", $"{ifAction.MinPoint}|{ifAction.MaxPoint}"));
            root.Add(new XElement($"{text}ActionMood", ifAction.ActionMood));
            root.Add(new XElement($"{text}Action", ifAction.Action));
           // root.Add(new XElement($"{text}Num", ifAction.Num));
            root.Add(new XElement($"{text}ForseMood", ifAction.ForseMood));

            root.Add(new XElement($"{text}CoreCount", ifAction.Core.Count));
            string str = "";// + b2 = ifAction.Core.; 
            for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
            {
                ifCore = ifAction.Core[i2];
                str = SaveRuleCore(ifCore);
                root.Add(new XElement($"{text}Core{i2}", str));
            }
            return root;
        }

        private static IfAction LoadRuleIfAction(XElement root, string text)
        {
            IfAction ifAction = new IfAction();

            ifAction.Result = int.Parse(root.Element($"{ text}Result").Value);
            ifAction.Prioritet = int.Parse(root.Element($"{ text}Prioritet").Value);
            ifAction.Point = int.Parse(root.Element($"{ text}Point").Value);

            int a = int.Parse(root.Element($"{ text}CoreCount").Value);

            for (int i = 0; i < a; i++)
            {
                ifAction.Core.Add(LoadRuleCore(root.Element($"{text}Core{i}").Value));
            }

            return ifAction;
        }
        private static RuleAction LoadRuleAction(XElement root, string text)
        {
            RuleAction ifAction = new RuleAction();

            string str = root.Element($"{ text}Point").Value;
            string[] subs = str.Split('|');

            ifAction.MinPoint = int.Parse(subs[0]);
            ifAction.MaxPoint = int.Parse(subs[1]);

            ifAction.ActionMood = int.Parse(root.Element($"{ text}ActionMood").Value);
            ifAction.Action = root.Element($"{ text}Action").Value;
           // ifAction.Num = int.Parse(root.Element($"{ text}Num").Value);
            ifAction.ForseMood = int.Parse(root.Element($"{ text}ForseMood").Value);
            
            int a = int.Parse(root.Element($"{ text}CoreCount").Value); 
            
            for (int i = 0; i < a; i++)
            {
                ifAction.Core.Add(LoadRuleCore(root.Element($"{text}Core{i}").Value));
            }

            return ifAction;

        }

        private static string SaveRuleIfActionSimple(IfAction ifAction)
        {
            string text =
                "" + frame.EqualString[ifAction.Result]
                + "_" + ifAction.Prioritet
                + "_" + ifAction.Point + "*";
            int b = ifAction.Core.Count;
            for (int i = 0; i < b; i++)
            {
                text += SaveRuleCoreSimple(ifAction.Core[i]);
                if (i + 1 != b)
                    text += "|";
            }
            return text;
        }
        private static string SaveRuleActionSimple(RuleAction ifAction)
        {
            string text =
                "" + ifAction.MinPoint
                + "_" + ifAction.MaxPoint
                + "_" + frame.PlayerString[ifAction.ActionMood]
                + "_" + ifAction.Action
              //  + "_" + ifAction.Num
                + "_" + frame.ForseTayp[ifAction.ForseMood]
                + "*";

            int b = ifAction.Core.Count;
            for (int i = 0; i < b; i++)
            {
                text += SaveRuleCoreSimple(ifAction.Core[i]);
                if (i + 1 != b)
                    text += "|";
            }

            return text;
        }


        public static void SaveSimpleRule(HeadRule head, int a)
        {
            SimpleTrigger simpleTrigger = new SimpleTrigger();
            string path = Application.dataPath + $"/Resources/Data/Rule/SimpleRule{a}"; ;
            if (path != "")
            {
                XElement root = new XElement("root");
                string str = "" + head.Name
                   + "_" + head.Cost
                   + "_" + head.CostExtend
                   + "_" + head.LevelCap
                   + "_" + head.Player;
                root.Add(new XElement("Head", str));

                str = "";
                int b = head.NeedRule.Count;
                for (int i = 0; i < b; i++)
                {
                    str += head.NeedRule[i];
                    if (i + 1 != b)
                        str += "_";
                }
                root.Add(new XElement("HeadNeedRule", str));

                str = "";
                b = head.EnemyRule.Count;
                for (int i = 0; i < b; i++)
                {
                    str += head.EnemyRule[i];
                    if (i + 1 != b)
                        str += "_";
                }
                root.Add(new XElement("HeadEnemyRule", str));

                str = "";
                TriggerAction triggerAction = null;
                IfAction ifAction = null;
                RuleAction ruleAction = null;
                string text = "";
                int b1 = 0;
                int b2 = 0;
                b = head.TriggerActions.Count;

                for (int i = 0; i < b; i++)
                {
                    triggerAction = head.TriggerActions[i];
                    str += frame.PlayerString[triggerAction.TargetPalyer]
                        +"_" + frame.Trigger[triggerAction.Trigger]
                        + "_" + triggerAction.CountMod
                        + "_" + triggerAction.CountModExtend;

                    text = "";
                    b1 = triggerAction.PlusAction.Count;
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text += SaveRuleIfActionSimple(triggerAction.PlusAction[i1]);
                        if (i1 + 1 != b1)
                            text += "/";
                    }
                    root.Add(new XElement($"Trigger{i}PlusAction", text));

                    text = "";
                    b1 = triggerAction.MinusAction.Count;
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text += SaveRuleIfActionSimple(triggerAction.MinusAction[i1]);
                        if (i1 + 1 != b1)
                            text += "/";
                    }
                    root.Add(new XElement($"Trigger{i}MinusAction", text));


                    text = "";
                    b1 = triggerAction.Action.Count;
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text += SaveRuleActionSimple(triggerAction.Action[i1]);
                        if (i1 + 1 != b1)
                            text += "/";
                    }
                    root.Add(new XElement($"Trigger{i}Action", text));


                    if (i + 1 != b)
                        str += "|";
                }
                root.Add(new XElement("TriggerHead", str));
                XDocument saveDoc = new XDocument(root);
                File.WriteAllText($"{path}.xml", saveDoc.ToString());
            }

        }


        private static XElement root;
        public static void SetSimpleRoot(int a)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/SimpleRule{a}";
            if (path != "")
            {
                root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
            }
        }
        public static string LoadSimpleRule(string mood, int b = 0 )
        {
            string path = "";
            switch (mood)
            {
                case ("Head"):
                    path = root.Element($"Head").Value;
                    break;
                case ("HeadNeedRule"):
                    path = root.Element($"HeadNeedRule").Value;
                    break;
                case ("HeadEnemyRule"):
                    path = root.Element($"HeadEnemyRule").Value;
                    break;
                case ("Trigger"):
                    path = root.Element($"TriggerHead").Value;
                    break;
                case ("TriggerPartPlus"):
                    path = $"{root.Element($"Trigger{b}PlusAction").Value}";
                    break;
                case ("TriggerPartMinus"):
                    path = $"{root.Element($"Trigger{b}MinusAction").Value}";
                    break;
                case ("TriggerPart"):
                    path = $"{root.Element($"Trigger{b}Action").Value}";
                    break;
            }
            return path;
        }

        public static void SaveRule(HeadRule head, int a)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/Rule{a}"; ;
            if (path != "")
            {
                XElement root = new XElement("root");

                root.Add(new XElement("HeadName", head.Name));
                root.Add(new XElement("HeadCost", head.Cost));
                root.Add(new XElement("HeadNameText", head.NameText));
                root.Add(new XElement("Tag", head.Tag));
                root.Add(new XElement("HeadCostExtend", head.CostExtend));
                root.Add(new XElement("HeadLevelCap", head.LevelCap));
                root.Add(new XElement("HeadPlayer", head.Player));


                int b = head.NeedRule.Count;
                root.Add(new XElement("HeadNeedRuleCount", b));
                for (int i = 0; i < b; i++)
                {
                    root.Add(new XElement($"HeadNeedRule{i}", head.NeedRule[i]));
                }

                b = head.EnemyRule.Count;
                root.Add(new XElement("HeadEnemyRuleCount", b));
                for (int i = 0; i < b; i++)
                {
                    root.Add(new XElement($"HeadEnemyRule{i}", head.EnemyRule[i]));
                }


                TriggerAction triggerAction = null;
                IfAction ifAction = null;
                RuleAction ruleAction = null;

                b = head.TriggerActions.Count;

                root.Add(new XElement("TriggerActionCount", b));

                string text = "";
                int b1 = 0;
                int b2 = 0;

                for (int i = 0; i < b; i++)
                {
                    triggerAction = head.TriggerActions[i];


                    //public bool CountMod;
                    //public bool CountModExtend;
                    root.Add(new XElement($"TargetPalyer{i}", triggerAction.TargetPalyer));
                    root.Add(new XElement($"Trigger{i}", triggerAction.Trigger));


                    root.Add(new XElement($"CountMod{i}", triggerAction.CountMod));
                    root.Add(new XElement($"CountModExtend{i}", triggerAction.CountModExtend));


                    b1 = triggerAction.PlusAction.Count;
                    root.Add(new XElement($"PlusActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = triggerAction.PlusAction[i1];
                        text = $"Trigger{i}PlusAction{i1}";
                        SaveRuleIfAction(ifAction, root, text);

                    }



                    b1 = triggerAction.MinusAction.Count;
                    root.Add(new XElement($"MinusActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = triggerAction.MinusAction[i1];
                        text = $"Trigger{i}MinusAction{i1}";
                        root = SaveRuleIfAction(ifAction, root, text);
                    }



                    b1 = triggerAction.Action.Count;
                    root.Add(new XElement($"ActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ruleAction = triggerAction.Action[i1];
                        text = $"Trigger{i}Action{i1}";

                        root = SaveRuleAction(ruleAction, root, text);
                    }
                }


                XDocument saveDoc = new XDocument(root);
                File.WriteAllText($"{path}.xml", saveDoc.ToString());
            }
        }

        public static HeadRule LoadRule(int a)
        {
            HeadRule head = null;
            string path = Application.dataPath + $"/Resources/Data/Rule/Rule{a}";
            if (path != "")
            {

                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                head = new HeadRule();
                head.Name = root.Element($"HeadName").Value;
                head.Cost = int.Parse(root.Element($"HeadCost").Value);
                head.NameText = root.Element($"HeadNameText").Value;
                head.Tag = root.Element($"Tag").Value;

                head.CostExtend = int.Parse(root.Element($"HeadCostExtend").Value);
                head.LevelCap = int.Parse(root.Element($"HeadLevelCap").Value);
                //head.CostMovePoint = int.Parse(root.Element($"CostMovePoint").Value);
                head.Player = bool.Parse(root.Element($"HeadPlayer").Value);


                int b = int.Parse(root.Element("HeadNeedRuleCount").Value);
                for (int i = 0; i < b; i++)
                {
                    head.NeedRule.Add(root.Element($"HeadNeedRule{i}").Value);
                }

                b = int.Parse(root.Element("HeadEnemyRuleCount").Value);
                for (int i = 0; i < b; i++)
                {
                    head.EnemyRule.Add(root.Element($"HeadEnemyRule{i}").Value);
                }


                TriggerAction triggerAction = null;
                IfAction ifAction = null;
                RuleAction ruleAction = null;


                string text = "";
                int b1 = 0;
                int b2 = 0;

                b = int.Parse(root.Element("TriggerActionCount").Value);
                for (int i = 0; i < b; i++)
                {
                    triggerAction = new TriggerAction();

                    triggerAction.PlusAction = new List<IfAction>();
                    triggerAction.MinusAction = new List<IfAction>();
                    triggerAction.Action = new List<RuleAction>();


                    triggerAction.CountMod = bool.Parse(root.Element($"CountMod{i}").Value);
                    triggerAction.CountModExtend = bool.Parse(root.Element($"CountModExtend{i}").Value);

                    triggerAction.TargetPalyer = int.Parse(root.Element($"TargetPalyer{i}").Value);
                    triggerAction.Trigger = int.Parse(root.Element($"Trigger{i}").Value);

                    b1 = int.Parse(root.Element($"PlusActionCount{i}").Value);
                    //Debug.Log(b1);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text = $"Trigger{i}PlusAction{i1}";
                        triggerAction.PlusAction.Add(LoadRuleIfAction(root, text));

                    }

                    b1 = int.Parse(root.Element($"MinusActionCount{i}").Value);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text = $"Trigger{i}MinusAction{i1}";
                        triggerAction.MinusAction.Add(LoadRuleIfAction(root, text));
                    }

                    b1 = int.Parse(root.Element($"ActionCount{i}").Value);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text = $"Trigger{i}Action{i1}";
                        
                        triggerAction.Action.Add(LoadRuleAction(root, text));
                    }

                    head.TriggerActions.Add(triggerAction);
                }



                // ruleConstructor.SetRule(head);

            }
            return head;
        }
        #endregion
    }
}
