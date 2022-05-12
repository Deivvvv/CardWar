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

    public bool Regen;//Это группа
    public bool Group;//Это группа

   // public string MoodEffect = "Orig";
    public List<ConstantSub> GuardConstant;//свойства с отрицанием
    public List<ConstantSub> AntiConstant;//свойства с отрицанием
    //public List<Constant> TwinConstant;//своиства исколючения, в одной карте они не могут быть установленны

  

}

[System.Serializable]
public class ConstantSub
{
    public Constant Stat;
    public float Size =1;
    public string MoodEffect = "Local";
}

