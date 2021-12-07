using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardConstructorUi : MonoBehaviour
{
    public List<CardBase> CardLibrary;
    public InputField NameFlied;

    public Transform BaseCard;

    public Transform StatCard;
    public Transform TraitCard;

    public Text[] StatCount;
    public Text[] TraitCount;

    public Text ManaCount;
    //public Text ManaCount;

    public GameObject OrigStat;
    public GameObject OrigCard;

    //public Sprite[] Icon;
    //public string[] StatName;
    //public int[] SellCount;
    //public string[] NameIcon;

    public Button SaveButton;
    public Button LoadButton;
    public Button DeliteButton;

    public Color[] SelectColor;
}
