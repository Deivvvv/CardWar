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
    public List<GameObject> SelectorsMain;

    [HideInInspector]
    public List<Transform> Selectors;


    public Transform IfSelector;
    public Transform ActionSelector;

    public GameObject ButtonOrig;
}
