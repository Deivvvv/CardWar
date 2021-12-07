using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saver;

public class CardConstructor : MonoBehaviour
{
    private int curentNum;
    private CardBase cardBase;
    public List<CardBase> LocalCard;

    [SerializeField]
    private CardConstructorUi Ui;
  //  [SerializeField]
    private GameData gameData;

    [SerializeField]
    private GameSetting gameSetting;

    private List<int> newData;
    //[SerializeField]
    //private XMLSaver saver;

    void Enject()
    {
        CardBase card = new CardBase();

        for (int i = 0; i < cardBase.Stat.Length; i++)
        {
            card.Stat[i] = cardBase.Stat[i];
        }

        for (int i = 0; i < cardBase.Trait.Length; i++)
        {
            card.Trait[i] = cardBase.Trait[i];
        }

        if (curentNum < 0)
        {
            LocalCard.Add(card);
            NewCard(LocalCard.Count-1);
        }
        else
        {
            LocalCard[curentNum] = card;
            ViewCardBase(curentNum);
        }
      //  Sort();
    }
    void Inject()
    {
        cardBase = new CardBase();
        if (curentNum < 0)
        {
            cardBase.Name = "New Hiro";
            cardBase.Stat = new int[13];
            cardBase.Trait = new string[5];

            cardBase.Stat[4] = 1;
        }
        else
        {
            cardBase = new CardBase();
            CardBase card = LocalCard[curentNum];

            for (int i = 0; i < cardBase.Stat.Length; i++)
            {
                cardBase.Stat[i] = card.Stat[i];
            }

            for (int i = 0; i < cardBase.Trait.Length; i++)
            {
                cardBase.Trait[i] = card.Trait[i];
            }
        }
        ReCalculate();
    }
    void Delite()
    {
        gameData.BlackList.Add(curentNum);
        LocalCard[curentNum].Body.gameObject.active = false;
       // Ui.
        SwitchCard(-1);
    }

    void Save()
    {
        //if (curentNum == -1)
        //{
        //    string path = $"/Resources/Hiro{gameData.AllCard.Count}";

        //    if (gameData.BlackList.Count > 0)
        //    {
        //        gameData.AllCard[gameData.BlackList[0]] = path;
        //        path = Application.dataPath + $"/Resources/Hiro{gameData.BlackList[0]}";
        //        gameData.BlackList.RemoveAt(0);
        //    }
        //    else
        //    {
        //        gameData.AllCard.Add(path);
        //        path = Application.dataPath + $"/Resources/Hiro{gameData.AllCard.Count - 1}";
        //    }

        //    XMLSaver.ISave(cardBase, path);
        //}
        //else
        //{
        //    string path = Application.dataPath + $"/Resources/Hiro{curentNum}";
        //    XMLSaver.ISave(cardBase, path);
        //}
    }
    void Load(int a)
    {
        string path = $"/Resources/Hiro{a}";
        //XMLSaver.ILoad(path, Ui,a);
    }

    void LoadUi()
    {
        GameObject GO = null;
        Ui.StatCount = new Text[cardBase.Stat.Length - 1];
        for (int i = 0; i < cardBase.Stat.Length - 1; i++)
        {
            GO = Instantiate(Ui.OrigStat);
            GO.transform.SetParent(Ui.StatCard);

            GO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = gameSetting.Icon[i];
            GO.transform.GetChild(1).gameObject.GetComponent<Text>().text = $"{gameSetting.SellCount[i]}/4";
            GO.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = $"{gameSetting.StatName[i]}";

            Button newButton = GO.transform.GetChild(3).gameObject.GetComponent<Button>();
            AddStatButton(false, newButton, i);
            newButton = GO.transform.GetChild(4).gameObject.GetComponent<Button>();
            AddStatButton(true, newButton, i);

            Ui.StatCount[i] = GO.transform.GetChild(5).gameObject.GetComponent<Text>();
            Ui.StatCount[i].text = $"{cardBase.Stat[i]}";
        }
        // cardBase.Stat
    }
    void AddStatButton(bool plus, Button button, int a)
    {
        if (plus)
            button.onClick.AddListener(() => StatUp(a));
        else
            button.onClick.AddListener(() => StatDown(a));
    }

    void StatUp(int a)
    {
        cardBase.Stat[a]++;
        ReCalculate();
        if (cardBase.Stat[12] > 10)
        {
            cardBase.Stat[a]--;
            ReCalculate();
        }
    }
    void StatDown(int a)
    {
        if (cardBase.Stat[a] > 0)
        {
            if (a == 4)
            {
                if (cardBase.Stat[4] > 1)
                    cardBase.Stat[4]--;
            }
            else
            {
                if (cardBase.Stat[4] > 0)
                    cardBase.Stat[a]--;
            }
            ReCalculate();
        }
    }
    void ReCalculate()
    {
        int a = 0;
        for (int i = 0; i < cardBase.Stat.Length - 1; i++)
        {
            a += cardBase.Stat[i] * gameSetting.SellCount[i];
        }
        cardBase.Stat[cardBase.Stat.Length - 1] = Mathf.CeilToInt(a / 4f);
        ViewCard();
    }
    void SwitchCardButton(int a, Button button)
    {
        button.onClick.AddListener(() => SwitchCard(a));
    }
    void SwitchCard(int a)
    {
        int b = curentNum + 1;
        //ѕосле по€влени€ метода сортировки перевести на локальный номер карты
        Ui.BaseCard.GetChild(b).gameObject.GetComponent<Image>().color = Ui.SelectColor[1];

        curentNum = a;
        b = curentNum + 1;

        Ui.BaseCard.GetChild(b).gameObject.GetComponent<Image>().color = Ui.SelectColor[0];

    }
    void NewCard(int i)
    {
        GameObject GO = Instantiate(Ui.OrigCard);// BaseCard.GetChild(a + 1).gameObject;
        GO.transform.SetParent(Ui.BaseCard);

        int a = Ui.BaseCard.childCount - 1;
        Button button = GO.GetComponent<Button>();
        SwitchCardButton(a, button);

        LocalCard[i].Body = GO.transform;
      //  LocalCard.Add(new CardBase());
      //  Save();

        ViewCardBase(i);
    }

    void ViewCard()
    {
        Ui.NameFlied.text = cardBase.Name;
        for (int i = 0; i < cardBase.Stat.Length - 1; i++)
        {
            Ui.StatCount[i].text = $"{cardBase.Stat[i]}";
        }
        Ui.ManaCount.text = $"÷ена: {cardBase.Stat[cardBase.Stat.Length - 1]}";
        // Ui.
    }
    void ViewCardBase(int a)
    {
        if (a != -1)
        {
            CardBase card = LocalCard[a];
               Transform trans = card.Body;

            //trans.GetChild(1).//портреты

            TMP_Text text = trans.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>();
            text.text = card.Name;

            text = trans.GetChild(2).GetChild(0).gameObject.GetComponent<TMP_Text>();
            for (int i = 0; i < card.Stat.Length; i++)
            {
                if (card.Stat[i] > 0)
                    text.text += $"<sprite name={gameSetting.NameIcon[0]}>{card.Stat[0]} ";
            }
            // trans.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>().text = LocalCard[a].Name;


            text = trans.GetChild(3).GetChild(0).gameObject.GetComponent<TMP_Text>();
            text.text = ""+ card.Stat[card.Stat.Length-1];
            //   GameObject GO = Ui.BaseCard.GetChild(a + 1).gameObject;
        }
        //  Ui.NameFlied.text = cardBase.Name;
    }

    void LoadBase()
    {
        int a = gameData.AllCard.Count;
        string path = "";

        CardConstructor cardConstructor = gameObject.GetComponent<CardConstructor>();
        for (int i = 0; i < a; i++)
        {
            // NewCard();
            path = Application.dataPath + $"/Resources/Hiro{i}";
            XMLSaver.ILoad(path, cardConstructor, i);
            NewCard(i);
        }
        // for (int i = 0; i < LocalCard.Count; i++)
        for (int i = 0; i < gameData.BlackList.Count; i++)
        {
            LocalCard[gameData.BlackList[i]].Body.gameObject.active = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curentNum = -1;
        cardBase = new CardBase();
        cardBase.Stat = new int[13];
        cardBase.Trait = new string[5];

        PreLoad();

        GameObject GO = Ui.BaseCard.GetChild(0).gameObject;
        GO.GetComponent<Image>().color = Ui.SelectColor[0];
        GO.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); ;

        LocalCard = new List<CardBase>();

        LoadBase();
        // NewCard();
        LoadUi();

        Inject();
    }

    void PreLoad()
    {
        gameData = new GameData();
        gameData.AllCard = new List<string>();
        gameData.BlackList = new List<int>();

        GameData AltData =  gameSetting.GlobalMyData;
        for (int i = 0; i < AltData.AllCard.Count; i++)
        {
            gameData.AllCard[i] = AltData.AllCard[i];
        }

        for (int i = 0; i < AltData.BlackList.Count; i++)
        {
            gameData.BlackList[i] = AltData.BlackList[i];
        }
        //ReservData
        //GlobalMyData
        //LocalMyData

        //ReservPlayerData
        //GlobalPlayerData
        //LocalPlayerData

    }


}
