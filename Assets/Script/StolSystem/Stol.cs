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
    private Stol stol;

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

    void Start()
    {
        stol = gameObject.GetComponent<Stol>();
        Core.ILoadGameSetting(gameSetting);
        Core.ISetStol(stol);

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

        AIBase.LoadCore(hiro[1], hiro[0], stol);
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
                   // CardView.IViewCard(newHiro.CardColod[a]); 
                }
            }
            else
                Ui.TargetCard.gameObject.active = false;
        }

        if (curentPlayer == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
                if (targetHiro != null)
                {
                    if (useCard == -1)
                    {
                        if (curentCard == null)
                            targetHiro.Play();
                        else if (action != -1)
                            targetHiro.Target();
                    }
                    else
                    {
                        targetHiro.CardLoad();
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                PostUse(false);
            }
        }
    }




    #region SubMetod

    void CardNull()
    {
        curentCard = null;
        Ui.UseCard.gameObject.active = false;
    }

    #endregion

    #region Public Metods
    public void PostUse(bool fullUse)
    {
        if (!fullUse)
        {
            if (useCard != -1)
            {
                useCard = -1; 
                CardNull();
            }
            else if (action != -1)
            {
                if (shotTime)
                {
                    if (curentCard.ShotAction.Count < 2)
                        CardNull();
                    action = -1;
                }
                else
                {
                    if (curentCard.Action.Count < 2)
                        CardNull();
                    action = -1;
                }
            }
            else if (curentCard != null)
            {
                CardNull();
            }
        }
        else
        {
            HiroUi(hiro[0]);
            HiroUi(hiro[1]);
            CardNull();
            useCard = -1;
            action = -1;
        }

        if (shotTime)
        {
            CallTable("ShotView");
        }
        else
        {
            CallTable("MeleeView");
        }
    }

    public void PreView(int line, int slot, int pos)
    {
        RealCard cardReal = hiro[line].Slots[slot].Position[pos];

        if (cardReal != null)
        {
            CardBase cardBase = hiro[line].CardColod[cardReal.Id];


            CardView.IViewTargetCard(cardBase, Ui.TargetCard);
        }
        else
            Ui.TargetCard.gameObject.active = false;
    }

    public void UseCard(int line, int slot, int pos)
    {
        Hiro newHiro = hiro[curentPlayer];
        TableRule.IUseCard(newHiro, hiro[line], useCard, slot, pos);
        // PostUse(true);

      //  HiroUi(newHiro);
    }

    public void SelectTarget(int line, int slot, int position)
    {
        bool load = true;
        targetCard = hiro[line].Slots[slot].Position[position];

        //  curentCard.MovePoint -= gameSetting.Library.Action[action].MoveCost;

        BattleSystem.IUseAction(actionTayp, curentCard, targetCard);
    }
    public void RandomClickHiro(int line)
    {
        Hiro hiroTarget = hiro[line];
        int a = hiroTarget.Slots.Length;
        for(int i = 0; i < a; i++)
        {
            if(hiroTarget.Slots[i].Position[1]== null)
                if (hiroTarget.Slots[i].Position[0] == null)
                {
                    ClickHiro(line, i, 0);
                    return;
                }
        }
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
                    Ui.UseCard.gameObject.active = true;
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

    public void ClickHead(int a)
    {
        if(useCard != -1)
        {

        }
        else if (curentCard != null)
            if (action != -1)
                BattleSystem.IUseActionHead(actionTayp, curentCard, hiro[a]);

    }

    #endregion

    #region UI Support
    public void SelectCardSpellTarget(int action, RealCard card)
    {
        curentCard = card;
        SelectAction(action);
        //if (shot)
        //{

        //}
        //else
        //{
        //    SelectAction(action);
        //}
    }
    #endregion

    #region ALT Public Metods

    #endregion

    #region AI Rule
    public void AIUseCard(int line, int slot, int position, int card)
    {
        useCard = card;
        UseCard(line, slot, position);
    }
    public void AIUseArmy(int line, int slot, int position, int card, int newAction)
    {
        curentCard = hiro[1].Army[card];
        SelectAction(newAction);
        SelectTarget(line, slot, position);
    }
    public void AIClickHead(int a, int card, int newAction)
    {
        curentCard = hiro[1].Army[card];
        SelectAction(newAction);
        ClickHead(a);
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

        for (int i = 0; i < a; i++)
        {
            b = cardSet.OrigCount[i];
            for (int i1 = 0; i1 < b; i1++)
                XMLSaver.Load(Application.dataPath + gameSetting.origPath + $"{cardSet.OrigCard[i]}", stol);
        }

        //random generate
        a = cardSet.AllCard;
        int r = 0;

        CardBase card = null;

        List<int> fastCard = new List<int>();
        List<int> slowCard = new List<int>();
        //scan fast and slow card
        for (int i = a - 1; i > -1; i--)
        {
            card = BufferColod[i];
            if (card.Trait.Count > 0)
            {
                b = card.Trait.FindIndex(x => x == "Fast");
                if (b != -1)
                {
                    fastCard.Add(i);
                    BufferColod.RemoveAt(i);
                }
                else
                {
                    b = card.Trait.FindIndex(x => x == "Slow");
                    if (b != -1)
                    {
                        slowCard.Add(i);
                        BufferColod.RemoveAt(i);
                    }
                }
            }
        }
        //pre generation
        a = fastCard.Count;
        if (a > 0)
        {
            if (a > 5)
            {
                for (int i = a; i > 0; i--)
                {
                    r = Random.Range(0, i);
                    b = fastCard[r];
                    hiro.CardColod.Add(BufferColod[b]);
                    BufferColod.RemoveAt(b);
                    fastCard.RemoveAt(r);
                }
            }
            else
            {
                for (int i = 0; i < a; i++)
                {
                    b = fastCard[i];
                    hiro.CardColod.Add(BufferColod[b]);
                    BufferColod.RemoveAt(b);
                }
            }
        }
        //post generation
        a = slowCard.Count;
        if (a > 0)
        {
            if (hiro.CardColod.Count < 7)
            {
                b = 7 - a;
                for (int i = b; i > -1; i--)
                {
                    r = Random.Range(0, i);
                    hiro.CardColod.Add(BufferColod[r]);
                    BufferColod.RemoveAt(r);
                }
            }


            for (int i = a; i > 0; i--)
            {
                r = Random.Range(0, i);
                b = slowCard[r];
                hiro.CardColod.Add(BufferColod[b]);
                BufferColod.RemoveAt(b);
                slowCard.RemoveAt(r);
            }
        }

        a = BufferColod.Count;
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

        CreateSlots(enemy, newHiro);

        Slot newSlot = null;
        Transform[] newSlotTrans = null;
        if (enemy)
        {
            newSlotTrans = Ui.EnemySlot;
            newHiro.Team = 1;
            hiro[1] = newHiro;
        }
        else
        {
            newSlotTrans = Ui.MySlot;
            newHiro.Team = 0;
            hiro[0] = newHiro;
        }
        newHiro.OrigSlots = newSlotTrans;


        int a = newHiro.Slots.Length;
        for (int i = 0; i < a; i++)
        {
            newSlot = new Slot();
            newSlot.Position = new RealCard[2];
            newSlot.Mesh = new MeshRenderer[2];
            if (enemy)
            {
                newSlot.Mesh[0] = newSlotTrans[i].GetChild(0).gameObject.GetComponent<MeshRenderer>();
                newSlot.Mesh[1] = newSlotTrans[i].GetChild(1).gameObject.GetComponent<MeshRenderer>();
            }
            else
            {
                newSlot.Mesh[0] = newSlotTrans[i].GetChild(0).gameObject.GetComponent<MeshRenderer>();
                newSlot.Mesh[1] = newSlotTrans[i].GetChild(1).gameObject.GetComponent<MeshRenderer>();
            }
            newHiro.Slots[i] = newSlot;
        }
        newHiro.Army = new List<RealCard>();

       

       // AddManaCanal(newHiro);
        LoadSet(newHiro, myCardSet);

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

    public void HiroUi(Hiro newHiro)
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

        CardView.IViewCard(newHiro.CardColod[a]);

        if (b == 0)
            GO.GetComponent<Button>().onClick.AddListener(() => GrabCard(a));
        //GO.GetComponent<Button>();
       
    }
    #endregion


    #region Stol
    void CallTable(string mood)
    {
        CardView.IViewLoadUi(hiro[0], mood, curentCard, Ui.MySlot, curentPlayer);
        CardView.IViewLoadUi(hiro[1], mood, curentCard, Ui.EnemySlot, curentPlayer);
    }

    void GrabCard(int a)
    {
        Hiro newHiro = hiro[0];
      //  a = newHiro.CardHand[a];
        CardBase cardBase = newHiro.CardColod[a];
        int b = cardBase.Stat.Count - 1;

        if (newHiro.ManaCurent >= cardBase.Stat[b])
        {
            useCard = a;

            CardView.IViewTargetCard(cardBase, Ui.UseCard);

            CallTable("SetCard");
        }
    }


    void SelectAction(int a)
    {
        action = a;

        actionTayp = gameSetting.Library.Action[action].Tayp;

        if (!IsAI)
        {
            if (actionTayp == "avtoActiv")
            {
                BattleSystem.IUseAction(actionTayp, curentCard, targetCard);
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


            CardView.IViewTargetCard(hiro[0].CardColod[curentCard.Id], Ui.UseCard);
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
                AIBase.AITurn(shotTime);
                NewTurn();
            }
            else
            {
                CallTable("MeleeView");
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
        Hiro targetHiro = hiro[curentPlayer];
        BattleSystem.NewTurn(targetHiro);

        shotTime = true;
        if (targetHiro.ShotHiro < 0)
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
                    AIBase.AITurn(shotTime);
                }
                else
                {
                    CallTable("ShotView");
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
