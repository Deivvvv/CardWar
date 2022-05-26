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

    private string defColor = "green";

    private int curentRule = -1;
    private string oldTag;

    private string ruleLable;
    private string comLable;
    private string actionLable;
    private string allText;
    private string mainText;
    //private string plusText;
    //private string minusText;

    //private string actionText;

    [SerializeField]
    private TextMeshProUGUI TT;
    [SerializeField]
    private TextMeshProUGUI TT1;
    [SerializeField]
    private TextMeshProUGUI TT2;

    private int[] keyWord;

    void PointerClick()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TT, Input.mousePosition, Camera.main);
        TMP_LinkInfo linkInfo = new TMP_LinkInfo();

        if (linkIndex == -1)
        {
            linkIndex = TMP_TextUtilities.FindIntersectingLink(TT1, Input.mousePosition, Camera.main);
            if (linkIndex == -1)
            {
                linkIndex = TMP_TextUtilities.FindIntersectingLink(TT2, Input.mousePosition, Camera.main);
                if (linkIndex == -1)
                {
                    Debug.Log("Open link -1");
                    return;
                }
                else
                    linkInfo = TT2.textInfo.linkInfo[linkIndex];
            }
            else
                linkInfo = TT1.textInfo.linkInfo[linkIndex];
        }
        else
            linkInfo = TT.textInfo.linkInfo[linkIndex];


       // TMP_LinkInfo linkInfo = (!extend) ? TT.textInfo.linkInfo[linkIndex] : TT1.textInfo.linkInfo[linkIndex];

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

    void AddCore(ref RuleAction action, int a)
    {
        for(int i =0; i< a;i++)
            action.Core.Add(AddRuleForm());
    }

    void NewFormAction(ref RuleAction action)
    {
        action.Core = new List<RuleForm>();
        string[] com = action.Action.Split('|');
        switch (com[0])
        {
            case ("Attack"):
                AddCore(ref action, 3);
                break;
            case ("Stat"):
                AddCore(ref action, 3);
                break;
            case ("Status"):
                AddCore(ref action, 1);
                break;
            case ("Effect"):
                if(com[1] == "Eternal")
                    AddCore(ref action, 5);
                else
                    AddCore(ref action, 7);
                break;
        }
    }

    void AddAction(int a)
    {
        TriggerAction triggerAction = head.TriggerActions[a];
        RuleAction action = new RuleAction();
        int b = triggerAction.Action.Count;
        action.Action = SwitchRuleText(0, "Action");
        NewFormAction(ref action);

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

        Debug.Log(b);
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
        triggerAction.TargetPalyer = 0;
        triggerAction.Trigger = 0;

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

        string[] com = ruleForm.Card.Split('/');


        string linkText = headText + $"Switch{headTextExtend}NextCard";
        string textData = "";
        if (com.Length == 1)
        {
            textData = $"{ruleForm.Card}";
            text += LinkSupport(colorText, linkText, textData);
        }
        else
        {
            textData = $"{com[0]} ";
            text += LinkSupport(colorText, linkText, textData);

            linkText = headText + $"Switch{headTextExtend}CardView";
            textData = $"[{com.Length-1}] ";
            text += LinkSupport(colorText, linkText, textData);


        }


        if (ruleForm.Card != "Null")
        {
            linkText = headText + $"Selector{headTextExtend}StatTayp";
            textData = $" - {ruleForm.StatTayp}";

            text += LinkSupport(colorText, linkText, textData);

            if(ruleForm.StatTayp == "Rule")
                linkText = headText + $"Selector{headTextExtend}SetStat|TagRule";
            else
                linkText = headText + $"Selector{headTextExtend}SetStat|{ruleForm.StatTayp}";
            textData = $" - {ruleForm.Stat}";

            text += LinkSupport(colorText, linkText, textData);


            if(ruleForm.StatTayp == "Stat" || ruleForm.StatTayp == "Stat-Max")
            {
                linkText = headText + $"Text{headTextExtend}Mod";
                textData = $" - {ruleForm.Mod}";

                text += LinkSupport(colorText, linkText, textData);


                linkText = headText + $"Text{headTextExtend}Num";
                textData = $" - {ruleForm.Num}";

                text += LinkSupport(colorText, linkText, textData);
            }

        }
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
        text2 = $"  -Очки { action.Point}";// text2 = $"\n-Очки { action.Point}";

        text1 += LinkSupport(colorText, linkText, text2);


        //linkText = headText + $"Switch{headTextExtend}Result";
        //text2 = $"\n-Ожидаемый результат {SwitchRuleText(action.Result, "Equal")}";

        //text1 += LinkSupport(colorText, linkText, text2);

        text1 += "\n";
        // RuleForm ruleForm = null;
        for (int i = 0; i < action.Core.Count; i++)
        {
            if (i % 2 > 0)
            {
                linkText = headText + $"Switch{headTextExtend}Result";
                text2 = $"  {SwitchRuleText(action.Result, "Equal")}  ";

                text1 += LinkSupport(colorText, linkText, text2);
            }
            //if (i % 2 > 0)
            //    text1 += " / ";
            //else
            //    text1 += "\n";
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


        linkText = headText + $"Text{headTextExtend}Name";
        text2 = $"\n- Имя { action.Name}";
        text1 += LinkSupport(colorText, linkText, text2);

        linkText = headText + $"Text{headTextExtend}MinPoint";
        text2 = $"\n- Стоймость мин { action.MinPoint}";
        text1 += LinkSupport(colorText, linkText, text2);
        
        linkText = headText + $"Text{headTextExtend}MaxPoint";
        text2 = $"  - мах {action.MaxPoint}";
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
            if (i % 2 > 0)
                text1 += " / ";
            else
                text1 += "\n";


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
            case ("Stat-Max"):
                str = library.Constants[a].Name;
                break;
            case ("Status"):
                str = frame.Status[a];
                break;
            case ("Tag"):
                str = frame.Tag[a];
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
            case ("Stat-Max"):
                ruleForm.Stat = SwitchRuleText(a, ruleForm.StatTayp);
                break;
            case ("Stat"):
                ruleForm.Stat = SwitchRuleText(a, ruleForm.StatTayp);
                break;
            case ("Status"):
                ruleForm.Stat = SwitchRuleText(a, ruleForm.StatTayp);
                break;
            case ("Rule"):
                int b = a / 1000;
                a = a % 1000;
                ruleForm.Stat = library.Rule[b].Name+ "_"+ library.Rule[b].Rule[a];// SwitchRuleText(a, ruleForm.StatTayp);
                break;
            case ("Mod"):
                if(a!=0)
                    ruleForm.Mod = a;
                break;
            case ("Num"):
                ruleForm.Num = a;
                break;
            //case ("NextCard"):
            //    ruleForm.Card = NextSwitch(ruleForm.Card, "Card");
            //    break;
            //case ("Card"):
            //    ruleForm.Card = a;
            //    break;
            case ("Tag"):
                ruleForm.Stat = SwitchRuleText(a, ruleForm.StatTayp);
                break;
            default:
                Debug.Log(text);
                break;
        }
    }
    void SetIntIfAction(IfAction action, string text, int a)
    {
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
        string[] com1 = null;
        string text = com[1];

        int a = 0;
        switch (com[0])
        {
            case ("Select"):
                if(text != "Card" && text != "UseKeyWord")
                    a = int.Parse(com[2]);
                com1 = stringMood.Split('_');
                switch (text)
                {
                    case ("UseKeyWord"):
                        string str = "";
                        for(int i = 0; i < keyWord.Length; i++)
                        {
                            a = keyWord[i];
                            if (a != 0)
                            {
                                str += $"/{frame.KeyWord[i]}-{frame.KeyWordStatus[a]}";
                            }
                        }

                        DeCoder($"Select_Card_All{str}");
                        return;
                        break;
                    case ("Tag"):
                        head.Tag = frame.Tag[a];//frame.Tag[a];
                        break;
                    case ("TagRule"):
                        GenerateComand($"_Rule_{a}");
                        return;
                        break;
                    case ("KeyWord"):
                        int b = int.Parse(com[3]);
                        keyWord[a] = b;
                        GenerateComand("_All");
                        return;
                        break;
                    //case ("All"):
                    //    Debug.Log(cod);
                    //    int b = int.Parse(com[3]);
                    //    keyWord[a] = b;
                    //    GenerateComand("_All");
                    //    break;

                    default:
                        string text2 = "";
                        if (text == "Rule")
                        {
                            text2 = library.Rule[a].Name + "_" + library.Rule[a].Rule[int.Parse(com[3])];
                            if(com1[0] != "Rule")
                            {
                                if(com1[0] == "NeedRule")
                                    head.NeedRule.Add(text2);
                                else
                                    head.EnemyRule.Add(text2);
                                LoadMainText();
                                LoadAllText();
                                ComandClear();
                                return;
                            }
                        } 

                        //Debug.Log(cod);
                        string text1 = com1[1];
                        int i1 = int.Parse(com1[0]);
                        int i2 = int.Parse(com1[2]);
                        TriggerAction triggerAction = head.TriggerActions[i1];
                        if (com1.Length == 4)
                        {
                            RuleAction action = triggerAction.Action[i2];
                            action.Action = SwitchRuleText(a, text);
                            NewFormAction(ref action);
                            CreateActionText(i1, i2);
                            TriggerActionText(i1);
                        }
                        else
                        {
                            int i = int.Parse(com1[3]);
                            if (text1 != "Action")
                            {
                                bool plus = ("Plus" == text1);
                                IfAction ifAction = (plus) ? triggerAction.PlusAction[i2] : triggerAction.MinusAction[i2];

                                switch (text) 
                                {
                                    case ("Rule"):
                                        ifAction.Core[i].Stat = text2;
                                        break;
                                    case ("Card"):
                                        if(com[2] == "All")
                                        {
                                            NewAll(ifAction.Core[i].Card);
                                            return;
                                        }
                                        else
                                            ifAction.Core[i].Card = com[2];
                                        break;
                                    default:
                                        SetIntRuleForm(ifAction.Core[i], text, a);
                                        break;
                                }

                                CreateIfText(i1, i2, plus);
                                TriggerIfText(i1, plus);
                            }
                            else
                            {
                                switch (text)
                                {
                                    case ("Rule"):
                                        triggerAction.Action[i2].Core[i].Stat = text2;
                                        break;
                                    case ("Card"):
                                        if (com[2] == "All")
                                        {
                                            NewAll(triggerAction.Action[i2].Core[i].Card);
                                            return;
                                        }
                                        else
                                            triggerAction.Action[i2].Core[i].Card = com[2];
                                        break;
                                    default:
                                        SetIntRuleForm(triggerAction.Action[i2].Core[i], text, a);
                                        break;
                                }

                                CreateActionText(i1, i2);
                                TriggerActionText(i1);
                            }
                        }
                        TriggerRootText(i1);
                        break;
                }
                ComandClear();
                LoadAllText();
                break;

            case ("Save"):
                CreateRule();
                //SaveRule(curentRule);
                break;
            case ("SaveAll"):
                break;
            case ("Zip"):
                SaveRuleSample(curentRule);
                break;
            case ("ZipAll"):
                SaveRuleSampleAll();
                break;

            case ("Tag"):
                GenerateComand("_"+text);
                stringMood = text;
                break;
            case ("MainAdd"):
                GenerateComand("_TagRule");
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
                    case ("Tag"):
                        if (com[2] == "-1")
                            ReturnTag();
                        else
                            SwitchTag(int.Parse(com[2]));
                       // SwitchTag(int.Parse(cod[2]));
                        break;
                    case ("LoadRule"):
                        LoadRule(com[2], int.Parse( com[3]));
                        break;
                    case ("NewRule"):
                        curentRule = -1;
                        //LoadRule(cod[2]);
                        break;
                        //$"Switch_Tag_-1", "Back\n");
                        //for (int i = 0; i < library.Rule[a].Rule.Count; i++)
                        //{
                        //    ruleLable += LinkSupport(defColor, $"Switch_LoadRule_{i}",

                }
                break;
            case ("Trigger"):
                a = int.Parse(text);
                if (a != null)
                {
                    int b = 0;
                    // int b1 = 0;
                    TriggerAction triggerAction = head.TriggerActions[a];
                    if (com.Length == 4)
                        b = int.Parse(com[3]);
                    else if (com.Length > 4)
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
                                        if (com[6] == "NextCard")
                                        {
                                            GenerateComand("_Card");
                                            return;
                                        }

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

                                        if (com[6] == "CardView")
                                        {
                                            ComandView(ruleForm.Card);
                                            return;
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
                                            case ("Name"):
                                                text = "" + ruleAction.Name;
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
                                com1 = com[6].Split('|');
                                if (com1.Length == 1)
                                    GenerateComand("_" + com[6]);// OpenSelector(com[6]);
                                else 
                                {
                                    GenerateComand("_" + com1[1]);
                                    //OpenSelector(com1[1]);
                                    stringMood = $"{a}_{com[3]}_{com[4]}_{com[5]}_{com1[0]}";
                                }
                            }
                            else
                                GenerateComand("_" + com[5]);
                            ///OpenSelector(com[5]);
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

                if (com[3] == "Name")
                {
                    int i1 = int.Parse(com[2]);
                    triggerAction.Action[i1].Name = text;
                    CreateActionText(a, i1);
                    TriggerActionText(a);
                    TriggerRootText(a);
                }
                else 
                { 
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
                                    //LoadAllText();
                                }
                                else
                                {
                                    bool plus = (text == "Plus");
                                    IfAction ifAction = (plus) ? triggerAction.PlusAction[i1] : triggerAction.MinusAction[i1];
                                    SetIntRuleForm(ifAction.Core[b1], com[4], i);
                                    CreateIfText(a, i1, plus);
                                    TriggerIfText(a, plus);
                                    TriggerRootText(a);
                                    //LoadAllText();

                                }

                            }
                            else if (text == "Action")
                            {
                                SetIntRuleAction(triggerAction.Action[i1], com[3], i);

                                CreateActionText(a, i1);
                                TriggerActionText(a);
                                TriggerRootText(a);
                                //LoadAllText();
                            }
                            else
                            {
                                bool plus = (text == "Plus");
                                IfAction ifAction = (plus) ? triggerAction.PlusAction[i1] : triggerAction.MinusAction[i1];

                                SetIntIfAction(ifAction, com[3], i);
                                CreateIfText(a, i1, plus);
                                TriggerIfText(a, plus);
                                TriggerRootText(a);
                                //LoadAllText();
                            }
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


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PointerClick();
        if (Input.GetMouseButtonDown(1))
        {
            Ui.TextWindow.active = false;
            ComandClear();
           // Ui.TextWindow.active = false;
        }
    }
    void Start()
    {
        Application.targetFrameRate = 30;

        head = new HeadRule();
        oldTag = head.Tag = frame.Tag[0];
        XMLSaver.SetRuleMainFrame(frame);
        library.Rule = new List<SubRuleHead>();
        Ui.TextWindowButton.onClick.AddListener(() => LoadData());

        CoreLoad();
        ReturnTag();
        SysComGenerate();
       // LoadBase();


        LoadMainText();
        LoadAllText();

    }

    #region CreateSystemData
    void NewAll(string str)
    {

        GenerateComand($"_ClearKeyWord_{str}");
        GenerateComand("_All");
    }

    void GenerateComand(string str)
    {
        string[] com = str.Split('_');
        actionLable = "";
        switch (com[1])
        {
            case ("TagRule"):
                for(int i =0; i < library.Rule.Count;i++)
                    actionLable += LinkSupport(defColor, $"Select_TagRule_{i}", $"{library.Rule[i].Name} ({library.Rule[i].Rule.Count})\n");
                break;
            case ("Rule"):
                int a = int.Parse(com[2]);
                for (int i = 0; i < library.Rule[a].Rule.Count; i++)
                    actionLable += LinkSupport(defColor, $"Select_Rule_{a}_{i}", $"{library.Rule[a].Rule[i]}\n");
                break;
            case ("StatTayp"):
                for (int i = 0; i < frame.StatTayp.Length; i++)
                    actionLable += LinkSupport(defColor, $"Select_StatTayp_{i}", $"{frame.StatTayp[i]}\n");
                break;
            
            case ("Legion"):
                for (int i = 0; i < library.Legions.Count; i++)
                    actionLable += LinkSupport(defColor, $"Select_Legion_{i}", $"{library.Legions[i].Name}\n");
                break;
            case ("CivilianGroups"):

                break;
            case ("Stat"):
                for (int i = 0; i < library.Constants.Count; i++)
                    actionLable += LinkSupport(defColor, $"Select_Stat_{i}", $"{library.Constants[i].Name}\n");
                break;
            case ("Stat-Max"):
                for (int i = 0; i < library.Constants.Count; i++)
                    actionLable += LinkSupport(defColor, $"Select_Stat_{i}", $"{library.Constants[i].Name}\n");
                break;

            case ("Action"):
                for (int i = 0; i < frame.Action.Length; i++)
                    actionLable += LinkSupport(defColor, $"Select_Action_{i}", $"{frame.Action[i]}\n");
                break;
            case ("Tag"):
                for (int i = 0; i < frame.Tag.Length; i++)
                    actionLable += LinkSupport(defColor, $"Select_Tag_{i}", $"{frame.Tag[i]}\n");
                break;
            case ("Status"):
                for (int i = 0; i < frame.Status.Length; i++)
                    actionLable += LinkSupport(defColor, $"Select_Status_{i}", $"{frame.Status[i]}\n");
                break;



            case ("Card"):
                for (int i = 0; i < frame.CardString.Length; i++)
                    actionLable += LinkSupport(defColor, $"Select_Card_{frame.CardString[i]}", $"{frame.CardString[i]}\n");
                break;

            case ("ClearKeyWord"):
                keyWord = new int[frame.KeyWord.Count];
                com = com[2].Split('/');
                string[] com1;
                int b;
                if (com[0] == "All")
                    for(int i =1;i< com.Length; i++)
                    {
                        com1 = com[i].Split('-');
                        a = frame.KeyWord.FindIndex(x => x == com1[0]);
                        b = frame.KeyWordStatus.FindIndex(x => x == com1[1]);

                        keyWord[a] = b;
                    }

                break;
            case ("All"):
                actionLable += LinkSupport(defColor, $"Select_UseKeyWord", $"UseKeyWord     ");
                //for (int i = 0; i < frame.KeyWordStatus.Length; i++)
                //    actionLable += frame.KeyWordStatus[i] + " ";

                string subStr = "";
                for (int i = 0; i < frame.KeyWord.Count; i++)
                {
                    actionLable += $"\n{frame.KeyWord[i]}   ";
                    for(int i1 = 0; i1 < frame.KeyWordStatus.Count; i1++)
                    {
                        subStr = frame.KeyWordStatus[i1];
                        if (keyWord[i] == i1)
                            actionLable += $"{subStr} ";
                        else
                            actionLable += LinkSupport(defColor, $"Select_KeyWord_{i}_{i1}", $"{subStr} ");
                    }
                }
                break;
        }


        TT2.text = comLable + actionLable;
    }

    void ComandClear()
    {
        TT2.text = comLable;
    }
    void ComandView(string str)
    {
        string[] com = str.Split('/');
        str = "";
        foreach(string s in com)
        {
            str += s + "\n";
        }


        TT2.text = comLable + str;
    }
    #endregion

    #region Library Rule
    void SysComGenerate()
    {

        comLable = "";
        comLable += LinkSupport(defColor, $"Save_", "Save      ");
        comLable += LinkSupport(defColor, $"SaveAll_", "   SaveAll    ");
        comLable += LinkSupport(defColor, $"Zip_", "   Zip     ");
        comLable += LinkSupport(defColor, $"ZipAll_", "    ZipAll\n");
        comLable += "\n";

        TT2.text = comLable;
    }

    void ReturnTag()
    {
        ruleLable = "";
        ruleLable += LinkSupport(defColor, $"Switch_NewRule", "NewRule\n");
        for (int i =0;i< library.Rule.Count; i++)
        {
            ruleLable += LinkSupport(defColor,$"Switch_Tag_{i}", library.Rule[i].Name + $" ({library.Rule[i].Rule.Count})\n");
        }
        TT1.text = ruleLable;
    }

    void SwitchTag(int a)
    {
        string text = library.Rule[a].Name;
        ruleLable = "";
        ruleLable += LinkSupport(defColor, $"Switch_Tag_-1", $"Back ({text})\n");
       
        for (int i = 0; i < library.Rule[a].Rule.Count; i++)
        {
            ruleLable += LinkSupport(defColor, $"Switch_LoadRule_{text}_{i}", library.Rule[a].Rule[i] + "\n");
        }

        TT1.text = ruleLable;
    }

    void LoadRule(string tag, int i)
    {
        curentRule = i;
        SetRule(XMLSaver.LoadRule(tag,i));
    }

    public void SetRule(HeadRule headRule)
    {
        head = headRule;
        oldTag = head.Tag;


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
        for (int i = 0; i < library.Rule.Count; i++)
        {
            for (int i1 = 0; i1 < library.Rule[i].Rule.Count; i1++)
            {
                rule = XMLSaver.LoadRule(library.Rule[i].Name,i1);
                XMLSaver.SaveSimpleRule(rule, i);
            }
        }
    }

    void SaveCore() { XMLSaver.SaveMainRule(library); }

    void CoreLoad() { XMLSaver.LoadMainRule(library); }

    void CreateRule()
    {
        int a = library.Rule.FindIndex(x => x.Name == oldTag);
        if (curentRule != -1)
        {
            if (oldTag != head.Tag)
            {
                int b = library.Rule[a].Rule.Count - 1;
                if (b != 0) 
                {
                    library.Rule[a].Rule[curentRule] = library.Rule[a].Rule[b];
                    library.Rule[a].Rule.RemoveAt(b);

                    System.IO.File.Move(oldTag + $"{b}", oldTag + $"{curentRule}");
                }
                else
                {
                    library.Rule.RemoveAt(a);
                }
                curentRule = -1;
                CreateRule();
                return;
            }

            library.Rule[a].Rule[curentRule] = head.Name;
           // Ui.SelectorLibrary.GetChild(curentRule+1).GetChild(0).gameObject.GetComponent<Text>().text = head.Name;
        }
        else if( a != -1)
        {
            curentRule = library.Rule[a].Rule.Count;
            library.Rule[a].Rule.Add(head.Name);
        }
        else
        {
            a = library.Rule.Count;
            SubRuleHead subRuleHead = new SubRuleHead();
            subRuleHead.Name = head.Tag;
            subRuleHead.Rule.Add(head.Name);
            library.Rule.Add(subRuleHead);
            curentRule = 0;
        }
        oldTag = head.Tag;
        SwitchTag(a);

        SaveRule(curentRule);
        SaveCore();
    }

    //public void CoreLoadRule()
    //{
    //    GameObject GO = null;

    //    for(int i =0; i< RuleName.Count; i++)
    //    {
    //        AddRuleButton(i, RuleName[i]);
    //    }
    //}
    //void AddRuleButton(int i, string text)
    //{
    //    GameObject GO = Instantiate(Ui.ButtonOrig);
    //    GO.transform.SetParent(Ui.SelectorLibrary);
    //    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = text;
    //    GO.GetComponent<Button>().onClick.AddListener(() => LoadRule(i));
    //}
    #endregion
}
//[System.Serializable]
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

//[System.Serializable]
public class IfAction 
{
    public int Prioritet;
    public int Point;

    public int Result;

    public List<RuleForm> Core = new List<RuleForm>();

    public string Text;
}

//[System.Serializable]
public class RuleForm
{
    public string Card = "Null";//0-null 1-card1 2-card2
    public string StatTayp;
    public string Stat;
    public int Mod =1;
    public int Num;
}
//[System.Serializable]
public class RuleAction
{
    public string Name = "Action";
    public int MinPoint;
    public int MaxPoint;

    public int ActionMood;//
    public string Action;//
    public List<RuleForm> Core = new List<RuleForm>();
    //public int Num;

    public int ForseMood;//

    public string RootText;
}

//[System.Serializable]
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


public class SubRuleHead
{
    public string Name;
    public List<string> Rule = new List<string>();
}

