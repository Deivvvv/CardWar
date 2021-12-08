using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Saver;


public class ColodConstructor : MonoBehaviour
{
    private GameSetting gameSetting;
    private GameData gameData;
    private ColodConstructorUi Ui;

    private int allCard;
    private int maxCard = 40;

    private string origPath = $"/Resources/Hiro";
    public List<CardBase> LocalCard;

    // Start is called before the first frame update
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
            //  XMLSaver.ILoad(i, colodConstructor);
        }
    }
    void NewCard(int i)
    {
        GameObject GO = Instantiate(Ui.OrigCard);// BaseCard.GetChild(a + 1).gameObject;
        GO.transform.SetParent(Ui.BaseCard);

        int a = Ui.BaseCard.childCount;
        Button button = GO.GetComponent<Button>();
        AddCardButton(a, button);

       // GO.GetComponent<Image>().color = Ui.SelectColor[1];

        LocalCard[i].Body = GO.transform;
        //  LocalCard.Add(new CardBase());
        //  Save();

       // ViewCardBase(i);
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
}
