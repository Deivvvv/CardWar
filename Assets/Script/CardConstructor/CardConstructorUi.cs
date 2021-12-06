using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardConstructorUi : MonoBehaviour
{
    public InputField NameFlied;

    public Transform BaseCard;

    public Transform StatCard;
    public Transform TraitCard;

    public Text[] StatCount;
    public Text[] TraitCount;

    public Text ManaCount;
    //public Text ManaCount;

    public GameObject OrigStat;

    public Sprite[] Icon;
    public string[] StatName;
    public int[] SellCount;
}
