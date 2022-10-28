/*
    2022/10/18
    Atsuki Kobayashi

    2022/10/24
    Atsuki Kobayashi
    ・鬼キャラと逃げキャラを条件によって分岐させて生成可能にした。
    ・鬼キャラと逃げキャラの生成位置をランダムにした。
    ・SetNameスクリプトより、名前を入力してからオブジェクトを生成する方式を実装。

    参考サイト==https://enia.hatenablog.com/entry/unity/introduction/20
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class RandomMatchMaker : MonoBehaviourPunCallbacks
{
    // インスペクターから設定
    public GameObject[] OniObject = {null,null,null};		 //鬼オブジェクト
    public GameObject[] PlayerObject = {null,null,null};	//プレイヤーオブジェクト
    public GameObject[] SpawnPoint;							 //キャラクタースポーンポイント
    public GameObject[] Character = {null,null,null,null};	//生成したキャラクターを格納する配列
    public GameObject MasterConfig;							//マスターコンフィグオブジェクト
    public Text Text;
    private int Number;										//鬼側か逃げる側かを識別するナンバー
    public static int gomi=0;
    public static int i = -1;								 //ナンバー
    public float currentTime;
    public static bool GameStartFlg = false;//ゲーム開始フラグ
    public static bool CharacterSpawnFlg =false;
    public static bool kasu = false;

    public static int unnko;
    public GameObject Oni_Button;
    void Update() {
        //SetNameスクリプトの名前入力後フラグがtrueになったらConnect関数を実行
        if(SetName.onEndEditFLG) {
            Connect();
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4) {
            Debug.Log("Connect");
            photonView.RPC(nameof(sinekasu),RpcTarget.All);
        }
    }

	//Photonマスターサーバー接続
    void Connect() {
        Number = GoToChooseChara.PlayMode;				//GotoGameSceneから鬼か逃げる側かを識別するナンバーを受け取る
        PhotonNetwork.ConnectUsingSettings();	//Photonネットワークへの接続処理部分(これがないとフォトンは使用できない)
    }

    //マスターサーバ？に接続した時
    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinRandomRoom(); //ランダムにルームに接続
    }

	//ロビーへの入室に失敗した場合
    public override void OnJoinedLobby() {
        PhotonNetwork.JoinRandomRoom(); //ランダムにルームに再接続
    }

	//ルームに参加できなかった場合
    public override void OnJoinRandomFailed(short returnCode, string message) {
        RoomOptions roomOptions = new RoomOptions();	//ルームをインスタンス化
        roomOptions.MaxPlayers = 4;						//ルーム接続の最大人数

        PhotonNetwork.CreateRoom("bakakasu", roomOptions);	//ルームを作成(ルームの名前を指定しない場合はnullを指定)
    }

	//ルームに参加した時
    public override void OnJoinedRoom() {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");						//シーン上のメインカメラを取得
        GameObject CinemachineManager = GameObject.FindWithTag("MainCameraManager");		//シーン上のメインカメラマネージャーを取得
        CinemachineManager.GetComponent<Cinemachine.CinemachineFreeLook>().enabled = true;	//メインカメラマネージャーのCinemachineFreeLookを有効にする
        CinemachineManager.GetComponent<Cinemachine.CinemachineCollider>().enabled = true;	//メインカメラマネージャーのCinemachineColliderを有効にする
        CinemachineFreeLook camera = CinemachineManager.GetComponent<CinemachineFreeLook>();//CinemachineFreeLookコンポーネントを取得
        switch(Number) {
            case 0:
                GameObject Player = PhotonNetwork.Instantiate(PlayerObject[GoToChooseChara.Characters].name,SpawnPoint[PhotonNetwork.CurrentRoom.PlayerCount-1].transform.position,Quaternion.identity,0);//Playerオブジェクトを生成
                camera.Follow = Player.transform; //CinemachineFreeLookコンポーネント内のFollowにOniオブジェクトのtransformを設定
                camera.LookAt = Player.transform; //CinemachineFreeLookコンポーネント内のLookAtにOniオブジェクトのtransformを設定
                break;
            case 1:
                GameObject Oni = PhotonNetwork.Instantiate(OniObject[GoToChooseChara.Characters].name,SpawnPoint[PhotonNetwork.CurrentRoom.PlayerCount-1].transform.position,Quaternion.identity,0);//Oniオブジェクトを生成
                camera.Follow = Oni.transform; //CinemachineFreeLookコンポーネント内のFollowにOniオブジェクトのtransformを設定
                camera.LookAt = Oni.transform; //CinemachineFreeLookコンポーネント内のLookAtにOniオブジェクトのtransformを設定
                break;
        }
		//ルームに入室している人数がルームの最大人数になったら
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4) {
            PhotonNetwork.CurrentRoom.IsOpen = false; //ルームを閉める
        }
    }
    [PunRPC]
    void sinekasu(){
        GameStartFlg = true; //ゲーム開始フラグをtrueにする
    }
}