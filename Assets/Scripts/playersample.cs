using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
using System;

public class playersample : MonoBehaviourPunCallbacks
{
    private CapsuleCollider col;
    private AnimatorStateInfo currentBaseState;
    public float animSpeed = 1.5f;
    private Rigidbody rb;
    private Animator anim;

    float inputHorizontal;
    float inputVertical;
    [SerializeField] private float initSpeed = 0.1f;
    private float speed;
    private float moveSpeed = 4.0f;

    private GameObject MySpawnPoint;//キャラクターのステージスポーンポイント
    public GameObject ResultPanel;//リザルトパネル
    public GameObject GoToTitleButton;//タイトルに戻るボタン
    public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
    public GameObject[] list = {null,null,null,null};

    public Canvas Canvas;
    private GameObject Panels;
    private Text result_text; //リザルトテキスト
    public CinemachineFreeLook camera;

    int time;
    int SpawnCnt = 0;
    public int GameTime=120000;//カウントダウンの時間
    [SerializeField]
    public static int SpawnFlg = 0;

    Text Text;
    void Start () {
        anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider> ();
        GameObject CameraObj = GameObject.FindWithTag("MainCameraManager");
        camera = CameraObj.GetComponent<Cinemachine.CinemachineFreeLook>();//メインカメラマネージャーのCinemachineFreeLookを有効にする
        speed = initSpeed;
        Panels = GameObject.Find("/Canvas").transform.Find("Result_PanelList").gameObject;
        Text = GameObject.Find("/Canvas").transform.Find("Time").gameObject.GetComponent<Text>();
        result_text = GameObject.Find("/Canvas").transform.Find("Result_PanelList").transform.Find("Result_TextBox").gameObject.GetComponent<Text>();
        SpawnPoint[0] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint").gameObject;
        SpawnPoint[1] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_01").gameObject;
        SpawnPoint[2] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_02").gameObject;
        SpawnPoint[3] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_03").gameObject;
        time = PhotonNetwork.ServerTimestamp;//サーバー時刻を取得
        time += GameTime;//カウントダウンの時間を加算しておく
    }

    void Update () {
        if(!photonView.IsMine){
            return;
        }
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        if(photonView.Owner.ActorNumber == 4){
                photonView.RPC(nameof(GOMI2),RpcTarget.All);
        }
        KASU();
    }

    void FixedUpdate(){
        if(!photonView.IsMine){
            return;
        }
            if(Input.GetMouseButton(0)){
                camera.enabled =true;
            }else{
                camera.enabled =false;
            }
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)

            if(inputHorizontal==0 && inputVertical==0){
                anim.SetFloat ("Speed", 0);//プレイヤーが移動してないときは走るアニメーションを止める
            }else{
                anim.SetFloat ("Speed", 1f);//プレイヤーが移動しているときは走るアニメーションを再生する
            }

            anim.speed = animSpeed;
            currentBaseState = anim.GetCurrentAnimatorStateInfo (0);

            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
    }
    public void OnCollisionEnter(Collision col){

        if(!photonView.IsMine){
            return;
        }
        //捕まったとき
        if(col.gameObject.GetComponent<oni_sample>() == true){//あたったオブジェクトにOni_Sampleがついているかどうか
            Panels.SetActive(true);//パネルを表示
            //textどうにかしたんで確認お願いします
            result_text.text = "Your Lose…";
            PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
            PhotonNetwork.Disconnect();//自分をサーバーから切断
        }
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
            if(time-PhotonNetwork.ServerTimestamp<=0){//時間切れ
                Panels.SetActive(true);//パネルを表示
                result_text.text = "Your Win!";
                PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
                PhotonNetwork.Disconnect();//自分をサーバーから切断
            }
        }
}