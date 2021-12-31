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
    /*Тело--
     Название
    описание
    Стоимость
    Стоимость очков действий
    Доп стоймость за последующие очки навыка
     
    уровень доступа - разработчик, игрок:
    *МакроДействие++
     */

    /*МакроДействие--
    Время в которое работает способность - Стрельба, бой, оба
    За кем следим - Я, враг, оба
    класс действия - Начало хода, конец хода, является используемой способностью. перед атакой, после атаки, при установке карты, при возвращении карты в руку, при смерти карты
     Условия Дейстивия положительные++
    условвия Дейстивия исключения++
     */

    /*УсловиеДейстивия--
     * охват - любой, все, несколько (2)
     * группа - все, друзья, враги
     * 
     * приоритет условия в цифровом эквиваленте, если цифра совпадает, то необходимо выполнение обоих условий //группа условия - и, или, , несколько (2и более), не более, только
    

    проверяемое событие
     условие - меньше, равно, больше
    
    проверяемые характеристики
    цель условия
     
     */

    /*Действие--
     * на кого действует - все, враги, друзья
     Область действия - цель, я, все
     Накладываемый эффект
     */
    #region Base
    private int sysMood=-1;

    public string Name = "Благочестие";//Название
    public string Info;//Описание

    public int Cost;//Цена
    public int CostExtend;//цена за доп очки навыков

    public int LevelCap;//Максимальный уровень способности

    public int CostMovePoint;

    public bool Player;

    private string stringMood = "";

    public List<TriggerAction> triggerActions;
    #endregion

    #region test
    private List<string> Mood;
    private List<string> TargetPalyer;
    private List<string> TargetTime;
    private List<string> IfString;

    [SerializeField]
    private Color[] colors;
    #endregion

    private List<InputField> mainField;


    private string allText;
    private string mainText;

    private string plusText;
    private string minusText;

    private string actionText;

    public TextMeshProUGUI TT;//Фракции
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
        allText += $"\n\nТригеры";
        int a = triggerActions.Count;
        for (int i = 0; i < a; i++)
        {
            allText += triggerActions[i].RootText;
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
                TriggerAction triggerAction = triggerActions[int.Parse(com[0])];
                i = int.Parse(text);
                if (i != null)
                {
                    if (com.Length > 1)
                    {
                        int i1 = int.Parse(com[2]);
                        switch (com[1])
                        {
                            case ("Plus"):
                                triggerAction.PlusAction[i1].Prioritet = i;
                                break;

                            case ("Minus"):
                                triggerAction.MinusAction[i1].Prioritet = i;
                                break;
                        }
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
        IAddLink(-1,0, color1, "Switch_RuleName", $"Правило -- {Name}");
        IAddLink(-1, 0, color1, "Switch_Cost", $"\nЦена: {Cost}");

        if (CostExtend == 0)
            IAddLink(-1, 0, color1, "Switch_CostExtend", $"\nЦена за доп очки: -- идентично");
        else
            IAddLink(-1, 0, color1, "Switch_CostExtend", $"\nЦена за доп очки: -- {CostExtend}");


        if (LevelCap == 0)
            IAddLink(-1, 0, color1, "Switch_LevelCap", $"\nМаксимальный уровень - без ограничений");
        else
            IAddLink(-1, 0, color1, "Switch_LevelCap", $"\nМаксимальный уровень - {LevelCap}");


        IAddLink(-1, 0, color1, "Switch_CostMovePoint", $"\nОчков хода за применение (? - может сделать в блоке действий или макро действий?) {CostMovePoint}");

        if (!Player)
            IAddLink(-1, 0, color1, "Switch_Player", $"\nУровень доступа - Разработчик");
        else
            IAddLink(-1, 0, color1, "Switch_Player", $"\nУровень доступа - Игрок");
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

    void DelIf(int a, bool plus, int b)
    {
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
        triggerAction.TargetTime = 0;//TargetTime[0];//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(свойства с кладбища)

        triggerAction.PlusAction = new List<IfAction>();
        triggerAction.MinusAction = new List<IfAction>();

        int a = triggerActions.Count;
        triggerAction.LocalId = a;
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
        triggerAction.MainText += $"<link=Trigger_{a}_Del><color=green>-Удалить триггер</color></link>";
        IAddLink(a, 0, "green", $"Trigger_{a}_Id", $"\n-ID({triggerAction.Id})");
        IAddLink(a, 0, "green", $"Trigger_{a}_Mood", $"\n-Фаза хода: {Mood[triggerAction.Mood]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetPalyer", $"\n-Проверяемый игрок: {TargetPalyer[triggerAction.TargetPalyer]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetTime", $"\n-Условие проверки: {TargetTime[triggerAction.TargetTime]}");
      //  IAddLink(a, 0, "green", $"Trigger_{a}_Only", $"\n-Одиночный режим работы {triggerAction.Only}");

        // triggerAction.MainText = "";
    }
    void TriggerPlusText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.PlusText = "\n\n-Условия";
        string text = "";
        int b = triggerAction.PlusAction.Count;
        for (int i = 0; i < b; i++)
        {
            triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusDel_{i}><color=green>-Удалить Условие</color></link>";
            text = "\n--Data";
            IAddLink(a, 1, "green", $"Trigger_{a}_Plus_{i}", text);
        }
        triggerAction.PlusText += $"\n\n<link=Trigger_{a}_PlusAdd><color=green>-Добавить Условие</color></link>";
    }
    void TriggerMinusText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.MinusText = "\n\n-Исключения";
        string text = "";
        int b = triggerAction.MinusAction.Count;
        for (int i = 0; i < b; i++)
        {
            text += "--Data";
            IAddLink(a, 1, "green", $"Trigger_{a}_PlusIf_{i}", text);
        }

        triggerAction.MinusText += $"\n<link=Add_{a}_PlusIf><color=green>-Добавить Исключение</color></link>";
    }
    void TriggerActionText(int a)
    {
        TriggerAction triggerAction = triggerActions[a];
        triggerAction.ActionText = "\n\n-Действия";
        string text = "";
        //int b = triggerAction.Action.Count;
        //for (int i = 0; i < b; i++)
        //{
        //    text += "--Data";
        //    IAddLink(a, 1, "green", $"Trigger_{a}_PlusIf_{i}", text);
        //}

        triggerAction.ActionText += $"\n<link=Add_{a}_PlusIf><color=green>-Добавить Действие</color></link>";
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
        //string text = $"\n\nТригеры";
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
            case ("Trigger"):
                int i = int.Parse(text);
                if (i != null)
                {
                    int b = 0;
                    TriggerAction triggerAction = triggerActions[i];
                    switch (com[2])
                    {
                        case ("Del"):
                            DelTrigger(i);
                            break;

                        case ("PlusDel"):
                            b = int.Parse(com[3]);
                            DelIf(i, true, b);
                            break;

                        case ("MinusDel"):
                            b = int.Parse(com[3]);
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
                            if (b == Mood.Count)
                                b = 0;
                            triggerAction.Mood = b;

                            TriggerMainText(i);
                            TriggerRootText(i);
                            LoadAllText();
                            break;

                        case ("TargetPalyer"):
                            b = triggerAction.TargetPalyer;
                            b++;
                            if (b == TargetPalyer.Count)
                                b = 0;
                            triggerAction.TargetPalyer = b;

                            TriggerMainText(i);
                            TriggerRootText(i);
                            LoadAllText();
                            break;

                        case ("TargetTime"):
                            Debug.Log("Ok");
                            stringMood = $"{i}";
                            Ui.SelectorsMain[0].active = true;
                            //b = triggerAction.Mood;
                            //b++;
                            //if (b > Mood.Count)
                            //    b == 0;
                            //triggerAction.Mood = b;

                            //TriggerMainText(i);
                            //TriggerRootText(i);
                            //LoadAllText();
                            break;
                            /*
                              $"Trigger_{a}_Id", $"\n-ID({triggerAction.Id})");
        IAddLink(a, 0, "green", $"Trigger_{a}_Mood", $"\n-Фаза хода: {Mood[triggerAction.Mood]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetPalyer", $"\n-Проверяемый игрок: {TargetPalyer[triggerAction.TargetPalyer]}");
        IAddLink(a, 0, "green", $"Trigger_{a}_TargetTime", $"\n-Условие проверки: {TargetTime[triggerAction.TargetTime]}");
                             
                             */
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
                int a = int.Parse(text);
                // TriggerAction triggerAction = triggerActions{ }
                //switch (text)
                //{ 
                
                //}
                break;
        }
    }

    void HideSelector()
    {
        int a = Ui.Selectors.Count;
        for(int i = 0; i < a; i++)
        {
            Ui.SelectorsMain[i].active = false;
        }
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
        LoadBase();

        LoadMainText();
       // AddPlusIf();
        //AddMinusIf();
        //AddActionIf();
        LoadAllText();

    }

    void CreateListButton(int b)
    {
        GameObject GO = null;
        int a = 0; switch (b)
        {
            case (0):
                a = TargetTime.Count;
                break;

            case (1):
                a = library.Legions.Count;
                break;

            case (2):
                a = library.CivilianGroups.Count;
                break;

            case (3):
                a = library.Constants.Count;
                break;

            case (4):
                a = library.Effects.Count;
                break;
        }
        // int a = TargetTime.Count;
        for (int i = 0; i < a; i++)
        {
            GO = Instantiate(Ui.ButtonOrig);
            GO.transform.SetParent(Ui.Selectors[b]);
            switch (b)
            {
                case (0):
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = TargetTime[i];
                    break;

                case (1):
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.Legions[i].Name;
                    break;

                case (2):
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.CivilianGroups[i].Name;
                    break;

                case (3):
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.Constants[i].Name;
                    break;

                case (4):
                    GO.transform.GetChild(0).gameObject.GetComponent<Text>().text = library.Effects[i].Name;
                    break;
            }
            ButtonSelector(b, i, GO.GetComponent<Button>());
        }
    }
    void LoadBase()
    {
        Ui.TextWindowButton.onClick.AddListener(() => LoadData());

        Name = "Благочестие";//Название

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
        //Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(свойства с кладбища)


        IfString = new List<string>();

        IfString.Add("Проверить текущее существо");//0-Creature
        IfString.Add("Проверить кол-во существ");//1-Creatures
        IfString.Add("Проверить Всех существ");//2-AllCreatures
        IfString.Add("Проверить голову");//3-Head
        IfString.Add("Проверить выбранное существо - для функции действие");//4-TargetCreature
        IfString.Add("Проверить эффекты стола");//5-Stol
        IfString.Add("Проверить использованную карту");//6-UseCard


        triggerActions = new List<TriggerAction>();



        GameObject GO = null;
        Ui.SelectorsMain = new List<GameObject>();
        Ui.Selectors = new List<Transform>();

        for (int i = 0; i < 5; i++)
        {
            GO = Instantiate(Ui.SelectorMain);
            GO.transform.SetParent(Ui.Canvas);
            GO.transform.position = Ui.SelectorMain.transform.position;
            Ui.SelectorsMain.Add(GO);
            Ui.Selectors.Add(GO.transform.GetChild(0).GetChild(0));
            CreateListButton(i);
        }

        /*
         * проверить текущеее существо
         * 
         проверить кол-во существ
        кого проверяем
        кол-во существ в на указанном столе больше меньше равно
            признак - (значение больше меньше равно)
                параметр 
        или
        ле
        группа или легион  
         
         */
    }
    void ButtonSelector(int b,int a, Button button)
    {
        switch (b)
        {
            case (0):
                button.onClick.AddListener(() => SwitchTargetTime(a));
                break;
            default:
                b--;
                button.onClick.AddListener(() => SwitchLibrary(a, b));
                break;
        }
    }
    void SwitchLibrary(int a, int b)
    {
        string text = "";
        switch (b) 
        {
            case (0):
                text = library.Legions[a].Name;
                break;

            case (1):
                text = library.CivilianGroups[a].Name;
                break;

            case (2):
                text = library.Constants[a].Name;
                break;

            case (3):
                text = library.Effects[a].Name;
                break;
        }

        string[] com = stringMood.Split('_');
        //0-trigger     1-if & else & action     2- IfAction num     3-string num

        int i = int.Parse(com[3]);
        TriggerAction triggerAction = triggerActions[int.Parse(com[0])];
        switch (int.Parse(com[1])) 
        {
            case (0):
                IfAction ifAction = triggerAction.PlusAction[int.Parse(com[2])];
                ifAction.TextData[i] = text;
                ifAction.IntData[i] = a;
                break;

            case (1):
                ifAction = triggerAction.MinusAction[int.Parse(com[2])];
                ifAction.TextData[i] = text;
                ifAction.IntData[i] = a;
                break;

            case (2):

                break;
        }

    }


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
    public int LocalId;
    public int Id;
    public int Mood;//All. Shot. Melee
    public int TargetPalyer;//All. My. Enemy
    public int TargetTime;//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(свойства с кладбища)

    public string RootText;
    public string MainText;
    public string PlusText;
    public string MinusText;
    public string ActionText;
    
    
    public List<IfAction> PlusAction;//события активаторы
    public List<IfAction> MinusAction;//события исключения
   // public List<Action> Action;
    public bool Only;//multy skill mood
}
/*Start Turn
 * 
 * 
 */
public class IfAction 
{
    public int Prioritet = 10;//приоритет действия
    public List<string> TextData;//текстовые поля
    public List<int> IntData;//числительные поля
    /*Проверяемое условие 
     Action.- нанесено н-ое кол-во урона, получено урона

    Start Turn - End Turn
    -- число карт (больше меньше равно)
    -- добавить признак для сортировки
    -- (или)выбранное значекние на карте (больше, меньше равно)
    -- -- проверямое число фиксированое или относительное относительно другого параметра
    -- (или)карта относится к опредленному типу или типам
    -- (или)карта имеет выбранное правило или правила
     */
}

