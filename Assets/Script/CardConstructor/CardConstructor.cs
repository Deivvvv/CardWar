using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

using UnityEngine.UI;
using TMPro;
using XMLSaver;
using Coder;


public class CardConstructor : MonoBehaviour
{
    /*
     кнопки для перехода галерею
    сохранить
     
     */
    string sysMood = " ";
    void SetSysMood(string str) { if (sysMood == " ") sysMood = str; }

    CoreSys core;
    Accses mainAccses = new Accses();
    Accses compliteAccses;
    CardCase card;

    CardConstructorUI ui;
    bool edit;
    string exitMood;
    List<int> key;
    List<int> publicGuildList, publicCardTaypList, publicCardClassList, publicLegionList, publicCivilianList, publicRaceList, publicStatList, publicRuleHeadList;
    List<int> cardTaypList = new List<int>(), legionList = new List<int>(), civilianList = new List<int>(), raceList = new List<int>(), statList, ruleHeadList;
    int intGuild = -1, intCardTayp = -1, intCardClass = 0, intId = -1, intLegion = -1, intCivilian = -1, intRace = -1;
    int startMana;

    CardConstructor cardSys;
    string[] localKey = { "CardClass", "Guild", "CardTayp", "Legion", "Race", "Civilian" };
    void StartData()
    {
#if UNITY_EDITOR
        SetSysMood("Editor");
#endif
#if UNITY_STANDALONE
        SetSysMood("PC");
#endif
#if UNITY_ANDROID
        SetSysMood("Android");
#endif

        ui = gameObject.GetComponent<CardConstructorUI>();
        //ui.SaveButton.onClick.AddListener(() => SaveButton());

        ui.SaveWindow.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => Save(true));
        ui.SaveWindow.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => Save(false));
        ui.StartWindow.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => SetCardMain());

        ui.ButtonList[0].onClick.AddListener(() => OpenStatWindow());
        ui.ButtonList[1].onClick.AddListener(() => OpenRuleWindow());
        ui.ButtonList[2].onClick.AddListener(() => Reset());
        ui.ButtonList[3].onClick.AddListener(() => SaveButton());
        ui.ButtonList[4].onClick.AddListener(() => FullReset());

        SetPublicList();

        key = new List<int>();
        key.Add(core.keyCardClass);
        key.Add(core.keyGuild);
        key.Add(core.keyCardTayp);
        key.Add(core.keyLegion);
        key.Add(core.keyRace);
        key.Add(core.keyCivilian);
        key.Add(core.keyTag);
        key.Add(core.keyStat);

        GameObject go;
        cardSys = gameObject.GetComponent<CardConstructor>();
        for (int i = 0; i < localKey.Length; i++)
        {
            go = Instantiate(ui.OrigRing);
            go.transform.SetParent(ui.StartWindow.transform.GetChild(0));
            ui.MainListInfo.Add(go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>());
            go.transform.GetChild(0).gameObject.GetComponent<CardCaseInfo>().Set(cardSys, i, -1,-1,"full");
            ui.MainList.Add(go.transform.GetChild(1).GetChild(0).GetChild(0));
            ui.MainListInfo[i].text = localKey[i];
        }
        if (sysMood != "Editor")
            ui.StartWindow.transform.GetChild(1).GetChild(0).gameObject.active = false;


        SetStat(0, publicCardClassList[0]);

        LoadList(0);

    }
    void SaveButton()
    {
        if (ui.StartWindow.active == false)
            ui.SaveWindow.active = true;
    }

    #region Fist Stage
    void SetPublicList()
    {
        List<int> Add(BD bD, bool full = false)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < bD.Base.Count; i++)
                if (!bD.Base[i].Look)
                    list.Add(i);
                else if (full)
                    list.Add(i);


            return list;
        }
        if (sysMood == "Editor")
        {
            publicGuildList = Add(core.bD[core.keyGuild], true);
            publicCardClassList = Add(core.bD[core.keyCardClass], true);
        }
        else
        {
            publicGuildList = Add(core.bD[core.keyGuild]);
            publicCardClassList = Add(core.bD[core.keyCardClass]);
        }


        publicCardTaypList = Add(core.bD[core.keyCardTayp]);
        publicCivilianList = Add(core.bD[core.keyCivilian]);
        publicLegionList = Add(core.bD[core.keyLegion]);
        publicRaceList = Add(core.bD[core.keyRace]);
    }

    int SetList(int mood)
    {
        ResetAccses();
        //Debug.Log(mood);
        if (mood >= localKey.Length)
            return -1;

        List<int> list = new List<int>();
        int a = mainAccses.Find(mainAccses.Need, key[mood], false);
        if (a != -1)
        {
            if (mainAccses.Need[a].Num.Count > 1)
            {
                for (int i = 0; i < mainAccses.Need[a].Num.Count; i++)
                    list.Add(mainAccses.Need[a].Num[i].Head);
            }
            else if (mainAccses.Need[a].Num.Count == 1)
            {
                list.Add(mainAccses.Need[a].Num[0].Head);
            }
        }
        else
        {
            List<int> oldList = new List<int>();
            switch (mood)
            {
                case (0):// ("CardClass"):
                    break;
                case (1):// ("Guild"):
                    oldList = publicGuildList;
                    break;
                case (2):// ("CardTayp"):
                    oldList = publicCardTaypList;
                    break;
                case (3):// ("intLegion"):
                    oldList = publicLegionList;
                    break;
                case (4):// ("intRace"):
                    oldList = publicRaceList;
                    break;
                case (5):// ("intCivilian"):
                    oldList = publicCivilianList;
                    break;
            }


            for (int i = 0; i < oldList.Count; i++)
                list.Add(oldList[i]);

            List<int> disList = mainAccses.ReturnAccses(localKey[mood]);
            for (int i = 0; i < disList.Count; i++)
                list.Remove(disList[i]);


            BD bd = core.bD[key[mood]];
            a = mainAccses.Find(mainAccses.Like, key[mood], false);
            if (a != -1)
            {
                // if(mainAccses.Like[a].Num.Count == 0)
                // {
                //    for (int i = 0; i <core.bD[key[mood]].Base.Count;i++)
                //        list.Add
                // }else
                if (key[mood] == core.keyRace)
                {
                    List<int> localList = new List<int>();
                    for (int i = 0; i < mainAccses.Like[a].Num.Count; i++)
                        localList.Add(mainAccses.Like[a].Num[i].Head);

                    List<int> oldId;
                    for (int i = 0; i < localList.Count; i++)
                    {
                        oldId = bd.Base[localList[i]].Race.UseRace;

                        for (int j = 0; j < oldId.Count; j++)
                            if (!bd.Base[oldId[j]].Look)
                                localList.Add(oldId[j]);
                    }


                    for (int i = 0; i < localList.Count; i++)
                        list.Add(localList[i]);
                }
                else
                    for (int i = 0; i < mainAccses.Like[a].Num.Count; i++)
                        list.Add(mainAccses.Like[a].Num[i].Head);

            }

            bool DisConnect(Accses accses, int key, int num)
            {
                int a = accses.Find(accses.Need, key, false);
                if (a != -1)
                {
                    int b = accses.Need[a].Find(num, false);
                    return (b == -1);
                }
                return false;
            }


            for (int i = 0; i < list.Count; i++)
            {
                Accses localAccses = bd.Base[list[i]].accses;
                //Guild ,Race ,Legion , Civilian, CardTayp, CardClass
                if (DisConnect(localAccses, core.keyGuild, intGuild)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyLegion, intLegion)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyRace, intRace)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyCivilian, intCivilian)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyCardTayp, intCardTayp)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyCardClass, intCardClass)) { list.RemoveAt(i); i--; continue; }
            }
            //b = mainAccses.Find(mainAccses.Need, key[mood], false);
            //if (b != -1)
            //{

            //}
        }

        switch (mood)
        {
            case (0):// ("CardClass"):
                break;
            case (1):// ("Guild"):
                     // oldList = publicGuildList;
                break;
            case (2):// ("CardTayp"):
                cardTaypList = list;
                break;
            case (3):// ("intLegion"):
                legionList = list;
                break;
            case (4):// ("intRace"):
                raceList = list;
                break;
            case (5):// ("intCivilian"):
                civilianList = list;
                break;
        }
        if (list.Count == 1) 
            return list[0];
        return -1;
    }

 
    #region SetMainData
    void ResetAccses()
    {
        //>> keyStat >> keyTag + keyRule >> keyLegion >>keyRace >> keyGuild >> keySocial >> keyCardTayp >> keyCardClass
        //intGuild, intCardTayp, intCardClass, intLegion, intCivilian, intRace
        void AddRace(int a)
        {
            if (a != -1)
            {
                mainAccses.Split(core.bD[core.keyRace].Base[a].accses);
                AddRace(core.bD[core.keyRace].Base[a].Race.MainRace);
            }
        }
        mainAccses = new Accses();


        if (intCardTayp != -1)
            mainAccses.Split(core.bD[core.keyCardTayp].Base[intCardTayp].accses);
        if (intCivilian != -1)
            mainAccses.Split(core.bD[core.keyCivilian].Base[intCivilian].accses);
        if (intRace != -1)
        {
            mainAccses.Split(core.bD[core.keyRace].Base[intRace].accses);
            AddRace(core.bD[core.keyRace].Base[intRace].Race.MainRace);
            int a = mainAccses.Find(mainAccses.Need,core.keyStat);
            mainAccses.Need[a].Find(core.bD[core.keyRace].Base[intRace].Race.MainStat);
        }
        if (intLegion != -1)
            mainAccses.Split(core.bD[core.keyLegion].Base[intLegion].accses);
        if (intGuild != -1)
            mainAccses.Split(core.bD[core.keyGuild].Base[intGuild].accses);
        if (intCardClass != -1)
            mainAccses.Split(core.bD[core.keyCardClass].Base[intCardClass].accses);

        mainAccses.AccsesComplite();
    }
    void LoadList(int a)
    {
        //void Clear(Transform window)
        //{
        //    for (int i = 0; i < window.childCount; i++)
        //        Destroy(window.GetChild(0).gameObject);
        //}
        void Connect(int mood, List<int> list, Transform window)
        {
            void AddButton(Button button, int mood, int a)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SetStat(mood, a));
            }
            //Debug.Log(window.childCount);
            BD bD = core.bD[key[mood]];
            GameObject go = null;

            //while (list.Count< window.childCount)
            //{
            //    Destroy(window.GetChild(list.Count).gameObject); 
            //}

            for (int j = window.childCount; j < list.Count;j++)
            {
                go = Instantiate(ui.OrigButton);
                go.transform.SetParent(window);
            }
            int c = window.childCount - list.Count;
            for (int i = 0; i < c; i++)
            {
                Destroy(window.GetChild(c - i).gameObject);
                // i--;
            }
            //Debug.Log(window.childCount);
            //Debug.Log("");
            //Debug.Log(list.Count);
            //Debug.Log(window.childCount);
            //Debug.Log(c);
            for (int i = 0; i < list.Count; i++)
            {
               // Debug.Log(bD.Base[list[i]].Name);
                //go = Instantiate(ui.OrigButton);
                //go.transform.SetParent(window);
                //Debug.Log(bD.Base[list[i]].Name);
                go = window.GetChild(i).gameObject;
                go.GetComponent<CardCaseInfo>().Set(cardSys, mood, i, -1, "full");
                go.transform.GetChild(0).GetComponent<Text>().text = bD.Base[list[i]].Name;

                AddButton(go.GetComponent<Button>(), mood, list[i]);
                //go.GetComponent<Button>().onClick.RemoveAllListeners();
                //a = list[i];
                //go.GetComponent<Button>().onClick.AddListener(() => SetMainStat(mood, a));

            }

            //int c = window.childCount - list.Count;
            //for (int i = 0; i < c; i++)
            //{
            //    Destroy(window.GetChild(0).gameObject);
            //    // i--;
            //}
            //Debug.Log(list.Count);
            //Debug.Log(window.childCount);
            //Debug.Log(window.childCount);
        }

        int keys = 0;
        List<int> list = null;
        switch (a)
        {
            case (0):// ("CardClass"):
                list = publicCardClassList;
                break;
            case (1):// ("Guild"):
                keys = intCardClass;
                list = publicGuildList;
                break;
            case (2):// ("CardTayp"):
                keys = intGuild;
                if (cardTaypList.Count > 0)
                    list = cardTaypList;
                break;
            case (3):// ("intLegion"):
                keys = intCardTayp;
                if (legionList.Count > 0)
                    list = legionList;
                break;
            case (4):// ("intRace"):
                keys = intLegion;
                if (raceList.Count > 0)
                    list = raceList;
                break;
            case (5):// ("intCivilian"):
                keys = intRace;
                if (civilianList.Count > 0)
                    list = civilianList;
                break;
        }
        if (keys == -1)
            list = null;
        ////Debug.Log(ui.MainList.Count);
        //Debug.Log(a);
        //Debug.Log(key[a]);
        //Debug.Log(localKey[a]);
        //Debug.Log(list);
        //if (list != null)
        //    Debug.Log(list.Count);
        //Debug.Log(core.bD[key[a]].Base.Count);
        if (list != null)
        //    Clear(ui.MainList[a]);
        //else
            Connect(a, list, ui.MainList[a]);
    }

    void Reset()  { ui.StartWindow.active = true; }
    void FullReset() { Reset();
        SetStat(1, -1);
    }
    void SetCardMain()
    {
        if (intGuild == -1 || intCardTayp == -1 || intCardClass == -1 || intLegion == -1 || intCivilian == -1 || intRace == -1)
            return;
        card = new CardCase(intGuild, intCardTayp, intCardClass, intId, intLegion, intCivilian, intRace);
        //Debug.Log(card.Guild);
        //Debug.Log(card.CardTayp); 
        //Debug.Log(card.CardClass);
        //Debug.Log(card.Id);
        //Debug.Log(card.Legion);
        //Debug.Log(card.Civilian);
        //Debug.Log(card.Race);

        int b;
        int a = mainAccses.Find(mainAccses.Need, core.keyStat,false);
        for (int i = 0; i < mainAccses.Need[a].Num.Count; i++) 
        {
            b = mainAccses.Need[a].Num[i].Head;
            card.Stat.Add(new StatExtend(b, core.bD[core.keyStat])); 
        }

        for (int j = 0; j < mainAccses.Need.Count; j++)
            if(mainAccses.Need[j].Head <0)
            { 
                b = mainAccses.Need[j].Head;
                a = mainAccses.Find(mainAccses.Need, b, false);
                SubInt sub = new SubInt(b);
                for (int i = 0; i < mainAccses.Need[a].Num.Count; i++)
                {
                    b = mainAccses.Need[a].Num[i].Head;
                    sub.Num.Add(new SubInt(b));
                }
                card.Trait.Add(sub);
            }

        ui.StartWindow.active = false;
        StartMana();
        SecondStageStart();
        //собрать итоговую информацию
    }

    void SetStat(int a, int b, bool full = true)
    {
        if(full)
            for (int i = a+1; i < ui.MainList.Count; i++)
                switch (i)
                {
                    case (0):// ("CardClass"):
                        intCardClass = -1;
                        break;
                    case (1):// ("Guild"):
                        intGuild = -1;
                        break;
                    case (2):// ("CardTayp"):
                        intCardTayp = -1;
                        break;
                    case (3):// ("intLegion"):
                        intLegion = -1;
                        intId = -1;
                        break;
                    case (4):// ("intRace"):
                        intRace = -1;
                        break;
                    case (5):// ("intCivilian"):
                        intCivilian = -1;
                        break;
                }
            

        void Clear(int intMood)
        {
            for (int a = intMood; a < 6; a++)
            {
                ui.MainListInfo[a].text = core.bD[key[a]].Name;

                for (int i = ui.MainList[a].childCount; i > 0; i--)
                    Destroy(ui.MainList[a].GetChild(i - 1).gameObject);
            }
        }

        if (a > 5)
            return;
        if (b != -1)
            ui.MainListInfo[a].text = core.bD[key[a]].Base[b].Name;
        else
            ui.MainListInfo[a].text = core.bD[key[a]].Name;
        switch (a)
        {
            case (0):// ("CardClass"):
                intCardClass = b;
                break;
            case (1):// ("Guild"):
                intGuild = b;
                break;
            case (2):// ("CardTayp"):
                intCardTayp = b;
                break;
            case (3):// ("intLegion"):
                intLegion = b;
                break;
            case (4):// ("intRace"):
                intRace = b;
                break;
            case (5):// ("intCivilian"):
                intCivilian = b;
                break;
        }
        if (b != -1)
        {
            b = SetList(a+1);
            if (b != -1)
            {
                SetStat(a + 1, b, false);
            }
            else
            {
                SetStat(a + 1, -1, false);
            }

            LoadList(a + 1);
            //LoadList(a);
        }
        else
            Clear(a + 1);
        //else
        //{
        //    SetStat(a + 1, -1, false);
        //    Clear(a + 1);
        //}
        //if (full) 
        //    ResetAccses();

    }



    #endregion

    void StartMana()
    {
        //int intGuild = -1, intCardTayp = -1, intCardClass = 0, intId = -1, intLegion = -1, intCivilian = -1, intRace = -1;


        startMana = 0;
       // Debug.Log(card.Guild);
        startMana += core.bD[core.keyGuild].Base[card.Guild].Cost;
       // Debug.Log(card.CardTayp);
        startMana += core.bD[core.keyCardTayp].Base[card.CardTayp].Cost;
       // Debug.Log(card.CardClass);
        startMana += core.bD[core.keyCardClass].Base[card.CardClass].Cost;
       // Debug.Log(card.Legion);
        startMana += core.bD[core.keyLegion].Base[card.Legion].Cost;
       // Debug.Log(card.Civilian);
        startMana += core.bD[core.keyCivilian].Base[card.Civilian].Cost;
      //  Debug.Log(card.Race);
        startMana += core.bD[core.keyRace].Base[card.Race].Cost;
        CountSize();
    }
    #endregion
    #region Second Stage
    bool statMood;
    void SecondStageStart()
    {
        statMood = true;
        ResetAccses();
        List<int> Add(BD bD)
        {
            bool DisConnect(Accses accses, int key, int num)
            {
                int a = accses.Find(accses.DisLike, key, false);
                if (a != -1)
                {
                    int b = accses.DisLike[a].Find(num, false);
                    return (b != -1);
                }
                return false;
            }

                List<int> list = new List<int>();
            for (int i = 0; i < bD.Base.Count; i++)
                if (!bD.Base[i].Look)
                    list.Add(i);


            for (int i = 0; i < list.Count; i++)
            {
                Accses localAccses = bD.Base[list[i]].accses;
                //Guild ,Race ,Legion , Civilian, CardTayp, CardClass
                if (DisConnect(localAccses, core.keyGuild, card.Guild)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyLegion, card.Legion)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyRace, card.Race)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyCivilian, card.Civilian)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyCardTayp, card.CardTayp)) { list.RemoveAt(i); i--; continue; }
                if (DisConnect(localAccses, core.keyCardClass, card.CardClass)) { list.RemoveAt(i); i--; continue; }
            }

            return list;
        }

        publicRuleHeadList = Add(core.bD[core.keyTag]);
        publicStatList = Add(core.bD[core.keyStat]);



        //GameObject go =null;
        //for (int i = 0; i < 2; i++)
        //{
        //    go = Instantiate(ui.OrigRing);
        //    go.transform.SetParent(ui.StatWindow);
        //    ui.StatRing.Add(go.transform.GetChild(1).GetChild(0).GetChild(0));

        //}
        //ui.StatWindow.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Добавленные статы";
        //go.transform.GetChild(0).gameObject.GetComponent<CardCaseInfo>().SetAlt(cardSys, -1, -1);
        ////Dectroy(go.transform.GetChild(0).gameObject.GetComponent<CardCaseInfo>());
        //ui.StatWindow.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "Доступные статы";
        //go.transform.GetChild(0).gameObject.GetComponent<CardCaseInfo>().SetAlt(cardSys, -2, -1);
        ////Dectroy(go.transform.GetChild(0).gameObject.GetComponent<CardCaseInfo>());
        //SetListStat();

        SetStatButton();
        SetRuleButton();


        FullAccsesCount();
        ui.StatWindow.gameObject.active = true;
        ui.RuleWindow.gameObject.active = false;

        //OpenStatWindow();

        //далее для механик


        ViewCard();
    }

    List<SubInt> listRule;
    Accses statAccses;
    Accses ruleAccses;

    void FullAccsesCount()
    {
        compliteAccses = new Accses();
        compliteAccses.SplitCard(card, mainAccses);
        if (statMood)
        {
            CountStatList();
        }
        else
        {
            CountRuleList();
        }

    }
    void ScanAccses(Accses accses, Accses fullAccses, SubInt sub, int root =0)
    {
        int a=-1,b = -1, c;
        for (int j = 0; j < accses.DisLike.Count; j++)
            if (accses.DisLike[j].Head == core.keyStat)
            {
                for (int k = 0; k < card.Stat.Count; k++)
                {
                    a = accses.DisLike[j].Find(card.Stat[k].Get("Stat"), false);
                    if (a != -1)
                    {
                        if(root == 0)
                            fullAccses.DisLike.Add(sub);
                        else
                        {
                            c = fullAccses.Find(fullAccses.DisLike, root);
                            fullAccses.DisLike[c].Num.Add(sub);
                        }
                        break;
                    }
                }

            }
            else if (accses.DisLike[j].Head < 0)
            {
                a = accses.Find(card.Trait, accses.DisLike[j].Head, false);
                if (a != -1)
                {
                    if (accses.DisLike[j].Num.Count != 0)
                    {
                        for (int k = 0; k < accses.DisLike[j].Num.Count; k++)
                        {
                            b = card.Trait[a].Find(accses.DisLike[j].Num[k].Head, false);
                            if (b != -1)
                            {
                                if (root == 0)
                                    fullAccses.DisLike.Add(sub);
                                else
                                {
                                    c = fullAccses.Find(fullAccses.DisLike, root);
                                    fullAccses.DisLike[c].Num.Add(sub);
                                }
                                break;
                            }
                        }

                        if (b == -1)
                            a = -1;
                        else
                            break;
                    }
                    else
                    {
                        if (root == 0)
                            fullAccses.DisLike.Add(sub);
                        else
                        {
                            c = fullAccses.Find(fullAccses.DisLike, root);
                            fullAccses.DisLike[c].Num.Add(sub);
                        }
                        break;
                    }

                }
            }
       // Debug.Log(a);
        if (a != -1)
            return;

        for (int j = 0; j < accses.Need.Count; j++)
            if (accses.Need[j].Head == core.keyStat)
            {
                for (int k = 0; k < card.Stat.Count; k++)
                {
                    a = accses.Need[j].Find(card.Stat[k].Get("Stat"), false);
                    if (a == -1)
                    {
                        if (root == 0)
                            fullAccses.Need.Add(sub);
                        else
                        {
                            c = fullAccses.Find(fullAccses.Need, root);
                            fullAccses.Need[c].Num.Add(sub);
                        }
                        break;
                    }
                }
                if (a == -1)
                    break;
            }
            else if (accses.Need[j].Head < 0)
            {
                a = accses.Find(card.Trait, accses.Need[j].Head, false);
                if (a != -1)
                {
                    if (accses.Need[j].Num.Count > 0)
                    {
                        for (int k = 0; k < accses.Need[j].Num.Count; k++)
                        {
                            b = card.Trait[a].Find(accses.Need[j].Num[k].Head, false);
                            if (b == -1)
                            {
                                if (root == 0)
                                    fullAccses.Need.Add(sub);
                                else
                                {
                                    c = fullAccses.Find(fullAccses.Need, root);
                                    fullAccses.Need[c].Num.Add(sub);
                                }
                                break;
                            }
                        }

                        if (b == -1)
                            a = -1;
                        else
                            break;
                    }
                    else
                    {
                        if (root == 0)
                            fullAccses.Need.Add(sub);
                        else
                        {
                            c = fullAccses.Find(fullAccses.Need, root);
                            fullAccses.Need[c].Num.Add(sub);
                        }
                        break;
                    }

                }
            }
        //Debug.Log(a);
        if (a == -1)
        {
            if (root == 0)
                fullAccses.Like.Add(sub);
            else
            {
                //Debug.Log(root);
                //Debug.Log(sub.Head);
                c = fullAccses.Find(fullAccses.Like, root);
                fullAccses.Like[c].Num.Add(sub);
            }
        }
    }

    void CountStatList()
    {
       // Debug.Log("Ok");
        List<int> listStat = new List<int>();
        for (int i = 0; i < publicStatList.Count; i++)
        {
            listStat.Add(publicStatList[i]);
            //  Debug.Log("Add Stat" + publicStatList[i]);

        }
        for (int i = 0; i < card.Stat.Count; i++)
        {
            listStat.Remove(card.Stat[i].Get("Stat"));
        }
            // listStat.Add(publicStatList[i]);

        int a, b, c;
        a = compliteAccses.Find(compliteAccses.Like, core.keyStat, false);
        if (a != -1)
            for (int i = 0; i < compliteAccses.Like[a].Num.Count; i++)
            {
                listStat.Add(publicStatList[i]);
               // Debug.Log("Add Stat" + publicStatList[i]);

            }
       // listStat.Add(compliteAccses.Like[a].Num[i].Head);
        

        a = compliteAccses.Find(compliteAccses.DisLike, core.keyStat, false);
        if (a != -1)
            for (int i = 0; i < compliteAccses.Like[a].Num.Count; i++)
            {
                listStat.Remove(compliteAccses.Like[a].Num[i].Head);
               // Debug.Log("Remove"+compliteAccses.Like[a].Num[i].Head);

            }
        //listStat.Remove(compliteAccses.Like[a].Num[i].Head);

        statAccses = new Accses();
        for (int i = 0; i < listStat.Count; i++)
        {
            SubInt sub = new SubInt(listStat[i]);
            Accses accses = core.bD[core.keyStat].Base[listStat[i]].accses;
            ScanAccses(accses, statAccses, sub);
           // Debug.Log("Remove" + listStat[i]);
        }
        //Debug.Log(statAccses.Like.Count);
        //Debug.Log(statAccses.Need.Count);
        //Debug.Log(statAccses.DisLike.Count);
        LoadStatButton();
    }

    void CountRuleList()
    {
        //Debug.Log("Ok");
        int a, b;
        int keyA = 0;
        List<SubInt> listRule = new List<SubInt>();
        for (int i = 0; i < publicRuleHeadList.Count; i++)
        {
            keyA = -publicRuleHeadList[i] - 1;
            SubInt sub = new SubInt(publicRuleHeadList[i]);
            for (int j = 0; j < core.head[keyA].Index.Count; i++)
                sub.Num.Add(new SubInt(core.head[keyA].Index[i]));
            listRule.Add(sub);
        }

        for (int i = 0; i < compliteAccses.Like.Count; i++)
            if (compliteAccses.Like[i].Head < 0)
            {
                a = compliteAccses.Find(listRule, compliteAccses.Like[i].Head);
                if (compliteAccses.Like[i].Num.Count == 0)
                {
                    for (int j = 0; j < core.head[-compliteAccses.Like[i].Head - 1].Index.Count; j++)
                        listRule[a].Find(core.head[-compliteAccses.Like[i].Head - 1].Index[j]);
                }
                else
                    for (int j = 0; j < compliteAccses.Like[i].Num.Count; j++)
                        listRule[a].Find(compliteAccses.Like[i].Num[j].Head);
            }


        for (int i = 0; i < card.Trait.Count; i++)
        {
            a = compliteAccses.Find(listRule, card.Trait[i].Head, false);
            if (a != -1)
                for (int j = 0; j < card.Trait[i].Num.Count; j++)
                {
                    b = listRule[a].Find(card.Trait[i].Num[j].Head, false);
                    if (b != -1)
                        listRule[a].Num.RemoveAt(b);
                }
        }


        for (int i = 0; i < compliteAccses.DisLike.Count; i++)
            if (compliteAccses.DisLike[i].Head < 0)
            {
                a = compliteAccses.Find(listRule, compliteAccses.DisLike[i].Head, false);
                if (a != -1)
                {
                    if (compliteAccses.DisLike[i].Num.Count == 0)
                    {

                        listRule.RemoveAt(a);
                        i--;
                    }
                    else
                    {
                        for (int j = 0; j < compliteAccses.DisLike[i].Num.Count; j++)
                        {
                            b = listRule[a].Find(compliteAccses.DisLike[i].Num[j].Head, false);
                            if (b != -1)
                            {
                                listRule.RemoveAt(b);
                                j--;
                            }

                        }
                        if (listRule[a].Num.Count == 0)
                        {
                            listRule.RemoveAt(a);
                            i--;
                        }
                    }

                }
            }

        ruleAccses = new Accses();
        for (int i = 0; i < listRule.Count; i++)
        {
            keyA = -listRule[i].Head - 1;
            SubInt sub = new SubInt(listRule[i].Head);
            Accses accses = core.bD[core.keyTag].Base[keyA].accses;
            ScanAccses(accses, ruleAccses, sub);
            for (int j = 0; j < listRule[i].Num.Count; j++)
            {
                //Debug.Log(listRule[i].Head);
                ScanAccses(accses, ruleAccses, listRule[i].Num[j], sub.Head);
            }
            
        }

        LoadRuleButton();
    }

    #region no specal metods

    void OpenStatWindow()
    {
        if (!statMood)
        {
            statMood = true;
            ui.StatWindow.gameObject.active = true;
            ui.RuleWindow.gameObject.active = false;
            CountStatList();
        }
    }

    void OpenRuleWindow()
    {
        if (statMood)
        {
            statMood = false;
            ui.StatWindow.gameObject.active = false;
            ui.RuleWindow.gameObject.active = true;
            CountRuleList();
        }
    }

    void SetStatButton()
    {
        GameObject CreateRing()
        {
            GameObject go = Instantiate(ui.OrigRing);
            go.transform.SetParent(ui.StatWindow.GetChild(0));


            go.transform.GetChild(0).GetComponent<CardCaseInfo>().Set(cardSys, ui.StatRing.Count, -1, -1, "statHead");
            ui.StatRing.Add(go.transform.GetChild(1));

            return go;
        }

        if (ui.StatRing.Count < 4)
        {
            string[] names = { "Активные статы", "Достпные статы", "Потеницально доступные статы", "Недоступные статы" };
            for (int i = 0; i < names.Length; i++)
            {
                GameObject go = CreateRing();
                go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = names[i];
                //SetMainRuleButton(i);
            }
        }
    }
    void SetRuleButton()
    {
        GameObject CreateRing()
        {
            GameObject go = Instantiate(ui.OrigRing);
            go.transform.SetParent(ui.RuleWindow.GetChild(0));

            go.transform.GetChild(0).GetComponent<CardCaseInfo>().Set(cardSys, ui.RuleRing.Count, -1, -1, "ruleHead");
            ui.RuleRing.Add(go.transform.GetChild(1));

            return go;
        }

        if (ui.RuleRing.Count < 4)
        {
            string[] names = { "Активные механики", "Достпные механики", "Потеницально доступные механики", "Недоступные механики" };

            for (int i = 0; i < names.Length; i++)
            {
                GameObject go = CreateRing();
                go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = names[i];
                //SetMainRuleButton(i);
            }
        }
    }
    void LoadStatButton()
    {
        void Build(int intMood)
        {
            void Clear(int intMood, int size)
            {
                for (int i = size; i < ui.StatRing[intMood].GetChild(0).GetChild(0).childCount; i++)
                {
                    Destroy(ui.StatRing[intMood].GetChild(0).GetChild(0).GetChild(i).gameObject);
                    //i--;
                }
            }
            void Build(int intMood, int i)
            {
                //Debug.Log(intMood);
                void SetStatButton(Button button, Button button1,int a)
                {
                    button.onClick.AddListener(() => EditStat(a, false));
                    button1.onClick.AddListener(() => EditStat(a, true));
                    //button.onClick.RemoveAllListeners();
                }

                void SetButton(Button button, int a)
                {
                    button.onClick.AddListener(() => SetStat(a));
                }
                GameObject go = null;
                if (intMood == 0)
                {
                    go = Instantiate(ui.OrigStatButton);
                    SetStatButton(go.transform.GetChild(1).gameObject.GetComponent<Button>(), go.transform.GetChild(2).gameObject.GetComponent<Button>(), i);
                }
                else
                {
                    go = Instantiate(ui.OrigButton);
                    if (intMood == 1)
                        SetButton(go.GetComponent<Button>(), i);
                }
                //Debug.Log(go);
                go.transform.SetParent(ui.StatRing[intMood].GetChild(0).GetChild(0));

                go.GetComponent<CardCaseInfo>().Set(cardSys, intMood, i, - 1, "Stat");
            }
            void SetInfo(int intMood, int i, List<SubInt> list)
            {
                //Debug.Log($"{i} {list[i].Head}");
                int b = list[i].Head;
                BD bd = core.bD[core.keyStat];
                string str = bd.Base[b].Name;
                str += $"\n{bd.Base[b].Cost}/4";
                ui.StatRing[intMood].GetChild(0).GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = str;

            }

            List<SubInt> list = null;
            switch (intMood)
            {
               // case (0):
                //    list = card.Stat;
                //    break;
                case (1):
                    list = statAccses.Like;
                    break;
                case (2):
                    list = statAccses.Need;
                    break;
                case (3):
                    list = statAccses.DisLike;
                    break;
            }
            //Debug.Log(card.Stat.Count);
            //Debug.Log(ui.StatRing[intMood].childCount);
            int i = 0;


            if (intMood == 0)
            {
                for (int j = ui.StatRing[intMood].GetChild(0).GetChild(0).childCount; j < card.Stat.Count; j++)
                    Build(intMood, j);
                for (; i < card.Stat.Count; i++)
                    StatView(i);
                //Clear(intMood, card.Stat.Count);
            }
            else
            {
                //Debug.Log(list.Count);
                for (int j = ui.StatRing[intMood].GetChild(0).GetChild(0).childCount; j < list.Count; j++)
                    Build(intMood, j);
                for (; i < list.Count; i++)
                    SetInfo(intMood, i, list);
                //Clear(intMood, list.Count);
            }
            Clear(intMood, i);
        }
        for (int i = 0; i < ui.StatRing.Count; i++)
            Build(i);
    }
    void LoadRuleButton()
    {
        void Build(int intMood)
        {
            void Clear(int intMood, int size)
            {

                for (int i = size; i < ui.RuleRing[intMood].GetChild(0).GetChild(0).childCount; i++)
                {
                    Destroy(ui.RuleRing[intMood].GetChild(0).GetChild(0).GetChild(i).gameObject);
                    //i--;
                }
            }
            void Build(int intMood,int i,int j, int size =-1)
            {
                void SetButton(Button button, int a, int b, int intMood)
                {
                    button.onClick.RemoveAllListeners();
                    switch (intMood)
                    {
                        case (0):
                            button.onClick.AddListener(() => RemoveRule(a, b));
                            //button.onClick.AddListener(() => CountSize());
                            break;
                        case (1):
                            button.onClick.AddListener(() => SetRule(a, b));
                            //button.onClick.AddListener(() => CountSize());
                            break;
                    }
                }

                //Debug.Log(ui.RuleRing[intMood].GetChild(0).GetChild(0));
                //Debug.Log(ui.RuleRing[intMood].GetChild(0).GetChild(0).childCount);
                //Debug.Log(size);
                GameObject go = null;
                if (size == -1)
                {
                    go = Instantiate(ui.OrigButton);
                    go.transform.SetParent(ui.RuleRing[intMood].GetChild(0).GetChild(0));
                }
                else
                    go = ui.RuleRing[intMood].GetChild(0).GetChild(0).GetChild(size).gameObject;

                go.GetComponent<CardCaseInfo>().Set(cardSys, intMood, i, j, "Rule");
                SetButton(go.GetComponent<Button>(), i, j, intMood);
            }
            void SetInfo(int intMood, int i, int j, List<SubInt> list,int size)
            {
                int mainA = -list[i].Head - 1;
                int mainB = list[i].Num[j].Head;
                //Debug.Log($"{i} {j} {mainA} {mainB}");

                string str = core.head[mainA].Rule[mainB];
                Text text = ui.RuleRing[intMood].GetChild(0).GetChild(0).GetChild(size).GetChild(0).gameObject.GetComponent<Text>();

                str += $"\nCost: {core.head[mainA].Cost[mainB]}";

                //Debug.Log(ui.RuleRing[intMood]);
                //Debug.Log(ui.RuleRing[intMood].GetChild(size));
                //Debug.Log(ui.RuleRing[intMood].GetChild(size).GetChild(0));
                //Debug.Log(text);
                text.text = str;
            }

            List<SubInt> list = null;
            switch (intMood) 
            {
                case (0):
                    list = card.Trait;
                    break;
                case (1):
                    list = ruleAccses.Like;
                    break;
                case (2):
                    list = ruleAccses.Need;
                    break;
                case (3):
                    list = ruleAccses.DisLike;
                    break;
            }

            int size = 0;
            for(int i=0;i< list.Count;i++)
                for (int j = 0; j < list[i].Num.Count; j++)
                {
                    //Debug.Log(ui.RuleRing[intMood].GetChild(0).GetChild(0));
                    //Debug.Log(ui.RuleRing[intMood].GetChild(0));

                    if (size > ui.RuleRing[intMood].GetChild(0).GetChild(0).childCount)
                        Build(intMood, i, j, size);
                    else
                        Build(intMood, i, j);

                    SetInfo(intMood, i, j, list, size);
                    size++;
                }
            Clear(intMood,size);
        }
        for (int i = 0; i < ui.StatRing.Count; i++)
            Build(i);
    }

    public void GetFullInfo()//вся статистика по текущему набору введенной информации
    {
        string str = "";
        ui.InfoPanel.text = str;
    }

    public void GetInfo(int mood, int a)//информация по наведенному узлу
    {
        string str = "";
        BD bd = core.bD[key[mood]];
        if (a == -1)
            switch (mood)
            {
                case (0):// ("CardClass"):
                    a = intCardClass;
                    break;
                case (1):// ("Guild"):
                    a = intGuild;
                    break;
                case (2):// ("CardTayp"):
                    a = intCardTayp;
                    break;
                case (3):// ("intLegion"):
                    a = intLegion;
                    break;
                case (4):// ("intRace"):
                    a = intRace;
                    break;
                case (5):// ("intCivilian"):
                    a = intCivilian;
                    break;
            }

        str = "Раздел: "+bd.Name;
        if (a == -1)
        {
            str += "\n" + bd.Info;
        }
        else
        {
            str += "\n" +bd.Base[a].Name;
            str += "\n" + bd.Base[a].Info;
            str += "\nЦена" + bd.Base[a].Cost;
            str += "\n" + DeCoder.AccsesText(bd.Base[a].accses,false);
        }

        ui.InfoPanel.text = str;
    }

    void SetStat(int a)
    {

        Debug.Log(card);
        BD bd = core.bD[core.keyStat];
        int b = statAccses.Like[a].Head;
        card.Stat.Add(new StatExtend(b, bd));
        CountSize();
        FullAccsesCount();
    }
    void EditStat(int a, bool add)
    {
        if (add)
            card.Stat[a].Edit("Max", 1);
        else if (card.Stat[a].Get("Max") ==1)
        {
            if (a != 0)
            {
                int b = compliteAccses.Find(compliteAccses.Need, core.keyStat, false);
                int c = compliteAccses.Need[b].Find(card.Stat[a].Get("Stat"), false);
                if (c == -1) {
                    card.Stat.RemoveAt(a);
                    FullAccsesCount();
                    CountSize();
                    return;
                } 
            }
        }
        else 
            card.Stat[a].Edit("Max", -1);
        StatView(a);
        CountSize();
    }

    void SetRule(int a, int b)
    {
        int mainA = ruleAccses.Like[a].Head;
        int mainB = ruleAccses.Like[a].Num[b].Head;

        int c = ruleAccses.Find(card.Trait, mainA);
        card.Trait[c].Find(mainB);
        CountSize();
        FullAccsesCount();
    }
    void RemoveRule(int a, int b)
    {
        int mainA = card.Trait[a].Head;
        int mainB = card.Trait[a].Num[b].Head;

        int d;
        int c = compliteAccses.Find(compliteAccses.Need, mainA, false);
        if(c!= -1)
        {
            d = compliteAccses.Need[c].Find(mainB);
            if (d != -1)
                return;
        }
        c = compliteAccses.Find(card.Trait, mainA, false);
        d = card.Trait[c].Find(mainB, false);
        card.Trait[c].Num.RemoveAt(d);
        if(card.Trait[c].Num.Count ==0)
            card.Trait.RemoveAt(c);

        FullAccsesCount();
        CountSize();
    }

    void StatView(int a)
    {
        int b = card.Stat[a].Get("Stat");

        BD bd = core.bD[core.keyStat];
        string str = bd.Base[b].Name;
        str += $"\n{bd.Base[b].Cost}/4 * {card.Stat[a].Get("Max")}";
        ui.StatRing[0].GetChild(0).GetChild(0).GetChild(a).GetChild(0).gameObject.GetComponent<Text>().text = str;
        //ui.StatRing[0].GetChild(a).GetChild(0).gameObject.GetComponent<Text>().text = str;
    }
    void CountSize()
    {
        float mana = startMana;
        int a, b;
        BD bd = core.bD[core.keyStat];
        for (int i = 0; i < card.Stat.Count; i++)
        {
            a = card.Stat[i].Get("Stat");
            b = card.Stat[i].Get("Max");
           // if (b <= 0)
          //  {
           //     RemoveStat(i);
           //     return;
          //  }
            mana += bd.Base[a].Cost * b;
        }

        bd = core.bD[core.keyTag];
        for (int i = 0; i < card.Trait.Count; i++)
        {
            //Debug.Log(bd);
            //Debug.Log(bd.Base.Count);
            //Debug.Log(-card.Trait[i].Head - 1);
            //Debug.Log(bd.Base[-card.Trait[i].Head - 1]);
            mana += bd.Base[-card.Trait[i].Head - 1].Cost;
            for (int j = 0; j < card.Trait[i].Num.Count; j++)
            {
                a = core.head[-card.Trait[i].Head - 1].Index.FindIndex(x => x == card.Trait[i].Num[j].Head);
                if (a != -1)
                    mana += core.head[-card.Trait[i].Head - 1].Cost[a];
            }
        }

        mana /= 4;
        card.Mana = Mathf.CeilToInt(mana);
        ViewCard();
    }

    void ViewCard()
    {
       SubSys.Gallery.ReadCard(card, ui.Body, false);
        //card
    }
    #endregion

    //#region Rule
    ////мусор
    //void SetMainRuleButton(int a)
    //{
    //    void Clear(int intMood, int size)
    //    {

    //        for (int i = size; i < ui.RuleRing[intMood].childCount; i++)
    //        {
    //            Destroy(ui.RuleRing[intMood].GetChild(size).gameObject);
    //            i--;
    //        }
    //    }


    //    void Build(int intMood, int size)
    //    {

    //        void SetButton(Button button, int a, int intMood)
    //        {
    //            switch (intMood)
    //            {
    //                case (1):
    //                    button.onClick.AddListener(() => RemoveRule(a));
    //                    //button.onClick.AddListener(() => CountSize());
    //                    break;
    //                case (1):
    //                    button.onClick.AddListener(() => SetMainRule(a));
    //                    //button.onClick.AddListener(() => CountSize());
    //                    break;
    //                case (2):
    //                    button.onClick.AddListener(() => SetRule(a));
    //                    break;
    //            }
    //        }

    //        for (int i = ui.RuleRing[intMood].childCount; i < size; i++)
    //        {
    //            GameObject go = Instantiate(ui.OrigButton);
    //            go.transform.SetParent(ui.RuleRing[intMood]);
    //            go.GetComponent<CardCaseInfo>().SetRule(cardSys, intMood, i);
    //            SetButton(go.transform.GetChild(1).gameObject.GetComponent<Button>(), i, intMood);
    //        }
    //    }

    //    List<SubInt> list = null;
    //    if (a == 0)
    //        list = card.Trait;
    //    else
    //        list = ruleList[a - 1];

    //    int size = 0;
    //    for (int i = 0; i < list.Count; i++)
    //        for (int j = 0; j < list[i].Num.Count; j++)
    //            size++;

    //    Clear(a, size);
    //    Build(a, size);

    //}

    

    //void SetListRule(Accses accses)
    //{
    //    localRuleList = new List<SubInt>();
    //    ruleListKey = new List<SubInt>();
    //    for (int i = 0; i < accses.Need.Count; i++) 
    //    {
    //        SubInt sub = new SubInt(accses.Need[i].Head);
    //        for (int j = 0; j < accses.Need[i].Count; j++)
    //            sub.Num.Add(new SubInt(accses.Need[i].Num[j].Head));
    //    }
    //       // for(int i)

    //}

    //void RemoveRule(int a, List<int> b, string log = "")
    //{
    //    int c;
    //    if (b == null)
    //    {
    //        c = card.Trait[a].Head;
    //        log += "\nRuleS" + core.bD[core.keyTag].Base[c].Name;
    //        card.Trait.RemoveAt(a);
    //    }
    //    else
    //        for (int i = 0; i < b.Count; i++)
    //        {
    //            log += "\nRule" + core.head[a].Rule[b[i]];
    //            c = card.Trait[a].Find(b[i], false);
    //            card.Trait[a].Num.RemoveAt(c);
    //        }
    //    FindConect(log);
    //}

    //void SetRule(int a,int b,int c)
    //{
    //    Accses accses = Saver.LoadRuleAccses(-ruleListKey[a].Num[b].Head - 1, ruleListKey[a].Num[b].Num[c].Head);
    //    int d = accses.Find(card.Trait, ruleListKey[a].Num[b].Head);
    //    card.Trait[d].Find(ruleListKey[a].Num[b].Num[c].Head);

    //    AddNeed(accses);
    //    if (ruleListKey.Count == 0)
    //    {
    //        FindConect();
    //    }

    //}
    //void SetListRule(int a ,int b)
    //{
    //    if (ruleListKey.Count > 0)
    //    {
    //        ruleListKey[a].Num[0].Head = b;

    //        bool preLoad = false;
    //        for (int i = 0; i < ruleListKey.Count && !preLoad; i++)
    //            preLoad = (ruleListKey[i].Num[0].Head == -1);

    //        if (preLoad)
    //            return;

    //        for(int i=0;i< ruleListKey.Count; i++)
    //            SetRule(i, ruleListKey[i].Head, ruleListKey[i].Num[0].Head);
            
    //        ruleListKey = new List<SubInt>();
    //        FindConect();
             
    //    }
    //    else
    //    {
    //        SubInt sub;
    //        ruleListKey = new List<SubInt>();
    //        SetListRule(Saver.LoadRuleAccses(-ruleListKey[1].Num[a].Head - 1, ruleListKey[1].Num[a].Num[b].Head));
    //        for(int i = 0; i < ruleList.Count; i++)
    //            if ( ruleList[i].Num.Count == 1)
    //            {
    //                sub = new SubInt(ruleList[i].Head);
    //                sub.Num.Add(new SubInt(ruleList[i].Num[0].Head));
    //                ruleListKey.Add(sub);
    //            }
    //            else if (ruleList[i].Num.Count == 0)
    //            {
    //                Debug.Log("Error load");
    //                ruleListKey = new List<SubInt>();
    //                SetListRule();
    //            }
    //            else
    //            {
    //                sub = new SubInt(ruleList[i].Head);
    //                sub.Num.Add(new SubInt(-1));
    //                ruleListKey.Add(sub);
    //            }
    //    }
    //}

    //void SetRuleButtonInfo(List<SubInt>list ,int a,int b, bool full = true)
    //{
    //    int mainA = -list[a].Head - 1;
    //    int mainB = list[a].Num[b].Head ;

    //    bool lost = false;
    //    List<string> sum = new List<string>();
    //    int maxSum =0;
    //    string str = core.head[mainA].Rule[mainB];
    //    Text text = ui.RuleRing[a].GetChild(1).GetChild(b).gameObject.GetComponent<Text>();

    //    sum.Add("" + core.head[mainA].Cost[mainB]);
    //    int c = mainAccses.Find(card.Trait, list[a].Head, false);
    //    if (c == -1)
    //        sum.Add("" + core.bD[core.keyTag].Base[mainA].Cost);

    //    if (full)
    //    {
    //        Accses accses = Saver.LoadRuleAccses(mainA, mainB);
    //        for (int i = 0; i < accses.Need.Count; i++)
    //        {
    //            c = mainAccses.Find(card.Trait, list[a].Head, false);
    //            if (c == -1)
    //                sum.Add("" + core.bD[core.keyTag].Base[mainA].Cost);
    //            if (accses.Need[i].Num.Count == 0)
    //                sum.Add("?");
    //            for (int j = 0; j < accses.Need[i].Num.Count; j++)
    //                sum.Add("" + core.head[-accses.Need[i].Head-1].Cost[accses.Need[i].Num[j].Head]);
    //        }
    //    }
      
    //    str += "\n";

    //    str += sum[0];
    //    maxSum += int.Parse(sum[0]);
    //    for (int i = 1; i < sum.Count; i++)
    //    {
    //        str += "+" + sum[i];
    //        if(sum[i] != "?")
    //            maxSum += int.Parse(sum[i]);

    //    }
    //    if (sum.Count != 1)
    //        str += "=" + maxSum ;
    //    if(lost)
    //        str += "+";


    //    text.text = str;
    //}
    
    //void SetRuleButton(int a)
    //{
    //    void Clear(int intMood, int size)
    //    {
    //        for (int i = size; i < ui.RuleRing[intMood].childCount; i++)
    //        {
    //            Destroy(ui.RuleRing[intMood].GetChild(size).gameObject);
    //            i--;
    //        }
    //    }

    //    int intMood = 0;
    //    Clear(a, ruleList[a].Num.Count);

    //    for (int i = ui.RuleRing[a].childCount; i < ruleList[a].Num.Count; i++)
    //    {
    //        GameObject go = Instantiate(ui.OrigButton);
    //        go.transform.SetParent(ui.RuleRing[a]);

    //        go.transform.gameObject.GetComponent<CardCaseInfo>().SetRuleAlt(cardSys, a, i);

    //        Button button = go.transform.GetChild(1).gameObject.GetComponent<Button>();
    //        button.onClick.AddListener(() => card.Stat[i].Edit("Max", -1));
    //        //button.onClick.AddListener(() => CountSize());

    //    }
    //    if (ruleListKey.Count > 0)
    //    {

    //    }
    //    else
    //    {
    //      //  for(int i=0;i<ruleList[0].Num.Count;i++)
    //            //ui.RuleWindow[a].ChildCount
    //    }
    //}

    //void AddHeadRule(int a )
    //{
    //    card.Trait.Add(new SubInt(a));
    //    AddNeed(core.bD[core.keyTag].Base[-a - 1].accses);
    //}
    //#endregion

    //#region Stat
    //void OpenStatWindow()
    //{
    //    if (statMood)
    //    {
    //        statMood = false;
    //        ui.StatWindow.gameObject.active = true;
    //        ui.RuleWindow.gameObject.active = false;
    //        SetListStat();
    //    }
    //}



    //void SetStat(int a, bool global = false)
    //{
    //    //a = core.bD[core.keyStat].Base[a]
    //    if (!global)
    //        a = statList[a];
    //    BD bd = core.bD[core.keyStat];
    //    card.Stat.Add(new StatExtend(a, bd.Base[a].Sub.Image));
    //    AddNeed(bd.Base[a].accses);

    //    FindConect();
    //    ViewCard();
    //    //SetStatButton();

    //}

    //void RemoveStat(int a, string log = "")
    //{
    //    int b = card.Stat[a].Get("Stat");
    //    int c = compliteAccses.Find(compliteAccses.Need, core.keyStat, false);
    //    if (c != -1)
    //    {
    //        int d = compliteAccses.Need[c].Find(b, false);
    //        if (d != -1)
    //        {
    //            card.Stat[a].Edit("Max", 1);
    //            return;
    //        }
    //    }

    //    card.Stat.RemoveAt(a);
    //    FindConect(log);

    //}

    //void StatView(int a)
    //{
    //    int b = card.Stat[a].Get("Stat");
    //    BD bd = core.bD[core.keyStat];
    //    string str = bd.Base[b].Name;
    //    str += $"\n{bd.Base[b].Cost}/4 * {card.Stat[a].Get("Max")}";
    //    ui.StatRing[0].GetChild(a).GetChild(0).gameObject.GetComponent<Text>().text = str;
    //}
    ////void SetStatButton()
    ////{
    ////    void Clear(int intMood, int size)
    ////    {
    ////        for (int i = size; i < ui.StatRing[intMood].childCount; i++)
    ////        {
    ////            Destroy(ui.StatRing[intMood].GetChild(size).gameObject);
    ////            i--;
    ////        }
    ////    }

    ////    int intMood = 0;
    ////    Clear(intMood, card.Stat.Count);

    ////    for (int i = ui.StatRing[intMood].childCount; i < card.Stat.Count; i++)
    ////    {
    ////        GameObject go = Instantiate(ui.OrigStatButton);
    ////        go.transform.SetParent(ui.StatRing[intMood]);

    ////        go.GetComponent<CardCaseInfo>().SetAlt(cardSys, intMood, i);

    ////        Button button = go.transform.GetChild(1).gameObject.GetComponent<Button>();
    ////        button.onClick.AddListener(() => card.Stat[i].Edit("Max", -1));
    ////        button.onClick.AddListener(() => CountSize());

    ////        button = go.transform.GetChild(2).gameObject.GetComponent<Button>();
    ////        button.onClick.AddListener(() => card.Stat[i].Edit("Max", 1));
    ////        button.onClick.AddListener(() => CountSize());
    ////    }

    ////    for (int i = 0; i < card.Stat.Count; i++)
    ////        StatView(i);


    ////    intMood = 1;
    ////    Clear(intMood, statList.Count);

    ////    for (int i = ui.StatRing[intMood].childCount; i < statList.Count; i++)
    ////    {
    ////        GameObject go = Instantiate(ui.OrigButton);
    ////        go.transform.SetParent(ui.StatRing[intMood]);

    ////        go.transform.gameObject.GetComponent<CardCaseInfo>().SetAlt(cardSys, intMood, i);

    ////        go.GetComponent<Button>().onClick.AddListener(() => SetStat(i));
    ////    }


    ////    for (int i = 0; i < statList.Count; i++)
    ////        ui.StatRing[1].GetChild(i).gameObject.GetComponent<Text>().text = core.bD[core.keyStat].Base[statList[i]].Name;
    ////}

    //void SetListStat()
    //{
    //    List<int> list = new List<int>();
    //    for (int i = 0; i < publicStatList.Count; i++)
    //        list.Add(publicStatList[i]);

    //    int a = compliteAccses.Find(compliteAccses.DisLike, core.keyStat, false);
    //    if(a != -1)
    //        for (int i = 0; i < compliteAccses.DisLike[a].Num.Count; i++)
    //            list.Remove(compliteAccses.DisLike[a].Num[i].Head);

    //    a = compliteAccses.Find(compliteAccses.Like, core.keyStat, false);
    //    if (a != -1)
    //        for (int i = 0; i < compliteAccses.Like[a].Num.Count; i++)
    //            if(core.bD[core.keyStat].Base[compliteAccses.Like[a].Num[i].Head].Look)
    //                list.Add(compliteAccses.Like[a].Num[i].Head);


    //    for (int i = 0; i < card.Stat.Count; i++)
    //        list.Remove(card.Stat[i].Get("Stat"));


    //    bool DisConnect(Accses accses,int key, int num)
    //    {
    //        int a = accses.Find(accses.DisLike, key, false);
    //        if (a != -1)
    //        {
    //            int b = accses.DisLike[a].Find( num, false);
    //            return (b != -1);
    //        }
    //        return false;
    //    }

    //    int b;
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        b = -1;
    //        Accses localAccses = core.bD[core.keyStat].Base[list[i]].accses;

    //        a = localAccses.Find(localAccses.DisLike, core.keyStat, false);
    //        if(a!= -1)
    //            for (int j = 0; j < card.Stat.Count; j++)
    //            {
    //                b = localAccses.DisLike[a].Find(card.Stat[j].Get("Stat"), false);
    //                if (b != -1) { list.RemoveAt(i); i--; break; }

    //            }
    //        if (b != -1)
    //            continue;

    //        for (int j = 0; j < localAccses.DisLike.Count; j++)
    //            if (localAccses.DisLike[j].Head < 0)
    //            {
    //                a = localAccses.Find(card.Trait, localAccses.DisLike[j].Head, false);
    //                if (a != -1)
    //                    if(localAccses.DisLike[j].Num.Count > 0)
    //                    {
    //                        for (int k = 0; k < localAccses.DisLike[j].Num.Count; k++)
    //                        {
    //                            b = card.Trait[a].Find(localAccses.DisLike[j].Num[k].Head, false);
    //                            if (b != -1)
    //                            { list.RemoveAt(i); i--; break; }
    //                        }
    //                        if (b != -1) 
    //                            break;
    //                    }
    //                    else { list.RemoveAt(i); i--; break; }
    //            }
    //    }


    //    statList = list;
    //    SetStatButton();
    //}
    //#endregion







    #endregion

    #region Save/Load
    public void Load(CoreSys coreSys, string str)
    {
        core = coreSys; 
        gameObject.GetComponent<CardConstructor>().enabled =true;
        StartData();
        if (str == " ")
        {
            return;
        }

        List<int> list = new List<int>(str.Split('/').Select(int.Parse).ToArray());
        card = Saver.LoadCard(list[0], list[1], list[2], list[3]);
        ui.NameTT.text = card.Name;
        ui.StartWindow.active = false;
        intGuild = card.Guild;
        intCardTayp = card.CardTayp;
        intCardClass = card.CardClass;
        intId = card.Id;
        intLegion = card.Legion;
        intCivilian = card.Civilian;
        intRace = card.Race;
        StartMana();
        SecondStageStart();
    }

    void Exit(string str)
    {
        if (edit)
        {
            ui.SaveWindow.active = true;
            return;
        }

    }
    void Save( bool use)
    {
        edit = false;
        ui.SaveWindow.active = false;
        card.Name = ui.NameTT.text;
        if (use)
            Saver.SaveGuildCard(card);

        Exit(exitMood);
    }
    #endregion
}
    // private List<Legion> actualLegion = new List<Legion>();

    // private Transform selector;
    // //private Transform ruleSelector;

    // private Guild curentGuild;
    // private string cardChar;

    // //constant
    // //private List<Constant> constants;
    // //private int curentConstant;

    // //ResetSystem
    // //private int oldAllCard;
    // //private List<CardCase> oldCard;
    // //private List<int> newCard;

    // //private int cardMod;
    // //private int cardModSize= 0;
    // //private int selectId;
    // private CardCase cardBase;
    // //public List<CardCase> LocalCard;
    // // [SerializeField]

    // [SerializeField]
    // private RuleMainFrame frame;
    // [SerializeField]
    // private CardConstructorUi Ui;
    // //  [SerializeField]
    //// private GameData gameData;

    // [SerializeField]
    // private GameSetting gameSetting;
    // private int selectId;
    // //private List<int> newData;
    // //private int curentFiltr;
    // //private bool filterRevers;

    // //private string origPath;
    //// private string origPathAlt;

    // public RenderTexture rTex;
    // public Camera captureCamera;

    // [SerializeField]
    // private TextMeshProUGUI TT;
    // [SerializeField]
    // private TextMeshProUGUI TT1;


    // void PointerClick()
    // {
    //     int linkIndex = TMP_TextUtilities.FindIntersectingLink(TT, Input.mousePosition, Camera.main);
    //     TMP_LinkInfo linkInfo = new TMP_LinkInfo();

    //     if (linkIndex == -1)
    //     {
    //         linkIndex = TMP_TextUtilities.FindIntersectingLink(TT1, Input.mousePosition, Camera.main);
    //         if (linkIndex == -1)
    //         {
    //             Debug.Log("Open link -1");
    //             return;
    //         }
    //         else
    //             linkInfo = TT1.textInfo.linkInfo[linkIndex];
    //     }
    //     else
    //         linkInfo = TT.textInfo.linkInfo[linkIndex];


    //     string selectedLink = linkInfo.GetLinkID();
    //     Debug.Log("Open link " + selectedLink);
    //     DeCoder(selectedLink);
    // }

    // void DeCoder(string str)
    // {
    //     string[] com = str.Split('_');
    //     int a = 0 , b=0;
    //     switch (com[0])
    //     {
    //         case ("Selector"):
    //             //GenerateTextExtend(com[1]);
    //             LoadSelector(com[1]);
    //             break;
    //         case ("Select"):
    //             a = int.Parse( com[2]);
    //             //gameSetting.Rule.FindIndex(x => x.Name == str)
    //             b = int.Parse(com[3]);
    //             //switch (com[1])
    //             //{
    //             //    //case ("Walk"):
    //             //    //    //b = int.Parse(com[3]);
    //             //    //    cardBase.WalkMood = gameSetting.Library.Rule[a].Rule[b];
    //             //    //    break;
    //             //    //case ("Action"):
    //             //    //   // b = int.Parse(com[3]);
    //             //    //    cardBase.ActionMood = gameSetting.Library.Rule[a].Rule[b];
    //             //    //    break;
    //             //    //case ("Def"):
    //             //    //    //b = int.Parse(com[3]);
    //             //    //    cardBase.DefMood = gameSetting.Library.Rule[a].Rule[b];
    //             //    //    break;
    //             //    //case ("Races"):
    //             //    //    cardBase.Races = cardBase.Guild.Races[a];
    //             //    //    break;
    //             //    //case ("Stat"):
    //             //    //    SwitchStat(a,b);
    //             //    //    break;
    //             //    //case ("Rule"):
    //             //    //    SwitchRule(a, b);
    //             //    //    break;
    //             //}
    //             break;
    //         case ("Rule"):
    //             a = int.Parse(com[2]);
    //             switch (com[1])
    //             {
    //                 case ("Down"):
    //                     DeliteRule(a);
    //                     break;
    //             }
    //             break;

    //         case ("Stat"):
    //             Debug.Log(com[1]);
    //             a = int.Parse(com[2]);
    //             switch (com[1])
    //             {
    //                 case ("Up"):
    //                     StatUp(a);
    //                     break;
    //                 case ("Down"):
    //                     StatDown(a);
    //                     break;
    //             }
    //             break;
    //         default:

    //             break;
    //     }
    // }


    // void CardCouner()
    // {
    //     if(selectId != -1)
    //         Ui.CounterCard.text = $"{selectId+1}/{gameSetting.AllCardPath.Count}";
    //     else
    //         Ui.CounterCard.text = $"0/{gameSetting.AllCardPath.Count}";
    // }

    // private string LinkSupport(string colorText, string linkText, string mainText)
    // {
    //     return $"<link={linkText}><color={colorText}>{mainText}</color></link>";
    // }
    // void GenerateText()
    // {
    //     string color = "#F4FF04";
    //     string str1;
    //     string str = $"Имя {cardBase.Name}";
    //     str += "\nРасса: " + LinkSupport(color, "Selector_Races", $"{cardBase.Races.Name}");
    //     //str += "\nЛегион: " + LinkSupport(color, "Selector_Legions", $"{cardBase.Legions.Name}");
    //     //str += "\nСоицальная группа: " + LinkSupport(color, "Selector_CivilianGroups", $"{cardBase.CivilianGroups.Name}");

    //     str += "\nВид: ";
    //     str1 = cardBase.Tayp;
    //     str += LinkSupport(color, "Selector_Tayp", $"{str1}");

    //     str += "\nТип: ";
    //     str1 = cardChar;
    //     str += LinkSupport(color, "Selector_CardChar", $"{str1}");

    //     str += "\nВыход на стол: ";
    //     str1 = (cardBase.WalkMood != null) ? cardBase.WalkMood.Name : "Null";
    //     str += LinkSupport(color, "Selector_Rule*Walk", $"{str1}");

    //     str += "\nБазовый набор атак: ";
    //     str1 = (cardBase.ActionMood != null) ?  cardBase.ActionMood.Name : "Null";
    //     str += LinkSupport(color, "Selector_Rule*Acton", $"{str1}");

    //     str += "\nБазовый набор защиты: ";
    //     str1 = (cardBase.DefMood != null) ?  cardBase.DefMood.Name : "Null";
    //     str += LinkSupport(color, "Selector_Rule*Def", $"{str1}");

    //     str += "\nХарактеристики\n";
    //     for (int i =0; i < cardBase.Stat.Count; i++)
    //     {
    //         str += LinkSupport(color, $"Selector_Stat*{i}", $"<sprite name={cardBase.Stat[i].IconName}> {cardBase.Stat[i].Name}");
    //         str += $"   { cardBase.StatSize[i]} *{cardBase.Stat[i].Cost}/4 "; 
    //         str += LinkSupport(color, $"Stat_Down_{i}", $"-");
    //         str += " ";
    //         str += LinkSupport(color, $"Stat_Up_{i}", $"+");
    //         str += "\n";
    //     }
    //     str += LinkSupport(color, $"Selector_Stat*{cardBase.Stat.Count}", $"Дополнительный параметр");

    //     HeadSimpleTrigger head = null;
    //     str += "\nНавыки\n";
    //     for (int i = 0; i < cardBase.Trait.Count; i++)
    //     {
    //         head = gameSetting.Rule[cardBase.TraitSize[i]];
    //         //Debug.Log(cardBase.Trait[i]);
    //         str += LinkSupport(color, $"Selector_Rule*{i}", $"{head.Name} ");
    //         str += $"  {head.Cost}/4 ";
    //         str += LinkSupport(color, $"Rule_Down_{i}", $"-");
    //        // str += " ";
    //        // str += LinkSupport(color, $"Rule_Plus_{i}", $"+");
    //         str += "\n";
    //     }
    //     str += LinkSupport(color, $"Selector_Rule*{cardBase.Trait.Count}", $"Дополнительный Навык");
    //     str += "\n";
    //     str += $"\nЦена карты ({cardBase.Mana})";

    //     TT.text = str;
    // }

    // void GenerateTextExtendClear()
    // {
    //     TT1.text = "";
    // }
    // void GenerateTextExtend(string text)
    // {
    //     string color = "#F4FF04";
    //     string[] com = text.Split('*');
    //     string str = "";
    //     switch (com[0]) 
    //     {
    //         case ("Races"):
    //             str += "Рассы в составе гильдии";
    //             for (int i = 0; i < cardBase.Guilds.Races.Count; i++)
    //                 str += LinkSupport(color, $"Select_Races_{i}", $"\n{cardBase.Guilds.Races[i].Name}");

    //             break;
    //         case ("Rule"):
    //             str += "Доступные механики";
    //             switch (com[1]) 
    //             {
    //                 case ("Walk"):
    //                     break;
    //                 case ("Action"):
    //                     break;
    //                 case ("Def"):
    //                     break;
    //                 case ("All"):
    //                     break;
    //             }

    //             break;
    //     }


    //     TT1.text = str;
    // }

    // #region IO card System
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //         PointerClick();
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         selector.gameObject.active = false;
    //         //Ui.TextWindow.active = false;
    //         //ComandClear();
    //         // Ui.TextWindow.active = false;
    //     }
    // }

    // void Enject()
    // {

    //     {
    //         int width = 100;
    //         int height = 150;
    //         Texture2D texture = new Texture2D(width, height);

    //         RenderTexture targetTexture = RenderTexture.GetTemporary(width, height);
    //         targetTexture.depth = 2;

    //         captureCamera.targetTexture = targetTexture;
    //         captureCamera.Render();
    //         RenderTexture.active = targetTexture;

    //         Rect rect = new Rect(0, 0, width, height);
    //         texture.ReadPixels(rect, 0, 0);
    //         texture.Apply();
    //         captureCamera.targetTexture = rTex;

    //         cardBase.Image = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    //         // cardBase.Image = texture.EncodeToPNG();
    //     }

    //     cardBase.Name = Ui.NameFlied.text;

    //     // CardCase card = Core.CardClone(cardBase);

    //     string path = curentGuild.Name
    //          + "/" + cardBase.Tayp
    //          + "/" + cardChar
    //          + "/";
    //     if (selectId != -1)
    //     {
    //         //int a = gameSetting.AllCard.FindIndex(x => x.Id == cardBase.Id);
    //         //if (a != -1)
    //         //{
    //         //    cardBase.Body = gameSetting.AllCard[a].Body;
    //         //    gameSetting.AllCard[a] = Core.CardClone(cardBase);
    //         //}
    //         //CardView.ViewCard(cardBase);

    //         //if (gameSetting.GameDataFile.Data[gameSetting.SystemKey].MasterKey != path)//Delite old card
    //         //{
    //         //    gameSetting.SystemKey = gameSetting.AllCard.FindIndex(x => x.Id == cardBase.Id);
    //         //    //int a = gameSetting.GameDataFile.Data.FindIndex(x => x.MasterKey == path);
    //         //    //string cod = gameSetting.AllCard[selectId];

    //         //}
    //         //int a = gameSetting.AllCard.FindIndex(x => x.Id == cardBase.Id);
    //         //if (a != -1)
    //         //    gameSetting.AllCard[a] = Core.CardClone(cardBase);
    //         //CardView.ViewCard(cardBase);
    //         //path = gameSetting.AllCardPath[cardBase.Id];
    //         //string[] com = path.Split('_');
    //         //a = int.Parse(com[0]);
    //         //path = gameSetting.GameDataFile.Data[a].MasterKey + "/" + com[1];
    //     }
    //     else
    //     {
    //         if (gameSetting.AllCard.Count < 20)
    //         {
    //             cardBase.Body = gameSetting.CardBody[gameSetting.AllCard.Count];
    //             CardView.ViewCard(cardBase);
    //             gameSetting.AllCard.Add(Core.CardClone(cardBase));
    //         }
    //         path = SetID(path);
    //     }

    //     CardCouner();
    //     XMLSaver.Save(cardBase, path );
    //     CardView.CardList();
    // }
    // string SetID(string path)
    // {
    //     int a = gameSetting.GameDataFile.Data.FindIndex(x => x.MasterKey == path);
    //     SubGameData sub = gameSetting.GameDataFile.Data[a];
    //     if (sub.Key != " ")
    //     {
    //         sub.Key += $"/{sub.Size}";
    //         sub.KeyComplite += $"/{a}_{sub.Size}";
    //     }
    //     else
    //     {
    //         sub.Key = $"{sub.Size}";
    //         sub.KeyComplite = $"{a}_{sub.Size}";
    //     }
    //     Debug.Log(sub.MasterKey);

    //     cardBase.Id = sub.Size;

    //     selectId = gameSetting.AllCardPath.Count;
    //     gameSetting.SystemKey = a;

    //     gameSetting.AllCardPath.Add($"{a}_{sub.Size}");
    //    // path += ""+sub.Size;
    //     sub.Size++;
    //     return path;
    // }

    // //       // CardCouner();
    // //        //string str = gameSetting.AllCardPath[cardBase.Id];
    // //        //string[] com = str.Split('_');
    // //        //int a = com[com.Length - 1];
    // //        //com[com.Length - 1] = "" + (a + 1);
    // //        //for()

    // //        //gameSetting.AllCardPath[cardBase.Id] = ""


    // //        //  string path = "";

    // //        if (gameData.BlackList.Count > 0)
    // //        {
    // //            int a = gameData.BlackList[0];

    // //            //path = $"/Resources/Hiro{a}";
    // //            //gameData.AllCard[a] = path;

    // //            //gameSetting.AllCard[a].Body.gameObject.active = true;
    // //            //card.Body = gameSetting.AllCard[a].Body;
    // //            //gameSetting.AllCard[a] = card;

    // //            //CardView.ViewCard(card);


    // //            gameData.BlackList.RemoveAt(0);
    // //        }
    // //        else
    // //        {
    // //            // path = origPath + $"{gameSetting.AllCard.Count}";

    // //            AddEdit(LocalCard.Count, card);
    // //            //    AddEdit(LocalCard.Count);

    // //            LocalCard.Add(card);
    // //            CardCouner();
    // //            CardViews();
    // //            //  NewCard(LocalCard.Count - 1);
    // //        }

    // //    }
    // //    else
    // //    {

    // //        card.Body = LocalCard[selectId].Body;

    // //        AddEdit(selectId, LocalCard[selectId]);

    // //        LocalCard[selectId] = card;

    // //        CardView.ViewCard(card);

    // //    }
    // //    CardCouner();

    // //    //Sort();
    // //}
    // void Inject()
    // {
    //     //ReLoadCard();

    //     if (selectId > -1)
    //     {
    //         cardBase = Core.CardClone(gameSetting.AllCard[selectId]);


    //         //Выгрузка данных в редактор героев
    //         Ui.NameFlied.text = cardBase.Name;
    //         ReCalculate();
    //         CardCouner();
    //     }
    // }
    // void Delite()
    // {
    //     ReLoadCard();
    //     if (selectId != -1)
    //     {
    //         SwitchCard(-1);
    //     }
    //     Ui.NameFlied.text = cardBase.Name;
    // }
    // #endregion

    // #region Save/Reset/Load System(Data Control)

    // void PreLoad()
    // {
    //     //origPath = $"/Resources/Data/Hiro";
    //     //origPathAlt = $"/Resources/Data";

    //     Core.LoadGameSetting(gameSetting);
    //     Core.LoadRules();


    //     selectId = -1;

    //     //LocalCard = new List<CardCase>();
    //     //newCard = new List<int>();
    //     //oldCard = new List<CardCase>();



    //     //GameObject GO = Ui.BaseCard.GetChild(0).gameObject;
    //     //GO.GetComponent<Image>().color = gameSetting.SelectColor[0];
    //     //GO.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); ;
    //     Ui.NewCardButton.GetComponent<Image>().color = gameSetting.SelectColor[0];
    //     Ui.NewCardButton.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); 

    //     Ui.ModButton[0].onClick.AddListener(() => CardView.Mod(false)); 
    //     Ui.ModButton[1].onClick.AddListener(() => CardView.Mod(true)); 

    //     Ui.EjectButton.onClick.AddListener(() => Enject());
    //     Ui.InjectButton.onClick.AddListener(() => Inject());
    //     Ui.DeliteButton.onClick.AddListener(() => Delite());

    //     Ui.SaveButton.onClick.AddListener(() => Save());
    //     Ui.ResetButton.onClick.AddListener(() => Reset());



    //     gameSetting.CardBody = Ui.CardBody;
    //     DataManager.GenerateKey(frame, gameSetting.Library);
    //     //gameSetting.Game
    //     //   gameData = XMLSaver.LoadGameData(origPathAlt);

    //     // oldAllCard = gameData.AllCard;

    //     LoadBase();

    //     CardCouner();
    //     //CardView.CardList();
    //     //CardViews();
    // }

    // //public void TransfData(GameData gameData1, GameData gameData2)
    // //{
    // //    gameData2 = new GameData();

    // //    gameData2.AllCard = gameData1.AllCard;
    // //    int a = gameData1.BlackList.Count;
    // //    gameData2.BlackList = new List<int>();
    // //    for (int i = 0; i < a; i++)
    // //    {
    // //        gameData2.BlackList.Add(gameData1.BlackList[i]);
    // //    }

    // //    gameData = gameData1;
    // //    gameDataReserv = gameData2;
    // //}


    // void LoadBase()
    // {

    //     //string path = "";
    //     //int a = gameData.AllCard;
    //     //for (int i = 0; i < a; i++)
    //     //{
    //     //    path = origPath + $"{i}";
    //     //    LocalCard.Add(XMLSaver.Load(path));
    //     //   // NewCard(i);
    //     //}

    //     //a = gameData.BlackList.Count;
    //     //for (int i = 0; i < a; i++)
    //     //{
    //     //    LocalCard[gameData.BlackList[i]].Body.gameObject.active = false;
    //     //}

    //     for(int i =0;i< Ui.CardBody.Count; i++)
    //     {
    //         SwitchCardButton(i, Ui.CardBody[i].gameObject.GetComponent<Button>());

    //         Ui.CardBody[i].gameObject.GetComponent<Image>().color = gameSetting.SelectColor[1];
    //         //SwitchCardButton
    //     }
    // }

    // void Reset()
    // {
    //     //int a = newCard.Count;
    //     //int b = 0;
    //     //for (int i = 0; i < a; i++)
    //     //{
    //     //    b = newCard[i];
    //     //    LocalCard[b] = oldCard[i];
    //     //    CardView.ViewCard(oldCard[i]);
    //     //}

    //     ////Remove add LocalCard
    //     //a = LocalCard.Count;
    //     //if (oldAllCard < a)
    //     //{
    //     //    a--;
    //     //    for (int i = oldAllCard - 1; i < a; i++)
    //     //    {
    //     //        Destroy(LocalCard[oldAllCard].Body.gameObject);
    //     //        LocalCard.RemoveAt(oldAllCard);
    //     //    }
    //     //}

    //     ////BlackList

    //     //TransfData(gameDataReserv, gameData);

    //     //// a = gameData.BlackList.Count;
    //     //// for (int i = 0; i < a; i++)
    //     //// {
    //     ////     b = gameData.BlackList[i];
    //     ////     if (LocalCard.Count > b)
    //     ////     {
    //     ////         LocalCard[b].Body.gameObject.active = true;
    //     ////     }
    //     //// }

    //     ////// TransfData(gameSetting.GlobalMyData, gameData);


    //     //// a = gameData.BlackList.Count;
    //     //// for (int i = 0; i < a; i++)
    //     //// {
    //     ////     b = gameData.BlackList[i];
    //     ////     LocalCard[b].Body.gameObject.active = false;
    //     //// }

    //     ////ResetData
    //     //newCard = new List<int>();
    //     //oldCard = new List<CardCase>();
    // }

    // //void AddEdit(int a, CardCase card)
    // //{
    // //    for (int i = 0; i < newCard.Count; i++)
    // //    {
    // //        if (a == newCard[i])
    // //            return;
    // //    }
    // //    newCard.Add(a);
    // //    oldCard.Add(card);
    // //}
    // void Save()
    // {

    //     XMLSaver.SaveGameData(gameSetting.GameDataFile);
    //     //// gameData.AllCard = LocalCard.Count;
    //     //// TransfData(gameData, gameSetting.GlobalMyData);
    //     //int b = 0;
    //     //string path = "";
    //     //for (int i = 0; i < newCard.Count; i++)
    //     //{
    //     //    b = newCard[i];
    //     //    path = origPath + $"{b}";
    //     //    XMLSaver.Save(LocalCard[b], path);
    //     //}

    //     //XMLSaver.SaveGameData(gameData, origPathAlt);
    //     //oldAllCard = gameData.AllCard;
    //     //newCard = new List<int>();
    //     //oldCard = new List<CardCase>();
    // }
    // #endregion

    // #region Create UI


    // void AddStatButton(bool plus, Button button, int a)
    // {
    //     button.onClick.RemoveAllListeners();
    //     if (plus)
    //         button.onClick.AddListener(() => StatUp(a));
    //     else
    //         button.onClick.AddListener(() => StatDown(a));
    // }
    // void SwitchCardButton(int a, Button button)
    // {
    //     button.onClick.AddListener(() => SwitchCard(a));
    // }

    // //void NewCard(int i)
    // //{
    // //    int a = Ui.BaseCard.childCount - 1;
    // //    GameObject GO = Instantiate(Ui.OrigCard);
    // //    GO.transform.SetParent(Ui.BaseCard);

    // //    Button button = GO.GetComponent<Button>();
    // //    SwitchCardButton(a, button);

    // //    GO.GetComponent<Image>().color = gameSetting.SelectColor[1];

    // //    LocalCard[i].Body = GO.transform;

    // //    CardView.ViewCard(LocalCard[i]);
    // //}




    // #endregion

    // #region Ui Use

    // void StatUp(int a)
    // {
    //     cardBase.StatSize[a]++;
    //     ReCalculate();
    //     if (cardBase.Mana > 10)
    //     {
    //         cardBase.StatSize[a]--;
    //         ReCalculate();
    //     }
    //    // Ui.StatUi[a].AllCount.text = "" + cardBase.StatSize[a];
    // }
    // void StatDown(int a)
    // {
    //     if (cardBase.StatSize[a] > 1)
    //     {
    //         cardBase.StatSize[a]--;
    //        // Ui.StatUi[a].AllCount.text = "" + cardBase.StatSize[a];
    //     }
    //     else if (a != 0)
    //         DeliteStat(a);


    //     ReCalculate();
    // }
    // void ReCalculate()
    // {
    //     int a = 0;
    //     for (int i = 0; i < cardBase.Stat.Count; i++)
    //     {
    //         if (cardBase.Stat[i] != null)
    //         {
    //             a += cardBase.StatSize[i] * cardBase.Stat[i].Cost;
    //         }
    //     }
    //     cardBase.Mana = Mathf.CeilToInt(a / 4f);

    //     GenerateText();
    //     //Ui.ManaCount.text = $"Цена:{a}/4={cardBase.Mana}";
    // }


    // void SwitchCard(int a)
    // {
    //     if (a == -1)
    //     {
    //         if(selectId != -1)
    //             CardView.CardColor(selectId, 1);
    //         selectId = -1;
    //         Ui.NewCardButton.GetComponent<Image>().color = gameSetting.SelectColor[0];

    //     }
    //     else if(a >=gameSetting.AllCard.Count)
    //         return;
    //     else
    //     {
    //         string[] com = gameSetting.AllCardPath[a].Split('_');
    //         gameSetting.SystemKey = int.Parse(com[0]);
    //     }

    //     if (selectId == -1)
    //         Ui.NewCardButton.GetComponent<Image>().color = gameSetting.SelectColor[0];

    //     CardView.CardColor(selectId, 1);
    //     selectId = a;
    //     CardView.CardColor(a,0);

    //     CardCouner();

    // }
    // #endregion
    // void DeliteStat(int a)
    // {
    //     cardBase.Stat.RemoveAt(a);
    //     cardBase.StatSize.RemoveAt(a);
    //     cardBase.StatSizeLocal.RemoveAt(a);
    // }
    // void DeliteRule(int a)
    // {
    //     cardBase.Trait.RemoveAt(a);
    // }

    // void SwitchRule(int a, int b, int c)
    // {
    //    // Debug.Log($"{a} {b} {c}");
    //     string str = gameSetting.Library.Rule[b].Name + "_" + gameSetting.Library.Rule[b].Rule[c];
    //     int d = gameSetting.Rule.FindIndex(x => x.Name == gameSetting.Library.Rule[b].Rule[c]);

    //    // Debug.Log(cardBase.Trait.Count);
    //     if (a >= cardBase.Trait.Count)
    //     {
    //         cardBase.Trait.Add(str);
    //         cardBase.TraitSize.Add(0);
    //         //AddRuleUi();
    //     }
    //     else
    //         cardBase.Trait[a] = str;

    //     //StatCaseUi caseUi = Ui.RuleUi[a];
    //     //int b = gameSetting.Rule.Find(x => x.Name == str);
    //     cardBase.TraitSize[a] = d;
    //     // caseUi.Name.text = str;
    //     ReCalculate();
    //     selector.gameObject.active = false;
    // }
    // void SwitchTayp(int a)
    // {
    //     if(cardBase.Tayp != frame.ClassCard[a])
    //     {
    //         cardBase.Trait = new List<string>();
    //         cardBase.TraitSize = new List<int>();
    //         SwitchCard(-1);

    //     }
    //     cardBase.Tayp = frame.ClassCard[a];
    //     selector.gameObject.active = false;
    //     GenerateText();
    // }
    // void SwitchCardChar(int a )
    // {
    //     cardChar = frame.CardTayp[a];
    //     selector.gameObject.active = false;
    //     GenerateText();
    // }

    // void SwitchStat(int a, int b)
    // {
    //     if (a >= cardBase.Stat.Count)
    //     {
    //         cardBase.Stat.Add(gameSetting.Library.Constants[b]);
    //         cardBase.StatSize.Add(0);
    //         cardBase.StatSizeLocal.Add(0);
    //         // AddStatUi();
    //         // Ui.StatUi[0].ButtonSwitch.enabled = false;
    //     }

    //     // StatCaseUi caseUi = Ui.StatUi[a];
    //     cardBase.StatSize[a] = 0;
    //     cardBase.Stat[a] = gameSetting.Library.Constants[b];
    //     StatUp(a);
    //     selector.gameObject.active = false;

    //     // caseUi.Name.text = cardBase.Stat[a].Name;
    //     // caseUi.Icon.sprite = cardBase.Stat[a].Icon;

    //     //  caseUi.SellCount.text = $"{ cardBase.Stat[a].Cost}/4";
    //     //caseUi.AllCount.text = "1";
    //     //if (cardBase.StatSize[a] == 0)
    //     //{
    //     //    DeliteStat(a);
    //     //    return;
    //     //}
    // }

    // #region SwitchGuild

    // void OpenGuildSelector()
    // {
    //     Ui.GuildSelector.active = true;
    // }

    // void LoadGuildSelector()
    // {
    //     GameObject GO = null;


    //     for (int i = 0; i < gameSetting.Library.Guilds.Count; i++)
    //     {
    //         GO = Instantiate(Ui.OrigButtonBanner);
    //         GO.transform.SetParent(Ui.GuildSelector.transform);

    //         GO.GetComponent<Image>().sprite = gameSetting.Library.Guilds[i].Icon;
    //         SetSwitchGuildButton(i, GO.GetComponent<Button>());
    //     }
    // }

    // void SetSwitchGuildButton( int a, Button button)
    // {
    //     button.onClick.AddListener(() => SwitchGuild(a));
    // }

    // void SwitchGuild(int i)
    // {
    //     Ui.GuildSelector.active = false;
    //     curentGuild = gameSetting.Library.Guilds[i];
    //     Ui.GuildBanner.sprite = curentGuild.Icon;

    //     ReLoadCard();


    // }
    // #endregion
    // void ReLoadCard()
    // {

    //     cardBase = new CardCase();
    //     cardBase.Stat = new List<Constant>();
    //     cardBase.StatSize = new List<int>();
    //     //for (int i =0; i < gameSetting.StatSize; i++)
    //     //{
    //     //    cardBase.Stat.Add(null);
    //     //    cardBase.StatSize.Add(0);
    //     //    SwitchStat(i, -1);
    //     //    Ui.StatUi[0].ButtonSwitch.enabled = false;
    //     //}

    //     cardBase.Tayp = frame.ClassCard[0];
    //     cardBase.Guilds = curentGuild;
    //     actualLegion = new List<Legion>();

    //     cardBase.Name = "New Hiro";
    //     SwitchRace(curentGuild.Races[0]);




    //     cardBase.TraitSize = new List<int>();
    //     cardBase.Trait = new List<string>();
    //     //for (int i = 0; i < gameSetting.RuleSize; i++)
    //     //{
    //     //    cardBase.Trait.Add(null);
    //     //    cardBase.TraitSize.Add(0);
    //     //    //SwitchStat(i, -1);
    //     //}
    // }
    // void SwitchRace(Race race)
    // {
    //     cardBase.Races = race;


    //     int a = 0;
    //     string statName = "";

    //     if (race.MainRace != null)
    //         statName = race.MainRace.MainStat.Name;
    //     else
    //         statName = race.MainStat.Name;

    //     a = gameSetting.Library.Constants.FindIndex(x => x.Name == statName);
    //     if (a > -1)
    //         SwitchStat(0, a);





    //     foreach (Legion legion in curentGuild.Legions)
    //     {
    //         a = cardBase.Races.Legions.FindIndex(x => x.Name == legion.Name);
    //         if (a >= 0)
    //             actualLegion.Add(legion);
    //     }


    //     //Ui.RaceText.text = race.Name;

    //     if (actualLegion.Count > 0)
    //         SwitchLegion(actualLegion[0]);
    // }
    // void SwitchLegion(Legion legion)
    // {
    //     cardBase.Legions = legion;
    //    // Ui.LegionText.text = legion.Name;

    //     if (legion.CivilianGroups.Count > 0)
    //         SwitchCivilian(legion.CivilianGroups[0]);
    // }

    // void SwitchCivilian(CivilianGroup civilian)
    // {
    //     cardBase.CivilianGroups = civilian;
    //     //Ui.CivilianText.text = civilian.Name;

    //     Ui.NameFlied.text = cardBase.Name;
    //     ReCalculate();
    // }

    // void LoadSelector(string data)
    // {

    //     //Debug.Log($"{data}");
    //     string[] com = data.Split('*');
    //     int a = 0; 
    //     int b = 0;
    //     int sizeData =0;
    //     //Debug.Log($"{data} {sizeData} {a} {b}");
    //     switch (com[0])
    //     {
    //         case("Race"):
    //             sizeData = curentGuild.Races.Count;
    //             break;

    //         case ("Legion"):
    //             sizeData = actualLegion.Count;
    //             break;

    //         case ("Civilian"):
    //             sizeData = cardBase.Legions.CivilianGroups.Count;
    //             break;

    //         case ("Stat"):
    //             a = int.Parse(com[1]);
    //             sizeData = gameSetting.Library.Constants.Count;
    //             break;

    //         case ("Rule"):
    //             a = int.Parse(com[1]);
    //             sizeData = gameSetting.Library.Rule.Count;
    //             break;

    //         case ("Tayp"):
    //             sizeData = frame.ClassCard.Count;
    //             break;
    //         case ("CardChar"):
    //             sizeData = frame.CardTayp.Count;
    //             break;
    //         default:
    //             a=int.Parse(com[1]);
    //             b = int.Parse(com[2]);
    //             sizeData = gameSetting.Library.Rule[b].Rule.Count;

    //             break;
    //     }
    //     selector.gameObject.active = true;
    //     //Debug.Log($"{data} {sizeData} {a} {b}");

    //     //if(selector.ChildCount > sizeData)
    //     //for(int i = sizeData; i< selector.ChildCount;)

    //     GameObject GO = null;
    //     if (selector.childCount > sizeData) 
    //     {
    //         for (int i = selector.childCount; i > sizeData; i--)
    //         {
    //             //Debug.Log($"{sizeData} {selector.childCount} {i}");
    //             Destroy(selector.GetChild(i-1).gameObject);
    //         } 
    //     }
    //     else if  (selector.childCount < sizeData)
    //     {
    //         for (int i = selector.childCount; i < sizeData; i++)
    //         {
    //             GO = Instantiate(Ui.OrigButton);
    //             GO.transform.SetParent(selector);
    //         }
    //     }

    //     //gameSetting.Library.Legions.Count;
    //     for (int i =0; i< sizeData;  i++)
    //     {
    //         GO = selector.GetChild(i).gameObject;
    //         Text text = GO.transform.GetChild(0).gameObject.GetComponent<Text>();
    //         switch (com[0])
    //         {
    //             case ("Race"):
    //                 text.text = curentGuild.Races[i].Name;
    //                 break;
    //             case ("Legion"):
    //                 text.text = actualLegion[i].Name;
    //                 break;
    //             case ("Civilian"):
    //                 text.text = cardBase.Legions.CivilianGroups[i].Name;
    //                 break;
    //             case ("Stat"):
    //                 text.text = gameSetting.Library.Constants[i].Name;
    //                 break;
    //             case ("Rule"):
    //                 text.text = gameSetting.Library.Rule[i].Name + $"({gameSetting.Library.Rule[i].Rule.Count})";
    //                 break;

    //             case ("Tayp"):
    //                 text.text = frame.ClassCard[i];
    //                 break;
    //             case ("CardChar"):
    //                 text.text = frame.CardTayp[i];
    //                 break;
    //             default:
    //                 text.text = gameSetting.Library.Rule[b].Rule[i];
    //                 break;
    //         }
    //         Debug.Log(text.text);
    //         //Debug.Log($"{data} {i} {a} {b}");
    //         SetSwitchButton(i, GO.GetComponent<Button>(), com[0], a,b);
    //     }
    // }

    // void SetSwitchButton(int a, Button button, string data, int b, int c)
    // {
    //     button.onClick.RemoveAllListeners();
    //     switch (data)
    //     {
    //         case ("Race"):
    //             button.onClick.AddListener(() => SwitchRace(curentGuild.Races[a]));
    //             break;
    //         case ("Legion"):
    //             button.onClick.AddListener(() => SwitchLegion(actualLegion[a]));
    //             break;
    //         case ("Civilian"):
    //             button.onClick.AddListener(() => SwitchCivilian(cardBase.Legions.CivilianGroups[a]));
    //             break;
    //         case ("Stat"):
    //             button.onClick.AddListener(() => SwitchStat(b,a));
    //             //text.text = gameSetting.Lybrary.Constants[i].Name;
    //             break;
    //         case ("Rule"):
    //             button.onClick.AddListener(() => LoadSelector(gameSetting.Library.Rule[a].Name +"*"+b + "*" + a));
    //             break;
    //         case ("Tayp"):
    //             button.onClick.AddListener(() => SwitchTayp(a));
    //             break;
    //         case ("CardChar"):
    //             button.onClick.AddListener(() => SwitchCardChar(a));
    //             break;
    //         default:
    //             button.onClick.AddListener(() => SwitchRule(b, c,a));
    //             //text.text = gameSetting.Lybrary.Rule[b].Rule[i].Name;
    //             break;
    //     }
    // }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     cardChar = frame.CardTayp[0];

    //     CreateCore();

    //     PreLoad();

    //     //LoadRuleSelector(0);


    //     // CreateStatButton();
    //    // Inject();

    //     //CreateRuleList();
    //     //LoadRuleSelector(0);

    //     //GenerateFiltr();

    // }

    // void CreateCore()
    // {
    //     XMLSaver.SetGameSetting(gameSetting);
    //     XMLSaver.LoadMainRule(gameSetting.Library);

    //     GameObject GO = Instantiate(Ui.SelectorOrigin);
    //     selector = GO.transform;
    //     selector.SetParent(Ui.SelectorOrigin.transform);
    //     GO.active = false;

    //     // CreateSystemButton();


    //     // gameSetting.library.Ru
    //     SwitchGuild(0);

    //     //LoadGuildSelector();

    //     //Ui.GuildButton.onClick.AddListener(() => OpenGuildSelector());
    // }

