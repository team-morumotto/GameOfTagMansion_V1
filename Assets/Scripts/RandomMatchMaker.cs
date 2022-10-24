/*
    2022/10/18
    Atsuki Kobayashi

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
    public GameObject PhotonObject;
    public GameObject SpawnPoint;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//Photonネットワークへの接続処理部分(これがないとフォトンは使用できない)
    }

    //マスターサーバ？に接続した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();//ランダムにルームに接続
    }

    public override void OnJoinedLobby()//ロビーへの入室
    {
        PhotonNetwork.JoinRandomRoom();//ランダムにルームに接続
    }

    public override void OnJoinRandomFailed(short returnCode, string message)//ルームへ参加できなかった
    {
        RoomOptions roomOptions = new RoomOptions();//ルームをインスタンス化
        roomOptions.MaxPlayers = 4;//ルーム接続の最大人数
        PhotonNetwork.CreateRoom(null, roomOptions);//ルームを作成(ルームの名前を指定しない場合はnullを指定)
    }

    public override void OnJoinedRoom()//ルームに参加
    {
        GameObject Player = PhotonNetwork.Instantiate(PhotonObject.name, SpawnPoint.transform.position, Quaternion.identity, 0);//Playerオブジェクトを生成
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");//シーン上のメインカメラを取得
        GameObject CinemachineManager = GameObject.FindWithTag("MainCameraManager");//シーン上のメインカメラマネージャーを取得
        CinemachineManager.GetComponent<Cinemachine.CinemachineFreeLook>().enabled = true;//メインカメラマネージャーのCinemachineFreeLookを有効にする
        CinemachineManager.GetComponent<Cinemachine.CinemachineCollider>().enabled = true;//メインカメラマネージャーのCinemachineColliderを有効にする
        CinemachineFreeLook camera = CinemachineManager.GetComponent<CinemachineFreeLook>();//CinemachineFreeLookコンポーネントを取得
        camera.Follow = Player.transform;//CinemachineFreeLookコンポーネント内のFollowにPlayerオブジェクトのtransformを設定
        camera.LookAt = Player.transform;//CinemachineFreeLookコンポーネント内のLookAtにPlayerオブジェクトのtransformを設定
    }
}