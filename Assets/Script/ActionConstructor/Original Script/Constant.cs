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

    public List<Constant> AntiConstant;//свойства с отрицанием
    public List<Constant> TwinConstant;//своиства исколючения, в одной карте они не могут быть установленны
}