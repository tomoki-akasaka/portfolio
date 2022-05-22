using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController_getsupply : MonoBehaviour
{
    public Transform getSupplyField;
    public CardView view;
    public CardModel model;


    bool isSelected;

    private void Awake()
    {
        view = this.GetComponent<CardView>();  //プレハブについているCardViewを取得
    }

    public CardController_getsupply(CardController c)
    {
        model = c.model; 
        view.Show(model);  //プレハウのテキストにカードのデータを与える
        isSelected = false;
        GameManager.instance.CreateCard("getsupply", c.model.carddataID, getSupplyField, c.model.isSupplyCard, c.model.cardCount);
    }

        public void Destroy()
    {
        Destroy(this.gameObject);
    }




}
