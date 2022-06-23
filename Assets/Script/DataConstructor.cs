using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saver;
using TMPro;

public class DataConstructor : MonoBehaviour
{
    public TMP_SpriteAsset tmp;
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
    List<MainGuild> guilds = new List<MainGuild>();

    // Start is called before the first frame update
    private string[] tayp = {"Guild","Legion", "Stat","Tayp","Tag","Color" };
    public Color[] colors;
    private string[] colorsStr;
    int charSize = 0;
    MainBase main;

    void Start()
    {
        //XMLSaver.LoadAtlas(gameObject.GetComponent<SpriteRenderer>(), tmp);
        CreateColor();

    }
    void NewMain(string str)
    {
        main = new MainBase();
        main.Tayp = str;
    }

    void DeCoder(string str)
    {
        string[] com = str.Split('_');
        switch (com[0])
        {
            case ("SetLink"):
                SetLink(com[1], int.Parse(com[2]);
                break;

            case ("Color"):
                main.ColorName = colorsStr[int.Parse(com[1])];
                break;
            case ("Char"):
                //main.ColorName = colorsStr[int.Parse(com[1])];
                break;
            case ("Switch"):
                switch (com[1])
                {
                    case ("Color"):
                        SwitchColor();
                        break;
                    case ("Char"):
                        SwitchChar();
                        break;
                }
                break;
        }

    }

    string SetLink(string str, int a )
    {
        string com = "#"
        //string[] com = str.Split("|");
        //int a = com
        switch (str)
        {
            case ("Guild"):
                str = guilds
                break;
        }

        return str;
    }



    void CreateColor()
    {
        colorsStr = new string[colors.Length];
        for(int i = 0; i < colorsStr.Length; i++)
        {
            colorsStr[i] = colors[i].ToString;
        }
    }

    string SwitchColor()
    {
        string str = AddLinkColor(0);
        for(int i = 1; i < colorsStr.Length; i++)
        {
            str += "\n" + AddLinkColor(i);
        }
        return str;
    }
    string SwitchChar()
    {
        string str = AddLinkChar(0);
        for (int i = 1; i < charSize; i++)
        {
            str += "\n" + AddLinkChar(i);
        }
        return str;

    }

    string Link(string str)
    {
        str = $"<link=Char_{i}><index={i}></link>";
        return str;
    }

    string AddLinkChar(int i)
    {
        return $"<link=Char_{i}><index={i}></link>";
    }
    string AddLinkColor(int i)
    {
        return $"<link=Color_{i}><color={colorsStr[i]}>{colorsStr[i]}</color></link>";
    }

    string AddLink(MainBase main)
    {

        return $"<link=Info_{main.Tayp}_{main.SysName}><color={main.ColorName}>{main.LocalName}</color></link>";
    }
    string AddInfoLink(MainBase main)
    {
        string str;
        if(main.Info == " ")
            str = $"<link=InfoLink_{main.Tayp}_{main.SysName}><color={colorsStr[0]}>[!]</color></link>";
        else
            str = $"<link=InfoLink_{main.Tayp}_{main.SysName}><color={colorsStr[1]}>[?]</color></link>";
        return str;
    }


}


public class MainBase
{
    public string SysName = " ";
    public string Tayp;
    public string LocalName = " ";
    public int ColorName = " ";
    public string Info = " ";
}

public class MainGuild
{
    public MainBase Base = new MainBase();
   // public 
}