using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CivilianGroup", menuName = "ScriptableObjects/CivilianGroup", order = 1)]
public class CivilianGroup : ScriptableObject
{
    public List<Legion> Legions;
   // public string Legion;//������ � ������� �������

    public string Name;//��� ������ - ����������

    public List<Titul> Tituls;

    public List<StringCase> �onstant;
}
[System.Serializable]
public class Titul
{
    //����� - �������� ����� ��� ��������� �������� ���� ���������
    //���� ��������� ��� ��������� ������
    public string Name;
    public int Cost;

    public List<string> Effect;
}

[System.Serializable]
public class StringCase
{
    //�������� �������������� � ���� ������
    //����������� ����������� � ����, ���� (0.1f)
    public Constant Name;
    public float Cost =1;
}