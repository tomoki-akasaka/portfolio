using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionFieldController : MonoBehaviourPunCallbacks
{
    public Transform enemyActionField;

    [PunRPC]
    private void CreateEnemyAction(int cardID)
    {
        GameManager.instance.CreateCard("normal", cardID, enemyActionField, false, 1);
    }

}
