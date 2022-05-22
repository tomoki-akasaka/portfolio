using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks , IPunObservable
{
    public Transform playerHandTransform;
    public Transform actionSupplyTransform1;
    public Transform actionSupplyTransform2;
    public Transform actionFieldTransform;
    public Transform enemyActionFieldTransform;
    //static DeckController playerDeck = GetComponent<DeckController>();
    public DeckController playerDeck;
    public DropActionField actionField;
    public RestActionTextController actionTextController;
    public GraveyardController graveyardController;
    public PurchasePhaseManager purchaseManager;
    public MoneyController moneyController;
    public Text buttonText;

    public GameObject resultPanel;
    public Text myResultPointText;
    public Text enemyResultPointText;
    public Text resultText;

    public GameObject cardPrefab;

    public GameObject getSupplyPrefab; 

    List<int> actionSupply;

    public bool isActionPhase;
    public bool isPurchasePhase;
    public bool isMyTurn;
    public Text isMyTurnText;
    public int destroyActionCard;
    public int resultPoint;
    public GameObject isMyTurnPanel;
    public List<int> purchaseCardList;
    //public GameObject turnDecisionPanel;



    //public List<int> playerDeck = new List<int>() { 1, 2, 1, 2, 1, 1, 1, 1, 2, 2, 2, 2 };
    //public関数にしてもgamemanagerを取得していないスクリプトからは関数を呼べないので
    //シングルトン化（どこからでもアクセスできるようにする）を行う
    public static GameManager instance;
    public void Awake()
    {
        {
            if (instance == null)
            {
                instance = this;
            }
        }

    }

    void Start()
    {
        StartGame();
    }

    public void TurnDicision()
    {
        //入室済みのプレイヤー数を見て順番を決定する
        var playerList = PhotonNetwork.PlayerList;
        var playerNum = playerList.Length;
        Debug.Log(playerNum);
        if (playerNum == 1)
        {
            isMyTurn = true;
        }
        int myTurn = playerNum;
        Debug.Log("あなたは"+ myTurn +"番目のプレイヤーです");
    }

    void StartGame()
    {
        TurnDicision();
        Debug.Log(isMyTurn);
        purchaseCardList = new List<int>();
        isMyTurnText.text = "接続中";

        resultPoint = 0;
        //場のアクションカード
        actionSupply = new List<int>() {31,32,33,34,35,36,37,38,39,40,1,11,2,12,3,13};
        destroyActionCard = 0;
        playerDeck.Init(new List<int>() { 1, 1, 11, 11, 1, 1, 1, 11, 1, 1 });
        playerDeck.Shuffle();
        Debug.Log("playerDeck.deck.Count" + playerDeck.deck.Count);
        graveyardController.Init(new List<int>());
        SettingInitHand(5);
        actionField.restActionCount = 1;
        SettingSupply();
        actionField.restActionCount = 1;
        
        PlayerTurn();
    }
   
    public void SettingInitHand(int hand)
    {

        for (int i = 0; i < hand; i++)
        {
            if (playerDeck.deck.Count == 0)  //残りデッキ枚数が0なら処理をスキップする
            {
                Debug.Log("deck == 0");
                graveyardController.Shuffle();
                playerDeck.AddCard(graveyardController.graveyard);
                graveyardController.Init(new List<int>());
                Debug.Log("残りデッキ" + playerDeck.deck.Count);
            }
            GiveCardToHand(playerDeck.deck, playerHandTransform);
            
        }
    }

    void GiveCardToHand(List<int> deck, Transform hand)
    {
        
        int cardID = deck[0];
        Debug.Log("ok");
        deck.RemoveAt(0);           //手札に追加したカードをデッキからとりのぞく
        CreateCard("normal", cardID, hand, false, 1);
    }

    public CardController CreateCard(string prefab_kind, int cardID, Transform hand, bool isSupplyCard, int restCardCount)
    {
        //生成したカードを管理できるようにCardController型で格納する
        //Debug.Log("a");
        GameObject prefab = new GameObject();
        
        if(prefab_kind == "normal")
        {
            prefab = cardPrefab;
        }
        else if(prefab_kind == "getSupply")
        {
            prefab = getSupplyPrefab;
        }
        GameObject card_gameobject = Instantiate(prefab, hand, false);
        CardController card = card_gameobject.GetComponent<CardController>();
        //生成したカードの情報をResourcesフォルダからとってくる
        if (hand.name == "PlayerHand")
        {
            card.Init(cardID, true, isSupplyCard, restCardCount);
        }
        else
        {
            card.Init(cardID, false, isSupplyCard, restCardCount);
        }

        return card;

    }

    void SettingSupply()
    {
        bool isActionSupplyField1 = true;
     
        foreach(int cardID in actionSupply)
        {
            if(isActionSupplyField1 == true)
            {
                CreateCard("normal", cardID, actionSupplyTransform1, true, -1);
            }
            else
            {
                CreateCard("normal", cardID, actionSupplyTransform2, true, -1);
            }
            isActionSupplyField1 = !isActionSupplyField1;
        }
    }

    public void StartActionPhase()
    {
        
        isActionPhase = true;

    }

    public void StartPurchasePhase()
    {
        isActionPhase = false;
        isPurchasePhase = true;
    }

    public void PhaseChange()
    {
        if(isActionPhase == true)
        {
            isActionPhase = false;
            isPurchasePhase = true;
            buttonText.text = "購入終了";

        }else
        
        if(isPurchasePhase == true)
        {
            
            isPurchasePhase = false;
            buttonText.text = "ターンエンド";
        }else

        if(isActionPhase != true && isPurchasePhase != true)
        {
            TurnEnd();
        }
    }

    public void TurnEnd()
    {
        isPurchasePhase = false;

        List<int> abandoneCards = new List<int>();
        //アクションフィールドのカードを取得
        CardController[] actionFieldCardList = actionFieldTransform.GetComponentsInChildren<CardController>();
        //アクションフィールドのカードを墓地に追加してオブジェクト削除
        foreach(CardController card in actionFieldCardList)
        {
            Debug.Log("actionfield");
            abandoneCards.Add(card.model.carddataID);
            card.Destroy();
        }
        //手札のカードを取得
        CardController[] handCardList = playerHandTransform.GetComponentsInChildren<CardController>();
        //手札のカードを墓地に追加してオブジェクト削除
        foreach (CardController card in handCardList)
        {
            Debug.Log("handField");
            Debug.Log("手札カードID" + card.model.carddataID);
            abandoneCards.Add(card.model.carddataID);
            card.Destroy();
        }
        graveyardController.AddGraveyard(abandoneCards);
        SettingInitHand(5);
        PlayerTurn();

        int a = 1;

        photonView.RPC(nameof(ChangeTurn), RpcTarget.All); 
    }

    public void PlayerTurn()
    {

        purchaseManager.Init(1);
        actionField.restActionCount = 1;
        actionTextController.ShowActionNum(1);
        moneyController.money = 0;
        moneyController.ShowMoney();
        buttonText.text = "アクション終了";
        isActionPhase = true;
    }

    public Transform enemyActionField;

   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    [PunRPC]
    private void ChangeTurn()
    {
        isMyTurn = !isMyTurn;
        CardController[] enemyActionCardList = enemyActionFieldTransform.GetComponentsInChildren<CardController>();
        foreach (CardController enemyCard in enemyActionCardList)
        {
            enemyCard.Destroy();
        }

        CheckGameEnd();   
    }

    public void CheckGameEnd()
    {
        if(destroyActionCard >= 3)
        {
            foreach(int cardID in playerDeck.deck)
            {
                if(cardID == 11)
                {
                    resultPoint += 1;
                }

                if(cardID == 12)
                {
                    resultPoint += 3;
                }

                if(cardID == 13)
                {
                    resultPoint += 6;
                }

                if(cardID == 14)
                {
                    resultPoint += 10;
                }
            }
            photonView.RPC(nameof(ShowResultPanel), RpcTarget.Others, resultPoint);
        }
    }

    [PunRPC]
    public void ShowResultPanel(int enemyResultPoint)
    {
        resultPanel.SetActive(true);
        myResultPointText.text = resultPoint.ToString();
        enemyResultPointText.text = enemyResultPoint.ToString();
        if(resultPoint > enemyResultPoint)
        {
            resultText.text = "YOU WIN";
        }else if(resultPoint < enemyResultPoint)
        {
            resultText.text = "YOU LOSE";
        }
        else
        {
            resultText.text = "DRAW";
        }



    }

    public void LoadLauncherScene()
    {
        resultPanel.SetActive(false);
        SceneManager.LoadScene("LauncherScene");
    }
    
    public void InvisibleIsMyTurnPanel()
    {
        isMyTurnPanel.SetActive(false);
    }
}
