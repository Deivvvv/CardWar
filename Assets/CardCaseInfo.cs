using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardCaseInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CardConstructor card;
    int mood;
    int data;

    // Start is called before the first frame update
    public void Set(CardConstructor _card,int _mood, int _data)
    {
        mood = _mood;
        data = _data;
        card = _card;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        card.GetInfo(mood, data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        card.GetFullInfo();
    }
}
