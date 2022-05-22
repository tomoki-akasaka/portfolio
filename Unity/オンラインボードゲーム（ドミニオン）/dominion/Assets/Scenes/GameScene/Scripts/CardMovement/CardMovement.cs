using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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
        CardController card = GetComponent<CardController>();
        
        //残りアクション回数が0　かつ　アクションフェイズ　かつ　持ったカードがアクションカード
        if (DropActionField.instance.restActionCount <= 0 && GameManager.instance.isActionPhase && card.model.isActionCard == true)
        {
            isDraggable = false;
        }
        //アクションフェーズ　かつ　サプライのカードを持った
        if(GameManager.instance.isActionPhase == true && card.model.isSupplyCard == true)
        {
            isDraggable = false;
        }
        //アクションフェーズ　かつ　使用済みカードの場合
        if(GameManager.instance.isActionPhase == true && card.transform.parent.gameObject.name == "ActionField")
        {
            isDraggable = false;
        }
        
        //購入フェーズ　かつ　サプライ以外のカードを持った
        if(GameManager.instance.isPurchasePhase == true && card.model.isSupplyCard != true)
        {
            isDraggable = false;

        }
        //購入フェーズ　かつ　指定したカードが残り金額より大きい
        if (GameManager.instance.isPurchasePhase == true && MoneyController.instance.money < card.model.cost)
        {
            isDraggable = false;
        }


        //アクションフェーズでも購入フェーズでもない
        if (GameManager.instance.isPurchasePhase != true && GameManager.instance.isActionPhase != true)
        {
            isDraggable = false;
        }
        
        //自分のターンでない場合
        if(GameManager.instance.isMyTurn == false)
        {
            isDraggable = false;
        }


        if (isDraggable == false)
        {
            
            return;
        }
        Debug.Log("start drag");
        
        if (card.model.isSupplyCard)
        {
            Vector3 currentPosition = transform.position;
            int siblingIndex = transform.GetSiblingIndex();
            CardController supplyCard = GameManager.instance.CreateCard("normal", card.model.carddataID, defaultParent, true, card.model.cardCount - 1);
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
        if (isDraggable == false)
        {
            isDraggable = true;
  
            return;
        }

        transform.SetParent(defaultParent, false);
        //マウスのポインタがカードを貫通しない
        GetComponent<CanvasGroup>().blocksRaycasts = true; 
    }
}
