using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StolUi : MonoBehaviour
{
    public GameObject OrigButton;

    public GameObject OrigAction;
    public GameObject OrigCase;

    public GameObject OrigCard;
    public GameObject OrigHiro;

    //public Transform UseCard;
    //public Transform TargetCard;

    public Transform MyHand;
    public Transform EnemyHand;

    public TMP_Text MyInfo;
    public TMP_Text EnemyInfo;

    public Button NextTurn;

    public Transform AllTacticCase;
    public Transform TacticCase;
    public List<Transform> AllTactic  = new List<Transform>();


    public Transform MyFirstStol;
    public Transform EnemyFirstStol;
}
