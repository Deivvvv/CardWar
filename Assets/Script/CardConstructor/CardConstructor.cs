using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardConstructor : MonoBehaviour
{
    private CardBase cardBase;
    private List<CardBase> LocalCard;

    public CardConstructorUi Ui;


    void LoadBase()
    {
        //подключаем общую библиотеку по фракции
    }
    void LoadUi()
    {
        GameObject GO = null;
        Ui.StatCount = new Text[cardBase.Stat.Length - 1];
        for (int i=0;i < cardBase.Stat.Length - 1; i++)
        {
            GO = Instantiate(Ui.OrigStat);
            GO.transform.SetParent(Ui.StatCard);

            GO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Ui.Icon[i];
            GO.transform.GetChild(1).gameObject.GetComponent<Text>().text = $"{Ui.SellCount[i]}/4";
            GO.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = $"{Ui.StatName[i]}";
          
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
        if(plus)
            button.onClick.AddListener(() => StatUp(a));
        else
            button.onClick.AddListener(() => StatDown(a));
    }

    void StatUp(int a)
    {
        if (cardBase.Stat[12] < 11)
        {
            cardBase.Stat[a]++;

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
        for(int i =0; i< cardBase.Stat.Length-1; i++)
        {
            a += cardBase.Stat[i] * Ui.SellCount[i];
        }
        cardBase.Stat[cardBase.Stat.Length - 1] = Mathf.CeilToInt(a / 4f);
        ViewCard();
        //Debug.Log(a);
    }

    void NewCard()
    {
        cardBase = new CardBase();
        cardBase.Name = "New Hiro";
        cardBase.Stat = new int[13];
        cardBase.Trait = new string[5];

        cardBase.Stat[4] = 1;

    }
    void ViewCard()
    {
        Ui.NameFlied.text = cardBase.Name;
        for (int i = 0; i < cardBase.Stat.Length - 1; i++)
        {
            Ui.StatCount[i].text = $"{cardBase.Stat[i]}";
        }
        Ui.ManaCount.text = $"Цена: {cardBase.Stat[cardBase.Stat.Length-1]}";
        // Ui.
    }
    void ViewCardBase()
    {
      //  Ui.NameFlied.text = cardBase.Name;
    }

    // Start is called before the first frame update
    void Start()
    {
        NewCard();
        LoadUi();
        ViewCard();
    }


}
