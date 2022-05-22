using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class DropActionField : MonoBehaviourPunCallbacks, IDropHandler, IPunObservable
{
    public enum TYPE
    {
        HAND,
        FIELD,
    }

    public TYPE type;
    public ActionEntity action;
    public Transform handTransform;
    public Transform enemyActionField;
    public DeckController playerDeck;
    public bool isDroppable;
    public int restActionCount;
    public MoneyController money;
    public PurchasePhaseManager purchaseManager;
    public CardMovement cardMovement;
    public Transform actionSupplyTransform1;
    public Transform actionSupplyTransform2;


    private void Start()
    {
         isDroppable = true;
    }

    public static DropActionField instance;
    public void Awake()
    {
        {
            if (instance == null)
            {
                instance = this;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        //残りアクション回数が0　かつ　アクションフェイズ　かつ　アクションカード
        if (restActionCount <= 0 && GameManager.instance.isActionPhase && card.model.isActionCard == true)
        {
            isDroppable = false;
        }

        //アクションフェース　かつ　サプライのカード
        if(GameManager.instance.isActionPhase == true && card.model.isSupplyCard == true)
        {
            isDroppable = false;
        }

        //購入フェーズ　かつ　サプライ以外のカード
        if(GameManager.instance.isPurchasePhase == true && card.model.isSupplyCard != true)
        {
            isDroppable = false;
        }

        //購入フェーズ　かつ　残り購入回数0
        if (GameManager.instance.isPurchasePhase == true && MoneyController.instance.money < card.model.cost)
        {
            isDroppable = false;
        }

        //アクションフェーズでも購入フェーズでもない
        if (GameManager.instance.isPurchasePhase != true && GameManager.instance.isActionPhase != true)
        {
            isDroppable = false;
        }

        if (card.movement.isDraggable == false || isDroppable == false)
        {
            isDroppable = true;
            return;
            
        }

        //手札にドロップされても何も処理を行わないのでreturn（typeはドロップされた場所）
        if (type == TYPE.HAND)
        {
            isDroppable = true;
            return;
        }
        
        
        if(card != null)
        {
            card.movement.defaultParent = this.transform;
         
        }

        if(GameManager.instance.isActionPhase == true)
        {
            //自分の使ったカードをRPCで相手フィールドに表示させる
            photonView.RPC(nameof(CreateEnemyAction), RpcTarget.Others, card.model.carddataID);

            if (card.model.isActionCard == true)
            {
                restActionCount--;
                //アクションを実行
                //Debug.Log("plusCard = " + card.model.plusCard);
                //カードを引く
                action.PlusCard(card.model.plusCard);
                //アクション回数を増やす
                //action.PlusAction
                action.PlusAction(card.model.plusAction);
                //購入回数を増やす
                action.PlusPurchase(card.model.plusPurchase);
                //金を増やす
                action.PlusMoney(card.model.money);
            }
            else
            {
                money.PlusMoney(card.model.money);
            }
        }

        if (GameManager.instance.isPurchasePhase == true)
        {

            photonView.RPC(nameof(CreateEnemyAction), RpcTarget.Others, card.model.carddataID);
            purchaseManager.Purchase(card.model.cost);
            photonView.RPC(nameof(ReduceSupplyCount), RpcTarget.Others, card.model.carddataID);
        } 

    }

    [PunRPC]
    private void CreateEnemyAction(int cardID)
    {
        GameManager.instance.CreateCard("normal", cardID, enemyActionField, false, 1);
    }

    [PunRPC]
    private void ReduceSupplyCount(int cardID)
    {
        CardController[] supplyFieldCardList1 = actionSupplyTransform1.GetComponentsInChildren<CardController>();
        foreach (CardController card in supplyFieldCardList1)
        {
            if(card.model.carddataID == cardID)
            {
                card.model.cardCount--;
                card.view.Show(card.model);
                if(card.model.cardCount == 0)
                {
                    card.Destroy();
                    GameManager.instance.destroyActionCard++;
                }
            }
           
        }
        CardController[] supplyFieldCardList2 = actionSupplyTransform2.GetComponentsInChildren<CardController>();
        foreach (CardController card in supplyFieldCardList2)
        {
            if (card.model.carddataID == cardID)
            {
                card.model.cardCount--;
                card.view.Show(card.model);
                if (card.model.cardCount == 0)
                {
                    card.Destroy();
                    GameManager.instance.destroyActionCard++;
                }
            }

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
  
    }
}
