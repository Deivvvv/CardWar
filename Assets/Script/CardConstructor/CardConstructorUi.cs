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




    public Transform StatCard;
    public Transform TraitCard;

    public Transform BaseFiltr;


    public GameObject OrigStat;
    public GameObject OrigCard;
    public GameObject OrigFiltr;
    public GameObject OrigButton;
    public GameObject OrigButtonBanner;
    public GameObject SelectorOrigin;


    public Button EjectButton;
    public Button InjectButton;
    public Button DeliteButton;

    public Button SaveButton;
    public Button ResetButton;

   // public Color[] SelectColor;


    public Transform ConstantSelectorMain;

    public Transform ConstantSelector;

    public GameObject RuleSelectorMain;
    public Transform RuleSelector;

    public Transform AddStat;
    public Transform AddRule;
    public Button AddStatButton;
    public Button AddRuleButton;

    public List<Transform> CardBody;

    public Text CounterCard;
    public GameObject NewCardButton;
    public Button[] ModButton;
}
