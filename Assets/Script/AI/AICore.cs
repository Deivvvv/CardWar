using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BattleTable;

namespace AICore
{
    public class AIBase
    {
        public static void AITurn(Hiro hiro, Stol stol, bool shotTime)
        {
            if (shotTime)
            {

            }
            else 
            {
                if (hiro.ManaCurent > 0)
                {
                    List<int> newHand = new List<int>();
                    int a = hiro.CardHand.Count;
                    for (int i = 0; i < a; i++)
                    {
                        newHand.Add(hiro.CardHand[i]);
                    }

                    IPalyCard(hiro, hiro.ManaCurent, stol, newHand);
                }
            }
        }

        static void IPalyCard(Hiro hiro, int m, Stol stol, List<int> newHand)
        {
            int a = newHand.Count;

            //  int m = hiro.ManaCurent;
            int s1 = -1;
            int s2 = 0;
            int s3 = 0;
            for (int i = 0; i < a; i++)
            {
                s3 = hiro.CardColod[hiro.CardHand[i]].Stat[12];
                if (s3 <= m)
                    if (s3 > s2)
                    {
                        s1 = i;
                        s2 = s3;
                    }
            }

            Debug.Log(a);
            if (s1 != -1)
            {
                m -= s3;
                if (m > 0)
                {
                    newHand.RemoveAt(s1);
                    IPalyCard(hiro, m, stol, newHand);
                }
                ITableHandler(hiro, hiro.CardHand[s1], stol);

            }
        }

        static void ITableHandler(Hiro hiro, int b, Stol stol)
        {
            int a = 7;
            for(int i = 0; i < a; i++)
            {
                if(hiro.Slots[i].Position[0] == null)
                {
                    stol.AIUseCard(1,i,0,b);
                    return;
                }
            }
        }
    }
}
