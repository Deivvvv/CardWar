using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace BattleTable
{
    public class Core
    {
        public static void ILoadGameSetting(GameSetting gameSetting)
        {
            BattleSystem.gameSetting = gameSetting;
            TableRule.gameSetting = gameSetting;
            CardView.gameSetting = gameSetting;
        }
        public static void ISetStol(Stol stol)
        {
            BattleSystem.stol = stol;
            TableRule.stol = stol;
        }
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
                    CardView.IViewEffect(card);
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

            
            CardView.IViewSlotClear(card);
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
                CardView.IViewSlotUi(card1);
            }

            if (card2.Hp <= 0)
                StatusSystem.IDie(card2);
            else
                CardView.IViewSlotUi(card2);
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
                CardView.IViewSlotUi(card2);
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

            card.MeleeDMG = cardBase.Stat[0];
            card.ShotDMG = cardBase.Stat[1];
            card.NoArmorDMG = cardBase.Stat[2];
            card.ArmorBreakerDMG = cardBase.Stat[3];

            card.Hp = cardBase.Stat[4];
            card.Helmet = cardBase.Stat[5];
            card.Shild = cardBase.Stat[6];
            card.Armor = cardBase.Stat[7];

            card.Agility1 = cardBase.Stat[8];
            card.Agility2 = cardBase.Stat[9];
            card.Agility3 = cardBase.Stat[10];
            card.Agility4 = cardBase.Stat[11];

            card.Mana = cardBase.Stat[12];


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
            ILoadAction(card, cardBase);
            CardView.IViewSlotUi(card);
        }

        static void ILoadAction(RealCard card, CardBase cardBase)
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
            int b = hiro1.CardColod[handNum].Stat[hiro1.CardColod[handNum].Stat.Count - 1];
            hiro1.ManaCurent -= b;

            ICreateCard(hiro1, hiro2, handNum, slot, pos);
            stol.PostUse(true);
        }

        public static void IUseCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        {
            CardBase card = hiro1.CardColod[handNum];
            int b = card.Stat[card.Stat.Count - 1];
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
        public static void IViewCard(CardBase card)
        {
           // Transform trans = card.Body;
            CardBaseUi Ui = card.Body.gameObject.GetComponent<CardBaseUi>();

            Texture2D texture = new Texture2D(100, 150);
            texture.LoadImage(card.Image);
            Ui.Avatar.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            Ui.Name.text = card.Name;

            Ui.Stat.text = "";
            for (int i = 0; i < card.Stat.Count - 1; i++)
            {
                if (card.Stat[i] > 0)
                    Ui.Stat.text += $"<sprite name={gameSetting.NameIcon[i]}>{card.Stat[i]} ";
            }

            //  Ui.Trait;

            Ui.Mana.text = "" + card.Stat[card.Stat.Count - 1];

            //if(Ui.Count != null)
            //        Ui.Count.text;
        }

        static void IViewSlot(RealCard newPosition, MeshRenderer mesh, string mood, int b, int team, RealCard curentCard, int line)
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

        public static void IViewLoadUi(Hiro newHiro, string mood, RealCard curentCard, Transform[] slots, int line)
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
                IViewSlot(newSlot.Position[b], newSlot.Mesh[b], mood, b, team, curentCard, line);

                b++;
                IViewSlot(newSlot.Position[b], newSlot.Mesh[b], mood, b, team, curentCard, line);
            }
        }

        public static void IViewEffect(RealCard card)
        {

        }
        public static void IViewTargetCard(CardBase cardBase, Transform Ui)
        {

            Ui.gameObject.active = true;
            Transform trans = cardBase.Body;
            cardBase.Body = Ui;
            IViewCard(cardBase);
            cardBase.Body = trans;
        }

        static void IViewSlotHandler(int a, int b, TMP_Text text)
        {
            if (a > 0)
                text.text += $"<sprite name={gameSetting.NameIcon[b]}>{a} ";
        }
        public static void IViewSlotUi(RealCard card)
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
                IViewSlotHandler(stat[i], i, text);
            }




        }
        public static void IViewSlotClear(RealCard card)
        {
            card.Ui.text = "";
        }
    }
}
