using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSupplyCardController : MonoBehaviour
{
    public GameObject getSupplyField;
    public Transform supplyFieldTransform1;
    public Transform supplyFieldTransform2;

    
    //サプライ取得画面を表示してcost_minからcost_maxまでのコストのサプライを表示
    public void ShowSupplyCard(int cost_min, int cost_max)
    {
        getSupplyField.SetActive(true);

        //foreach()
        List<CardController> supplyCardList = new List<CardController>();
        foreach(CardController c  in supplyFieldTransform1.GetComponentsInChildren<CardController>())
        {
            if (cost_min <= c.model.cost && c.model.cost<= cost_max)
            {
                supplyCardList.Add(c);
            }
        }
        foreach(CardController c in supplyFieldTransform2.GetComponentsInChildren<CardController>())
        {
            if (cost_min <= c.model.cost && c.model.cost<= cost_max)
            {
                supplyCardList.Add(c);
            } 
        }
        foreach(CardController c in supplyCardList)
        {
            CardController_getsupply card = new CardController_getsupply(c);
        }

    }

    //取得するサプライカードが確定されたときに呼び出し（buttonにアタッチ）
    public void CloseSupplyField()
    {

    }


}
