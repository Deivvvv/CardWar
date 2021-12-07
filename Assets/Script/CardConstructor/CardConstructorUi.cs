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

    public GameObject OrigStat;
    public GameObject OrigCard;

    //public Sprite[] Icon;
    //public string[] StatName;
    //public int[] SellCount;
    //public string[] NameIcon;

    public Button EjectButton;
    public Button InjectButton;
    public Button DeliteButton;

    public Button SaveButton;
    public Button ResetButton;

    public Color[] SelectColor;
}
