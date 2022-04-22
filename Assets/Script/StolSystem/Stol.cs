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
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private StolUi Ui;
    // private List<CardBase> enemyCard;
    [SerializeField]
    private GameSetting gameSetting;
    //  private GameData gameData;

    private int useCard =-1;

    private string actionTayp;
    private int curentPlayer;

    private GameObject selectable;

    void Start()
    {
        XMLSaver.SetGameSetting(gameSetting);
        Core.LoadGameSetting(gameSetting);

        HiroHead.SetUI(Ui);
        HiroHead.Install();

        //int r = Random.Range(0, 2);
        //if (r == 0)
        //    curentPlayer = 0;
        //else
        //    curentPlayer = 1;
     //   curentPlayer = 1;
       // NewTurn();
       // Ui.NextTurn.onClick.AddListener(() => NewTurn());

       // HiroUi(hiro[0]);
       // HiroUi(hiro[1]);

    }
    void Update()
    {
        //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, 100))
        //{
        //    TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
        //    if (targetHiro != null)
        //    {
        //        if (selectable != hit.transform.gameObject)
        //        {
        //            selectable = hit.transform.gameObject;
        //            Ui.TargetCard.gameObject.active = true;
        //            targetHiro.PreView();
        //            // CardView.IViewCard(newHiro.CardColod[a]); 
        //        }
        //    }
        //    else
        //        Ui.TargetCard.gameObject.active = false;
        //}
    }

    //void Update()
    //{
    //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, 100))
    //    {
    //        TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
    //        if (targetHiro != null)
    //        {
    //            if (selectable != hit.transform.gameObject)
    //            {
    //                selectable = hit.transform.gameObject;
    //                Ui.TargetCard.gameObject.active = true;
    //                targetHiro.PreView();
    //               // CardView.IViewCard(newHiro.CardColod[a]); 
    //            }
    //        }
    //        else
    //            Ui.TargetCard.gameObject.active = false;
    //    }

    //    if (curentPlayer == 0)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            TargetHiro targetHiro = hit.transform.gameObject.GetComponent<TargetHiro>();
    //            if (targetHiro != null)
    //            {
    //                if (useCard == -1)
    //                {
    //                    if (curentCard == null)
    //                        targetHiro.Play();
    //                    else if (action != -1)
    //                        targetHiro.Target();
    //                }
    //                else
    //                {
    //                    targetHiro.CardLoad();
    //                }
    //            }
    //        }
    //        else if (Input.GetMouseButtonDown(1))
    //        {
    //            PostUse(false);
    //        }
    //    }
    //}

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
        //GameObject GO = Instantiate(Ui.OrigCard);
        //int b = newHiro.Team;
        //if(b==0)
        //    GO.transform.SetParent(Ui.MyHand);
        //else
        //    GO.transform.SetParent(Ui.EnemyHand);

        //newHiro.CardColod[a].Body = GO.transform;

        //CardView.ViewCard(newHiro.CardColod[a]);

        //if (b == 0)
        //    GO.GetComponent<Button>().onClick.AddListener(() => GrabCard(a));
        //GO.GetComponent<Button>();
       
    }
    #endregion


}
