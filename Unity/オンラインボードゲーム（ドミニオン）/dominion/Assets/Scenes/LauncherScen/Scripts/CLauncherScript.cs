using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CLauncherScript : MonoBehaviourPunCallbacks
{
    #region Public変数定義

    //Public変数の定義はココで
    public GameObject connectButton;
    public GameObject playerName_obj;
    public InputField playerName;
    #endregion

    #region Private変数
    //Private変数の定義はココで
    #endregion

    #region Public Methods
    //ログインボタンを押したときに実行される
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {           //Photonに接続できていなければ
            PhotonNetwork.ConnectUsingSettings();   //Photonに接続する
            PhotonNetwork.NickName = playerName.text;
            Debug.Log("Photonに接続しました。");
        }
        connectButton.SetActive(false);
        playerName_obj.SetActive(false);
        
    }
    #endregion

    #region Photonコールバック
    //ルームに入室前に呼び出される
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMasterが呼ばれました");
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    //ルームに入った時に呼ばれる
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに入りました。");
        //battleシーンをロード
        PhotonNetwork.LoadLevel("GameScene");
    }

    #endregion
}