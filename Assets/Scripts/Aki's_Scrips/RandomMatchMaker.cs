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
    private int Number;										//鬼側か逃げる側かを識別するナンバー
    public static bool GameStartFlg = false;//ゲーム開始フラグ
    public static bool CharacterSpawnFlg =false;
    public static bool kasu = false;
    bool ConnectFlg = true;
    bool DebugMode = false;
    public static string Name;

    void Update() {
        //SetNameスクリプトの名前入力後フラグがtrueになったらConnect関数を実行
        if(GoToChooseChara.OnEndEditFlg && ConnectFlg) {
            ConnectFlg = false;
            Connect();
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4) {
            photonView.RPC(nameof(sinekasu),RpcTarget.All);
        }
    }

	//Photonマスターサーバー接続
    void Connect() {
        if(Input.GetKey(KeyCode.LeftControl)){//レフトコントロールを推していたらデバッグモード
            DebugMode = true;
            Debug.Log("デバッグモード");
        }
        Number = GoToChooseChara.PlayMode;      //GotoGameSceneから鬼か逃げる側かを識別するナンバーを受け取る
        PhotonNetwork.ConnectUsingSettings();	//Photonネットワークへの接続処理部分(これがないとフォトンは使用できない)
    }

    //マスターサーバに接続した時
    public override void OnConnectedToMaster() {
        if(DebugMode){
            PhotonNetwork.JoinRoom(oni_sample.RoomTest); //デバッグルームに接続
        }
        else{
            PhotonNetwork.JoinRandomRoom(); //ランダムにルームに接続
        }
    }

	//ロビーへの入室に失敗した場合
    public override void OnJoinedLobby() {
        if(DebugMode){
            PhotonNetwork.JoinRoom(oni_sample.RoomTest); //デバッグルームに接続
        }
        else{
            PhotonNetwork.JoinRandomRoom(); //ランダムにルームに接続
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message) {
        // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
        RoomOptions roomOptions = new RoomOptions();	         //ルームをインスタンス化
        roomOptions.MaxPlayers = 1;//ルームの最大人数を設定(intをbyteにキャスト)
        PhotonNetwork.CreateRoom(null,roomOptions);              //ルームを作成(ルームの名前を指定しない場合はnullを指定)
    }

	//ルームに参加できなかった場合
    public override void OnJoinRoomFailed(short returnCode, string message) {
        if(DebugMode){
            RoomOptions roomOptions = new RoomOptions();	//ルームをインスタンス化
            roomOptions.IsVisible = false;
            roomOptions.MaxPlayers = 1;//ルームの最大人数を設定(intをbyteにキャスト)

            PhotonNetwork.CreateRoom(oni_sample.RoomTest , roomOptions);	//ルームを作成
        }
        else{
            RoomOptions roomOptions = new RoomOptions();	//ルームをインスタンス化
            roomOptions.MaxPlayers = 1;//ルームの最大人数を設定(intをbyteにキャスト)						//ルーム接続の最大人数

            PhotonNetwork.CreateRoom(null, roomOptions);	//ルームを作成
        }
    }

	//ルームに参加した時
    public override void OnJoinedRoom() {
        Transform mainCamera = GameObject.FindWithTag("MainCamera").transform.Find("CameraCollider").transform;						//シーン上のメインカメラを取得
        GameObject CinemachineManager = GameObject.FindWithTag("MainCameraManager");		//シーン上のメインカメラマネージャーを取得
        CinemachineManager.GetComponent<Cinemachine.CinemachineFreeLook>().enabled = true;	//メインカメラマネージャーのCinemachineFreeLookを有効にする
        CinemachineFreeLook camera = CinemachineManager.GetComponent<CinemachineFreeLook>();//CinemachineFreeLookコンポーネントを取得
        switch(Number) {
            case 0:
                GameObject Player = PhotonNetwork.Instantiate(PlayerObject[GoToChooseChara.Characters].name,SpawnPoint[PhotonNetwork.CurrentRoom.PlayerCount-1].transform.position,Quaternion.identity,0);//Playerオブジェクトを生成
                camera.Follow = Player.transform; //CinemachineFreeLookコンポーネント内のFollowにPlayerオブジェクトを設定
                camera.LookAt = Player.transform.Find("LookAtObject").gameObject.transform; //CinemachineFreeLookコンポーネント内のLookAtにPlayerオブジェクトのLookAtObjectのtransformを設定
                PivotColliderController.m_start = CinemachineManager.transform;//MainCameraManagerのtransformをPivotStartに代入
                PivotColliderController.m_end = Player.transform;//Player(自分)のtransformをPivotEndに代入
                PivotColliderController.PlayerObj = Player;//Player(自分)のオブジェクトをPlayerObjに代入
                break;
            case 1:
                GameObject Oni = PhotonNetwork.Instantiate(OniObject[GoToChooseChara.Characters].name,SpawnPoint[PhotonNetwork.CurrentRoom.PlayerCount-1].transform.position,Quaternion.identity,0);//Oniオブジェクトを生成
                camera.Follow = Oni.transform; //CinemachineFreeLookコンポーネント内のFollowにPlayerオブジェクトを設定
                camera.LookAt = Oni.transform.Find("LookAtObject").gameObject.transform; //CinemachineFreeLookコンポーネント内のLookAtにPlayerオブジェクトのLookAtObjectのtransformを設定
                PivotColliderController.m_start = CinemachineManager.transform;//MainCameraManagerのtransformをPivotStartに代入
                PivotColliderController.m_end = Oni.transform;//Player(自分)のtransformをPivotEndに代入
                PivotColliderController.PlayerObj = Oni;//Player(自分)のオブジェクトをPlayerObjに代入
                break;
        }
		//ルームに入室している人数がルームの最大人数になったら
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            PhotonNetwork.CurrentRoom.IsOpen = false; //ルームを閉める
            PhotonNetwork.CurrentRoom.IsVisible = false; //ルームを非表示にする
            GameStartFlg = true;
        }
    }
    [PunRPC]
    void sinekasu(){
        GameStartFlg = true; //ゲーム開始フラグをtrueにする
    }
}