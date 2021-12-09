using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleTable
{
 
    public static class BattleSystem 
    {
       public static void IPlayCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos)
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

        public static void IHit(RealCard card1, RealCard card2)
        {

        }

        public static void IShot(RealCard card1, RealCard card2)
        {

        }
    }
}
