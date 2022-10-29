using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class playersample : MonoBehaviourPunCallbacks
{
    //## Unity オブジェクトリスト ##//
    private Text result_text; //リザルトテキスト
    private GameObject Panels;
    public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
    public CinemachineFreeLook camera;

    //## Character系の変数 ##//
    private Rigidbody rb;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    public float animSpeed = 1.5f;
    float inputHorizontal;
    float inputVertical;
    private float moveSpeed = 5.0f;

    //## ワールド等外部的変数 ##//
	int CNT=0;
	float Timen=60f;
    public int Nokori_Player=3;
    private int isExitCountMax = 10;
    private int isExitCountA = 0;
    private int isExitCountB = 0;
    //#### ここまで変数置き場 ####//
    Text Text;
    void Start () {
        isExitCountMax = 10;
        anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        GameObject CameraObj = GameObject.FindWithTag("MainCameraManager");
        camera = CameraObj.GetComponent<Cinemachine.CinemachineFreeLook>();//メインカメラマネージャーのCinemachineFreeLookを有効にする
        Panels = GameObject.Find("/Canvas").transform.Find("Result_PanelList").gameObject;
        Text = GameObject.Find("/Canvas").transform.Find("Time").gameObject.GetComponent<Text>();
        result_text = GameObject.Find("/Canvas").transform.Find("Result_PanelList").transform.Find("Result_TextBox").gameObject.GetComponent<Text>();
        SpawnPoint[0] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint").gameObject;
        SpawnPoint[1] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_01").gameObject;
        SpawnPoint[2] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_02").gameObject;
        SpawnPoint[3] = GameObject.Find("/stage2.0").transform.Find("SpawnPoint_03").gameObject;
    }

    void Update () {
        if(!photonView.IsMine){
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            //ビルドの場合
            #else
                Application.Quit();//ゲームプレイ終了
            //なんかあったとき
            #endif
        }
		Character_Spawn();
        Player_Win();//プレイヤーが勝つための関数
    }
    
    void Character_Spawn(){
        if(!RandomMatchMaker.GameStartFlg){
            return;
        }
        photonView.RPC(nameof(Game_Now_Update),RpcTarget.All);
        if(CNT!=0){
            return;
        }
        CNT++;//以下の関数内の処理を一回だけ行うための処理

        var actor = photonView.Owner.ActorNumber;//ルームに入ってきたプレイヤーの入室順番号を入手
        switch(actor){//各プレイヤーの入室順番号によってスポーンポイントを変更
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

    void Player_Win(){
        if(!RandomMatchMaker.GameStartFlg){
            return;
        }
        Timen -= Time.deltaTime;              //残り時間のカウントダウン
        Text.text= Mathf.Floor(Timen).ToString();          //stringにキャストしtextに代入
        if(Timen <= 0){                       //残り時間0秒を下回ると
            Timen = 0;                        //Timenに0を代入
            Text.text =(0).ToString();          //残り時間を0に上書きし表示
            Panels.SetActive(true);           //パネルを表示
            result_text.text = "You Win!";
            PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
            PhotonNetwork.Disconnect();//ルームから退出
            isExit();
        }
    }
    void Player_Lose(Collision col) {
        //捕まったとき
        if(col.gameObject.GetComponent<oni_sample>() == true){  //あたったオブジェクトにOni_Sampleがついているかどうか
            Text.text = 0.ToString();
            Panels.SetActive(true);                             //パネルを表示
            result_text.text = "Your Lose…";
            PhotonNetwork.Destroy(gameObject);                  //自分を全体から破棄
            PhotonNetwork.Disconnect();//ルームから退出
            //Invoke("Out_After_Delay", 3.0f);                    //3秒後にOut_After_Delay関数を呼び出す
            isExit();
        }
    }

    void FixedUpdate(){
        if(!photonView.IsMine){
            return;
        }
        PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
        inputHorizontal = Input.GetAxisRaw("Horizontal");    //横方向の値を入力
        inputVertical = Input.GetAxisRaw("Vertical");        //縦方向の値を入力
        if(inputHorizontal==0 && inputVertical==0){
            anim.SetFloat ("Speed", 0);                      //プレイヤーが移動してないときは走るアニメーションを止める
        }else{
            anim.SetFloat ("Speed", 1f);                     //プレイヤーが移動しているときは走るアニメーションを再生する
        }

        anim.speed = animSpeed;
        currentBaseState = anim.GetCurrentAnimatorStateInfo (0);
        //Debug.Log("Fixed");
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
        Player_Lose(col);//触れた対象のコライダー情報を引数に渡す
    }

    void OutAfterDelay(){
        //エディタの場合
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        //ビルドの場合
        #else
            Application.Quit();//ゲームプレイ終了
        //なんかあったとき
        #endif
    }

    // 暫定処理として、10秒経過後に強制的にプログラムを終了する
    void isExit() {
        isExitCountA += 1;      // 毎フレームカウントアップ
        if(isExitCountA >= 60){ // 60フレーム経過したら
            isExitCountA = 0;   // カウントをリセット
            isExitCountB += 1;  // 1秒経過したことにする
        }
        if(isExitCountB >= isExitCountMax){ // 10秒経過したら
            isExitCountB = 0;               // カウントをリセット
            isExitCountA = 0;               // カウントをリセット
            PhotonNetwork.LeaveRoom();      // ルームから退出
            GotoTitleScene.exeend();		// プログラムを終了する
        }
    }

    [PunRPC]
    void Game_Now_Update(){
        RandomMatchMaker.GameStartFlg = true;
    }
}