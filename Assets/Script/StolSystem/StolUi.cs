using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StolUi : MonoBehaviour
{
    public Transform[] MySlot;
    public Transform[] EnemySlot;

    public GameObject OrigAction;
    public GameObject OrigCase;
    public GameObject OrigSlot;
    public GameObject OrigSlotEnemy;

    public GameObject OrigCard;
    public GameObject OrigHiro;

    public Transform UseCard;
    public Transform TargetCard;

    public Transform MyHand;
    public Transform EnemyHand;

    public TMP_Text MyInfo;
    public TMP_Text EnemyInfo;

    public Button NextTurn;

    public GameObject MeleeTurn;
    public GameObject ShotTurn;
}
