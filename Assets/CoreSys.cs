using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Coder;
using XMLSaver;


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

        Saver.LoadBDAll();
        Saver.LoadAllRule();

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

        LoadScene("Main");
       // OpenScene("Main");
        gameObject.GetComponent<CoreSys>().enabled = true;

       // OpenScene("Main");
    }
    void LoadScene(string str)
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
            SceneManager.LoadScene(str, LoadSceneMode.Single);
        }
    }

    public void OpenScene( BaseUi _ui, Transform transform)
    {
        ui = _ui;
        canvasTransform = transform;
        ui.ExitButton.onClick.AddListener(() => OpenRedactor());


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
        Debug.Log(mood);
        switch (mood)
        {
            case ("Main"):
                string[] name = {"������","�����", "�����" };
                string[] com = {"Colod", "Card" , "Exit"};
                for(int i = 0; i < com.Length; i++)
                {
                    go = Instantiate(ui.OrigButton);
                    go.transform.SetParent(ui.Menu);
                    SetLoader(go.GetComponent<Button>(), com[i]);
                    go.transform.GetChild(0).gameObject.GetComponent<Text>().text = name[i];
                }
                break;
            case ("BD"):
                //TT = ui.TT;
                //nameTT = ui.NameTT;

             //   ui.ExitButton.onClick.AddListener(() => OpenScene("Main"));
                break;
            case ("Rule"):
                //TT = ui.TT;
                //nameTT = ui.NameTT;


             //   ui.ExitButton.onClick.AddListener(() => OpenScene("Main"));
                break;
            case ("Gallery"):

                string[] s = { "NewCard", "Colod", "Edit" };
                for (int i = 0; i < s.Length; i++)
                {
                    go = Instantiate(ui.OrigButton);
                    go.transform.GetChild(0).gameObject.GetComponent<Text>().text = s[i];

                    SceneManager.LoadScene(s[i], LoadSceneMode.Single);
                    go.transform.SetParent(ui.Menu);
                }
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
