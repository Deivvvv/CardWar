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
    private List<Legion> actualLegion = new List<Legion>();

    private Transform selector;
    private Transform ruleSelector;

    private Guild curentGuild;
    //rule
    int keyRule = -1;
    List<string> ruleList;

    //constant
    private List<Constant> constants;
    private int curentConstant;

    //ResetSystem
    private CardConstructor cardConstructor;
    private int oldAllCard;
    private List<CardBase> oldCard;
    private List<int> newCard;

    private int cardMod;
    private int cardModSize= 0;
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

    private string origPath;
    private string origPathAlt;

    public RenderTexture rTex;
    public Camera captureCamera;

    [SerializeField]
    private TextMeshProUGUI TT;
    [SerializeField]
    private TextMeshProUGUI TT1;


    void PointerClick()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TT, Input.mousePosition, Camera.main);
        TMP_LinkInfo linkInfo = new TMP_LinkInfo();

        if (linkIndex == -1)
        {
            linkIndex = TMP_TextUtilities.FindIntersectingLink(TT1, Input.mousePosition, Camera.main);
            if (linkIndex == -1)
            {
                Debug.Log("Open link -1");
                return;
            }
            else
                linkInfo = TT1.textInfo.linkInfo[linkIndex];
        }
        else
            linkInfo = TT.textInfo.linkInfo[linkIndex];


        string selectedLink = linkInfo.GetLinkID();
        Debug.Log("Open link " + selectedLink);
        DeCoder(selectedLink);
    }

    void DeCoder(string str)
    {

    }

    void CardViews()
    {
        int a;
        for(int i=0; i < Ui.CardBody.Count; i++)
        {
            a = cardModSize + i;
            if (a < LocalCard.Count)
            {
                LocalCard[a].Body = Ui.CardBody[i];
                CardView.ViewCard(LocalCard[a]);
            }
            else
            {
                CardView.ClearCard(Ui.CardBody[i]);
            }

        }
    }
    void CardCouner()
    {
        if(curentNum != -1)
            Ui.CounterCard.text = $"{curentNum}/{LocalCard.Count}";
        else
            Ui.CounterCard.text = $"0/{LocalCard.Count}";
    }
    void Mod(bool up)
    {
        if (up)
        {
            if (cardModSize + Ui.CardBody.Count < LocalCard.Count)
            {
                cardMod++;
                //SwitchCard(-1);
            }
        }
        else if (cardMod > 0)
        {
            //SwitchCard(-1);
            cardMod--;
        }

        cardModSize = cardMod * Ui.CardBody.Count;
        CardViews();
    }

    private string LinkSupport(string colorText, string linkText, string mainText)
    {
        return $"<link={linkText}><color={colorText}>{mainText}</color></link>";
    }
    void GenerateText()
    {
        string color = "#F4FF04";
        string str1;
        string str = $"Имя{cardBase.Name}" +
            "\nРасса: " + LinkSupport(color, "Selector_Races", $"{cardBase.Races.Name}") +
            "\nЛегион: " + LinkSupport(color, "Selector_Legions", $"{cardBase.Legions.Name}") +
            "\nСоицальная группа: " + LinkSupport(color, "Selector_CivilianGroups", $"{cardBase.CivilianGroups.Name}") +
            "\nВыход на стол: ";

        str1 = (cardBase.WalkMood != null) ? cardBase.WalkMood.Name : "Null";
        str += LinkSupport(color, "Selector_Rule_Walk", $"{str1}");

        str += "\nБазовый набор атак: ";
        str1 = (cardBase.ActionMood != null) ?  cardBase.ActionMood.Name : "Null";
        str += LinkSupport(color, "Selector_Rule_Acton", $"{str1}");

        str += "\nБазовый набор защиты: ";
        str1 = (cardBase.DefMood != null) ?  cardBase.DefMood.Name : "Null";
        str += LinkSupport(color, "Selector_Rule_Def", $"{str1}");

        str += "\nХарактеристики\n";
        //    "Характеристики" +
        //    "Навыки";
        for (int i =0; i < cardBase.Stat.Count; i++)
        {
            str += LinkSupport(color, $"Selector_Stat_{i}", $"<sprite name={cardBase.Stat[i].IconName}>{cardBase.StatSize[i]}");
            str += " "; 
            str += LinkSupport(color, $"Stat_Minus_{i}", $"-");
            str += " ";
            str += LinkSupport(color, $"Stat_Plus_{i}", $"+");
            str += "\n";
        }
        str += LinkSupport(color, $"Selector_Stat_{cardBase.Stat.Count}", $"Дополнительный параметр");

        str += "\nНавыки\n";
        for (int i = 0; i < cardBase.Trait.Count; i++)
        {
            str += LinkSupport(color, $"Selector_Rule_{i}", $"{cardBase.Trait[i].Name}");
            str += " ";
            str += LinkSupport(color, $"Rule_Minus_{i}", $"-");
           // str += " ";
           // str += LinkSupport(color, $"Rule_Plus_{i}", $"+");
            str += "\n";
        }
        str += LinkSupport(color, $"Selector_Rule_{cardBase.Trait.Count}", $"Дополнительный Навык");

        TT.text = str;
    }

    void GenerateTextExtendClear()
    {
        TT1.text = "";
    }
    void GenerateTextExtend(string text)
    {
        string str = "";
        switch (text) 
        {
            case (""):
                break;
        }


        TT1.text = str;
    }
    //#region Filtr System
    //void GenerateFiltr()
    //{
    //    int a = gameSetting.SellCount.Length + 1;

    //    GameObject GO = null;

    //    GO = Instantiate(Ui.OrigFiltr);// BaseCard.GetChild(a + 1).gameObject;
    //    GO.transform.SetParent(Ui.BaseFiltr);

    //    AddFiltr(-1, GO.GetComponent<Button>());


    //    for (int i = 0; i < a; i++)
    //    {

    //        GO = Instantiate(Ui.OrigFiltr);// BaseCard.GetChild(a + 1).gameObject;
    //        GO.transform.SetParent(Ui.BaseFiltr);

    //        GO.GetComponent<Image>().sprite = gameSetting.Icon[i];
    //        AddFiltr(i, GO.GetComponent<Button>());
    //    }
    //}
    //void AddFiltr(int a, Button button)
    //{

    //    button.onClick.AddListener(() => SetFiltr(a));
    //}
    //void SetFiltr(int a)
    //{
    //    if (curentFiltr == a)
    //        filterRevers = !filterRevers;
    //    else
    //        filterRevers = false;

    //    curentFiltr = a;

    //    Sort();
    //}

    //void Sort()
    //{
    //    IEnumerable<CardBase> items = null;//biomData.Arsenal.OrderBy(i => i.Class).ThenBy(x => x.CostMin).ThenBy(x => x.Qvailty);


    //    if (curentFiltr == -1)
    //    {
    //        items = LocalCard;
    //    }
    //    else
    //    {
    //        if (filterRevers)
    //            items = LocalCard.OrderBy(i => i.Stat[curentFiltr]);
    //        else
    //            items = LocalCard.OrderByDescending(i => i.Stat[curentFiltr]);


    //    }

    //    foreach (CardBase item in items)
    //    {
    //        item.Body.SetParent(Ui.TraitCard);
    //        item.Body.SetParent(Ui.BaseCard);
    //    }
    //}
    //#endregion
    #region IO card System
    void Enject()
    {

        {
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

           // cardBase.Image = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            cardBase.Image = texture.EncodeToPNG();
        }

        cardBase.Name = Ui.NameFlied.text;

        CardBase card = Core.CardClone(cardBase);


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

                CardView.ViewCard(card);


                gameData.BlackList.RemoveAt(0);
            }
            else
            {
                // path = origPath + $"{LocalCard.Count}";

                AddEdit(LocalCard.Count, card);
                //    AddEdit(LocalCard.Count);

                LocalCard.Add(card);
                CardCouner();
                CardViews();
                //  NewCard(LocalCard.Count - 1);
            }

        }
        else
        {

            card.Body = LocalCard[curentNum].Body;

            AddEdit(curentNum, LocalCard[curentNum]);

            LocalCard[curentNum] = card;

            CardView.ViewCard(card);

        }

        //Sort();
    }
    void Inject()
    {
        //ReLoadCard();

        keyRule = -1;
        if (curentNum > -1)
        {
            cardBase = Core.CardClone(LocalCard[curentNum]);
           // Debug.Log(cardBase.Stat.Count);
            //Debug.Log(Ui.StatUi.Count);
            if (cardBase.Stat.Count < Ui.StatUi.Count)
            {
                for (int i = cardBase.Stat.Count ; i < Ui.StatUi.Count; i++)
                {
                    DeliteStat(cardBase.Stat.Count);
                }
            }
            else if (cardBase.Stat.Count > Ui.StatUi.Count)
            {
                for (int i = Ui.StatUi.Count ; i < cardBase.Stat.Count; i++)
                {
                    AddStatUi();
                }
            }

            if (cardBase.Trait.Count < Ui.RuleUi.Count)
            {
                for (int i = cardBase.Trait.Count; i < Ui.RuleUi.Count; i++)
                {
                    DeliteRule(cardBase.Trait.Count);
                }
            }
            else if (cardBase.Trait.Count > Ui.RuleUi.Count)
            {
                for (int i = Ui.RuleUi.Count; i < cardBase.Trait.Count; i++)
                {
                    AddRuleUi();
                }
            }

            //Выгрузка данных в редактор героев
            Ui.NameFlied.text = cardBase.Name;
        }
        ReCalculate();
    }
    void Delite()
    {
        if (curentNum != -1)
        {
            gameData.BlackList.Add(curentNum);
            //LocalCard[curentNum].Body.gameObject.active = false;
            // Ui.
            SwitchCard(-1);
        }

        ReLoadCard();

        Ui.NameFlied.text = cardBase.Name;
       // ViewCard();
    }
    #endregion

    #region Save/Reset/Load System(Data Control)

    void PreLoad()
    {
        origPath = Application.dataPath + $"/Resources/Data/Hiro";
        origPathAlt = Application.dataPath + $"/Resources/Data";

        Core.LoadGameSetting(gameSetting);
        Core.LoadRules();
        cardConstructor = gameObject.GetComponent<CardConstructor>();


        curentNum = -1;
        cardMod = 0;
        // Delite();

        LocalCard = new List<CardBase>();
        newCard = new List<int>();
        oldCard = new List<CardBase>();



        //GameObject GO = Ui.BaseCard.GetChild(0).gameObject;
        //GO.GetComponent<Image>().color = Ui.SelectColor[0];
        //GO.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); ;
        Ui.NewCardButton.GetComponent<Image>().color = Ui.SelectColor[0];
        Ui.NewCardButton.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); 

        Ui.ModButton[0].onClick.AddListener(() => Mod(false)); 
        Ui.ModButton[1].onClick.AddListener(() => Mod(true)); 

        Ui.EjectButton.onClick.AddListener(() => Enject());
        Ui.InjectButton.onClick.AddListener(() => Inject());
        Ui.DeliteButton.onClick.AddListener(() => Delite());

        Ui.SaveButton.onClick.AddListener(() => Save());
        Ui.ResetButton.onClick.AddListener(() => Reset());

        gameData = new GameData();
        gameData.BlackList = new List<int>();

        oldAllCard = gameData.AllCard;

        XMLSaver.LoadGameData(origPathAlt, cardConstructor);

        LoadBase();

        CardCouner();
        CardViews();
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
            LocalCard.Add(XMLSaver.Load(path));
           // NewCard(i);
        }

        a = gameData.BlackList.Count;
        for (int i = 0; i < a; i++)
        {
            LocalCard[gameData.BlackList[i]].Body.gameObject.active = false;
        }

        for(int i =0;i< Ui.CardBody.Count; i++)
        {
            Button button = Ui.CardBody[i].gameObject.GetComponent<Button>();
            SwitchCardButton(i, button);

            Ui.CardBody[i].gameObject.GetComponent<Image>().color = Ui.SelectColor[1];
            //SwitchCardButton
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
            CardView.ViewCard(oldCard[i]);
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

        TransfData(gameDataReserv, gameData);

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
            path = origPath + $"{b}";
            XMLSaver.Save(LocalCard[b], path);
        }

        XMLSaver.SaveGameData(gameData, origPathAlt);
        oldAllCard = gameData.AllCard;
        newCard = new List<int>();
        oldCard = new List<CardBase>();
    }
    #endregion

    #region Create UI


    void AddStatButton(bool plus, Button button, int a)
    {
        button.onClick.RemoveAllListeners();
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
        GameObject GO = Instantiate(Ui.OrigCard);
        GO.transform.SetParent(Ui.BaseCard);

        Button button = GO.GetComponent<Button>();
        SwitchCardButton(a, button);

        GO.GetComponent<Image>().color = Ui.SelectColor[1];

        LocalCard[i].Body = GO.transform;

        CardView.ViewCard(LocalCard[i]);
    }




    #endregion

    #region Ui Use

    void StatUp(int a)
    {
        cardBase.StatSize[a]++;
        ReCalculate();
        if (cardBase.Mana > 10)
        {
            cardBase.StatSize[a]--;
            ReCalculate();
        }
        Ui.StatUi[a].AllCount.text = "" + cardBase.StatSize[a];
    }
    void StatDown(int a)
    {
        if (cardBase.StatSize[a] > 1)
        {
            cardBase.StatSize[a]--;
            Ui.StatUi[a].AllCount.text = "" + cardBase.StatSize[a];
        }
        else if (a != 0)
            DeliteStat(a);


        ReCalculate();
    }
    void ReCalculate()
    {
        int a = 0;
        for (int i = 0; i < cardBase.Stat.Count; i++)
        {
            if (cardBase.Stat[i] != null)
            {
                a += cardBase.StatSize[i] * cardBase.Stat[i].Cost;
            }
        }
        cardBase.Mana = Mathf.CeilToInt(a / 4f);


        Ui.ManaCount.text = $"Цена:{a}/4={cardBase.Mana}";
    }


    void SwitchCard(int a)
    {
        if (cardModSize + a > LocalCard.Count)
            return;

        if (curentNum != -1)
        {
            if(curentNum < cardModSize + Ui.CardBody.Count && curentNum >= cardModSize)
            //if (cardModSize + curentNum < LocalCard.Count)
            {
                Ui.CardBody[curentNum - cardModSize].gameObject.GetComponent<Image>().color = Ui.SelectColor[1];
               // Ui.NewCardButton.gameObject.GetComponent<Image>().color = Ui.SelectColor[0];
            }
        }
        else
            Ui.NewCardButton.gameObject.GetComponent<Image>().color = Ui.SelectColor[1];

        if (a == -1)
            curentNum = -1;
        else
            curentNum = cardModSize + a;

        if (curentNum != -1)
            Ui.CardBody[a].gameObject.GetComponent<Image>().color = Ui.SelectColor[0];
        else
            Ui.NewCardButton.gameObject.GetComponent<Image>().color = Ui.SelectColor[0];
        //CardCouner();
    }
    #endregion

    #region Constant and Rule
    void AddStatUi()
    {
        GameObject GO = Instantiate(Ui.OrigStat);
        GO.transform.SetParent(Ui.StatCard);

        //  GO.GetComponent<Image>().sprite = gameSetting.Library.Guilds[i].Icon;

        StatCaseUi caseUi = GO.GetComponent<StatCaseUi>();

        int i = Ui.StatUi.Count;
        Ui.StatUi.Add(caseUi);

        AddStatButton(false, caseUi.ButtonMinus, i);
        AddStatButton(true, caseUi.ButtonPlus, i);
        // caseUi.ButtonSwitch.onClick.AddListener(() => LoadStatSelector(a);
        SetSelectorStatButton(i, caseUi.ButtonSwitch);

        caseUi.Name.text = cardBase.Stat[i].Name;
        caseUi.Icon.sprite = cardBase.Stat[i].Icon;

        caseUi.SellCount.text = $"{ cardBase.Stat[i].Cost}/4";
        caseUi.AllCount.text = "1";

        Ui.AddStat.SetParent(Ui.TraitCard);
        Ui.AddStat.SetParent(Ui.StatCard);
        ReCalculate();
        NewStatSelector();
    }

    void DeliteStat(int a)
    {
        if( a > 0)
        {
            StatCaseUi ui = Ui.StatUi[a];
            Ui.StatUi.RemoveAt(a);
            Destroy(ui.gameObject);
            if(a < cardBase.Stat.Count)
            {
                cardBase.Stat.RemoveAt(a);
                cardBase.StatSize.RemoveAt(a);
            }
            for (int i = a; i < Ui.StatUi.Count; i++)
            {
                Debug.Log(i);
                ui = Ui.StatUi[i];

                AddStatButton(false, ui.ButtonMinus, i);
                AddStatButton(true, ui.ButtonPlus, i);

                SetSwitchRuleButton(ui.ButtonSwitch, cardBase.Stat[i].Name);
            }
        }
        ReCalculate();
        NewStatSelector();
    }


    void SwitchStat(int a, int b)
    {
        if (selector != null)
            Destroy(selector.gameObject);

        if(a >= cardBase.Stat.Count)
        {
            cardBase.Stat.Add(gameSetting.Library.Constants[b]);
            cardBase.StatSize.Add(0);
            cardBase.StatSizeLocal.Add(0);
            AddStatUi();
            Ui.StatUi[0].ButtonSwitch.enabled = false;
        }

        StatCaseUi caseUi = Ui.StatUi[a];
        cardBase.StatSize[a] = 0;
        cardBase.Stat[a] = gameSetting.Library.Constants[b];
        StatUp(a);

        caseUi.Name.text = cardBase.Stat[a].Name;
        caseUi.Icon.sprite = cardBase.Stat[a].Icon;

        caseUi.SellCount.text = $"{ cardBase.Stat[a].Cost}/4";
        //caseUi.AllCount.text = "1";
        //if (cardBase.StatSize[a] == 0)
        //{
        //    DeliteStat(a);
        //    return;
        //}
    }


    void AddRuleUi()
    {
        GameObject GO = Instantiate(Ui.OrigStat);
        GO.transform.SetParent(Ui.TraitCard);

        //  GO.GetComponent<Image>().sprite = gameSetting.Library.Guilds[i].Icon;

        StatCaseUi caseUi = GO.GetComponent<StatCaseUi>();

        int i = Ui.RuleUi.Count;
        Ui.RuleUi.Add(caseUi);

        //AddRuleButton(false, caseUi.ButtonMinus, i);
        //AddRuleButton(true, caseUi.ButtonPlus, i);

        DeliteRuleButton(i, caseUi.ButtonMinus);
        // caseUi.ButtonSwitch.onClick.AddListener(() => LoadStatSelector(a);
        SetSelectorRuleButton(i, caseUi.ButtonSwitch);
        //SetSwitchRuleButton(ui.ButtonSwitch, cardBase.Stat[i].Name);
        caseUi.Name.text = cardBase.Trait[i].Name;

        //caseUi.Name.text = cardBase.Stat[a].Name;
       // caseUi.Icon.sprite = cardBase.Stat[a].Icon;

        caseUi.SellCount.text = $"{ cardBase.Trait[i].Cost}/4";
        caseUi.AllCount.text = "1";


        Ui.AddRule.SetParent(Ui.StatCard);
        Ui.AddRule.SetParent(Ui.TraitCard);
        NewRuleSelector();
    }

    void DeliteRule(int a)
    {
        StatCaseUi ui = Ui.RuleUi[a];
        Ui.RuleUi.RemoveAt(a);
        Destroy(ui.gameObject);

        if (a < cardBase.Trait.Count)
        {
            cardBase.Trait.RemoveAt(a);
            cardBase.TraitSize.RemoveAt(a);
        }
        for (int i = a; i < Ui.RuleUi.Count; i++)
        {
            ui = Ui.RuleUi[i];

            //AddStatButton(false, ui.ButtonMinus, i);
            //AddStatButton(true, ui.ButtonPlus, i);

            DeliteRuleButton(i, ui.ButtonMinus);
            SetSelectorRuleButton(i, ui.ButtonSwitch);
            //SetSwitchRuleButton(ui.ButtonSwitch, cardBase.Stat[i].Name);
        }
        NewRuleSelector();
    }

    void DeliteRuleButton(int a, Button button)
    {
        button.onClick.AddListener(() => DeliteRule(a));
    }

    void NewStatSelector()
    {
        Ui.AddStatButton.onClick.RemoveAllListeners();
        Ui.AddStatButton.onClick.AddListener(() => LoadStatSelector(cardBase.Stat.Count));
    }
    void NewRuleSelector()
    {
        Ui.AddRuleButton.onClick.RemoveAllListeners();
        Ui.AddRuleButton.onClick.AddListener(() => LoadRuleSelector(cardBase.Trait.Count));
    }

    void CreateSystemButton()
    {
        Ui.StatUi = new List<StatCaseUi>();

        Ui.RaceButton.onClick.AddListener(() => LoadSelector("Race"));
        Ui.LegionButton.onClick.AddListener(() => LoadSelector("Legion"));
        Ui.CivilianButton.onClick.AddListener(() => LoadSelector("Civilian"));

        NewStatSelector();
        NewRuleSelector();
        //Ui.StatUi[0].ButtonSwitch.enabled = false;

        //Ui.RuleButtons
        //for (int i = 0; i < gameSetting.RuleSize; i++)
        //{
        //    GO = Instantiate(Ui.OrigStat);
        //    GO.transform.SetParent(Ui.TraitCard);

        //    //  GO.GetComponent<Image>().sprite = gameSetting.Library.Guilds[i].Icon;

        //    StatCaseUi caseUi = GO.GetComponent<StatCaseUi>();

        //    Ui.RuleUi.Add(caseUi);

        //    SetRemoveRuleButton(caseUi.ButtonMinus, i);
        //    //AddStatButton(false, caseUi.ButtonMinus, i);
        //    //AddStatButton(true, caseUi.ButtonPlus, i);
        //    // caseUi.ButtonSwitch.onClick.AddListener(() => LoadStatSelector(a);
        //    SetSelectorRuleButton(i, caseUi.ButtonSwitch);
        //    //  Ui.StatButtons[i].Name =
        //}
        //Ui.RuleUi[0].ButtonSwitch.enabled = false;

    }
    void SetSelectorStatButton(int a, Button button)
    {
        button.onClick.AddListener(() => LoadStatSelector(a));
    }

    void SetSwitchStatButton(int a, Button button, int b)
    {
        button.onClick.AddListener(() => SwitchStat(a, b));
    }

    void LoadStatSelector(int a)
    {

        if (selector != null)
            Destroy(selector.gameObject);

        GameObject GO = Instantiate(Ui.SelectorOrigin);
        selector = GO.transform;
        selector.SetParent(Ui.SelectorOrigin.transform);
        List<Constant> actualConstant = new List<Constant>();
        //gameSetting.Library.Constants.Count;
        {
            List<Constant> oldConstant = gameSetting.Library.Constants;
            for (int i = 0; i < oldConstant.Count; i++)
            {
                actualConstant.Add(oldConstant[i]);
            }


            string constName = "";
            int b = 0;
            for (int i = 0; i < cardBase.Stat.Count; i++)
            {
                if (cardBase.Stat[i] != null)
                {
                    constName = cardBase.Stat[i].Name;
                    b = oldConstant.FindIndex(x => x.Name == constName);
                    if (b > -1)
                        actualConstant[b] = null;
                }
            }
        }

        for (int i = 0; i < actualConstant.Count; i++)
        {
            if (actualConstant[i] != null)
            {
                GO = Instantiate(Ui.OrigButton);
                GO.transform.SetParent(selector);
                GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = actualConstant[i].Name;

                SetSwitchStatButton(a, GO.GetComponent<Button>(), i);
            }
        }
    }

    //rule

    void SwitchRule(string str)
    {
        int a = keyRule;
        if (a == -1)
            return;
        Ui.RuleSelectorMain.active = false;
        //Debug.Log(a);

        if (a >= cardBase.Trait.Count)
        {
            cardBase.Trait.Add(gameSetting.Rule.Find(x => x.Name == str));
            cardBase.TraitSize.Add(0);
            AddRuleUi();
        }

        StatCaseUi caseUi = Ui.RuleUi[a];
        //int b = gameSetting.Rule.Find(x => x.Name == str);
        cardBase.Trait[a] = gameSetting.Rule.Find(x => x.Name == str);
        //  cardBase.StatSize[a] = 0;
        caseUi.Name.text = str;
        ReCalculate();

        keyRule = -1;
    }

    //void SetRemoveRuleButton(Button button, int a)
    //{
    //    button.onClick.AddListener(() => PreSwitchRule(a));
    //}
    //void PreSwitchRule( int a)
    //{
    //    keyRule = a;
    //    SwitchRule("-1");
    //}



    void SetSelectorRuleButton(int a, Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => LoadRuleSelector(a));
    }

    void SetSwitchRuleButton(Button button, string srt)
    {
        //button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => SwitchRule(srt));
    }

    void CreateRuleList()
    {
        //cardBase.Trait = new List<string>();
        //for (int i = 0; i < gameSetting.RuleSize; i++)
        //{
        //    keyRule = i;
        //    SwitchRule("-1");
        //    //cardBase.Trait.Add("");
        //    //Ui.RuleUi.Name = "";
        //    //SwitchStat(i, -1);
        //}
        //keyRule = -1;
        ruleList = new List<string>();
        /*
         расширенный метод, для исключеняи лишних - недоступных компонентов
         
         */
        for(int i =0; i < gameSetting.Library.Rule.Count; i++)
        {
            ruleList.Add(gameSetting.Library.Rule[i].Name);
        }

        int a = Ui.RuleSelector.childCount;
        if(a > ruleList.Count)
        {
            for (int i = a - ruleList.Count; i > 0; i--)
                Destroy(Ui.RuleSelector.GetChild(0).gameObject);
        }
        else if (a < ruleList.Count)
        {
            GameObject GO = null;
            for (int i = a; i <ruleList.Count; i++)
            {
                GO = Instantiate(Ui.OrigButton);
                GO.transform.SetParent(Ui.RuleSelector);
            }
        }

        for(int i =0; i< ruleList.Count; i++)
        {
            Ui.RuleSelector.GetChild(i).GetChild(0).GetComponent<Text>().text = ruleList[i];
            SetSwitchRuleButton(Ui.RuleSelector.GetChild(i).gameObject.GetComponent<Button>(), ruleList[i]);
        }





    }

    void LoadRuleSelector(int a)
    {
        keyRule = a;
        Ui.RuleSelectorMain.active = true;

    }


    #endregion

    #region SwitchGuild

    void OpenGuildSelector()
    {
        Ui.GuildSelector.active = true;
    }

    void LoadGuildSelector()
    {
        GameObject GO = null;


        for (int i = 0; i < gameSetting.Library.Guilds.Count; i++)
        {
            GO = Instantiate(Ui.OrigButtonBanner);
            GO.transform.SetParent(Ui.GuildSelector.transform);

            GO.GetComponent<Image>().sprite = gameSetting.Library.Guilds[i].Icon;
            SetSwitchGuildButton(i, GO.GetComponent<Button>());
        }
    }

    void SetSwitchGuildButton( int a, Button button)
    {
        button.onClick.AddListener(() => SwitchGuild(a));
    }

    void SwitchGuild(int i)
    {
        Ui.GuildSelector.active = false;
        curentGuild = gameSetting.Library.Guilds[i];
        Ui.GuildBanner.sprite = curentGuild.Icon;

        ReLoadCard();


    }
    #endregion
    void ReLoadCard()
    {

        cardBase = new CardBase();
        cardBase.Stat = new List<Constant>();
        cardBase.StatSize = new List<int>();
        //for (int i =0; i < gameSetting.StatSize; i++)
        //{
        //    cardBase.Stat.Add(null);
        //    cardBase.StatSize.Add(0);
        //    SwitchStat(i, -1);
        //    Ui.StatUi[0].ButtonSwitch.enabled = false;
        //}

        cardBase.Guilds = curentGuild;
        actualLegion = new List<Legion>();

        cardBase.Name = "New Hiro";
        SwitchRace(curentGuild.Races[0]);




        cardBase.TraitSize = new List<int>();
        cardBase.Trait = new List<HeadSimpleTrigger>();
        //for (int i = 0; i < gameSetting.RuleSize; i++)
        //{
        //    cardBase.Trait.Add(null);
        //    cardBase.TraitSize.Add(0);
        //    //SwitchStat(i, -1);
        //}
    }
    void SwitchRace(Race race)
    {
        cardBase.Races = race;


        int a = 0;
        string statName = "";

        if (race.MainRace != null)
            statName = race.MainRace.MainStat.Name;
        else
            statName = race.MainStat.Name;

        a = gameSetting.Library.Constants.FindIndex(x => x.Name == statName);
        if (a > -1)
            SwitchStat(0, a);





        foreach (Legion legion in curentGuild.Legions)
        {
            a = cardBase.Races.Legions.FindIndex(x => x.Name == legion.Name);
            if (a >= 0)
                actualLegion.Add(legion);
        }


        Ui.RaceText.text = race.Name;

        if (actualLegion.Count > 0)
            SwitchLegion(actualLegion[0]);
    }
    void SwitchLegion(Legion legion)
    {
        cardBase.Legions = legion;
        Ui.LegionText.text = legion.Name;

        if (legion.CivilianGroups.Count > 0)
            SwitchCivilian(legion.CivilianGroups[0]);
    }

    void SwitchCivilian(CivilianGroup civilian)
    {
        cardBase.CivilianGroups = civilian;
        Ui.CivilianText.text = civilian.Name;

        Ui.NameFlied.text = cardBase.Name;
        ReCalculate();
    }

    void LoadSelector(string data)
    {
        int sizeData =0;
        switch (data)
        {
            case("Race"):
                sizeData = curentGuild.Races.Count;
                break;

            case ("Legion"):
                sizeData = actualLegion.Count;
                break;

            case ("Civilian"):
                sizeData = cardBase.Legions.CivilianGroups.Count;
                break;
        }
        if(selector != null)
            Destroy(selector.gameObject);

        GameObject GO = Instantiate(Ui.SelectorOrigin);
        selector = GO.transform;
        selector.SetParent(Ui.SelectorOrigin.transform);


        //gameSetting.Library.Legions.Count;
        for (int i =0; i< sizeData;  i++)
        {
            GO = Instantiate(Ui.OrigButton);
            GO.transform.SetParent(selector);
            Text text = GO.transform.GetChild(0).gameObject.GetComponent<Text>();
            switch (data)
            {
                case ("Race"):
                    text.text = curentGuild.Races[i].Name;
                    break;

                case ("Legion"):
                    text.text = actualLegion[i].Name;
                    break;

                case ("Civilian"):
                    text.text = cardBase.Legions.CivilianGroups[i].Name;
                    break;
            }
            SetSwitchButton(i, GO.GetComponent<Button>(), data);
        }
    }

    void SetSwitchButton(int a, Button button, string data)
    {
        switch (data)
        {
            case ("Race"):
                button.onClick.AddListener(() => SwitchRace(curentGuild.Races[a]));
                break;

            case ("Legion"):
                button.onClick.AddListener(() => SwitchLegion(actualLegion[a]));
                break;

            case ("Civilian"):
                button.onClick.AddListener(() => SwitchCivilian(cardBase.Legions.CivilianGroups[a]));
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateCore();

        PreLoad();

        //LoadRuleSelector(0);


        // CreateStatButton();
        Inject();

        CreateRuleList();
        //LoadRuleSelector(0);

        //GenerateFiltr();

    }

    void CreateCore()
    {
        XMLSaver.SetGameSetting(gameSetting);
        XMLSaver.LoadMainRule(gameSetting.Library);


        CreateSystemButton();


        // gameSetting.library.Ru
        SwitchGuild(0);

        LoadGuildSelector();

        Ui.GuildButton.onClick.AddListener(() => OpenGuildSelector());
    }
}
