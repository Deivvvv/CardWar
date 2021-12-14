using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace BattleTable 
{
    class StatusSystem : MonoBehaviour
    {
        public static void IDie(RealCard card)
        {
            Destroy(card.Body);
            card.HiroMain.Army.Remove(card);
            if (card.ShotDMG > 0)
                card.HiroMain.ShotHiro--;

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
        static void IAddEffect(string name, int size, RealCard card)
        {
            int a = card.Effect.Count;
            Effect effect = null;
            for(int i = 0; i < a; i++)
            {
                effect = card.Effect[i];
                if(effect.Name == name)
                {
                    effect.Size += size;
                    if (effect.Size <= 0)
                        card.Effect.RemoveAt(i);
                    CardView.IViewEffect(card);
                    return;
                }
            }

            effect = new Effect();
            effect.Name = name;
            effect.Size = size;
            card.Effect.Add(effect);
        }

        static void ISlash(RealCard card1, RealCard card2)
        {
            card1.MovePoint--;
            card2.MovePoint--;

            card1.Hp -= card2.MeleeDMG;
            card2.Hp -= card1.MeleeDMG;

            if (card1.Hp <= 0)
                StatusSystem.IDie(card1);
            if (card2.Hp <= 0)
                StatusSystem.IDie(card2);
        }

        static void IShot(RealCard card1, RealCard card2)
        {
            card1.MovePoint--;
            card2.Hp -= card1.ShotDMG;

            if (card1.Hp <= 0)
                StatusSystem.IDie(card1);
            if (card2.Hp <= 0)
                StatusSystem.IDie(card2);
        }

        public static void IUseAction(string actionTayp, RealCard card1, RealCard card2)
        {
            switch (actionTayp)
            {
                case ("Slash"):
                    if (card1.Team != card2.Team)
                        if (card2.Position != 1)
                            ISlash(card1, card2);
                        else 
                            return;
                    else
                        return;
                    break;
                case ("Hit"):
                    if (card1.Team != card2.Team)
                        if (card1.ShotDMG > 0)
                            IShot(card1, card2);
                        else
                            return;
                    else
                        return;
                    break;
                default:
                    Debug.Log(actionTayp);
                    break;
            }
        }
    }

    public class TableRule : MonoBehaviour
    {
        static void ICreateCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos, GameSetting gameSetting)
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


            card.Slot = slot;
            card.Position = pos;
            card.Team = hiro1.Team;
            card.Id = handNum;
            card.HiroMain = hiro1;


            hiro1.Army.Add(card);
            hiro2.Slots[slot].Position[pos] = card;
            if (card.ShotDMG > 0)
                card.HiroMain.ShotHiro++;

            GameObject GO = Instantiate(gameSetting.OrigHiro);

            card.Body = GO;
            if (card.Team == 0)
                GO.transform.position = new Vector3(slot * 3, 0, -(pos * 4) + 5);
            else
                GO.transform.position = new Vector3(slot * 3, 0, pos * 4 + 1 + 10);

            //  cardBase.Body.active = false;

            Destroy(cardBase.Body.gameObject);
            hiro1.CardHand.Remove(handNum);

            //прогрузка спосбностей
            ILoadAction(card, gameSetting);
        }

        static void ILoadAction(RealCard card, GameSetting gameSetting)
        {
            card.Action = new List<int>();
            card.ShotAction = new List<int>();
            card.PasiveAction = new List<int>();

            //if (card.MeleeDMG > 0)
            //    card.Action.Add(gameSetting.Library.Action.IndexOf("Slash"));
        }

        static void IPlayCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos, GameSetting gameSetting)
        {
            RealCard targetCard = hiro1.Slots[slot].Position[pos];
            if (targetCard == null)
            {
                //  ICreateCard(useCard, slot, pos, hiro[curentPlayer], hiro[line]);
                ICreateCard(hiro1, hiro2, handNum, slot, pos, gameSetting);
            }
        }//порцелура инициализации логики карты после предварительной расшифровки

        public static void IUseCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos, GameSetting gameSetting)
        {
            int b = hiro1.CardColod[handNum].Stat[hiro1.CardColod[handNum].Stat.Length - 1];
            if (hiro1.ManaCurent >= b)
            {
                RealCard targetCard = hiro1.Slots[slot].Position[pos];
                if (targetCard == null)
                {
                    hiro1.ManaCurent -= b;
                    //  ICreateCard(useCard, slot, pos, hiro[curentPlayer], hiro[line]);
                    IPlayCard(hiro1, hiro2, handNum, slot, pos, gameSetting);
                }
            }
         

        }//процедура определения задачи карты и возможности ее реализации
    }

    public static class CardView
    {
        public static void IViewCard(CardBase card, GameSetting gameSetting)
        {
           // Transform trans = card.Body;
            CardBaseUi Ui = card.Body.gameObject.GetComponent<CardBaseUi>();

            // Ui.Avatar.sprite = ?;//портреты
            Ui.Name.text = card.Name;

            Ui.Stat.text = "";
            for (int i = 0; i < card.Stat.Length - 1; i++)
            {
                if (card.Stat[i] > 0)
                    Ui.Stat.text += $"<sprite name={gameSetting.NameIcon[i]}>{card.Stat[i]} ";
            }

            //  Ui.Trait;

            Ui.Mana.text = "" + card.Stat[card.Stat.Length - 1];

            //if(Ui.Count != null)
            //        Ui.Count.text;
        }

        static void ISlotView(RealCard newPosition, MeshRenderer mesh, string mood, int b, GameSetting gameSetting, int team, RealCard curentCard)
        {

            mesh.material = gameSetting.TargetColor[0];
            //int a - используется для указания на использование первой или второй линин позиции
            if(newPosition != null)
                switch (mood)
                {
                    case ("ShotView"):
                        if (newPosition.Team == team)
                            if (newPosition.MovePoint > 0)
                                if (newPosition.ShotAction.Count > 0)
                                {
                                    mesh.material = gameSetting.TargetColor[1];
                                }
                        break;

                    case ("ShotTarget"):
                        if (newPosition.Team != team)
                            if (newPosition.MovePoint > 0)
                                if (newPosition.ShotAction.Count > 0)
                                {
                                    mesh.material = gameSetting.TargetColor[2];
                                }
                        if (newPosition == curentCard)
                            mesh.material = gameSetting.TargetColor[3];

                        break;

                    case ("MeleeView"):
                        if (newPosition.Team == team)
                            if (newPosition.MovePoint > 0)
                                if (newPosition.Action.Count > 0)
                                {
                                    mesh.material = gameSetting.TargetColor[1];
                                }
                        break;

                    case ("MeleeTarget"):
                        if (newPosition.Team != team)
                            if (newPosition.MovePoint > 0)
                                if (newPosition.ShotAction.Count > 0)
                                {
                                    mesh.material = gameSetting.TargetColor[2];
                                }
                        if (newPosition == curentCard)
                            mesh.material = gameSetting.TargetColor[3];

                        break;

                    default:
                        break;
                }
            else
                switch (mood)
                {
                    case ("SetCard"):
                        if(b == 0)
                        {
                            mesh.material = gameSetting.TargetColor[1];
                        }
                        break;
                    default:
                        break;
                }
        }

        public static void ILoadUiView(Hiro newHiro, string mood, GameSetting gameSetting, RealCard curentCard , Transform[] slots)
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

            RealCard newPosition = null;
            MeshRenderer mesh = null;
            int a = slots.Length;
            int b = 0;
            for (int i = 0; i < a; i++)
            {
                b = 0;
                newPosition = hiroSlot[i].Position[b];

                mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                ISlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);

                //if (newPosition != null)
                //{
                //    mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                //    SlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);
                //}
                //else if (mood == "SetCard")
                //{
                //    mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                //    SlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);
                //}


                b++;
                newPosition = hiroSlot[i].Position[b];

                mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                ISlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);
                //if (newPosition != null)
                //{
                //    mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                //    SlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);
                //}
            }
        }

        public static void IViewEffect(RealCard card)
        {

        }
    }
}
