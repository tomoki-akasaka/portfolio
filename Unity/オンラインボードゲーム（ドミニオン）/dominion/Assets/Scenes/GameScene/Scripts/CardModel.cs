using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel 
{
    public int cost;
    public int money;
    public int plusAction;
    public int plusCard;
    public int plusMoney;
    public int carddataID;
    public int plusPurchase;
    public int cardCount;
    public bool isSupplyCard;
    public bool isActionCard;
    

    public Sprite icon;

    public CardModel(int cardID, bool isPlayer, bool supply, int restCardCount)
    {
        //Resources/CardEntityListにあるカードの中で引数で指定したカードの情報をとってくる
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntity/card" + cardID);
        icon = cardEntity.icon;
        //サプライのカードならtrueを入れる
        isSupplyCard = supply;
        isActionCard = cardEntity.isActionCard;
        plusAction = cardEntity.plusAction;
        plusCard = cardEntity.plusCard;
        money = cardEntity.money;
        cost = cardEntity.cost;
        carddataID = cardID;
        plusPurchase = cardEntity.plusPurchase;
        if(restCardCount == -1)
        {
            cardCount = cardEntity.cardCount;
        }
        else
        {
            cardCount = restCardCount;

        }
    }
}
