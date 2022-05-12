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

    public bool Regen;//��� ������
    public bool Group;//��� ������

   // public string MoodEffect = "Orig";
    public List<ConstantSub> GuardConstant;//�������� � ����������
    public List<ConstantSub> AntiConstant;//�������� � ����������
    //public List<Constant> TwinConstant;//�������� �����������, � ����� ����� ��� �� ����� ���� ������������

  

}

[System.Serializable]
public class ConstantSub
{
    public Constant Stat;
    public float Size =1;
    public string MoodEffect = "Local";
}

