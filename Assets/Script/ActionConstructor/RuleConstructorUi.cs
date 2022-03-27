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
    public GameObject SelectorMainAltRule;
    [HideInInspector]
    public GameObject SelectorMainStatTayp;

    //[HideInInspector]
    //public GameObject SelectorMainIfCore1;
    //[HideInInspector]
    //public GameObject SelectorMainIfCore2;



    [HideInInspector]
    public Transform SelectorLegion;
    [HideInInspector]
    public Transform SelectorCivilianGroups;
    [HideInInspector]
    public Transform SelectorConstants;
    [HideInInspector]
    public Transform SelectorEffects;
    [HideInInspector]
    public Transform SelectorAltRule;
    [HideInInspector]
    public Transform SelectorStatTayp;

    //[HideInInspector]
    //public GameObject SelectorIfCore1;
    //[HideInInspector]
    //public GameObject SelectorIfCore2;



    //public Transform IfSelector;
    //public Transform ActionSelector;



    public Transform SelectorLibrary;
    public Button SaveButton;
    public Button NewRuleButton;

    public GameObject ButtonOrig;
}
