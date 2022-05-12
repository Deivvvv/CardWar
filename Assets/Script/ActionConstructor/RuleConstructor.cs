using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saver;

public class RuleConstructor : MonoBehaviour
{
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
        mainText += LinkSupport(color, "Text_RuleNameText", $"\nИмя -- { head.NameText}");
        mainText += LinkSupport(color, "Tag_Tag", $"\nTag { head.Tag}");

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

    void NewFormAction(ref RuleAction action)
    {
        switch (action.Action)
        {
            case (""):
                break;
        }
    }

    void AddAction(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        RuleAction action = new RuleAction();
        int b = triggerAction.Action.Count;
        action.Action = SwitchRuleText(0, "Action");
        //NewFormAction(ref action);
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

    string RuleFarmeSupportExtend(RuleForm ruleForm, string headText, string headTextExtend)
    {
        string colorText = "#F4FF04";
        string text = "";

        string linkText = headText + $"Switch{headTextExtend}NextCard";
        string textData = $"\n-{ frame.CardString[ruleForm.Card]}";

        text += LinkSupport(colorText, linkText, textData);
        if(ruleForm.Card != 0)
        {
            linkText = headText + $"Selector{headTextExtend}StatTayp";
            textData = $" - {ruleForm.StatTayp}";

            text += LinkSupport(colorText, linkText, textData);


            linkText = headText + $"Selector{headTextExtend}SetStat|{ruleForm.StatTayp}";
            textData = $" - {ruleForm.Stat}";

            text += LinkSupport(colorText, linkText, textData);

            if(ruleForm.StatTayp == "stat")
            {
                linkText = headText + $"Text{headTextExtend}Mod";
                textData = $" - {ruleForm.Mod}";

                text += LinkSupport(colorText, linkText, textData);

            }
        }



        linkText = headText + $"Text{headTextExtend}Num";
        textData = $" - {ruleForm.Num}";

        text += LinkSupport(colorText, linkText, textData);
        return text;
    }

    void CreateIfText( int a, int b, bool plus) 
    {
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

        linkText = headText + $"Switch{headTextExtend}ActionMood";
        text2 = $"\n-Область применения { frame.PlayerString[action.ActionMood]}";

        text1 += LinkSupport(colorText, linkText, text2);

        linkText = headText + $"Selector{headTextExtend}Action";//all only
        text2 = $"\n-Действие { action.Action}";

        text1 += LinkSupport(colorText, linkText, text2);

        linkText = headText + $"Switch{headTextExtend}ForseMood";
        text2 = $"\n-Тип воздействия { frame.ForseTayp[action.ForseMood]}";

        text1 += LinkSupport(colorText, linkText, text2);


        // RuleForm ruleForm = null;
        for (int i=0; i < action.Core.Count; i++)
        {
            text1 += RuleFarmeSupport(action.Core[i], headText, headTextExtend+$"{i}_");
        }
        //linkText = headText +"Text"+ headTextExtend + "Num";
        //text2 = $"\n - {action.Num}";

        //text1 += LinkSupport(colorText, linkText, text2);


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
            case ("Status"):
                str = frame.Status[a];
                break;
            case ("Action"):
                str = frame.Action[a];
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
            case ("Status"):
                Ui.SelectorMainStatus.active = true;
                break;

            case ("Action"):
                Ui.SelectorMainAction.active = true;
                break;
            case ("Tag"):
                Ui.SelectorMainTag.active = true;
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
        Ui.SelectorMainTag.active = false;
        Ui.SelectorMainStatTayp.active = false;
        Ui.SelectorMainAltRule.active = false;
        Ui.SelectorMainLegion.active = false;
        Ui.SelectorMainCivilianGroups.active = false;
        Ui.SelectorMainConstants.active = false;
        Ui.SelectorMainAction.active = false;
        Ui.SelectorMainStatus.active = false;
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
                //Debug.Log(ruleForm.StatTayp);
                ruleForm.Stat = SwitchRuleText(a, ruleForm.StatTayp);
                break;
            case ("Status"):
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
            case ("Num"):
                //action.Num = a;
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
            case ("ForseMood"):
                if (a >= frame.ForseTayp.Length)
                    a = 0;
                break;
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
            case ("Tag"):
                OpenSelector(text);
                stringMood = text;
                break;
            case ("MainAdd"):
                OpenSelector(text);
                stringMood = com[1];
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
                    case ("RuleNameText"):
                        LoadTextWindowData($"{head.NameText}", text);
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
                    //Debug.Log(com[2]);
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
                                        //      case ("TargetPalyer"):
                                        //triggerAction.TargetPalyer = NextSwitch(triggerAction.TargetPalyer, com[3]);

                                        //TriggerMainText(a);
                                        //TriggerRootText(a);
                                        //LoadAllText();
                                        //break;
                                        if (com[3] == "Action")
                                        {
                                            RuleAction ruleAction = triggerAction.Action[b];
                                            if(com[5] == "ForseMood")
                                            {
                                                ruleAction.ForseMood = NextSwitch(ruleAction.ForseMood, "ForseMood");
                                            }
                                            else
                                                ruleAction.ActionMood = NextSwitch(ruleAction.ActionMood, "TargetPalyer");
                                        }
                                        else { 
                                            IfAction ifAction = (com[3] == "Plus") ? triggerAction.PlusAction[b] : triggerAction.MinusAction[b];
                                        SetIntIfAction(ifAction, "Result", 0);
                                        }
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
                                            //case ("ActionMood"):

                                               // break;

                                            case ("Num"):
                                            //    text = "" + ruleAction.Num;
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

            case ("RuleNameText"):
                head.NameText = text;
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
        Application.targetFrameRate = 30;
        head = new HeadRule();
        head.Tag = frame.Tag[0];
        XMLSaver.SetRuleMainFrame(frame);

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

            case ("Action"):
                a = frame.Action.Length;
                break;
            case ("Rule"):
                a = RuleName.Count;
                break;
            case ("Tag"):
                a = frame.Tag.Length;
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

                case ("Action"):
                    GO.transform.SetParent(Ui.SelectorAction);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = frame.Action[i];
                    break;
                case ("Rule"):
                    GO.transform.SetParent(Ui.SelectorAltRule);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = RuleName[i];
                    break;
                case ("Tag"):
                    GO.transform.SetParent(Ui.SelectorTag);
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = frame.Tag[i];
                    break;
            }
            ButtonSelector(text, i, GO.GetComponent<Button>());
        }
    }
    void LoadBase()
    {
        Ui.TextWindowButton.onClick.AddListener(() => LoadData());
        Ui.SaveButton.onClick.AddListener(() => CreateRule());
        Ui.SaveSimpleButton.onClick.AddListener(() => SaveRuleSample(curentRule));
        Ui.SaveSimpleAllButton.onClick.AddListener(() => SaveRuleSampleAll());
        Ui.NewRuleButton.onClick.AddListener(() => NewRule());

        GameObject GO = null;
        string[] text = frame.StatTayp;
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

                case ("Action"):
                    Ui.SelectorMainAction = GO;
                    Ui.SelectorAction = GO.transform.GetChild(0).GetChild(0);
                    break;
                case ("Rule"):
                    Ui.SelectorMainAltRule = GO;
                    Ui.SelectorAltRule = GO.transform.GetChild(0).GetChild(0);
                    break;
                case ("Tag"):
                    Ui.SelectorMainTag = GO;
                    Ui.SelectorTag= GO.transform.GetChild(0).GetChild(0);
                    break;
                case ("Status"):
                    Ui.SelectorMainStatus = GO;
                    Ui.SelectorStatus = GO.transform.GetChild(0).GetChild(0);
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
                case ("Tag"):
                    head.Tag = frame.Tag[a];
                    break;
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
            TriggerAction triggerAction = head.TriggerActions[i1];
            Debug.Log(com.Length);
            if (com.Length == 4)
            {
                triggerAction.Action[i2].Action = SwitchRuleText(a, text);
                //SetIntRuleForm(triggerAction.Action[i2], text, a);
                CreateActionText(i1, i2);
                TriggerActionText(i1);
            }
            else
            {
                int i = int.Parse(com[3]);
                if (text1 != "Action")
                {
                    bool plus = ("Plus" == text1);
                    IfAction ifAction = (plus) ? triggerAction.PlusAction[i2] : triggerAction.MinusAction[i2];

                    SetIntRuleForm(ifAction.Core[i], text, a);
                    CreateIfText(i1, i2, plus);
                    TriggerIfText(i1, plus);
                }
                else
                {
                    SetIntRuleForm(triggerAction.Action[i2].Core[i], text, a);

                    CreateActionText(i1, i2);
                    TriggerActionText(i1);
                }
            }
            TriggerRootText(i1);
            LoadAllText();

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
        SetRule(XMLSaver.LoadRule(i));
    }

    public void SetRule(HeadRule headRule)
    {
        head = headRule;


        LoadMainText();
        int a = head.TriggerActions.Count;
        TriggerAction triggerAction = null;
        for (int i = 0; i < a; i++)
        {
            triggerAction = head.TriggerActions[i];
            TriggerMainText(i);

            for (int i1 = 0; i1 < triggerAction.PlusAction.Count; i1++)
                CreateIfText(i, i1, true);
            TriggerIfText(i, true);

            for (int i1 = 0; i1 < triggerAction.MinusAction.Count; i1++)
                CreateIfText(i, i1, false);
            TriggerIfText(i, false);

            for (int i1 = 0; i1 < triggerAction.Action.Count; i1++)
                CreateActionText(i, i1);
            TriggerActionText(i);

            TriggerRootText(i);
        }
        LoadAllText();
    }

    void SaveRule(int i)
    {
         XMLSaver.SaveRule(head, i);
    }
    void SaveRuleSample(int i)
    {
        XMLSaver.SaveSimpleRule(head, i);
    }
    void SaveRuleSampleAll()
    {
        HeadRule rule = null;
        for ( int i =0; i < RuleName.Count; i++)
        {
            rule = XMLSaver.LoadRule(i);
            XMLSaver.SaveSimpleRule(rule, i);
        }
    }

    void SaveCore()
    {
        XMLSaver.SaveMainRule(library);
    }

    void CoreLoad()
    {
        XMLSaver.LoadMainRule(library);
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

    public int ActionMood;//
    public string Action;//
    public List<RuleForm> Core = new List<RuleForm>();
    //public int Num;

    public int ForseMood;//

    public string RootText;
}

[System.Serializable]
public class HeadRule
{
    public string Name = "World";//Название
    public string Info;//Описание
    public string NameText = "Благочестие";//Название
    public string Tag; //Описание

    public int Cost;//Цена
    public int CostExtend;//цена за доп очки навыков

    public int LevelCap;//Максимальный уровень способности

    //public int CostMovePoint;

    public bool Player;


    public List<TriggerAction> TriggerActions = new List<TriggerAction>();

    public List<string> NeedRule = new List<string>();
    public List<string> EnemyRule = new List<string>();
}
