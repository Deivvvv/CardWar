using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardConstructor : MonoBehaviour
{
    private CardBase cardBase;
    private List<CardBase> LocalCard;

    public CardConstructorUi Ui;


    void LoadBase()
    {
        //���������� ����� ���������� �� �������
    }

    void NewCard()
    {
        cardBase = new CardBase();
        cardBase.Name = "New Hiro";
        cardBase.Stat = new int[13];
        cardBase.Trait = new string[5];
    }
    void ViewCard()
    {

    }


    void LoadUi()
    {/*
     ��������� �������
        ���������� ����������
     
     */

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
