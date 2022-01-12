using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Constant", menuName = "ScriptableObjects/Constant", order = 1)]
public class Constant : ScriptableObject
{
    public string NameLocalization;
    public string Name;

    public Sprite Icon;
    public string IconName;
    public int Cost;

    public bool Group;
    public bool CombineConstant;

    public List<Constant> AntiConstant;//свойства с отрицанием
    public List<Constant> TwinConstant;//своиства исколючения, в одной карте они не могут быть установленны

    public List<Effect1> Effect;
  

}

[System.Serializable]
public class Effect1
{
    //public bool Use;
    public string Text;
    public int Id;
    public int EffectStrnth;
    public Effect2[] Form;
}

[System.Serializable]
public class Effect2
{
    public Effect Effects;
    public Constant Constants;
//    public string Action;
    public Constant TargetConstant;
    public int Int;
    public bool Plus;
}