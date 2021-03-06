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
    BaseUi ui;
    public RuleMainFrame frame;
    public List<BD> bD;
    public List<SubRuleHead> head;
    string mood = "Main";
    //ImageBd

    //private List<TextMeshProUGUI> TT;
    //private TMP_InputField nameTT;

    // Start is called before the first frame update
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

        LoadScene("Main");
       // OpenScene("Main");
        gameObject.GetComponent<CoreSys>().enabled = true;

       // OpenScene("Main");
    }
    void LoadScene(string str)
    {
        if (str == "Exit")
            Application.Quit();
        else
        {
            mood = str;
            SceneManager.LoadScene(str, LoadSceneMode.Single);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PointerClick();
        if (Input.GetMouseButtonDown(1))
        {
           // Ui.TextWindow.active = false;
           // ComandClear();
            // Ui.TextWindow.active = false;
        }
    }
    void PointerClick()
    {
        TMP_LinkInfo linkInfo = new TMP_LinkInfo();
        int linkIndex = -1;
        for(int i = 0; i < ui.TT.Count; i++)
        {
            linkIndex = TMP_TextUtilities.FindIntersectingLink(ui.TT[i], Input.mousePosition, Camera.main);
            if (linkIndex != -1)
            {
                linkInfo = ui.TT[i].textInfo.linkInfo[linkIndex];
                break;
            }
        }
        if(linkIndex == -1)
            return;

        string selectedLink = linkInfo.GetLinkID();
        Debug.Log("Open link " + selectedLink);
        DeCoder.Read(selectedLink);
    }

    public void OpenScene( BaseUi _ui)
    {
        ui = _ui;


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
        string str = "";
        switch (mood)
        {
            case ("Main"):
                string[] name = {"??????","?????", "????????", "???? ??????", "?????" };
                string[] com = {"Colod", "Card" , "Rule", "BD", "Exit"};
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

        }
        ui.ExitButton.onClick.AddListener(() => LoadScene("Main"));
        DeCoder.SetTT(ui.TT, ui.NameTT);
        DeCoder.Starter(mood);
    }

    void SetLoader(Button button, string str)
    {
        button.onClick.AddListener(() => LoadScene(str));
    }

}
