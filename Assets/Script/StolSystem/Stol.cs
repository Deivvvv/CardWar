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
    private int action =-1;

    private string actionTayp;
    private int curentPlayer;
    private bool shotTime;
    // void LoadSlotButton
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

    void Update()
    {
        if (curentPlayer == 0)
        {
            if (Input.GetMouseButtonDown(0))
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
            else if (Input.GetMouseButtonDown(1))
            {
                if (action != -1)
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
    void UseAction()
    {
        curentCard.MovePoint -= gameSetting.Library.Action[action].MoveCost;
        switch (actionTayp)
        {
            case("-1"):
                break;
            default:
                Debug.Log(actionTayp);
                break;
        }
        curentCard = null;
        action =-1;

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
            //UseAction();
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
    public void SelectTarget(int line, int slot, int position)
    {
        bool load = true;
        RealCard targetCard = hiro[line].Slots[slot].Position[position];

        //action

        if(targetCard.Team == curentCard.Team)
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


    void PlayCard(int handNum, int slot, int pos, Hiro hiro1, Hiro hiro2)
    {
        int b = hiro1.CardColod[handNum].Stat[hiro1.CardColod[handNum].Stat.Length - 1];
        if (hiro1.ManaCurent >= b)
        {
            hiro1.ManaCurent -= b;
            BattleSystem.IPlayCard(hiro1, hiro2, handNum, slot, pos);
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
        List<Slot> hiroSlot = hiro[line].Slots;
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
            newPosition = hiroSlot[i].Position[b];
            if (newPosition != null)
            {
                mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                SlotView(newPosition, mesh, mood, b);
            }

            b++;
            newPosition = hiroSlot[i].Position[b];
            if (newPosition != null)
            {
                mesh = slots[i].GetChild(b).gameObject.GetComponent<MeshRenderer>();
                SlotView(newPosition, mesh, mood, b);
            }

        }
    }


}
