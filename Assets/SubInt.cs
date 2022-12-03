using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubInt
{
    private char[] keys = { '/', ':', '?', '|', ',' };
    public int Head;
    public List<SubInt> Num = new List<SubInt>();
    public SubInt(int a)
    {
        Head = a;
    }

    public SubInt(string str, int key)
    {
        key--;
        if (key < 0)
            return;

        string[] mood = str.Split(keys[key]);
        if (mood[0] == " ")
            return;

        // Num = new List<SubInt>();
        //Debug.Log(mood[0]);
        Head = int.Parse(mood[0]);

        for (int i = 1; i < mood.Length; i++)
        {
            SubInt sub = new SubInt(mood[i], key);
            if(sub.Num != null)
                Num.Add(sub);

        }

    }
    public void Edit(int a , bool add)
    {
        if (add)
            Find(a);
        else
        {
            a = Find(a, false);
            if (a != -1)
                Num.RemoveAt(a);
        }
    }

    public int Find(int a, bool add = true)
    {
        int i = Num.FindIndex(x => x.Head == a);
        if(add)
            if (i == -1)
            {
                Num.Add(new SubInt(a));
                i = Num.Count - 1;
            }
        return i;
    }
    public void Sort()
    {
        if (Num.Count > 1)
        {
            Num.OrderBy(x => x.Head);
            for (int i = 0; i < Num.Count; i++)
                Num.Sort();

        }
    }
    public string Zip(int key)
    {
        string str = $"{Head}";
        if (Num.Count > 0)
        {
            str +=$"{ keys[key]}";
            str += $"{Num[0].Zip(key-1)}";
            for(int i=1;i<Num.Count;i++)
                str += $"{keys[key]}{Num[i].Zip(key - 1)}";
        }
       // else
           // str += " ";

        return str;
    }

}

