using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleMainFrame", menuName = "ScriptableObjects/RuleMainFrame", order = 1)]
public class RuleMainFrame : ScriptableObject
{
    //public string[] BoolString;
    public string[] ForseTayp;
    public string[] EqualString;

    public string[] PlayerString;
    public string[] CardString;
    public string[] SysStat = { "Skip", "MainStat", "Mana", "Speed", "ManaCard", "EnemyMana" };
    //public string[] StatTayp;
    [HideInInspector] public List<string> Tayp;

    [Space(10)]
    [HideInInspector] public string[] Trigger;
    [HideInInspector] public List<SubAction> Action;

    [Space(10)]
    //public List<string> KeyWord;
    public List<string> KeyWordStatus;


    [Space(10)]
    // public List<string> ClassCard;
    // public List<string> CardTayp;

    [Space(10)]
    [SerializeField] private Color[] colors;
    [HideInInspector] public string[] ColorsStr;


    /*
     Triggers
    action - создает кнопку для вызова механики за очко действия

     
     comand
    transf plan - перемещает систему из одного плана в дургой
    StstAdd - увеличивает/уменьшает стат
    StatusAdd - Добаляет статус, если его нет
    StatusRemove - Убирает статус, если он есть
    attak - иницирует действие атаки


     */
    public void Convert()
    {
        {
            string[] com = { "Guild", "Legion", "Stat", "Tag", "Plan", "Civilian", "Status", "CardTayp", "CardClass", "Race", "StatGroup", "Mark" };
            Tayp = new List<string>(com);
        }

        {
            //создать кнопку действия , кто-то выполнил действие // перенос на другой план // кого-то перенесли на другой план
            string[] com = { "StartTurn", "WedgeAttack", "BeforeAttack", "Attack", "ProtAction", "AfterAttack", "BeforeAction", "Action", "AfterAction", "BeforePreparation", "Preparation", "AfterPreparation", "CardReaction", "MovePlane", "AttackReaction", "AnotherEquip", "SelfEquip", "BeforeCard", "TakingDamage", "Hurt" };
            Trigger = com;
        }

        /*
        "StartTurn", "WedgeAttack", "BeforeAttack", "Attack", ProtAction", "AfterAttack", "BeforeAction", "Action", "AfterAction", "BeforePreparation", "Preparation", "AfterPreparation", "CardReaction", "MovePlane", "AttackReaction" "AnotherEquip", "SelfEquip", "BeforeCard", "TakingDamage", "Hurt"
        1: эффект в начале хода 
        2: эффеки при смене хода
        3: в конце

         вклинивание в порядок атаки
        1: перед атакой
        атака 
        дейстыие защиты
         2: после атаки

        до дейстыия
        во время действия
        после дейстыия

        до подготовки
        во время подготовки
        после подготовки

        реакция (на разыгранную карту оппонента)

        эффект при перемещении между планами

        реакция (при перемещении между планами других карт

        реакция ( активируется после чужой атаки


         при экипировке кого либо

         при экипировке самого существа
       
         до разыгрывания карты протиыника

         до атаки карты противника

         при получении урона

         когда существо ранено

         когда сущестыо что-то сделало

         за каждую активацию другого триггера

         */

        Action = new List<SubAction>();
        Action.Add(new SubAction("Attack", "Meele_Shot"));
        Action.Add(new SubAction("Stat", "Add_Clear_MainStat_Replace"));//изсенить стат - удалить стат - заменить основной стат - заменить параметр
                                                                        // Action.Add(new SubAction("Karma", "Clear_Add_Remove", "PG_PS_PL_PT_NG_NS_NL_NT"));//karma_{global}_{all}_Stat   .. перобсудить этот раздел
                                                                        // Action.Add(new SubAction("Switch", "Guild_Ligion_Social_Race"));
                                                                        // Action.Add(new SubAction("Effect", "Add_Remove_Replace", "Eternal_NoEternal"));
        Action.Add(new SubAction("Rule", "Use_Add_Delite"));
        Action.Add(new SubAction("Status", "Add_Remove_Replace"));
        Action.Add(new SubAction("Transf", " "));
        Action.Add(new SubAction("Create", " "));
        /*
           karma
          global = общая статистика за весь матч
          turn
          local

          positiv
          negativ
          all

           */




        ColorsStr = new string[colors.Length];
        for (int i = 0; i < ColorsStr.Length; i++)
            ColorsStr[i] = ColorUtility.ToHtmlStringRGB(colors[i]);
    }
}

public class SubAction
{
    public SubAction(string name, string str, string ext =" ")
    {
        Name = name;
        Extend = str.Split('_');
        Dop = ext.Split('_');
    }
    public string Name;
    public string[] Extend;
    public string[] Dop;
}

