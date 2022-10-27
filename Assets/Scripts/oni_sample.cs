using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class oni_sample : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    public float speed = 5f;
    private float inputHorizontal;
    private float inputVertical;
    private Animator anim;
    private Text Text;
    private float time;
    public int GameTime=120000;//カウントダウンの時間
    private int SpawnCnt = 0;
    public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
    public GameObject[] list = {null,null,null,null};

    //プレイヤーキル関連
    [SerializeField] private int PlayerPeople = 2; //プレイヤー人数なんで人数変わったら変えろ
    private bool playersetflag = false; //人数ifして必要人数になってたらゲームがスタートしたと認識してtrueになる
    private GameObject Panels;
    
    void Start(){
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Panels = GameObject.Find("/Canvas").transform.Find("Result_PanelList").gameObject;
        Text = GameObject.Find("/Canvas").transform.Find("Time").gameObject.GetComponent<Text>();//編集:aki
        SpawnPoint[0] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint").gameObject;
        SpawnPoint[1] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_01").gameObject;
        SpawnPoint[2] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_02").gameObject;
        SpawnPoint[3] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_03").gameObject;
        
    }

    void Update(){
        if(!photonView.IsMine){
            return;
        }
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
            inputHorizontal = Input.GetAxis ("Horizontal");				// 入力デバイスの水平軸をhで定義
            inputVertical = Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
            if(photonView.Owner.ActorNumber == 4){
                photonView.RPC(nameof(GOMI2),RpcTarget.All);
            }
            KASU();
            //rb.velocity = new Vector3(h, rb.velocity.y, v);

        //最大人数になったのでゲームがスタートしたと認識する
        if(PhotonNetwork.PlayerList.Length == PlayerPeople&&playersetflag == false){
            playersetflag = true;
        }
        //自分以外たおしたら
        if(playersetflag == true&&PhotonNetwork.PlayerList.Length == 1){
            kill_every_survivor();
        }
    }
    void FixedUpdate(){
        if(!photonView.IsMine){
            return;
        }
        if(inputHorizontal==0 && inputVertical==0){
                anim.SetFloat ("Speed", 0);//プレイヤーが移動してないときは走るアニメーションを止める
            }else{
                anim.SetFloat ("Speed", 1);//プレイヤーが移動しているときは走るアニメーションを再生する
            }
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
    
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
    
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * speed + new Vector3(0, rb.velocity.y, 0);
    
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }
    void OnCollisionEnter(Collision col){
        if(col.gameObject.GetComponent<PhotonView>() == false){
            return;
        }
        //photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, p);
    }

    /*[PunRPC]
    private void RpcSendMessage(string message) {
        Debug.Log(message);
    }*/
    private void kill_every_survivor(){
        //勝ったからパネル出す(おおむね処理はプレイヤーからもってきました)
        Panels.SetActive(true);
        //result_text.text = "Your Lose…";
        PhotonNetwork.Destroy(gameObject);
        PhotonNetwork.Disconnect();
        }

        void KASU(){
            if(SpawnCnt!=0){
                return;
            }
            if(!RandomMatchMaker.GameStartFlg){
                return;
            }
            photonView.RPC(nameof(GOMI),RpcTarget.All);
            time = PhotonNetwork.ServerTimestamp;//サーバー時刻を取得
            time += GameTime;//カウントダウンの時間を加算しておく
            SpawnCnt++;
            var actor = photonView.Owner.ActorNumber;
            switch(actor){
                case 1:
                transform.position = SpawnPoint[0].transform.position;
                break;
                case 2:
                transform.position = SpawnPoint[1].transform.position;
                break;
                case 3:
                transform.position = SpawnPoint[2].transform.position;
                break;
                case 4:
                transform.position = SpawnPoint[3].transform.position;
                break;
            }
        }

        [PunRPC]
        void GOMI(){
            RandomMatchMaker.GameStartFlg = true;
        }

        [PunRPC]
        void GOMI2(){
            Text.text = ((time-PhotonNetwork.ServerTimestamp)/1000).ToString();//残り時間の計算と表示(サーバー時刻と連動)
        }
}