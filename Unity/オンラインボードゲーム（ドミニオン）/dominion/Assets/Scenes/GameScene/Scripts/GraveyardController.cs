using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraveyardController : MonoBehaviour
{
    public List<int> graveyard;

    public void Init(List<int> cards)
    {
        graveyard = cards;
    }

    public void AddGraveyard(List<int> abandonedCards)
    {

        foreach(int i in abandonedCards)
        {
            graveyard.Add(i);
        }
        Debug.Log("墓地" + graveyard.Count);
        Debug.Log("墓地カードID" + graveyard[0]);
    }

    public void Shuffle()
    {
        graveyard = graveyard.OrderBy(a => Guid.NewGuid()).ToList();
    }



}
