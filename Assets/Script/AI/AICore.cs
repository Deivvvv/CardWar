using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BattleTable;

namespace AICore
{
    //public class AIBase
    //{
    //    private static Hiro my;
    //    private static Hiro enemy;
    //    private static Stol stol;

    //    public static void LoadCore(Hiro hiro, Hiro hiro2, Stol stol1)
    //    {
    //        my = hiro;
    //        enemy = hiro2;
    //        stol = stol1;
    //    }


    //    public static void AITurn( bool shotTime)
    //    {
    //        if (!shotTime)
    //            if (my.ManaCurent > 0)
    //            {
    //                List<int> newHand = new List<int>();
    //                int a = my.CardHand.Count;
    //                for (int i = 0; i < a; i++)
    //                {
    //                    newHand.Add(my.CardHand[i]);
    //                }

    //                PalyCard(my.ManaCurent, newHand);
    //            }

    //        UseArmy(shotTime);

    //    }
    //    static void UseArmy(bool shotTime)
    //    {
    //        List<RealCard> cardList = new List<RealCard>();
    //        int a = my.Army.Count;
    //        for (int i = 0; i < a; i++)
    //        {
    //            cardList.Add(my.Army[i]);
    //        }

    //        RealCard card = null;
    //        for (int i = 0; i < a; i++)
    //        {
    //            card = cardList[i];
    //            if(card.MovePoint>0)
    //                if (shotTime)
    //                {
    //                    if (card.ShotAction.Count > 0)
    //                        SelectAction(card.ShotAction, i, shotTime);
    //                }
    //                else
    //                {
    //                    if (card.Action.Count > 0)
    //                        SelectAction(card.Action, i, shotTime);
    //                }
    //        }
    //    }
    //    static void SelectAction(List<int> action, int a, bool shotTime)
    //    {
    //        int newAction = action[0];
    //        SelectTarget(newAction, a, shotTime);
    //      //  card.
    //    }

    //    static void SelectTarget( int action, int b, bool shotTime)
    //    {
    //        int line = 0;
    //        int slot = 0;
    //        int position = 0;
    //        // enemy
    //        int a = enemy.Army.Count;
    //        RealCard card = null;
    //        for (int i = 0; i < a; i++)
    //        {
    //            card = enemy.Army[i];
    //            line = card.Line;
    //            slot = card.Slot;
    //            position = card.Position;
    //            if (shotTime)
    //            {
    //                if (card.Position == 0)
    //                {
    //                    stol.AIUseArmy(line, slot, position, b, action);
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                stol.AIUseArmy(line, slot, position, b, action);
    //                return;
    //            }
    //        }

    //        if (a == 0)
    //            stol.AIClickHead(0, b, action);
    //    }

    //    static void PalyCard( int m, List<int> newHand)
    //    {
    //        int a = newHand.Count;

    //        //  int m = my.ManaCurent;
    //        int s1 = -1;
    //        int s2 = 0;
    //        int s3 = 0;
    //        for (int i = 0; i < a; i++)
    //        {
    //        //Allert    s3 = my.CardColod[my.CardHand[i]].Stat[12];
    //            if (s3 <= m)
    //                if (s3 > s2)
    //                {
    //                    s1 = i;
    //                    s2 = s3;
    //                }
    //        }

    //        if (s1 != -1)
    //        {
    //            m -= s3;
    //            if (m > 0)
    //            {
    //                newHand.RemoveAt(s1);
    //                PalyCard(m, newHand);
    //            }
    //            ITableHandler( my.CardHand[s1]);

    //        }
    //    }

    //    static void ITableHandler( int b)
    //    {
    //        int a = 7;
    //        for(int i = 0; i < a; i++)
    //        {
    //            if(my.Slots[i].Position[0] == null)
    //            {
    //                stol.AIUseCard(1,i,0,b);
    //                return;
    //            }
    //        }
    //    }
    //}
}
