using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saver;
using TMPro;
using UnityEngine.UI;

public class DataConstructor : MonoBehaviour
{
    int sysA = 0, sysB = 0, sysC =0;
    private List<BD> bD;
    private string sysLanguage;
    // private string sysLanguageDef = "Eng";

    [SerializeField]
    private TMP_InputField nameFlied;
    private string nameFliedMood;
     
    [SerializeField]
    private Button loadButton;

    [SerializeField]
    private TMP_SpriteAsset tmp;
    [SerializeField]
    private TextMeshProUGUI TT1;//Sys
    [SerializeField]
    private TextMeshProUGUI TT2;//Info
    //[SerializeField]
    //private TextMeshProUGUI TT2;//MainImput

    /*
        общие данные

    системное имя
    имя внутри текущей локализации
    системный цвет

        классы данных
    гильдии
    рассы
    легионы
    социальные группы

    тэги



    для тегов - 
    связать тэг с гильдией, легионом

    знаками 
    [?]-обозначена кнопка получения стравки 
    [!]-требует нажать ее для устранения проблемы
     */

    // Start is called before the first frame update
    private string[] tayp = {"Guild","Legion", "Stat","Tag","Plan", "Association", "CivilGroup", "Status","CardTayp","CardClass"};
    public Color[] colors;
    private string[] colorsStr;
    int iconSize = 0;
    MainBase mainBase;

    #region IO
    void Save()
    {
        XMLSaver.SaveBase(sysA, sysB, mainBase);
    }
    #endregion
    void Reset()
    {
        bD = new List<BD>();
        for (int i = 0; i < tayp.Length; i++)
        {
            BD bd = new BD();
            bd.Name = tayp[i];
            bd.Base.Add(new MainBase());

            bD.Add(bd);
        }
        XMLSaver.SaveAllBD(bD);
    }

    void Start()
    {
        loadButton.onClick.AddListener(() => SetLocText());
        sysLanguage = "Eng";
        XMLSaver.LoadDataLang(sysLanguage);
        //XMLSaver.LoadAtlas(gameObject.GetComponent<SpriteRenderer>(), tmp);
        CreateColor();

        Reset();


        bD =  XMLSaver.LoadAllData();

        mainBase = bD[0].Base[0];

        HeadText();
        BaseInfo();
    }

    //void SysGen()
    //{
    //    string[] tayp = { "Guild", "Legion", "Stat", "Tayp", "Tag" };
    //    for (int i = 0; i < tayp.Length; i++)
    //        AddBDHead(tayp[i]);
    //}

    #region HEAD
    void HeadClear()
    {
        TT1.text = "";
    }
    void HeadText()
    {
        string text = AddLink("Sys|Menu", " Open SysMenu") + "\n"; ;
        for (int i = 0; i < tayp.Length; i++)
        {
            text += AddLink($"Open|Text_{i}", bD[i].Name + $"({bD[i].Base.Count})") + "\n";
        }

        TT1.text = text;
    }
    void OpenText(int a)
    {
        string text = AddLink($"Re|NameHead|{a}", $"{bD[a].Name}-ReName") + "\n";
        text = AddLink($"Re|InfoHead|{a}", $"{bD[a].Name}-ReInfo") + "\n";

        text += AddLink($"Open|Back", "Back") + "\n";
        for (int i = 0; i < bD[a].Base.Count; i++)
            text += AddLink($"Open|{a}_{i}", bD[a].Base[i].Name) + "\n";

        TT1.text = text;
    }

    //void SysText()
    //{
    //    string text = AddLink("Sys_Core", " Core");//core -обращение к маин функциям
    //}

    //void InfoText()
    //{
    //    string text = AddLink($"Open", "NewTayp") + "\n";
    //    for (int i = 0; i < bD[a].Base.Count; i++)
    //        text = AddLink($"Open|{bD[a].Name}_{a}_{i}", bD[a].Base[i].Name) + "\n";

    //    TT2.text = text;
    //}

    #endregion

    #region Switch
    void CreateColor()
    {
        colorsStr = new string[colors.Length];
        for (int i = 0; i < colorsStr.Length; i++)
        {
            colorsStr[i] = ColorUtility.ToHtmlStringRGB(colors[i]);// colors[i].ToString();
        }
    }

    string SwitchColor()
    {
        string str = AddLinkColor(0);
        for (int i = 1; i < colorsStr.Length; i++)
        {
            str += "\n" + AddLinkColor(i);
        }
        return str;
    }
    string SwitchIcon()
    {
        string str = AddLinkIcon(0);
        for (int i = 1; i < iconSize; i++)
        {
            str += "\n" + AddLinkIcon(i);
        }
        return str;

    }
    #endregion
    #region Info
    void BaseInfo()
    {
        string text = "" +bD[sysA].Name +"\n";
        /*
         SAVE()     LOAD()

        NAmeLOcalization
        Color localization
        Info
         
         */
        text += AddLink("Re|Name", "Name "+ mainBase.Name, mainBase.ColorName);
        if (mainBase.Info == "Void")
            text += AddLink("Re|Info", "[!]", colorsStr[1]);
        else
            text += AddLink("Re|Info", "[?]", colorsStr[0]);
        text += "\n" +AddLink("Open|SwicthColor", $"Color = {mainBase.ColorName}", mainBase.ColorName);

        TT2.text = text;
    }
    #endregion

    #region DeCoder
    /*
     Sys - раздел системных комманд
     Open - открывает определенный раздел проводника
     Switch - переключает раздел на другой из содержимого статической библиотеки
     Link - сформировать ссылку для текста
     Re - взять текст из поля
     */
    void DeCoder(string str)
    {
        int a = 0;
        string[] com = str.Split('|');
        switch (com[0])
        {
            case ("Sys"):
                DeCoderSys(com[1]);
                break;
            case ("Open"):
                DeCoderOpen(com[1]);
                break;
            case ("Switch"):
                DeCoderSwitch(com[1]);
                break;
            case ("Link"):
                DeCoderLink(com[1]);
                break;
            case ("Re"):
                if (com.Length > 2)
                    GetLocText(com[1], int.Parse(com[2]));
                else
                    GetLocText(com[1]);
                break;
        }

    }

    void DeCoderSys(string str)
    {
        string[] com = str.Split('_');
        switch (com[0])
        {
            case ("Menu"):
                //выведет меню
                //DeCoderSys(com[1]);
                break;
        }
    }
    void DeCoderOpen(string str)
    {
        string[] com = str.Split('_');
        switch (com[0])
        {
            case ("Back"):
                HeadText();
                break;
            case ("Text"):
                OpenText(int.Parse(com[1]));
                break;
            case ("Save"):
                Save();
                break;
            case ("New"):
                sysA = int.Parse(com[1]);
                sysB = bD[sysA].Base.Count;
                bD[sysA].Base.Add(new MainBase());
                mainBase = bD[sysA].Base[sysB];
                Save();
                BaseInfo();
                break;
            default:
                sysA = int.Parse(com[1]);
                sysB = int.Parse(com[2]);
                mainBase = bD[sysA].Base[sysB];
                BaseInfo();
                break;
        }
    }
    void DeCoderSwitch(string str)
    {
        string[] com = str.Split('_');
        switch (com[0])
        {
            case ("Color"):
                mainBase.ColorName = colorsStr[int.Parse(com[1])];
                break;
        }
        BaseInfo();
    }
    void DeCoderLink(string str)
    {
        string[] com = str.Split('_');
        switch (com[0])
        {
            case ("Sys"):
                DeCoderSys(com[1]);
                break;
        }
    }
    void DeCoderRe(string str)
    {
        string[] com = str.Split('_');
        switch (com[0])
        {
            case ("Sys"):
                DeCoderSys(com[1]);
                break;
        }
    }

    #endregion
    #region Localization

    void LocClose()
    {
        nameFlied.gameObject.active = false;
    }

    void GetLocText(string text, int a =0)
    {
        sysC = a;
        TT1.text = "";
        nameFliedMood = text;
        switch (nameFliedMood)
        {
            case ("Name"):
                text = mainBase.Name;
                //LocName();
                break;
            case ("InfoHead"):
                text = bD[a].Info;
                break;
            case ("NameHead"):
                text = bD[a].Name;
                break;
            case ("Info"):
                text = mainBase.Info;
                //OpenLinkList()
                break;
        }
        nameFlied.text = text;
        nameFlied.gameObject.active = true;
        HeadClear();
    }
    void SetLocText()
    {
        string text = " ";
        switch (nameFliedMood)
        {
            case ("InfoHead"):
                bD[sysC].Info = nameFlied.text;
                break;
            case ("NameHead"):
                bD[sysC].Name = nameFlied.text;
                break;
            case ("Name"):
                mainBase.Name = nameFlied.text;
                break;
            case ("Info"):
                mainBase.Info = nameFlied.text;
                break;
        }
        nameFlied.gameObject.active = false;
        // DefultUi()
        HeadText();
        BaseInfo();
    }

    void OpenLinkList()
    {
        string text = AddLink($"Remove_Link", "RemoveFragmet") + "\n";
        for (int i = 0; i < tayp.Length; i++)
            text = AddLink($"Open_{i}", tayp[i]) + "\n";

        TT2.text = text;
    }

    #endregion


    #region Link
    string Link(int a)
    {
        return $"<link=Char_{a}><index={a}></link>";
    }

    string AddLinkIcon(int i)
    {
        return $"<link=Icon_{i}><index={i}></link>";
    }
    string AddLinkColor(int i)
    {
        return $"<link=Switch|Color_{i}><color=#{colorsStr[i]}>{colorsStr[i]}</color></link>";
    }

    //string AddLink(MainBase main)
    //{
    //    return $"<link=Info_{main.SysName}><color={main.ColorName}>{main.LocalName}</color></link>";
    //}
    //string AddInfoLink(MainBase main)
    //{
    //    string str;
    //    if(main.Info == "Void")
    //        str = $"<link=InfoLink_{main.SysName}><color={colorsStr[1]}>[!]</color></link>";
    //    else
    //        str = $"<link=InfoLink_{main.SysName}><color={colorsStr[0]}>[?]</color></link>";
    //    return str;
    //}


    string AddLink(string link, string text, string colors = "ffff00")
    {
        return $"<link={link}><color=#{colors}>{text}</color></link>";
    }
    #endregion
    
}
public class BD {
    public string Name = "Void";
    public string Info = "Void";
    public List<MainBase> Base = new List<MainBase>();
}



public class MainBase
{
    //public int SysName = " ";
    //public string Tayp;
    public string Name = "Void";
    public string ColorName = "ffff00";
    public string Info = "Void";
    public int Cost;

    /*
     */
}

//public class MainGuild { }
//public class MainLegion { }
//public class MainRace { }
//public class MainCivilianGroup { }
//public class MainTag { }
//public class MainStat { }