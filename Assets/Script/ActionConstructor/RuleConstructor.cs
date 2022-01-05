using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuleConstructor : MonoBehaviour
{
    [SerializeField]
    private RuleMainFrame frame;
   
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

    private List<string> IfString_L0;
    private List<string> IfString_L1;

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
        Debug.Log("Open link " + selectedLink);
        DeCoder(selectedLink);
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
                //  string text1 = com[1];
                int c = int.Parse(com[0]);
                TriggerAction triggerAction = triggerActions[c];
                i = int.Parse(text);
                if (i != null)
                {
                    if (com.Length > 1)
                    {
                        int i1 = int.Parse(com[2]);
                        int b1 = int.Parse(com[3]);

                        text = com[1];
                        IfAction ifAction = null;

                        switch (text)
                        {
                            case ("Plus"):
                                ifAction = triggerAction.PlusAction[i1];

                                if (b1 == -1)
                                    ifAction.Prioritet = i;
                                else
                                    ifAction.IntData[b1] = i;
                                break;

                            case ("Minus"):
                                ifAction = triggerAction.MinusAction[i1];

                                if (b1 == -1)
                                    ifAction.Prioritet = i;
                                else
                                    ifAction.IntData[b1] = i;
                                break;
                        }
                        Debug.Log(b1);
                        LableIfAction(ifAction, text, c);
                        // TriggerRootText(c);
                        TextReStruct(); 
                    }
                    else
                    {
                        triggerAction.Id = i;
                        TextReStruct();
                    }
                }
                //switch (com[2])
                //{
                //    case ("Plus"):
                //        break;
                //}
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
    void TextReStruct()
    {
        int a = triggerActions.Count;
        for(int i = 0; i < a; i++)
        {
            TriggerMainText(i);
            TriggerPlusText(i);
            TriggerMinusText(i);
            TriggerActionText(i);
            TriggerRootText(i);
        }
        LoadAllText();
    }

    void AddIf(int a, bool plus)
    {
        TriggerAction triggerAction = triggerActions[a];
        IfAction ifAction = new IfAction();

        // int b = 5;
        // for (int i = 0; i < b; i++)
        //  {
        //ifAction.TextData.Add("None");
        //ifAction.IntData.Add(-1);
        // }


        CoreLableIfAction(ifAction, 0, 0);

        if (plus)
        {
            ifAction.LocalId = triggerAction.PlusAction.Count;
            triggerAction.PlusAction.Add(ifAction);
            SwitchLableIfAction(ifAction, 0, 0,"Plus", a);
        }
        else
        {
            ifAction.LocalId = triggerAction.MinusAction.Count;
            triggerAction.MinusAction.Add(ifAction);
            SwitchLableIfAction(ifAction, 0, 0, "Minus",a);
        }
        //TriggerRootText(a);
        //LoadAllText();
    }

    void DelIf(int a, bool plus, int b)
    {
        Ui.TextWindow.active = false;
        TriggerAction triggerAction = triggerActions[a];
        if (plus)
        {
            triggerAction.PlusAction.RemoveAt(b);
            TriggerPlusText(a);
        }
        else
        {
            triggerAction.MinusAction.RemoveAt(b);
            TriggerMinusText(a);
        }
        TriggerRootText(a);
        LoadAllText();
    }
    void AddTrigger()
    {
        TriggerAction triggerAction = new TriggerAction();
        triggerAction.Id =-1;
        triggerAction.Mood = 0;// Mood[0];//All. Shot. Melee
        triggerAction.TargetPalyer = 0;//TargetPalyer[0];//All. My. Enemy
        triggerAction.TargetTime = 0;//TargetTime[0];//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(�������� � ��������)

        triggerAction.PlusAction = new List<IfAction>();
        triggerAction.MinusAction = new List<IfAction>();

        int a = triggerActions.Count;
        triggerActions.Add(triggerAction);
        
        TriggerMainText(a);
        TriggerPlusText(a);
        TriggerMinusText(a);
        TriggerActionText(a);
        TriggerRootText(a);
        LoadAllText();
    }
    void DelTrigger(int a)
    {
        triggerActions.RemoveAt(a);
        TextReStruct();
    }

    #region Trigger Text
    void TriggerMainText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.MainText = "\n------";
        triggerAction.MainText += $"<link=Trigger_{a}_Del><color=green>-������� �������</color></link>";
        IAddLink(a, 0, "green", $"Trigger_{a}_Id", $"\n-ID({triggerAction.Id})");
        IAddLink(a, 0, "green", $"Trigger_{a}_Mood", $"\n-���� ����: {frame.TurnString[triggerAction.Mood]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetPalyer", $"\n-����������� �����: {frame.PlayerString[triggerAction.TargetPalyer]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetTime", $"\n-������� ��������: {frame.Trigger[triggerAction.TargetTime]}");

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
            triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusDel_{i}><color=green>-������� �������</color></link>";
            triggerAction.PlusText += triggerAction.PlusAction[i].Text;
         //   IfActionText(true, triggerAction, a, i);
          //  triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusDel_{i}><color=green>-������� �������</color></link>";
            //text = "\n--Data";
            //IAddLink(a, 1, "green", $"Trigger_{a}_Plus_{i}", text);
        }
        triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusAdd><color=green>-�������� �������</color></link>";
    }
    void TriggerMinusText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.MinusText = "\n\n-����������";
        string text = "";
        int b = triggerAction.MinusAction.Count;
        for (int i = 0; i < b; i++)
        {
            //text += "--Data";
            //IAddLink(a, 2, "green", $"Trigger_{a}_PlusIf_{i}", text);
        }

        triggerAction.MinusText += $"\n<link=Trigger_{a}_MinusAdd><color=green>-�������� ����������</color></link>";
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

    void CoreLableIfAction(IfAction ifAction, int a, int b)
    {
        ifAction.TextData = new List<string>();
        ifAction.IntData = new List<int>();
        ifAction.Core = new List<int>();

        ifAction.MainCore[0] = a;
        ifAction.MainCore[1] = b;
        //ifAction.Core.Add(a);
        //ifAction.Core.Add(b);

        //Debug.Log(frame.AllTriggers[a].Name);
        //Debug.Log(frame.AllTriggers[a].Form[b].Name);

        ifAction.Form = frame.AllTriggers[a].Form[b];
    }
    void SwitchLableIfAction(IfAction ifAction, int c, int d, string ifText, int t)
    {
        //ifAction.TextData = new List<string>();
        //ifAction.IntData = new List<int>();

        int cAlt = c + 1;
        int b = ifAction.Core.Count;
      //  Debug.Log($"{b} {c} {d}   ");
        if (cAlt > b)
        {
          //  Debug.Log($"{b} {c} {d}   ");
            ifAction.Core.Add(d);
        }
        else //if (c == b)
        {
         //   Debug.Log($"{b} {c} {d}   ");
          //  Debug.Log(ifAction.Core[c]);
            ifAction.Core[c] = d;

          //  Debug.Log($"{b} {c} {d}   ");
            if (cAlt < b) 
                for (int i = c; i < b; i++)
                {
                    ifAction.Core.RemoveAt(cAlt);
                }
        }

      //  Debug.Log($"{b} {c} {d}   ");
        //if (c > b)
        //{
        //    Debug.Log($"{b} {c} {d}   ");
        //    ifAction.Core.Add(d);

        //    //ifAction.TextData = new List<string>();
        //    //ifAction.IntData = new List<int>();

        //}
        //else
        //{
        //    Debug.Log($"{b} {c} {d}   ");
        //    ifAction.Core[c] = d;
        //    b = ifAction.Core.Count;
        //    if(c<b)
        //    for (int i = c; i < b; i++)
        //    {
        //        ifAction.Core.RemoveAt(c);
        //    }
        //    Debug.Log($"{b} {c} {d}   ");
        //}

       // Debug.Log($"{b} {c} {d}   ");
        PreLableIfAction(ifAction);

        LableIfAction(ifAction, ifText,t);
        //ifAction.TextData = new List<string>();
        //ifAction.IntData = new List<int>();
    }
    void PreLableIfAction(IfAction ifAction)
    {
        RuleFarmeMacroCase form = null;
        int b = ifAction.Core.Count;
        int a2 = 0;
        int a3 = 0;
        int b1 = 0;
        //  for (int i = 0; i < b; i++)
        for (int i = 0; i < b; i++)
        {
            form = ifAction.Form.Form[ifAction.Core[i]];

            b1 = form.Form.Length;
            for (int i1 = 0; i1 < b1; i1++)
            {
                if(a2 <= ifAction.IntData.Count)
                {
                    ifAction.IntData.Add(0);
                    ifAction.TextData.Add("");
                }

                a3 = ifAction.IntData[a2];
                switch (form.Form[i1].Rule)
                {
                    case ("Legion"):
                        if (a3 <= library.Legions.Count)
                        {
                           // Debug.Log(a3);
                            ifAction.IntData[a2] = 0;
                            ifAction.TextData[a2] = library.Legions[0].Name;
                        }
                        else
                            ifAction.TextData[a2] = library.Legions[a3].Name;
                        break;
                    case ("Constant"):
                        if (a3 <= library.Constants.Count)
                        {
                         //   Debug.Log(a3);
                            ifAction.IntData[a2] = 0;
                            ifAction.TextData[a2] = library.Constants[0].Name;
                        }
                        else
                            ifAction.TextData[a2] = library.Constants[a3].Name;
                        break;

                    case ("Group"):
                        if (a3 <= library.CivilianGroups.Count)
                        {
                           // Debug.Log(a3);
                            ifAction.IntData[a2] = 0;
                            ifAction.TextData[a2] = library.CivilianGroups[0].Name;
                        }
                        else
                            ifAction.TextData[a2] = library.CivilianGroups[a3].Name;
                        break;

                    case ("GroupLevel"):
                        if (a3 <= library.CivilianGroups.Count)
                        {
                            ifAction.IntData[a2] = 0;
                            ifAction.TextData[a2] = library.CivilianGroups[0].Name;
                        }
                        else
                            ifAction.TextData[a2] = library.CivilianGroups[a3].Name;
                        break;

                    case ("Int"):
                        ifAction.TextData[a2] = "0";
                        ifAction.IntData[a2] = 0;
                        break;

                    case ("Bool"):
                        if (a3 <= frame.BoolString.Length)
                        {
                         //   Debug.Log(a3);
                            ifAction.IntData[a2] = 0;
                            ifAction.TextData[a2] = frame.BoolString[0];
                        }
                        else
                            ifAction.TextData[a2] = frame.BoolString[a3];
                        break;

                    case ("Equal"):
                        if (a3 <= frame.EqualString.Length)
                        {
                          //  Debug.Log(a3);
                            ifAction.IntData[a2] = 0;
                            ifAction.TextData[a2] = frame.EqualString[0];
                        }
                        else
                            ifAction.TextData[a2] = frame.EqualString[a3];
                        break;
                    default:
                        Debug.Log($"!Erorr! {form.Form[i].Rule}");
                        break;
                }

                a2++;
            }

        }
    }


    void LableIfAction(IfAction ifAction, string ifText, int t)
    {
        string linkText = "";
        string colorText = "#F4FF04";

        int b = ifAction.Core.Count;
        int b1 = 0;
        int a = ifAction.MainCore[0];
        int a1 = ifAction.MainCore[1];
        int a2 = 0;
        RuleFarmeMacroCase form = null;
        string text = "";
        string text1 = "";
        string text2 = "";


        linkText = $"Trigger_{t}_Int_{ifAction.LocalId}_-1_{ifText}";

        text2 = $"\n\n-���������({ifAction.Prioritet})  ";

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";



        linkText = $"Trigger_{t}_Core1_{ifAction.LocalId}_-1_{ifText}";

        text2 = "\n���� ��������: " + frame.AllTriggers[a].Name;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";



        linkText = $"Trigger_{t}_Core2_{ifAction.LocalId}_-1_{ifText}";

        text2 = "\n����������� ��������: " + frame.AllTriggers[a].Form[a1].Name;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";








        int l = ifAction.LocalId;
        for (int i = 0; i < b; i++)
        {
            int a3 = -1;
            int a4 = -1;

            a = ifAction.Core[i];

            form = ifAction.Form.Form[a];

            a1 = form.Id;
            b1 = form.Form.Length;
            text1 += "\n";
            for (int i1 = 0; i1 < a1; i1++)
            {
                text1 += "    ";
            }

            a4 = ifAction.Form.Form.Count;
            for (int i1 = i; i1 < a4; i1++)
            {
               // Debug.Log($"{a} {ifAction.Form.Form.Count - 1} {i1} {b1}");
                if (a1 > ifAction.Form.Form[i1].Id)
                    i1 = a4;

               // Debug.Log($"Ok");
                if (a1 == ifAction.Form.Form[i1].Id)
                    if (a3 == -1)
                    {
                        a3 = i1;
                    }
                    else
                    {
                       // Debug.Log($"i1_{i1}");
                        text2 = $"����� - ";
                        linkText = $"Trigger_{t}_NextData_{l}_{i}_{ifText}";
                        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
                        //   Debug.Log($"Ok");
                        i1 = a4;
                    }
            }

              
            //    NextData
            for (int i1 = 0; i1 < b1; i1++)
            {
              //  Debug.Log($"Ok");
               // Debug.Log($"{a} {ifAction.Form.Form.Count - 1} {i1} {b1}");
                text1 += "---";
                text = form.Form[i1].Rule;

                linkText = $"Trigger_{t}_{text}_{l}_{a2}_{ifText}";

                text = ifAction.TextData[a2];

                //   string text = $"<link={linkText}><color={colorText}>{text2}</color></link>";
                text2 = form.Form[i1].Text;

                text2 = text2.Replace("{Rule}", $" {text} ");


                text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
               // text1 += "-{" + form.Form[i1].Rule + "} " + text2;
                a2++;
              //  switch(){}
            }

        }
        //������ ������� ������� �������� � ����� ������������

     //   Debug.Log($"{a} {ifAction.Form.Form.Count-1} ");
        //Debug.Log(ifAction.Form.Form[a].Id);
        //Debug.Log(ifAction.Form.Form[a+1].Id);
        if (a < ifAction.Form.Form.Count-1)
            if (ifAction.Form.Form[a].Id < ifAction.Form.Form[a+1].Id)
            {
                text2 = "\n�����";
                linkText = $"Trigger_{t}_NewData_{l}_{a+1}_{ifText}";
                text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
            }

        //int a3 = ifAction.Core[ifAction.Core.Count - 1];
        //a2 = ifAction.MainCore[1];
        //if (b <= frame.AllTriggers[a].Form[a2].Form.Count)
        //    if(a1)
        //text2 = "\n�����";
        //linkText = $"Trigger_{t}_NextData_{l}_{b}_{ifText}";
        //text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";





        ifAction.Text = text1;


        switch (ifText)
        {
            case ("Plus"):
                TriggerPlusText(t);
                break;
            case ("Minus"):
                TriggerMinusText(t);
                break;
            default:
                Debug.Log(ifText);
                break;
        }
        TriggerRootText(t);
        LoadAllText();

    }


    //void NewMethod(IfAction ifAction)
    //{
    //    string text = "";
    //    int a = frame.AllTriggers.FindIndex(x => x.Name == ifAction.TextData[0]);
    //    if (a == -1)
    //    {
    //        a = 0;
    //    }

    //    RuleBaseFrame altFrame = frame.AllTriggers[a];

    // //   a = altFrame.Form.FindIndex(x => x.Name == text);
     
    //    int b = altFrame.Form.FindIndex(x => x.Name == ifAction.TextData[1]);
    //    if (b == -1)
    //        b = 0;

    //    RuleFrame form = frame.AllTriggers[a].Form[b];
    //    a = form.Form.Count;
    //    for (int i = 0; i < a; i++)
    //    {

    //    }
    //}

    void IfActionText(bool plus, TriggerAction triggerAction, int a, int i)
    {
        IfAction ifAction = triggerAction.PlusAction[i];
        string text = ifAction.TextData[0];
        string text1 = "";
        string textLink = "";

        ////0-trigger     1-if & else & action     2- IfAction num     3-string num  com[5] =libray(Legion)
        //stringMood = $"{i}_0_{com[3]}_{com[4]}";

        //stringMood = $"{i}_1_{com[3]}_{com[4]}";

        // b = int.Parse(com[3]);
        //int b1 = int.Parse(com[4]);
        //stringMood = $"{i}_{com[5]}_{b}_{b1}";

        /*text1 +=$"���������({ifAction.Prioritet})"
         
         
         */
        text1 += $"\n\n-���������({ifAction.Prioritet})  ";
        if (plus)
        {
            textLink = $"Trigger_{a}_Int_{i}_-1_Plus";
            IAddLink(a, 1, "green", textLink, text1);
            IAddLink(a, 1, "red", $"Trigger_{a}_PlusDel_{i}", "-������� �������");
        }
        else
        {
            textLink = $"Trigger_{a}_Int_{i}_-1_Minus";
            IAddLink(a, 2, "green", textLink, text1);
            IAddLink(a, 2, "red", $"Trigger_{a}_MinusDel_{i}", "-������� ����������");
        }
        //textLink = $"Trigger_{a}_Minus_{i}_SelectTarget";
        //IAddLink(a, 2, "green", textLink, text1);

        // triggerAction.PlusText += $"\n<link={textLink}><color=green>-���������: {text}</color></link>";


        //0-trigger     1-if & else & action     2- IfAction num     3-string num  com[5] =libray(Legion)
        //stringMood = $"{i}_0_{com[3]}_{com[4]}";

        //  ������������� ����� ��������� ���������� �� ������ ������ �������


        //frame.AllTiggers


        text1 = $"\n-���������: {text}";
        if(plus)
            textLink = $"Trigger_{a}_Plus_{i}_0_Select";
        else
            textLink = $"Trigger_{a}_Minus_{i}_0_Select";




        //  textLink = $"Trigger_{a}_Minus_{i}_0_{text}";
        IAddLink(a, 1, "green", textLink, text1);
        switch (text)
        {

            case ("Creature")://��������� ������� ��������
                text = ifAction.TextData[1];

                text1 = $"\n����������� ���������: {text}";

                if (plus)
                    textLink = $"Trigger_{a}_Plus_{i}_1_Select";
                else
                    textLink = $"Trigger_{a}_Minus_{i}_1_Select";

                IAddLink(a, 1, "green", textLink, text1);
                switch (text)
                {
                    case ("Legion"):
                        if (plus)
                            textLink = $"Trigger_{a}_Plus_{i}_2_SwitchBool";
                        else
                            textLink = $"Trigger_{a}_Minus_{i}_2_Select";
                        text1 = $" {ifAction.TextData[2]}";
                        IAddLink(a, 1, "green", textLink, text1);
                        if (plus)
                            textLink = $"Trigger_{a}_Plus_{i}_3_Select";
                        else
                            textLink = $"Trigger_{a}_Minus_{i}_3_Select";
                        text1 = $" {ifAction.TextData[3]}";
                        // text1 = $" {ifAction.TextData[2]}";
                        IAddLink(a, 1, "green", textLink, text1);
                        // triggerAction.PlusText += "����������� ���������: ������ (�����\�������) {ifAction.TextData[2]}";
                        // if(ifAction.TextData[2] ="")
                        break;

                    case ("Civilian"):
                        break;

                    case ("Creature"):
                        break;

                    default:
                      //  text1 = "\n����������� ���������";
                      //  IAddLink(a, 1, "green", textLink, text1);
                        break;
                }
                /*
                 �������� �������: ��������, ������, ��������
                    ���� ������
                        ������� �������� �������
                    ���� ��������
                        ������� ������ 
                            (���)������� ������
                                ������, ������, �����(�� ���������)
                    ���� ��������
                        ������������� ��� ������
                            ���� ������
                                �� 
                                �������
                                ������, ������, �����
                                ��������
                                �������� (�� ��������� ���������)

                 */
                break;

            case ("Creatures")://��������� ���-�� �������
                break;
            case ("AllCreatures")://��������� ���� �������
                break;

            case ("Head")://��������� ������
                break;

            case ("TargetCreature")://��������� ��������� ��������
                break;
            case ("Stol")://��������� ������� �����
                break;

            case ("UseCard")://��������� �������������� �����
                break;
                /*
       * ��������� �������� ��������
       * 
       ��������� ���-�� �������
      ���� ���������
      ���-�� ������� � �� ��������� ����� ������ ������ �����
          ������� - (�������� ������ ������ �����)
              �������� 
      ���
      ��
      ������ ��� ������  

       */
        }
        // triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusDel_{i}><color=green>-������� �������</color></link>";
        // text = "\n--Data";
        // IAddLink(a, 1, "green", $"Trigger_{a}_Plus_{i}", text);

        //  triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusAdd><color=green>-�������� �������</color></link>";

        //if(plus) 
        //    triggerAction.PlusText += text;
    }
    #endregion
    void LoadTextWindowData(string text, string mood)
    {
        stringMood = mood;
        Ui.TextWindow.active = true;
        Ui.TextInput.text = text;
    }

    void OpenSelector(string text)
    {
        HideSelector();
        switch (text)
        {
            case ("Legion"):
                Ui.SelectorMainLegion.active = true;
                break;

            case ("CivilianGroups"):
                Ui.SelectorMainCivilianGroups.active = true;
                break;

            case ("Constants"):
                Ui.SelectorMainConstants.active = true;
                break;

            case ("Effects"):
                Ui.SelectorMainEffects.active = true;
                break;

            case ("Select"):
             //   LoadSelector();
              //  Ui.SelectorsMain[5].active = true;
                break;
        }
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
            case ("Trigger"):
                int i = int.Parse(text);
                if (i != null)
                {
                    int b = 0;
                    int b1 = 0;
                    TriggerAction triggerAction = triggerActions[i];

                    IfAction ifAction = null;
                    if (com.Length > 5)
                    {
                        b = int.Parse(com[3]);
                        b1 = int.Parse(com[4]);
                        switch (com[5])
                        {
                            case ("Plus"):
                                ifAction = triggerActions[i].PlusAction[b];
                                break;

                            case ("Minus"):
                                ifAction = triggerActions[i].MinusAction[b];
                                break;
                        }
                    }
                    switch (com[2])
                    {
                        case ("Del"):
                            DelTrigger(i);
                            break;

                        case ("PlusDel"):
                            DelIf(i, true, b);
                            break;

                        case ("MinusDel"):
                            DelIf(i, false, b);
                            break;

                        case ("PlusAdd"):
                            AddIf(i, true);
                            break;

                        case ("MinusAdd"):
                            AddIf(i, false);
                            break;

                        case ("Id"):
                            LoadTextWindowData($"{triggerAction.Id}", $"{i}");
                            break;

                        case ("Mood"):
                            b = triggerAction.Mood;
                            b++;
                            if (b == frame.TurnString.Length)
                                b = 0;
                            triggerAction.Mood = b;

                            TriggerMainText(i);
                            TriggerRootText(i);
                            LoadAllText();
                            break;

                        case ("TargetPalyer"):
                            b = triggerAction.TargetPalyer;
                            b++;
                            if (b == frame.TurnString.Length)
                                b = 0;
                            triggerAction.TargetPalyer = b;

                            TriggerMainText(i);
                            TriggerRootText(i);
                            LoadAllText();
                            break;

                        case ("TargetTime"):
                          //  stringMood = $"{i}";
                            b = triggerAction.TargetTime;
                            b++;
                            if (b == frame.Trigger.Length)
                                b = 0;
                            triggerAction.TargetTime = b;

                            TriggerMainText(i);
                            TriggerRootText(i);
                            LoadAllText();
                            // Ui.SelectorsMain[0].active = true;
                            break;


                        case ("Bool"):
                            int a = ifAction.IntData[b1];
                            text = "";

                            a++;
                            if (a >= frame.BoolString.Length)
                                a = 0;

                            text = frame.BoolString[a];
                            ifAction.IntData[b1] = a;
                            ifAction.TextData[b1] = text;
                            LableIfAction(ifAction, com[5],i);

                            break;
                        case ("Equal"):
                            a = ifAction.IntData[b1];
                            text = "";

                            a++;
                            if (a >= frame.EqualString.Length)
                                a = 0;

                            text = frame.EqualString[a];
                            ifAction.IntData[b1] = a;
                            ifAction.TextData[b1] = text;
                            LableIfAction(ifAction, com[5],i);
                            break;

                        case ("Core1"):
                            a = ifAction.MainCore[0];
                            text = "";

                            a++;
                            if (a >= frame.AllTriggers.Count)
                                a = 0;

                            CoreLableIfAction(ifAction, a, 0);
                            SwitchLableIfAction(ifAction, 0, 0, com[5], i);

                            break;

                        case ("Core2"):
                            b = ifAction.MainCore[0];
                            a = ifAction.MainCore[1];
                            text = "";

                            a++;
                            if (a >= frame.AllTriggers[b].Form.Count)
                                a = 0;

                            CoreLableIfAction(ifAction, b, a);
                            SwitchLableIfAction(ifAction, 0, 0, com[5], i);

                            break;

                        case ("NextData"):

                            a = ifAction.Core[b1];
                            int a1 = ifAction.Form.Form[a].Id;
                        //    Debug.Log(a1);
                            bool load = false;
                            int a4 = ifAction.Form.Form.Count;

                            //if (a == a4)
                            //    a = 0;
                            //else
                            for (int i1 = a+1; i1 < a4; i1++)
                            {
                                // Debug.Log($"{a} {ifAction.Form.Form.Count - 1} {i1} {b1}");
                                if (a1 > ifAction.Form.Form[i1].Id)
                                    i1 = a4;

                                Debug.Log($"{a} {a4}");
                                // Debug.Log($"Ok");
                                if (a1 == ifAction.Form.Form[i1].Id)
                                {
                                    a = i1;
                                    load = true;
                                    i1 = a4;
                                }
                            }

                            if (!load)
                            {
                                Debug.Log($"{a} {a4}");
                                int  a3 = -1;
                                for (int i1 = a; i1 > 0; i1--)
                                {
                                    // Debug.Log($"{a} {ifAction.Form.Form.Count - 1} {i1} {b1}");
                                    if (a1 > ifAction.Form.Form[i1].Id)
                                    {
                                        a3 = i1;
                                        i1 = 0;
                                    }

                                    if(a3 ==-1)
                                        if (i1 == 1)
                                            a3 = 0;
                                }
                                //  Debug.Log(a3);
                                Debug.Log($"{a} {a4} {a3}");
                                for (int i1 = a3; i1 < a; i1++)
                                {
                                    // Debug.Log($"{a} {ifAction.Form.Form.Count - 1} {i1} {b1}");
                                    if (a1 > ifAction.Form.Form[i1].Id)
                                        i1 = a4;

                                    // Debug.Log($"Ok");
                                    if (a1 == ifAction.Form.Form[i1].Id)
                                    {
                                        a = i1;
                                        load = true;
                                        i1 = a4;
                                    }
                                }
                            }







                            Debug.Log($"{a} {a4}");

                            //a++;
                            //if (a >= ifAction.Form.Form.Count)
                            //    a = 0;
                           // Debug.Log($"load  {a}  {frame.AllTriggers[b].Form[b1].Form.Count}");
                            //c(b)= num  b1 - int
                            SwitchLableIfAction(ifAction, b1, a, com[5], i);

                            break;

                        case ("NewData"):
                            a = ifAction.Core.Count;

                            SwitchLableIfAction(ifAction,  b1,a, com[5], i);

                            break;


                        case ("Legion"):
                            stringMood = $"{i}_{com[5]}_{com[3]}_{com[4]}";
                            Ui.SelectorMainLegion.active = true;
                            break;

                        //case ("Int"):

                        //    stringMood = $"{i}_{com[5]}_{com[3]}_{com[4]}";
                        //  //  linkText = $"Trigger_{t}_Int_{ifAction.LocalId}_-1_{ifText}";
                        //   // text = ifAction.


                        //    LoadTextWindowData($"{triggerAction.Id}", $"{i}");
                        //    //a = ifAction.Core[b1];
                        //    //a++;
                        //    //if (a >= ifAction.Form.Form.Count)
                        //    //    a = 0;
                        //    //// Debug.Log($"load  {a}  {frame.AllTriggers[b].Form[b1].Form.Count}");
                        //    ////c(b)= num  b1 - int
                        //    //SwitchLableIfAction(ifAction, b1, a, com[5], i);

                        //    break;



                        //case ("Plus"):
                        //    //0-trigger     1-if & else & action     2- IfAction num     3-string num  com[5] =libray(Legion)
                        //    stringMood = $"{i}_{com[5]}_{com[3]}_{com[4]}";
                        //    PreSelectorStringIF(triggerActions[i].PlusAction[b], b1);
                        //   // OpenSelector(com[5]);
                        //    break;

                        //case ("Minus"):
                        //    //0-trigger     1-if & else & action     2- IfAction num     3-string num  com[5] =libray(Legion)
                        //    stringMood = $"{i}_1_{com[3]}_{com[4]}";
                        //    PreSelectorStringIF(triggerActions[i].PlusAction[b], b1);
                        //    break;

                        case ("Int"):


                            stringMood = $"{i}_{com[5]}_{b}_{b1}";

                            Ui.TextWindow.active = true;

                            switch (com[5])
                            {
                                case ("Plus"):
                                    if (b1 == -1)
                                        Ui.TextInput.text = "" + triggerAction.PlusAction[b].Prioritet;
                                    else
                                        Ui.TextInput.text = "" + triggerAction.PlusAction[b].IntData[b1];
                                    break;
                                case ("Minus"):
                                    if (b1 == -1)
                                        Ui.TextInput.text = "" + triggerAction.MinusAction[b].Prioritet;
                                    else
                                        Ui.TextInput.text = "" + triggerAction.MinusAction[b].IntData[b1];
                                    break;
                            }
                            break;

                    }
                }

                break;
            case ("Add"):
                switch (text)
                {
                    case ("Trigger"):
                        AddTrigger();
                        break;
                }

                break;
            default:
              //  int a = int.Parse(text);
                // TriggerAction triggerAction = triggerActions{ }
                //switch (text)
                //{ 
                
                //}
                break;
        }
    }

    #region Add Function 
 

 

    void HideSelector()
    {
        Ui.SelectorMainLegion.active = false;
        Ui.SelectorMainCivilianGroups.active = false;
        Ui.SelectorMainConstants.active = false;
        Ui.SelectorMainEffects.active = false;
        //int a = Ui.Selectors.Count;
        //for(int i = 0; i < a; i++)
        //{
        //    Ui.SelectorsMain[i].active = false;
        //}
    }
    #endregion
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PointerClick();
        if (Input.GetMouseButtonDown(1))
        {
            Ui.TextWindow.active = false;
            HideSelector();
           // Ui.TextWindow.active = false;
        }
    }
    void Start()
    {
        Application.targetFrameRate = 30;

        LoadBase();

        LoadMainText();
        LoadAllText();

    }

    void CreateListButton(string text)
    {
        GameObject GO = null;
        int a = 0; 
        switch (text)
        {
            case ("Legion"):
                a = library.Legions.Count;
                break;

            case ("CivilianGroups"):
                a = library.CivilianGroups.Count;
                break;

            case ("Constants"):
                a = library.Constants.Count;
                break;

            case ("Effects"):
                a = library.Effects.Count;
                break;

            //case ("IfCore1"):
            //    a = frame.AllTriggers.Count;
            //    break;

            //case ("IfCore2"):
            //    a = frame.AllTriggers..Count;
                //break;
        }

        for (int i = 0; i < a; i++)
        {
            GO = Instantiate(Ui.ButtonOrig);
            switch (text)
            {
                case ("Legion"):
                    GO.transform.SetParent(Ui.SelectorLegion);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.Legions[i].Name;
                    break;

                case ("CivilianGroups"):
                    GO.transform.SetParent(Ui.SelectorCivilianGroups);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.CivilianGroups[i].Name;
                    break;

                case ("Constants"):
                    GO.transform.SetParent(Ui.SelectorConstants);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.Constants[i].Name;
                    break;

                case ("Effects"):
                    GO.transform.SetParent(Ui.SelectorEffects);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.Effects[i].Name;
                    break;
            }
            ButtonSelector(text, i, GO.GetComponent<Button>());
        }
    }
    void LoadBase()
    {
        Ui.TextWindowButton.onClick.AddListener(() => LoadData());

        Name = "�����������";//��������

        triggerActions = new List<TriggerAction>();



        GameObject GO = null;
        //Ui.SelectorsMain = new List<GameObject>();
        //Ui.Selectors = new List<Transform>();

        //  string[] text = new string[] {"Legion", "CivilianGroups", "Constants", "Effects", "IfCore1", "IfCore2" };
        string[] text = new string[] { "Legion", "CivilianGroups", "Constants", "Effects"};
        int a = text.Length;
        for (int i = 0; i < a; i++)
        {
            GO = Instantiate(Ui.SelectorMain);
            GO.transform.SetParent(Ui.Canvas);
            GO.transform.position = Ui.SelectorMain.transform.position;
           // Ui.SelectorsMain.Add(GO);
          //  Ui.Selectors.Add(GO.transform.GetChild(0).GetChild(0));

            switch (text[i]) 
            {
                case ("Legion"):
                    Ui.SelectorMainLegion = GO;
                    Ui.SelectorLegion = GO.transform.GetChild(0).GetChild(0);
                    break;

                case ("CivilianGroups"):
                    Ui.SelectorMainCivilianGroups = GO;
                    Ui.SelectorCivilianGroups = GO.transform.GetChild(0).GetChild(0);
                    break;

                case ("Constants"):
                    Ui.SelectorMainConstants = GO;
                    Ui.SelectorConstants = GO.transform.GetChild(0).GetChild(0);
                    break;

                case ("Effects"):
                    Ui.SelectorMainEffects = GO;
                    Ui.SelectorEffects = GO.transform.GetChild(0).GetChild(0);
                    break;

                //case ("IfCore1"):
                //    Ui.SelectorMainIfCore1 = GO;
                //    Ui.SelectorIfCore1 = GO.transform.GetChild(0).GetChild(0);
                //    break;

                //case ("IfCore2"):
                //    Ui.SelectorMainIfCore2 = GO;
                //    Ui.SelectorIfCore2 = GO.transform.GetChild(0).GetChild(0);
                //    break;
            }

            CreateListButton(text[i]);
        }
    }
    void ButtonSelector(string text, int a, Button button)
    {
        button.onClick.AddListener(() => SwitchLibrary(a, text));



        //switch (b)
        //{
        //    case (0):
        //        button.onClick.AddListener(() => SwitchTargetTime(a));
        //        break;
        //    default:
        //        b--;
        //        button.onClick.AddListener(() => SwitchLibrary(a, b));
        //        break;
        //}
    }
    void SwitchLibrary(int a, string text1)
    {
        string text = "";
        switch (text1)
        {
            case ("Legion"):
                text = library.Legions[a].Name;
                break;

            case ("CivilianGroups"):
                text = library.CivilianGroups[a].Name;
                break;

            case ("Constants"):
                text = library.Constants[a].Name;
                break;

            case ("Effects"):
                text = library.Effects[a].Name;
                break;
        }

        string[] com = stringMood.Split('_');
        //0-trigger     1-if & else & action     2- IfAction num     3-string num

        text1 = com[1];

        int i1 = int.Parse(com[0]);
        int i2 = int.Parse(com[2]);
        int i = int.Parse(com[3]);
        TriggerAction triggerAction = triggerActions[i1];
        IfAction ifAction = null;
        switch (text1)
        {
            case ("Plus"):
                ifAction = triggerAction.PlusAction[i2];
                break;

            case ("Minus"):
                ifAction = triggerAction.MinusAction[i2];
                break;

            //case (2):

            //    break;
        }

        ifAction.TextData[i] = text;
        ifAction.IntData[i] = a;


        HideSelector();
        LableIfAction(ifAction, text1, i1);
        //TriggerRootText(i1);
        //LoadAllText();
    }

    void UnHideSelectorButton(Transform transform, string text, int l)
    {
        int a = 0;
        switch (l)
        {
            case (0):
                a = IfString_L0.FindIndex(x => x == text);
                break;
            case (1):
                a = IfString_L1.FindIndex(x => x == text);
                break;
        }
        Debug.Log(a);
        if (a != -1)
            transform.GetChild(a).gameObject.active = false;

    }

    void PostSelectorStringIF(IfAction ifAction, int l, string text)
    {
        switch (l)
        {
            case (0):
                //ifAction.TextData = new List<string>;
                //ifAction.IntData = new List<int>;

                ifAction.TextData.Add(text);
                ifAction.IntData.Add(0);

                break;

            case (1):
                switch (text) 
                {
                    case ("Legion"):
                        break;
                }

                break;

            default:
                if (ifAction.TextData.Count <= l)
                {
                    ifAction.TextData.Add("None");
                    ifAction.IntData.Add(0);
                }
                break;
        }

        //else
        //{

        //}
      //  HideSelector();
       
    }


    void PreSelectorStringIF(IfAction ifAction, int l)
    {
        Ui.TextWindow.active = false;
        int c= 0;
        l++;
        //if (l != 0) 
            c = 4 + l;

     //   Ui.SelectorsMain[c].active = true;
        string text = ifAction.TextData[l]; 
    //    Transform transform = Ui.Selectors[c];
        int a = transform.childCount;
        //for (int i = 0; i < a; i++)
        //{
        //    transform.GetChild(i).gameObject.active = false;
        //}
        //switch (l)
        //{
        //    case (0):
        //        UnHideSelectorButton(transform, "Creature", l);
        //        break;


        //    case (1):
        //        string text1 = ifAction.TextData[0];
        //        switch (text1)
        //        {
        //            //global
        //            case ("Creature")://��������� ������� �������

        //                UnHideSelectorButton(transform, "Legion", l);
        //                UnHideSelectorButton(transform, "Creature", l);
        //                UnHideSelectorButton(transform, "Constant", l);

        //                break;
        //            case ("Creatures")://��������� ���-�� �������

        //                UnHideSelectorButton(transform, "Legion", l);
        //                UnHideSelectorButton(transform, "Creature", l);
        //                UnHideSelectorButton(transform, "Constant", l);

        //                break;
        //            case ("AllCreatures")://��������� ���� �������

        //                UnHideSelectorButton(transform, "Legion", l);
        //                UnHideSelectorButton(transform, "Creature", l);
        //                UnHideSelectorButton(transform, "Constant", l);

        //                break;

        //            case ("Head")://��������� ������
        //                break;

        //            case ("TargetCreature")://��������� ��������� ��������
        //                break;
        //            case ("Stol")://��������� ������� �����
        //                break;

        //            case ("UseCard")://��������� �������������� �����
        //                break;
        //        }
        //        break;

        //    case (2):
        //        text1 = ifAction.TextData[1];
        //        switch (text)
        //        {
        //            case ("Legion"):
        //                //UnHideSelectorButton(transform, "Legion", l);
        //                //UnHideSelectorButton(transform, "Creature", l);
        //                //UnHideSelectorButton(transform, "Constant", l);

        //                break;
        //            case ("Creature"):
        //                break;
        //            case ("Constant"): 
        //                break;
        //        }
        //        break;
        //}
    }

    //void SwitchLibraryExtend(TriggerAction triggerAction, int b, string text, bool plus)
    //{
    //    int c = 0;
    //    IfAction ifAction = null;
    //    if(plus)
    //        ifAction = triggerAction.PlusAction[b];
    //    else
    //        ifAction = triggerAction.MinusAction[b];

    //    switch (text)
    //    {
    //        //global
    //        case ("Creature")://��������� ������� �������
    //            /*
    //            �������� �������: ��������, ������, ��������
    //               ���� ������
    //                   ������� �������� �������
    //               ���� ��������
    //                   ������� ������ 
    //                       (���)������� ������
    //                           ������, ������, �����(�� ���������)
    //               ���� ��������
    //                   ������������� ��� ������
    //                       ���� ������
    //                           �� 
    //                           �������
    //                           ������, ������, �����
    //                           ��������
    //                           �������� (�� ��������� ���������)

    //            */

    //            c = IfString.FindIndex(x => x == "Legion");
    //            if (c != -1)
    //                UnHideSelectorButton(Ui.Selectors[4], c);

    //            c = IfString.FindIndex(x => x == "Creature");
    //            if (c != -1)
    //                UnHideSelectorButton(Ui.Selectors[4], c);
    //            //int c = card.Trait.FindIndex(x => x == "Fast");
    //            //text1 = $"����������� ���������: ������:";
    //            //IAddLink(a, 1, "green", textLink, text1);
    //            //text1 = $" (�����/�������)";
    //            //IAddLink(a, 1, "green", textLink, text1);
    //            //// text1 = $" {ifAction.TextData[2]}";
    //            //IAddLink(a, 1, "green", textLink, text1);
    //            //// triggerAction.PlusText += "����������� ���������: ������ (�����\�������) {ifAction.TextData[2]}";
    //            //// if(ifAction.TextData[2] ="")
    //            break;
    //        case ("Creatures")://��������� ���-�� �������

    //            break;
    //        case ("AllCreatures")://��������� ���� �������

    //            break;

    //        case ("Head")://��������� ������
    //            break;

    //        case ("TargetCreature")://��������� ��������� ��������
    //            break;
    //        case ("Stol")://��������� ������� �����
    //            break;

    //        case ("UseCard")://��������� �������������� �����
    //            break;

    //            //local
    //        case ("Legion"):
    //            int a = 3;
    //            ifAction.TextData = new List<string>();
    //            ifAction.IntData  = new List<int>();
    //            for (int i = 0; i < a; i++)
    //            {
    //                ifAction.TextData.Add("");
    //                ifAction.IntData.Add(0);
    //            }
    //            break;
    //    }
    //}


    void SwitchTargetTime(int a)
    {
        int i = int.Parse(stringMood);
        triggerActions[i].TargetTime = a;
        HideSelector();
        TriggerMainText(i);
        TriggerRootText(i);
        LoadAllText();
    }
    #endregion

}
public class TriggerAction
{
    public int Id;
    public int Mood;//All. Shot. Melee
    public int TargetPalyer;//All. My. Enemy
    public int TargetTime;//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(�������� � ��������)

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
public class IfAction 
{
    public int LocalId;
    public int Prioritet = 10;//��������� ��������
    public List<string> TextData;//��������� ����
    public List<int> IntData;//������������ ����
    public int[] MainCore = new int[2];
    public List<int> Core;

    public RuleFrame Form;
    public string Text;
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

