using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestActionTextController : MonoBehaviour
{
    public Text restActionText;

    private void Start()
    {
        restActionText.text = "1";
    }

    public void ShowActionNum(int num)
    {
        restActionText.text = num.ToString();
    }




}
