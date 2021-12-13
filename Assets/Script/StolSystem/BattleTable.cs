using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace BattleTable 
{
    public static class BattleSystem 
    {

        public static void IHit(RealCard card1, RealCard card2)
        {

        }

        public static void IShot(RealCard card1, RealCard card2)
        {

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
            card.Id = handNum;//hiro1.CardColod[handNum];


            hiro1.Army.Add(card);
            hiro2.Slots[slot].Position[pos] = card;

            // CreateCard.Create(card, slot, pos, gameSetting);

            GameObject GO = Instantiate(gameSetting.OrigHiro);

            card.Body = GO;
            if (card.Team == 0)
                GO.transform.position = new Vector3(slot * 3, 0, -(pos * 4) + 5);
            else
                GO.transform.position = new Vector3(slot * 3, 0, pos * 4 + 1 + 10);

            //  cardBase.Body.active = false;

            Destroy(cardBase.Body.gameObject);
            hiro1.CardHand.Remove(handNum);

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

        static void SlotView(RealCard newPosition, MeshRenderer mesh, string mood, int b, GameSetting gameSetting, int team, RealCard curentCard)
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

        public static void LoadUiView(Hiro newHiro, string mood, GameSetting gameSetting, RealCard curentCard , Transform[] slots)
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
                SlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);

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
                SlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);
                //if (newPosition != null)
                //{
                //    mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                //    SlotView(newPosition, mesh, mood, b, gameSetting, team, curentCard);
                //}
            }
        }

    }
}
