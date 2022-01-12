using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

using Saver;
using BattleTable;


public class CardConstructor : MonoBehaviour
{
    /*
     * Раса - Легион
     * Легион - Легион
     * Выбрать социальную группу
     * 
     * на основе социальной группы Фиксирывать корневой парметр, его утрата приведет к смерти армии
     * Добавить три любых иных парметра
     * 
     * Вывести правила(черты, которые карта наследует от выше перечисленных групп), 
     * их стоимость не указывается внутри системы т.к. они наследуются от выше стоящих источников
     * Выбрать до 5 правил как дополнительно
     */


    private List<Constant> constants;
    private int curentConstant;

    //ResetSystem
    private CardConstructor cardConstructor;
    private int oldAllCard;
    private List<CardBase> oldCard;
    private List<int> newCard;

    private int curentNum;
    private CardBase cardBase;
    public List<CardBase> LocalCard;
   // [SerializeField]
    [SerializeField]
    private CardConstructorUi Ui;
  //  [SerializeField]
    private GameData gameData;
    private GameData gameDataReserv;

    [SerializeField]
    private GameSetting gameSetting;

    private List<int> newData;
    private int curentFiltr;
    private bool filterRevers;
    //[SerializeField]
    //private XMLSaver saver;

    private string origPath;
    private string origPathAlt;

    public RenderTexture rTex;
    public Camera captureCamera;

    #region Filtr System
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
    #endregion
    #region IO card System
    void Enject()
    {
        CardBase card = new CardBase();
        card.Stat = new List<int>();
        card.Trait = new List<string>();
        card.Name = Ui.NameFlied.text;

        for (int i = 0; i < cardBase.Stat.Count; i++)
        {
            card.Stat[i] = cardBase.Stat[i];
        }

        for (int i = 0; i < cardBase.Trait.Count; i++)
        {
            card.Trait[i] = cardBase.Trait[i];
        }

        int width = 100;
        int height = 150;
        Texture2D texture = new Texture2D(width, height);

        RenderTexture targetTexture = RenderTexture.GetTemporary(width, height);
        targetTexture.depth = 2;

        captureCamera.targetTexture = targetTexture;
        captureCamera.Render();
        RenderTexture.active = targetTexture;

        Rect rect = new Rect(0, 0, width, height);
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();
        captureCamera.targetTexture = rTex;

        card.Image = texture.EncodeToPNG();

        if (curentNum < 0)
        {
          //  string path = "";

            if (gameData.BlackList.Count > 0)
            {
                int a = gameData.BlackList[0];

                //path = $"/Resources/Hiro{a}";
                //gameData.AllCard[a] = path;

                LocalCard[a].Body.gameObject.active = true;
                card.Body = LocalCard[a].Body;
                LocalCard[a] = card;

                CardView.IViewCard(card);


                gameData.BlackList.RemoveAt(0);
            }
            else
            {
               // path = origPath + $"{LocalCard.Count}";

                AddEdit(LocalCard.Count, card);
            //    AddEdit(LocalCard.Count);

                LocalCard.Add(card);
                NewCard(LocalCard.Count - 1);
            }

        }
        else
        {

            card.Body = LocalCard[curentNum].Body;

            AddEdit(curentNum, LocalCard[curentNum]);

            LocalCard[curentNum] = card;

            CardView.IViewCard(card);

        }

        Sort();
    }
    void Inject()
    {
        int a = curentNum;
        curentNum = -1;
        Delite();
        curentNum = a;

        if (curentNum > -1)
        {
            CardBase card = LocalCard[curentNum];
            cardBase.Name = card.Name; 
            Ui.NameFlied.text = cardBase.Name;

            for (int i = 0; i < cardBase.Stat.Count; i++)
            {
                cardBase.Stat[i] = card.Stat[i];
            }

            for (int i = 0; i < cardBase.Trait.Count; i++)
            {
                cardBase.Trait[i] = card.Trait[i];
            }

            //Выгрузка данных в редактор героев
        }
        ReCalculate();
    }
    void Delite()
    {
        if (curentNum != -1)
        {
            gameData.BlackList.Add(curentNum);
            LocalCard[curentNum].Body.gameObject.active = false;
            // Ui.
            SwitchCard(-1);
        }
        cardBase = new CardBase();

        cardBase.Name = "New Hiro";
        cardBase.Stat =  new List<int>();
        cardBase.Trait = new List<string>();
        cardBase.Stat[4] = 1;
        Ui.NameFlied.text = cardBase.Name;
        ViewCard();
    }
    #endregion

    #region Save/Reset/Load System(Data Control)

    void PreLoad()
    {
        origPath = Application.dataPath + $"/Resources/Data/Hiro";
        origPathAlt = Application.dataPath + $"/Resources/Data";

        Core.ILoadGameSetting(gameSetting);
        cardConstructor = gameObject.GetComponent<CardConstructor>();

       // MakeScrenshot();
        
        curentNum = -1;
        // Delite();

        LocalCard = new List<CardBase>();
        newCard = new List<int>();
        oldCard = new List<CardBase>();



        GameObject GO = Ui.BaseCard.GetChild(0).gameObject;
        GO.GetComponent<Image>().color = Ui.SelectColor[0];
        GO.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); ;

        Ui.EjectButton.onClick.AddListener(() => Enject());
        Ui.InjectButton.onClick.AddListener(() => Inject());
        Ui.DeliteButton.onClick.AddListener(() => Delite());

        Ui.SaveButton.onClick.AddListener(() => Save());
        Ui.ResetButton.onClick.AddListener(() => Reset());

        gameData = new GameData();
                                          // gameData.AllCard = // = new List<string>();
        gameData.BlackList = new List<int>();

        oldAllCard = gameData.AllCard;

        // TransfData(gameSetting.GlobalMyData, gameData);
        XMLSaver.LoadGameData(origPathAlt, cardConstructor);

        LoadBase();
    }

    public void TransfData(GameData gameData1, GameData gameData2)
    {
        gameData2 = new GameData();

        gameData2.AllCard = gameData1.AllCard;
        int a = gameData1.BlackList.Count;
        gameData2.BlackList = new List<int>();
        for (int i = 0; i < a; i++)
        {
            gameData2.BlackList.Add(gameData1.BlackList[i]);
        }

        gameData = gameData1;
        gameDataReserv = gameData2;
    }


    void LoadBase()
    {

        string path = "";
        int a = gameData.AllCard;
        for (int i = 0; i < a; i++)
        {
            path = origPath + $"{i}";
            XMLSaver.Load(path, cardConstructor);
            NewCard(i);
        }

        a = gameData.BlackList.Count;
        for (int i = 0; i < a; i++)
        {
            LocalCard[gameData.BlackList[i]].Body.gameObject.active = false;
        }
    }

    void Reset()
    {
        int a = newCard.Count;
        int b = 0;
        for (int i = 0; i < a; i++)
        {
            b = newCard[i];
            LocalCard[b] = oldCard[i];
            CardView.IViewCard(oldCard[i]);
        }

        //Remove add LocalCard
        a = LocalCard.Count;
        if (oldAllCard < a)
        {
            a--;
            for (int i = oldAllCard - 1; i < a; i++)
            {
                Destroy(LocalCard[oldAllCard].Body.gameObject);
                LocalCard.RemoveAt(oldAllCard);
            }
        }

        //BlackList

        TransfData( gameDataReserv, gameData);

        // a = gameData.BlackList.Count;
        // for (int i = 0; i < a; i++)
        // {
        //     b = gameData.BlackList[i];
        //     if (LocalCard.Count > b)
        //     {
        //         LocalCard[b].Body.gameObject.active = true;
        //     }
        // }

        //// TransfData(gameSetting.GlobalMyData, gameData);


        // a = gameData.BlackList.Count;
        // for (int i = 0; i < a; i++)
        // {
        //     b = gameData.BlackList[i];
        //     LocalCard[b].Body.gameObject.active = false;
        // }

        //ResetData
        newCard = new List<int>();
        oldCard = new List<CardBase>();
    }

  //  void TransfData(GameData Data1, GameData Data2)
    //{
    //    Data2.AllCard = Data1.AllCard;


    //    if (Data1.BlackList.Count > Data2.BlackList.Count)
    //    {
    //        int a = Data1.BlackList.Count - Data2.BlackList.Count;
    //        for (int i = 0; i < a; i++)
    //        {
    //            Data2.BlackList.Add(-1);
    //        }
    //    }
    //    else if (Data1.BlackList.Count < Data2.BlackList.Count)
    //    {
    //        int a = Data2.BlackList.Count - Data1.BlackList.Count;
    //        for (int i = 0; i < a; i++)
    //        {
    //            Data2.BlackList.RemoveAt(0);
    //        }
    //    }

    //    for (int i = 0; i < Data1.BlackList.Count; i++)
    //    {
    //        Data2.BlackList[i] = Data1.BlackList[i];
    //    }
    //}
    void AddEdit(int a, CardBase card)
    {
        for (int i = 0; i < newCard.Count; i++)
        {
            if (a == newCard[i])
                return;
        }
        newCard.Add(a);
        oldCard.Add(card);
    }
    void Save()
    {
        TransfData(gameData, gameDataReserv);
        // gameData.AllCard = LocalCard.Count;
        // TransfData(gameData, gameSetting.GlobalMyData);
        int b = 0;
        string path = "";
        for (int i = 0; i < newCard.Count; i++)
        {
            b = newCard[i];
            path =  origPath + $"{b}";
            XMLSaver.Save(LocalCard[b], path);
        }

        XMLSaver.SaveGameData(gameData,  origPathAlt);
        oldAllCard = gameData.AllCard;
        newCard = new List<int>();
        oldCard = new List<CardBase>();
    }
    #endregion

    #region Create UI
    void CreateStatButton()
    {
        GameObject GO = null;
      //  Ui.StatCount = new Text[cardBase.Stat.Count - 1];
        for (int i = 0; i < 4; i++)
        {
            GO = Instantiate(Ui.OrigStat);
            GO.transform.SetParent(Ui.StatCard);

            //GO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = gameSetting.Icon[i];
            //GO.transform.GetChild(1).gameObject.GetComponent<Text>().text = $"{gameSetting.SellCount[i]}/4";
            //GO.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = $"{gameSetting.StatName[i]}";

            Button newButton = GO.transform.GetChild(3).gameObject.GetComponent<Button>();
            AddStatButton(false, newButton, i);
            newButton = GO.transform.GetChild(4).gameObject.GetComponent<Button>();
            AddStatButton(true, newButton, i);

            Ui.StatCount[i] = GO.transform.GetChild(5).gameObject.GetComponent<Text>();
            //Ui.StatCount[i].text = $"{cardBase.Stat[i]}";
        }
        // cardBase.Stat

    }
    void LoadStatUI(int a, Constant constant)
    {
        GameObject GO = Ui.StatCard.GetChild(a).gameObject;
        if (constant != null)
        {
            GO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = constant.Icon;
            GO.transform.GetChild(1).gameObject.GetComponent<Text>().text = $"{constant.Cost}/4";
            GO.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = constant.NameLocalization;

            Ui.StatCount[a].text = $"{cardBase.Stat[a]}";
        }
        else
        {
            GO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
            GO.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
            GO.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "Выберете параметр";

            Ui.StatCount[a].text = "";
        }
    }


    void AddStatButton(bool plus, Button button, int a)
    {
        if (plus)
            button.onClick.AddListener(() => StatUp(a));
        else
            button.onClick.AddListener(() => StatDown(a));
    }
    void SwitchCardButton(int a, Button button)
    {
        button.onClick.AddListener(() => SwitchCard(a));
    }

    void NewCard(int i)
    {
        int a = Ui.BaseCard.childCount - 1;
        GameObject GO = Instantiate(Ui.OrigCard);// BaseCard.GetChild(a + 1).gameObject;
        GO.transform.SetParent(Ui.BaseCard);

        Button button = GO.GetComponent<Button>();
        SwitchCardButton(a, button);

        GO.GetComponent<Image>().color = Ui.SelectColor[1];

        LocalCard[i].Body = GO.transform;

        CardView.IViewCard(LocalCard[i]);
    }

    #region Constant

    void CreateConstantSeclector()
    {
        GameObject GO = null;
        constants = gameSetting.Library.Constants;

        for (int i = 0; i < constants.Count; i++)
        {
            GO = Instantiate(Ui.OrigButton);
            GO.transform.SetParent(Ui.ConstantSelector);

            LoadConstantButton(GO.GetComponent<Button>(), i);

            GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = constants[i].NameLocalization;
        }
    }

    void LoadConstantButton(Button button, int a)
    {
        button.onClick.AddListener(() => LoadConstant(a));
    }

    void LoadConstant( int a)
    {
        cardBase.Stat[curentConstant] = a;

        LoadStatUI(curentConstant, constants[a]);
    }

    #endregion


    #region Rule

    #endregion

    #endregion

    #region Ui Use

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
        for (int i = 0; i < cardBase.Stat.Count - 1; i++)
        {
            a += cardBase.Stat[i] * gameSetting.SellCount[i];
        }
        cardBase.Stat[cardBase.Stat.Count - 1] = Mathf.CeilToInt(a / 4f);
        ViewCard();
    }

    void ViewCard()
    {
        for (int i = 0; i < cardBase.Stat.Count - 1; i++)
        {
            Ui.StatCount[i].text = $"{cardBase.Stat[i]}";
        }
        Ui.ManaCount.text = $"Цена: {cardBase.Stat[cardBase.Stat.Count - 1]}";
        // Ui.
    }

    void SwitchCard(int a)
    {
        if (curentNum != -1)
        {
            if (curentNum < gameData.AllCard)
                LocalCard[curentNum].Body.gameObject.GetComponent<Image>().color = Ui.SelectColor[1];
        }
        else
            Ui.BaseCard.GetChild(0).gameObject.GetComponent<Image>().color = Ui.SelectColor[1];

        curentNum = a;

        if (curentNum != -1)
            LocalCard[curentNum].Body.gameObject.GetComponent<Image>().color = Ui.SelectColor[0];
        else
            Ui.BaseCard.GetChild(0).gameObject.GetComponent<Image>().color = Ui.SelectColor[0];

    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        NewCard();

        PreLoad();

        CreateStatButton();
        Inject();

        GenerateFiltr();
    }

    void NewCard()
    {
        cardBase = new CardBase();

        cardBase.Name = "New Hiro";
        cardBase.Stat = new List<int>();
        cardBase.Trait = new List<string>();
    }


}
