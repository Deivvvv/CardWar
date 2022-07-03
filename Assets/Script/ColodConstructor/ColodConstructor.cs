using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BattleTable;


public class ColodConstructor : MonoBehaviour
{
    //[SerializeField]
    //private GameSetting gameSetting;
    //[SerializeField]
    //private RuleMainFrame frame;
    //[SerializeField]
    //private ColodConstructorUi Ui;


    //private List<string> colod;
    //private string nameGuild;
    //private int maxCard = 40;

    //private CardSet cardSet;
    //private List<CardBase> card;
    //private List<Transform> body;
    //private int mod;

    //void Start()
    //{
    //    XMLSaver.SetGameSetting(gameSetting);
    //    Core.LoadGameSetting(gameSetting);
    //    gameSetting.CardBody = Ui.GBody;
    //    body = Ui.Body;
    //    DataManager.GenerateKey(frame, gameSetting.Library);
    //    SetGuild(gameSetting.ActualGuild);

    //    CardView.CardListLite(card, cardSet.Size, body, mod);
    //    SetButton();       // Calculation();
    //}
    //void SetGuild(int a)
    //{
    //    gameSetting.ActualGuild = a;
    //    nameGuild = gameSetting.Library.Guilds[a].Name;
    //    colod = XMLSaver.LoadCardSets(nameGuild);
    //    cardSet = XMLSaver.LoadCardSet(nameGuild+"/"+ gameSetting.ActualColod);
    //    Ui.NameFlied.text = cardSet.Name; 

    //    card = new List<CardBase>();
    //    string[] com;
    //    for (int i = 0; i < cardSet.Path.Count; i++)
    //    {
    //        com = cardSet.Path[i].Split('_');
    //        card.Add( XMLSaver.Load(gameSetting.GameDataFile.Data[int.Parse(com[0])].MasterKey + com[1]));
    //    }

    //}


    //void SetButton()
    //{
    //    Ui.SaveButton.onClick.AddListener(() => Save());

    //    Ui.GUp.onClick.AddListener(() => CardView.Mod(true));
    //    Ui.GDown.onClick.AddListener(() => CardView.Mod(false));
    //    Ui.CUp.onClick.AddListener(() => Mod(true));
    //    Ui.CDown.onClick.AddListener(() => Mod(false));

    //    for(int i =0;i<Ui.GBody.Count;i++)
    //        AddCardButton(i, Ui.GBody[i].gameObject.GetComponent<Button>());

    //    for (int i = 0; i < body.Count; i++)
    //        RemoveCardButton(i, body[i].gameObject.GetComponent<Button>());
    //}

    //void Mod(bool up)
    //{
    //    int oldMod = mod;
    //    if (up)
    //    {
    //        if((mod+1) * body.Count < card.Count)
    //            mod++;
    //    }else if(mod > 0)
    //        mod--;

    //    if(mod != oldMod)
    //    {
    //        int a;
    //        CardView.CardListLite(card, cardSet.Size, body, mod);
    //        //for (int i = 0; i < body.Count; i++)
    //        //{
    //        //    a = mod * body.Count + i;
    //        //    if (a < localCard.Count)
    //        //    {
    //        //        RemoveCardButton(a, body[i].gameObject.GetComponent<Button>());
    //        //    }
    //        //    else
    //        //        RemoveButton(body[i].gameObject.GetComponent<Button>());
    //        //}
    //    }
    //}
    //void Save()
    //{
    //    cardSet.Name = Ui.NameFlied.text;
    //    XMLSaver.SaveCardSet(cardSet, nameGuild + "/" + cardSet.Name);
    //    XMLSaver.SaveCardSets(colod, nameGuild);
    //}

    //void Load(int a)
    //{
    //    cardSet = XMLSaver.LoadCardSet(nameGuild + "/" + colod[a]);
    //    Ui.NameFlied.text = cardSet.Name;

    //    card = new List<CardBase>();
    //    string[] com;
    //    for (int i = 0; i < cardSet.Path.Count; i++)
    //    {
    //        com = cardSet.Path[i].Split('_');
    //        card.Add(XMLSaver.Load(gameSetting.GameDataFile.Data[int.Parse(com[0])].MasterKey + com[1]));
    //    }
    //}   
    ////void SetNewSet()
    ////{
    ////    ClearTable();
    ////    origCard = new List<int>();
    ////    origCount = new List<int>();
    ////    blackList = new List<int>();
    ////    origTrans = new List<Transform>();
    ////    int a = cardSet.OrigCard.Count;
    ////    int b = 0;
    ////    for(int i = 0; i < a; i++)
    ////    {
    ////        b = cardSet.OrigCard[i];
    ////        origCard.Add(b);
    ////        origCount.Add(cardSet.OrigCount[i]);
    ////        AddCardTable(b);

    ////    }
    ////}

    ////void NewCard(int i)
    ////{
    ////    int a = Ui.BaseCard.childCount;
    ////    GameObject GO = Instantiate(Ui.OrigCard);
    ////    GO.transform.SetParent(Ui.BaseCard);

    ////    Button button = GO.GetComponent<Button>();
    ////    AddCardButton(a, button);

    ////   // GO.GetComponent<Image>().color = Ui.SelectColor[1];

    ////    LocalCard[i].Body = GO.transform;
    ////    //  LocalCard.Add(new CardBase());
    ////    //  Save();

    ////    ViewCardBase(i);
    ////}
    ////void AddCardButton(int a, Button button)
    ////{
    ////    button.onClick.AddListener(() => AddCard(a));
    ////}
    ////void AddCard(int a)
    ////{
    ////    if (allCard < maxCard)
    ////    {

    ////        int b = origCard.Count;
    ////        for (int i = 0; i < b; i++)
    ////        {
    ////            if (a == origCard[i])
    ////            {
    ////                if (origCount[i] < 3)
    ////                {
    ////                    if (origCount[i] == 0)
    ////                        origTrans[i].gameObject.active = true;

    ////                    origCount[i]++;
    ////                    Calculation();
    ////                   // ViewCardTable(i);
    ////                }

    ////                return;
    ////            }

    ////        }


    ////        if (blackList.Count > 0)
    ////        {
    ////            b = blackList[0];
    ////            blackList.RemoveAt(0);
    ////            origCard[b] = a;
    ////            origCount[b] = 1;

    ////            origTrans[b].gameObject.active = true;
    ////          //  ViewCardTable(b);
    ////        }
    ////        else
    ////        {
    ////            origCard.Add(a);
    ////            origCount.Add(1);
    ////            AddCardTable(a);
    ////        }


    ////        // ViewCardTable(b);
    ////        Calculation();
    ////    }
    ////   // NewCardTable(b);
    ////}
    ////void AddCardTable(int i)
    ////{
    ////    int a = Ui.DeskCard.childCount;

    ////    GameObject GO = Instantiate(Ui.OrigCardColod);
    ////    GO.transform.SetParent(Ui.DeskCard);

    ////    origTrans.Add(GO.transform);

    ////    Button button = GO.GetComponent<Button>();
    ////    RemoveCardButton(a, button);

    ////   // ViewCardTable(a);
    ////    // GO.GetComponent<Image>().color = Ui.SelectColor[1];

    ////    // LocalCard[i].Body = GO.transform;
    ////}

    ////void Calculation()
    ////{
    ////    int a = origCard.Count;
    ////    allCard = 0;

    ////    for(int i = 0; i < a; i++)
    ////    {
    ////        allCard += origCount[i];
    ////    }

    ////    Ui.CardCount.text = $"{allCard}/{maxCard}";
    ////}

    //void RemoveButton(Button button) { button.onClick.RemoveAllListeners(); }
    //void RemoveCardButton(int a, Button button)
    //{
    //    button.onClick.RemoveAllListeners();
    //    button.onClick.AddListener(() => RemoveCard(a));
    //}
    //void AddCardButton(int a, Button button)  {  button.onClick.AddListener(() => AddCard(a)); }

    //void RemoveCard(int i)
    //{
    //    int a = mod * body.Count + i; 
    //    if (a < card.Count)
    //    {
    //        if (cardSet.Size[a] == 1)
    //        {
    //            card.RemoveAt(a);
    //            cardSet.Path.RemoveAt(a);
    //            cardSet.Size.RemoveAt(a);
    //        }
    //        else
    //            cardSet.Size[a]--;
    //        CardView.CardListLite(card, cardSet.Size, body, mod);
    //    }

    //}
    //void AddCard(int a)
    //{
    //    string path = gameSetting.AllCardPath[gameSetting.AllCard[a].Id];
    //    int b = cardSet.Path.FindIndex(x => x == path);
    //    if(b != -1)
    //    {
    //        if (cardSet.Size[b] < 3)
    //            cardSet.Size[b]++;
    //    }
    //    else
    //    {
    //        card.Add(gameSetting.AllCard[a]);
    //        cardSet.Path.Add(path);
    //        cardSet.Size.Add(1);
    //    }
    //    CardView.CardListLite(card, cardSet.Size, body, mod);
    //}



}
