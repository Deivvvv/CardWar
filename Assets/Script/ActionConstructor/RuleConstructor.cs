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

    [SerializeField]
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
        allText += $"\n\nТригеры";
        int a = head.TriggerActions.Count;
        for (int i = 0; i < a; i++)
        {
            allText += head.TriggerActions[i].RootText + "\n";
        }
        allText += LinkSupport("green", "Add_Trigger", "\nДобавить триггер");

        TT.text = allText;
    }




    void LoadMainText()
    {
        string color = "#F4FF04";
        mainText = "";

        mainText += LinkSupport(color, "Text_RuleName", $"Правило -- { head.Name}");
        mainText += LinkSupport(color, "Text_Cost", $"\nЦена: { head.Cost}");

        if (head.CostExtend == 0)
            mainText += LinkSupport(color, "Text_CostExtend", $"\nЦена за доп очки: -- идентично");
        else
            mainText += LinkSupport(color, "Text_CostExtend", $"\nЦена за доп очки: -- { head.CostExtend}");


        if (head.LevelCap == 0)
            mainText += LinkSupport(color, "Text_LevelCap", $"\nМаксимальный уровень - без ограничений");
        else
            mainText += LinkSupport(color, "Text_LevelCap", $"\nМаксимальный уровень - { head.LevelCap}");

        if (!head.Player)
            mainText += LinkSupport(color, "Text_Player", $"\nУровень доступа - Разработчик");
        else
            mainText += LinkSupport(color, "Text_Player", $"\nУровень доступа - Игрок");

        mainText += "\nТребуемые механики";
        mainText += LinkSupport(color, "MainAdd_NeedRule", $"\nСоздать связь");
        for(int i =0; i < head.NeedRule.Count; i++)
            mainText += LinkSupport(color, $"MainRemove_NeedRule_{head.NeedRule[i]}", $"\n    Разорвать связь с { head.NeedRule[i]}");

        mainText += "\nИсключающие механики";
        mainText += LinkSupport(color, "MainAdd_EnemyRule", $"\nСоздать связь");
        for (int i = 0; i < head.EnemyRule.Count; i++)
            mainText += LinkSupport(color, $"MainRemove_EnemyRule_{head.EnemyRule[i]}", $"\n    Разорвать связь с { head.EnemyRule[i]}");

    }

    RuleForm AddRuleForm()
    {
        RuleForm ruleForm = new RuleForm();
        ruleForm.StatTayp = "Null";
        ruleForm.Stat = "Null";


        return ruleForm;
    }

    void AddAction(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        RuleAction action = new RuleAction();
        int b = triggerAction.Action.Count;
        action.Core.Add(AddRuleForm());
        action.Core.Add(AddRuleForm());
        action.Core.Add(AddRuleForm());

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

    IfAction CreateIfAction()
    {
        IfAction ifAction = new IfAction();
        ifAction.Prioritet = 10;
        ifAction.Point = 10;
        //ifAction.Result;
    //ifAction.Core = new List<RuleForm>();
        ifAction.Core.Add(AddRuleForm());
        ifAction.Core.Add(AddRuleForm());


        return ifAction;
    }

    void AddIf(int a, bool plus)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        IfAction ifAction = CreateIfAction();

        if (plus)
            triggerAction.PlusAction.Add(ifAction);
        else
            triggerAction.MinusAction.Add(ifAction);
        CreateIfText(a,(plus)? triggerAction.PlusAction.Count-1:triggerAction.MinusAction.Count - 1, plus);
        TriggerIfText(a, plus);
        TriggerRootText(a);
        LoadAllText();

    }
    void DelIf(int a, bool plus, int b)
    {
        Ui.TextWindow.active = false;
        TriggerAction triggerAction = head.TriggerActions[a];


        if (plus)
            triggerAction.PlusAction.RemoveAt(b);
        else
            triggerAction.MinusAction.RemoveAt(b);

        TriggerIfText(a, plus);
        TriggerRootText(a);
        LoadAllText();
    }

    void AddTrigger()
    {
        TriggerAction triggerAction = new TriggerAction();
        //triggerAction.Id =-1;
       // triggerAction.Mood = 0;// Mood[0];//All. Shot. Melee
        triggerAction.TargetPalyer = 0;//TargetPalyer[0];//All. My. Enemy
        triggerAction.Trigger = 0;//TargetTime[0];//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(свойства с кладбища)

        triggerAction.PlusAction = new List<IfAction>();
        triggerAction.MinusAction = new List<IfAction>();
        triggerAction.Action = new List<RuleAction>();

        int a = head.TriggerActions.Count;
        head.TriggerActions.Add(triggerAction);
        
        TriggerMainText(a); 
        TriggerIfText(a, true);
        TriggerIfText(a, false);
        TriggerActionText(a);
        TriggerRootText(a);
        LoadAllText();
    }
    void DelTrigger(int a)
    {
        head.TriggerActions.RemoveAt(a);
        LoadAllText();
       // TextReStruct();
    }

    #region Trigger Text
    private void TriggerMainText(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        string color = "green";
        string text = "\n------";
        text += LinkSupport(color, $"Trigger_{a}_Del", " - Удалить триггер");

        text += LinkSupport(color, $"Trigger_{a}_Switch_CountMod", 
            $"\n-Режим счетчика с суммой: {SwitchRuleText((triggerAction.CountMod ? 1 : 0), "Bool")}");

        text += LinkSupport(color, $"Trigger_{a}_Switch_CountModExtend", 
            $"\n-Режим счетчика с разницей: { SwitchRuleText((triggerAction.CountModExtend ? 1 : 0), "Bool")}");

        text += LinkSupport(color, $"Trigger_{a}_Switch_TargetPalyer", $"\n-Проверяемый игрок: {frame.PlayerString[triggerAction.TargetPalyer]}");
        text += LinkSupport(color, $"Trigger_{a}_Switch_Trigger", $"\n-Проверяемая фаза: {frame.Trigger[triggerAction.Trigger]}");
        triggerAction.MainText = text;
    }
    private void TriggerIfText(int a,bool plus)
    {
        string addText, addLink, text = "";
        TriggerAction triggerAction = head.TriggerActions[a];
        int b = 0;
        if (plus)
        {
            text = $"\n\n-Условия";

            b = triggerAction.PlusAction.Count;
            for (int i = 0; i < b; i++)
            {
                text += LinkSupport("green", $"Trigger_{a}_PlusDel_{i}", $"\n-Удалить Условие");
                text += triggerAction.PlusAction[i].Text;
            }
            text += $"\n\n<link=Trigger_{a}_PlusAdd><color=green>-Добавить Условие</color></link>";
            triggerAction.PlusText = text;
        }
        else
        {
            text = $"\n\n-Исключения";

            b = triggerAction.MinusAction.Count;
            for (int i = 0; i < b; i++)
            {
                text += LinkSupport("green", $"Trigger_{a}_MinusDel_{i}", $"\n-Удалить Исключение");
                text += triggerAction.MinusAction[i].Text;
            }
            text += $"\n\n<link=Trigger_{a}_MinusAdd><color=green>-Добавить Исключение</color></link>";
            triggerAction.MinusText = text;
        }
        //Debug.Log(text);
    }
    private void TriggerActionText(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        string text = "\n\n-Действия";
        RuleAction action = null;
        for (int i = 0; i < triggerAction.Action.Count; i++)
        {
            action = triggerAction.Action[i];
            text += LinkSupport("green", $"Trigger_{a}_ActionDel_{i}", "\n-Удалить Действие");
            text += action.RootText;
        }
        text += LinkSupport("green", $"Trigger_{a}_ActionAdd", "\n-Добавить Действие");
        triggerAction.ActionText = text;
    }
    private string LinkSupport(string colorText, string linkText, string mainText)
    {
        return  $"<link={linkText}><color={colorText}>{mainText}</color></link>";
    }
    
    string RuleFarmeSupport(RuleForm ruleForm, string headText, string headTextExtend)
    {
        string colorText = "#F4FF04";
        string text = "";

        string linkText = headText + $"Switch{headTextExtend}NextCard";
        string textData = $"\n-{ frame.CardString[ruleForm.Card]}";

        text += LinkSupport(colorText, linkText, textData);


        linkText = headText + $"Selector{headTextExtend}StatTayp";
        textData = $" - {ruleForm.StatTayp}";

        text += LinkSupport(colorText, linkText, textData);


        linkText = headText + $"Selector{headTextExtend}SetStat|{ruleForm.StatTayp}";
        textData = $" - {ruleForm.Stat}";

        text += LinkSupport(colorText, linkText, textData);


        linkText = headText + $"Text{headTextExtend}Mod";
        textData = $" - {ruleForm.Mod}";

        text += LinkSupport(colorText, linkText, textData);


        linkText = headText + $"Text{headTextExtend}Num";
        textData = $" - {ruleForm.Num}";

        text += LinkSupport(colorText, linkText, textData);
        return text;
    }

    void CreateIfText( int a, int b, bool plus) 
    {
        Debug.Log(a);
        string linkText = "";
        string colorText = "#F4FF04";

        TriggerAction triggerAction = head.TriggerActions[a];

        //   Debug.Log($"{a}  {i}");
        string headText = $"Trigger_{a}_";
        string headTextExtend = (plus)? $"_Plus_{b}_" : $"_Minus_{b}_";
        string text1 = "";
        string text2 = "";
        IfAction action = (plus) ? triggerAction.PlusAction[b] : triggerAction.MinusAction[b];


        linkText = headText + $"Text{headTextExtend}Prioritet";
        text2 = $"\n-Приоритет { action.Prioritet}";

        text1 += LinkSupport(colorText, linkText, text2);


        linkText = headText + $"Text{headTextExtend}Point";
        text2 = $"\n-Очки { action.Point}";

        text1 += LinkSupport(colorText, linkText, text2);


        linkText = headText + $"Switch{headTextExtend}Result";
        text2 = $"\n-Ожидаемый результат {SwitchRuleText(action.Result, "Equal")}";

        text1 += LinkSupport(colorText, linkText, text2);
        
        // RuleForm ruleForm = null;
        for (int i = 0; i < action.Core.Count; i++)
        {
            text1 += RuleFarmeSupport(action.Core[i], headText, headTextExtend + $"{i}_");
        }
        action.Text = text1;
    }

    void CreateActionText( int a, int b)
    {
        string linkText = "";
        string colorText = "#F4FF04";

        TriggerAction triggerAction = head.TriggerActions[a];

        //   Debug.Log($"{a}  {i}");
        string headText = $"Trigger_{a}_";
        string headTextExtend = $"_Action_{b}_";
        string text1 = "";
        string text2 = "";
        RuleAction action = triggerAction.Action[b];


        //public int MinPoint;
        //public int MaxPoint;
        linkText = headText + $"Text{headTextExtend}MinPoint";
        text2 = $"\n - Стоймость мин { action.MinPoint}";
        text1 += LinkSupport(colorText, linkText, text2);
        
        linkText = headText + $"Text{headTextExtend}MaxPoint";
        text2 = $"\n - Стоймость мах {action.MaxPoint}";
        text1 += LinkSupport(colorText, linkText, text2);

        linkText = headText + $"Selector{headTextExtend}ActionMood";
        text2 = $"\n-Область применения { action.ActionMood}";

        text1 += LinkSupport(colorText, linkText, text2);

        linkText = headText + $"Selector{headTextExtend}Action";
        text2 = $"\n-Цель { action.Action}";

        text1 += LinkSupport(colorText, linkText, text2);


       // RuleForm ruleForm = null;
        for(int i=0; i < action.Core.Count; i++)
        {
            text1 += RuleFarmeSupport(action.Core[i], headText, headTextExtend+$"{i}_");
        }
        linkText = headText +"Text"+ headTextExtend + "Num";
        text2 = $"\n - {action.Num}";

        text1 += LinkSupport(colorText, linkText, text2);


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

    public string SwitchRuleText(int a, string rule)
    {
        string str = "";
        switch (rule)
        {
            case ("Legion"):
                str = library.Legions[a].Name;
                break;
            case ("Stat"):
                str = library.Constants[a].Name;
                break;
            case ("Group"):
                str = library.CivilianGroups[a].Name;
                break;
            case ("GroupLevel"):
                //int b = library.CivilianGroups[b].Tituls.FindIndex(x => x.Name == com[3]);
                //library.CivilianGroups[b].Tituls[a].Name;
                // str = library.CivilianGroups[b].Name;// need fix it
                break;
            case ("Int"):
                str = $"{a}";
                break;
            case ("Bool"):
                if (a > 0)
                    str = "True";
                else
                    str = "False";
                break;
            case ("Equal"):
                str = frame.EqualString[a];
                break;
            default:
                Debug.Log($"!Erorr! {rule}");
                break;
        }

        return str;
        //ifCore.TextData[a] = str;
    }


    //public void LableIfAction(IfAction ifAction, string ifText, int t)
    //{
    //    string color = "#F4FF04";
    //    string text = "";

    /*
        public int Card;//0-null 1-card1 2-card2
    public string StatTayp;
    public string Stat;
    public int Mod;
    public int Num;
}
public class RuleAction
{
    public string ActionMood;
    public string Action;
    public List<RuleForm> Rules;
    public int Num;

    public int Target;

    public string RootText;
     
     */

    //    string linkText = "";
    //    string colorText = "#F4FF04";

    //    int b = ifAction.Core.Count;
    //    int b1 = 0;
    //    int a = ifAction.MainCore[0];
    //    int a1 = ifAction.MainCore[1];
    //    int a2 = 0;
    //    RuleFarmeMacroCase form = null;
    //    IfCore ifCore = null;
    //    string text = "";
    //    string text1 = "";
    //    string text2 = "";


    //    Debug.Log(ifAction.LocalId);
    //    linkText = $"Trigger_{t}_Int_{ifAction.LocalId}_-1_{ifText}";

    //    text2 = $"\n\n-Приоритет({ifAction.Prioritet})  ";

    //    text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";



    //    linkText = $"Trigger_{t}_Core1_{ifAction.LocalId}_-1_{ifText}";

    //    text2 = "\nЦель проверки: " + frame.AllTriggers[a].Name;

    //    text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";



    //    linkText = $"Trigger_{t}_Core2_{ifAction.LocalId}_-1_{ifText}";

    //    text2 = "\nПроверяемый параметр: " + frame.AllTriggers[a].Form[a1].Name;

    //    text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";








    //    int l = ifAction.LocalId;
    //    for (int i = 0; i < b; i++)
    //    {
    //        a2 = 0;
    //        int a3 = -1;
    //        int a4 = -1;

    //        ifCore = ifAction.Core[i];
    //        a = ifCore.Core;

    //        form = ifAction.Form.Form[a];

    //        a1 = form.Id;
    //        b1 = form.Form.Length;
    //        text1 += "\n";
    //        for (int i1 = 0; i1 < a1; i1++)
    //        {
    //            text1 += "    ";
    //        }



    //        a4 = ifAction.Form.Form.Count;

    //        if(i!=0)
    //            if (a1 == ifAction.Form.Form[i - 1].Id)
    //                a4 = 0;

    //        for (int i1 = i; i1 < a4; i1++)
    //        {
    //           // Debug.Log(i);
    //            //  Debug.Log(a4);
    //            //  Debug.Log($"{a} {ifAction.Form.Form.Count - 1} {i1} {b1}");
    //            if (a1 > ifAction.Form.Form[i1].Id)
    //                i1 = a4;
    //            else
    //            {
    //                if (a1 == ifAction.Form.Form[i1].Id)
    //                    if (a3 == -1)
    //                    {
    //                        //  Debug.Log($"Ok {i1}");
    //                        a3 = i1;
    //                    }
    //                    else if (a1 != ifAction.Form.Form[i1 - 1].Id)
    //                    {
    //                       // Debug.Log(ifAction.Form.Form[i1].Id);
    //                       // Debug.Log(ifAction.Form.Form[i1 - 1].Id);
    //                        //  Debug.Log($"i1_{i1}");
    //                        text2 = $"Далее - \n";
    //                        linkText = $"Trigger_{t}_NextData_{l}_{i}_{ifText}_{i1}";
    //                        text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
    //                        //   Debug.Log($"Ok");
    //                        i1 = a4;
    //                    }
    //            }
    //        }


    //        //    NextData
    //        //for()
    //        for (int i1 = 0; i1 < b1; i1++)
    //        {
    //            text1 += "---";
    //            text = form.Form[i1].Rule;

    //            linkText = $"Trigger_{t}_{text}_{l}_{a2}_{ifText}_{i}";
    //            //Debug.Log(linkText);
    //            //Debug.Log(ifCore.TextData.Count);
    //            //Debug.Log(i1);
    //            text = ifCore.TextData[i1];
    //            text2 = form.Form[i1].Text;
    //            text2 = text2.Replace("{Rule}", $" {text} ");


    //            text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";

    //           // Debug.Log(linkText);
    //            a2++;
    //        }
    //       // Debug.Log(linkText);

    //    }

    //    //Debug.Log(ifText);
    //    if (a < ifAction.Form.Form.Count-1)
    //        if (ifAction.Form.Form[a].Id < ifAction.Form.Form[a+1].Id)
    //            if(ifAction.Form.Form[a + 1].Form.Length>0)
    //            {
    //                text2 = "\nДалее";
    //                linkText = $"Trigger_{t}_NewData_{l}_{a + 1}_{ifText}";
    //                text1 += $"<link={linkText}><color={colorText}>{text2}</color></link>";
    //            }

    //   // Debug.Log(ifText);

    //    ifAction.Text = text1;


    //    switch (ifText)
    //    {
    //        case ("Plus"):
    //            TriggerPlusText(t);
    //            break;
    //        case ("Minus"):
    //            TriggerMinusText(t);
    //            break;
    //        default:
    //            Debug.Log(ifText);
    //            break;
    //    }
    //    TriggerRootText(t);
    //    LoadAllText();

    //}


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
            case ("StatTayp"):
                Ui.SelectorMainStatTayp.active = true;
                break;
            case ("Legion"):
                Ui.SelectorMainLegion.active = true;
                break;

            case ("CivilianGroups"):
                Ui.SelectorMainCivilianGroups.active = true;
                break;

            case ("Stat"):
                Ui.SelectorMainConstants.active = true;
                break;

            case ("Effects"):
                Ui.SelectorMainEffects.active = true;
                break;

            //case ("Select"):
            // //   LoadSelector();
            //  //  Ui.SelectorsMain[5].active = true;
            //    break;
            case ("NeedRule"):
                Ui.SelectorMainAltRule.active = true;
                break;
            case ("EnemyRule"):
                Ui.SelectorMainAltRule.active = true;
                break;
            default:
                Debug.Log(text);
                break;
        }
    }
    void HideSelector()
    { 
        Ui.SelectorMainStatTayp.active = false;
        Ui.SelectorMainAltRule.active = false;
        Ui.SelectorMainLegion.active = false;
        Ui.SelectorMainCivilianGroups.active = false;
        Ui.SelectorMainConstants.active = false;
        Ui.SelectorMainEffects.active = false;
    }

    //RuleForm
    void SetIntRuleForm(RuleForm ruleForm, string text, int a)
    {
        /*
         public int Card;//0-null 1-card1 2-card2
    public string StatTayp;
    public string Stat;
    public int Mod;
    public int Num;
         
         */
        switch (text)
        {
            case ("Legion"):
                ruleForm.Stat = SwitchRuleText(a, ruleForm.StatTayp);
                break;
            case ("StatTayp"):
                ruleForm.StatTayp = frame.StatTayp[a];
                break;
            case ("Stat"):
                Debug.Log(ruleForm.StatTayp);
                ruleForm.Stat = SwitchRuleText(a, ruleForm.StatTayp);
                break;
            case ("Mod"):
                if(a!=0)
                    ruleForm.Mod = a;
                break;
            case ("Num"):
                ruleForm.Num = a;
                break;
            case ("NextCard"):
                ruleForm.Card = NextSwitch(ruleForm.Card, "Card");
                break;
            case ("Card"):
                ruleForm.Card = a;
                break;
            default:
                Debug.Log(text);
                break;
        }
    }
    void SetIntIfAction(IfAction action, string text, int a)
    {/*
       public int Prioritet;
    public int Point;
      
      */
        switch (text)
        {
            //action.Num = a;
            case ("Prioritet"):
                action.Prioritet = a;
                break;
            case ("Point"):
                action.Point = a;
                break;
            case ("Result"):
                action.Result = NextSwitch(action.Result,"Equal");
                break;
        }
    }

    void SetIntRuleAction(RuleAction action, string text, int a)
    {
        switch (text)
        {
            case ("ActionMood"):
                action.ActionMood =""+ a;
                break;
            case ("Action"):
                action.Action = "" + a;
                break;
            case ("Num"):
                action.Num = a;
                break;
            case ("Target"):
                action.Target = a;
                break;
            case ("MaxPoint"):
                action.MaxPoint = a;
                if (action.MinPoint > action.MaxPoint)
                     action.MinPoint = action.MaxPoint;
                break;
            case ("MinPoint"):
                action.MinPoint = a;
                if (action.MinPoint > action.MaxPoint)
                    action.MaxPoint = action.MinPoint;
                break;
            default:
                Debug.Log(text);
                break;
        }
    }

    int NextSwitch(int a, string mood)
    {
        a++;
        switch (mood) 
        {
            case ("Card"):
                if (a >= frame.CardString.Length)
                    a = 0;
                break;
            case ("TargetPalyer"):
                if (a >= frame.PlayerString.Length)
                    a = 0;
                break;
            case ("Trigger"):
                if (a >= frame.Trigger.Length)
                    a = 0;
                break;
            case ("Equal"):
                if (a >= frame.EqualString.Length)
                    a = 0;
                break;
            case ("StatTayp"):
                if (a >= frame.StatTayp.Length)
                    a = 0;
                break;
            default:
                Debug.Log(mood);
                break;
        }
        return a;
    }
    void DeCoder(string cod)
    {
        string[] com = cod.Split('_');
        string text = com[1];
        switch (com[0])
        {

            case ("MainAdd"):
                OpenSelector(text);
                stringMood = com[2];
                break;
            case ("MainRemove"):
                switch (text)
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

            case ("Text"):
                switch (text)
                {
                    case ("RuleName"):
                        LoadTextWindowData(head.Name, text);
                        break;

                    case ("Cost"):
                        LoadTextWindowData($"{head.Cost}", text);
                        break;

                        //case ("CostExtend"):
                        //    LoadTextWindowData($"{head.CostExtend}", text);
                        //    break;

                        //case ("LevelCap"):
                        //    LoadTextWindowData($"{head.LevelCap}", text);
                        //    break;

                        //case ("CostMovePoint"):
                        //    LoadTextWindowData($"{head.CostMovePoint}", text);
                        //    break;

                }
                break;
            case ("Switch"):
                switch (text)
                {
                    case ("Player"):
                        head.Player = !head.Player;
                        LoadMainText();
                        LoadAllText();
                        break;
                }
                break;
            case ("Trigger"):
                int a = int.Parse(text);
                if (a != null)
                {
                    int b = 0;
                    // int b1 = 0;
                    TriggerAction triggerAction = head.TriggerActions[a];

                    if (com.Length > 4)
                    {
                        b = int.Parse(com[4]);


                        if (com.Length > 5)
                        {
                            if (com.Length > 6)
                            {
                                stringMood = $"{a}_{com[3]}_{com[4]}_{com[5]}_{com[6]}";
                            }
                            else
                                stringMood = $"{a}_{com[3]}_{com[4]}_{com[5]}";
                        }
                    }
                    Debug.Log(com[2]);
                    switch (com[2])
                    {
                        case ("Del"):
                            DelTrigger(a);
                            break;

                        case ("PlusDel"):
                            DelIf(a, true, b);
                            break;

                        case ("MinusDel"):
                            DelIf(a, false, b);
                            break;

                        case ("PlusAdd"):
                            AddIf(a, true);
                            break;

                        case ("MinusAdd"):
                            AddIf(a, false);
                            break;

                        case ("ActionDel"):
                            DelAction(a, b);
                            break;
                        case ("ActionAdd"):
                            AddAction(a);
                            break;

                        case ("Switch"):
                            switch (com[3])
                            {
                                case ("TargetPalyer"):
                                    triggerAction.TargetPalyer = NextSwitch(triggerAction.TargetPalyer, com[3]);

                                    TriggerMainText(a);
                                    TriggerRootText(a);
                                    LoadAllText();
                                    break;
                                case ("Trigger"):
                                    triggerAction.Trigger = NextSwitch(triggerAction.Trigger, com[3]);

                                    TriggerMainText(a);
                                    TriggerRootText(a);
                                    LoadAllText();
                                    // Ui.SelectorsMain[0].active = true;
                                    break;
                                case ("CountMod"):
                                    triggerAction.CountMod = !triggerAction.CountMod;

                                    TriggerMainText(a);
                                    TriggerRootText(a);
                                    LoadAllText();
                                    break;
                                case ("CountModExtend"):
                                    triggerAction.CountModExtend = !triggerAction.CountModExtend;

                                    TriggerMainText(a);
                                    TriggerRootText(a);
                                    LoadAllText();
                                    break;
                                default:
                                    if (com.Length > 6)
                                    {
                                        RuleForm ruleForm = null;
                                        int c = int.Parse(com[5]);
                                        if (com[3] == "Action")
                                        {
                                            RuleAction ruleAction = triggerAction.Action[b];
                                            ruleForm = ruleAction.Core[c];
                                        }
                                        else
                                        {
                                            IfAction ifAction = (com[3] == "Plus") ? triggerAction.PlusAction[b] : triggerAction.MinusAction[b];

                                            ruleForm = ifAction.Core[c];
                                        }

                                        SetIntRuleForm(ruleForm, com[6], 0);
                                    }// Result
                                    else
                                    {
                                        IfAction ifAction = (com[3] == "Plus") ? triggerAction.PlusAction[b] : triggerAction.MinusAction[b];
                                        SetIntIfAction(ifAction, "Result", 0);
                                       // SetIntRuleForm(ruleForm, com[5], 0);
                                    }

                                    switch (com[3])
                                    {
                                        case ("Action"):
                                            CreateActionText(a, b);
                                            TriggerActionText(a);
                                            break;
                                        case ("Plus"):
                                            CreateIfText(a, b, true);
                                            TriggerIfText(a, true);
                                            break;
                                        case ("Minus"):
                                            CreateIfText(a, b, false);
                                            TriggerIfText(a, false);
                                            break;
                                    }

                                    TriggerRootText(a);
                                    LoadAllText();
                                    break;
                            }
                            break;
                        case ("Text"):
                            text = "";
                            switch (com.Length) 
                            {
                                case (7):
                                    int c = int.Parse(com[5]);
                                    RuleForm ruleForm = null;
                                    if (com[3] == "Action")
                                    {
                                        RuleAction ruleAction = triggerAction.Action[b];
                                        ruleForm = ruleAction.Core[c];
                                    }
                                    else
                                    {
                                        IfAction ifAction = (com[3] == "Plus") ? triggerAction.PlusAction[b] : triggerAction.MinusAction[b];
                                        ruleForm = ifAction.Core[c];
                                    }

                                    switch (com[6])
                                    {
                                        case ("Num"):
                                            text = ""+ruleForm.Num;
                                            break;
                                        case ("Mod"):
                                            text = "" + ruleForm.Mod;
                                            break;
                                        default:
                                            Debug.Log(com[6]);
                                            break;
                                    }
                                    break;
                                case (6):
                                    if (com[3] == "Action")
                                    {
                                        RuleAction ruleAction = triggerAction.Action[b];
                                        switch (com[5])
                                        {
                                            case ("Num"):
                                                text = "" + ruleAction.Num;
                                                break;
                                            case ("MinPoint"):
                                                text = "" + ruleAction.MinPoint;
                                                break;
                                            case ("MaxPoint"):
                                                text = "" + ruleAction.MaxPoint;
                                                break;
                                            default:
                                                Debug.Log(com[5]);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        IfAction ifAction = (com[3] == "Plus") ? triggerAction.PlusAction[b] : triggerAction.MinusAction[b];
                                        switch (com[5])
                                        {
                                            /*
                                             public int Prioritet;
    public int Point;
                                             
                                             */
                                            case ("Prioritet"):
                                                text = "" + ifAction.Prioritet;
                                                break;
                                            case ("Point"):
                                                text = "" + ifAction.Point;
                                                break;
                                            default:
                                                Debug.Log(com[5]);
                                                break;
                                        }
                                    }

                                    
                                    break;
                                default:
                                    Debug.Log(com.Length);
                                    break;

                            }
                            //trigger_0_TExt_Action_i_num
                            //trigger_0_TExt_Action_num

                            LoadTextWindowData(text,stringMood);
                            break;
                        case ("Selector"):
                            Debug.Log(cod);
                            if (com.Length == 7)
                            {
                                string[] com1 = com[6].Split('|');
                                if (com1.Length == 1)
                                    OpenSelector(com[6]);
                                else
                                {
                                    OpenSelector(com1[1]);
                                    stringMood = $"{a}_{com[3]}_{com[4]}_{com[5]}_{com1[0]}";
                                }
                            }
                            else
                                OpenSelector(com[5]);
                            /*
                             
                string[] com = text.Split('/');
                switch (com[1])
                {
                    case ("Legion"):
                        OpenSelector(com[1]);
                        break;

                    case ("CivilianGroups"):
                        Ui.SelectorMainCivilianGroups = GO;
                        Ui.SelectorCivilianGroups = GO.transform.GetChild(0).GetChild(0);
                        break;

                    case ("Stat"):
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
                    default:
                        Debug.Log(com[1]);
                        break;
                }
                             
                             */
                            //stringMood = $"{a}_{com[3]}_{com[4]}_{com[5]}";
                            break;

                        //case ("GroupLevel"):

                        //    a = ifCore.IntData[b1];
                        //    b = ifCore.IntData[b1 - 2];
                        //    text = "";

                        //    a++;
                        //    if (a >= library.CivilianGroups[b].Tituls.Count)
                        //        a = 0;

                        //    text = library.CivilianGroups[b].Tituls[a].Name;///frame.EqualString[a];
                        //    ifCore.IntData[b1] = a;
                        //    ifCore.TextData[b1] = text;
                        //    LableIfAction(ifAction, com[5], i);
                        //    break;
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
                //  string text1 = com[1];
                int a = int.Parse(com[0]);
                TriggerAction triggerAction = head.TriggerActions[a];
                i = int.Parse(text);
                if (i != null)
                {
                    if (com.Length > 1)
                    {
                        text = com[1];
                        int i1 = int.Parse(com[2]);
                        if (com.Length > 4)
                        {
                            int b1 = int.Parse(com[3]);
                            //int b2 = int.Parse(com[4]);
                            if (text == "Action")
                            {
                                SetIntRuleForm(triggerAction.Action[i1].Core[b1], com[4], i);
                                //RuleAction action = triggerAction.Action[i1];
                                //switch (com[4])
                                //{
                                //    case ("Mod"):
                                //        action.Core[b1].Mod = i;
                                //        break;
                                //    case ("Num"):
                                //        action.Core[b1].Num = i;
                                //        break;
                                //}

                                CreateActionText(a, i1);
                                TriggerActionText(a);
                                TriggerRootText(a);
                                LoadAllText();
                            }
                            else
                            {
                                bool plus = (text == "Plus");
                                IfAction ifAction = (plus) ? triggerAction.PlusAction[i1] : triggerAction.MinusAction[i1];
                                SetIntRuleForm(ifAction.Core[b1], com[4],  i);
                                CreateIfText(a, i1, plus);
                                TriggerIfText(a, plus);
                                TriggerRootText(a);
                                LoadAllText();

                            }

                        }
                        else if (text == "Action")
                        {
                            SetIntRuleAction(triggerAction.Action[i1], com[3],  i);

                            CreateActionText(a, i1);
                            TriggerActionText(a);
                            TriggerRootText(a);
                            LoadAllText();
                        }
                        else
                        {
                            bool plus = (text == "Plus");
                            IfAction ifAction = (plus) ? triggerAction.PlusAction[i1]: triggerAction.MinusAction[i1];

                            SetIntIfAction(ifAction, com[3], i);
                            CreateIfText(a, i1, plus);
                            TriggerIfText(a, plus);
                            TriggerRootText(a);
                            LoadAllText();
                        }
                    }
                    else
                    {
                        //triggerAction.Id = i;
                        //TextReStruct();

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

    #region CreateSystemData
    void CreateListButton(string text)
    {
        GameObject GO = null;
        int a = 0; 
        switch (text)
        {
            case ("StatTayp"):
                a = frame.StatTayp.Length;
                break;
            case ("Legion"):
                a = library.Legions.Count;
                break;

            case ("CivilianGroups"):
                a = library.CivilianGroups.Count;
                break;

            case ("Stat"):
                a = library.Constants.Count;
                break;

            case ("Effects"):
                a = library.Effects.Count;
                break;
            case ("Rule"):
                a = RuleName.Count;
                break;
        }

        for (int i = 0; i < a; i++)
        {
            GO = Instantiate(Ui.ButtonOrig);
            switch (text)
            {
                case ("StatTayp"):
                    GO.transform.SetParent(Ui.SelectorStatTayp);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = frame.StatTayp[i];
                    break;
                case ("Legion"):
                    GO.transform.SetParent(Ui.SelectorLegion);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.Legions[i].Name;
                    break;

                case ("CivilianGroups"):
                    GO.transform.SetParent(Ui.SelectorCivilianGroups);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.CivilianGroups[i].Name;
                    break;

                case ("Stat"):
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
        string[] text = frame.StatTayp;// new string[] { "Legion", "CivilianGroups", "Constants", "Effects","Rule", "StatTayp" };
        string mainText = "";
        for (int i = 0; i < text.Length; i++)
        {
            mainText = text[i];
            GO = Instantiate(Ui.SelectorMain);
            //Debug.Log(text[i]);
            switch (mainText)
            {
                case ("StatTayp"):
                    Ui.SelectorMainStatTayp = GO;
                    Ui.SelectorStatTayp = GO.transform.GetChild(0).GetChild(0);
                    break;

                case ("Legion"):
                    Ui.SelectorMainLegion = GO;
                    Ui.SelectorLegion = GO.transform.GetChild(0).GetChild(0);
                    break;

                case ("CivilianGroups"):
                    Ui.SelectorMainCivilianGroups = GO;
                    Ui.SelectorCivilianGroups = GO.transform.GetChild(0).GetChild(0);
                    break;

                case ("Stat"):
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
                default:
                    Debug.Log(mainText);
                    break;
            }

            if (mainText != "Null")
            {
                GO.name = "Selector" + mainText;
                GO.transform.SetParent(Ui.Canvas);
                GO.transform.position = Ui.SelectorMain.transform.position;
                CreateListButton(mainText);
            }
            else
            {
                Destroy(GO);
            }
        }
    }
    void ButtonSelector(string text, int a, Button button)
    {
        button.onClick.AddListener(() => SwitchLibrary(a, text));
    }
    void SwitchLibrary(int a, string text)
    {
        //SwitchRuleText
        string[] com = stringMood.Split('_');
        if (com.Length == 1)
        {
            switch (com[0])
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
            string text1 = com[1];
            //trigger
            //text1 = com[1];
            // switch()
            int i1 = int.Parse(com[0]);
            int i2 = int.Parse(com[2]);
            int i = int.Parse(com[3]);
            TriggerAction triggerAction = head.TriggerActions[i1];
            if (text1 != "Action")
            {
                bool plus = ("Plus" == text1);
                IfAction ifAction = (plus) ? triggerAction.PlusAction[i2]: triggerAction.MinusAction[i2];

                //i2 = int.Parse(com[4]);
                SetIntRuleForm(ifAction.Core[i], text, a);
                //SetIntIfAction(ifAction, com[3], i);
                CreateIfText(i1, i2, plus);
                TriggerIfText(i1, plus);
                TriggerRootText(i1);
                LoadAllText();
                //IfCore ifCore = ifAction.Core[i2];
                ////ifAction.CoreTextData[i] = text;
                //ifCore.IntData[i] = a;
                //PreLableIfAction(ifAction, ifCore);
                //LableIfAction(ifAction, text1, i1);
            }
            else
            {
                // RuleAction action = triggerAction.Action[i2];
                SetIntRuleForm(triggerAction.Action[i2].Core[i], text, a);
                //SetIntRuleAction(triggerAction.Action[i2], text,  a);

                CreateActionText(i1, i2);
                TriggerActionText(i1);
                TriggerRootText(i1);
                LoadAllText();
                //switch (i)
                //{
                //    case (0):
                //        action.EffectData = a;
                //        break;
                //    case (1):
                //        action.RelizeConstant = a;
                //        break;
                //    case (2):
                //        action.TargetConstant = a;
                //        break;
                //        //case (3):

                //        //    action.IntData = a;

                //        //    break;
                //}
                ////ifAction.TextData[i] = text;
                ////ifAction.IntData[i] = a;

                //CreateActionText(i1, i2);
                //TriggerActionText(i1);
                //TriggerRootText(i1);
            }

        }
        LoadAllText();
        HideSelector();
    }

    #endregion

    #region Library Rule

    private int curentRule =-1;
    public List<string> RuleName;

    void NewRule()
    {
        curentRule = -1;
    }
    void LoadRule(int i)
    {
        curentRule = i;
      //  XMLSaver.LoadRule(ruleConstructor, i);
    }

    public void SetRule(HeadRule headRule)
    {
        head = headRule;


        LoadMainText();
        int a = head.TriggerActions.Count;
        for (int i = 0; i < a; i++)
        {
            TriggerMainText(i);
            TriggerIfText(i, true);
            TriggerIfText(i, false);
            TriggerActionText(i);
            TriggerRootText(i);
        }
        LoadAllText();
        //  TriggerAction triggerAction1 = null;
        //  IfAction ifAction = null;
        ////  RuleAction ruleAction = null;

        //  int b = head.TriggerActions.Count;
        //  int b1 = 0;
        //  for (int i = 0; i < b; i++)
        //  {
        //      triggerAction1 = head.TriggerActions[i];
        //      TriggerMainText(i);

        //      b1 = triggerAction1.PlusAction.Count;
        //      for (int i1 = 0; i1 < b1; i1++)
        //      {
        //          ifAction = triggerAction1.PlusAction[i1];
        //          ifAction.LocalId = i1;

        //          ifAction.Form = frame.AllTriggers[ifAction.MainCore[0]].Form[ifAction.MainCore[1]];
        //          int b2 = ifAction.Core.Count;
        //          for (int i2 = 0; i2 < b2; i2++)
        //          {
        //              PreLableIfAction(ifAction, ifAction.Core[i2]);
        //          }

        //          LableIfAction(ifAction, "Plus", i);
        //      }
        //      TriggerPlusText(i);


        //      b1 = triggerAction1.MinusAction.Count;
        //      for (int i1 = 0; i1 < b1; i1++)
        //      {
        //          ifAction = triggerAction1.MinusAction[i1];
        //          ifAction.LocalId = i1;

        //          ifAction.Form = frame.AllTriggers[ifAction.MainCore[0]].Form[ifAction.MainCore[1]];

        //          for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
        //          {
        //              PreLableIfAction(ifAction, ifAction.Core[i2]);
        //          }
        //          LableIfAction(ifAction, "Minus", i);
        //      }
        //      TriggerMinusText(i);

        //      b1 = triggerAction1.Action.Count;
        //      for (int i1 = 0; i1 < b1; i1++)
        //      {
        //          // ruleAction = triggerAction1.Action[i1];
        //          CreateActionText(i, i1);
        //      }
        //      TriggerActionText(i);

        //      TriggerRootText(i);
        //  }


        // LoadAllText();
    }

    void SaveRule(int i)
    {
         XMLSaver.SaveRule(head, i);
       // XMLSaver.SaveSimpleRule(head, i);
    }

    void SaveCore()
    {
        XMLSaver.SaveMainRule(library);
    }

    void CoreLoad()
    {
        //RuleName = 
            XMLSaver.LoadMainRule(library);
       // ruleConstructor.RuleCount = library.RuleName.Count;
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
            curentRule = RuleName.Count;
            // RuleCount++;
            AddRuleButton(RuleName.Count, head.Name);
            library.RuleName.Add(head.Name);
        }
        SaveRule(curentRule);
        SaveCore();
    }

    public void CoreLoadRule()
    {
        GameObject GO = null;

        for(int i =0; i< RuleName.Count; i++)
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
    #endregion
}
[System.Serializable]
public class TriggerAction
{
    public bool CountMod;
    public bool CountModExtend;
    // public int Id;
    public int TargetPalyer;
    public int Trigger;

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
    public int Prioritet;
    public int Point;

    public int Result;

    public List<RuleForm> Core = new List<RuleForm>();

    public string Text;
}

[System.Serializable]
public class RuleForm
{
    public int Card;//0-null 1-card1 2-card2
    public string StatTayp;
    public string Stat;
    public int Mod =1;
    public int Num;
}
[System.Serializable]
public class RuleAction
{
    public int MinPoint;
    public int MaxPoint;

    public string ActionMood;//
    public string Action;//
    public List<RuleForm> Core = new List<RuleForm>();
    public int Num;

    public int Target;

    public string RootText;
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
