using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public List<int> deck = new List<int>();

    public void Init(List<int> cardDeck)
    {
        this.deck = cardDeck;
    }

    public void Shuffle()
    {
        deck = deck.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var item in deck)
        {
            print(item);
        }
    }

    public void AddCard(List<int> cards)
    {
        foreach(int i in cards)
        {
            Debug.Log("addcard to deck");
            deck.Add(i);
        }
    }

}
