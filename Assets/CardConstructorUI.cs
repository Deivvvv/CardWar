using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class CardConstructorUI : MonoBehaviour
{

    public TMP_InputField NameTT;
    public GameObject SaveWindow;
    public CardBody Body;
    public GameObject StartWindow;
    public GameObject OrigButton;
    public GameObject OrigStatButton;
    public GameObject OrigRing;
    public TextMeshProUGUI InfoPanel;
    [HideInInspector] public List<Transform> MainList;
    [HideInInspector] public List<Text> MainListInfo;


    public Transform StatWindow;
    public Transform RuleWindow;
    public Transform LocalRuleWindow;
    [HideInInspector] public List<Transform> StatRing;
    [HideInInspector] public List<Transform> RuleRing;
    [HideInInspector] public List<Transform> LocalRuleRing;

    public List<Button> ButtonList;
}
