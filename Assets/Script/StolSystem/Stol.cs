using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

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

    private int useCard =-1;

    private int sizeSlot = 7;

    private RealCard curentCard;
    private int action =-1;

    private string actionTayp;
    private int curentPlayer;
    private bool shotTime;
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
        NewTurn();
    }

    void Update()
    {
        if (curentPlayer == 0)
        {

            if (Input.GetMouseButtonUp(0))
            {
                if (useCard != -1)
                {
                    if (useCard == -1)
                    {
                        int layerMask = 1 << 8;

                        // This would cast rays only against colliders in layer 8.
                        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                        layerMask = ~layerMask;
                        RaycastHit hit;
                        // Does the ray intersect any objects excluding the player layer
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                        {
                            TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
                            if (targetHiro != null) 
                            { 
                                targetHiro.CardLoad();
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (useCard == -1)
                {
                    int layerMask = 1 << 8;

                    // This would cast rays only against colliders in layer 8.
                    // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                    layerMask = ~layerMask;
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the player layer
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                    {
                        TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
                        if (targetHiro != null)
                        {
                            if (curentCard == null)
                                targetHiro.Play();
                            else if (action != -1)
                                targetHiro.Target();
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (useCard != -1)
                {
                    useCard = -1;
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

    public void UseCard(int line, int slot, int pos)
    {
        TableRule.IUseCard(hiro[curentPlayer], hiro[line], useCard, slot, pos);
    }

    public void SelectTarget(int line, int slot, int position)
    {
        bool load = true;
        RealCard targetCard = hiro[line].Slots[slot].Position[position];

        //action

        if (targetCard.Team == curentCard.Team)
        {

        }
    }

    public void ClickHiro(int line, int slot, int position)
    {
        bool load = true;
        curentCard = hiro[line].Slots[slot].Position[position];
        if (curentCard.Team == curentPlayer)
        {
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
        else
            load = false;

        if (!load)
            curentCard = null;
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
        //public List<Slot> Slots;
        //public List<RealCard> Army;


        //public List<CardBase> CardColod;
        //public List<int> CardHand;

        AddManaCanal(newHiro);
        LoadSet(newHiro, myCardSet);
        // newHiro

        if (enemy)
        {
            hiro[1] = newHiro;
        }
        else
            hiro[0] = newHiro;


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

    void SlotView(RealCard newPosition, MeshRenderer mesh, string mood, int a)
    {
        mesh.material = Ui.TargetColor[0];
        //int a - используется для указания на использование первой или второй линин позиции
        switch (mood)
        {
            case ("ShotView"):
                if (newPosition.Team == curentPlayer)
                    if (newPosition.MovePoint > 0)
                        if (newPosition.ShotAction.Count > 0)
                        {
                            mesh.material = Ui.TargetColor[1];
                        }
                break;

            case ("ShotTarget"):
                if (newPosition.Team != curentPlayer)
                    if (newPosition.MovePoint > 0)
                        if (newPosition.ShotAction.Count > 0)
                        {
                            mesh.material = Ui.TargetColor[2];
                        }
                if (newPosition == curentCard)
                    mesh.material = Ui.TargetColor[3];

                break;

            case ("MeleeView"):
                if (newPosition.Team == curentPlayer)
                    if (newPosition.MovePoint > 0)
                        if (newPosition.Action.Count > 0)
                        {
                            mesh.material = Ui.TargetColor[1];
                        }
                break;

            case ("MeleeTarget"):
                if (newPosition.Team != curentPlayer)
                    if (newPosition.MovePoint > 0)
                        if (newPosition.ShotAction.Count > 0)
                        {
                            mesh.material = Ui.TargetColor[2];
                        }
                if (newPosition == curentCard)
                    mesh.material = Ui.TargetColor[3];

                break;
        }
    }

    void LoadUiView(int line, string mood)
    {// ViewArmy
        Transform[] slots = null;
        Slot[] hiroSlot = hiro[line].Slots;
        // Slot newSlot = null;

        if (line == 0)
            slots = Ui.MySlot;
        else
            slots = Ui.EnemySlot;

        RealCard newPosition = null;
        MeshRenderer mesh = null;
        int a = slots.Length;
        int b = 0;
        for (int i = 0; i < a; i++)
        {
            b = 0;
            newPosition = hiroSlot[i].Position[b];
            if (newPosition != null)
            {
                mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                SlotView(newPosition, mesh, mood, b);
            }
            //else

            b++;
            newPosition = hiroSlot[i].Position[b];
            if (newPosition != null)
            {
                mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                SlotView(newPosition, mesh, mood, b);
            }

        }
    }

    void HiroUi()
    {

    }
    #endregion


    #region Stol

    void GrabCard(int a)
    {
        Hiro newHiro = hiro[0];
        a = newHiro.CardHand[a];
        CardBase cardBase = newHiro.CardColod[a];
        int b = cardBase.Stat.Length - 1;

        if (newHiro.ManaCurent >= cardBase.Stat[b])
        {
            useCard = a;
        }
    }


    void CardReset(Hiro newHiro)
    {
        int a = newHiro.Army.Count;
        for (int i = 0; i < a; i++)
            newHiro.Army[i].MovePoint = 1;
    }// Временное решение


    void UseAction()
    {
        curentCard.MovePoint -= gameSetting.Library.Action[action].MoveCost;
        switch (actionTayp)
        {
            case ("Slash"):
                break;
            case ("Hit"):
                break;
            default:
                Debug.Log(actionTayp);
                break;
        }
        curentCard = null;
        action = -1;

        if (shotTime)
        {
            LoadUiView(0, "ShotView");
            LoadUiView(1, "ShotView");
        }
        else
        {

            LoadUiView(0, "MeleeView");
            LoadUiView(1, "MeleeView");
        }
    }
    void SelectAction(int a)
    {
        action = a;

        actionTayp = gameSetting.Library.Action[action].Tayp;

        if (actionTayp == "avtoActiv")
        {
            action = -1;
            curentCard = null;
            UseAction();
            //Приписать расширение для авто активации соответвующих скилов

        }
        else if (shotTime)
        {
            LoadUiView(0, "ShotTarget");
            LoadUiView(1, "ShotTarget");
        }
        else
        {

            LoadUiView(0, "MeleeTarget");
            LoadUiView(1, "MeleeTarget");
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
        shotTime = false;
        if (curentPlayer == 0)
        {
            LoadUiView(0, "MeleeView");
            LoadUiView(1, "MeleeView");
        }
        else
        {
            LoadUiView(0, "Clear");
            LoadUiView(1, "Clear");
        }
        //LoadUIMelee
    }
    void ShotTurn()
    {

        if (curentPlayer == 1)
            curentPlayer = 0;
        else
            curentPlayer = 1;

        shotTime = true;
        if (curentPlayer == 0)
        {
            LoadUiView(0, "ShotView");
            LoadUiView(1, "ShotView");
        }
        else
        {
            LoadUiView(0, "Clear");
            LoadUiView(1, "Clear");
        }
        //  if (curentPlayer ==1)
        // LoadUiShot();

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
                hiro.NextCard++;
            }
        }

    }

    #endregion
}
