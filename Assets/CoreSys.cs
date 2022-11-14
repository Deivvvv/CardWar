using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Coder;
using XMLSaver;
using SubSys;


public class CoreSys : MonoBehaviour
{
    public int keyTayp,keyStat, keyTag, keyPlan, keyGuild, keyLegion, keyCivilian, keyRace, keyCardTayp, keyCardClass, keyStatus, keyStatGroup, keyMark;
    BaseUi ui;
    public RuleMainFrame frame;
    public List<BD> bD;
    public List<SubRuleHead> head;
    string mood = "Main";
    GameObject redactor;
    Transform canvasTransform;
    //ImageBd

    //private List<TextMeshProUGUI> TT;
    //private TMP_InputField nameTT;

    // Start is called before the first frame update

    void ExitRedactor()
    {
        DeCoder.Read("Sys|Save");
        Destroy(redactor);
    }
    void OpenRedactor()
    {
        if (redactor != null)
            return;
        redactor  = Instantiate(ui.OrigRedactor);
        redactor.transform.SetParent(canvasTransform);
        redactor.GetComponent<RedactorUi>().ExitButton.onClick.AddListener(() => ExitRedactor());
       // redactor.active = false;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 15;
        frame.Convert();

        Saver.LoadDataLang("Rus");
        //Saver.LoadDataLang("Eng");
        Saver.SetCore(gameObject.GetComponent<CoreSys>());
        DeCoder.SetCore(gameObject.GetComponent<CoreSys>());


        keyStat = DeCoder.ReturnIndex("Stat");
        keyTag = DeCoder.ReturnIndex("Tag");
        keyPlan = DeCoder.ReturnIndex("Plan");
        keyGuild = DeCoder.ReturnIndex("Guild");
        keyLegion = DeCoder.ReturnIndex("Legion");
        keyRace = DeCoder.ReturnIndex("Race");
        keyCivilian = DeCoder.ReturnIndex("Civilian");

        keyCardTayp = DeCoder.ReturnIndex("CardTayp");
        keyCardClass = DeCoder.ReturnIndex("CardClass");
        keyStatus = DeCoder.ReturnIndex("Status");
        keyStatGroup = DeCoder.ReturnIndex("StatGroup");
        keyMark = DeCoder.ReturnIndex("Mark");

        Saver.LoadBDAll();
        Saver.LoadAllRule();
        //Saver.Reload();

        LoadScene("Main");
       // OpenScene("Main");
        gameObject.GetComponent<CoreSys>().enabled = true;

       // OpenScene("Main");
    }
    public void LoadScene(string str)
    {
        //switch (mood)
        //{
        //    case ("Rule"):
        //        DeCoder.Read("Sys|Save");
        //        break;
        //    case ("BD"):
        //        DeCoder.Read("Sys|Save");
        //        break;
        //}

        if (str == "Exit")
            Application.Quit();
        else
        {
            mood = str;
            string[] com = mood.Split('|');
            SceneManager.LoadScene(com[0], LoadSceneMode.Single);
        }
    }

    public void OpenScene( BaseUi _ui, Transform transform)
    {
        ui = _ui;
        canvasTransform = transform;


        // BaseUi ui = null;
        //DeCoder.SetMood(str);
        //SceneManager.LoadScene(str, LoadSceneMode.Single);
        //return;

        GameObject go = null;
        //Debug.Log(go); 
        //go = GameObject.FindGameObjectsWithTag("Main")[0];
        //Debug.Log(go);

        //if (go == null)
        //    return;
        string[] com = mood.Split('|');
        Debug.Log(mood);
        switch (com[0])
        {
            case ("Main"):
                ui.ExitButton.onClick.AddListener(() => OpenRedactor());
                string[] name = {"Галлерея","Конструктор карт", "Выход" };
                string[] comS = { "Gallery", "CardCreator| |Main", "Exit"};
                for(int i = 0; i < comS.Length; i++)
                {
                    go = Instantiate(ui.OrigButton);
                    go.transform.SetParent(ui.Menu);
                    SetLoader(go.GetComponent<Button>(), comS[i]);
                    go.transform.GetChild(0).gameObject.GetComponent<Text>().text = name[i];
                }
                break;
            case ("CardCreator"):
                ui.ExitButton.onClick.AddListener(() => LoadScene(com[2]));
                ui.gameObject.GetComponent<CardConstructor>().Load(gameObject.GetComponent<CoreSys>(), com[1]);
                break;
            case ("Gallery"):
                //Gallery.ResetColod(_ui.gameObject.GetComponent<ColodConstructorUi>());
                Gallery.Reset(_ui, _ui.gameObject.GetComponent<ColodConstructorUi>());
                //ui.ExitButton.onClick.AddListener(() => LoadScene("Main"));
                //ui.Buttons[2].onClick.AddListener(() => LoadScene("CardCreator| |Gallery"));
                //ui.gameObject.GetComponent<CardConstructor>().Load(gameObject.GetComponent<CoreSys>(), mood);
                //CardConstructor.Load(mood);


                //string[] s = { "NewCard", "Colod", "Edit" };
                //for (int i = 0; i < s.Length; i++)
                //{
                //    go = Instantiate(ui.OrigButton);
                //    go.transform.GetChild(0).gameObject.GetComponent<Text>().text = s[i];

                //    SceneManager.LoadScene(s[i], LoadSceneMode.Single);
                //    go.transform.SetParent(ui.Menu);
                //}
                break;

        }
       // ui.ExitButton.onClick.AddListener(() => LoadScene("Main"));
        //DeCoder.SetTT(ui.TT, ui.NameTT);
        //DeCoder.Starter(mood);
    }

    void SetLoader(Button button, string str)
    {
        button.onClick.AddListener(() => LoadScene(str));
    }

}
