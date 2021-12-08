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


    void Start()
    {
        LoadBase();
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
    }
    void NewCard(int i)
    {
        GameObject GO = Instantiate(Ui.OrigCard);
        GO.transform.SetParent(Ui.BaseCard);

        int a = Ui.BaseCard.childCount;
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

    }

    void RemoveCardButton(int a, Button button)
    {
        button.onClick.AddListener(() => RemoveCard(a));
    }
    void RemoveCard(int a)
    {

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
}
