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
        private static CardConstructor cardConstructor;
        private static RuleConstructor ruleConstructor;
        private static ColodConstructor colodConstructor;
        private static Stol stol;


        public static void SetCardConstructor(CardConstructor _cardConstructor)
        {
            cardConstructor = _cardConstructor;
        }
        public static void SetColodConstructor(ColodConstructor _colodConstructor)
        {
            colodConstructor = _colodConstructor;
        }
        public static void SetStol(Stol _stol)
        {
            stol = _stol;
        }
        public static void SetGameSetting(GameSetting _gameSetting)
        {
            gameSetting = _gameSetting;
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
        public static void LoadCardSet( string path, ColodConstructor colodConstructor)
        {

            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardSet cardSet = new CardSet();
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


                colodConstructor.TransfData(cardSet);

            }
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
                root.Add(new XElement("Trait" + i, cardBase.Trait[i]));
                root.Add(new XElement("TraitSize" + i, cardBase.TraitSize[i]));
            }

            string path1 = "";
            a = cardBase.Image.Length;
            root.Add(new XElement("Image", a));
            for (int i = 0; i < a; i++)
            {
                path1 += $"{cardBase.Image[i]}.";
                // root.Add(new XElement("Trait" + i, cardBase.Trait[i]));
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



        public static void Load(string path, string mood)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;

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

                    //else
                    //    Debug.Log(data);

                    //  cardBase.Stat.Add( int.Parse(root.Element($"Stat{cardBase.Stat.name}").Value));
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new List<string>();
                cardBase.TraitSize = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait.Add(root.Element($"Trait{i}").Value);
                    cardBase.TraitSize.Add(int.Parse(root.Element($"TraitSize{i}").Value));
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


                switch (mood) 
                {
                    case ("Card"):
                        cardConstructor.LocalCard.Add(cardBase);
                        break;

                    case ("Colod"):
                        colodConstructor.LocalCard.Add(cardBase);
                        break;


                    case ("Stol"):
                        stol.BufferColod.Add(cardBase);
                        break;
                }


            }
        }
        #endregion



        #region Rule
        public static void LoadMainRule( ActionLibrary library)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                int b = int.Parse(root.Element("RuleCount").Value);//= 0;
                //if(ruleConstructor != null)
                //    ruleConstructor.RuleCount = b;

                List<string> text = new List<string>();
                for(int i = 0; i < b; i++)
                {
                    text.Add(root.Element($"RuleName{i}").Value);
                }
               // if (ruleConstructor != null)
                //    ruleConstructor.RuleName = text;
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



        private static XElement SaveRuleRoot(IfAction ifAction, XElement root, string text)
        {
            IfCore ifCore = null;

            root.Add(new XElement($"{text}Prioritet", ifAction.Prioritet));

            root.Add(new XElement($"{text}MainCore1", ifAction.MainCore[0]));
            root.Add(new XElement($"{text}MainCore2", ifAction.MainCore[1]));


            root.Add(new XElement($"{text}CoreCount", ifAction.Core.Count));
            string str = "";// + b2 = ifAction.Core.; 
            for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
            {
                ifCore = ifAction.Core[i2];
                str = "" + ifCore.Core;
                for (int i3 = 0; i3 < ifCore.IntData.Count; i3++)
                {
                    str += $"_{ifCore.IntData[i3]}";
                }
                root.Add(new XElement($"{text}Core{i2}", str));
            }
            return root;
        }

        private static IfAction LoadRuleRoot(XElement root, string text)
        {

            IfCore ifCore = null;
            IfAction ifAction = new IfAction();
            //ifAction.LocalId = i;
            //Debug.Log(ifAction.LocalId);

            ifAction.Prioritet = int.Parse(root.Element($"{text}Prioritet").Value);

            ifAction.MainCore[0] = int.Parse(root.Element($"{text}MainCore1").Value);
            ifAction.MainCore[1] = int.Parse(root.Element($"{text}MainCore2").Value);


            ifAction.Core = new List<IfCore>();
            //ifCore = ruleConstructor.NewIfCore();
            int b2 = int.Parse(root.Element($"{text}CoreCount").Value);

            for (int i2 = 0; i2 < b2; i2++)
            {
                ifCore = new IfCore();
                ifCore.TextData = new List<string>();
                ifCore.IntData = new List<int>();
                string str = root.Element($"{text}Core{i2}").Value;

                string[] number = str.Split('_');

                ifCore.Core = int.Parse(number[0]);

                for (int i3 = 1; i3 < number.Length; i3++)
                {
                    ifCore.TextData.Add("");
                    ifCore.IntData.Add(int.Parse(number[i3]));
                }

                //ifAction.Core.Add(int.Parse(root.Element($"{text}Core{i2}").Value));
                ifAction.Core.Add(ifCore);

                //ruleConstructor.PreLableIfAction(ifAction, ifCore);

            }
            return ifAction;
        }

        public static void SaveRule(HeadRule head,int a)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/Rule{a}"; ;
            if (path != "")
            {
                XElement root = new XElement("root");

                root.Add(new XElement("HeadName", head.Name));
                root.Add(new XElement("HeadCost", head.Cost));
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
                    root.Add(new XElement($"HeadEnemyRule{i}", head.NeedRule[i]));
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
                    root.Add(new XElement($"TargetPalyer{i}", triggerAction.TargetPalyer));
                    root.Add(new XElement($"TargetTime{i}", triggerAction.TargetTime));


                    b1 = triggerAction.PlusAction.Count;
                    root.Add(new XElement($"PlusActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = triggerAction.PlusAction[i1];
                        text = $"Trigger{i}PlusAction{i1}";
                        SaveRuleRoot(ifAction, root, text);

                    }



                    b1 = triggerAction.MinusAction.Count;
                    root.Add(new XElement($"MinusActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = triggerAction.MinusAction[i1];
                        text = $"Trigger{i}MinusAction{i1}";
                        SaveRuleRoot(ifAction, root, text);
                    }



                    b1 = triggerAction.Action.Count;
                    root.Add(new XElement($"ActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ruleAction = triggerAction.Action[i1];
                        text = $"Trigger{i}Action{i1}";

                        root.Add(new XElement($"{text}Mood", ruleAction.Mood));
                        root.Add(new XElement($"{text}TargetPalyer", ruleAction.TargetPalyer));
                        root.Add(new XElement($"{text}EffectData", ruleAction.EffectData));
                        root.Add(new XElement($"{text}RelizeConstant", ruleAction.RelizeConstant));
                        root.Add(new XElement($"{text}TargetConstant", ruleAction.TargetConstant));
                        root.Add(new XElement($"{text}IntData", ruleAction.IntData));

                    }
                }


                XDocument saveDoc = new XDocument(root);
                File.WriteAllText($"{path}.xml", saveDoc.ToString());
            }
        }
        public static void LoadRule(RuleConstructor _ruleConstructor, int a)
        {
            ruleConstructor = _ruleConstructor;
            string path = Application.dataPath + $"/Resources/Data/Rule/Rule{a}";
            if (path != "")
            {

                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                HeadRule head = new HeadRule();
                head.Name = root.Element($"HeadName").Value;
                head.Cost = int.Parse(root.Element($"HeadCost").Value);
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

                    // text.Add(root.Element($"RuleName{i}").Value);
                    triggerAction.TargetPalyer = int.Parse(root.Element($"TargetPalyer{i}").Value);
                    triggerAction.TargetTime = int.Parse(root.Element($"TargetTime{i}").Value);

                    b1 = int.Parse(root.Element($"PlusActionCount{i}").Value);
                    //Debug.Log(b1);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text = $"Trigger{i}PlusAction{i1}";
                        triggerAction.PlusAction.Add(LoadRuleRoot(root, text));

                    }

                    b1 = int.Parse(root.Element($"MinusActionCount{i}").Value);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        text = $"Trigger{i}MinusAction{i1}";
                        triggerAction.MinusAction.Add(LoadRuleRoot(root, text));
                    }

                    b1 = int.Parse(root.Element($"ActionCount{i}").Value);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ruleAction = new RuleAction();
                        text = $"Trigger{i}Action{i1}";

                        ruleAction.Mood = int.Parse(root.Element($"{text}Mood").Value);
                        ruleAction.TargetPalyer = int.Parse(root.Element($"{text}TargetPalyer").Value);
                        ruleAction.EffectData = int.Parse(root.Element($"{text}EffectData").Value);
                        ruleAction.RelizeConstant = int.Parse(root.Element($"{text}RelizeConstant").Value);
                        ruleAction.TargetConstant = int.Parse(root.Element($"{text}TargetConstant").Value);
                        ruleAction.IntData = int.Parse(root.Element($"{text}IntData").Value);
                        root.Add(new XElement($"{text}Mood", ruleAction.Mood));

                        triggerAction.Action.Add(ruleAction);
                    }

                    head.TriggerActions.Add(triggerAction);
                }



                ruleConstructor.SetRule(head);

            }
        }
        #endregion
    }
}
