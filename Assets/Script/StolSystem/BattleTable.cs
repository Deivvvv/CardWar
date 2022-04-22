using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Saver;

namespace BattleTable
{
    public class Core
    {
        public static void LoadGameSetting(GameSetting gameSetting)
        {
            //BattleSystem.gameSetting = gameSetting;
            TableRule.gameSetting = gameSetting;
            CardView.gameSetting = gameSetting;
            GameEventSystem.gameSetting = gameSetting;
            HiroHead.gameSetting = gameSetting;
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


            //newCard.Body = card.Body;// ¬озможно времнна€ мера, после обноклени€ интерфеса конструктора точно будет  не нужно

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
        }
    }

    public static class GameEventSystem
    {
        //p

        public static GameSetting gameSetting;
        private static List<Hiro> hiro;
        private static CardBase card1, card2;

        public static void SetHiro(List<Hiro> _hiro) { List<Hiro> hiro = _hiro;  }

        public static int FindInt(string attribute, CardBase cardBase)
        {
            string[] comAttribute = attribute.Split('_');
            float sum = -1;
            switch (comAttribute[0])
            {
                case ("Null"):
                    sum = 0;
                    break;
                case ("Legion"):
                    if (cardBase.Legions.Name == comAttribute[1])
                        sum = 0;
                    break;
                case ("Guilds"):
                    if (cardBase.Guilds.Name == comAttribute[1])
                        sum = 0;
                    break;
                case ("Stat"):
                    switch (comAttribute[1])
                    {
                        case ("Null"):
                            sum = 0;
                            break;
                        default:
                            // if ƒобавить вызов всего содержимого группы
                            for(int i=0; i< cardBase.Stat.Count; i++)
                            {
                                if (cardBase.Stat[i].Name == comAttribute[1])
                                {
                                    sum = cardBase.StatSize[i];
                                    sum *= int.Parse(comAttribute[2]);
                                    i = cardBase.Stat.Count;
                                }
                            }
                            break;
                    }

                    sum += int.Parse(comAttribute[3]);
                    break;
                case ("Trait"):
                    for (int i = 0; i < cardBase.Trait.Count; i++)
                    {
                        if (cardBase.Trait[i] == comAttribute[1])
                        {
                            sum = 0;
                            i = cardBase.Trait.Count;
                        }
                    }

                    break;
                    //Guilds Races Legions CivilianGroups StatSize Mana Trait TraitSize
            }

            return Mathf.FloorToInt(sum);
        }
        private static bool FindMenager(string attribute)
        {
            string[] mainAttribute = attribute.Split('/');
            int sum1 =-1, sum2 =0;
            string[] comAttribute = mainAttribute[0].Split('_');

            switch (comAttribute[1])
            {
                case ("0"):
                    sum1 = FindInt(mainAttribute[1], card1);
                    break;
                case ("1"):
                    sum1 = FindInt(mainAttribute[1], card2);
                    break;
            }
            switch (comAttribute[2])
            {
                case ("0"):
                    sum2 = FindInt(mainAttribute[2], card1);
                    break;
                case ("1"):
                    sum2 = FindInt(mainAttribute[2], card2);
                    break;
                case ("2"):
                    if(sum1 > -1)
                        sum2 = sum1;
                    break;
            }

            switch (comAttribute[0])
            {
                case (" =")://0
                    return (sum1 == sum2);
                    break;
                case (" !=")://1
                    return (sum1 != sum2);
                    break;
                case (" >")://2
                    return (sum1 > sum2);
                    break;
                case (" <")://3
                    return (sum1 < sum2);
                    break;
            }

            Debug.Log(attribute);
            return false;
        }


        public static void PreCall(HeadSimpleTrigger head, CardBase _card1, CardBase _card2)
        {
            card1 = _card1;
            card2 = _card2;
            //SimpleTrigger simpleTrigger = null;
            for (int i = 0; i < head.SimpleTriggers.Count; i++)
            {
                Call(head.SimpleTriggers[i]);
            }
        }
        private static void Call(SimpleTrigger simpleTrigger)
        {//HeadSimpleTrigger simpleTrigger

            int noUse, use;
            noUse = CallSub(simpleTrigger.MinusPrior, simpleTrigger.CountMod);
            use = CallSub(simpleTrigger.PlusPrior, simpleTrigger.CountMod);

            SimpleAction action = null;
            if (simpleTrigger.CountModExtend)
            {
                int sum = use - noUse;
                for (int i = 0; i < simpleTrigger.Action.Count; i++)
                {
                    action = simpleTrigger.Action[i];
                    if (action.MinPoint <= sum && action.MaxPoint >= sum)
                        PreUseEffect(action.Action, action.ActionFull);
                   // PreUseCommand(simpleTrigger.Action[i].Action);
                }
            }
            else if (use > noUse)
            {
                for (int i = 0; i < simpleTrigger.Action.Count; i++)
                {
                    action = simpleTrigger.Action[i];
                    PreUseEffect(action.Action, action.ActionFull);
                }
            }

        }
        private static int CallSub(List<SimpleIfCore> simpleIfCore, bool extend)
        {
            int sum=0;
            SimpleIfCore simpleIf = null;
            for (int i = 0; i < simpleIfCore.Count; i++)
            {
                simpleIf = simpleIfCore[i];

                if (FindMenager(simpleIf.Attribute))
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

        private static void PreUseEffect(string action, string actionFull)
        {
            // string[] com = action.Split('/');
            string[] com = action.Split('_');
            //1-действие вообщем 2- уточнение 3 - конкретное действие
            // Die NewTarget (0 -мой, 1- вражеский 2- любой)


            CardBase local1 = card1;
            CardBase local2 = card2;
            switch (com[0])
            {
                case ("TargetCreature"):
                    UseEffect(actionFull, local1, local2);
                    //com[1] All  My Enemy
                    break;
                case ("NewTargetCreature"):
                    //CallNewTarget(); UseEffect(str)
                    break;
                case ("AllCreature"):
                    break;

                case ("Effect"):
                    switch (com[1])
                    {
                        case ("MyCard"):
                            //Die(local1);
                            break;
                        case ("TargetCard"):
                            //NewTarget("Effect/{com[2]}");
                            break;
                    }
                    break;

                case ("Die"):
                    switch (com[1])
                    {
                        case ("MyCard"):
                            //Die(local1);
                            break;
                        case ("TargetCard"):
                            //NewTarget("Die/{com[2]}");
                            break;
                    }
                    break;
            }
            /*
             * 
             * 
             * 
             * mood me,enemy,all
             TargetCreature
             AllCreature
             
            метамарфозы статов
             */
        }

        static void UseEffect(string actionFull, CardBase local1, CardBase local2)
        {
            string[] com = actionFull.Split('/');
            string[] com1 = com[0].Split('|');

            switch (com1[0]) 
            {
                case ("AddStat"):
                    AddStat("Local", com[1], local1, local2);//All-Max-Local
                    break;

                case ("AddEffect"):

                    Effect newEffect = new Effect();
                    newEffect.Name = com1[1];

                    if(com1[2] == "Eternal")
                    {
                        local2.InfinityEffect.Add(newEffect);
                    }
                    else
                    {
                        local2.Effect.Add(newEffect);
                        com1 = com[1].Split('|');
                        newEffect.Turn = FindInt(com1[0], local1) / int.Parse(com1[1]) + int.Parse(com1[2]);
                    }

                    com1 = com[2].Split('|');
                    newEffect.Power = FindInt(com1[0], local1) / int.Parse(com1[1]) + int.Parse(com1[2]);

                    com1 = com[3].Split('|');
                    newEffect.Prioritet = FindInt(com1[0], local1) / int.Parse(com1[1]) + int.Parse(com1[2]);

                    newEffect.Target = local2;
                    break;

                case ("Die"):
                    //Die(local2);
                    break;
                case ("Guard"):
                    local2.Guard.Add(local1);
                    break;
                //case ("Support"):
                //    local2.Support.Add(local1);
                //    break;
            }

        }
        
        public static void MelleAction(string actionFull, CardBase localCard1, CardBase localCard2)
        {
            string[] com = actionFull.Split('_');

            List<Constant> stat = new List<Constant>();
            List<int> statSize = new List<int>();
            List<string> mood = new List<string>();
            //int a = gameSetting.Library.Constants.FindIndex(x => x.Name == text);
            //if (group.Group)
            //{
            //}
            string text = com[1];
            int sum;
            Constant group = gameSetting.Library.Constants.Find(x => x.Name == text);
            if(group.Group)
                for (int i = 0; i < group.AntiConstant.Count; i++)
                {
                    text = group.AntiConstant[i].Name;
                    actionFull = $"{com[0]}_{text}_{com[2]}_{com[3]}";
                    sum = FindInt(actionFull, localCard1);
                    
                    if (sum > 0) 
                    {
                        stat.Add(gameSetting.Library.Constants.Find(x => x.Name == text));
                        statSize.Add(sum);
                        mood.Add(group.moodEffect); 
                    }
                    //a = stat.FindIndex(x => x.Name == text);
                    //if(a != -1)
                    //{
                    //    statSize[a] += sum;
                    //}
                    //else
                    //{
                    //    stat.Add(gameSetting.Library.Constants.Find(x => x.Name == text));
                    //    statSize.Add(sum);
                    //}

                }
            else
            {
                stat.Add(group);
                sum = FindInt(actionFull, localCard1);
                statSize.Add(sum);
                mood.Add(group.moodEffect);
            }


            int a, b;
            for (int i = 0; i < stat.Count; i++)
            {
                foreach (Constant actualStats in stat[i].AntiConstant)
                {
                    a = localCard2.Stat.FindIndex(x => x.Name == actualStats.Name);
                    if (a != -1)
                    {
                        foreach (Constant stats in localCard2.Stat[i].GuardConstant)
                        {
                            b = localCard2.Stat.FindIndex(x => x.Name == stats.Name);
                            if (b != -1)
                            {
                                statSize[i] -= localCard2.StatSize[b];
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
                                localCard2.StatSize[a] -= statSize[i];
                                localCard2.StatSizeLocal[a] -= statSize[i];
                                break;
                            case ("Max"):
                                localCard2.StatSize[a] -= statSize[i];
                                break;
                            case ("Local"):
                                localCard2.StatSizeLocal[a] -= statSize[i];
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

            CardView.ViewCard(localCard1);
            CardView.ViewCard(localCard2);
        }

        public static CardBase SwitchCard(CardBase localCard1, CardBase localCard2, string mood)
        {
            return (mood == "0") ? localCard1 : localCard2;
        }

        public static void AddStat(string mood, string actionFull, CardBase localCard1, CardBase localCard2)
        {
            string[] com = actionFull.Split('|');

            CardBase local1 = SwitchCard(localCard1, localCard2, com[0]);
            CardBase local2 = SwitchCard(localCard1, localCard2, com[2]);
            CardBase local3 = SwitchCard(localCard1, localCard2, com[4]);

            string text = com[6];
            int sum = FindInt(com[1], local1);
            int sum2 = FindInt(com[3], local2);
            if (sum2 != 0)
                sum /= sum2;
            sum += int.Parse(com[5]);

            int a = local3.Stat.FindIndex(x => x.Name == text);
            if(a == -1)
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
                        if (local3.StatSizeLocal[a] > local2.StatSize[a])
                            local3.StatSizeLocal[a] = local2.StatSize[a];
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
            //card = new List<CardBase>();
            List<int> cardBase = new List<int>();
            //интегрирывать из старгого метода позже
            List<int> cardBaseFast = new List<int>();
            List<int> cardBaseSlow = new List<int>();
            CardSet cardSet = XMLSaver.LoadCardSet(Application.dataPath + $"/Resources/CardSet");

            for (int i = 0; i < cardSet.OrigCard.Count; i++)
            {
                hiro.CardColod.Add(XMLSaver.Load(Application.dataPath + $"/Resources/Data/Hiro{cardSet.OrigCard[i]}"));
                hiro.CardColod[i].MyHiro = hiro;
                Core.CardClear(hiro.CardColod[i]);

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
            foreach(string str in gameSetting.Library.AllTactic)
            {
                go = Instantiate(stolUi.OrigButton);
                trans = go.transform;
                trans.SetParent(stolUi.AllTacticCase);
                trans.GetChild(0).gameObject.GetComponent<Text>().text = str;
                stolUi.AllTactic.Add(trans);

                go.GetComponent<Button>().onClick.AddListener(() => HiroHead.UseTactic(str));
            }
        }

        static void CreateUiCard(CardBase card)
        {
            Transform myHand = (card.MyHiro.Team == 0) ? stolUi.MyHand : stolUi.EnemyHand;

            GameObject GO = Instantiate(stolUi.OrigCard);
            card.Body = GO.transform;
            card.Body.SetParent(myHand);

            GO.GetComponent<Button>().onClick.AddListener(() => HiroHead.Reply(card));


            CardView.ViewCard(card);
            //return GO.transform;
        }

    }


    public class HiroHead
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

        static void ReplyStol(bool extend, bool enemy)
        {
            Hiro myHiro = (enemy) ? hiro[1] : hiro[0];
            if (calls.Count > 0)
            {
                bool use = false;
                CardBase firstCard = calls[0].Card;
                string mood = calls[0].Mood;
                switch (calls[0].Text)//calls[0].Action
                {
                    case ("UseSlot"):

                        if (firstCard.MyHiro.ManaCurent >= firstCard.Mana)
                            if (CallMood(myHiro, firstCard, mood))
                            {

                                firstCard.MyHiro.ManaCurent -= firstCard.Mana;
                                if(enemy)
                                    firstCard.Body.SetParent(stolUi.EnemyFirstStol);
                                else
                                    firstCard.Body.SetParent(stolUi.MyFirstStol);

                                firstCard.Tayp = "Create";
                                int a = firstCard.Stat.FindIndex(x => x.Name == "MeleeDamage");
                                if (a != -1)
                                    firstCard.Tactic.Add("Attack");
                                a = firstCard.Stat.FindIndex(x => x.Name == "RangedDamage");
                                if (a != -1)
                                    firstCard.Tactic.Add("Shot");

                                HiroUi(firstCard.MyHiro);
                            }

                        use = true;
                        break;
                    default:
                        Debug.Log(calls[0].Text);
                        break;
                }
                if (use)
                {
                    calls.RemoveAt(0);
                    if (calls.Count > 0)
                        Reply(null);
                }

            }
        }
        
        public static void Reply(CardBase card)
        {
            if (calls.Count > 0)
            {
                bool use = false;
                CardBase firstCard = calls[0].Card;
                string mood = calls[0].Mood; 
                if (card == null)
                    switch (calls[0].Text)//calls[0].Action
                    {
                        
                        default:
                            Debug.Log(calls[0].Text);
                            break;
                    }
                else
                    switch (calls[0].Text)//calls[0].Action
                    {
                        case ("Attack"):
                            if (CallMood(firstCard.MyHiro, card, mood))
                                GameEventSystem.MelleAction("",firstCard,card); 

                                break;
                        case ("Shot"):

                            break;
                        //case ("UseSlot"):
                        //    if (firstCard == null)
                        //    {
                        //        if (CallMood(hiro, card1, mood))
                        //        {
                        //            card1.MyHiro.ManaCurent -= card1.Mana;
                        //            Create.PlayCard(card1);
                        //        }
                        //    }
                        //    break;
                        default:
                            Debug.Log(calls[0].Text);
                            break;
                    }
                

                if (use)
                {
                    calls.RemoveAt(0);
                    if (calls.Count > 0)
                        Reply(null);
                }
            }
            else if (card != null)
            {
                switch (card.Tayp)
                {
                    case ("HandCreate"):
                        if (card.MyHiro.ManaCurent >= card.Mana)
                        {
                            AddCall("UseSlot", "My", "CreateBody", card);
                        }
                        break;
                    case ("Create"):
                        if (card.Tactic.Count > 0)
                            if (card.Tactic.Count == 1)
                            {
                                AddCall(card.Tactic[0], "My", "CreateBody", card);
                            }
                            else
                                TacticList(card);
                                //if (card.MyHiro.ManaCurent >= card.Mana)
                                //{
                                //AddCall("Attack", "My", "CreateBody", card);
                                //}
                        break;
                    default:
                        Debug.Log(card.Tayp);
                        break;
                }

                Reply(null);
            }

        }


        public static void UseTactic(string str)
        {
            calls[0].Text = str;
            Reply(null);
            //AddCall(str, "My", "CreateBody", card);
        }

        static bool CallMood( Hiro hiro, CardBase card, string mood)
        {
            switch (mood)
            {
                case ("All"):
                    return true;
                    break;
                case ("My"):
                    if(hiro.Team == card.MyHiro.Team)
                        return true;
                    break;
                case ("Enemy"):
                    if (hiro.Team != card.MyHiro.Team)
                        return true;
                    break;
            }
            return false;
        }

        static void TacticList(CardBase card)
        {
            AddCall("Void", "My", "CreateBody", card);
            stolUi.TacticCase.gameObject.active = true;
            foreach (Transform child in stolUi.TacticCase)
            {
                child.SetParent(stolUi.AllTacticCase);
            }
            int a = 0;

            foreach (string tactic in card.Tactic)
            {
                a = gameSetting.Library.AllTactic.FindIndex(x => x == tactic);
                stolUi.AllTactic[a].SetParent(stolUi.TacticCase);
            }
            
        }

        public static void RemoveCall()
        {
            calls.RemoveAt(0);
        }

        public static void Install()
        {
            //hiro = new Hiro[2];
            Create.gameSetting = gameSetting;
            Create.stolUi = stolUi;

            Create.CreateHiro(false);
            Create.CreateHiro(true);

            hiro = Create.hiro;
            GameEventSystem.SetHiro(hiro);

            Create.CreateTacticList();

            NextTurn();
            NextTurn();


            stolUi.NextTurn.onClick.AddListener(() => NextTurn());
            stolUi.MyFirstStol.gameObject.GetComponent<Button>().onClick.AddListener(() => ReplyStol(false, false));

            stolUi.EnemyFirstStol.gameObject.GetComponent<Button>().onClick.AddListener(() => ReplyStol(false, true));
            //true, false));
            // PlayCard(ca
        }
        #endregion
        static void AddCall (string text, string mood, string action, CardBase card)
        {
            SubString call = new SubString();
            call.Text = text;
            call.Mood = mood;
            call.Action = action;
            //call.Num = num;
            call.Card = card;

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
            hiro.Ui.text = $"Hp {hiro.Hp} Mana ({hiro.ManaMax}|{hiro.Mana}|{hiro.ManaCurent})";
        }
    }

    public class TableRule : MonoBehaviour
    {
        public static GameSetting gameSetting;

        //static void LoadAction(RealCard card, CardBase cardBase)
        //{
        //    card.Action = new List<int>();
        //    card.ShotAction = new List<int>();
        //    card.PasiveAction = new List<int>();
        //    card.Trait = new List<string>();
        //    card.Effect = new List<Effect>();

        //    int a = 0;
        //    if (card.MeleeDMG > 0) 
        //    {
        //        a = gameSetting.Library.Action.FindIndex(x => x.Name == "Slash");//.Action.Find(x => x.Name == "Slash"));
        //        card.Action.Add(a);
        //    }
        //    if (card.ShotDMG > 0)
        //    {
        //        a = gameSetting.Library.Action.FindIndex(x => x.Name == "Hit");
        //        card.ShotAction.Add(a);
        //    }

        //    string traitCase = "";
        //    Trait trait = null;
        //    int b = 0;
        //    a = cardBase.Trait.Count;
        //    for(int i = 0; i < a; i++)
        //    {
        //        traitCase = cardBase.Trait[i];
        //        card.Trait.Add(traitCase);
        //        if (traitCase != "")
        //        {
        //            b = gameSetting.Library.Action.FindIndex(x => x.Name == traitCase);
        //            if (b == -1)
        //                Debug.Log(traitCase);
        //            trait = gameSetting.Library.Action[b];

        //            switch (trait.ClassTayp)
        //            {
        //                case ("Action")://Action
        //                    card.Action.Add(b);
        //                    break;

        //                case ("ShotAction")://ShotAction
        //                    card.ShotAction.Add(b);
        //                    break;

        //                case ("PasiveAction")://PasivAction
        //                    card.PasiveAction.Add(b);
        //                    break;
        //            }

        //            // b = card.Trait.FindIndex(x => x == "Dash");
        //            switch (traitCase)
        //            {
        //                case ("Dash"):
        //                    CloseMetod.IRestoreMP(card);
        //                    break;

        //                case ("Rush"):
        //                    CloseMetod.IRestoreMP(card);
        //                    StatusSystem.IAddEffect("NoHead", 1, card);
        //                    break;

        //                case ("Provacator"):
        //                    if (card.Position == 0)
        //                        card.HiroMain.Provacator.Add(card);
        //                    break;

        //                case ("BodyGuard"):
        //                   // b = gameSetting.Library.Action.FindIndex(x => x.Name == traitCase);
        //                    stol.SelectCardSpellTarget(b, card);
        //                    //if (card.Position == 0)
        //                    //    card.HiroMain.Provacator.Add(card);
        //                    break;

        //                //case ("Provacator"):
        //                //    if (card.Position == 0)
        //                //        card.HiroMain.Provacator.Add(card);
        //                //    break;
        //            }
        //        }
        //    }

        //}
        //static void IPlayCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        //{
        //  //  int b = hiro1.CardColod[handNum].Stat[hiro1.CardColod[handNum].Stat.Count - 1];
        //  //  hiro1.ManaCurent -= b;

        //    ICreateCard(hiro1, hiro2, handNum, slot, pos);
        //    stol.PostUse(true);
        //}

        //public static void IUseCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        //{
        //    CardBase card = hiro1.CardColod[handNum];
        //    int b = 0;// card.Stat[card.Stat.Count - 1];
        //    if (hiro1.ManaCurent >= b)
        //    {
        //        bool targetHiro = false;
        //        RealCard targetCard = hiro2.Slots[slot].Position[pos];
        //        if (targetCard != null) 
        //            targetHiro = true;

        //        if (!targetHiro)
        //        {
        //            if(hiro1.Team == hiro2.Team)
        //            {
        //                if(pos == 1)
        //                {
        //                    b = card.Trait.FindIndex(x => x == "ArGuard");
        //                    if (b != -1)
        //                    {
        //                        targetCard = hiro2.Slots[slot].Position[0];
        //                        if (targetCard != null)
        //                        {
        //                            b = targetCard.Trait.FindIndex(x => x == "AvGuard");
        //                            if (b != -1)
        //                                IPlayCard(hiro1, hiro2, handNum, slot, pos);
        //                        }
        //                        else
        //                            IPlayCard(hiro1, hiro2, handNum, slot, pos);
        //                    }
        //                }
        //                else
        //                {
        //                    IPlayCard(hiro1, hiro2, handNum, slot, pos);
        //                }
        //            }

        //           // ICreateCard(hiro1, hiro2, handNum, slot, pos);

        //            //IPlayCard(hiro1, hiro2, handNum, slot, pos);//¬ременно заморожено
        //            //stol.PostUse(true);
        //        }
        //       // switch(targetCard)
        //    }
         

        //}//процедура определени€ задачи карты и возможности ее реализации
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
                if (card.Trait[i] != "")
                    Ui.Trait.text += $"{card.Trait[i]} \n";
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
        //    //int a - используетс€ дл€ указани€ на использование первой или второй линин позиции
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
