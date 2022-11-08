using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardCaseInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CardConstructor card;
    int head;
    int data;
    int dataSub;
    string mood;

    // Start is called before the first frame update
    public void Set(CardConstructor _card, int _head, int _data, int _dataSub, string _mood)
    {
        head = _head;
        data = _data;
        card = _card;
        dataSub = _dataSub;
        mood = _mood;// "rule";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (mood)
        {
            case ("full"):
                card.GetInfo(head, data);
                break;
            case ("stat"):
                card.GetInfo(head, data);
                break;
            case ("rule"):
                //card.GetInfo(head, data, dataSub);
                break;
            case ("statHead"):
                card.GetInfo(head, data);
                break;
            case ("ruleHead"):
                //card.GetInfo(head, data, dataSub);
                break;

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        card.GetFullInfo();
    }
}
