using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Linq;

using Coder;
using XMLSaver;
using SubSys;

namespace TableSys
{
    class ActionLine
    {
        public int Prioritet;
        RuleAction action;
        int card1, card2;
        List<int> cardInt;
        bool reCall;//этот параметр  перезапускает расчет силы действия
        List<int> power;//-1 как заглушка, обозначает потребность в расчетах?

        public ActionLine(bool _reCall,List<int> _cardInt, int _card1, int _card2, RuleAction actions, List<int> _power)
        {//true,allCardLocal,card1,card2, triggerAction.Action[i],0
            
            reCall = _reCall;
            action = actions;
            Prioritet = action.Prioritet;
            cardInt = _cardInt;
            card1 = _card1;
            card2 = _card2;
            power = _power;
        }
        public ActionLine Clone()
        {//true,allCardLocal,card1,card2, triggerAction.Action[i],0

            return new ActionLine(reCall,cardInt,card1,card2,action,power);
        }

        public bool Use()
        {//выполнить утверждение
            return IfSys.UseAction(action,cardInt,card1,card2,reCall,power);
        }
        public void NewTagert(int a)
        {
            card2 = a;
            //return Use();
        }
        public int GetCard(int a)
        {
            if (a == 0)
                return card1;
            else
                return card2;
        }
    }

    class TriggerLineCase
    {
        TriggerAction triggerAction;
        int card1, card2;
        public List<ActionLine> ActionTrigger;
        public TriggerLineCase(TriggerAction _triggerAction, int _card1, int _card2)
        {
            triggerAction = _triggerAction;
            card1 = _card1;
            card2 = _card2;
            ActionTrigger = new List<ActionLine>();
            //ActionTrigger.Add()
        }
        public TriggerLineCase(TriggerLineCase triggerLineCase)
        {
            triggerAction = triggerLineCase.GetTrigger();
            card1 = triggerLineCase.GetCard(0);
            card2 = triggerLineCase.GetCard(1);
            ActionTrigger = new List<ActionLine>();
            for (int i = 0; i < triggerLineCase.ActionTrigger.Count; i++)
                ActionTrigger.Add( triggerLineCase.ActionTrigger[i].Clone());
        }
        public TriggerAction GetTrigger()  {  return triggerAction; }
        public int GetCard(int a)
        {
            if (a == 0)
                return card1;
            else
                return card2;
        }
        public void Sort()
        {
            int a = 0;
            for (int i = 0; i < ActionTrigger.Count; i++)
            {
                a = i;
                for (int j = i; j < ActionTrigger.Count; j++)
                    if (ActionTrigger[a].Prioritet < ActionTrigger[j].Prioritet)
                        a = j;
                if(a != i)
                {
                    ActionLine line = ActionTrigger[i];
                    ActionTrigger[i] = ActionTrigger[a];
                    ActionTrigger[a] = line;
                }
            }
        }
        public bool Use()
        {
            if (ActionTrigger.Count == 0)
                IfSys.UseRule(ActionTrigger, triggerAction, card1, card2);

            for (int i = 0; i < ActionTrigger.Count; i++)
                if (!ActionTrigger[i].Use())
                    return false;

            return true;
        }
        public int Prioritet()
        {
            return triggerAction.Prioritet;
        }
    }
    
    class TriggerLine
    {
        public int Trigger;
        public List<TriggerLineCase> RuleTrigger;
        public TriggerLine( TriggerAction triggerAction,int card1,int card2)
        {
            Trigger = triggerAction.Trigger;
            RuleTrigger = new List<TriggerLineCase>();
            RuleTrigger.Add(new TriggerLineCase(triggerAction,  card1, card2));

        }
        public TriggerLine(TriggerLine triggerLine)
        {
            Trigger = triggerLine.Trigger;
            RuleTrigger = new List<TriggerLineCase>();
            for(int i=0;i< triggerLine.RuleTrigger.Count;i++)
                RuleTrigger.Add(new TriggerLineCase(triggerLine.RuleTrigger[i]));

        }
        public void Sort()
        {
            int a = 0;
            for (int i = 0; i < RuleTrigger.Count; i++)
            {
                a = i;
                for (int j = i; j < RuleTrigger.Count; j++)
                    if (RuleTrigger[a].Prioritet() < RuleTrigger[j].Prioritet())
                        a = j;
                if (a != i)
                {
                    TriggerLineCase line = RuleTrigger[i];
                    RuleTrigger[i] = RuleTrigger[a];
                    RuleTrigger[a] = line;
                }
            }
           // RuleTrigger.OrderBy(x => x.Prioritet);
        }
    }

    class Line
    {
       // public ActionLine StartActions;
        public List<TriggerLine> Actions;
        //public Line(ActionLine _Actions)
        //{
        //    StartActions = _Actions;
        //}
    }

    static class RootSys
    {
        static List<SubInt> triggers;
        static CoreSys core;
        //List<TriggerAction> ruleOrig;
        static List<HeadRule> rule;
        static List<string> ruleName;
        static List<CardCase> allCard;
        static List<int> colod1 ;
        static List<int> colod2 ;
        public static SubInt GetTriggers( int trigger , int cardId =-1)
        {
            if (cardId == -1)
                return triggers[trigger];

            return triggers[trigger].Num[triggers[trigger].Find(cardId, false)];
        }

        #region Rule
        static List<int> FindRuleIndex(int a)
        {
            List<int> nums = new List<int>();
            for (int j = 0; j < rule.Count; j++)
                if (a == rule[j].Tag)
                    nums.Add(j);
            return nums;
        }
        static int AddRule(HeadRule newRule)
        {
            int keyRule = core.frame.Action.FindIndex(x => x.Name == "Rule");
            int Id = rule.Count;
            rule.Add(newRule);
            int c = core.head[newRule.Tag].Index.FindIndex(x => x == newRule.Id);
            ruleName.Add(core.head[newRule.Tag].Rule[c]);

            for (int i = 0; i < newRule.Trigger.Count; i++)
            {
                TriggerAction action = newRule.Trigger[i];
                for(int j = 0; j < action.Action.Count; j++)
                {
                    if(action.Action[j].Action == keyRule)
                    {
                        RuleForm resultCore = action.Action[j].ResultCore;
                        int b;
                        int a = -resultCore.Tayp - 1;

                        List<int> nums = FindRuleIndex(a);
                        if (nums.Count > 0)
                        {
                            b = -1;
                            for (int k = 0; k < nums.Count; k++)
                                if (resultCore.TaypId == nums[k])// newRule[nums[k]].Id)
                                {
                                    b = k;
                                    resultCore.TaypId = nums[k];
                                    break;
                                }
                            if (b == -1)
                                resultCore.TaypId = AddRule(Saver.LoadRule(a, resultCore.TaypId));

                        }
                        else
                            resultCore.TaypId = AddRule(Saver.LoadRule(a, resultCore.TaypId));
                    }
                }
            }


            return Id;
        }
        static void ConvertRule(CardCase card)
        {
            int a, b ;
            for (int i = 0; i < card.Trait.Count; i++)
            {
                a = -card.Trait[i].Head - 1;
                List<int> nums = FindRuleIndex(a);
                if (nums.Count > 0)
                {
                    for (int j = 0; j < card.Trait[i].Num.Count; j++)
                    {
                        b = -1;
                        for (int k = 0; k < nums.Count; k++)
                            if (card.Trait[i].Num[j].Head == rule[nums[k]].Id)
                            {
                                b = k;
                                card.Trait[i].Num[j].Head = nums[k];
                                break;
                            }
                        if(b ==-1)
                            card.Trait[i].Num[j].Head = AddRule(Saver.LoadRule(a, card.Trait[i].Num[j].Head));
                    }
                }
                else
                    for (int j = 0; j < card.Trait[i].Num.Count; j++)
                        card.Trait[i].Num[j].Head = AddRule(Saver.LoadRule(a, card.Trait[i].Num[j].Head));
            }

        }
        #endregion
        static List<int> SortCard(List<CardCase> cardOrig, List<int> size)
        {
            List<int> nums = new List<int>();
            //int keyPosition = core.frame.Acton.FindIndex(x => x.Name == "SwitchPosition");
            for(int i = 1; i < cardOrig.Count; i++)
            {
                for (int j = 1; j < size[i]; j++)
                {
                    CardCase localCard = new CardCase(cardOrig[i]);
                    AddAllCard(localCard);
                    nums.Add(localCard.Id);
                }

                nums.Add(cardOrig[i].Id);
            }

            int a ;
            List<int> numsResult = new List<int>();
            while (nums.Count > 0)
            {
                a = Random.Range(0, nums.Count);
                numsResult.Add(a);
            }

            return numsResult;
        }
        static void AddAllCard(CardCase card)
        {
            card.Id = allCard.Count;
            allCard.Add(card);
        }
        static void StartData(string colod)
        {
            //Сброс ядра
            core = DeCoder.GetCore();
            ruleName = new List<string>();
            rule = new List<HeadRule>();
            allCard = new List<CardCase>();
            triggers = new List<SubInt>();
            for (int i = 0; i < core.frame.Trigger.Length; i++)
                triggers.Add(new SubInt(i));

                List<CardCase> allCard1 = new List<CardCase>();
            List<CardCase> allCard2 = new List<CardCase>();
            List<int> size1 = new List<int>();
            List<int> size2 = new List<int>();

            //запуск загрузки карт (!соло режим)
            string[] com = colod.Split('|');

            int[] com1 = com[0].Split('/').Select(int.Parse).ToArray();
            Saver.LoadColod(com1[0], com1[1], allCard1, size1);

            com1 = com[1].Split('/').Select(int.Parse).ToArray();
            Saver.LoadColod(com1[0], com1[1], allCard2, size2);

            for(int i=0;i<allCard2.Count;i++)
                allCard2[i].Team =1;

            AddAllCard(allCard1[0]);
            AddAllCard(allCard2[0]);

            for (int i = 1; i < allCard1.Count; i++)
                AddAllCard(allCard1[i]);
            for (int i = 1; i < allCard2.Count; i++)
                AddAllCard(allCard2[i]);


            for (int i = 0; i < allCard.Count; i++)
                ConvertRule(allCard[i]);

            colod1 = SortCard(allCard1, size1);
            colod2 = SortCard(allCard2, size2);

            //запуск предварительных процедур
            IfSys.Connect(allCard, rule);
            UiSys.Connect(allCard, rule, ruleName);
            //запуск игры
            Play();
        }
        public static void PositionCard(int team, int id, int position )
        {
            List<int> oldColod = (team ==0)? colod1: colod2;

            List<int> newColod = new List<int>();
            oldColod.Remove(id);
            if (position >= oldColod.Count)
            {
                newColod = oldColod;
                newColod.Add(id);
            }
            else
                for (int i = 0; i < oldColod.Count; i++) 
                {
                    if (i == position)
                        newColod.Add(id);
                    newColod.Add(oldColod[i]);
                }

            if (team == 0)
                colod1 = newColod;
            else
                colod2 = newColod;
        }
        static void ConnectCardRoot(CardCase card)
        {
            for(int i=0;i<card.Trait.Count;i++)
                for (int j = 0; j < card.Trait[i].Num.Count; j++)
                {
                    HeadRule localRule = rule[card.Trait[i].Num[j].Head];
                    for(int k =0;k< localRule.Trigger.Count; k++)
                    {
                        int a = triggers[localRule.Trigger[k].Trigger].Find(card.Id);
                        triggers[localRule.Trigger[k].Trigger].Num[a].Find(card.Trait[i].Num[j].Head);
                    }
                }
        }
        public static void ConnectCard(int id, int trigger, int num , bool add)
        {
            if (add)
                triggers[trigger].Num[id].Find(num);
            else
            {
                int a = triggers[trigger].Num[id].Find(num,false);
                if (a != -1)
                    triggers[trigger].Num[id].Num.RemoveAt(a);
            }
        }

        static void CallTrigger(int trigger)
        {
            for(int i = 0; i < triggers[trigger].Num.Count; i++)
                for (int j = 0; j < triggers[trigger].Num[i].Num.Count; j++)
                    IfSys.UseTrigger(triggers[trigger].Num[i].Head, -1, rule[triggers[trigger].Num[i].Num[j].Head].Trigger[ trigger]);
                
        }

        static void Play()
        {
            //запуск сесии
            CallTrigger(0);
        }
    }
    public class UiSys : MonoBehaviour
    {
        static int myTeam, enemyTeam =1, rememberPlan =1, rememberPlanEnemy =1, cardId=-1;

        static CoreSys core;
        static TriggerAction triggerActionCase;
        static List<TriggerAction> triggerActionList;
        static List<string> planName;
        static List<SubInt> sizePlan;
        static List<SubInt> sizePlanLocal;
        //static List<bool> planVisible;

        static List<CardCase> allCard;
        static List<CardCase> allCardSimulation;
        static List<HeadRule> rule;
       // static List<string> Name;
        static bool compliteFind =false;
        static bool compliteUse = false;
        static TableUi ui;
        public static void Connect(List<CardCase> _allCardGame, List<HeadRule> _rule, List<string> _Name) 
        { 
            allCard = _allCardGame;
            rule = _rule;
            core = DeCoder.GetCore();
         //   Name = _Name;
        }
        public static void SetUi(TableUi _ui) { ui = _ui; }

        public static void View(List<CardCase> _allCardSimulation)
        {
            allCardSimulation = _allCardSimulation;


            CountPlan();
            ViewCard(myTeam, rememberPlan, ui.PlanView[myTeam]);
            ViewCard(enemyTeam, rememberPlanEnemy, ui.PlanView[enemyTeam]);
            ViewStol(myTeam);
            ViewStol(enemyTeam);
        }
        static void LoadPlanButton()
        {

            void AddPlayerButton(int team)
            {
                void AddButton(Button button,int team, int i)
                {
                    button.onClick.AddListener(() => ViewPlan(team, i));
                }
                sizePlan.Add(new SubInt(team));
                for (int i = 0; i < planName.Count; i++)
                {
                    sizePlan[team].Num.Add(new SubInt(0));
                    GameObject go = Instantiate(ui.OrigButton);
                    go.transform.SetParent(ui.PlanButton[team]);

                    AddButton(go.GetComponent<Button>(),team,i);

                }
            }
            planName = new List<string>();
            //planVisible = new List<bool>();
            sizePlan = new List<SubInt>();
            for (int i = 0; i < core.bD[core.keyPlan].Base.Count; i++)
            {
                planName.Add(core.bD[core.keyPlan].Base[i].Name);
                //planVisible.Add(true);
            }
            AddPlayerButton(0);
            AddPlayerButton(1);
        }
        static void ViewPlanButton(int team, bool localMood = false)
        {
            SubInt sub = (localMood) ? sizePlanLocal[team] : sizePlan[team];
            for (int i = 0; i < planName.Count; i++)
                if(sub.Num[i].Head > 0)
                {
                    ui.PlanButton[team].GetChild(i).gameObject.active = true;
                    ui.PlanButton[team].GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = planName[i] + " " + sub.Num[i];
                }
                else
                    ui.PlanButton[team].GetChild(i).gameObject.active = false;
        }
        static void ViewPlan(int team,int a)
        {
            if (team == myTeam)
                rememberPlan = a;
            else
                rememberPlanEnemy = a;
            ViewCard(team, a, ui.PlanView[team]);
        }
        static void ViewStol(int team)
        {
            Transform trans = ui.Stol[team];
            ViewCard(team, 3,trans);//допустим 3 - стол
        }
        static void CountPlan()
        {
            if (allCardSimulation == null)
            {
                sizePlan[0].Num = new List<SubInt>();
                sizePlan[1].Num = new List<SubInt>();
                for (int i = 0; i < planName.Count; i++)
                {
                    sizePlan[0].Num.Add(new SubInt(0));
                    sizePlan[1].Num.Add(new SubInt(0));
                }

                for (int i = 0; i < allCard.Count; i++)
                    sizePlan[allCard[i].Team].Num[allCard[i].Plan].Num.Add(new SubInt(allCard[i].Id));
            }
            else
            {
                sizePlanLocal[0].Num = new List<SubInt>();
                sizePlanLocal[1].Num = new List<SubInt>();
                for (int i = 0; i < planName.Count; i++)
                {
                    sizePlanLocal[0].Num.Add(new SubInt(0));
                    sizePlanLocal[1].Num.Add(new SubInt(0));
                }

                for (int i = 0; i < allCardSimulation.Count; i++)
                    sizePlanLocal[allCardSimulation[i].Team].Num[allCardSimulation[i].Plan].Num.Add(new SubInt(allCardSimulation[i].Id));
            }
        }
        static void ViewCard(int team, int a , Transform trans)
        {
            List<CardCase> localCard = null;
            SubInt sub = null;
            if (allCardSimulation == null)
            {
                sub = sizePlan[team].Num[a];
                localCard = allCard;
            }
            else
            {
                sub = sizePlanLocal[team].Num[a];
                localCard = allCardSimulation;
            }

            for (int i = sub.Num.Count; i < trans.childCount; i++)
                Destroy(trans.GetChild(i).gameObject);

            void AddButton(Button button, int a)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => Use(a, false));
            }

            for (int i = trans.childCount; i < sub.Num.Count; i++)
            {
                GameObject go = Instantiate(ui.CardOrig);
                go.transform.SetParent(trans);
            }

            for (int i = 0; i < sub.Num.Count; i++)
            {
                GameObject go = trans.GetChild(i).gameObject;

                AddButton(go.GetComponent<Button>(), sub.Num[i].Head);
                Gallery.ReadCard(localCard[sub.Num[i].Head], go.GetComponent<CardBody>(), true);
                go.GetComponent<TableInfo>().Set(sub.Num[i].Head, null);
            }

        }


        public static void ClearActionButton()
        {
            triggerActionList = new List< TriggerAction>();
            for (int i = 0; i < ui.CardButton.childCount; i++)
                Destroy(ui.CardButton.GetChild(i).gameObject);
        }
        public static void AddActionButton(int cardId, TriggerAction triggerAction)
        {
            ui.CardButton.gameObject.active = true;
            GameObject go = Instantiate(ui.OrigButton);
            go.transform.SetParent(ui.CardButton);
            go.transform.GetChild(0).gameObject.GetComponent<Text>().text = triggerAction.Name;
            go.GetComponent<TableInfo>().Set( cardId, triggerAction);////=>FindTarget(cardId, ruleId,  triggerId)

            triggerActionList.Add(triggerAction); 
            go.GetComponent<Button>().onClick.AddListener(() => SelectTarget(triggerAction));//bool simulated
        }
        public static void SelectTarget(TriggerAction triggerAction)//bool simulated
        {
            //включить интерфейс выбора
            ui.CardButton.gameObject.active = false;
           // cardId = _cardId;
            triggerActionCase = triggerAction;
            Use(-1, true);
           // FindTarget();
        }
        public static void Use(int card, bool simulated)//вызывается через мышку
        {
            if (compliteUse)
                return;
            compliteUse = true;
            ui.CardButton.gameObject.active = false;
            if (cardId == -1)//(IfSys.line == null && triggerAction== null)
            {
                if ( allCard[card].ActionPoint >0)
                    IfSys.FindButton(card);

                compliteUse = false;
                if (triggerActionList.Count>0)
                {
                    cardId = card;
                    if (triggerActionList.Count == 0)
                        SelectTarget(triggerActionList[0]);
                }
                else 
                {
                    //cardId == -1
                    ui.CardButton.gameObject.active = false; 
                }
                //compliteUse = false;
                return;
            }

            if (!IfSys.Simulation(allCard, simulated, cardId, card, triggerActionCase))
                if (!simulated)
                    if(triggerActionCase != null)
                    {
                        allCard[cardId].ActionPoint--;
                        triggerActionCase = null;
                    }


            //IfSys.UseRule(cardId, card,rule[ruleId].Trigger[triggerId]);

            //cardId = -1;
            // allCard[cardId].ActionPoint--;
            compliteUse = false;
        }
        //static void FindTarget()
        //{
        //    if (cardId == -1 || ruleId == -1 || triggerId == -1)
        //        return;
        //    if (compliteFind)
        //        return;
        //    compliteFind = true;
        //    sizePlanSort = new List<SubInt>();
        //    for (int i = 0; i < sizePlan.Count; i++) 
        //    {
        //        sizePlanSort.Add(new SubInt(0));
        //        for (int j = 1; j < sizePlan[i].Num.Count; j++)
        //            for (int k = 0; k < sizePlan[i].Num[j].Num.Count; k++)
        //                if (IfSys.UseRule(cardId, sizePlan[i].Num[j].Num[k].Head, rule[ruleId].Trigger[triggerId], false))
        //                {
        //                    int a =sizePlanSort[i].Find(sizePlan[i].Num[j].Head);
        //                    sizePlanSort[i].Num[a].Find(sizePlan[i].Num[j].Num[k].Head);
        //                } 
        //    }
        //    //StartLine() == таким образом запускаем последовательность действий? 
        //    compliteFind = false;
        //}
       
    }
    static class IfSys
    {
        static bool simulation; 
        static List<CardCase> allCardGame;

        static Line line; //линия события. обозначает 
        //static int num;Оно не нужно, меняем на последнйи номер линии
        static CoreSys core;
        static List<HeadRule> rule;

        public static void Connect(List<CardCase> _allCardGame, List<HeadRule> _rule)
        {
            allCardGame = _allCardGame;
            rule = _rule;
            core = DeCoder.GetCore();
            line = new Line();
        }
        public static bool Simulation(List<CardCase> allCard, bool simulated, int card1, int card2, TriggerAction triggerAction)
        {
            Line fantomLine = null;
            simulation = simulated;
            if (simulation)
            {
                allCardGame = new List<CardCase>();
                for (int i = 0; i < allCard.Count; i++)
                    allCardGame.Add(new CardCase(allCard[i]));

                fantomLine = line;
                line = new Line();
                for(int i = 0; i < fantomLine.Actions.Count; i++)
                    line.Actions.Add(new TriggerLine(fantomLine.Actions[i]));
                
            }
            else
                allCardGame = allCard;

            if(triggerAction != null)
                UseTrigger(card1, card2, triggerAction);
            else
            {
                line.Actions[line.Actions.Count - 1].RuleTrigger[0].ActionTrigger[0].NewTagert(card2);
            }
            while (true)
            {
                if (line.Actions.Count == 0)
                    break;

                if (!UseLine())
                    break;
            }



            if (simulation)
                UiSys.View(allCardGame);
            else
                UiSys.View(null);

            if (simulated)
            {
                line = fantomLine;
                return false;
            }

            if(line.Actions.Count == 0)
                return true;
            return false;
        }
        public static void FindButton(int cardId)
        {
            UiSys.ClearActionButton();
            //SetBuffer(cardId);
            CardCase card = allCardGame[cardId];
            SubInt triggers = RootSys.GetTriggers(1, cardId);
            for (int i = 0; i < triggers.Num.Count; i++)
                for (int j = 0; j < rule[i].Trigger.Count; j++)
                    if (rule[i].Trigger[j].Trigger == 1 && card.Plan == rule[i].Trigger[j].Plan)
                        UiSys.AddActionButton(cardId, rule[i].Trigger[j]);
            // UseTrigger(cardId, -1, triggers.Num[i].Head, 1);

            // for (int i = 0; i < line.Actions.Count; i++)
            //     for (int j = 0; j < line.Actions[i].RuleTrigger.Count; j++)

        }
        static int ReturnTriggerId(string str)
        {
            for (int i = 0; i < core.frame.Trigger.Length; i++)
                if (core.frame.Trigger[i] == str)
                    return i;
            return -1;
        }
        static void ConnectTrigger(int card1, int card2, int trigger)
        {
            void ConnectTriggerSub(SubInt sub, int card2, int trigger)
            {
                for (int i = 0; i < sub.Num.Count; i++)
                    UseTrigger(sub.Head, card2, rule[sub.Num[i].Head].Trigger[ trigger]);
            }
            SubInt triggers = RootSys.GetTriggers(trigger, card1);

            if (card1 == -1)
                for (int i = 0; i < triggers.Num.Count; i++)
                    ConnectTriggerSub(triggers.Num[i], card2, trigger);
            else
                ConnectTriggerSub(triggers, card2, trigger);
        }
        public static void UseTrigger(int card1, int card2, TriggerAction triggerAction)
        {
            int a;
            string str = core.frame.Trigger[triggerAction.Trigger];
            if (allCardGame[card1].Plan == triggerAction.Plan)
            {
                switch (str)
                {
                    case ("Attack"):
                        a = ReturnTriggerId("AfterAttack");
                        ConnectTrigger(card1, card2, a);
                        ConnectTrigger(card2, card1, a);
                        a = ReturnTriggerId("ProtAttack");
                        ConnectTrigger(card2, card1, a);
                        break;
                    case ("Action"):
                        a = ReturnTriggerId("AfterAction");
                        ConnectTrigger(-1, card1, a);
                        break;
                    case ("Preparation"):
                        a = ReturnTriggerId("AfterPreparation");
                        ConnectTrigger(-1, card1, a);
                        break;
                }
                LineAddTrigger(triggerAction, card1, card2);
                //   UseRule(rule[ruleId].Trigger[i], true, card1,card2);
                switch (str)
                {
                    case ("Attack"):
                        a = ReturnTriggerId("BeforeAttack");
                        ConnectTrigger(card1, card2, a);
                        ConnectTrigger(card2, card1, a);
                        a = ReturnTriggerId("WedgeAttack");
                        ConnectTrigger(card1, card2, a);
                        ConnectTrigger(card2, card1, a);
                        break;
                    case ("Action"):
                        a = ReturnTriggerId("BeforeAction");
                        ConnectTrigger(-1, card1, a);
                        break;
                    case ("Preparation"):
                        a = ReturnTriggerId("BeforePreparation");
                        ConnectTrigger(-1, card1, a);
                        break;
                }

            }
        }

        public static bool UseRule(List<ActionLine> actionTrigger,TriggerAction triggerAction, int card1, int card2)
        {
            int CountSize(List<IfAction> actions, bool sumMod, List<int> allCardLocal, int card1, int card2)
            {
                int size = 0;
                for (int i = 0; i < actions.Count; i++)
                {
                    size += UseIfAction(actions[i], allCardLocal, card1,  card2);

                    if (size != 0 && !sumMod)
                        break;
                }
                return size;
            }
            bool complite;
            List<int> allCardLocal = new List<int>();

            int p = 0;
            if (triggerAction.MinusAction.Count > 0)
            {
                int m = CountSize(triggerAction.MinusAction,triggerAction.CountMod,allCardLocal,card1,card2);

                complite = (triggerAction.PlusAction.Count == 0 && m == 0);
                if (!complite)
                {
                    p = CountSize(triggerAction.PlusAction, triggerAction.CountMod, allCardLocal, card1, card2);
                    if (triggerAction.CountModExtend)
                    {
                        p -= m;
                        complite = true;
                    }
                    else
                        complite = (p > m);
                }
            }
            else if (triggerAction.PlusAction.Count > 0)
            {
                p = CountSize(triggerAction.PlusAction, triggerAction.CountMod, allCardLocal, card1, card2);
                complite = (p > 0);
            }
            else
                complite = true;


            if (complite)
            {
                for (int i = 0; i < triggerAction.Action.Count; i++)
                {
                    RuleAction actions = triggerAction.Action[i];
                    if (actions.Min <= p && actions.Max >= p)
                    {
                        //if (core.frame.Action[actions.Action].Dop[actions.ActionDop] == "NeedTarget")
                        //{
                        //    actionTrigger.Add(new ActionLine(true, allCardLocal, card1, -1, actions, power));
                        //}
                        //else
                            actionTrigger.Add(new ActionLine(true, allCardLocal, card1, card2, actions, new List<int>()));
                    }
                }
                       // ConnectAction(actionTrigger,triggerAction.Action[i]);
            }
            return complite;
        }
        static bool UseLine(int a =-1)
        {
            if(a == -1)
                a = line.Actions.Count - 1;
            if (a == -1)
                return true;

            TriggerLine tLine = line.Actions[a];
            //tLine.Sort();
            for (int i = 0; i < tLine.RuleTrigger.Count; i++)
            {
                if (tLine.RuleTrigger[0].Use())
                {
                    tLine.RuleTrigger.RemoveAt(0);
                    i--;
                }
                else if (simulation)//пропуск действия т.к. симуляция без участия игрока
                {
                    tLine.RuleTrigger.RemoveAt(0);
                    i--;
                }
                else
                    return false;
            }
            line.Actions.RemoveAt(a);
            return true;
        }

        static void LineAddTrigger(TriggerAction triggerAction, int card1, int card2)
        {

            if(line.Actions.Count == 0)
            {
                line.Actions.Add(new TriggerLine( triggerAction, card1, card2));

                return;
            }

            
            int a = line.Actions.Count - 1;

            if (line.Actions[a].Trigger != triggerAction.Trigger)
            {
                line.Actions.Add(new TriggerLine( triggerAction, card1, card2));
                line.Actions[a].Sort();
               // if (!UseLine(a))

                //    if (!UseLine(a))
                //    return;
            }
            else
                line.Actions[a].RuleTrigger.Add(new TriggerLineCase(triggerAction, card1,card2));
        }

        public static bool UseAction(RuleAction actions,List<int>cardInt, int card1, int card2, bool reCall,List<int> power)
        {

            CardCase card = null;
            if (reCall)
            {
                List<RuleForm> formList = new List<RuleForm>();
                for (int i = 0; i < actions.Core.Count; i++)
                    formList.Add(actions.Core[i]);
                power = new List<int>();
                for (int i = 0; i < formList.Count; i++)
                {
                    string str1 = core.frame.CardString[formList[i].Card];
                    int a = 0;
                    float b = 0, c = 0;
                    card = ReturnCard(str1, card1, card2);
                    if (card != null)
                    {
                        b = IfResult(card, formList[i], cardInt);
                    }
                    else if (str1 == "TakeCard")
                    {
                        for (int j = 0; j < cardInt.Count; j++)
                            if (IfResult(allCardGame[cardInt[j]], formList[i], cardInt) !=-1)
                                b++;
                    }
                    else
                        return false;

                    if (i + 1 != formList.Count)
                    {
                        card = ReturnCard(core.frame.CardString[formList[i + 1].Card], card1, card2);
                        if (card != null)
                        {
                            c = IfResult(card, formList[i + 1], cardInt);
                        }
                        else if (str1 == "TakeCard")
                        {
                            for (int j = 0; j < cardInt.Count; j++)
                                if (IfResult(allCardGame[cardInt[j]], formList[i + 1], cardInt) != -1)
                                    c++;
                        }
                        else
                            return false;
                    }

                    if (c != 0)
                        a = Mathf.FloorToInt(b / c);
                    else
                        a = Mathf.FloorToInt(b);
                    power.Add(a);
                }

            }


            void UseAct(CardCase card ,RuleForm ruleForm, string str, string strEx, List<int> power, int card1, int card2)
            {
                //int finalPower;
                //if(ruleForm.Tayp == core.KeyStat)
                    //finalPower =

                switch (core.frame.Action[actions.Action].Name)
                {
                    case ("Attack"):
                        ConnectTrigger(card1, card2, ReturnTriggerId("Attack"));
                        float f = power[0] / power[1];
                        f *= ruleForm.Mod;
                        f += ruleForm.Num;
                        AttackStat(allCardGame[card1], allCardGame[card2], ruleForm.TaypId, core.frame.ForseTayp[ruleForm.Forse], f);
                        //SubInt triggers = RootSys.GetTriggers(1, card1);
                       // UseTrigger(card1,card2,)
                        break;
                    case ("Rule"):
                        switch (strEx)
                        {
                            case ("Use"):
                                //2
                                ConnectTrigger(card1, card2, 2);
                                //int a = ReturnTriggerId("Attack");
                                break;
                            case ("Add"):
                                SubInt sub = null;
                                //CardCase card = ReturnCard( allCardGame[card2];
                                for(int i=0;i< card.Trait.Count; i++)
                                    if (card.Trait[i].Head == ruleForm.Tayp)
                                    {
                                        sub = card.Trait[i];
                                        break;
                                    }
                                if(sub == null)
                                {
                                    sub = new SubInt(ruleForm.Tayp);
                                    card.Trait.Add(sub);
                                }

                                break;
                            case ("Remove"):
                                //sub = null;
                                //CardCase card = ReturnCard( allCardGame[card2];
                                for (int i = 0; i < card.Trait.Count; i++)
                                    if (card.Trait[i].Head == ruleForm.Tayp)
                                    {
                                        sub = card.Trait[i]; 
                                        int a = sub.Find(ruleForm.TaypId, false);
                                        if (a != -1)
                                        {
                                            //sub.Find(ruleForm.TaypId);
                                            for (int j = 0; j < rule[ruleForm.TaypId].Trigger.Count; j++)
                                                RootSys.ConnectCard(card.Id, rule[ruleForm.TaypId].Trigger[j].Trigger, j, false);
                                            sub.Num.RemoveAt(a);
                                            if (sub.Num.Count == 0)
                                                card.Trait.RemoveAt(i);
                                        }
                                    }
                                return;
                                break;
                        }
                        break;
                    case ("Status"):
                        switch (strEx)
                        {
                            case ("Add"):
                                for (int i = 0; i < card.Status.Count; i++)
                                    if (card.Status[i] == ruleForm.TaypId)
                                        return;
                                card.Status.Add(ruleForm.TaypId);
                                break;
                            case ("Remove"):
                                card.Status.Remove(ruleForm.TaypId);
                                break;
                            case ("Replace"):
                                card.Status.Remove(power[0]);
                                for (int i = 0; i < card.Status.Count; i++)
                                    if (card.Status[i] == ruleForm.TaypId)
                                        return;
                                card.Status.Add(power[1]);
                                break;
                        }
                        break;
                    case ("Transf"):
                        card.Plan = ruleForm.TaypId;
                        ConnectTrigger(card1, card2, ReturnTriggerId("MovePlane"));
                        break;

                    case ("SwitchPosition"):
                        RootSys.PositionCard(card.Team, card.Id, ruleForm.Num);
                        break;
                    case ("Stat"):
                        f = power[0] / power[1];
                        f *= ruleForm.Mod;
                        f += ruleForm.Num;
                        string strForse = core.frame.ForseTayp[ruleForm.Forse];
                        switch (strEx)
                        {
                            case ("Add"):
                                StatEdit(false,card1,card2, ruleForm.TaypId, f, strForse);
                                break;
                            case ("Set"):
                                StatEdit(true, card1, card2, ruleForm.TaypId, f, strForse);
                                break;
                            case ("Clear"):
                                StatEdit(true, card1, card2, ruleForm.TaypId, 0, strForse);
                                break;
                            case ("MainStat"):
                                StatEdit(false, card2, -1, allCardGame[card1].Stat[0].GetStat(), f, strForse);
                                break;
                            case ("MainStatSet"):
                                StatEdit(true, card2, -1, allCardGame[card1].Stat[0].GetStat(), f, strForse);
                                break;
                            case ("Replace"):
                                int a = allCardGame[card1].Stat.FindIndex(x => x.GetStat() == ruleForm.TaypId);
                                if (a != -1)
                                    allCardGame[card1].Stat[a].Swap(ruleForm.Num, core.bD[core.keyStat]);
                                break;
                            case ("ReplaceMainStat"):
                                allCardGame[card1].Stat[0].Swap(ruleForm.Num, core.bD[core.keyStat]);
                                break;
                                //"Add_Clear_MainStat_Replace"
                        }
                        break;
                    case ("Equip"):
                        card.Plan = card2;
                        ConnectTrigger(card1, card2, ReturnTriggerId("SelfEquip"));
                        break;
                }
            } 
            string str = core.frame.Action[actions.Action].Name;
            string strEx = core.frame.Action[actions.Action].Extend[actions.ActionExtend];
            
            card = ReturnCard(core.frame.CardString[actions.ResultCore.Card], card1, card2);
            if (card != null)
            {
                UseAct(card, actions.ResultCore, str, strEx, power, card1,card2);
            }
            else if (str == "TakeCard")
            {
                for (int j = 0; j < cardInt.Count; j++)
                    UseAct(allCardGame[cardInt[j]], actions.ResultCore, str, strEx, power, cardInt[j], card2);
            }
            else
                return false;

            return true;
        }
        static void AttackStat(CardCase card1, CardCase card2, int stat, string forse, float size)
        {
            void AttackStatUse(CardCase card1, CardCase card2, int stat, string forse, float size)
            {
                BD bd = core.bD[core.keyStat];
                for (int i = 0; i < bd.Base[stat].Sub.AntiStat.Count; i++)
                {
                    int a = card1.Stat.FindIndex(x => x.GetStat() == bd.Base[stat].Sub.AntiStat[i]);
                    if (a != -1)
                    {
                        float f = 0;
                        if (size != 0)
                            f = size;
                        else
                            f = card1.Stat[a].Get(forse);

                        if (f > 0)
                            for (int j = 0; j < bd.Base[stat].Sub.DefStat.Count; j++)
                            {
                                int b = card2.Stat.FindIndex(x => x.GetStat() == bd.Base[stat].Sub.DefStat[j]);
                                if (b != -1)
                                    f -= card2.Stat[b].Get("Local");
                            }

                        if (f > 0)
                            StatEdit(false,card1.Id,-1, bd.Base[stat].Sub.AntiStat[i], f, forse);
                    }
                }
            }

            int root = -1;
            //, CardCase card, List<int> stat, List<int> size, int power,, bool defMood
            if (stat < 0)
                root = -stat - 1;
            if (root != -1)
            {
                MainBase mainBase = core.bD[core.keyStatGroup].Base[root];
                root = mainBase.Group.MainSize;
                for (int i = 0; i < mainBase.Group.Stat.Count; i++)
                {
                    int a = card1.Stat.FindIndex(x => x.GetStat() == mainBase.Group.Stat[i]);
                    if (a != -1)
                    {
                        string strForse = core.frame.ForseTayp[mainBase.Group.Forse[i]];
                        float f = card1.Stat[a].Get(strForse) * mainBase.Group.Size[i] / root;

                        AttackStatUse(card1, card2, mainBase.Group.Stat[i], forse, f);

                    }

                }

            }
            else
                AttackStatUse(card1, card2, stat, forse, size);



        }

        static void StatEdit(bool set,int card1, int card2, int stat, float size, string forse)
        { //int a = -1;
          //for(int i =0;i<card.Stat.Count;i++)
          //    if(card.Stat[i].GetStat() == stat)
          //    {
          //        a = i;
          //        break;
          //    }
            void StatEditUse(bool set, CardCase card, int stat, int size, string forse)
            {
                BD bdStat = core.bD[core.keyStat];
                MainBase mainBase = bdStat.Base[stat];
                StatExtend se = null;
                int a = card.Stat.FindIndex(x => x.GetStat() == stat);
                if (a != -1)
                    se = card.Stat[a];
                else
                {
                    if (mainBase.Sub.Antipod != -1)
                    {
                        a = card.Stat.FindIndex(x => x.GetStat() == mainBase.Sub.Antipod);
                        if (a != -1)
                        {
                            se = card.Stat[a];
                            size *= -1;
                        }

                    }

                    if (a == -1)
                        se = new StatExtend(stat, bdStat);
                }

                if (size < 0)
                    if (mainBase.Sub.Antipod != -1)
                    {
                        if (mainBase.Sub.Antipod == se.GetStat())
                            se.Swap(stat, bdStat);
                        else
                            se.Swap(mainBase.Sub.Antipod, bdStat);

                        size *= -1;
                    }


                if (set)
                    se.Set(forse, size);
                else
                    se.Edit(forse, size);

                a = se.Get("Local");
                if (a < 0)
                    if (mainBase.Sub.Antipod != -1)
                    {
                        if (mainBase.Sub.Antipod == se.GetStat())
                            se.Swap(stat, bdStat);
                        else
                            se.Swap(mainBase.Sub.Antipod, bdStat);
                    }
            }

            CardCase cards = allCardGame[card1];
            int root = -1;
            if (stat < 0)
                root = -stat - 1;
            if (root != -1)
            {
                MainBase mainBase = core.bD[core.keyStatGroup].Base[root];
                root = mainBase.Group.MainSize;

                CardCase cards1 = allCardGame[card2];
                for (int i = 0; i < mainBase.Group.Stat.Count; i++)
                {
                    int a = cards1.Stat.FindIndex(x => x.GetStat() == mainBase.Group.Stat[i]);
                    if (a != -1)
                    {
                        string strForse = core.frame.ForseTayp[mainBase.Group.Forse[i]];
                        float f = cards1.Stat[a].Get(strForse) * mainBase.Group.Size[i] / root;

                        StatEditUse(set, cards, stat, Mathf.FloorToInt(f), forse);

                    }

                }

            }
            else
                StatEditUse(set, cards, stat, Mathf.FloorToInt(size), forse);


        }


        static bool ReturnBool(CardCase card, int tayp, int taypId)
        {
            int GetRace(int id, int targetId)
            {
                if (id != targetId)
                {
                    MainBase mainBase = core.bD[core.keyRace].Base[id];
                    if (mainBase.Race.MainRace != -1)
                        return GetRace(mainBase.Race.MainRace, targetId);
                }

                return id;
            }

            int a = 0;
            if (tayp < 0)
            {
                SubInt sub = new SubInt(0);
                a = sub.Find(tayp, false);
                if (a != -1)
                    a = sub.Num[a].Find(taypId,false); 
            }
            else
                switch (core.frame.Tayp[tayp])
                {
                    case ("Guild"):
                        a = card.Guild;
                        break;
                    case ("Legion"):
                        a = card.Legion;
                        break;
                    case ("Plan"):
                        a = card.Plan;
                        break;
                    case ("Civilian"):
                        a = card.Civilian;
                        break;
                    case ("Status"):
                        for (int i = 0; i < card.Status.Count; i++)
                            if (card.Status[i] == taypId)
                            {
                                a = card.Status[i];
                                break;
                            }

                        break;
                    case ("CardTayp"):
                        a = card.CardTayp;
                        break;
                    case ("CardClass"):
                        a = card.CardClass;
                        break;
                    case ("Race"):
                        a = card.Race;
                        if (a != taypId)
                        {
                            MainBase mainBase = core.bD[core.keyRace].Base[a];
                            if (mainBase.Race.MainRace != -1)
                                a = GetRace(mainBase.Race.MainRace, taypId);
                        }
                        break;
                        //"Status", "CardTayp", "CardClass", "Race"
                }


            bool result = a == taypId;
            //if (revers)
            //    return !result;
            return result;
        }
        static int ReturnInt(CardCase card ,RuleForm form, List<int> cardInt)
        {
            if (form.Tayp == core.keyStat)
            {
                if (form.TaypId < 0)
                    switch(core.frame.SysStat[-form.TaypId - 1])
                    {
                        case ("Skip"):
                            return form.Num;
                            break;
                        case ("MainStat"):
                            return card.Stat[0].GetStat();
                            break;
                        case ("Mana"):
                            return allCardGame[card.Team].ActionPoint;
                            break;
                        case ("EnemyMana"):
                            if(card.Team ==0)
                                return allCardGame[1].ActionPoint;
                            else
                                return allCardGame[0].ActionPoint;
                            break;
                        case ("ManaMax"):
                            return allCardGame[card.Team].ActionPointMax;
                            break;
                        case ("EnemyManaMax"):
                            if (card.Team == 0)
                                return allCardGame[1].ActionPointMax;
                            else
                                return allCardGame[0].ActionPointMax;
                            break;
                        case ("Speed"):
                            return card.Speed;
                            break;
                        case ("ManaCard"):
                            return card.Mana;
                            break;
                        //case ("AllCount"):
                        //    return cardInt.Count;
                        //    break;
                        case ("PlanSize"):
                            return core.bD[core.keyPlan].Base[form.Mod].Plan.Size;
                            break;
                        case ("Action"):
                            return card.ActionPoint;
                            break;
                        case ("ActionMax"):
                            return card.ActionPointMax;
                            break;
                        case ("LocalCount"):
                            return cardInt.Count;
                            break;
                    }




                for (int i = 0; i < card.Stat.Count; i++)
                    if (form.TaypId == card.Stat[i].GetStat())
                        return card.Stat[i].Get(core.frame.ForseTayp[form.Forse]);

                MainBase mainBase = core.bD[core.keyStat].Base[form.TaypId];
                if(mainBase.Sub.Antipod != -1)
                {
                    for (int i = 0; i < card.Stat.Count; i++)
                        if (mainBase.Sub.Antipod == card.Stat[i].GetStat())
                            return card.Stat[i].Get(core.frame.ForseTayp[form.Forse]);
                }
                return 0;
            }
            else if (form.Tayp == core.keyStatGroup)
            {
                float a = 0;
                MainBase mainBase = core.bD[core.keyStatGroup].Base[form.TaypId];
                for(int i = 0; i < mainBase.Group.Stat.Count; i++)
                    for(int j=0;j<card.Stat.Count;i++)
                        if(mainBase.Group.Stat[i] == card.Stat[j].GetStat())
                        {
                            a += card.Stat[j].Get(core.frame.ForseTayp[mainBase.Group.Forse[i]]) * mainBase.Group.Size[i];
                            break;
                        }
                a /= mainBase.Group.MainSize;
                return Mathf.FloorToInt(a);
            }


            return -1;
        }
        static bool EqualBool(int a , int b , int mood)
        {
            switch (mood)
            {
                case (0)://==
                    return (a == b);
                    break;
                case (1)://!=
                    return (a != b);
                    break;
                case (2)://<=
                    return (a <= b);
                    break;
                case (3)://>=
                    return (a >= b);
                    break;
                case (4)://<
                    return (a < b);
                    break;
                case (5)://>
                    return (a > b);
                    break;
            }
            return false;
        }

        static List<CardCase> ReturnCard(string str, List<int> cardList, RuleForm form1, RuleForm form2, int moodResult,CardCase card2)
        {
            //if (cardList == null)
            //    cardList = allCard;
            switch (str)
            {
                case ("NewCollectCard"):
                    for (int i = 0; i < cardList.Count; i++)
                    {
                        cardList.RemoveAt(0);
                        i--;
                    }
                    return ReturnCard("CollectCard", cardList, form1,form2,moodResult,card2);
                    break;
                case ("CollectCard"):
                    for (int i = 0; i < allCardGame.Count; i++)
                    {
                        if(IfResultFull(allCardGame[i],form1,card2,form2,moodResult,cardList))
                        {
                            int c = cardList.FindIndex(x => x == allCardGame[i].Id);
                            if (c == -1)
                                cardList.Add(allCardGame[i].Id);
                        }
                    }
                    break;
                case ("RemoveCard"):
                    for (int i = 0; i < cardList.Count; i++)
                    {
                        if (IfResultFull(allCardGame[cardList[i]], form1,card2, form2, moodResult, cardList))
                        {
                            int c = cardList.FindIndex(x => x == cardList[i]);
                            if (c != -1)
                            {
                                cardList.RemoveAt(c);
                                i--;
                            }

                        }
                    }
                    break;
                //case ("GetLine"):
                //    for (int i = 0; i < line.Actions.Count; i++)
                //    {
                //        int c = cardList.FindIndex(x => x == line.Actions[i].MyCard.Id);
                //        if (c == -1)
                //            cardList.Add(line.Actions[i].MyCard.Id);

                //        c = cardList.FindIndex(x => x == line.Actions[i].TargetCard.Id);
                //        if (c == -1)
                //            cardList.Add(line.Actions[i].TargetCard.Id);
                //    }
                //    break;
            }

            List<CardCase> cardCases = new List<CardCase>();
            for (int i = 0; i < cardList.Count; i++)
                cardCases.Add(allCardGame[cardList[i]]);
            return cardCases;
        }
        static CardCase ReturnCard(string str, int card1, int card2)
        {
            CardCase card = null;
            //ActionLine action =null;

            string[] com = str.Split('|');
            //if (com[0] == "First")
            //{
            //    action = line.StartActions;
            //    str = com[1];
            //}
            //else
            //    action = line.Actions[line.Actions.Count - 1].RuleTrigger[0].ActionTrigger[0];


            switch (com[0])
            {
                case ("MyCard"):
                    card = allCardGame[card1];//allCardGame[action.GetCard(0)];
                    break;
                case ("TargetCard"):
                    card = allCardGame[card2];//allCardGame[action.GetCard(1)];
                    break;
            }
            if(com.Length>1) 
                switch (com[1])
                {
                    case ("Main"):
                        card = allCardGame[card.Team];
                        break;
                    case ("MainEnemy"):
                        if (card.Team == 0)
                            card = allCardGame[1];
                        else
                            card = allCardGame[0];
                        break;
                }

            return card;
        }

        static bool IfResultFull(CardCase card1, RuleForm form1, CardCase card2, RuleForm form2, int moodResult, List<int> cardInt)
        {
            int c = 0;
            int b = IfResult(card1, form1, cardInt);
            if (card2 != null)
            {
                //card = ReturnCard(core.frame.CardString[form2.Card]);
                c = IfResult(card2, form2, cardInt);
            }

            return EqualBool(b, c, moodResult);

        }
        static int IfResult(CardCase card, RuleForm form, List<int> cardInt)
        {
            if (form.Tayp == core.keyStat || form.Tayp == core.keyStatGroup)
            {
                return ReturnInt(card, form, cardInt);
            }
            else if (ReturnBool(card, form.Tayp, form.TaypId)) 
                    return 0;

            return -1;
        }
        static int UseIfAction(IfAction action, List<int> cardInt, int card1,int card2)
        {
            int h=0;
            //if(sunMod)
            for (int i = 0; i < action.Core.Count; i++)
            {
                RuleForm form1 = action.Core[i];
                RuleForm form2 =null;
                CardCase card = null;
                if (form1.Tayp == core.keyStat || form1.Tayp == core.keyStatGroup)
                {
                    form2 = action.ResultCore[h];
                    card = ReturnCard(core.frame.CardString[form2.Card], card1, card2);
                    h++;
                }

                string str = core.frame.CardString[form1.Card];
                string[] com = str.Split('|');
                if (com[0] == "NewCollectCard" || com[0] == "CollectCard" || com[0] == "RemoveCard" || com[0] == "GetLine")
                {
                    List<int> cards = (com.Length == 1) ? null : cardInt;
                    ReturnCard(com[0], cards, form1, form2, action.Result[i],card);
                    //continue;
                }
                // else if ("TakeCard")
                //card = (com.Length == 1) ? ReturnCard(com[0], null) : ReturnCard(com[0], cardInt);
                else
                {
                    if (IfResultFull(ReturnCard(str, card1, card2), form1,card,form2, action.Result[i], cardInt))
                        return 0;
                }
            }

            return action.Point;
        }


    }


}
