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
    /*����--
     ��������
    ��������
    ���������
    ��������� ����� ��������
    ��� ��������� �� ����������� ���� ������
     
    ������� ������� - �����������, �����:
    *�������������++
     */

    /*�������������--
    ����� � ������� �������� ����������� - ��������, ���, ���
    �� ��� ������ - �, ����, ���
    ����� �������� - ������ ����, ����� ����, �������� ������������ ������������. ����� ������, ����� �����, ��� ��������� �����, ��� ����������� ����� � ����, ��� ������ �����
     ������� ��������� �������������++
    �������� ��������� ����������++
     */

    /*����������������--
     * ����� - �����, ���, ��������� (2)
     * ������ - ���, ������, �����
     * 
     * ��������� ������� � �������� �����������, ���� ����� ���������, �� ���������� ���������� ����� ������� //������ ������� - �, ���, , ��������� (2� �����), �� �����, ������
    

    ����������� �������
     ������� - ������, �����, ������
    
    ����������� ��������������
    ���� �������
     
     */

    /*��������--
     * �� ���� ��������� - ���, �����, ������
     ������� �������� - ����, �, ���
     ������������� ������
     */
    #region Base
    public string Name;//��������
    public string Info;//��������

    public int Cost;//����
    public int CostExtend;//���� �� ��� ���� �������

    public int LevelCap;//������������ ������� �����������

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
    public string TargetTime;//Action. Start Turn. End Turn. PreAction. PostAction. PlayCard. DeadCard. DeadAnotherCard. PlayAnotherCard. PostDeadTurn(�������� � ��������)
    public List<IfAction> PlusAction;//������� ����������
    public List<IfAction> MinusAction;//������� ����������
    //public List<Action> Action;
}
/*Start Turn
 * 
 * 
 */
public class IfAction 
{
    public int Prioritet = 10;//��������� ��������
    public List<string> TextData;//��������� ����
    public List<int> IntData;//������������ ����
    /*����������� ������� 
     Action.- �������� �-�� ���-�� �����, �������� �����

    Start Turn - End Turn
    -- ����� ���� (������ ������ �����)
    -- �������� ������� ��� ����������
    -- (���)��������� ��������� �� ����� (������, ������ �����)
    -- -- ���������� ����� ������������ ��� ������������� ������������ ������� ���������
    -- (���)����� ��������� � ������������ ���� ��� �����
    -- (���)����� ����� ��������� ������� ��� �������
     */
}

