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
    public GameObject SelectorMainAction;
    [HideInInspector]
    public GameObject SelectorMainAltRule;
    [HideInInspector]
    public GameObject SelectorMainStatTayp;
    [HideInInspector]
    public GameObject SelectorMainTag;
    [HideInInspector]
    public GameObject SelectorMainStatus;



    [HideInInspector]
    public Transform SelectorLegion;
    [HideInInspector]
    public Transform SelectorCivilianGroups;
    [HideInInspector]
    public Transform SelectorConstants;
    [HideInInspector]
    public Transform SelectorAction;
    [HideInInspector]
    public Transform SelectorAltRule;
    [HideInInspector]
    public Transform SelectorStatTayp;
    [HideInInspector]
    public Transform SelectorTag;
    [HideInInspector]
    public Transform SelectorStatus;



    public Transform SelectorLibrary;
    public Button SaveButton;
    public Button SaveSimpleButton;
    public Button SaveSimpleAllButton;
    public Button NewRuleButton;

    public GameObject ButtonOrig;
}
