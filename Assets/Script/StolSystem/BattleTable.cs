using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace BattleTable
{
    public class Core
    {
        public static void LoadGameSetting(GameSetting gameSetting)
        {
            BattleSystem.gameSetting = gameSetting;
            TableRule.gameSetting = gameSetting;
            CardView.gameSetting = gameSetting;
            GameEventSystem.gameSetting = gameSetting;
        }
        public static void SetStol(Stol stol)
        {
            BattleSystem.stol = stol;
            TableRule.stol = stol;
            GameEventSystem.stol = stol;
        }
    }

    class GameEventSystem
    {
        public static GameSetting gameSetting;
        public static Stol stol;
        public static CardBase card1, card2;

        /*
         В чей ход выполняется эффект
        в какую фазу выполняем условие
         
        три базовых действия

        возможность разыграть карту
        действие атаки
        действие смерти

        все триггеры событий
        карта есть в колоде
        карта есть в руке
        карта пришла в руку
        карта была разыгранна
        карта лежит на столе в конце хода
        карта умерла
        карта атаковала
        карта получила урон и не умерла
        карта получила баф(внешний, иначе поулчится цикл)
         */
        //List<TriggerAction> triggerActions = new List<TriggerAction>();
        //List<CardBase> cardBases = new List<CardBase>();
        //List<int> colod = new List<int>();
        //List<int> hand = new List<int>();

        private static int FindInt(string attribute, CardBase cardBase)
        {
            string[] comAttribute = attribute.Split('_');
            int sum = -1;
            switch (comAttribute[0])
            {
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
                            //sum = 0;
                            return -1;
                            break;
                        case ("One"):
                            sum = 1;
                            break;
                        default:
                            for(int i=0; i< cardBase.Stat.Count; i++)
                            {
                                if(cardBase.Stat[i].Name == comAttribute[1])
                                {
                                    sum = cardBase.StatSize[i];
                                    i = cardBase.Stat.Count;
                                }
                            }
                            break;
                    }

                    sum *= int.Parse(comAttribute[2]);
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

            return sum;
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
                case ("-1"):
                    if(sum1 > -1)
                        sum2 = sum1;
                    break;
            }

            switch (comAttribute[0])
            {
                case (" ="):
                    return (sum1 == sum2);
                    break;
                case (" !="):
                    return (sum1 != sum2);
                    break;
                case (" >"):
                    return (sum1 > sum2);
                    break;
                case (" <"):
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
           // string[] com = str.Split('/');
            //string[] com1 = com[0].Split('_');

            CardBase local1 = card1;
            CardBase local2 = card2;
            switch (action)
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

                    TestEffect newEffect = new TestEffect();
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
        static void MelleAction()
        {

        }

        static void AddStat(string mood, string actionFull, CardBase local1, CardBase local2)
        {
            string[] com = actionFull.Split('|');
            int sum = FindInt(com[0], local1) / int.Parse(com[1]) + int.Parse(com[2]);
            int a = local2.Stat.FindIndex(x => x.Name == com[3]);
            if(a == -1 && mood != "Local")
            {
                a = gameSetting.Library.Constants.FindIndex(x => x.Name == com[3]);
                local2.Stat.Add(gameSetting.Library.Constants[a]);
                switch (mood)
                {
                    case ("All"):
                        local2.StatSize.Add(sum);
                        local2.StatSizeLocal.Add(sum);
                        break;
                    case ("Max"):
                        local2.StatSize.Add(sum);
                        local2.StatSizeLocal.Add(0);
                        break;
                    case ("LocalForse"):
                        local2.StatSize.Add(0);
                        local2.StatSizeLocal.Add(sum);
                        break;

                }
            }
            //else  if (local1.Team != local2.Team)
            //{

            //}
            else
            {
                switch (mood) 
                {
                    case ("All"):
                        local2.StatSize[a] += sum;
                        local2.StatSizeLocal[a] += sum;
                        break;
                    case ("Max"):
                        local2.StatSize[a] += sum;
                        break;
                    case ("Local"):
                        local2.StatSizeLocal[a] += sum;
                        if (local2.StatSizeLocal[a] > local2.StatSize[a])
                            local2.StatSizeLocal[a] = local2.StatSize[a];
                        break;
                    case ("LocalForse"):
                        local2.StatSizeLocal[a] += sum;
                        break;

                }

            }
        }

        //void CollectToken(string statOrig, CardBase card1)
        //{
        //    /*
        //           стат1 - тот который передаем цели
        //          стат2 - донор, его параметр конвертируеться

        //          объявляется иницаторы событий
        //          карата 1 и карта 2
        //          карта 2 может задействоаать телохранителей и защитников
        //          телохранители принмают на себя урон, прикрывая цель, 
        //          защитники бьют карту 1 без ответа

        //          List<Constant> stat;
        //          List<string> statSize;

        //          AddPreEffect();
        //          AddPostEffect();
        //           */
        //    int a = gameSetting.Constatnt.FindIndex(x => x.Name == statOrig);
        //    Constatnt stat = gameSetting.Constatnt[a];
        //    int sum = 0;
        //    if (stat.Group)
        //    {
        //        for (int i=0;i< stat.TwinConstant.Count)
        //        {
        //            a = card1.Stat.FindIndex(x => x.Name == stat.TwinConstant[i].Name);
        //            if (a > -1)
        //                sum += card1.StatSize[a];
        //        }
        //    }
        //    else
        //    {
        //        a = card1.Stat.FindIndex(x => x.Name == stat);
        //        if (a > -1)
        //            sum = card1.StatSize[a];
        //    }
        //}
    }



    class StatusSystem : MonoBehaviour
    {
        public static void IAddEffect(string name, int size, RealCard card)
        {
            int a = card.Effect.Count;
            Effect effect = null;
            for (int i = 0; i < a; i++)
            {
                effect = card.Effect[i];
                if (effect.Name == name)
                {
                    effect.Size += size;
                    if (effect.Size <= 0)
                        card.Effect.RemoveAt(i);
                    CardView.ViewEffect(card);
                    return;
                }
            }
            /*
             гибкость 3 существо при выходе получает авангард арьергард или +1\+1
             
             
             */
            effect = new Effect();
            effect.Name = name;
            effect.Size = size;
            card.Effect.Add(effect);
        }
        public static void IDie(RealCard card)
        {
            Destroy(card.Body);
            card.HiroMain.Army.Remove(card);
            if (card.ShotDMG > 0)
                card.HiroMain.ShotHiro--;

            
            CardView.ViewSlotClear(card);
            card.HiroMain.Slots[card.Slot].Position[card.Position] = null;
        }

        public static void IStun(RealCard card)
        {
            //Destroy(card.Body);
            //card.HiroMain.Army.Remove(card);
            //if (card.ShotDMG > 0)
            //    card.HiroMain.ShotHiro--;

            //card.HiroMain.Slots[card.Slot].Position[card.Position] = null;
        }
    }

    public static class BattleSystem
    {
        public static Stol stol;
        public static GameSetting gameSetting;
        public static void IStatys(RealCard card)
        {
            List<int> blackList = new List<int>();

            int a = card.Effect.Count;
            Effect effect = null;
            for(int i = 0; i < a; i++)
            {
                effect = card.Effect[i];
                effect.Size--;
                switch (effect.Name)
                {
                    case ("Stun"):
                        StatusSystem.IStun(card);
                        break;
                }
                if (effect.Size <= 0)
                    blackList.Add(i);
            }

            a = blackList.Count;
            for (int i = a; i > 0; i--)
            {
                card.Effect.RemoveAt(blackList[i]);
            }
        }

        #region Action Head
        static void ISlashHead(RealCard card1, Hiro hiro)
        {
           
        }

        static void IShotHead(RealCard card1, Hiro hiro)
        {
           
        }
        #endregion
        #region Action Card
        static void ISlash(RealCard card1, RealCard card2)
        {
            //  card1.MovePoint--;


            card1.Hp -= card2.MeleeDMG;
            card2.Hp -= card1.MeleeDMG;

            if (card1.Hp <= 0)
            {

                StatusSystem.IDie(card1);
            }
            else
            {
                //string action = "";
                //int a = card1.PasiveAction.Count;
                //int c = 0;
                //int d = 0;
                //for (int i = 0; i < a; i++)
                //{
                //    c = card1.PasiveAction[i];
                //    action = gameSetting.Library.Action[c].Name;
                //    switch (action)
                //    {
                //        case("Piercing");
                //            d = card2.Hp;
                //            if (d < 0)
                //            {
                //                Hiro hiro = card2.HiroMain;
                //                hiro.Hp -= d;
                //                stol.HiroUi(hiro);
                //            }
                //            break;
                //    }
                //}
                CardView.ViewSlotUi(card1);
            }

            if (card2.Hp <= 0)
                StatusSystem.IDie(card2);
            else
                CardView.ViewSlotUi(card2);
        }

        static void IShot(RealCard card1, RealCard card2)
        {
           // card1.MovePoint--;
            card2.Hp -= card1.ShotDMG;

            //if (card1.Hp <= 0)
            //    StatusSystem.IDie(card1);
            //else
            //    CardView.IViewSlotUi(card1);

            if (card2.Hp <= 0)
                StatusSystem.IDie(card2);
            else
                CardView.ViewSlotUi(card2);
        }
        #endregion

        public static void IUseActionHead(string actionTayp, RealCard card, Hiro hiro)
        {
            int b = card.Effect.FindIndex(x => x.Name == "NoHead");

            if (b == -1) 
            { 
                b = gameSetting.Library.Action.FindIndex(x => x.Name == actionTayp);
                int a = gameSetting.Library.Action[b].MoveCost;

                if (card.MovePoint >= a)
                {
                    bool useAction = false;
                    switch (actionTayp)
                    {
                        case ("Slash"):

                            //ISlashHead
                            useAction = true;
                            break;
                        case ("Hit"):
                            //IShotHead
                            useAction = true;
                            break;
                        default:
                            Debug.Log(actionTayp);
                            return;
                            break;
                    }

                    if (useAction)
                    {
                        card.MovePoint -= a;
                        stol.PostUse(true);
                    }
                }
            }
        }

        public static void IUseAction(string actionTayp, RealCard card1, RealCard card2)
        {
            int b = gameSetting.Library.Action.FindIndex(x => x.Name == actionTayp);
            int a = gameSetting.Library.Action[b].MoveCost;

            if (card1.MovePoint >= a)
            {
                a = card2.HiroMain.Provacator.Count;
                bool useAction = false;
                switch (actionTayp)
                {
                    case ("Slash"):
                        if (card2 != null)
                            if (card1.Team != card2.Team)
                                if (card2.Position == 0)
                                {
                                    if( a > 0)
                                    {
                                        RealCard card3 = null;
                                        for (int i = 0; i < a; i++)
                                        {
                                            card3 = card2.HiroMain.Provacator[i];
                                            if (card3.Slot == card2.Slot)
                                            {
                                                i = a;
                                                useAction = true;
                                            }
                                        }
                                    }
                                    else
                                        useAction = true;

                                    if (useAction)
                                    {
                                        ISlash(card1, card2);
                                    }
                                }

                        break;
                    case ("Hit"):
                        if (card2 != null)
                            if (card1.Team != card2.Team)
                                if (card1.ShotDMG > 0)
                                {
                                    if (a > 0)
                                    {
                                        if (card2.Position == 0)
                                        {
                                            RealCard card3 = null;
                                            for (int i = 0; i < a; i++)
                                            {
                                                card3 = card2.HiroMain.Provacator[i];
                                                if (card3.Slot == card2.Slot)
                                                {
                                                    i = a;
                                                    useAction = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        useAction = true;

                                    if (useAction)
                                    {
                                        IShot(card1, card2); 
                                    }
                                }
                        break;
                    default:
                        Debug.Log(actionTayp);
                        return;
                        break;
                }

                Debug.Log(a);
                if (useAction)
                {
                    ////  int b = gameSetting.Library.Action.FindIndex(x => x.Name == actionTayp);

                    card1.MovePoint -= a;
                    stol.PostUse(true);
                }
            }
        }

       public static void NewTurn(Hiro hiro)
        {
            RealCard card = null;
            int a = hiro.Army.Count;
            for(int i = 0; i < a; i++)
            {
                card = hiro.Army[i];
                CloseMetod.IRestoreMP(card);
                IStatys(card);
              //  card.MovePoint = 1;
            }
        }

    }
    class CloseMetod 
    {
        public static void IRestoreMP(RealCard card)
        {
            card.MovePoint = 1;
        }

    }


    public class TableRule : MonoBehaviour
    {
        public static Stol stol;
        public static GameSetting gameSetting;
        static void ICreateCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        {
            RealCard card = new RealCard();
            CardBase cardBase = hiro1.CardColod[handNum];

            card.Name = cardBase.Name;

            //card.MeleeDMG = cardBase.Stat[0];
            //card.ShotDMG = cardBase.Stat[1];
            //card.NoArmorDMG = cardBase.Stat[2];
            //card.ArmorBreakerDMG = cardBase.Stat[3];

            //card.Hp = cardBase.Stat[4];
            //card.Helmet = cardBase.Stat[5];
            //card.Shild = cardBase.Stat[6];
            //card.Armor = cardBase.Stat[7];

            //card.Agility1 = cardBase.Stat[8];
            //card.Agility2 = cardBase.Stat[9];
            //card.Agility3 = cardBase.Stat[10];
            //card.Agility4 = cardBase.Stat[11];

            //card.Mana = cardBase.Stat[12];


            card.Line = hiro2.Team;
            card.Slot = slot;
            card.Position = pos;
            card.Team = hiro1.Team;
            card.Id = handNum;
            card.HiroMain = hiro1;


            hiro1.Army.Add(card);

            hiro2.Slots[slot].Position[pos] = card;
            card.Ui = hiro2.OrigSlots[slot].GetChild(2 + pos).gameObject.GetComponent<TMP_Text>();


            if (card.ShotDMG > 0)
                card.HiroMain.ShotHiro++;

            GameObject GO = Instantiate(gameSetting.OrigHiro);

            card.Body = GO;
            GO.transform.position = hiro2.OrigSlots[slot].GetChild(pos).position;

            //if (card.Team == 0)
            //    GO.transform.position =  new Vector3(slot * 3, 0, -(pos * 4) + 5);
            //else
            //    GO.transform.position =  new Vector3(slot * 3, 0, pos * 4 + 1 + 10);

            //  cardBase.Body.active = false;

            Destroy(cardBase.Body.gameObject);
            hiro1.CardHand.Remove(handNum);

            //прогрузка спосбностей
            LoadAction(card, cardBase);
            CardView.ViewSlotUi(card);
        }

        static void LoadAction(RealCard card, CardBase cardBase)
        {
            card.Action = new List<int>();
            card.ShotAction = new List<int>();
            card.PasiveAction = new List<int>();
            card.Trait = new List<string>();
            card.Effect = new List<Effect>();

            int a = 0;
            if (card.MeleeDMG > 0) 
            {
                a = gameSetting.Library.Action.FindIndex(x => x.Name == "Slash");//.Action.Find(x => x.Name == "Slash"));
                card.Action.Add(a);
            }
            if (card.ShotDMG > 0)
            {
                a = gameSetting.Library.Action.FindIndex(x => x.Name == "Hit");
                card.ShotAction.Add(a);
            }

            string traitCase = "";
            Trait trait = null;
            int b = 0;
            a = cardBase.Trait.Count;
            for(int i = 0; i < a; i++)
            {
                traitCase = cardBase.Trait[i];
                card.Trait.Add(traitCase);
                if (traitCase != "")
                {
                    b = gameSetting.Library.Action.FindIndex(x => x.Name == traitCase);
                    if (b == -1)
                        Debug.Log(traitCase);
                    trait = gameSetting.Library.Action[b];

                    switch (trait.ClassTayp)
                    {
                        case ("Action")://Action
                            card.Action.Add(b);
                            break;

                        case ("ShotAction")://ShotAction
                            card.ShotAction.Add(b);
                            break;

                        case ("PasiveAction")://PasivAction
                            card.PasiveAction.Add(b);
                            break;
                    }

                    // b = card.Trait.FindIndex(x => x == "Dash");
                    switch (traitCase)
                    {
                        case ("Dash"):
                            CloseMetod.IRestoreMP(card);
                            break;

                        case ("Rush"):
                            CloseMetod.IRestoreMP(card);
                            StatusSystem.IAddEffect("NoHead", 1, card);
                            break;

                        case ("Provacator"):
                            if (card.Position == 0)
                                card.HiroMain.Provacator.Add(card);
                            break;

                        case ("BodyGuard"):
                           // b = gameSetting.Library.Action.FindIndex(x => x.Name == traitCase);
                            stol.SelectCardSpellTarget(b, card);
                            //if (card.Position == 0)
                            //    card.HiroMain.Provacator.Add(card);
                            break;

                        //case ("Provacator"):
                        //    if (card.Position == 0)
                        //        card.HiroMain.Provacator.Add(card);
                        //    break;
                    }
                }
            }

        }
        static void IPlayCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        {
          //  int b = hiro1.CardColod[handNum].Stat[hiro1.CardColod[handNum].Stat.Count - 1];
          //  hiro1.ManaCurent -= b;

            ICreateCard(hiro1, hiro2, handNum, slot, pos);
            stol.PostUse(true);
        }

        public static void IUseCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        {
            CardBase card = hiro1.CardColod[handNum];
            int b = 0;// card.Stat[card.Stat.Count - 1];
            if (hiro1.ManaCurent >= b)
            {
                bool targetHiro = false;
                RealCard targetCard = hiro2.Slots[slot].Position[pos];
                if (targetCard != null) 
                    targetHiro = true;

                if (!targetHiro)
                {
                    if(hiro1.Team == hiro2.Team)
                    {
                        if(pos == 1)
                        {
                            b = card.Trait.FindIndex(x => x == "ArGuard");
                            if (b != -1)
                            {
                                targetCard = hiro2.Slots[slot].Position[0];
                                if (targetCard != null)
                                {
                                    b = targetCard.Trait.FindIndex(x => x == "AvGuard");
                                    if (b != -1)
                                        IPlayCard(hiro1, hiro2, handNum, slot, pos);
                                }
                                else
                                    IPlayCard(hiro1, hiro2, handNum, slot, pos);
                            }
                        }
                        else
                        {
                            IPlayCard(hiro1, hiro2, handNum, slot, pos);
                        }
                    }

                   // ICreateCard(hiro1, hiro2, handNum, slot, pos);

                    //IPlayCard(hiro1, hiro2, handNum, slot, pos);//Временно заморожено
                    //stol.PostUse(true);
                }
               // switch(targetCard)
            }
         

        }//процедура определения задачи карты и возможности ее реализации
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

        static void ViewSlot(RealCard newPosition, MeshRenderer mesh, string mood, int b, int team, RealCard curentCard, int line)
        {

            mesh.material = gameSetting.TargetColor[0];
            //int a - используется для указания на использование первой или второй линин позиции
            if(newPosition != null)
                switch (mood)
                {
                    case ("ShotView"):
                        if (newPosition.Team == line)
                            if (newPosition.MovePoint > 0)
                                if (newPosition.ShotAction.Count > 0)
                                {
                                    mesh.material = gameSetting.TargetColor[1];
                                }
                        break;

                    case ("ShotTarget"):
                        if (newPosition.Team != line)
                            //if (line == team)
                                mesh.material = gameSetting.TargetColor[2];
                        else if (newPosition == curentCard)
                            mesh.material = gameSetting.TargetColor[3];

                        break;

                    case ("MeleeView"):
                        if (newPosition.Team == line)
                            if (newPosition.MovePoint > 0)
                                if (newPosition.Action.Count > 0)
                                {
                                    mesh.material = gameSetting.TargetColor[1];
                                }
                        break;

                    case ("MeleeTarget"):
                        if (newPosition.Team != line)
                        {
                           // if (line == team)
                                if (b == 0)
                                    mesh.material = gameSetting.TargetColor[2];
                        }
                        else if (newPosition == curentCard)
                            mesh.material = gameSetting.TargetColor[3];

                        break;

                    default:
                        break;
                }
            else
                switch (mood)
                {
                    case ("SetCard"):
                        if (line == team)
                            if (b == 0)
                            {
                                mesh.material = gameSetting.TargetColor[1];
                            }
                        break;
                    default:
                        break;
                }
        }

        public static void ViewLoadUi(Hiro newHiro, string mood, RealCard curentCard, Transform[] slots, int line)
        {
          //  Transform[] slots = null;
            Slot[] hiroSlot = newHiro.Slots;

            int team = newHiro.Team;
            if(curentCard!= null)
                team = curentCard.Team;
            // Slot newSlot = null;

            //if (newHiro.Team == 0)
            //    slots = Ui.MySlot;
            //else
            //    slots = Ui.EnemySlot;

            Slot newSlot = null;
            RealCard newPosition = null;
            MeshRenderer mesh = null;
            int a = slots.Length;
            int b = 0;
            for (int i = 0; i < a; i++)
            {
                newSlot = hiroSlot[i];

                b = 0;
                ViewSlot(newSlot.Position[b], newSlot.Mesh[b], mood, b, team, curentCard, line);

                b++;
                ViewSlot(newSlot.Position[b], newSlot.Mesh[b], mood, b, team, curentCard, line);
            }
        }

        public static void ViewEffect(RealCard card)
        {

        }
        public static void ViewTargetCard(CardBase cardBase, Transform Ui)
        {

            Ui.gameObject.active = true;
            Transform trans = cardBase.Body;
            cardBase.Body = Ui;
            ViewCard(cardBase);
            cardBase.Body = trans;
        }

        static void ViewSlotHandler(int a, int b, TMP_Text text)
        {
            if (a > 0)
                text.text += $"<sprite name={gameSetting.NameIcon[b]}>{a} ";
        }
        public static void ViewSlotUi(RealCard card)
        {
            TMP_Text text = card.Ui;
            text.text = "";

            int[] stat = new int[] 
            { 
                card.MeleeDMG,
                card.ShotDMG,
                card.NoArmorDMG,
                card.ArmorBreakerDMG,

                card.Hp,
                card.Helmet,
                card.Shild,
                card.Armor,

                card.Agility1,
                card.Agility2,
                card.Agility3,
                card.Agility4
            };

            int a = stat.Length;
            for (int i = 0; i < a; i++)
            {
                ViewSlotHandler(stat[i], i, text);
            }




        }
        public static void ViewSlotClear(RealCard card)
        {
            card.Ui.text = "";
        }
    }
}
