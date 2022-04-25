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
        }

        public static void LoadRules()
        {
            gameSetting.Rule = new List<HeadSimpleTrigger>();
            for (int i = 0; i < gameSetting.Library.RuleName.Count; i++)
                gameSetting.Rule.Add(Core.ReadRule(i));

            //int a = 0;
            //gameSetting.DefRule = new List<HeadSimpleTrigger>();
            //for (int i = 0; i < gameSetting.DefRuleText.Count; i++)
            //{
            //    a = gameSetting.Library.RuleName.FindIndex(x => x == gameSetting.DefRuleText[i]);
            //    gameSetting.DefRule.Add(gameSetting.Rule[a]);
            //}
        }


        public static void GenerateActionCard(CardBase card)
        {
            card.Action = new List<int>();
            card.InHand = new List<int>();
            card.NextTurn = new List<int>();
            //for(int i =0;i< gameSetting.Rule.Count; i++)
            int a = 0;
            foreach (HeadSimpleTrigger head in card.Trait)
            {
                foreach (SimpleTrigger simpleTrigger in head.SimpleTriggers)
                {
                    a = simpleTrigger.CodName;
                    switch (simpleTrigger.Trigger)
                    {
                        case ("Action"):
                            card.Action.Add(a);
                            break;
                        case ("InHand"):
                            card.InHand.Add(a);
                            break;
                        case ("NextTurn"):
                            card.NextTurn.Add(a);
                            break;
                    }
                }
            }

            // if()

        }
        public static void GenerateAction()
        {
            gameSetting.Action = new List<SimpleTrigger>();
            gameSetting.InHand = new List<SimpleTrigger>();
            gameSetting.NextTurn = new List<SimpleTrigger>();
            //for(int i =0;i< gameSetting.Rule.Count; i++)
            foreach (HeadSimpleTrigger head in gameSetting.Rule)
            {
                foreach (SimpleTrigger simpleTrigger in head.SimpleTriggers)
                {
                    switch (simpleTrigger.Trigger)
                    {
                        case ("Action"):
                            simpleTrigger.CodName = gameSetting.Action.Count;
                            gameSetting.Action.Add(simpleTrigger);
                            break;
                        case ("InHand"):
                            simpleTrigger.CodName = gameSetting.InHand.Count;
                            gameSetting.InHand.Add(simpleTrigger);
                            break;
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
            }

            for (int i = 0; i < card.Trait.Count; i++)
            {
                newCard.Trait.Add(card.Trait[i]);
                newCard.TraitSize.Add(card.TraitSize[i]);
            }
            
            for (int i = 0; i < card.StatSizeLocal.Count; i++)
            {
                newCard.StatSizeLocal.Add(card.StatSizeLocal[i]);
            }

            newCard.Image =  card.Image;

            newCard.Class = card.Class;
            newCard.Iniciativa = card.Iniciativa;
            newCard.MyHiro = card.MyHiro;
            newCard.Tayp = card.Tayp;

            for (int i = 0; i < card.Action.Count; i++)
            {
                newCard.Action.Add(card.Action[i]);
            }

            //newCard.Body = card.Body;// Возможно времнная мера, после обнокления интерфеса конструктора точно будет  не нужно

            return newCard;
        }

        public static void CardClear(CardBase card)
        {
            for (int i = 0; i < card.Stat.Count; i++)
            {
                if (card.Stat[i] == null)
                {
                    card.Stat.RemoveAt(i);
                    card.StatSize.RemoveAt(i);
                    i--;
                }
                else
                    card.StatSizeLocal.Add(card.StatSize[i]);
            }

            for (int i = 0; i < card.Trait.Count; i++)
            {
                if (card.Trait[i] == null)
                {
                    card.Trait.RemoveAt(i);
                    card.TraitSize.RemoveAt(i);
                    i--;
                }
            }

            card.WalkMood = gameSetting.Rule.Find(x => x.Name == "Walk");
        }

        public static HeadSimpleTrigger ReadRule(int a)
        {
            XMLSaver.SetSimpleRoot(a);
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
            action.Mood = sub[2];

            action.Num = int.Parse(sub[4]);

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
                    if (hiro.Team == card.MyHiro.Team)
                        return true;
                    break;
                case ("Enemy"):
                    if (hiro.Team != card.MyHiro.Team)
                        return true;
                    break;
            }
            return false;
        }
        public static int FindInt(string attribute)
        {
            string[] comAttribute = attribute.Split(':');
            CardBase card = GetCard(comAttribute[0]);
            
            comAttribute = comAttribute[1].Split('_');
            float sum = -1;
            switch (comAttribute[0])
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
                            for(int i=0; i< card.Stat.Count; i++)
                            {
                                if (card.Stat[i].Name == comAttribute[1])
                                {
                                    sum = card.StatSize[i];
                                    sum *= int.Parse(comAttribute[2]);
                                    i = card.Stat.Count;
                                }
                            }
                            break;
                    }

                    sum += int.Parse(comAttribute[3]);
                    break;
                case ("Trait"):
                    for (int i = 0; i < card.Trait.Count; i++)
                    {
                        if (card.Trait[i].Name == comAttribute[1])
                        {
                            sum = 0;
                            i = card.Trait.Count;
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
            }

            //Debug.Log(attribute);
            return false;
        }

        public static bool UseRule(SimpleTrigger simpleTrigger, CardBase _card1, CardBase _card2)
        {// HeadSimpleTrigger head
            card1 = _card1;
            card2 = _card2;
            //Записать ситуации когда, автомат не может сработать
           // switch(simpleTrigger.)
           // if (card1.Tayp)
            //HeadSimpleTrigger head = gameSetting.Rule[a];
            //if(card1)

            int noUse = CallSub(simpleTrigger.MinusPrior, simpleTrigger.CountMod);
            int use = CallSub(simpleTrigger.PlusPrior, simpleTrigger.CountMod);

            if (simpleTrigger.CountModExtend)
            {
                int sum = use - noUse;

                foreach (SimpleAction action in simpleTrigger.Action)
                {
                    if (action.MinPoint <= sum && action.MaxPoint >= sum)
                        UseAction(action);
                }
            }
            else if (use > noUse)
            {
                foreach (SimpleAction action in simpleTrigger.Action)
                {
                    UseAction(action);
                }
            }
            return true;
        }

        private static int CallSub(List<SimpleIfCore> simpleIfCore, bool extend)
        {
            int sum=0;
            SimpleIfCore simpleIf = null;
            for (int i = 0; i < simpleIfCore.Count; i++)
            {
                simpleIf = simpleIfCore[i];

                if (FindMenager(simpleIf))
                {
                    if(i+1 < simpleIfCore.Count)
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


        static void UseAction(SimpleAction action)
        {
            switch (action.Action)
            {
                case ("InGround"):

                    break;

                case ("Melle"):
                    MelleAction(action);
                    break;
                case ("Shot"):

                    break;
                case ("AddStat"):

                    break;

                case ("Die"):

                    break;

                case ("Effect"):

                    break;

                case ("EffectEternal"):

                    break;
            }
        }

        //static void UseEffect(string actionFull, CardBase local1, CardBase local2)
        //{
        //    string[] com = actionFull.Split('/');
        //    string[] com1 = com[0].Split('|');

        //    switch (com1[0])
        //    {
        //        case ("AddStat"):
        //            AddStat("Local", com[1], local1, local2);//All-Max-Local
        //            break;

        //        case ("AddEffect"):

        //            Effect newEffect = new Effect();
        //            newEffect.Name = com1[1];

        //            if (com1[2] == "Eternal")
        //            {
        //                local2.InfinityEffect.Add(newEffect);
        //            }
        //            else
        //            {
        //                local2.Effect.Add(newEffect);
        //                com1 = com[1].Split('|');
        //                newEffect.Turn = FindInt(com1[0], local1) / int.Parse(com1[1]) + int.Parse(com1[2]);
        //            }

        //            com1 = com[2].Split('|');
        //            newEffect.Power = FindInt(com1[0], local1) / int.Parse(com1[1]) + int.Parse(com1[2]);

        //            com1 = com[3].Split('|');
        //            newEffect.Prioritet = FindInt(com1[0], local1) / int.Parse(com1[1]) + int.Parse(com1[2]);

        //            newEffect.Target = local2;
        //            break;

        //        case ("Die"):
        //            //Die(local2);
        //            break;
        //        case ("Guard"):
        //            local2.Guard.Add(local1);
        //            break;
        //            //case ("Support"):
        //            //    local2.Support.Add(local1);
        //            //    break;
        //    }

        //}

        public static void MelleAction(SimpleAction action)
        {
            string[] com = action.ActionFull.Split('|');
            string[] com1 = com[0].Split('_');

            List<Constant> stat = new List<Constant>();
            List<int> statSize = new List<int>();
            List<string> mood = new List<string>();
            //int a = gameSetting.Library.Constants.FindIndex(x => x.Name == text);
            //if (group.Group)
            //{
            //}
            string text = com[1];
            string actionFull = "";
            int sum;
            Constant group = gameSetting.Library.Constants.Find(x => x.Name == text);
            if (group.Group)
                for (int i = 0; i < group.AntiConstant.Count; i++)
                {
                    text = group.AntiConstant[i].Name;
                    actionFull = $"{com[0]}_{text}_{com[2]}_{com[3]}";
                    sum = FindInt(actionFull);

                    if (sum > 0)
                    {
                        stat.Add(gameSetting.Library.Constants.Find(x => x.Name == text));
                        statSize.Add(sum);
                        mood.Add(group.moodEffect);
                    }

                }
            else
            {
                stat.Add(group);
                sum = FindInt(com[0]);
                statSize.Add(sum);
                mood.Add(group.moodEffect);
            }


            int a, b;
            for (int i = 0; i < stat.Count; i++)
            {
                foreach (Constant actualStats in stat[i].AntiConstant)
                {
                    a = card2.Stat.FindIndex(x => x.Name == actualStats.Name);
                    if (a != -1)
                    {
                        foreach (Constant stats in card2.Stat[i].GuardConstant)
                        {
                            b = card2.Stat.FindIndex(x => x.Name == stats.Name);
                            if (b != -1)
                            {
                                statSize[i] -= card2.StatSize[b];
                                if (statSize[i] <= 0)
                                {
                                    statSize[i] = 0;
                                    break;
                                }
                            }
                        }


                        switch (mood[i])
                        {
                            case ("All"):
                                card2.StatSize[a] -= statSize[i];
                                card2.StatSizeLocal[a] -= statSize[i];
                                break;
                            case ("Max"):
                                card2.StatSize[a] -= statSize[i];
                                break;
                            case ("Local"):
                                card2.StatSizeLocal[a] -= statSize[i];
                                break;
                            case ("LocalForse"):
                                card2.StatSizeLocal[a] -= statSize[i];
                                break;

                        }
                        //Debug.Log(localCard2.StatSizeLocal[a]);
                        //if(localCard2.StatSize[a] <=0)
                        //RemoveStat(localCard2,a);
                    }
                }
            }
            // if (localCard2.StatSizeLocal <= 0)
            //Die();

            CardView.ViewCard(card2);
            //CardView.ViewCard(localCard2);
        }

        public static void AddStat(string mood, string actionFull, CardBase localCard1, CardBase localCard2)
        {
            string[] com = actionFull.Split('|');

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
                if (mood != "Local")
                {
                    a = gameSetting.Library.Constants.FindIndex(x => x.Name == text);
                    local3.Stat.Add(gameSetting.Library.Constants[a]);
                    switch (mood)
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
                switch (mood)
                {
                    case ("All"):
                        local3.StatSize[a] += sum;
                        local3.StatSizeLocal[a] += sum;
                        break;
                    case ("Max"):
                        local3.StatSize[a] += sum;
                        break;
                    case ("Local"):
                        local3.StatSizeLocal[a] += sum;
                        if (local3.StatSizeLocal[a] > local3.StatSize[a])
                            local3.StatSizeLocal[a] = local3.StatSize[a];
                        break;
                    case ("LocalForse"):
                        local3.StatSizeLocal[a] += sum;
                        break;

                }

            }
        }

    }

    class Create : MonoBehaviour
    {
        public static GameSetting gameSetting;
        public static List<Hiro> hiro = new List<Hiro>();
        public static StolUi stolUi;

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
                CardBase newCard = Core.CardClone(hiro.CardColod[hiro.CardHandFull[hiro.NextCard]]);
                hiro.CardHand.Add(newCard);

                hiro.NextCard++;
                CreateUiCard(newCard);
            }
        }

        static void LoadCardSet(Hiro hiro)
        {

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
                Core.GenerateActionCard(card);

                hiro.CardColod.Add(card);



                for (int i1 = 0; i1 < cardSet.OrigCount[i]; i1++)
                {
                    cardBase.Add(cardSet.OrigCard[i]);
                }
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

                go.GetComponent<Button>().onClick.AddListener(() => HiroHead.UseTactic(i));
            }
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
                            AddCall(card, null, card.WalkMood.SimpleTriggers[0]);
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
                switch (card.Tayp)
                {
                    case ("HandCreate"):
                        if (GameEventSystem.CallMood(myHiro, card, mood))
                        {
                            card.MyHiro.ManaCurent -= card.Mana;
                            card.Body.SetParent(myHiro.UiStol);
                            card.Tayp = "Create";

                            foreach(int i in card.PlayCard)
                            {
                                AddCall(card, null, gameSetting.PlayCard[i]);
                            }
                            //GameEventSystem.UseRule()

                            HiroUi(card.MyHiro);
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
                bool use = GameEventSystem.UseRule(calls[0].Action, calls[0].Card1, calls[0].Card2);

                if (use)
                {
                    RemoveCall(true);
                    if (calls.Count > 0)
                        Reply();
                }
                //bool use = false;
                //CardBase firstCard = calls[0].Card1;
                //string mood = calls[0].Mood; 
                //if (card == null)
                //    switch (calls[0].Text)//calls[0].Action
                //    {
                        
                //        default:
                //            Debug.Log(calls[0].Text);
                //            break;
                //    }
                //else
                //    switch (calls[0].Text)//calls[0].Action
                //    {
                //        case ("Attack"):
                //            Debug.Log(CallMood(firstCard.MyHiro, card, mood));
                //            if (CallMood(firstCard.MyHiro, card, mood))
                //                GameEventSystem.MelleAction("",firstCard,card); 

                //                break;
                //        case ("Shot"):

                //            break;
                //        //case ("UseSlot"):
                //        //    if (firstCard == null)
                //        //    {
                //        //        if (CallMood(hiro, card1, mood))
                //        //        {
                //        //            card1.MyHiro.ManaCurent -= card1.Mana;
                //        //            Create.PlayCard(card1);
                //        //        }
                //        //    }
                //        //    break;
                //        default:
                //            Debug.Log(calls[0].Text);
                //            break;
                //    }
                

                //if (use)
                //{
                //    calls.RemoveAt(0);
                //    if (calls.Count > 0)
                //        Reply(null);
                //}
            }

        }

        public static void UseTactic(int a)
        {
            //GameEventSystem.UseRule();
            calls[0].Action = gameSetting.Action[a];
            Reply();
        }



        static void TacticList(CardBase card)
        {
            AddCall(card, null, null);
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
            calls.RemoveAt(0);
        }
        public static Hiro Install()
        {
            //hiro = new Hiro[2];
            Create.gameSetting = gameSetting;
            Create.stolUi = stolUi;

            Core.LoadRules();
            Core.GenerateAction();

            Create.CreateHiro(false);
            Create.CreateHiro(true);

            hiro = Create.hiro;
            //GameEventSystem.SetHiro(hiro);

            Create.CreateTacticList();




            NextTurn();
            NextTurn();


            stolUi.NextTurn.onClick.AddListener(() => NextTurn());
            stolUi.MyFirstStol.gameObject.GetComponent<Button>().onClick.AddListener(() => ReplyStol(false, false));

            stolUi.EnemyFirstStol.gameObject.GetComponent<Button>().onClick.AddListener(() => ReplyStol(false, true));
            //true, false));
            // PlayCard(ca
            return hiro[0];
        }
        #endregion
        public static void AddCall (CardBase card1, CardBase card2, SimpleTrigger action)
        {
        
            SubString call = new SubString();
            //call.Text = text;
            //call.Mood = mood;

            //call.Num = num;
            call.Card1 = card1;
            call.Card2 = card2;
            call.Action = action;




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

            HiroUi(newHiro);
        }
       
        public static void HiroUi(Hiro hiro)
        {
            
            hiro.Ui.text = $"Hp {hiro.Hp} Card: { hiro.CardColod.Count - hiro.NextCard} Mana ({hiro.ManaMax}|{hiro.Mana}|{hiro.ManaCurent})";
        }
    }

    public static class CardView
    {
        public static GameSetting gameSetting;
        public static void ViewCard(CardBase card)
        {
           // Transform trans = card.Body;
            CardBaseUi Ui = card.Body.gameObject.GetComponent<CardBaseUi>();

            Texture2D texture = new Texture2D(100, 150);
            texture.LoadImage(card.Image);
            Ui.Avatar.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            Ui.Name.text = card.Name;

            Ui.Stat.text = "";
            for (int i = 0; i < card.Stat.Count; i++)
            {
                if (card.StatSize[i] > 0)
                {
                    Ui.Stat.text += $"<sprite name={card.Stat[i].IconName}>{card.StatSize[i]} \n";
                }
                //Ui.Stat.text += $"{card.Stat[i].IconName} {card.StatSize[i]} ";
            }
            Ui.Trait.text = "";
            for (int i = 0; i < card.Trait.Count; i++)
            {
                if (card.Trait[i] != null)
                    Ui.Trait.text += $"{card.Trait[i].Name} \n";
                //Ui.Stat.text += $"{card.Stat[i].IconName} {card.StatSize[i]} ";
            }

            //  Ui.Trait;

            Ui.Mana.text = "" + card.Mana;

            //if(Ui.Count != null)
            //        Ui.Count.text;
        }

        //static void ViewSlot(RealCard newPosition, MeshRenderer mesh, string mood, int b, int team, RealCard curentCard, int line)
        //{

        //    mesh.material = gameSetting.TargetColor[0];
        //    //int a - используется для указания на использование первой или второй линин позиции
        //    if(newPosition != null)
        //        switch (mood)
        //        {
        //            case ("ShotView"):
        //                if (newPosition.Team == line)
        //                    if (newPosition.MovePoint > 0)
        //                        if (newPosition.ShotAction.Count > 0)
        //                        {
        //                            mesh.material = gameSetting.TargetColor[1];
        //                        }
        //                break;

        //            case ("ShotTarget"):
        //                if (newPosition.Team != line)
        //                    //if (line == team)
        //                        mesh.material = gameSetting.TargetColor[2];
        //                else if (newPosition == curentCard)
        //                    mesh.material = gameSetting.TargetColor[3];

        //                break;

        //            case ("MeleeView"):
        //                if (newPosition.Team == line)
        //                    if (newPosition.MovePoint > 0)
        //                        if (newPosition.Action.Count > 0)
        //                        {
        //                            mesh.material = gameSetting.TargetColor[1];
        //                        }
        //                break;

        //            case ("MeleeTarget"):
        //                if (newPosition.Team != line)
        //                {
        //                   // if (line == team)
        //                        if (b == 0)
        //                            mesh.material = gameSetting.TargetColor[2];
        //                }
        //                else if (newPosition == curentCard)
        //                    mesh.material = gameSetting.TargetColor[3];

        //                break;

        //            default:
        //                break;
        //        }
        //    else
        //        switch (mood)
        //        {
        //            case ("SetCard"):
        //                if (line == team)
        //                    if (b == 0)
        //                    {
        //                        mesh.material = gameSetting.TargetColor[1];
        //                    }
        //                break;
        //            default:
        //                break;
        //        }
        //}

        //public static void ViewLoadUi(Hiro newHiro, string mood, RealCard curentCard, Transform[] slots, int line)
        //{
        //  //  Transform[] slots = null;
        //    Slot[] hiroSlot = newHiro.Slots;

        //    int team = newHiro.Team;
        //    if(curentCard!= null)
        //        team = curentCard.Team;
        //    // Slot newSlot = null;

        //    //if (newHiro.Team == 0)
        //    //    slots = Ui.MySlot;
        //    //else
        //    //    slots = Ui.EnemySlot;

        //    Slot newSlot = null;
        //    RealCard newPosition = null;
        //    MeshRenderer mesh = null;
        //    int a = slots.Length;
        //    int b = 0;
        //    for (int i = 0; i < a; i++)
        //    {
        //        newSlot = hiroSlot[i];

        //        b = 0;
        //        ViewSlot(newSlot.Position[b], newSlot.Mesh[b], mood, b, team, curentCard, line);

        //        b++;
        //        ViewSlot(newSlot.Position[b], newSlot.Mesh[b], mood, b, team, curentCard, line);
        //    }
        //}

        //public static void ViewTargetCard(CardBase cardBase, Transform Ui)
        //{

        //    Ui.gameObject.active = true;
        //    Transform trans = cardBase.Body;
        //    cardBase.Body = Ui;
        //    ViewCard(cardBase);
        //    cardBase.Body = trans;
        //}

        //static void ViewSlotHandler(int a, int b, TMP_Text text)
        //{
        //    if (a > 0)
        //        text.text += $"<sprite name={gameSetting.NameIcon[b]}>{a} ";
        //}
        //public static void ViewSlotUi(RealCard card)
        //{
        //    TMP_Text text = card.Ui;
        //    text.text = "";

        //    int[] stat = new int[] 
        //    { 
        //        card.MeleeDMG,
        //        card.ShotDMG,
        //        card.NoArmorDMG,
        //        card.ArmorBreakerDMG,

        //        card.Hp,
        //        card.Helmet,
        //        card.Shild,
        //        card.Armor,

        //        card.Agility1,
        //        card.Agility2,
        //        card.Agility3,
        //        card.Agility4
        //    };

        //    int a = stat.Length;
        //    for (int i = 0; i < a; i++)
        //    {
        //        ViewSlotHandler(stat[i], i, text);
        //    }




        //}
        //public static void ViewSlotClear(RealCard card)
        //{
        //    card.Ui.text = "";
        //}
    }
}
