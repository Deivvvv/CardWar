using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFrame", menuName = "ScriptableObjects/RuleFrame", order = 1)]
public class RuleFrame : ScriptableObject
{
    //Служит для Создания формы под конкретный комплекс правил
    public string Name; //название формы правила
    public List<RuleFarmeMacroCase> Form;//Text Rule
    /*
     * RuleFarmeMacroCase - указывает на конструкцию оудного утверждения и выражает максимальный уровень условий
     * 
     * техт - подсказка в конструкторе
     *  Legion  bool - указывает что нужно установить соответсвие на утверждение
     (Rule=Legion) - указывает что нужно использовать легион для заполнения данными
     (Rule=bool) - требует указать, правда или лож
     (Rule=Int) - поле требует заполнение цыфрой
     (Rule=Equal)  - требует указать, больше, меньше или равно
     (Rule=Constant) - поле требует заполнение константой
     (Rule=Group) - поле требует заполнение социальной группой
     (Rule=GroupLevel) - поле требует заполнение социальной группой в указаном значении

     */
}
