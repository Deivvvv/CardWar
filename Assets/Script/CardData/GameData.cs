using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public int Size;
    //public List<GameDataData> Guild;//data
    public List<SubGameData> Data;

    /*������� �������� ���������*/
    public List<string> Path;//����
    public List<string> Com;//��� �������
    public List<CardBase> Card;//����� ��� ���������

    /*
     size
    list MasterKey
    list key
    size
     */
    ////UseData
    //public List<CardBase> UseCard;
    //public List<string> CardPath;// string[] CardPath;
}


/*
      �������
         ������
             ��� ������ ������
                 ����� ������ ������

      */
//[System.Serializable]
//public class GameDataData
//{
//    //nomad//spell//L//1
//    public string Name = " ";
//    public bool Use;
//    public bool End;
//    public int Size;
//    public List<GameDataData> Data;//data
//}
public class SubGameData
{
    public string MasterKey;
    public string Key = " ";
    public int Size;
    public string KeyComplite = " ";

    public bool Use = true;
}



