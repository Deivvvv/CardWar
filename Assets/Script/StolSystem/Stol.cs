using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using AICore;
using BattleTable;
using Saver;

public class Stol : MonoBehaviour
{
    public bool IsAI;
    [SerializeField]
    private Camera camera;

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

    private int useCard =-1;

    private int sizeSlot = 7;

    private RealCard targetCard;
    private RealCard curentCard;
    private int action =-1;

    private string actionTayp;
    private int curentPlayer;
    private bool shotTime;

    private GameObject selectable;
    // void LoadSlotButton
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
        curentPlayer = 1;
        NewTurn();
        Ui.NextTurn.onClick.AddListener(() => NewTurn());

        HiroUi(hiro[0]);
        HiroUi(hiro[1]);
    }


    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
            if (targetHiro != null)
            {
                if (selectable != hit.transform.gameObject)
                {
                    selectable = hit.transform.gameObject;
                    Ui.TargetCard.gameObject.active = true;
                    targetHiro.PreView();
                   // CardView.IViewCard(newHiro.CardColod[a], gameSetting); 
                }
            }
            else
                Ui.TargetCard.gameObject.active = false;
        }

        if (curentPlayer == 0)
        {

            //if (Input.GetMouseButtonUp(0))
            //{
            //    if (useCard != -1)
            //    {
            //        if (useCard == -1)
            //        {
            //            int layerMask = 1 << 8;

            //            // This would cast rays only against colliders in layer 8.
            //            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            //            layerMask = ~layerMask;
            //            RaycastHit hit;
            //            // Does the ray intersect any objects excluding the player layer
            //            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            //            {
            //                TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
            //                if (targetHiro != null) 
            //                { 
            //                    targetHiro.CardLoad();
            //                }
            //            }
            //        }
            //    }
            //}
            if (Input.GetMouseButtonDown(0))
            {
                // ray = camera.ScreenPointToRay(Input.mousePosition); 
                //// RaycastHit hit;
                // if (Physics.Raycast(ray, out hit, 100)) 
                // { 
                TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
                if (targetHiro != null)
                    {
                        if (useCard == -1)
                        {
                            Debug.Log("Ok");
                            if (curentCard == null)
                                targetHiro.Play();
                            else if (action != -1)
                                targetHiro.Target();
                        }
                        else
                        {
                            targetHiro.CardLoad();
                         //   IUseCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos, GameSetting gameSetting)
                         //IPlayCard(Hiro hiro1, Hiro hiro2, int handNum, int slot, int pos, GameSetting gameSetting)
                         //   IPlayCard();
                        }
                    }
               // }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (useCard != -1)
                {
                    Debug.Log("Ok");
                    useCard = -1;
                    Ui.UseCard.gameObject.active = false;

                    if (shotTime)
                    {
                        CallTable("ShotView");
                    }
                    else
                    {
                        CallTable("MeleeView");
                    }
                }
                else if (action != -1)
                {
                    if (shotTime)
                    {
                        if (curentCard.ShotAction.Count < 2)
                            curentCard = null;
                        action = -1;
                    }
                    else
                    {
                        if (curentCard.Action.Count < 2)
                            curentCard = null;
                        action = -1;
                    }
                }
                else if (curentCard != null)
                {
                    curentCard = null;
                }
            }
        }
    }





    #region Public Metods

    public void PreView(int line, int slot, int pos)
    {
        RealCard cardReal = hiro[line].Slots[slot].Position[pos];
        // int a = hiro[line].Slots[slot].Position[pos].Id;
        //   int team =
        if (cardReal != null)
        {
            CardBase cardBase = hiro[line].CardColod[cardReal.Id];
            Transform trans = cardBase.Body;
            cardBase.Body = Ui.TargetCard;
            CardView.IViewCard(cardBase, gameSetting);
            cardBase.Body = trans;
            //  CardView.IViewCard(hiro[line].CardColod[cardReal.Id], gameSetting);
        }
        else
            Ui.TargetCard.gameObject.active = false;
    }

    public void UseCard(int line, int slot, int pos)
    {
        Hiro newHiro = hiro[curentPlayer];
        TableRule.IUseCard(newHiro, hiro[line], useCard, slot, pos, gameSetting);
        HiroUi(newHiro);
        if (curentPlayer == 0)
        {
            CallTable("Clear");
            Ui.UseCard.gameObject.active = false;
        }
    }

    public void SelectTarget(int line, int slot, int position)
    {
        bool load = true;
        targetCard = hiro[line].Slots[slot].Position[position];

        //  curentCard.MovePoint -= gameSetting.Library.Action[action].MoveCost;

        BattleSystem.IUseAction(actionTayp, curentCard, targetCard, gameSetting);
    }

    public void ClickHiro(int line, int slot, int position)
    {
        bool load = false;
        curentCard = hiro[line].Slots[slot].Position[position];
        if(curentCard!= null)
            if (curentCard.Team == curentPlayer)
            {
                load = true;
                if (curentCard.MovePoint > 0)
                {
                    if (shotTime)
                    {
                        if (curentCard.ShotAction.Count > 1)
                        {
                            //SupportUi
                        }
                        else if (curentCard.ShotAction.Count > 0)
                        {
                            SelectAction(curentCard.ShotAction[0]);
                        }
                        else
                            load = false;

                    }
                    else
                    {
                        if (curentCard.Action.Count > 1)
                        {
                            //SupportUi
                        }
                        else if (curentCard.Action.Count > 0)
                        {
                            SelectAction(curentCard.Action[0]);
                        }
                        else
                            load = false;
                    }
                }
                else
                    load = false;
            }

        if (!load)
            curentCard = null;
    }
    #endregion

    #region AI Rule
    public void AIUseCard(int line, int slot, int position, int card)
    {
        useCard = card;
        UseCard(line, slot, position);
    }
    #endregion

    #region PreStart

    void CreateSlots(bool enemy, Hiro newHiro)
    {
        GameObject origSlot = null;
        int a = sizeSlot;
        int b = 3;
        if (enemy)
        {
            Ui.EnemySlot = new Transform[a];
            origSlot = Ui.OrigSlotEnemy;
        }
        else
        {
            Ui.MySlot = new Transform[a];
            origSlot = Ui.OrigSlot;
        }

        GameObject GO = null;
        Transform trans = null;
        Vector3 v = new Vector3(0, 0, 0);
        Stol stol = gameObject.GetComponent<Stol>();

        for (int i=0; i < a; i++)
        {
            GO = Instantiate(origSlot);
            trans = GO.transform;

            if (enemy)
            {
                Ui.EnemySlot[i] = trans;
                v = new Vector3(i * b, 0, 10);
                //   GO.transform.eulerAngles = new Vector3(0, 180, 0);
                trans.GetChild(0).gameObject.GetComponent<TargetHiro>().Set(1, i, 0, stol);
                trans.GetChild(1).gameObject.GetComponent<TargetHiro>().Set(1, i, 1, stol);
            }
            else
            {
                Ui.MySlot[i] = trans;
                v = new Vector3(i * b, 0, 0);

                trans.GetChild(0).gameObject.GetComponent<TargetHiro>().Set(0, i, 0, stol);
                trans.GetChild(1).gameObject.GetComponent<TargetHiro>().Set(0, i, 1, stol);
            }


            trans.position = v;
        }
    }

    void LoadSet(Hiro hiro, CardSet cardSet)
    {
        hiro.CardColod = new List<CardBase>();
        BufferColod = new List<CardBase>();
        int a = cardSet.OrigCard.Count;
        int b = 0;
        Stol stol = gameObject.GetComponent<Stol>();
        for (int i = 0; i < a; i++)
        {
            b = cardSet.OrigCount[i];
            for (int i1 = 0; i1 < b; i1++)
                XMLSaver.ILoad(Application.dataPath + gameSetting.origPath + $"{cardSet.OrigCard[i]}", stol);
        }

        a = cardSet.AllCard;
        int r = 0;
        for (int i = a; i > 0; i--)
        {
            r = Random.Range(0, i);
            hiro.CardColod.Add(BufferColod[r]);
            BufferColod.RemoveAt(r);
        }

        hiro.CardHand = new List<int>();
        AddCardInHand(hiro, 5);

    }

    void CreateHiro(bool enemy)
    {
        Hiro newHiro = new Hiro();
        newHiro.Slots = new Slot[sizeSlot];
        int a = newHiro.Slots.Length;
        for (int i = 0; i < a; i++)
        {
            newHiro.Slots[i] = new Slot();
            newHiro.Slots[i].Position = new RealCard[2];
        }
        newHiro.Army = new List<RealCard>();

        if (enemy)
        {
            newHiro.Team = 1;
            hiro[1] = newHiro;
        }
        else
        {
            newHiro.Team = 0;
            hiro[0] = newHiro;
        }

        AddManaCanal(newHiro);
        LoadSet(newHiro, myCardSet);

        CreateSlots(enemy, newHiro);
    }
    #endregion

    #region UiUse
    //void CreateUi(int slot, int pos, int line)
    //{
    //    RealCard newHiro = hiro[line].Slots[slot].Position[pos];
    //    int a = newHiro.Action.Count;
    //    Transform trans = null;
    //    if (pos == 0)
    //    {
    //        if (line == 0)
    //            trans = Ui.MySlotUi[slot];
    //        else
    //            trans = Ui.EnemySlotUi[slot];
    //    }
    //    else
    //    {
    //        if (line == 0)
    //            trans = Ui.MySlotUiArergard[slot];
    //        else
    //            trans = Ui.EnemySlotUiArergard[slot];
    //    }


    //    GameObject GO = Instantiate(Ui.OrigCase);
    //    GO.transform.SetParent(trans);
    //    Transform trans1 = GO.transform;

    //    int b = 0;
    //    for (int i = 0; i < a; i++)
    //    {
    //        b = newHiro.Action[i];
    //        GO = Instantiate(Ui.OrigAction);
    //        GO.transform.SetParent(trans1);
    //        //if (gameSetting.ActionLibrary[b].Tayp == "Melee")
    //        //{
    //        //    GO.transform.SetParent(trans1);
    //        //}
    //        //else if (gameSetting.ActionLibrary[b].Tayp == "Shot")
    //        //{

    //        //}
    //        //  GO.GetComponent<Image>().sprite = gameSetting.ActionLibrary[b].Icon;
    //        GO.GetComponent<Button>().onClick.AddListener(() => PreUseAction(b, slot, pos, line));
    //    }
    //}

    void HiroUi(Hiro newHiro)
    {
        int b = newHiro.Team;
        TMP_Text text = null;

        if (b == 0)
            text = Ui.MyInfo;
        else
            text = Ui.EnemyInfo;

        text.text = $"Hp: {newHiro.Hp}" +
                $"\nCard: {newHiro.CardColod.Count - newHiro.NextCard}" +
                $"\nMana: {newHiro.ManaCurent}/{newHiro.Mana}";
    }

    void NewUiCard(Hiro newHiro, int a)
    {
        GameObject GO = Instantiate(Ui.OrigCard);
        int b = newHiro.Team;
        if(b==0)
            GO.transform.SetParent(Ui.MyHand);
        else
            GO.transform.SetParent(Ui.EnemyHand);

        newHiro.CardColod[a].Body = GO.transform;

        CardView.IViewCard(newHiro.CardColod[a], gameSetting);

        if (b == 0)
            GO.GetComponent<Button>().onClick.AddListener(() => GrabCard(a));
        //GO.GetComponent<Button>();
       
    }
    #endregion


    #region Stol
    void CallTable(string mood)
    {
        CardView.ILoadUiView(hiro[0], mood, gameSetting, curentCard, Ui.MySlot);
        CardView.ILoadUiView(hiro[1], mood, gameSetting, curentCard, Ui.EnemySlot);

        //CardView.LoadUiView(hiro[0], mood, gameSetting, curentCard, Ui);
        //CardView.LoadUiView(hiro[1], mood, gameSetting, curentCard, Ui);
    }

    void GrabCard(int a)
    {
        Hiro newHiro = hiro[0];
      //  a = newHiro.CardHand[a];
        CardBase cardBase = newHiro.CardColod[a];
        int b = cardBase.Stat.Length - 1;

        if (newHiro.ManaCurent >= cardBase.Stat[b])
        {
            useCard = a;

            Ui.UseCard.gameObject.active = true;
            Transform trans = cardBase.Body;
            cardBase.Body = Ui.UseCard;
            CardView.IViewCard(cardBase, gameSetting);
            cardBase.Body = trans;

            CallTable("SetCard");
        }
    }

    void CardReset(Hiro newHiro)
    {
        int a = newHiro.Army.Count;
        for (int i = 0; i < a; i++)
            newHiro.Army[i].MovePoint = 1;
    }// Временное решение

    void SelectAction(int a)
    {
        action = a;

        actionTayp = gameSetting.Library.Action[action].Tayp;

        if (actionTayp == "avtoActiv")
        {
            BattleSystem.IUseAction(actionTayp, curentCard, targetCard, gameSetting);
            action = -1;
            curentCard = null;

        }
        else if (shotTime)
        {
            CallTable("ShotTarget");
        }
        else
        {
            CallTable("MeleeTarget");
        }
    }


    void NewTurn()
    {
        if (!shotTime)
            ShotTurn();
        else
            MeleeTurn();

    }
    void MeleeTurn()
    {
        AddManaCanal(hiro[curentPlayer]);
        AddCardInHand(hiro[curentPlayer], 1);
        shotTime = false;

        Ui.ShotTurn.active = false;
        Ui.MeleeTurn.active =  true;
        if (curentPlayer == 0)
        {
            CallTable("MeleeView");
        }
        else
        {
          //  CallTable("Clear");
            if (IsAI)
            {
                AIBase.AITurn(hiro[1], gameObject.GetComponent<Stol>(), shotTime);
                NewTurn();
            }
            else
            {
                CallTable("Clear");
            }
        }
        //LoadUIMelee
    }
    void ShotTurn()
    {
        if (curentPlayer == 1)
            curentPlayer = 0;
        else
        {
            curentPlayer = 1;
        }

        shotTime = true;
        if (hiro[curentPlayer].ShotHiro < 0)
        {
            Ui.ShotTurn.active = true;
            Ui.MeleeTurn.active = false;
            if (curentPlayer == 0)
            {
                CallTable("ShotView");
            }
            else
            {
                if (IsAI)
                {
                    AIBase.AITurn(hiro[1], gameObject.GetComponent<Stol>(), shotTime);
                }
               // CallTable("Clear");
            }
        }
        else
            NewTurn();


    }


    void AddManaCanal(Hiro hiro)
    {
        if (hiro.Mana < hiro.ManaMax)
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

                NewUiCard(hiro, hiro.NextCard);

                hiro.NextCard++;
                if (hiro.CardHand.Count > 10)
                {
                    Destroy(hiro.CardColod[hiro.CardHand[0]].Body.gameObject);
                    hiro.CardHand.RemoveAt(0);
                }

            }
        }
        HiroUi(hiro);

    }

    #endregion
}
