using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BattleTable;
using Saver;

public class Stol : MonoBehaviour
{
    [SerializeField]
    private StolUi Ui;
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

    private RealCard curentCard;
    private int action;

    private int curentPlayer;
    private bool shotTime;
    // void LoadSlotButton
    void AddManaCanal(Hiro hiro)
    {
        if(hiro.Mana < hiro.ManaMax)
            hiro.Mana++;

        hiro.ManaCurent = hiro.Mana;
    }
    void AddCardInHand(Hiro hiro, int a)
    {
        int b = hiro.CardColod.Count;
        for (int i = 0; i < a; i++)
        {
            if (hiro.NextCard < b) 
            {
                hiro.CardHand.Add(hiro.NextCard);
                hiro.NextCard++; 
            }
        }

    }

    void LoadSet(Hiro hiro, CardSet cardSet)
    {
        hiro.CardColod = new List<CardBase>();
        BufferColod = new List<CardBase>();
        int a = cardSet.OrigCard.Count;
        int b = 0;
        Stol stol = gameObject.GetComponent<Stol>();
        for (int i =0; i < a; i++)
        {
            b = cardSet.OrigCount[i];
            for (int i1 = 0; i1 < b; i1++)
                XMLSaver.ILoad(Application.dataPath + gameSetting.origPath + $"{cardSet.OrigCard[i]}", stol);
        }

        a = cardSet.AllCard;
        int r = 0;
        for (int i = a; i > 0; i--)
        {
            r = Random.Range(0,i);
            hiro.CardColod.Add(BufferColod[r]);
            BufferColod.RemoveAt(r);
        }

        hiro.CardHand = new List<int>();
        AddCardInHand(hiro, 5);

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
        CreateHiro(true);

        int r = Random.Range(0, 2);
        if (r == 0)
            curentPlayer = 0;
        else
            curentPlayer = 1;
    }

    //void Update()
    //{

    //}
    void CreateUi(int slot, int pos, int line)
    {
        RealCard newHiro = hiro[line].Slots[slot].Position[pos];
        int a = newHiro.Action.Count;
        Transform trans = null;
        if (pos == 0)
        {
            if (line == 0)
                trans = Ui.MySlotUi[slot];
            else
                trans = Ui.EnemySlotUi[slot];
        }
        else
        {
            if (line == 0)
                trans = Ui.MySlotUiArergard[slot];
            else
                trans = Ui.EnemySlotUiArergard[slot];
        }


        GameObject GO = Instantiate(Ui.OrigCase);
        GO.transform.SetParent(trans);
        Transform trans1 = GO.transform;

        int b = 0;
        for (int i = 0; i < a; i++)
        {
            b = newHiro.Action[i];
            GO = Instantiate(Ui.OrigAction);
            GO.transform.SetParent(trans1);
            //if (gameSetting.ActionLibrary[b].Tayp == "Melee")
            //{
            //    GO.transform.SetParent(trans1);
            //}
            //else if (gameSetting.ActionLibrary[b].Tayp == "Shot")
            //{

            //}
            //  GO.GetComponent<Image>().sprite = gameSetting.ActionLibrary[b].Icon;
            GO.GetComponent<Button>().onClick.AddListener(() => PreUseAction(b, slot, pos, line));
        }
    }

    void PreUseAction(int newAction, int slot, int pos, int line)
    {
        //a - номер скила в формате всех возможностей карты
        //b номер слота
        //с номер позиции
        curentCard = hiro[line].Slots[slot].Position[pos];
        action = newAction;
        //подгрузка UI
    }

    void UseAction(int action, int slot, int pos, int line)
    {
        //a - номер скила в формате всех возможностей карты
        //b номер слота
        //с номер позиции


    }

    void PlayCard(int handNum, int slot, int pos, Hiro hiro1, Hiro hiro2)
    {
        int b = hiro1.CardColod[handNum].Stat[hiro1.CardColod[handNum].Stat.Length-1];
        if (hiro1.ManaCurent >= b)
        {
            hiro1.ManaCurent -= b;
            BattleSystem.IPlayCard(hiro1, hiro2, handNum, slot, pos);
        }
    }

    void NewTurn()
    {
        if (shotTime)
            ShotTurn();
        else
            MeleeTurn();
    }
    void MeleeTurn()
    {
        AddManaCanal(hiro[curentPlayer]);
        shotTime = true;
        //LoadUIMelee
    }
    void ShotTurn()
    {

        if (curentPlayer == 1)
            curentPlayer = 0;
        else
            curentPlayer = 1;

        shotTime = false;
        //LoadUIShot

    }

}
