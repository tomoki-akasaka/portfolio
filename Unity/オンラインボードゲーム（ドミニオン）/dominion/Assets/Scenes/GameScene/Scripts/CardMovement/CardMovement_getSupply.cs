using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardMovement_getSupply : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform defaultParent;
    public bool isDraggable;

    void Start()
    {
        defaultParent = transform.parent;
        CardController card = GetComponent<CardController>();
        isDraggable = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CardController_getsupply card = GetComponent<CardController_getsupply>();
        
        if (isDraggable == false)
        {
            return;
        }
        Debug.Log("start drag");
        
        if (card.model.isSupplyCard)
        {
            Vector3 currentPosition = transform.position;
            int siblingIndex = transform.GetSiblingIndex();
            CardController supplyCard = GameManager.instance.CreateCard("getSupply", card.model.carddataID, defaultParent, true, card.model.cardCount - 1);
            supplyCard.transform.SetSiblingIndex(siblingIndex);
        }

        defaultParent = transform.parent;
        //カードを動かしている間、親の親を親に設定する
        transform.SetParent(defaultParent.parent, false);
        //マウスのポインタがカードを貫通する
        GetComponent<CanvasGroup>().blocksRaycasts = false; 
    } 

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggable == false)
        {
            return;
        }

        //カードの場所にマウスの場所を代入
        transform.position = eventData.position; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CardController_getsupply card = GetComponent<CardController_getsupply>();
        if (isDraggable == false)
        {
            isDraggable = true;
  
            return;
        }

        transform.SetParent(defaultParent, false);
        if (defaultParent != GameManager.instance.actionFieldTransform)
        {
            card.Destroy();
        }

        //マウスのポインタがカードを貫通しない
        GetComponent<CanvasGroup>().blocksRaycasts = true; 
    }
}
