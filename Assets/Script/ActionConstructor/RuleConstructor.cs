using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuleConstructor : MonoBehaviour
{
    [SerializeField]
    private ActionLibrary library;
    [SerializeField]
    private RuleConstructorUi Ui;
    /*����--
     ��������
    ��������
    ���������
    ��������� ����� ��������
    ��� ��������� �� ����������� ���� ������
     
    ������� ������� - �����������, �����:
    *�������������++
     */

    /*�������������--
    ����� � ������� �������� ����������� - ��������, ���, ���
    �� ��� ������ - �, ����, ���
    ����� �������� - ������ ����, ����� ����, �������� ������������ ������������. ����� ������, ����� �����, ��� ��������� �����, ��� ����������� ����� � ����, ��� ������ �����
     ������� ��������� �������������++
    �������� ��������� ����������++
     */

    /*����������������--
     * ����� - �����, ���, ��������� (2)
     * ������ - ���, ������, �����
     * 
     * ��������� ������� � �������� �����������, ���� ����� ���������, �� ���������� ���������� ����� ������� //������ ������� - �, ���, , ��������� (2� �����), �� �����, ������
    

    ����������� �������
     ������� - ������, �����, ������
    
    ����������� ��������������
    ���� �������
     
     */

    /*��������--
     * �� ���� ��������� - ���, �����, ������
     ������� �������� - ����, �, ���
     ������������� ������
     */
    #region Base
    private int sysMood=-1;

    public string Name = "�����������";//��������
    public string Info;//��������

    public int Cost;//����
    public int CostExtend;//���� �� ��� ���� �������

    public int LevelCap;//������������ ������� �����������

    public int CostMovePoint;

    public bool Player;

    private string stringMood = "";

    public List<TriggerAction> triggerActions;
    #endregion

    #region test
    private List<string> Mood;
    private List<string> TargetPalyer;
    private List<string> TargetTime;

    [SerializeField]
    private Color[] colors;
    #endregion

    private List<InputField> mainField;


    private string allText;
    private string mainText;

    private string plusText;
    private string minusText;

    private string actionText;

    public TextMeshProUGUI TT;//�������
    #region Main

    void PointerClick()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TT, Input.mousePosition, Camera.main);
        if (linkIndex == -1)
        {
            Debug.Log("Open link -1");
            return;
        }
        Ui.MouseIndicator.position = new Vector3(1300, Input.mousePosition.y, 0);
        //  TMP_LinkInfo linkInfo = textMessage.textInfo.linkInfo[linkIndex];
        //  string selectedLink = linkInfo.GetLinkID();
        TMP_LinkInfo linkInfo = TT.textInfo.linkInfo[linkIndex];
        string selectedLink = linkInfo.GetLinkID();
        DeCoder(selectedLink);
        Debug.Log("Open link " + selectedLink);
    }
    void LoadAllText()
    {
        allText = mainText;
        //allText += plusText;
        //allText += minusText;
        //allText += actionText;
        allText += $"\n\n�������";
        int a = triggerActions.Count;
        for (int i = 0; i < a; i++)
        {
            allText += triggerActions[i].RootText;
            allText += $"\n";
        }
        allText += $"<link=Add_Trigger><color=green>\n�������� �������</color></link>";

        TT.text = allText;
    }

    void IAddLink(int trigger,int rootText, string colorText,string linkText, string linkInfo)
    {
        string text = $"<link={linkText}><color={colorText}>{linkInfo}</color></link>";
        if (trigger < 0)
        {
            mainText += text;
        }
        else
        {
            TriggerAction triggerAction = triggerActions[trigger];
            switch (rootText)
            {
                case (0):
                    triggerAction.MainText += text;
                    break;

                case (1):
                    triggerAction.PlusText += text;
                    break;

                case (2):
                    triggerAction.MinusText += text;
                    break;

                case (3):
                    triggerAction.ActionText += text;
                    break;
            }
        }
    }

    void LoadData()
    {
        string text = Ui.TextInput.text;
        switch (stringMood) 
        {
            case ("Name"):
                Name = text;
                break;
            case ("Cost"):
                int i = int.Parse(text);
                if (i != null)
                    Cost = i;
                break;

            case ("CostExtend"):
                i = int.Parse(text);
                if (i != null)
                    CostExtend = i;
                break;

            case ("LevelCap"):
                i = int.Parse(text);
                if (i != null)
                    LevelCap = i;
                break;

            case ("CostMovePoint"):
                i = int.Parse(text);
                if (i != null)
                    CostMovePoint = i;
                break;

            default:

                string[] com = stringMood.Split('_');
                break;
        }

        Ui.TextWindow.active = false;
        LoadMainText();
        LoadAllText();
    }

    void LoadMainText()
    {
        string color1 = "#F4FF04";
        mainText = "";
        IAddLink(-1,0, color1, "Switch_RuleName", $"������� -- {Name}");
        IAddLink(-1, 0, color1, "Switch_Cost", $"\n����: {Cost}");

        if (CostExtend == 0)
            IAddLink(-1, 0, color1, "Switch_CostExtend", $"\n���� �� ��� ����: -- ���������");
        else
            IAddLink(-1, 0, color1, "Switch_CostExtend", $"\n���� �� ��� ����: -- {CostExtend}");


        if (LevelCap == 0)
            IAddLink(-1, 0, color1, "Switch_LevelCap", $"\n������������ ������� - ��� �����������");
        else
            IAddLink(-1, 0, color1, "Switch_LevelCap", $"\n������������ ������� - {LevelCap}");


        IAddLink(-1, 0, color1, "Switch_CostMovePoint", $"\n����� ���� �� ���������� (? - ����� ������� � ����� �������� ��� ����� ��������?) {CostMovePoint}");

        if (!Player)
            IAddLink(-1, 0, color1, "Switch_Player", $"\n������� ������� - �����������");
        else
            IAddLink(-1, 0, color1, "Switch_Player", $"\n������� ������� - �����");
    }

    void AddIf(int a, bool plus)
    {
        TriggerAction triggerAction = triggerActions[a];

        if (plus)
        {
            triggerAction.PlusAction.Add(new IfAction());
            TriggerPlusText(a);
        }
        else
        {
            triggerAction.MinusAction.Add(new IfAction());
            TriggerMinusText(a);
        }
        TriggerRootText(a);
        LoadAllText();
    }
    void AddTrigger()
    {
        TriggerAction triggerAction = new TriggerAction();
        triggerAction.Id =0;
        triggerAction.Mood = Mood[0];//All. Shot. Melee
        triggerAction.TargetPalyer = TargetPalyer[0];//All. My. Enemy
        triggerAction.TargetTime = TargetTime[0];//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(�������� � ��������)

        //triggerAction.RootText;
        //triggerAction.MainText;
        //triggerAction.PlusText;
        //triggerAction.MinusText;
        //triggerAction.ActionText;


        triggerAction.PlusAction = new List<IfAction>();
        triggerAction.MinusAction = new List<IfAction>();
        triggerAction.Only = false;

        int a = triggerActions.Count;
        triggerActions.Add(triggerAction);
        
        TriggerMainText(a);
        TriggerPlusText(a);
        TriggerMinusText(a);
        TriggerActionText(a);
        TriggerRootText(a);
    }
    #region Trigger Text
    void TriggerMainText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.MainText = "\n------";
        triggerAction.MainText += $"<link=Add_{a}_PlusIf><color=green>-������� �������</color></link>";
        IAddLink(a, 0, "green", $"Trigger_{a}_Id", $"\n-ID({triggerAction.Id})");
        IAddLink(a, 0, "green", $"Trigger_{a}_Mood", $"\n-���� ����: {triggerAction.Mood}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetPalyer", $"\n-����������� �����: {triggerAction.TargetPalyer}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetTime", $"\n-������� ��������: {triggerAction.TargetTime}");
      //  IAddLink(a, 0, "green", $"Trigger_{a}_Only", $"\n-��������� ����� ������ {triggerAction.Only}");

        // triggerAction.MainText = "";
    }
    void TriggerPlusText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.PlusText = "\n\n-�������";
        string text = "";
        int b = triggerAction.PlusAction.Count;
        for (int i = 0; i < b; i++)
        {
            text += "--Data";
            IAddLink(a, 1, "green", $"Trigger_{a}_PlusIf_{i}", text);
        }
        triggerAction.PlusText += $"\n<link=Add_{a}_PlusIf><color=green>-�������� �������</color></link>";
    }
    void TriggerMinusText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.MinusText = "\n\n-����������";
        string text = "";
        int b = triggerAction.MinusAction.Count;
        for (int i = 0; i < b; i++)
        {
            text += "--Data";
            IAddLink(a, 1, "green", $"Trigger_{a}_PlusIf_{i}", text);
        }

        triggerAction.MinusText += $"\n<link=Add_{a}_PlusIf><color=green>-�������� ����������</color></link>";
    }
    void TriggerActionText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.ActionText = "\n\n-��������";
        string text = "";
        //int b = triggerAction.Action.Count;
        //for (int i = 0; i < b; i++)
        //{
        //    text += "--Data";
        //    IAddLink(a, 1, "green", $"Trigger_{a}_PlusIf_{i}", text);
        //}

        triggerAction.ActionText += $"\n<link=Add_{a}_PlusIf><color=green>-�������� ��������</color></link>";
    }
    void TriggerRootText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        string text = $""; 

        text += triggerAction.MainText;
        text += triggerAction.PlusText;
        text += triggerAction.MinusText;
        text += triggerAction.ActionText;
        triggerAction.RootText = text;
        //string text = $"\n\n�������";
        // IAddLink(0, "Green", "Switch_LevelCap", $"+++");


        // IAddLink()
        // plusText = text;
    }
    #endregion
    void LoadTextWindowData(string text, string mood)
    {
        stringMood = mood;
        Ui.TextWindow.active = true;
        Ui.TextInput.text = text;
    }

    void DeCoder(string cod)
    {
        string[] com = cod.Split('_');

        string text = com[1];
        switch (com[0])
        {
            case ("Switch"):
                switch (text)
                {
                    case ("RuleName"):
                        LoadTextWindowData(Name, text);
                        break;

                    case ("Cost"):
                        LoadTextWindowData($"{Cost}", text);
                        break;

                    case ("CostExtend"):
                        LoadTextWindowData($"{CostExtend}", text);
                        break;

                    case ("LevelCap"):
                        LoadTextWindowData($"{LevelCap}", text);
                        break;

                    case ("CostMovePoint"):
                        LoadTextWindowData($"{CostMovePoint}", text);
                        break;
                    case ("Player"):
                        Player = !Player;
                        LoadMainText();
                        LoadAllText();
                       // LoadTextWindowData($"{CostMovePoint}", text);
                        break;
                }
                break;
            //case ("If"):
            //    switch (text) 
            //    {
            //        case ("Add"):
            //            text = com[2];
            //            break;

            //        case ("Plus"):

            //            break;

            //        case ("Minus"):

            //            break;
            //    }
            //    break;
            case ("Add"):
                switch (text)
                {
                    case ("Trigger"):
                        AddTrigger();
                        LoadAllText();
                        break;
                }

                break;
            default:
                int a = int.Parse(text);
                // TriggerAction triggerAction = triggerActions{ }
                switch (text)
                { 
                
                }
                break;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PointerClick();
        if (Input.GetMouseButtonDown(1))
            Ui.TextWindow.active = false;
    }
    void Start()
    {
        LoadBase();

        LoadMainText();
       // AddPlusIf();
        //AddMinusIf();
        //AddActionIf();
        LoadAllText();

    //mainField = new List<InputField>();
    //AddField(0, "Name");
    //AddField(0, "Cost");
    //AddField(0, "CostExtend");
    //AddField(0, "LevelCap");
    //AddField(0, "CostMovePoint");
    //AddField(0, "Player");
}


    void LoadBase()
    {
        Ui.TextWindowButton.onClick.AddListener(() => LoadData());

        Name = "�����������";//��������

        Mood = new List<string>();
        Mood.Add("All");
        Mood.Add("Shot");
        Mood.Add("Melee");

        TargetPalyer = new List<string>();
        TargetPalyer.Add("All");
        TargetPalyer.Add("My");
        TargetPalyer.Add("Enemy");


        TargetTime = new List<string>();
        TargetTime.Add("Action");
        TargetTime.Add("Start Turn");
        TargetTime.Add("End Turn");
        TargetTime.Add("PreAction");
        TargetTime.Add("PostAction");
        TargetTime.Add("PlayCard");
        TargetTime.Add("DeadCard");
        TargetTime.Add("DeadAnotherCard");
        TargetTime.Add("PlayAnotherCard");
        TargetTime.Add("PostDeadTurn");
        //Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(�������� � ��������)

        triggerActions = new List<TriggerAction>();
    }
    #endregion

}
public class TriggerAction
{
    public int Id;
    public string Mood;//All. Shot. Melee
    public string TargetPalyer;//All. My. Enemy
    public string TargetTime;//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(�������� � ��������)

    public string RootText;
    public string MainText;
    public string PlusText;
    public string MinusText;
    public string ActionText;
    
    
    public List<IfAction> PlusAction;//������� ����������
    public List<IfAction> MinusAction;//������� ����������
   // public List<Action> Action;
    public bool Only;//multy skill mood
}
/*Start Turn
 * 
 * 
 */
public class IfAction 
{
    public int Prioritet = 10;//��������� ��������
    public List<string> TextData;//��������� ����
    public List<int> IntData;//������������ ����
    /*����������� ������� 
     Action.- �������� �-�� ���-�� �����, �������� �����

    Start Turn - End Turn
    -- ����� ���� (������ ������ �����)
    -- �������� ������� ��� ����������
    -- (���)��������� ��������� �� ����� (������, ������ �����)
    -- -- ���������� ����� ������������ ��� ������������� ������������ ������� ���������
    -- (���)����� ��������� � ������������ ���� ��� �����
    -- (���)����� ����� ��������� ������� ��� �������
     */
}

