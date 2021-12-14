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
                Debug.Log(hiro.ManaCurent);
                if (hiro.ManaCurent > 0)
                    IPalyCard(hiro, hiro.ManaCurent, stol);
            }
        }

        static void IPalyCard(Hiro hiro, int m, Stol stol)
        {
            int a = hiro.CardHand.Count;

            Debug.Log(m);
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

            if (s1 != -1)
            {
                m -= s3;
                if (m > 0)
                    IPalyCard(hiro, m, stol);

                ITableHandler(hiro, hiro.CardHand[s1], stol);
            }
        }

        static void ITableHandler(Hiro hiro, int b, Stol stol)
        {
            Debug.Log(b);
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
