using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public Image iconImage;
    public Text restSupplyCardCount;
    public GameObject restCountPanel;

    public void Show(CardModel cardModel)
    {
        iconImage.sprite = cardModel.icon;
        if(cardModel.isSupplyCard == true)
        {
            restCountPanel.SetActive(true);
            restSupplyCardCount.text = cardModel.cardCount.ToString();

        }
    }

}
