using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    public Text moneyText;
    public int money;

    //シングルトン化（どこからでもアクセスできるようにする）を行う
    public static MoneyController instance;
    public void Awake()
    {
        {
            if (instance == null)
            {
                instance = this;
            }
        }
    }

    private void Start()
    {
        money = 0;
        moneyText.text = "0";
    }

    public void PlusMoney(int num)
    {
        money += num;
        ShowMoney();
    }

    public void ReduceMoney(int num)
    {
        money -= num;
        ShowMoney();
    }

    public void ShowMoney()
    {
        moneyText.text = money.ToString();
    }

}
