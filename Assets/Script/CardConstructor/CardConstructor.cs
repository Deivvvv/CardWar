using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saver;

using System.Linq;

public class CardConstructor : MonoBehaviour
{
    private int curentNum;
    private CardBase cardBase;
    public List<CardBase> LocalCard;
    [SerializeField]
    private List<int> newCard;
    [SerializeField]
    private CardConstructorUi Ui;
  //  [SerializeField]
    private GameData gameData;

    [SerializeField]
    private GameSetting gameSetting;

    private List<int> newData;
    private int curentFiltr;
    private bool filterRevers;
    //[SerializeField]
    //private XMLSaver saver;

    private string origPath = $"/Resources/Hiro";

    void GenerateFiltr()
    {
        int a = gameSetting.SellCount.Length + 1;

        GameObject GO = null;

        GO = Instantiate(Ui.OrigFiltr);// BaseCard.GetChild(a + 1).gameObject;
        GO.transform.SetParent(Ui.BaseFiltr);

        AddFiltr(-1, GO.GetComponent<Button>());


        for (int i = 0; i < a; i++)
        {

            GO = Instantiate(Ui.OrigFiltr);// BaseCard.GetChild(a + 1).gameObject;
            GO.transform.SetParent(Ui.BaseFiltr);

            GO.GetComponent<Image>().sprite = gameSetting.Icon[i];
            AddFiltr(i, GO.GetComponent<Button>());
        }
    }
    void AddFiltr(int a, Button button)
    {

        button.onClick.AddListener(() => SetFiltr(a)); 
    }
    void SetFiltr(int a)
    {
        if (curentFiltr == a)
            filterRevers = !filterRevers;
        else
            filterRevers = false;

        curentFiltr = a;

        Sort();
    }

    void Sort()
    {
        IEnumerable<CardBase> items = null;//biomData.Arsenal.OrderBy(i => i.Class).ThenBy(x => x.CostMin).ThenBy(x => x.Qvailty);


        if (curentFiltr == -1)
        {
            items = LocalCard;
        }
        else
        {
            if(filterRevers)
                items = LocalCard.OrderBy(i => i.Stat[curentFiltr]);
            else
                items = LocalCard.OrderByDescending(i => i.Stat[curentFiltr]);
          

        }

        foreach (CardBase item in items)
        {
            item.Body.SetParent(Ui.TraitCard);
            item.Body.SetParent(Ui.BaseCard);
        }
    }

    void Enject()
    {
        CardBase card = new CardBase();
        card.Stat = new int[cardBase.Stat.Length];
        card.Trait = new string[cardBase.Trait.Length ];
        card.Name = Ui.NameFlied.text;

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
            string path = "";

            if (gameData.BlackList.Count > 0)
            {
                int a = gameData.BlackList[0];

                //path = $"/Resources/Hiro{a}";
                //gameData.AllCard[a] = path;

                LocalCard[a].Body.gameObject.active = true;
                card.Body = LocalCard[a].Body;
                LocalCard[a] = card;
                ViewCardBase(a);
                
                gameData.BlackList.RemoveAt(0);
                AddEdit(a);
            }
            else
            {
                path = Application.dataPath + origPath + $"{LocalCard.Count}";

                AddEdit(LocalCard.Count);

                LocalCard.Add(card);
                NewCard(LocalCard.Count - 1);
            }

        }
        else
        {
            int a = curentNum - 1;
            card.Body = LocalCard[a].Body;
            LocalCard[a] = card;
            ViewCardBase(a);

            AddEdit(a);
        }

        Sort();
    }
    void Inject()
    {
        int a = curentNum;
        curentNum = -1;
        Delite();
        curentNum = a;

        if (curentNum > 0)
        {
            CardBase card = LocalCard[curentNum-1];
            cardBase.Name = card.Name; 
            Ui.NameFlied.text = cardBase.Name;

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
        if (curentNum != -1)
        {
            gameData.BlackList.Add(curentNum-1);
            LocalCard[curentNum-1].Body.gameObject.active = false;
            // Ui.
            SwitchCard(-1);
        }
        cardBase = new CardBase();

        cardBase.Name = "New Hiro";
        cardBase.Stat = new int[13];
        cardBase.Trait = new string[5];
        cardBase.Stat[4] = 1;
        Ui.NameFlied.text = cardBase.Name;
        ViewCard();
    }

    void TransfData(GameData Data1, GameData Data2)
    {
        Data2.AllCard = Data1.AllCard;


        if (Data1.BlackList.Count > Data2.BlackList.Count)
        {
            int a = Data1.BlackList.Count - Data2.BlackList.Count;
            for (int i = 0; i < a; i++)
            {
                Data2.BlackList.Add(-1);
            }
        }
        else if (Data1.BlackList.Count < Data2.BlackList.Count)
        {
            int a = Data2.BlackList.Count - Data1.BlackList.Count;
            for (int i = 0; i < a; i++)
            {
                Data2.BlackList.RemoveAt(0);
            }
        }

        for (int i = 0; i < Data1.BlackList.Count; i++)
        {
            Data2.BlackList[i] = Data1.BlackList[i];
        }
    }
    void AddEdit(int a)
    {
        for (int i = 0; i < newCard.Count; i++)
        {
            if (a == newCard[i])
                return;
        }
        newCard.Add(a);
    }

    void Save()
    {
        gameData.AllCard = LocalCard.Count;
        TransfData(gameData, gameSetting.GlobalMyData);
        int b = 0;
        string path = "";
        for (int i = 0; i < newCard.Count;i++)
        {
            b = newCard[i];
            path = Application.dataPath + origPath + $"{b}";
            XMLSaver.ISave(LocalCard[b], path);
        }
        newCard = new List<int>();

    }
    void Reset(int a)
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
        int b = curentNum;
        //ѕосле по€влени€ метода сортировки перевести на локальный номер карты
        if(b!= -1)
            Ui.BaseCard.GetChild(b).gameObject.GetComponent<Image>().color = Ui.SelectColor[1];
        else
            Ui.BaseCard.GetChild(0).gameObject.GetComponent<Image>().color = Ui.SelectColor[1];

        curentNum = a;
        b = curentNum;

        if (b != -1)
            Ui.BaseCard.GetChild(b).gameObject.GetComponent<Image>().color = Ui.SelectColor[0];
        else
            Ui.BaseCard.GetChild(0).gameObject.GetComponent<Image>().color = Ui.SelectColor[0];

    }
    void NewCard(int i)
    {
        GameObject GO = Instantiate(Ui.OrigCard);// BaseCard.GetChild(a + 1).gameObject;
        GO.transform.SetParent(Ui.BaseCard);

        int a = Ui.BaseCard.childCount - 1;
        Button button = GO.GetComponent<Button>();
        SwitchCardButton(a, button);

        GO.GetComponent<Image>().color = Ui.SelectColor[1];

        LocalCard[i].Body = GO.transform;
      //  LocalCard.Add(new CardBase());
      //  Save();

        ViewCardBase(i);
    }

    void ViewCard()
    {
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
            text.text = "";
            for (int i = 0; i < card.Stat.Length-1; i++)
            {
                if (card.Stat[i] > 0) 
                    text.text += $"<sprite name={gameSetting.NameIcon[i]}>{card.Stat[i]} ";
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

        string path = "";
        int a = gameData.AllCard;
        CardConstructor cardConstructor = gameObject.GetComponent<CardConstructor>();
        for (int i = 0; i < a; i++)
        {
            path = Application.dataPath + origPath + $"{i}";
            XMLSaver.ILoad(path, cardConstructor);
            NewCard(i);
        }

        a = gameData.BlackList.Count;
        for (int i = 0; i < a; i++)
        {
            LocalCard[gameData.BlackList[i]].Body.gameObject.active = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cardBase = new CardBase();

        cardBase.Name = "New Hiro";
        cardBase.Stat = new int[13];
        cardBase.Trait = new string[5];
        PreLoad();


        LoadBase();
        // NewCard();

        LoadUi();
        Inject();

        GenerateFiltr();
    }

    void PreLoad()
    {
        curentNum = -1;
       // Delite();

        LocalCard = new List<CardBase>();
        newCard = new List<int>();



        GameObject GO = Ui.BaseCard.GetChild(0).gameObject;
        GO.GetComponent<Image>().color = Ui.SelectColor[0];
        GO.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); ;

        Ui.EjectButton.onClick.AddListener(() => Enject());
        Ui.InjectButton.onClick.AddListener(() => Inject());
        Ui.DeliteButton.onClick.AddListener(() => Delite());

        Ui.SaveButton.onClick.AddListener(() => Save());

        gameData = gameSetting.ReservData;//= new GameData();
       // gameData.AllCard = // = new List<string>();
        gameData.BlackList = new List<int>();


        TransfData( gameSetting.GlobalMyData, gameData);


    }


}
