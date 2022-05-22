using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasePhaseManager : MonoBehaviour
{
    public MoneyController moneyController;
    public int restPurchaseCount;

    private void Start()
    {
        restPurchaseCount = 1;
    }

    public void Init(int num)
    {
        restPurchaseCount = num;
    }

    public void Purchase(int cost)
    {
        moneyController.ReduceMoney(cost);
        restPurchaseCount--;
        if(restPurchaseCount <= 0)
        {
            GameManager.instance.PhaseChange();
        }
     
    }

    public void AddPurchaseCount(int num)
    {
        restPurchaseCount += num;

    }


    
}
