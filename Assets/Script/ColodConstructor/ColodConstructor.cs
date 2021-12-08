using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Saver;
using TMPro;


public class ColodConstructor : MonoBehaviour
{
    [SerializeField]
    private GameSetting gameSetting;
    private GameData gameData;
    [SerializeField]
    private ColodConstructorUi Ui;

    [SerializeField]
    private CardSet cardSet;

    private int allCard;
    private int maxCard = 40;

    private string origPath = $"/Resources/Hiro";
    public List<CardBase> LocalCard;


    private List<int> blackList;
    private List<int> origCard;
    private List<int> origCount;
    private List<Transform> origTrans;
    // public List<Transform> OrigBody;

    void Start()
    {
        LoadBase();
        Calculation();
    }

    void LoadBase()
    {

        //GameObject GO = Ui.BaseCard.GetChild(0).gameObject;
        //GO.GetComponent<Image>().color = Ui.SelectColor[0];
        //GO.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); ;

        //Ui.EjectButton.onClick.AddListener(() => Enject());
        //Ui.InjectButton.onClick.AddListener(() => Inject());
        //Ui.DeliteButton.onClick.AddListener(() => Delite());

        //Ui.SaveButton.onClick.AddListener(() => Save());

        gameData = gameSetting.GlobalMyData;
        LocalCard = new List<CardBase>();
        int a = gameData.AllCard;
        string path = "";
        ColodConstructor colodConstructor = gameObject.GetComponent<ColodConstructor>();
        for (int i = 0; i < a; i++)
        {
            path = Application.dataPath + origPath + $"{i}";
            XMLSaver.ILoad(path, colodConstructor);
            NewCard(i);
        }

        a = gameData.BlackList.Count;
        for (int i = 0; i < a; i++)
        {
            LocalCard[gameData.BlackList[i]].Body.gameObject.active = false;
        }


        SetNewSet();
    }
    void SetNewSet()
    {
        ClearTable();
        origCard = new List<int>();
        origCount = new List<int>();
        blackList = new List<int>();
        origTrans = new List<Transform>();
        int a = cardSet.OrigCard.Count;
        int b = 0;
        for(int i = 0; i < a; i++)
        {
            b = cardSet.OrigCard[i];
            origCard.Add(b);
            origCount.Add(cardSet.OrigCount[i]);
            AddCardTable(b);
          
        }
    }
    void ClearTable()
    {
        int a = Ui.DeskCard.childCount;
        for(int i = 0; i < a; i++)
        {
            Destroy(Ui.DeskCard.GetChild(0).gameObject);
        }
    }

    void NewCard(int i)
    {
        int a = Ui.BaseCard.childCount;
        GameObject GO = Instantiate(Ui.OrigCard);
        GO.transform.SetParent(Ui.BaseCard);

        Button button = GO.GetComponent<Button>();
        AddCardButton(a, button);

       // GO.GetComponent<Image>().color = Ui.SelectColor[1];

        LocalCard[i].Body = GO.transform;
        //  LocalCard.Add(new CardBase());
        //  Save();

        ViewCardBase(i);
    }
    void AddCardButton(int a, Button button)
    {
        button.onClick.AddListener(() => AddCard(a));
    }
    void AddCard(int a)
    {
        if (allCard < maxCard)
        {

            int b = origCard.Count;
            for (int i = 0; i < b; i++)
            {
                if (a == origCard[i])
                {
                    if (origCount[i] < 3)
                    {
                        if (origCount[i] == 0)
                            origTrans[i].gameObject.active = true;

                        origCount[i]++;
                        Calculation();
                        ViewCardTable(i);
                    }

                    return;
                }

            }



                if (blackList.Count > 0)
                {
                    b = blackList[0];
                    blackList.RemoveAt(0);
                    origCard[b] = a;
                    origCount[b] =1;

                    origTrans[b].gameObject.active = true;
                    ViewCardTable(b);
                }
                else
                {
                    origCard.Add(a);
                    origCount.Add(1);
                    AddCardTable(a);
                }
               // ViewCardTable(b);
            Calculation();
        }
       // NewCardTable(b);
    }
    void AddCardTable(int i)
    {
        int a = Ui.DeskCard.childCount;

        GameObject GO = Instantiate(Ui.OrigCardColod);
        GO.transform.SetParent(Ui.DeskCard);

        origTrans.Add(GO.transform);

        Button button = GO.GetComponent<Button>();
        RemoveCardButton(a, button);

        ViewCardTable(a);
        // GO.GetComponent<Image>().color = Ui.SelectColor[1];

        // LocalCard[i].Body = GO.transform;
    }

    void Calculation()
    {
        int a = origCard.Count;
        allCard = 0;

        for(int i = 0; i < a; i++)
        {
            allCard += origCount[i];
        }

        Ui.CardCount.text = $"{allCard}/{maxCard}";
    }

    void RemoveCardButton(int a, Button button)
    {
        button.onClick.AddListener(() => RemoveCard(a));
    }
    void RemoveCard(int a)
    {
        origCount[a]--;
        if (origCount[a] < 1)
        {
            origTrans[a].gameObject.active = false;
        }
        else
        {
            ViewCardTable(a);
        }
        Calculation();
        //  if(origCount)
    }

    void ViewCardBase(int a)
    {
        CardBase card = LocalCard[a];
        Transform trans = card.Body;

        //trans.GetChild(1).//портреты

        TMP_Text text = trans.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>();
        text.text = card.Name;

        text = trans.GetChild(2).GetChild(0).gameObject.GetComponent<TMP_Text>();
        text.text = "";
        for (int i = 0; i < card.Stat.Length - 1; i++)
        {
            if (card.Stat[i] > 0)
                text.text += $"<sprite name={gameSetting.NameIcon[i]}>{card.Stat[i]} ";
        }
        // trans.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>().text = LocalCard[a].Name;


        text = trans.GetChild(3).GetChild(0).gameObject.GetComponent<TMP_Text>();
        text.text = "" + card.Stat[card.Stat.Length - 1];
        //   GameObject GO = Ui.BaseCard.GetChild(a + 1).gameObject;
    }
    void ViewCardTable(int a)
    {

        CardBase card = LocalCard[origCard[a]];

        Transform body = Ui.DeskCard.GetChild(a);


        if (origCount[a] > 1)
        {
            body.GetChild(1).gameObject.active = true;
        }
        else
            body.GetChild(1).gameObject.active = false;

       // Debug.Log(a);
        if (origCount[a] > 2)
            body.GetChild(0).gameObject.active = true;
        else
            body.GetChild(0).gameObject.active = false;

        Transform trans = body.GetChild(2);

      //  Debug.Log(a);
        //trans.GetChild(1).//портреты

        TMP_Text text = trans.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>();
        text.text = card.Name;

        text = trans.GetChild(2).GetChild(0).gameObject.GetComponent<TMP_Text>();
        text.text = "";
        for (int i = 0; i < card.Stat.Length - 1; i++)
        {
            if (card.Stat[i] > 0)
                text.text += $"<sprite name={gameSetting.NameIcon[i]}>{card.Stat[i]} ";
        }
        // trans.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>().text = LocalCard[a].Name;

     //   Debug.Log(a);

        text = trans.GetChild(3).GetChild(0).gameObject.GetComponent<TMP_Text>();
        text.text = "" + card.Stat[card.Stat.Length - 1];

        trans.GetChild(4).GetChild(0).gameObject.GetComponent<Text>().text = $"X{origCount[a]}";
      //  text.text = $"X{origCount[a]}";
     //   Debug.Log(a);
        //   GameObject GO = Ui.BaseCard.GetChild(a + 1).gameObject;
    }
}
