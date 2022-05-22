using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEntity : MonoBehaviour
{
    public RestActionTextController actionText;
    public PurchasePhaseManager purchaseManager;
    public MoneyController moneyController;
    public GameObject GetSupplyPanel;
    public GetSupplyCardController getSupplyCardController;

    public void PlusCard( int num)
    {
        GameManager.instance.SettingInitHand(num);
        actionText.ShowActionNum(DropActionField.instance.restActionCount);

    }

    public void PlusAction(int num)
    {
        DropActionField.instance.restActionCount += num;
        actionText.ShowActionNum(DropActionField.instance.restActionCount);

    }

    public void PlusPurchase(int num)
    {
        purchaseManager.AddPurchaseCount(num);
    }

    public void PlusMoney(int num)
    {
        moneyController.PlusMoney(num);
    }

    public void GetSupplyCard(int cost_min, int cost_max)
    {
        getSupplyCardController.ShowSupplyCard(cost_min, cost_max);
    }


}
