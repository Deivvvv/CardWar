using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saver;

public class RuleConstructor : MonoBehaviour
{
    private RuleConstructor ruleConstructor;
    [SerializeField]
    private RuleMainFrame frame;
   
    [SerializeField]
    private ActionLibrary library;
    [SerializeField]
    private RuleConstructorUi Ui;

    private string stringMood = "";

    private HeadRule head;

    [SerializeField]
    private Color[] colors;



    private string allText;
    private string mainText;
    //private string plusText;
    //private string minusText;

    //private string actionText;

    [SerializeField]
    private TextMeshProUGUI TT;
    #region Rule Constructor

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
        allText += $"\n\nТригеры";
        int a = head.TriggerActions.Count;
        for (int i = 0; i < a; i++)
        {
            allText += head.TriggerActions[i].RootText;
            allText += $"\n";
        }
        allText += $"<link=Add_Trigger><color=green>\nДобавить триггер</color></link>";

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
            TriggerAction triggerAction = head.TriggerActions[trigger];
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
            case ("RuleName"):
                head.Name = text;
                break;
            case ("Cost"):
                int i = int.Parse(text);
                if (i != null)
                    head.Cost = i;
                break;

            case ("CostExtend"):
                i = int.Parse(text);
                if (i != null)
                    head.CostExtend = i;
                break;

            case ("LevelCap"):
                i = int.Parse(text);
                if (i != null)
                    head.LevelCap = i;
                break;

            //case ("CostMovePoint"):
            //    i = int.Parse(text);
            //    if (i != null)
            //        head.CostMovePoint = i;
            //    break;

            default:
                string[] com = stringMood.Split('_');
                string[] com1 = text.Split('/');
                //  string text1 = com[1];
                int c = int.Parse(com[0]);
                TriggerAction triggerAction = head.TriggerActions[c];
                if (com1.Length > 1)
                {
                    int i1 = int.Parse(com[2]);
                    RuleAction action = triggerAction.Action[i1];
                    action.Modifer = Ui.TextInput.text;
                    CreateActionText(c, i1);
                    TriggerActionText(c);
                    TriggerRootText(c);
                    //LoadAllText();
                }
                else
                {
                    i = int.Parse(text);
                    if (i != null)
                    {
                        if (com.Length > 1)
                        {
                            int i1 = int.Parse(com[2]);
                            int b1 = int.Parse(com[3]);
                            int b2 = int.Parse(com[4]);

                            text = com[1];
                            //  Debug.Log(com[1] == "Action");
                            if (text == "Action")
                            {
                                RuleAction action = triggerAction.Action[i1];

                                if (com[2] == "String")
                                {
                                    action.Modifer = Ui.TextInput.text;
                                }
                                else
                                {
                                    action.IntData = i;
                                }

                                CreateActionText(c, i1);
                                TriggerActionText(c);
                                TriggerRootText(c);
                                LoadAllText();
                            }
                            else
                            {
                                IfAction ifAction = null;

                                switch (text)
                                {
                                    case ("Plus"):
                                        ifAction = triggerAction.PlusAction[i1];
                                        break;

                                    case ("Minus"):
                                        ifAction = triggerAction.MinusAction[i1];
                                        break;
                                }

                                if (b1 == -1)
                                    ifAction.Prioritet = i;
                                else
                                {
                                    IfCore ifCore = ifAction.Core[b2];

                                    ifCore.IntData[b1] = i;
                                    SwitchRuleText(ifCore, b1, "Int");

                                    //PreLableIfAction(ifAction, ifCore);
                                    LableIfAction(ifAction, text, c);
                                }

                            }
                        }
                        else
                        {
                            triggerAction.Id = i;
                            TextReStruct();
                        }
                    }
                }
                //switch (com[2])
                //{
                //    case ("Plus"):
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
        IAddLink(-1,0, color1, "Switch_RuleName", $"Правило -- { head.Name}");
        IAddLink(-1, 0, color1, "Switch_Cost", $"\nЦена: { head.Cost}");

        if (head.CostExtend == 0)
            IAddLink(-1, 0, color1, "Switch_CostExtend", $"\nЦена за доп очки: -- идентично");
        else
            IAddLink(-1, 0, color1, "Switch_CostExtend", $"\nЦена за доп очки: -- { head.CostExtend}");


        if (head.LevelCap == 0)
            IAddLink(-1, 0, color1, "Switch_LevelCap", $"\nМаксимальный уровень - без ограничений");
        else
            IAddLink(-1, 0, color1, "Switch_LevelCap", $"\nМаксимальный уровень - { head.LevelCap}");


       // IAddLink(-1, 0, color1, "Switch_CostMovePoint", $"\nОчков хода за применение (? - может сделать в блоке действий или макро действий?) {CostMovePoint}");

        if (!head.Player)
            IAddLink(-1, 0, color1, "Switch_Player", $"\nУровень доступа - Разработчик");
        else
            IAddLink(-1, 0, color1, "Switch_Player", $"\nУровень доступа - Игрок");

        mainText += "\nТребуемые механики";
        IAddLink(-1, 0, color1, "Add/NeedRule", $"\nСоздать связь");
        for(int i =0; i < head.NeedRule.Count; i++)
            IAddLink(-1, 0, color1, $"Remove/NeedRule/{head.NeedRule[i]}", $"\n    Разорвать связь с { head.NeedRule[i]}");

        mainText += "\nИсключающие механики";
        //if (!Player) head.NeedRule EnemyRule
        IAddLink(-1, 0, color1, "Add/EnemyRule", $"\nСоздать связь");
        for (int i = 0; i < head.EnemyRule.Count; i++)
            IAddLink(-1, 0, color1, $"Remove/EnemyRule/{head.EnemyRule[i]}", $"\n    Разорвать связь с { head.EnemyRule[i]}");
        //    IAddLink(-1, 0, color1, "Add/NeedRule", $"\nСоздать связь");
        //else
        //    IAddLink(-1, 0, color1, "Switch_Player", $"\nУровень доступа - Игрок");

    }
    void TextReStruct()
    {
        int a = head.TriggerActions.Count;
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

    void AddAction(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        RuleAction action = new RuleAction();
        int b = triggerAction.Action.Count;

        triggerAction.Action.Add(action);
        CreateActionText(a,b);

        TriggerActionText(a);
        TriggerRootText(a);
        LoadAllText();

    }
    void DelAction(int a, int b)
    {
        Ui.TextWindow.active = false;
        TriggerAction triggerAction = head.TriggerActions[a];

        triggerAction.Action.RemoveAt(b);
        TriggerActionText(a);
        TriggerRootText(a);
        LoadAllText();
    }


    void AddIf(int a, bool plus)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        IfAction ifAction = new IfAction();



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

    }

    void DelIf(int a, bool plus, int b)
    {
        Ui.TextWindow.active = false;
        TriggerAction triggerAction = head.TriggerActions[a];

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
        triggerAction.TargetTime = 0;//TargetTime[0];//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(свойства с кладбища)

        triggerAction.PlusAction = new List<IfAction>();
        triggerAction.MinusAction = new List<IfAction>();
        triggerAction.Action = new List<RuleAction>();

        int a = head.TriggerActions.Count;
        head.TriggerActions.Add(triggerAction);
        
        TriggerMainText(a);
        TriggerPlusText(a);
        TriggerMinusText(a);
        TriggerActionText(a);
        TriggerRootText(a);
        LoadAllText();
    }
    void DelTrigger(int a)
    {
        head.TriggerActions.RemoveAt(a);
        TextReStruct();
    }

    #region Trigger Text
    void TriggerMainText(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        triggerAction.MainText = "\n------";
        triggerAction.MainText += $"<link=Trigger_{a}_Del><color=green>-Удалить триггер</color></link>";
     //   IAddLink(a, 0, "green", $"Trigger_{a}_Id", $"\n-ID({triggerAction.Id})");
     //   IAddLink(a, 0, "green", $"Trigger_{a}_Mood", $"\n-Фаза хода: {frame.TurnString[triggerAction.Mood]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetPalyer", $"\n-Проверяемый игрок: {frame.PlayerString[triggerAction.TargetPalyer]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetTime", $"\n-Условие проверки: {frame.Trigger[triggerAction.TargetTime]}");

        //  IAddLink(a, 0, "green", $"Trigger_{a}_Only", $"\n-Одиночный режим работы {triggerAction.Only}");

        // triggerAction.MainText = "";
    }
    void TriggerPlusText(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        triggerAction.PlusText = "\n\n-Условия";

        int b = triggerAction.PlusAction.Count;
        for (int i = 0; i < b; i++)
        {
            triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusDel_{i}><color=green>-Удалить Условие</color></link>";
            triggerAction.PlusText += triggerAction.PlusAction[i].Text;
        }
        triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusAdd><color=green>-Добавить Условие</color></link>";
    }
    void TriggerMinusText(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        triggerAction.MinusText = "\n\n-Исключения";


        int b = triggerAction.MinusAction.Count;
        for (int i = 0; i < b; i++)
        {
            triggerAction.MinusText += $"\n\n<link=Trigger_{a}_PlusDel_{i}><color=green>-Удалить Исключение</color></link>";
            triggerAction.MinusText += triggerAction.MinusAction[i].Text;
        }

        triggerAction.MinusText += $"\n<link=Trigger_{a}_MinusAdd><color=green>-Добавить Исключение</color></link>";
    }
    void TriggerActionText(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        triggerAction.ActionText = "\n\n-Действия";
        string text = "";
        int b = triggerAction.Action.Count;
        int c = 0;
        RuleAction action = null;
        for (int i = 0; i < b; i++)
        {
            action = triggerAction.Action[a];
            text = "\n-Удалить Действие";
            IAddLink(a, 3, "green", $"Trigger_{a}_ActionDel_{i}", text);


            triggerAction.ActionText += action.RootText;
        }

        triggerAction.ActionText += $"\n<link=Trigger_{a}_ActionAdd><color=green>-Добавить Действие</color></link>";
    }
    void CreateActionText( int a, int i)
    {

        string linkText = "";
        string colorText = "#F4FF04";

        TriggerAction triggerAction = head.TriggerActions[a];

     //   Debug.Log($"{a}  {i}");
        string text1 = "";
        string text2 = "";
        int c = 0;
        int d = 0;
        RuleAction action = triggerAction.Action[i];


        linkText = $"Trigger_{a}_Action_{i}_Mood_Action";
        text2 = $"\n-Цель {frame.AllTriggers[action.Mood].Name}";

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
      //  Debug.Log(d);

        linkText = $"Trigger_{a}_Action_{i}_TargetPalyer_Action";
        text2 = $"\n-Целевой игрок {frame.PlayerString[action.TargetPalyer]}";

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";


       // Debug.Log(d);
        c = action.EffectData;
        linkText = $"Trigger_{a}_Effects_{i}_{d}_Action";
        text2 = $"\n-Эффект {library.Effects[c].Name}";
        d++;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";

       // Debug.Log(d);
        c = action.RelizeConstant;
        linkText = $"Trigger_{a}_Constant_{i}_{d}_Action";
        text2 = $"\n-Целевой парметр {library.Constants[c].Name}";
        d++;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";


       // Debug.Log(d);
        c = action.TargetConstant;
        linkText = $"Trigger_{a}_Constant_{i}_{d}_Action";
        if (c != -1)
        {
            text2 = $"\n-Параметр от которого зависит: {library.Constants[c].Name}";
        }
        else
            text2 = $"\n-Параметр от которого зависит: Нету";
        d++;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";


        c = action.IntData;
        linkText = $"Trigger_{a}_Int_{i}_{d}_Action";
        text2 = $"\n-Добавочное число {c}";
        d++;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";

        linkText = $"Trigger_{a}_String_{i}_{d}_Action";
        text2 = $"\n-Модификатор соотношения {action.Modifer}";
        d++;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";


        action.RootText = text1;
    }

    void TriggerRootText(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        string text = $""; 

        text += triggerAction.MainText;
        text += triggerAction.PlusText;
        text += triggerAction.MinusText;
        text += triggerAction.ActionText;
        triggerAction.RootText = text;
    }

    public IfCore NewIfCore()
    {
        IfCore ifCore = new IfCore();
        ifCore.TextData = new List<string>();
        ifCore.IntData = new List<int>();

        return ifCore;
    }

    void CoreLableIfAction(IfAction ifAction, int a, int b)
    {
        ifAction.Core = new List<IfCore>();
        ifAction.Core.Add( NewIfCore());

        ifAction.MainCore[0] = a;
        ifAction.MainCore[1] = b;


        ifAction.Form = frame.AllTriggers[a].Form[b];
    }
    void SwitchLableIfAction(IfAction ifAction, int c, int d, string ifText, int t)
    {

        int cAlt = c + 1;
        int b = ifAction.Core.Count;

        if (cAlt > b)
        {
            ifAction.Core.Add(NewIfCore());
            ifAction.Core[b].Core = d;
        }
        else 
        {
            if (cAlt < b)
                for (int i = cAlt; i < b; i++)
                {
                    ifAction.Core.RemoveAt(c);
                }


            ifAction.Core[c].Core = d;

        }

        if (d < ifAction.Form.Form.Count-1)
            if (ifAction.Form.Form[d].Id == ifAction.Form.Form[d + 1].Id)
            {
                SwitchLableIfAction(ifAction, cAlt, d + 1, ifText, t);
                return;
            }

        SwicthIfCore(ifAction, c);

        PreLableIfAction(ifAction, ifAction.Core[c]);

        LableIfAction(ifAction, ifText,t);
    }

    void SwitchRuleText(IfCore ifCore, int a, string rule)
    {
        int b = ifCore.IntData[a];
       // ifCore.IntData[a] = b;
        string str = "";
        switch (rule)
        {
            case ("Legion"):
                str = library.Legions[b].Name;
                break;
            case ("Constant"):
                str = library.Constants[b].Name;
                break;
            case ("Group"):
                str = library.CivilianGroups[b].Name;
                break;
            case ("GroupLevel"):
                Debug.Log($"!Erorr! {rule}");
                //library.CivilianGroups[b].Tituls[a].Name;
                // str = library.CivilianGroups[b].Name;// need fix it
                break;
            case ("Int"):
                str = $"{b}";
                break;
            case ("Bool"):
                str = frame.BoolString[b];
                break;
            case ("Equal"):
                str = frame.EqualString[b];
                break;
            default:
                Debug.Log($"!Erorr! {rule}");
                break;
        }
        ifCore.TextData[a] = str;
    }

    void SwicthIfCore(IfAction ifAction, int a)
    {

        IfCore ifCore = ifAction.Core[a];
        RuleFarmeMacroCase form = ifAction.Form.Form[ifCore.Core];

        ifCore = NewIfCore();
        for (int i = 0; i < form.Form.Length; i++)
        {
            ifCore.IntData.Add(0);
            ifCore.TextData.Add("");
        }
        ifAction.Core[a] = ifCore;
        PreLableIfAction(ifAction, ifCore);
    }

    public void PreLableIfAction(IfAction ifAction , IfCore ifCore)
    {
        RuleFarmeMacroCase form = ifAction.Form.Form[ifCore.Core];


        for(int i =0; i < ifCore.IntData.Count; i++)
        {
            SwitchRuleText(ifCore, i, form.Form[i].Rule);
        }
    }


    public void LableIfAction(IfAction ifAction, string ifText, int t)
    {
        string linkText = "";
        string colorText = "#F4FF04";

        int b = ifAction.Core.Count;
        int b1 = 0;
        int a = ifAction.MainCore[0];
        int a1 = ifAction.MainCore[1];
        int a2 = 0;
        RuleFarmeMacroCase form = null;
        IfCore ifCore = null;
        string text = "";
        string text1 = "";
        string text2 = "";


        Debug.Log(ifAction.LocalId);
        linkText = $"Trigger_{t}_Int_{ifAction.LocalId}_-1_{ifText}";

        text2 = $"\n\n-Приоритет({ifAction.Prioritet})  ";

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";



        linkText = $"Trigger_{t}_Core1_{ifAction.LocalId}_-1_{ifText}";

        text2 = "\nЦель проверки: " + frame.AllTriggers[a].Name;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";



        linkText = $"Trigger_{t}_Core2_{ifAction.LocalId}_-1_{ifText}";

        text2 = "\nПроверяемый параметр: " + frame.AllTriggers[a].Form[a1].Name;

        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";








        int l = ifAction.LocalId;
        for (int i = 0; i < b; i++)
        {
            a2 = 0;
            int a3 = -1;
            int a4 = -1;

            ifCore = ifAction.Core[i];
            a = ifCore.Core;

            form = ifAction.Form.Form[a];

            a1 = form.Id;
            b1 = form.Form.Length;
            text1 += "\n";
            for (int i1 = 0; i1 < a1; i1++)
            {
                text1 += "    ";
            }



            a4 = ifAction.Form.Form.Count;

            if(i!=0)
                if (a1 == ifAction.Form.Form[i - 1].Id)
                    a4 = 0;

            for (int i1 = i; i1 < a4; i1++)
            {
               // Debug.Log(i);
                //  Debug.Log(a4);
                //  Debug.Log($"{a} {ifAction.Form.Form.Count - 1} {i1} {b1}");
                if (a1 > ifAction.Form.Form[i1].Id)
                    i1 = a4;
                else
                {
                    if (a1 == ifAction.Form.Form[i1].Id)
                        if (a3 == -1)
                        {
                            //  Debug.Log($"Ok {i1}");
                            a3 = i1;
                        }
                        else if (a1 != ifAction.Form.Form[i1 - 1].Id)
                        {
                           // Debug.Log(ifAction.Form.Form[i1].Id);
                           // Debug.Log(ifAction.Form.Form[i1 - 1].Id);
                            //  Debug.Log($"i1_{i1}");
                            text2 = $"Далее - \n";
                            linkText = $"Trigger_{t}_NextData_{l}_{i}_{ifText}_{i1}";
                            text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
                            //   Debug.Log($"Ok");
                            i1 = a4;
                        }
                }
            }


            //    NextData
            //for()
            for (int i1 = 0; i1 < b1; i1++)
            {
                text1 += "---";
                text = form.Form[i1].Rule;

                linkText = $"Trigger_{t}_{text}_{l}_{a2}_{ifText}_{i}";
                //Debug.Log(linkText);
                //Debug.Log(ifCore.TextData.Count);
                //Debug.Log(i1);
                text = ifCore.TextData[i1];
                text2 = form.Form[i1].Text;
                text2 = text2.Replace("{Rule}", $" {text} ");


                text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";

               // Debug.Log(linkText);
                a2++;
            }
           // Debug.Log(linkText);

        }

        //Debug.Log(ifText);
        if (a < ifAction.Form.Form.Count-1)
            if (ifAction.Form.Form[a].Id < ifAction.Form.Form[a+1].Id)
                if(ifAction.Form.Form[a + 1].Form.Length>0)
                {
                    text2 = "\nДалее";
                    linkText = $"Trigger_{t}_NewData_{l}_{a + 1}_{ifText}";
                    text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
                }

       // Debug.Log(ifText);

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
        string[] com = cod.Split('/');
        if (com.Length > 1)
        {
            switch (com[0])
            {
                case ("Add"):
                    switch (com[1])
                    {
                        case ("NeedRule"):
                            Ui.SelectorMainAltRule.active = true;
                            stringMood = cod;
                            //head.NeedRule.Add(com[2]);
                            break;
                        case ("EnemyRule"):
                            Ui.SelectorMainAltRule.active = true;
                            stringMood = cod;
                            //head.EnemyRule.Add(com[2]);
                            break;
                    }
                    break;
                case ("Remove"):
                    switch (com[1])
                    {
                        case ("NeedRule"):
                            head.NeedRule.Remove(com[2]);
                            break;
                        case ("EnemyRule"):
                            head.EnemyRule.Remove(com[2]);
                            break;
                    }
                    LoadMainText();
                    LoadAllText();
                    break;
            }
        }
        else
        {
            com = cod.Split('_');
            string text = com[1];
            switch (com[0])
            {
                case ("Switch"):
                    switch (text)
                    {
                        case ("RuleName"):
                            LoadTextWindowData(head.Name, text);
                            break;

                        case ("Cost"):
                            LoadTextWindowData($"{head.Cost}", text);
                            break;

                        case ("CostExtend"):
                            LoadTextWindowData($"{head.CostExtend}", text);
                            break;

                        case ("LevelCap"):
                            LoadTextWindowData($"{head.LevelCap}", text);
                            break;

                        //case ("CostMovePoint"):
                        //    LoadTextWindowData($"{head.CostMovePoint}", text);
                        //    break;
                        case ("Player"):
                            head.Player = !head.Player;
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
                        TriggerAction triggerAction = head.TriggerActions[i];

                        IfAction ifAction = null;
                        IfCore ifCore = null;

                        if (com.Length > 3)
                        {
                            b = int.Parse(com[3]);
                        }


                        if (com.Length > 5)
                        {

                            //   b = int.Parse(com[3]);
                            if (com[5] != "Action")
                                b1 = int.Parse(com[4]);
                            //Debug.Log()

                            switch (com[5])
                            {
                                case ("Plus"):
                                    ifAction = head.TriggerActions[i].PlusAction[b];
                                    break;

                                case ("Minus"):
                                    ifAction = head.TriggerActions[i].MinusAction[b];
                                    break;
                            }

                            if (com.Length > 6)
                            {
                                ifCore = ifAction.Core[int.Parse(com[6])];

                                stringMood = $"{i}_{com[5]}_{com[3]}_{com[4]}_{com[6]}";
                            }
                            else
                                stringMood = $"{i}_{com[5]}_{com[3]}_{com[4]}";
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
                                int a = ifCore.IntData[b1];

                                a++;
                                if (a >= frame.BoolString.Length)
                                    a = 0;

                                ifCore.IntData[b1] = a;
                                PreLableIfAction(ifAction, ifCore);

                                LableIfAction(ifAction, com[5], i);

                                break;
                            case ("Equal"):
                                a = ifCore.IntData[b1];

                                a++;
                                if (a >= frame.EqualString.Length)
                                    a = 0;

                                ifCore.IntData[b1] = a;
                                PreLableIfAction(ifAction, ifCore);

                                LableIfAction(ifAction, com[5], i);
                                break;

                            case ("Core1"):
                                a = ifAction.MainCore[0];

                                a++;
                                if (a >= frame.AllTriggers.Count)
                                    a = 0;

                                CoreLableIfAction(ifAction, a, 0);
                                SwitchLableIfAction(ifAction, 0, 0, com[5], i);

                                break;

                            case ("Core2"):
                                b = ifAction.MainCore[0];
                                a = ifAction.MainCore[1];

                                a++;
                                if (a >= frame.AllTriggers[b].Form.Count)
                                    a = 0;

                                CoreLableIfAction(ifAction, b, a);
                                SwitchLableIfAction(ifAction, 0, 0, com[5], i);

                                break;

                            case ("NextData"):

                                a = ifAction.Core[b1].Core;
                                int a1 = ifAction.Form.Form[a].Id;
                                bool load = false;
                                int a4 = ifAction.Form.Form.Count;

                                for (int i1 = a + 1; i1 < a4; i1++)
                                {
                                    if (a1 > ifAction.Form.Form[i1].Id)
                                        i1 = a4;

                                    if (a1 != ifAction.Form.Form[i1 - 1].Id)
                                        if (a1 == ifAction.Form.Form[i1].Id)
                                        {
                                            a = i1;
                                            load = true;
                                            i1 = a4;
                                        }
                                }

                                if (!load)
                                {

                                    int a3 = -2;
                                    for (int i1 = a; i1 > 0; i1--)
                                    {
                                        if (a1 > ifAction.Form.Form[i1].Id)
                                        {
                                            a3 = i1;
                                            i1 = 0;
                                        }

                                        if (a3 == -2)
                                            if (i1 == 1)
                                                a3 = 0;
                                    }

                                    for (int i1 = 0; i1 < a; i1++)
                                    {
                                        if (a1 > ifAction.Form.Form[i1].Id)
                                            i1 = a4;

                                        if (a1 == ifAction.Form.Form[i1].Id)
                                        {
                                            a = i1;
                                            load = true;
                                            i1 = a4;
                                        }
                                    }
                                }

                                //Debug.Log($"{ifAction}, {b1}, {a}, {com[5]}, {i}");
                                SwitchLableIfAction(ifAction, b1, a, com[5], i);

                                break;

                            case ("NewData"):
                                a = ifAction.Core.Count;
                                a = 1 + ifAction.Core[a - 1].Core;
                                SwitchLableIfAction(ifAction, b1, a, com[5], i);

                                break;


                            case ("Legion"):
                                //stringMood = $"{i}_{com[5]}_{com[3]}_{com[4]}";
                                Ui.SelectorMainLegion.active = true;
                                break;

                            case ("Int"):


                                //stringMood = $"{i}_{com[5]}_{b}_{b1}";

                                Ui.TextWindow.active = true;

                                switch (com[5])
                                {
                                    case ("Plus"):
                                        if (b1 == -1)
                                            Ui.TextInput.text = "" + ifAction.Prioritet;
                                        else
                                            Ui.TextInput.text = "" + ifCore.IntData[b1];
                                        break;
                                    case ("Minus"):
                                        if (b1 == -1)
                                            Ui.TextInput.text = "" + ifAction.Prioritet;
                                        else
                                            Ui.TextInput.text = "" + ifCore.IntData[b1];
                                        break;
                                    case ("Action"):
                                        Ui.TextInput.text = "" + triggerAction.Action[b].IntData;
                                        break;
                                }
                                break;
                            case ("String"):
                                Ui.TextWindow.active = true;
                                switch (com[5])
                                {
                                    case ("Action"):
                                        Ui.TextInput.text = "" + triggerAction.Action[b].Modifer;
                                        break;
                                }
                                break;

                            case ("Constant"):
                                Ui.SelectorMainConstants.active = true;
                                break;
                            case ("Group"):
                                Ui.SelectorMainCivilianGroups.active = true;
                                break;

                            case ("Effects"):
                                Ui.SelectorMainEffects.active = true;
                                break;


                            case ("GroupLevel"):

                                a = ifCore.IntData[b1];
                                b = ifCore.IntData[b1 - 2];
                                text = "";

                                a++;
                                if (a >= library.CivilianGroups[b].Tituls.Count)
                                    a = 0;

                                text = library.CivilianGroups[b].Tituls[a].Name;///frame.EqualString[a];
                                ifCore.IntData[b1] = a;
                                ifCore.TextData[b1] = text;
                                LableIfAction(ifAction, com[5], i);
                                break;




                            case ("ActionDel"):
                                DelAction(i, b);
                                break;
                            case ("ActionAdd"):
                                AddAction(i);
                                break;


                            case ("Action"):
                                int b2 = b;
                                RuleAction action = head.TriggerActions[i].Action[b2];
                                switch (com[4])
                                {
                                    case ("Mood"):
                                        b = action.Mood;
                                        b++;
                                        if (b == frame.AllTriggers.Count)
                                            b = 0;
                                        action.Mood = b;
                                        break;
                                    case ("TargetPalyer"):
                                        b = action.TargetPalyer;
                                        b++;
                                        if (b == frame.PlayerString.Length)
                                            b = 0;
                                        action.TargetPalyer = b;
                                        break;

                                }

                                CreateActionText(i, b2);

                                TriggerActionText(i);
                                TriggerRootText(i);
                                LoadAllText();
                                //linkText = $"Trigger_{a}_Action_{i}_Mood_Action";
                                //text2 = $"\n-Цель {action.Mood}";

                                //text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";


                                //linkText = $"Trigger_{a}_Action_{i}_TargetPalyer_Action";
                                //AddAction(i);
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
                    break;
            }
        }
    }


    void HideSelector()
    {
        Ui.SelectorMainAltRule.active = false;
        Ui.SelectorMainLegion.active = false;
        Ui.SelectorMainCivilianGroups.active = false;
        Ui.SelectorMainConstants.active = false;
        Ui.SelectorMainEffects.active = false;
    }

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
        ruleConstructor = GetComponent<RuleConstructor>();
        Application.targetFrameRate = 30;
        head = new HeadRule();

        CoreLoad();
        CoreLoadRule();

        LoadBase();
      //  SaveCore();
        //CoreLoad();
        //CoreLoadRule();


        LoadMainText();
        LoadAllText();

      //  SaveCore();
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
            case ("Rule"):
                a = RuleCount;
                break;
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
                case ("Rule"):
                    GO.transform.SetParent(Ui.SelectorAltRule);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = RuleName[i];
                    break;
            }
            ButtonSelector(text, i, GO.GetComponent<Button>());
        }
    }
    void LoadBase()
    {
        Ui.TextWindowButton.onClick.AddListener(() => LoadData());
        Ui.SaveButton.onClick.AddListener(() => CreateRule());
        Ui.NewRuleButton.onClick.AddListener(() => NewRule());

        GameObject GO = null;
        string[] text = new string[] { "Legion", "CivilianGroups", "Constants", "Effects","Rule"};
        int a = text.Length;
        for (int i = 0; i < a; i++)
        {
            GO = Instantiate(Ui.SelectorMain);
            GO.transform.SetParent(Ui.Canvas);
            GO.transform.position = Ui.SelectorMain.transform.position;

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
                case ("Rule"):
                    Ui.SelectorMainAltRule = GO;
                    Ui.SelectorAltRule = GO.transform.GetChild(0).GetChild(0);
                    break;
            }

            CreateListButton(text[i]);
        }
    }
    void ButtonSelector(string text, int a, Button button)
    {
        button.onClick.AddListener(() => SwitchLibrary(a, text));
    }
    void SwitchLibrary(int a, string text1)
    {
        // string text = "";
        //switch (text1)
        //{
        //    case ("Legion"):
        //        text = library.Legions[a].Name;
        //        break;

        //    case ("CivilianGroups"):
        //        text = library.CivilianGroups[a].Name;
        //        break;

        //    case ("Constants"):
        //        text = library.Constants[a].Name;
        //        break;

        //    case ("Effects"):
        //        text = library.Effects[a].Name;
        //        break;
        //}
        string[] com = stringMood.Split('/');
        Debug.Log(stringMood);
        if (com.Length > 1)
        {
            switch (com[1])
            {
                case ("NeedRule"):
                    head.NeedRule.Add(RuleName[a]);
                    break;
                case ("EnemyRule"):
                    head.EnemyRule.Add(RuleName[a]);
                    break;
            }
            LoadMainText();

        }
        else
        {
            com = stringMood.Split('_');

            text1 = com[1];

            int i1 = int.Parse(com[0]);
            int i2 = int.Parse(com[2]);
            int i = int.Parse(com[3]);
            TriggerAction triggerAction = head.TriggerActions[i1];
            if (text1 != "Action")
            {
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
                i2 = int.Parse(com[4]);
                IfCore ifCore = ifAction.Core[i2];
                //ifAction.CoreTextData[i] = text;
                ifCore.IntData[i] = a;
                PreLableIfAction(ifAction, ifCore);
                LableIfAction(ifAction, text1, i1);
            }
            else
            {
                RuleAction action = triggerAction.Action[i2];
                switch (i)
                {
                    case (0):
                        action.EffectData = a;
                        break;
                    case (1):
                        action.RelizeConstant = a;
                        break;
                    case (2):
                        action.TargetConstant = a;
                        break;
                        //case (3):

                        //    action.IntData = a;

                        //    break;
                }
                //ifAction.TextData[i] = text;
                //ifAction.IntData[i] = a;

                CreateActionText(i1, i2);
                TriggerActionText(i1);
                TriggerRootText(i1);
            }

        }
        LoadAllText();
        HideSelector();
    }





    #endregion

    #region Library Rule

    private int curentRule =-1;
    public int RuleCount;
    public List<string> RuleName;

    void NewRule()
    {
        curentRule = -1;
    }
    void LoadRule(int i)
    {
        curentRule = i;
        XMLSaver.LoadRule(ruleConstructor, i);
    }

    public void SetRule(HeadRule headRule)
    {
        head = headRule;

        
        LoadMainText();

        TriggerAction triggerAction1 = null;
        IfAction ifAction = null;
      //  RuleAction ruleAction = null;

        int b = head.TriggerActions.Count;
        int b1 = 0;
        for (int i = 0; i < b; i++)
        {
            triggerAction1 = head.TriggerActions[i];
            TriggerMainText(i);

            b1 = triggerAction1.PlusAction.Count;
            for (int i1 = 0; i1 < b1; i1++)
            {
                ifAction = triggerAction1.PlusAction[i1];
                ifAction.LocalId = i1;

                ifAction.Form = frame.AllTriggers[ifAction.MainCore[0]].Form[ifAction.MainCore[1]];
                int b2 = ifAction.Core.Count;
                for (int i2 = 0; i2 < b2; i2++)
                {
                    PreLableIfAction(ifAction, ifAction.Core[i2]);
                }

                LableIfAction(ifAction, "Plus", i);
            }
            TriggerPlusText(i);


            b1 = triggerAction1.MinusAction.Count;
            for (int i1 = 0; i1 < b1; i1++)
            {
                ifAction = triggerAction1.MinusAction[i1];
                ifAction.LocalId = i1;

                ifAction.Form = frame.AllTriggers[ifAction.MainCore[0]].Form[ifAction.MainCore[1]];

                for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
                {
                    PreLableIfAction(ifAction, ifAction.Core[i2]);
                }
                LableIfAction(ifAction, "Minus", i);
            }
            TriggerMinusText(i);

            b1 = triggerAction1.Action.Count;
            for (int i1 = 0; i1 < b1; i1++)
            {
                // ruleAction = triggerAction1.Action[i1];
                CreateActionText(i, i1);
            }
            TriggerActionText(i);

            TriggerRootText(i);
        }


        LoadAllText();
    }

    void SaveRule(int i)
    {
        XMLSaver.SaveRule(head, i);
    }

    void SaveCore()
    {
        XMLSaver.SaveMainRule(library);
    }

    void CoreLoad()
    {
        XMLSaver.LoadMainRule(library);
        ruleConstructor.RuleCount = library.RuleName.Count;
        RuleName = library.RuleName;
    }

    void CreateRule()
    {
        if(curentRule != -1)
        {
            library.RuleName[curentRule] = head.Name;
            Ui.SelectorLibrary.GetChild(curentRule+1).GetChild(0).gameObject.GetComponent<Text>().text = head.Name;
        }
        else
        {
            curentRule = RuleCount;
            library.RuleName.Add(head.Name);
            // RuleCount++;
            AddRuleButton(RuleCount, head.Name);
            RuleCount++;
        }
        SaveRule(curentRule);
        SaveCore();
    }

    public void CoreLoadRule()
    {
        GameObject GO = null;

        for(int i =0; i< RuleCount; i++)
        {
            AddRuleButton(i, RuleName[i]);
        }
    }
    void AddRuleButton(int i, string text)
    {
        GameObject GO = Instantiate(Ui.ButtonOrig);
        GO.transform.SetParent(Ui.SelectorLibrary);
        GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = text;
        GO.GetComponent<Button>().onClick.AddListener(() => LoadRule(i));
    }

    //void LoadRuleButton(Button button, int i)
    //{
    //    button.onClick.AddListener(() => LoadRule( i));
    //}
    // private List<string> libraryRule;
    #endregion
}
[System.Serializable]
public class TriggerAction
{
    public int Id;
    public int Mood;
    public int TargetPalyer;
    public int TargetTime;

    public string RootText;
    public string MainText;
    public string PlusText;
    public string MinusText;
    public string ActionText;
    
    
    public List<IfAction> PlusAction;
    public List<IfAction> MinusAction;

    public List<RuleAction> Action;
}

[System.Serializable]
public class IfAction 
{
    public int LocalId;
    public int Prioritet = 10;
    public int[] MainCore = new int[2];
    public List<IfCore> Core;

    public RuleFrame Form;
    public string Text;
}

public class IfCore
{
    public int Core;
    public List<string> TextData;
    public List<int> IntData;
}



public class RuleAction
{//
    public string RootText;

    public int Mood;
    public int TargetPalyer;


    //3:дать эффект (Эффект)// параметр для наследования свойств эффекта // Выбрать параметр для наследования свойств или оставить пустым для использования числа// указать добавочное число

    public int EffectData;//Effect  дать эффект (Эффект)
    public int RelizeConstant;//Constant параметр для наследования свойств эффекта 
    public int TargetConstant =-1;//Constant Выбрать параметр для наследования свойств или оставить пустым для использования числа

    public int IntData;
    public string Modifer = "1/1";


    //public List<int> EffectData;//Effect  дать эффект (Эффект)
    //public List<int> RelizeConstant;//Constant параметр для наследования свойств эффекта 
    //public List<int> TargetConstant;//Constant Выбрать параметр для наследования свойств или оставить пустым для использования числа

    //public List<int> IntData;
}

[System.Serializable]
public class HeadRule
{
    public string Name = "Благочестие";//Название
    public string Info;//Описание

    public int Cost;//Цена
    public int CostExtend;//цена за доп очки навыков

    public int LevelCap;//Максимальный уровень способности

    //public int CostMovePoint;

    public bool Player;


    public List<TriggerAction> TriggerActions = new List<TriggerAction>();

    public List<string> NeedRule = new List<string>();
    public List<string> EnemyRule = new List<string>();
}
