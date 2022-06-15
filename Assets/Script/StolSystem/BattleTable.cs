using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Saver;

namespace BattleTable
{
    public static class Core
    {
        private static GameSetting gameSetting;

        public static void LoadGameSetting(GameSetting _gameSetting)
        {
            gameSetting = _gameSetting;
            CardView.gameSetting = _gameSetting;
            GameEventSystem.gameSetting = _gameSetting;
            HiroHead.gameSetting = _gameSetting;

            DataManager.gameSetting = _gameSetting;
        }

        public static void LoadRules()
        {
            string str;
            gameSetting.Rule = new List<HeadSimpleTrigger>();
            for (int i = 0; i < gameSetting.Library.Rule.Count; i++)
                for (int i1 = 0; i1 < gameSetting.Library.Rule[i].Rule.Count; i1++)
                {
                    str = gameSetting.Library.Rule[i].Name + "_" + gameSetting.Library.Rule[i].Rule[i1];
                    gameSetting.Rule.Add(Core.ReadRuleSimple(str));

                }

            //int a = 0;
            //gameSetting.DefRule = new List<HeadSimpleTrigger>();
            //for (int i = 0; i < gameSetting.DefRuleText.Count; i++)
            //{
            //    a = gameSetting.Library.RuleName.FindIndex(x => x == gameSetting.DefRuleText[i]);
            //    gameSetting.DefRule.Add(gameSetting.Rule[a]);
            //}
        }

        private static List<string> call = new List<string>();
        public static void CallRuleLoad( CardBase card)
        {
            int a =0;
            foreach(string str in card.Trait)
            {
                a = call.FindIndex(x => x == str);
                if (a == -1)
                    call.Add(str);
            }
        }

        public static void CallRuleFinish()
        {
            gameSetting.Rule = new List<HeadSimpleTrigger>();
            foreach (string str in call)
                gameSetting.Rule.Add(Core.ReadRule(str));

            call = null;
        }

        public static void GenerateActionCard(CardBase card)
        {
            card.PlayCard = new List<int>();
            card.PlayAnotherCard = new List<int>();

            card.Die = new List<int>();
            card.AnotherDie = new List<int>();

            card.Action = new List<int>();
            card.AnotherAction = new List<int>();

            card.PreAction = new List<int>();
            card.PostAction = new List<int>();
            //card.InHand;
            card.NextTurn = new List<int>();

            card.Status = new List<string>();

            int a = 0;
            int id = card.Id;
            HeadSimpleTrigger head = null;
            foreach (int i in card.TraitSize)
            {
                head = gameSetting.Rule[i];
                foreach (SimpleTrigger simpleTrigger in head.SimpleTriggers)
                {
                    a = simpleTrigger.CodName;
                    switch (simpleTrigger.Trigger)
                    {
                        case ("PreAction"):
                            card.PreAction.Add(a);
                            break;
                        case ("PostAction"):
                            card.PostAction.Add(a);
                            break;
                        case ("PlayCard"):
                            card.PlayCard.Add(a);
                            break;
                        case ("PlayAnotherCard"):
                            card.PlayAnotherCard.Add(a);
                            AddEcho("PlayAnotherCard", id);
                            break;

                        case ("Die"):
                            card.Die.Add(a);
                            break;
                        case ("AnotherDie"):
                            card.AnotherDie.Add(a);
                            AddEcho("AnotherDie", id);
                            break;

                        case ("Action"):
                            card.Action.Add(a);
                            break;
                        case ("AnotherAction"):
                            card.AnotherAction.Add(a);
                            AddEcho("AnotherAction", id);
                            break;
                        //case ("InHand"):
                        //    card.InHand.Add(a);
                        //    break;
                        case ("NextTurn"):
                            card.NextTurn.Add(a);

                            if (simpleTrigger.TargetPalyer != "Enemy")
                                AddEcho("NextTurn", id);
                            if (simpleTrigger.TargetPalyer != "My")
                                AddEcho("NextTurnElse", id);

                            break;
                    }
                }
            }
        }
        public static void GenerateAction()
        {
            gameSetting.PreAction = new List<SimpleTrigger>();
            gameSetting.PostAction = new List<SimpleTrigger>();
            gameSetting.PlayCard = new List<SimpleTrigger>();
            gameSetting.PlayAnotherCard = new List<SimpleTrigger>();
            gameSetting.Die = new List<SimpleTrigger>();
            gameSetting.AnotherDie = new List<SimpleTrigger>();
            gameSetting.Action = new List<SimpleTrigger>();
            gameSetting.AnotherAction = new List<SimpleTrigger>();
            gameSetting.NextTurn = new List<SimpleTrigger>();

            //for(int i =0;i< gameSetting.Rule.Count; i++)
            foreach (HeadSimpleTrigger head in gameSetting.Rule)
            {
                foreach (SimpleTrigger simpleTrigger in head.SimpleTriggers)
                {
                    switch (simpleTrigger.Trigger)
                    {
                        case ("PreAction"):
                            simpleTrigger.CodName = gameSetting.PreAction.Count;
                            gameSetting.PreAction.Add(simpleTrigger);
                            break;
                        case ("PostAction"):
                            simpleTrigger.CodName = gameSetting.PostAction.Count;
                            gameSetting.PostAction.Add(simpleTrigger);
                            break;
                        case ("PlayCard"):
                            simpleTrigger.CodName = gameSetting.PlayCard.Count;
                            gameSetting.PlayCard.Add(simpleTrigger);
                            break;
                        case ("PlayAnotherCard"):
                            simpleTrigger.CodName = gameSetting.PlayAnotherCard.Count;
                            gameSetting.PlayAnotherCard.Add(simpleTrigger);
                            break;

                        case ("Die"):
                            simpleTrigger.CodName = gameSetting.Die.Count;
                            gameSetting.Die.Add(simpleTrigger);
                            break;
                        case ("AnotherDie"):
                            simpleTrigger.CodName = gameSetting.AnotherDie.Count;
                            gameSetting.AnotherDie.Add(simpleTrigger);
                            break;

                        case ("Action"):
                            simpleTrigger.CodName = gameSetting.Action.Count;
                            gameSetting.Action.Add(simpleTrigger);
                            break;
                        case ("AnotherAction"):
                            simpleTrigger.CodName = gameSetting.AnotherAction.Count;
                            gameSetting.AnotherAction.Add(simpleTrigger);
                            break;
                        //case ("InHand"):
                        //    card.InHand.Add(a);
                        //    break;
                        case ("NextTurn"):
                            simpleTrigger.CodName = gameSetting.NextTurn.Count;
                            gameSetting.NextTurn.Add(simpleTrigger);
                            break;
                    }
                }
            }
            /*
             Action
            InHand
            NextTurn

             */

        }

        public static void AddEcho(string str, int id)
        {
            int a = 0;
            switch (str)
            {
                case ("PlayAnotherCard"):
                    a = gameSetting.PlayAnotherCardBody.FindIndex(x => x == id);
                    if (a != -1)
                        gameSetting.PlayAnotherCardBody.Add(id);
                    break;

                case ("AnotherDie"):
                    a = gameSetting.AnotherDieBody.FindIndex(x => x == id);
                    if (a != -1)
                        gameSetting.AnotherDieBody.Add(id);
                    break;

                case ("AnotherAction"):
                    a = gameSetting.AnotherActionBody.FindIndex(x => x == id);
                    if (a != -1)
                        gameSetting.AnotherActionBody.Add(id);
                    break;

                case ("NextTurn"):
                    a = gameSetting.NextTurnBody.FindIndex(x => x == id);
                    if (a != -1)
                        gameSetting.NextTurnBody.Add(id);
                    break;
                case ("NextTurnElse"):
                    a = gameSetting.NextTurnBodyElse.FindIndex(x => x == id);
                    if (a != -1)
                        gameSetting.NextTurnBodyElse.Add(id);

                    break;
            }
        }

        public static CardBase CardClone(CardBase card)
        {
            CardBase newCard = new CardBase();

            newCard.Name = card.Name;

            newCard.Guilds = card.Guilds;
            newCard.Races = card.Races;
            newCard.Legions = card.Legions;
            newCard.CivilianGroups = card.CivilianGroups;

            newCard.Mana = card.Mana;
            for (int i = 0; i < card.Stat.Count; i++)
            {
                newCard.Stat.Add(card.Stat[i]);
                newCard.StatSize.Add(card.StatSize[i]);
                newCard.StatSizeLocal.Add(card.StatSizeLocal[i]);
                //card.StatSizeLocal.Add(card.StatSize[i]);
            }

            for (int i = 0; i < card.Trait.Count; i++)
            {
                newCard.Trait.Add(card.Trait[i]);
                newCard.TraitSize.Add(card.TraitSize[i]);
            }

            //for (int i = 0; i < card.StatSizeLocal.Count; i++)
            //{
            //    newCard.StatSizeLocal.Add(card.StatSizeLocal[i]);
            //}
            newCard.Tayp = card.Tayp;

            newCard.Image =  card.Image;

            //newCard.Body = card.Body;// Возможно времнная мера, после обнокления интерфеса конструктора точно будет  не нужно

            return newCard;
        }
        public static void CardCloneExtended(CardBase card1, CardBase card2)
        {
            if(gameSetting.AllCard.Count != 0)
                card2.Id = gameSetting.AllCard[ gameSetting.AllCard.Count-1 ].Id + 1;
            gameSetting.AllCard.Add(card2);

            card2.PlayCard = new List<int>();
            card2.PlayAnotherCard = new List<int>();

            card2.Die = new List<int>();
            card2.AnotherDie = new List<int>();

            card2.Action = new List<int>();
            card2.AnotherAction = new List<int>();

            card2.PreAction = new List<int>();
            card2.PostAction = new List<int>();
            //card.InHand;
            card2.NextTurn = new List<int>();

            card2.Status = new List<string>();


            card2.WalkMood = card1.WalkMood;
            card2.Class = card1.Class;
            card2.Iniciativa = card1.Iniciativa;
            card2.MyHiro = card1.MyHiro;
            card2.Tayp = card1.Tayp;

            for (int i = 0; i < card1.Status.Count; i++)
                card2.Status.Add(card1.Status[i]);


            for (int i = 0; i < card1.PreAction.Count; i++)
                card2.PreAction.Add(card1.PreAction[i]);

            for (int i = 0; i < card1.PostAction.Count; i++)
                card2.PostAction.Add(card1.PostAction[i]);


            for (int i = 0; i < card1.Action.Count; i++)
                card2.Action.Add(card1.Action[i]);

            for (int i = 0; i < card1.AnotherAction.Count; i++)
                card2.AnotherAction.Add(card1.AnotherAction[i]);


            for (int i = 0; i < card1.PlayCard.Count; i++)
                card2.PlayCard.Add(card1.PlayCard[i]);

            for (int i = 0; i < card1.PlayAnotherCard.Count; i++)
                card2.PlayAnotherCard.Add(card1.PlayAnotherCard[i]);


            for (int i = 0; i < card1.Die.Count; i++)
                card2.Die.Add(card1.Die[i]);

            for (int i = 0; i < card1.AnotherDie.Count; i++)
                card2.AnotherDie.Add(card1.AnotherDie[i]);


            for (int i = 0; i < card1.NextTurn.Count; i++)
                card2.NextTurn.Add(card1.NextTurn[i]);

        }

        public static void CardClear(CardBase card)
        {
            //for (int i = 0; i < card.Stat.Count; i++)
            //{
            //    //if (card.Stat[i] == null)
            //    //{
            //    //    card.Stat.RemoveAt(i);
            //    //    card.StatSize.RemoveAt(i);
            //    //    i--;
            //    //}
            //    //else
            //        card.StatSizeLocal.Add(card.StatSize[i]);
            //}

            //for (int i = 0; i < card.Trait.Count; i++)
            //{
            //    if (card.Trait[i] == null)
            //    {
            //        card.Trait.RemoveAt(i);
            //        card.TraitSize.RemoveAt(i);
            //        i--;
            //    }
            //}

            card.WalkMood = gameSetting.Rule.Find(x => x.Name == "Walk");
            HeadSimpleTrigger headSimple = gameSetting.Rule.Find(x => x.Name == "atk1");
            //card.DefAction = headSimple.SimpleTriggers[0];
            card.DefAction = headSimple.SimpleTriggers[0].Action[0];
        }

        public static HeadSimpleTrigger ReadRuleSimple(string str)
        {
            XMLSaver.SetSimpleRoot(str);
            HeadSimpleTrigger simpleTrigger = new HeadSimpleTrigger();
            LoadHead(simpleTrigger);
            LoadHeadExtended(simpleTrigger);

            //LoadTrigger(simpleTrigger);
            //for (int i = 0; i < simpleTrigger.SimpleTriggers.Count; i++)
            //    LoadTriggerExtended(simpleTrigger, i);

            return simpleTrigger;
        }
        public static HeadSimpleTrigger ReadRule(string str)
        {
            XMLSaver.SetSimpleRoot(str);
            HeadSimpleTrigger simpleTrigger = new HeadSimpleTrigger();
            LoadHead(simpleTrigger);
            LoadHeadExtended(simpleTrigger);

            LoadTrigger(simpleTrigger);
            for(int i =0;i <simpleTrigger.SimpleTriggers.Count;i++)
                LoadTriggerExtended(simpleTrigger, i);

            return simpleTrigger;
        }
        private static void LoadHead(HeadSimpleTrigger simpleTrigger)
        {
            string[] sub = XMLSaver.LoadSimpleRule("Head").Split('_');
            simpleTrigger.Name = sub[0];
            simpleTrigger.Cost = int.Parse(sub[1]);
            simpleTrigger.CostExtend = int.Parse(sub[2]);
            simpleTrigger.LevelCap = int.Parse(sub[3]);
            simpleTrigger.Player = bool.Parse(sub[4]);
        }
        private static void LoadHeadExtended( HeadSimpleTrigger simpleTrigger)
        {
            string[] sub = XMLSaver.LoadSimpleRule("HeadNeedRule").Split('_');
            if(sub[0] !="")
                for (int i = 0; i < sub.Length; i++)
                {
                    simpleTrigger.NeedRule.Add(sub[i]);
                }

            sub = XMLSaver.LoadSimpleRule("HeadEnemyRule").Split('_');
            if (sub[0] != "")
                for (int i = 0; i < sub.Length; i++)
                {
                    simpleTrigger.EnemyRule.Add(sub[i]);
                }
        }
        private static void LoadTrigger(HeadSimpleTrigger simpleTrigger)
        {
            SimpleTrigger simpleTriggers = null;
            string[] sub = XMLSaver.LoadSimpleRule("Trigger").Split('/');
            string[] sub1 = null;
            for (int i =0;i < sub.Length; i++)
            {
                sub1 = sub[i].Split('_');
                simpleTriggers = new SimpleTrigger();
                simpleTriggers.CountMod = bool.Parse(sub1[2]);
                simpleTriggers.CountModExtend = bool.Parse(sub1[3]);
                simpleTriggers.TargetPalyer = sub1[0];
                simpleTriggers.Trigger = sub1[1];

                simpleTrigger.SimpleTriggers.Add(simpleTriggers);
            }
        }

        private static SimpleIfCore LoadIfCore(string str)
        {
            SimpleIfCore ifCore = new SimpleIfCore();
            string[] sub = str.Split('*');
            ifCore.Attribute = sub[1];

            sub = sub[0].Split('_');
            ifCore.Result = sub[0];
            ifCore.Point = int.Parse(sub[1]);
            ifCore.Prioritet = int.Parse(sub[2]);

            return ifCore;
        }
        private static SimpleAction LoadAction(string str)
        {
            SimpleAction action = new SimpleAction();
            string[] sub = str.Split('*');
            action.ActionFull = sub[1];

            sub = sub[0].Split('_');
            action.MinPoint = int.Parse(sub[0]);
            action.MaxPoint = int.Parse(sub[1]);
            action.Action = sub[3];
            action.Player = sub[2];


            action.Mood = sub[4];
       // action.Num = int.Parse(sub[4]);

            return action;
        }
        private static void LoadTriggerExtended(HeadSimpleTrigger simpleTrigger, int b)
        {
            SimpleTrigger simpleTriggers = simpleTrigger.SimpleTriggers[b];
            string[] sub = XMLSaver.LoadSimpleRule("TriggerPartPlus", b).Split('/');
            if (sub[0] != "")
                foreach (string str in sub)
                {
                    simpleTriggers.PlusPrior.Add(LoadIfCore(str));
                }

            sub = XMLSaver.LoadSimpleRule("TriggerPartMinus", b).Split('/');
            if (sub[0] != "")
                foreach (string str in sub)
                {
                    simpleTriggers.MinusPrior.Add(LoadIfCore(str));
                }

            sub = XMLSaver.LoadSimpleRule("TriggerPart", b).Split('/');
            if (sub[0] != "")
                foreach (string str in sub)
                {
                    simpleTriggers.Action.Add(LoadAction(str));
                }


        }
    }

    public static class GameEventSystem
    {

        public static GameSetting gameSetting;
        //private static List<Hiro> hiro;
        private static CardBase card1, card2;

        //public static void SetHiro(List<Hiro> _hiro) { List<Hiro> hiro = _hiro;  }
        public static CardBase GetCard(string str)
        {
            switch (str)
            {
                case ("Card1"):
                    return card1;
                    break;
                case ("Card2"):
                    return card2;
                    break;
            }
            return null;
        }
        public static bool CallMood(Hiro hiro, CardBase card, string mood)
        {
            switch (mood)
            {
                case ("All"):
                    return true;
                    break;
                case ("My"):
                    return hiro.Team == card.MyHiro.Team;
                    break;
                case ("Enemy"):
                    return hiro.Team != card.MyHiro.Team;
                    break;
            }
            return false;
        }
        static int UseKeyWord(string[] str , string attribute, int num =-1)
        {
            int sum = 0;
            CardBase mainCard = GetCard("Card1");
            if(mainCard == null)
                mainCard = GetCard("Card2");

            string[] com;
            foreach (CardBase card in gameSetting.AllCard)
            {
                if (card.Id != num)
                {
                    card1 = card;
                    for (int i = 1; i < str.Length; i++)
                    {
                        com = str[i].Split('-');
                        if (card.Tayp == com[0])
                        {
                            if (CallMood(mainCard.MyHiro, card, com[1]))
                                if (FindInt(attribute) >= 0)
                                    sum++;
                            break;
                        }


                    }
                }
            }
            return sum;
        }

        public static int FindInt(string attribute)
        {
            float sum = 0;
            string[] com;
            string[] comAttribute = attribute.Split(':');
            CardBase card = GetCard(comAttribute[0]);
            if (card == null)
            {
                com = comAttribute[0].Split('_');
                if(com[0] == "All")
                {
                    card = card1;
                    //com[0] == "card1";
                    if(com.Length >1)
                        sum = UseKeyWord(com, "Card1:" + comAttribute[1], card.Id);
                    else
                        sum = UseKeyWord(com,"Card1:"+ comAttribute[1]);
                    card1 = card;
                }

                return Mathf.FloorToInt(sum);

            }
            
            comAttribute = comAttribute[1].Split('_');
            sum = -1;
            com = comAttribute[0].Split('-');

            switch (com[0])
            {
                case ("Null"):
                    sum = 0;
                    break;
                case ("Legion"):
                    if (card.Legions.Name == comAttribute[1])
                        sum = 0;
                    break;
                case ("Guilds"):
                    if (card.Guilds.Name == comAttribute[1])
                        sum = 0;
                    break;
                case ("Stat"):
                    switch (comAttribute[1])
                    {
                        case ("Null"):
                            sum = 0;
                            break;
                        default:
                            // if Добавить вызов всего содержимого группы
                            for (int i=0; i< card.Stat.Count; i++)
                            {
                                if (card.Stat[i].Name == comAttribute[1])
                                {
                                    if(com[1]== "Max")
                                        sum = card.StatSize[i];
                                    else
                                        sum = card.StatSizeLocal[i];

                                    sum *= int.Parse(comAttribute[2]);
                                    break;
                                   // i = card.Stat.Count;
                                }
                            }
                            break;
                    }

                    sum += int.Parse(comAttribute[3]);
                    break;
                case ("Trait"):
                    for (int i = 0; i < card.Trait.Count; i++)
                    {
                        if (card.Trait[i] == comAttribute[1])
                        {
                            return 0;
                            //break;
                            //i = card.Trait.Count;
                        }
                    }

                    break;
                    //Guilds Races Legions CivilianGroups StatSize Mana Trait TraitSize
            }

            return Mathf.FloorToInt(sum);
        }
        private static bool FindMenager(SimpleIfCore simpleIf)
        {
            string[] com = simpleIf.Attribute.Split('|');
            //Debug.Log(simpleIf.Attribute);
            int sum1 = FindInt(com[0]);
            int sum2 = FindInt(com[1]);

            switch (simpleIf.Result)
            {
                case ("=")://0
                    return (sum1 == sum2);
                    break;
                case ("!=")://1
                    return (sum1 != sum2);
                    break;
                case (">")://2
                    return (sum1 > sum2);
                    break;
                case ("<")://3
                    return (sum1 < sum2);
                    break;
                default:
                    Debug.Log(simpleIf.Result);
                    break;
            }

            //Debug.Log(attribute);
            return false;
        }

        public static bool UseRule(SimpleTrigger simpleTrigger, CardBase _card1, CardBase _card2, bool echo)
        {// HeadSimpleTrigger head

            if (_card1 == null)
            {
                if (!CallMood(_card2.MyHiro, _card2, simpleTrigger.TargetPalyer))
                    return false;
            }
            else if (_card2 == null)
            {
                if (!CallMood(_card1.MyHiro, _card1, simpleTrigger.TargetPalyer))
                    return false;
            }
            else if (!CallMood(_card1.MyHiro, _card2, simpleTrigger.TargetPalyer))
                return false;

            card1 = _card1;
            card2 = _card2;
            //if (!CallMood(card1.myHiro, card, mood))
            //return false;
            bool isUse = false;

            int noUse = CallSub(simpleTrigger.MinusPrior, simpleTrigger.CountMod);
            int use = CallSub(simpleTrigger.PlusPrior, simpleTrigger.CountMod);

            if (simpleTrigger.CountModExtend)
            {
                int sum = use - noUse;

                foreach (SimpleAction action in simpleTrigger.Action)
                {
                    if (action.MinPoint <= sum && action.MaxPoint >= sum)
                        isUse = UseAction(action, echo);
                }
            }
            else if (use > noUse)
            {
                foreach (SimpleAction action in simpleTrigger.Action)
                {
                   isUse = UseAction(action,echo);
                }
            }
            return isUse;
        }

        private static int CallSub(List<SimpleIfCore> simpleIfCore, bool extend)
        {
            int sum=0;
            SimpleIfCore simpleIf = null;
            for (int i = 0; i < simpleIfCore.Count; i++)
            {
                simpleIf = simpleIfCore[i];
                bool use = FindMenager(simpleIf);
                if (use)
                {
                    if (i+1 < simpleIfCore.Count)
                    {
                        if(simpleIfCore[i+1].Prioritet != simpleIf.Prioritet)
                        {
                            i = simpleIfCore.Count;
                            sum = simpleIf.Point;
                        }
                    }
                    else
                        sum = simpleIf.Point;
                }
            }
            return sum;
        }


        static bool UseAction(SimpleAction action, bool echo)
        {
            string[] com = action.Action.Split('|');
            switch (com[0])
            {
                case ("InGround"):

                    break;

                case ("Attack"):
                    if(card2 != null)
                    {
                        CardBase card3 = card1, card4 = card2;

                        if(!echo)
                            for (int i = 0; i < card1.PreAction.Count; i++)
                                UseRule(gameSetting.PreAction[card3.PreAction[i]],card3,card4,true);

                        card1 = card3; card2 = card4;

                        //card1 = card3; card2 = card4;

                        switch (com[1])
                        {
                            case ("Shot"):
                                MelleAction(ref action);
                                break;
                            case ("Melee"):
                                if (card3.Iniciativa >= card4.Iniciativa)
                                { card1 = card3; card2 = card4;
                                    MelleAction(ref action);
                                }
                                else
                                { card2 = card3; card1 = card4;
                                    MelleAction(ref card1.DefAction);
                                }

                                if (card2.StatSizeLocal[0] > 0)
                                {
                                    if (card3.Iniciativa >= card4.Iniciativa)
                                    { card2 = card3; card1 = card4;
                                        MelleAction(ref card1.DefAction);
                                    }
                                    else
                                    { card1 = card3; card2 = card4; 
                                        MelleAction(ref action); }
                                }
                                break;
                        }

                        if (!echo && card3.StatSizeLocal[0] > 0)
                            for (int i = 0; i < card3.PostAction.Count; i++)
                                UseRule(gameSetting.PostAction[card3.PreAction[i]], card3, card4, true);

                        return true;
                    }
                   // Debug.Log(action.Action);
                    break;

                    break;
                case ("Stat"):
                    if (card2 != null)
                    {
                        switch (com[1])
                        {
                            case ("Add"):
                                AddStat(action);
                                break;
                        }
                        return true;
                    }
                    break;
                case ("Status"):
                    if (card2 != null)
                    {
                        switch (com[1])
                        {
                            case ("Add"):
                                NewStatus(card2, action.ActionFull, true);
                                break;
                            case ("Remove"):
                                NewStatus(card2, action.ActionFull, false);
                                break;
                        }
                        return true;
                    }
                    break;

                case ("Die"):
                    if (card2 != null)
                    {
                        Create.Die(card2);
                        return true;
                    }
                    break;

                case ("Effect"):

                    if (card2 != null)
                    {
                        string mood;
                        bool eternal = (com[1] == "Eternal");
                        mood = (eternal) ? com[2] : com[1];


                        switch (mood)
                        {
                            case ("Add"):
                                UseEffect(action, eternal);
                                break;
                            case ("Remove"):
                                //RemoveEffect
                                // UseEffect(action.ActionFull, eternal);
                                break;
                        }


                        // MelleAction(action);
                        return true;
                    }
                    break;

                case ("Iniciativa"):
                    switch (com[1])
                    {
                        case ("Plus"):
                            card1.Iniciativa++;
                            break;
                        case ("Minus"):
                            card1.Iniciativa--;
                            break;
                    }
                    return true;
                    break;

                default:
                    Debug.Log(action.Action);
                    break;
            }
            return false;
        }

        static void NewStatus(CardBase card, string str, bool add)
        {
            int a =  card.Status.FindIndex(x => x == str);
            if(a != 0)
            {
                if (!add) 
                    card.Status.RemoveAt(a);
            }
            else if (add)
                card.Status.Add(str);
        }

        static void UseEffect(SimpleAction action, bool eternal)
        {
            Effect effect = new Effect();
            int a = 0;
            string[] com = action.ActionFull.Split('|');
            if (eternal)
            {
                card2.InfinityEffect.Add(effect);
            }
            else
            {
                card2.Effect.Add(effect);
                effect.Turn = FindInt(com[a]) / FindInt(com[a + 1]);
                a += 2;
            }

            effect.Power = FindInt(com[a]) / FindInt(com[a + 1]);
            a += 2;

            effect.Prioritet = FindInt(com[a]) / FindInt(com[a + 1]);
            a += 2;

            effect.Target = card2;

            effect.Com = com[a];
        }

        static void GetStat(Constant localStat, ref List<ConstantSub> stat)
        {
            foreach (ConstantSub actualStat in localStat.AntiConstant)
            {
                if (actualStat.Stat.Group)
                {
                    GetStat(actualStat.Stat, ref stat);
                }
                else
                    stat.Add(actualStat);
            }
                //return stat;
        }

        public static void MelleAction(ref SimpleAction action)
        {
            string[] com = action.ActionFull.Split('|');
            string[] com1 = com[2].Split('_');

            List<ConstantSub> stat = new List<ConstantSub>();

            int mod1 = FindInt(com[0]);
            int mod2 = FindInt(com[1]);
            if (mod2 != 0)
                mod1 /= mod2;

            Constant group = gameSetting.Library.Constants.Find(x => x.Name == com1[1]);
            GetStat(group, ref stat);


            string mood;
            int a, b, sum;
            foreach (ConstantSub localStat in stat)
            {
                sum = (int)( localStat.Size * mod1);
                if (action.Mood != "Orig")
                    mood = action.Mood;
                else
                    mood = localStat.MoodEffect;


                a = card2.Stat.FindIndex(x => x.Name == localStat.Stat.Name);
                if (a != -1)
                {
                    foreach (ConstantSub stats in localStat.Stat.GuardConstant)
                    {
                        b = card2.Stat.FindIndex(x => x.Name == stats.Stat.Name);
                        if (b != -1)
                        {
                            sum -= card2.StatSize[b];
                            if (sum <= 0)
                            {
                                sum = 0;
                                break;
                            }
                        }
                    }



                    switch (mood)
                        {
                            case ("All"):
                                card2.StatSize[a] -= sum;
                                card2.StatSizeLocal[a] -= sum;
                                break;
                            case ("Max"):
                                card2.StatSize[a] -= sum;
                                break;
                            case ("Local"):
                                card2.StatSizeLocal[a] -= sum;
                            break;
                            case ("LocalForse"):
                                card2.StatSizeLocal[a] -= sum;
                            break;
                        }
                  
                    //Debug.Log(localCard2.StatSizeLocal[a]);
                    //if(localCard2.StatSize[a] <=0)
                    //RemoveStat(localCard2,a);
                }
            }
            if (card2.StatSizeLocal[0] <= 0)
                Create.Die(card2);
            else
                CardView.ViewCard(card2);

            //CardView.ViewCard(localCard2);
        }

        public static void AddStat(SimpleAction action)
        {
            string[] com = action.ActionFull.Split('|');

            CardBase local3 = GetCard(com[4]);

            string text = com[6];
            int sum = FindInt(com[1]);
            int sum2 = FindInt(com[3]);
            if (sum2 != 0)
                sum /= sum2;
            sum += int.Parse(com[5]);

            int a = local3.Stat.FindIndex(x => x.Name == text);
            if (a == -1)
            {
                if (action.Mood != "Local")
                {
                    a = gameSetting.Library.Constants.FindIndex(x => x.Name == text);
                    local3.Stat.Add(gameSetting.Library.Constants[a]);
                    switch (action.Mood)
                    {
                        case ("All"):
                            local3.StatSize.Add(sum);
                            local3.StatSizeLocal.Add(sum);
                            break;
                        case ("Max"):
                            local3.StatSize.Add(sum);
                            local3.StatSizeLocal.Add(0);
                            break;
                        case ("LocalForse"):
                            local3.StatSize.Add(0);
                            local3.StatSizeLocal.Add(sum);
                            break;

                    }
                }
            }
            else
            {
                switch (action.Mood)
                {
                    case ("All"):
                        local3.StatSize[a] += sum;
                        local3.StatSizeLocal[a] += sum;
                        break;
                    case ("Max"):
                        local3.StatSize[a] += sum;
                        break;
                    case ("Local"):
                        if (local3.StatSizeLocal[a] < local3.StatSize[a])
                        {
                            local3.StatSizeLocal[a] += sum;
                            if (local3.StatSizeLocal[a] > local3.StatSize[a])
                                local3.StatSizeLocal[a] = local3.StatSize[a];
                        }
                        break;
                    case ("LocalForse"):
                        local3.StatSizeLocal[a] += sum;
                        break;

                }

            }
            CardView.ViewCard(card2);
        }


    }

    class Create : MonoBehaviour
    {
        public static GameSetting gameSetting;
        public static List<Hiro> hiro = new List<Hiro>();
        public static StolUi stolUi;

        public static void Die(CardBase card)
        {
            Destroy(card.Body.gameObject);
            for (int i = 0; i < card.Die.Count; i++)
                HiroHead.AddCall(card, null, gameSetting.Die[card.Die[i]],false);
            HiroHead.EchoDie(card);
            HiroHead.ClearEcho(card.Id);

            int a = gameSetting.AllCard.FindIndex(x => x.Id == card.Id);
            gameSetting.AllCard.RemoveAt(a);
            //Hiro hiro = card.MyHiro;

            //for (int i = 0; i < card2.Die.Count; i++)
            //    HiroHead.AddCall(card2, null, gameSetting.Die[card2.Die[i]]);



        }

        public static void CreateHiro(bool enemy)
        {
            Hiro newHiro = new Hiro();
            newHiro.Team = (enemy) ? 1 : 0;
            newHiro.Ui = (enemy) ? stolUi.EnemyInfo : stolUi.MyInfo;
            newHiro.UiStol = (enemy) ? stolUi.EnemyFirstStol : stolUi.MyFirstStol;

            hiro.Add(newHiro);
            LoadCardSet(newHiro);
        }
        public static void AddCardInHand(Hiro hiro, int a)
        {
            for (int i = 0; i < a; i++)
            {
                CardBase card = hiro.CardColod[hiro.CardHandFull[hiro.NextCard]];
                CardBase newCard = Core.CardClone(card);
                Core.CardCloneExtended(card, newCard);
                hiro.CardHand.Add(newCard.Id);

                hiro.NextCard++;
                CreateUiCard(newCard);
            }
        }

        static void LoadCardSet(Hiro hiro)
        {
            //Debug.Log(hiro);
            CardBase card = null;
            List<int> cardBase = new List<int>();
            //интегрирывать из старгого метода позже
            List<int> cardBaseFast = new List<int>();
            List<int> cardBaseSlow = new List<int>();
            CardSet cardSet = XMLSaver.LoadCardSet(Application.dataPath + $"/Resources/CardSet");

            for (int i = 0; i < cardSet.OrigCard.Count; i++)
            {
                card = XMLSaver.Load(Application.dataPath + $"/Resources/Data/Hiro{cardSet.OrigCard[i]}");

                card.MyHiro = hiro;
                Core.CardClear(card);
                //Core.GenerateActionCard(card);
                //Core.CardCloneExtended(card, card);

                hiro.CardColod.Add(card);



                for (int i1 = 0; i1 < cardSet.OrigCount[i]; i1++)
                {
                    cardBase.Add(cardSet.OrigCard[i]);
                }
            }

            foreach(CardBase card1 in hiro.CardColod)
                Core.CallRuleLoad(card1);

            //for (int i = 0; i < hiro.CardColod.Count; i++)
            //{
            //    Core.CallRuleLoad(ref hiro.CardColod[i]);
            //}

            Core.CallRuleFinish();

            for (int i = 0; i < hiro.CardColod.Count; i++)
            {
                card = hiro.CardColod[i];
                Core.GenerateActionCard(card);
                Core.CardCloneExtended(card, card);
            }



            List<int> sortCardBase = new List<int>();
            int r;
            while (cardBase.Count > 0)
            {
                r = Random.Range(0, cardBase.Count);
                sortCardBase.Add(cardBase[r]);
                cardBase.RemoveAt(r);
            }
            hiro.CardHandFull = sortCardBase;
            AddCardInHand(hiro, gameSetting.StartHandSize);
            // ConvertCard(card);

            //for (int i = a - 1; i > -1; i--)
            //{
            //    card = BufferColod[i];
            //    if (card.Trait.Count > 0)
            //    {
            //        b = card.Trait.FindIndex(x => x == "Fast");
            //        if (b != -1)
            //        {
            //            fastCard.Add(i);
            //            BufferColod.RemoveAt(i);
            //        }
            //        else
            //        {
            //            b = card.Trait.FindIndex(x => x == "Slow");
            //            if (b != -1)
            //            {
            //                slowCard.Add(i);
            //                BufferColod.RemoveAt(i);
            //            }
            //        }
            //    }
            //}
            ////pre generation
            //a = fastCard.Count;
            //if (a > 0)
            //{
            //    if (a > 5)
            //    {
            //        for (int i = a; i > 0; i--)
            //        {
            //            r = Random.Range(0, i);
            //            b = fastCard[r];
            //            hiro.CardColod.Add(BufferColod[b]);
            //            BufferColod.RemoveAt(b);
            //            fastCard.RemoveAt(r);
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < a; i++)
            //        {
            //            b = fastCard[i];
            //            hiro.CardColod.Add(BufferColod[b]);
            //            BufferColod.RemoveAt(b);
            //        }
            //    }
            //}
            ////post generation
            //a = slowCard.Count;
            //if (a > 0)
            //{
            //    if (hiro.CardColod.Count < 7)
            //    {
            //        b = 7 - a;
            //        for (int i = b; i >= 0; i--)
            //        {
            //            r = Random.Range(0, i);
            //            hiro.CardColod.Add(BufferColod[r]);
            //            BufferColod.RemoveAt(r);
            //        }
            //    }


            //    for (int i = a; i > 0; i--)
            //    {
            //        r = Random.Range(0, i);
            //        b = slowCard[r];
            //        hiro.CardColod.Add(BufferColod[b]);
            //        BufferColod.RemoveAt(b);
            //        slowCard.RemoveAt(r);
            //    }
            //}

            //a = BufferColod.Count;
            //for (int i = a; i > 0; i--)
            //{
            //    r = Random.Range(0, i);
            //    hiro.CardColod.Add(BufferColod[r]);
            //    BufferColod.RemoveAt(r);
            //}
        }
        

        public static void CreateTacticList()
        {
            GameObject go = null;
            Transform trans = null;
            SimpleTrigger SimpleTrigger = null;
            for (int i = 0; i < gameSetting.Action.Count; i++)
            {
                //SimpleTrigger = gameSetting.Action[i];
                go = Instantiate(stolUi.OrigButton);
                trans = go.transform;
                trans.SetParent(stolUi.AllTacticCase);
                trans.GetChild(0).gameObject.GetComponent<Text>().text = $"Действие{i}";//gameSetting.DefRule[i].Name;
                stolUi.AllTactic.Add(trans);

                AddTactic(go.GetComponent<Button>(),i);
            }
        }
        private static void AddTactic(Button button, int i)
        {
            button.onClick.AddListener(() => HiroHead.UseTactic(i));
        }


        static void CreateUiCard(CardBase card)
        {
            Transform myHand = (card.MyHiro.Team == 0) ? stolUi.MyHand : stolUi.EnemyHand;

            GameObject GO = Instantiate(stolUi.OrigCard);
            card.Body = GO.transform;
            card.Body.SetParent(myHand);

            GO.GetComponent<Button>().onClick.AddListener(() => HiroHead.ReplyCard(card));


            CardView.ViewCard(card);
            //return GO.transform;
        }

    }

    public static class HiroHead
    {
        public static GameSetting gameSetting;
        private static List<Hiro> hiro;
        private static StolUi stolUi;
        public static void SetUI(StolUi _stolUI) { stolUi = _stolUI; }
        private static bool player;
        private static List<SubString> calls = new List<SubString>();
        #region Start Table
        #region Echo
        static void EchoPlayCard(CardBase card)
        {
            foreach (int a in gameSetting.PlayAnotherCardBody)
            {
                CardBase cardLocal = gameSetting.AllCard[a];
                foreach (int i in cardLocal.PlayAnotherCard)
                    AddCall(cardLocal, card, gameSetting.PlayAnotherCard[i], true);
            }
        }
        static void EchoAction(CardBase card)
        {
            foreach (int a in gameSetting.AnotherActionBody)
            {
                CardBase cardLocal = gameSetting.AllCard[a];
                foreach (int i in cardLocal.AnotherAction)
                    AddCall(cardLocal, card, gameSetting.AnotherAction[i], true);
            }
        }
        public static void EchoDie(CardBase card)
        {
            foreach (int a in gameSetting.AnotherDieBody)
            {
                CardBase cardLocal = gameSetting.AllCard[a];
                foreach (int i in cardLocal.AnotherDie)
                    AddCall(cardLocal, card, gameSetting.AnotherDie[i], true);
            }
        }
        public static void EchoNextTurn(int team)
        {
            foreach (int a in gameSetting.NextTurnBody)
            {
                CardBase cardLocal = gameSetting.AllCard[a];
                foreach (int i in cardLocal.AnotherDie)
                    if (team == cardLocal.MyHiro.Team)
                        AddCall(cardLocal, null, gameSetting.AnotherDie[i], true);
            }

            foreach (int a in gameSetting.NextTurnBodyElse)
            {
                CardBase cardLocal = gameSetting.AllCard[a];
                foreach (int i in cardLocal.AnotherDie)
                    if (team != cardLocal.MyHiro.Team)
                        AddCall(cardLocal, null, gameSetting.AnotherDie[i], true);
            }
        }
        public static void ClearEcho(int i)
        {
            gameSetting.PlayAnotherCardBody.Remove(i);
            gameSetting.AnotherActionBody.Remove(i);
            gameSetting.AnotherDieBody.Remove(i);
            gameSetting.NextTurnBody.Remove(i);
            gameSetting.NextTurnBodyElse.Remove(i);

        }
        #endregion
        #region Reply

        static void ReplyHiro(bool enemy)
        {
            Hiro myHiro = (enemy) ? hiro[1] : hiro[0];
        }

        public static void ReplyCard(CardBase card)
        {
            //Hiro myHiro = (enemy) ? hiro[1] : hiro[0];
            if(calls.Count > 0)
            {   
                if(calls[0].Action != null)
                {
                    calls[0].Card2 = card;
                    Reply();
                }
            }
            else //if(card.ActiveRule.Count = 1)
            {
                switch (card.Tayp)
                {
                    case ("Create"):
                        TacticList(card);
                        break;
                    case ("HandCreate"):
                        if (card.MyHiro.ManaCurent >= card.Mana)
                        {
                            AddCall(card, null, card.WalkMood.SimpleTriggers[0], false);
                        }
                        //TacticList(CardBase card);
                        break;
                }
            }
        }

        static void ReplyStol(bool extend, bool enemy)
        {
            Hiro myHiro = (enemy) ? hiro[1] : hiro[0];
            if (calls.Count > 0)
            {
                CardBase card = calls[0].Card1;
                string mood = calls[0].Action.TargetPalyer;
                //bool use = 
                switch (card.Tayp)
                {
                    case ("HandCreate"):
                        if (GameEventSystem.CallMood(myHiro, card, mood))
                        {
                            card.MyHiro.ManaCurent -= card.Mana;
                            card.Body.SetParent(myHiro.UiStol);
                            card.Tayp = "Create";

                            card.MyHiro.CardHand.Remove(card.Id);
                            card.MyHiro.PlayColod.Add(card.Id);

                            foreach (int i in card.PlayCard)
                            {
                                AddCall(card, null, gameSetting.PlayCard[i], false);
                            }

                            HiroUi(card.MyHiro);

                            EchoPlayCard(card);

                            //gameSetting.PlayAnotherCardBody.Add(card);


                            RemoveCall(true);
                            Reply();
                        }
                        break;

                }

            }
        }
        

        static void Reply()
        {
            if (calls.Count > 0)
            {
                //calls[0].Card1.Body.localScale = new Vector3(.8f, .8f, .8f);
                bool use = GameEventSystem.UseRule(calls[0].Action, calls[0].Card1, calls[0].Card2, calls[0].Echo);

                if (use)
                {
                    if (!calls[0].Echo)
                    {
                        if (calls[0].Card2 == null)
                            EchoAction(calls[0].Card1);
                        else
                            EchoAction(calls[0].Card2);
                    }

                    RemoveCall(true);
                    if (calls.Count > 0)
                        Reply();
                }
            }

        }

        public static void UseTactic(int a)
        {

            //GameEventSystem.UseRule();
            calls[0].Action = gameSetting.Action[a];
            Reply();
            stolUi.TacticCase.gameObject.active = false;
        }
        #endregion


        static void TacticList(CardBase card)
        {
            AddCall(card, null, null, false);
            stolUi.TacticCase.gameObject.active = true;
            foreach (Transform child in stolUi.TacticCase)
            {
                child.SetParent(stolUi.AllTacticCase);
            }
            int a = 0;

            foreach (int i in card.Action)
            {
                stolUi.AllTactic[i].SetParent(stolUi.TacticCase);
            }
            
        }

        public static void RemoveCall(bool forseRemove)
        {
            //для дальнейших механик
            //forseRemove
            if (calls.Count > 0)
            {
                calls[0].Card1.Body.localScale = new Vector3(.7f, .7f, .7f);
                calls.RemoveAt(0);
            }
        }
        public static void Install()
        {
            //hiro = new Hiro[2];
            Create.gameSetting = gameSetting;
            Create.stolUi = stolUi;
            gameSetting.AllCard = new List<CardBase>();

            //Core.LoadRules();
            Core.GenerateAction();

            Create.CreateHiro(false);
            Create.CreateHiro(true);

            hiro = Create.hiro;
            //GameEventSystem.SetHiro(hiro);

            Create.CreateTacticList();




            //NextTurn();
            //NextTurn();


            stolUi.NextTurn.onClick.AddListener(() => NextTurn());
            stolUi.MyFirstStol.gameObject.GetComponent<Button>().onClick.AddListener(() => ReplyStol(false, false));

            stolUi.EnemyFirstStol.gameObject.GetComponent<Button>().onClick.AddListener(() => ReplyStol(false, true));
            //true, false));
            // PlayCard(ca
        }
        #endregion
        public static void AddCall (CardBase card1, CardBase card2, SimpleTrigger action, bool Echo)
        {
        
            SubString call = new SubString();
            //call.Text = text;
            //call.Mood = mood;

            //call.Num = num;
            card1.Body.localScale = new Vector3(.9f, .9f, .9f);
            call.Card1 = card1;
            call.Card2 = card2;
            call.Action = action;
            call.Echo = Echo;




            calls.Add(call); 
        }


        private static void NextTurn( )
        {
            //Deactive button

            player = !player;
            Hiro newHiro = (player) ? hiro[0] : hiro[1];

            if (newHiro.Mana < newHiro.ManaMax)
                newHiro.Mana++;
            newHiro.ManaCurent = newHiro.Mana;
            Create.AddCardInHand(newHiro, 1);

            CardBase card = null;
            for (int i = 0; i < newHiro.PlayColod.Count; i++)
            {
                card = gameSetting.AllCard[newHiro.PlayColod[i]];
                Regen(card);
            }

            EchoNextTurn(newHiro.Team);

            HiroUi(newHiro);
        }

        static void Regen(CardBase card)
        {
            for (int i = 0; i < card.Stat.Count; i++)
                if (card.Stat[i].Regen)
                    card.StatSizeLocal[i] = card.StatSize[i];

            card.Iniciativa = 2;
            //for (int i = 0; i < card.Effect.Count; i++)
            //HiroHead.AddCall()
            //UseRule()

            //for (int i = 0; i < card.EffectEternal.Count; i++)


        }

        public static void HiroUi(Hiro hiro)
        {
            
            hiro.Ui.text = $"Hp {hiro.Hp} Card: { hiro.CardHandFull.Count - hiro.NextCard} Mana ({hiro.ManaMax}|{hiro.Mana}|{hiro.ManaCurent})";
        }
       
    }



    public static class CardView
    {
        public static GameSetting gameSetting;
        private static int cardMod = 0;
        public static void ViewCard(CardBase card)
        {
            CardBaseUi Ui = card.Body.gameObject.GetComponent<CardBaseUi>();

            //Texture2D texture = new Texture2D(100, 150);
           // texture.LoadImage(card.Image);
            Ui.Avatar.sprite = card.Image;
            // Ui.Avatar.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            Ui.Name.text = card.Name;

            Ui.Stat.text = "";
            for (int i = 0; i < card.Stat.Count; i++)
            {
                if (card.StatSize[i] > 0)
                {
                    Ui.Stat.text += $"<sprite name={card.Stat[i].IconName}>{card.StatSize[i]}/{card.StatSizeLocal[i]} \n";
                }
                //Ui.Stat.text += $"{card.Stat[i].IconName} {card.StatSize[i]} ";
            }
            Ui.Trait.text = "";
            for (int i = 0; i < card.Trait.Count; i++)
            {
                if (card.Trait[i] != null)
                    Ui.Trait.text += $"{card.Trait[i]} \n";
                //Ui.Stat.text += $"{card.Stat[i].IconName} {card.StatSize[i]} ";
            }


            Ui.Mana.text = "" + card.Mana;


        }

        public static void ClearCard(Transform body)
        {
            CardBaseUi Ui = body.gameObject.GetComponent<CardBaseUi>();

           // Texture2D texture = new Texture2D(10, 15);
            //texture.LoadImage(card.Image);
            Ui.Avatar.sprite = null;

            Ui.Name.text = "";

            Ui.Stat.text = "";
            Ui.Trait.text = "";

            Ui.Mana.text = "";


        }

       /*
        функция поэтаной загрузки для гильдии и не гилбьдии
       доп функция разбивающия списки на несколько категорий и затем собирающая обратно
       существа-спелы = нормальные, разработчика, легендарки
        гильдия_категория_под раздел_имя карты в виде номера
        */
       //void PreLoad()
       // {
       //     //запрос в систему использованием имени гильдии.
       //     //если команда(all)- система принудительно вернет всю библиотеку
       //     //если команда(не neitral)- система вернет ее + neitral
       //     //формат запроса (гильдия) (перечесление запрашиваемых областей)
       //     /*
       //      гильдия
       //         группа
       //             тип достпа группы
       //                 карты внутри группы
             
       //      */

       // }

        public static void CardColor(int a, int b)
        {
            //int cardModSize = cardMod * gameSetting.CardBody.Count;
            //for (int i = 0; i < gameSetting.CardBody.Count; i++)
            //{
            //    a = cardModSize + i;
            //    if (a == curentNum)
            //        Ui.CardBody[i].gameObject.GetComponent<Image>().color = gameSetting.SelectColor[0];
            //    else
            //        Ui.CardBody[i].gameObject.GetComponent<Image>().color = gameSetting.SelectColor[1];


            //    if (a < CardSize.Count)
            //    {
            //        LocalCard[a].Body = CardBody[i];
            //        CardView.ViewCard(CardOrig[a]);
            //    }
            //    else
            //    {
            //        CardView.ClearCard(CardBody[i]);
            //    }

            //}
        }
        public static void CardReset()
        {
            cardMod = 0;
            CardList();
        }

        public static void CardList()
        {
            gameSetting.AllCard = new List<CardBase>();

            string[] com;
            int a;
            int cardModSize = cardMod * gameSetting.CardBody.Count;
            for (int i = 0; i < gameSetting.CardBody.Count; i++)
            {
                a = cardModSize + i;
                if (a < gameSetting.AllCardPath.Count)
                {
                    com = gameSetting.AllCardPath[a].Split('_');
                    CardBase card = XMLSaver.Load(gameSetting.GameDataFile.Data[int.Parse(com[0])].MasterKey +com[1]);
                    card.Id = a;
                    gameSetting.AllCard.Add(card);
                    card.Body = gameSetting.CardBody[i];
                    ViewCard(card);
                }
                else
                    ClearCard(gameSetting.CardBody[i]);
            }
        }

        public static void Mod(bool up)
        {
            if (up)
            {
                if ((cardMod * gameSetting.CardBody.Count) + gameSetting.CardBody.Count
                    < gameSetting.AllCardPath.Count)
                {
                    cardMod++;
                    CardList();
                }
            }
            else if (cardMod > 0)
            {
                cardMod--;
                CardList();
            }

        }
    }

    public static class DataManager
    {
        public static GameSetting gameSetting;
        //public static void GenerateData(RuleMainFrame frame, ActionLibrary Library)
        //{
        //    string path = $"/Resources/Data";
        //    //gameSetting.GameDataFile = XMLSaver.LoadGameData(path);

        //    GameData gameData = new GameData();
        //    gameData.Guild = new List<GameDataData>();
        //    for (int i = 0; i < Library.Guilds.Count; i++)
        //    {
        //        gameData.Guild.Add(new GameDataData());
        //        gameData.Guild[i].Name = Library.Guilds[i].Name;
        //        gameData.Guild[i].Data = new List<GameDataData>();
        //        for (int i1 = 0; i1 < frame.ClassCard.Count; i1++)
        //        {
        //            gameData.Guild[i].Data.Add(new GameDataData());
        //            gameData.Guild[i].Data[i1].Name = frame.ClassCard[i1];
        //            gameData.Guild[i].Data[i1].Data = new List<GameDataData>(frame.CardTayp.Count);
        //            for (int i2 = 0; i2 < frame.CardTayp.Count; i2++)
        //            {
        //                gameData.Guild[i].Data[i1].Data.Add(new GameDataData());
        //                gameData.Guild[i].Data[i1].Data[i2].Name = frame.CardTayp[i2];
        //                gameData.Guild[i].Data[i1].Data[i2].End = true;

        //               // gameData.Guild[i].Data[i1].Data[i2].Data = new List<GameDataData>();
        //                //gameData.Guild[i].Data[i1].Data[i2].Size = -1;

        //               // gameData.Guild[i].Data[i1].Data[i2].Data.Add(new GameDataData());
        //                //gameData.Guild[i].Data[i1].Data[i2].Data[0].Size = -1;
        //               // gameData.Guild[i].Data[i1].Data[i2].Data[0].Name = " ";
        //               // gameData.Guild[i].Data[i1].Data[i2].Data[0].End = true;
        //               // gameData.Guild[i].Data[i1].Data[i2].Data[0].Data = new List<GameDataData>();
        //                //gameData.Guild[i].Data[i1].Data[i2].Data[0].Data.Add(new GameDataData());
        //            }

        //        }
        //    }

        //    XMLSaver.SaveGameData(gameData,path);
        //}
        public static void GenerateKey(RuleMainFrame frame, ActionLibrary Library)
        {
            string[] com;
            string path = $"/Resources/Data";
            gameSetting.GameDataFile = new GameData();
            gameSetting.GameDataFile.Data = new List<SubGameData>();
            for (int i = 0; i < Library.Guilds.Count; i++)
                for (int i1 = 0; i1 < frame.ClassCard.Count; i1++)
                    for (int i2 = 0; i2 < frame.CardTayp.Count; i2++)
                    {
                        SubGameData sub = new SubGameData();
                        sub.MasterKey = Library.Guilds[i].Name + "/" + frame.ClassCard[i1] + "/" + frame.CardTayp[i2] + "/";
                        XMLSaver.LoadGameData(sub);
                        int a = gameSetting.GameDataFile.Data.Count;
                        gameSetting.GameDataFile.Data.Add(sub);

                        if (sub.Key != " ")
                        {
                            com = sub.Key.Split('/');
                            for (int i3 = 0; i3 < com.Length; i3++)
                            {
                                if (sub.KeyComplite != " ")
                                    sub.KeyComplite += "/";
                                sub.KeyComplite += $"{a}_{com[i3]}";
                            }
                        }
                    }
            SetKey();
        }
        public static void SetKey()
        {
            string path = " ";
            for (int i = 0; i < gameSetting.GameDataFile.Data.Count; i++)
                if (gameSetting.GameDataFile.Data[i].Use
                    && gameSetting.GameDataFile.Data[i].Key != " ")
                {
                    if (path != " ")
                        path += "/";
                    path += gameSetting.GameDataFile.Data[i].KeyComplite;

                }

            //Debug.Log(path);
            if(path !=" ")
                gameSetting.AllCardPath = new List<string>(path.Split('/'));
            else
                gameSetting.AllCardPath = new List<string>();
            CardView.CardReset();
        }

       // public static void UseData()
       // {
       //     string path = $"/Resources/Data";
       //     gameSetting.GameDataFile = XMLSaver.LoadGameData(path);
       //    // mainData = " ";
       //     gameSetting.GameDataFile.Data = new List<SubGameData>();
       //     for (int i = 0; i < gameSetting.GameDataFile.Guild.Count; i++)
       //     {
       //         DataSupport(gameSetting.GameDataFile.Guild[i], "");
       //     }


       //    // Debug.Log(mainData);
       //     //if(mainData != " ")
       //     //{
       //     //    string[] com = mainData.Split('_');
       //     //    gameSetting.AllCardPath = new List<string>(com);
       //     //}
       //     //else
       //     //    gameSetting.AllCardPath = new List<string>();

       //     CardView.CardReset();
       // }

       //// static string mainData;

       // static void DataSupport(GameDataData gameData, string path)
       // {
       //     if (gameData == null)
       //         return;
       //     if (gameData.Use)
       //         return;

       //     if (gameData.End)
       //     {
       //         SubGameData gd = new SubGameData();
       //         gd.MasterKey = path;
       //         gd.Key = gameData.Data[0].Name;
       //         gd.Size = gameData.Size;
       //         int a = gameSetting.GameDataFile.Data.Count;
       //         if (a > 0)
       //             gd.Size += gameSetting.GameDataFile.Data[a - 1].Size;// += gameData.Size;
       //         gameSetting.GameDataFile.Data.Add(gd);
       //     }
       //     else if (gameData.Data != null)
       //         for (int i = 0; i < gameData.Data.Count; i++)
       //             DataSupport(gameData.Data[i], path + i + "/");
       // }

       // public static void DataTravel(string key,string path)
       // {
       //     //if (path == "ViewAll")
       //     //{
       //     //    mainData = " ";
       //     //    for (int i = 0; i < gameSetting.GameDataFile.Guild.Count; i++)
       //     //    {
       //     //        DataSupport(gameSetting.GameDataFile.Guild[i], "");
       //     //    }
       //     //    string[] com = mainData.Split('_');
       //     //    gameSetting.AllCardPath = new List<string>(com);
       //     //}
       //     CardView.CardReset();
       //     //string[] com = key.Split('_');
       //     //switch (path) 
       //     //{
       //     //    case ("Back"):
       //     //        main = com[0];
       //     //        for (int i = 1; i < com.Length - 1; i++)
       //     //        {
       //     //            main +="_" +com[i];
       //     //        }
       //     //        break;
       //     //    case ("All"):
       //     //        int a = int.Parse(com[0]);
       //     //        GameDataData = gameSetting.GameDataFile.Guild[a];
       //     //        break;

       //     //}


       //     //string[] com = path.Split('_');
       //     //switch (com[com.Length - 1])
       //     //{

       //     //}

       // }
       // public static void LoadData()
       // {

       // }

       // public static void ReadPath()
       // {
       //    // main
       // }

    }
    
}
