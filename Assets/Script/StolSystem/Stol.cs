using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BattleTable;
using Saver;

public class Stol : MonoBehaviour
{
    // private List<CardBase> enemyCard;
    [SerializeField]
    private GameSetting gameSetting;
  //  private GameData gameData;

    [SerializeField]
    private CardSet myCardSet;
    [SerializeField]
    private CardSet enemyCardSet;

    private Hiro[] hiro = new Hiro[2];

   public List<CardBase> BufferColod;

    // void LoadSlotButton
    void AddManaCanal(Hiro hiro)
    {
        if(hiro.Mana < hiro.ManaMax)
            hiro.Mana++;

        hiro.ManaCurent = hiro.Mana;
    }

    void LoadSet(Hiro hiro, CardSet cardSet)
    {
        hiro.CardColod = new List<CardBase>();
        BufferColod = new List<CardBase>();
        int a = cardSet.AllCard;
        int b = 0;
        Stol stol = gameObject.GetComponent<Stol>();
        for (int i =0; i < a; i++)
        {
            hiro.CardColod.Add(null);
            b = cardSet.OrigCount[i];
            for (int i1 = 0; i1 < b; i1++)
                XMLSaver.ILoad(gameSetting.origPath + $"{cardSet.OrigCard[i]}", stol);
        }

        List<CardBase> newColod = new List<CardBase>();
        int r = 0;
        for (int i = 0; i < a; i++)
        {
            r = Random.Range(0,a);
            a--;
            hiro.CardColod[i] = BufferColod[r];
            BufferColod.RemoveAt(r);
        }

        a = cardSet.AllCard;
        for (int i = 0; i < a; i++)
        {
            Debug.Log(hiro.CardColod[i]);
        }
    }

    void CreateHiro(bool enemy)
    {
        Hiro newHiro = new Hiro();

        AddManaCanal(newHiro);
        LoadSet(newHiro, myCardSet);
       // newHiro

        if (enemy) 
        {
            hiro[1] = newHiro; 
        }
        else
            hiro[0] = newHiro;

    }
    // Start is called before the first frame update
    void Start()
    {
        CreateHiro(false);

    }
    void PreLoad()
    {
      //  gameData = gameSetting.GlobalMyData;
       // LoadData();

        //CreateHiro(0);
        //CreateHiro(1);
    }

    void MeleeTurn()
    {

    }
    void ShotTurn()
    {

    }
    void NextCard()
    {

    }

}
