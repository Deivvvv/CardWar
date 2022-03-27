using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFrame", menuName = "ScriptableObjects/RuleFrame", order = 1)]
public class RuleFrame : ScriptableObject
{
    //������ ��� �������� ����� ��� ���������� �������� ������
    public string Name; //�������� ����� �������
    public List<RuleFarmeMacroCase> Form;//Text Rule
    /*
     * RuleFarmeMacroCase - ��������� �� ����������� ������� ����������� � �������� ������������ ������� �������
     * 
     * ���� - ��������� � ������������
     *  Legion  bool - ��������� ��� ����� ���������� ����������� �� �����������
     (Rule=Legion) - ��������� ��� ����� ������������ ������ ��� ���������� �������
     (Rule=bool) - ������� �������, ������ ��� ���
     (Rule=Int) - ���� ������� ���������� ������
     (Rule=Equal)  - ������� �������, ������, ������ ��� �����
     (Rule=Constant) - ���� ������� ���������� ����������
     (Rule=Group) - ���� ������� ���������� ���������� �������
     (Rule=GroupLevel) - ���� ������� ���������� ���������� ������� � �������� ��������

     */
}
