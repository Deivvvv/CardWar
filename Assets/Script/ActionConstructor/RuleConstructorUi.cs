using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleConstructorUi : MonoBehaviour
{
    public Transform MouseIndicator;

    public GameObject LableOrig;
    public Transform Lable;

    public GameObject TextWindow;
    public InputField TextInput;
    public Button TextWindowButton;

    public Transform Canvas;
    public GameObject SelectorMain;

    [HideInInspector]
    public GameObject SelectorMainLegion;
    [HideInInspector]
    public GameObject SelectorMainCivilianGroups;
    [HideInInspector]
    public GameObject SelectorMainConstants;
    [HideInInspector]
    public GameObject SelectorMainEffects;


    [HideInInspector]
    public Transform SelectorLegion;
    [HideInInspector]
    public Transform SelectorCivilianGroups;
    [HideInInspector]
    public Transform SelectorConstants;
    [HideInInspector]
    public Transform SelectorEffects;


    public Transform IfSelector;
    public Transform ActionSelector;

    public GameObject ButtonOrig;
}
