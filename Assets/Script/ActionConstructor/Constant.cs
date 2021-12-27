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

    public string AntiConstant;//�������� � ����������
    public string TwinConstant;//�������� �����������, � ����� ����� ��� �� ����� ���� ������������
}

public class CivilianGroup 
{
    public string Name;//��� ������ - ����������

    public List<string> Titul;
    public List<int> TitulCost;

    public List<string> �onstant;
    public List<int> �onstantModifer;
}

/*
 Civilian Group
public string

 
 */