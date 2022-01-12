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

            int a = cardBase.Stat.Count;
            root.Add(new XElement("Stat", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("Stat" + i, cardBase.Stat[i]));
                root.Add(new XElement("Trait" + i, cardBase.Trait[i]));
            }

            a = cardBase.Trait.Count;
            root.Add(new XElement("Rule", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("Rule" + i, cardBase.Rule[i]));
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

        public static void Load(string path, CardConstructor cardConstructor)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;

                int a = int.Parse(root.Element("Stat").Value);
                cardBase.Stat = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Stat.Add( int.Parse(root.Element($"Stat{i}").Value));
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new List<string>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait.Add(root.Element($"Trait{i}").Value);
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


                cardConstructor.LocalCard.Add(cardBase);

            }
        }
        public static void Load(string path, ColodConstructor colodConstructor)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;

                int a = int.Parse(root.Element("Stat").Value);
                cardBase.Stat = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Stat.Add(int.Parse(root.Element($"Stat{i}").Value));
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new List<string>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait.Add(root.Element($"Trait{i}").Value);
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

                colodConstructor.LocalCard.Add(cardBase);

            }
        }
        public static void Load(string path, Stol stol)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;

                int a = int.Parse(root.Element("Stat").Value);
                cardBase.Stat = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Stat.Add(int.Parse(root.Element($"Stat{i}").Value));
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new List<string>();
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait.Add(root.Element($"Trait{i}").Value);
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

                stol.BufferColod.Add(cardBase);

            }
        }
        #endregion



        #region Rule
        public static void LoadMainRule(RuleConstructor ruleConstructor)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                int b = int.Parse(root.Element("RuleCount").Value);//= 0;
                ruleConstructor.RuleCount = b;
             //  RuleCount = int.Parse(mainRule[b]);
              //  b++;
                List<string> text = new List<string>();
                //b = int.Parse(root.Element("RuleCount").Value);//= 0;
                for(int i = 0; i < b; i++)
                {
                    text.Add(root.Element($"RuleName{i}").Value);
                }
                ruleConstructor.RuleName = text;
            }
        }
        public static void SaveMainRule(RuleConstructor ruleConstructor)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
            if (path != "")
            {
                XElement root = new XElement("root");

                int b = ruleConstructor.RuleCount;

                root.Add(new XElement("RuleCount", b));

                for(int i = 0; i < b; i++)
                {
                    root.Add(new XElement($"RuleName{i}", ruleConstructor.RuleName[i]));
                }


                XDocument saveDoc = new XDocument(root);
                File.WriteAllText($"{path}.xml", saveDoc.ToString());
            }
        }

        public static void SaveRule(List<string> mainRule, List<TriggerAction> triggerAction,int a)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/Rule{a}"; ;
            if (path != "")
            {
                XElement root = new XElement("root");

                int b = mainRule.Count;

                root.Add(new XElement("MainRuleCount", b));

                for (int i = 0; i < b; i++)
                {
                    root.Add(new XElement($"MainRule{i}", mainRule[i]));
                }


                TriggerAction triggerAction1 = null;
                IfAction ifAction = null;
                RuleAction ruleAction = null;

                 b = triggerAction.Count;

                root.Add(new XElement("TriggerActionCount", b));

                string text = "";
                int b1 = 0;
                int b2 = 0;
                for (int i = 0; i < b; i++)
                {
                    triggerAction1 = triggerAction[i];
                    root.Add(new XElement($"TargetPalyer{i}", triggerAction1.TargetPalyer));
                    root.Add(new XElement($"TargetTime{i}", triggerAction1.TargetTime));


                    b1 = triggerAction[i].PlusAction.Count;
                    root.Add(new XElement($"PlusActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = triggerAction[i].PlusAction[i1];
                        text = $"Trigger{i}PlusAction{i1}";

                        root.Add(new XElement($"{text}Prioritet", ifAction.Prioritet));

                        b2 = ifAction.IntData.Count;
                        root.Add(new XElement($"{text}IntDataCount", b2));
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            root.Add(new XElement($"{text}IntData{i2}", ifAction.IntData[i2]));
                        }

                        root.Add(new XElement($"{text}MainCore1", ifAction.MainCore[0]));
                        root.Add(new XElement($"{text}MainCore2", ifAction.MainCore[1]));


                        b2 = ifAction.Core.Count;
                        root.Add(new XElement($"{text}CoreCount", b2));
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            root.Add(new XElement($"{text}Core{i2}", ifAction.Core[i2]));
                        }


                    }



                    b1 = triggerAction[i].MinusAction.Count;
                    root.Add(new XElement($"MinusActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = triggerAction[i].MinusAction[i1];
                        text = $"Trigger{i}MinusAction{i1}";

                        root.Add(new XElement($"{text}Prioritet", ifAction.Prioritet));

                        b2 = ifAction.IntData.Count;
                        root.Add(new XElement($"{text}IntDataCount", b2));
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            root.Add(new XElement($"{text}IntData{i2}", ifAction.IntData[i2]));
                        }

                        root.Add(new XElement($"{text}MainCore1", ifAction.MainCore[0]));
                        root.Add(new XElement($"{text}MainCore2", ifAction.MainCore[1]));


                        b2 = ifAction.Core.Count;
                        root.Add(new XElement($"{text}CoreCount", b2));
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            root.Add(new XElement($"{text}Core{i2}", ifAction.Core[i2]));
                        }


                    }



                    b1 = triggerAction[i].Action.Count;
                    root.Add(new XElement($"ActionCount{i}", b1));
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ruleAction = triggerAction[i].Action[i1];
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
        public static void LoadRule(RuleConstructor ruleConstructor, int a)
        {
            string path = Application.dataPath + $"/Resources/Data/Rule/Rule{a}"; ;
            if (path != "")
            {
                List<string> mainRule = new List<string>();
                List<TriggerAction> triggerAction = new List<TriggerAction>();

                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                int b = int.Parse(root.Element("MainRuleCount").Value);

                for (int i = 0; i < b; i++)
                {
                    mainRule.Add(root.Element($"MainRule{i}").Value);
                }


                TriggerAction triggerAction1 = null;
                IfAction ifAction = null;
                RuleAction ruleAction = null;


                string text = "";
                int b1 = 0;
                int b2 = 0;

                b = int.Parse(root.Element("TriggerActionCount").Value);
                for (int i = 0; i < b; i++)
                {
                    triggerAction1 = new TriggerAction();

                    triggerAction1.PlusAction = new List<IfAction>();
                    triggerAction1.MinusAction = new List<IfAction>();
                    triggerAction1.Action = new List<RuleAction>();

                    // text.Add(root.Element($"RuleName{i}").Value);
                    triggerAction1.TargetPalyer = int.Parse(root.Element($"TargetPalyer{i}").Value);
                    triggerAction1.TargetTime = int.Parse(root.Element($"TargetTime{i}").Value);

                    b1 = int.Parse(root.Element($"PlusActionCount{i}").Value);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = new IfAction();
                        text = $"Trigger{i}PlusAction{i1}";

                        ifAction.Prioritet = int.Parse(root.Element($"{text}Prioritet").Value);

                        b2 = int.Parse(root.Element($"{text}IntDataCount").Value);
                        ifAction.IntData = new List<int>();
                        ifAction.TextData = new List<string>();
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            ifAction.TextData.Add("");
                            ifAction.IntData.Add(int.Parse(root.Element($"{text}IntData{i2}").Value));
                        }

                        ifAction.MainCore[0] = int.Parse(root.Element($"{text}MainCore1").Value);
                        ifAction.MainCore[1] = int.Parse(root.Element($"{text}MainCore2").Value);


                        ifAction.Core = new List<int>();
                        b2 = int.Parse(root.Element($"{text}CoreCount").Value);
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            ifAction.Core.Add(int.Parse(root.Element($"{text}Core{i2}").Value));
                        }
                        triggerAction1.PlusAction.Add(ifAction);
                    }

                    b1 = int.Parse(root.Element($"MinusActionCount{i}").Value);
                    for (int i1 = 0; i1 < b1; i1++)
                    {
                        ifAction = new IfAction();
                        text = $"Trigger{i}MinusAction{i1}";

                        ifAction.Prioritet = int.Parse(root.Element($"{text}Prioritet").Value);

                        b2 = int.Parse(root.Element($"{text}IntDataCount").Value);
                        ifAction.IntData = new List<int>();
                        ifAction.TextData = new List<string>();
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            ifAction.TextData.Add("");
                            ifAction.IntData.Add(int.Parse(root.Element($"{text}IntData{i2}").Value));
                        }

                        ifAction.MainCore[0] = int.Parse(root.Element($"{text}MainCore1").Value);
                        ifAction.MainCore[1] = int.Parse(root.Element($"{text}MainCore2").Value);


                        ifAction.Core = new List<int>();
                        b2 = int.Parse(root.Element($"{text}CoreCount").Value);
                        for (int i2 = 0; i2 < b2; i2++)
                        {
                            ifAction.Core.Add(int.Parse(root.Element($"{text}Core{i2}").Value));
                        }
                        triggerAction1.MinusAction.Add(ifAction);
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

                        triggerAction1.Action.Add(ruleAction);
                    }

                    triggerAction.Add(triggerAction1);
                }



                ruleConstructor.SetRule(mainRule, triggerAction);

            }
        }
        #endregion
    }
}
