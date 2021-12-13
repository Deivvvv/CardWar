using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static class TableRule
    {
        static void ICreateCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        {
            RealCard card = new RealCard();
            CardBase cardBase = hiro1.CardColod[hiro1.CardHand[handNum]];

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
            card.Id = hiro1.CardHand[handNum];


            hiro1.Army.Add(card);
            hiro2.Slots[slot].Position[pos] = card;
        }
        static void IPlayCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        {
            RealCard targetCard = hiro1.Slots[slot].Position[pos];
            if (targetCard == null)
            {
                //  ICreateCard(useCard, slot, pos, hiro[curentPlayer], hiro[line]);
                ICreateCard(hiro1, hiro2, handNum, slot, pos);
            }
        }//порцелура инициализации логики карты после предварительной расшифровки

        public static void IUseCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
        {
            int b = hiro1.CardColod[handNum].Stat[hiro1.CardColod[handNum].Stat.Length - 1];
            if (hiro1.ManaCurent >= b)
            {
                RealCard targetCard = hiro1.Slots[slot].Position[pos];
                if (targetCard == null)
                {
                    hiro1.ManaCurent -= b;
                    //  ICreateCard(useCard, slot, pos, hiro[curentPlayer], hiro[line]);
                    IPlayCard(hiro1, hiro2, handNum, slot, pos);
                }
            }
         

        }//процедура определения задачи карты и возможности ее реализации
    }
}
