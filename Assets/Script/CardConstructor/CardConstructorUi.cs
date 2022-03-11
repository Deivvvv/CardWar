using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardConstructorUi : MonoBehaviour
{
    public InputField NameFlied;

    public Image GuildBanner;
    public GameObject GuildSelector;
    public Transform BaseCard;

    public Button GuildButton;

    public Button RaceButton;
    public Text RaceText;

    public Button LegionButton;
    public Text LegionText;

    public Button CivilianButton;
    public Text CivilianText;

    public List<StatCaseUi> StatUi;
    public List<StatCaseUi> RuleUi;
    //public Button[] StatButtons;
    //public Button[] RuleButtons;



    public Transform StatCard;
    public Transform TraitCard;

    public Transform BaseFiltr;

    public Text[] StatCount;
    public Text[] TraitCount;

    public Text ManaCount;

    public GameObject OrigStat;
    public GameObject OrigCard;
    public GameObject OrigFiltr;
    public GameObject OrigButton;
    public GameObject OrigButtonBanner;
    public GameObject SelectorOrigin;

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


    public Transform ConstantSelectorMain;

    public Transform ConstantSelector;

    public GameObject RuleSelectorMain;
    public Transform RuleSelector;
}
