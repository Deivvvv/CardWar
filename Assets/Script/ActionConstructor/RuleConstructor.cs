using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public string Name;//Название
    public string Info;//Описание

    public int Cost;//Цена
    public int CostExtend;//цена за доп очки навыков

    public int LevelCap;//Максимальный уровень способности

    public int CostMovePoint;

    public bool Player;

    public List<MacroAction> macroActions;
    #endregion



    public void Start()
    {
        AddFlied(0, "Name");
        AddFlied(0, "Cost");
        AddFlied(0, "CostExtend");
        AddFlied(0, "LevelCap");
        AddFlied(0, "CostMovePoint");
        AddFlied(0, "Player");
    }


    void LoadBase()
    {

    }
    void AddFlied(int a, string text)
    {
        GameObject GO = null;
        switch (a)
        {
            case (0):
                GO = Instantiate(Ui.LableOrig);
                GO.transform.SetParent(Ui.Lable);

                GO.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = text;
                //switch (text)
                //{ 
                //    case (""):
                //        break;
                //}

                break;
        }
    }

}
public class MacroAction 
{
    public string Mood;//All. Shot. Melee
    public string TargetPalyer;//All. My. Enemy
    public string TargetTime;//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(свойства с кладбища)
    public List<IfAction> PlusAction;//события активаторы
    public List<IfAction> MinusAction;//события исключения
    //public List<Action> Action;
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

