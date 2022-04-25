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




}
