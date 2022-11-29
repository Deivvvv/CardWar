using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TableSys;
using Coder;

public class TableInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int cardId;
    TriggerAction triggerAction;
    
    public void Set(int _cardId, TriggerAction _triggerAction)
    {
        triggerAction = _triggerAction;
        cardId = _cardId;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (triggerAction != null)
        {
            UiSys.SelectTarget(triggerAction);
        }
        else
        {
            UiSys.Use(cardId,true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       UiSys.View(null);
    }
}
