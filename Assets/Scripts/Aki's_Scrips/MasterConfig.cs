using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace MasterConfig{
    public class MasterConfig : MonoBehaviourPunCallbacks
    {
        public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
        public float currentTime;
        public GameObject[] list = {null,null,null,null};
        public GameObject MasterObject;
        public Text Text;
        public static bool GameTimeStartFlg = false;

        //残り人数生やす用
        public Text peopletext;
        
        void Update(){
            if(RandomMatchMaker.GameStartFlg){
                ArraySet();
                RandomMatchMaker.GameStartFlg = false;
                GameTimeStartFlg =true;
            }
            if(GameTimeStartFlg){
                photonView.RPC(nameof(GameStart),RpcTarget.All);
            }
            Numberofpeopleleft();
        }
        void ArraySet(){
            PhotonNetwork.ConnectUsingSettings();//Photonネットワークへの接続処理部分(これがないとフォトンは使用できない)
            photonView.RPC(nameof(SetStageSpawn),RpcTarget.All);
        }

        [PunRPC]
        void SetStageSpawn(){
            GameObject[] Character_Nige = GameObject.FindGameObjectsWithTag("Nigeru");//逃げる側のキャラクターを配列に格納
            GameObject[] Character_Oni = GameObject.FindGameObjectsWithTag("Oni");//鬼側のキャラクターを配列に格納

            Array.Copy(Character_Nige,list,Character_Nige.Length);//逃げる側のキャラクターを配列に格納
            Array.Copy(Character_Oni,0,list,Character_Nige.Length,Character_Oni.Length);//逃げる側のキャラクターを配列に格納
            for (int i = 0; i < 2; i++)
            {
                list[i].transform.position = SpawnPoint[i].transform.position;//キャラクターのスポーンポイントを設定
            }

            Instantiate(MasterObject, new Vector3(0,0,0), Quaternion.identity);//時間を司るオブジェクトを生成
        }

        [PunRPC]
        void GameStart(){
            GameTimeStartFlg =true;
        }
        //残り人数の反映
        private void Numberofpeopleleft(){
            peopletext.text = "残り人数:" + (PhotonNetwork.PlayerList.Length)+"人";
        }
    }
}