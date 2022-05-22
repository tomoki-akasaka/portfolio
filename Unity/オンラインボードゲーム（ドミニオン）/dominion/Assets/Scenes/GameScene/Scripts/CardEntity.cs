using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]
//カードデータそのもの
public class CardEntity : ScriptableObject
{
    public enum TYPE
    {
        HAND,
        FIELD,
        SUPPLY,
    }
    public int point;
    public new string name;
    public int plusAction;
    public int plusCard;
    public int cost;
    public int money;
    public int plusPurchase;
    public bool isActionCard;
    public Sprite icon;
    public int cardCount;

}
