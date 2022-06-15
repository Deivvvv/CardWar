using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public int Size;
    //public List<GameDataData> Guild;//data
    public List<SubGameData> Data;

    /*система контроля изменений*/
    public List<string> Path;//путь
    public List<string> Com;//что сделали
    public List<CardBase> Card;//карты для изменений

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
      гильдия
         группа
             тип достпа группы
                 карты внутри группы

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
}

//public class GameDataTayp
//{
//    public int AllSize;
//    public string Name;
//    public List<GameDataForm> Form;//data
//}

//public class GameDataForm
//{
//    public int AllSize;
//    public string Name;
//    public List<GameDataForm> Form;//data
//}


