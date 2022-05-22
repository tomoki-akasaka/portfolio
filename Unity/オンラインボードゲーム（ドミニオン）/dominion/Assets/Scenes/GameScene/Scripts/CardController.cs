using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;  //見かけ
    public CardModel model;  //データ
    public CardMovement movement; //カードの移動に関する

    private void Awake()
    {
        view = this.GetComponent<CardView>();  //プレハブについているCardViewを取得
        movement = this.GetComponent<CardMovement>();//カードについているCardMovementを取得
    }

    public void Init(int cardID, bool isPlayer, bool isSupplyCard, int restCardCount)
    {
        model = new CardModel(cardID, isPlayer, isSupplyCard, restCardCount); //CardModelでカードのデータがResourcesから読み込まれる
        view.Show(model);  //プレハウのテキストにカードのデータを与える
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
