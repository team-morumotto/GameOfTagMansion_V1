using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;
using System;

public class oni_sample : MonoBehaviourPunCallbacks
{
    //## Unity オブジェクトリスト ##//
    private Text Text;
    private Text result_text; //リザルトテキスト
    private Text catch_text; //捕まったとき用
    public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
    private GameObject Panels;
    public CinemachineFreeLook camera;
    //## Character系の変数 ##//
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    private Rigidbody rb;
    public float speed = 6.25f;//鬼の移動速度
    public static string RoomTest = "Room";
    public float animSpeed = 1.5f;
    private float inputHorizontal;
    private float inputVertical;
    //## ワールド等外部的変数 ##//
    private int SpawnCnt = 0;
    private bool playersetflag = false; //人数ifして必要人数になってたらゲームがスタートしたと認識してtrueになる
    int CNT=0;
    float Timen=60f;//カウントダウンの時間(ローカル時間が引かれるため可変)
    private int isExitCountMax = 10;
    private int isExitCountA = 0;
    private int isExitCountB = 0;
    //#### ここまで変数置き場 ####//
    void Start(){
        isExitCountMax = 10;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GameObject CameraObj = GameObject.FindWithTag("MainCameraManager");
        camera = CameraObj.GetComponent<Cinemachine.CinemachineFreeLook>();//メインカメラマネージャーのCinemachineFreeLookを有効にする
        Panels = GameObject.Find("/Canvas").transform.Find("Result_PanelList").gameObject;
        Text = GameObject.Find("/Canvas").transform.Find("Time").gameObject.GetComponent<Text>();
        result_text = GameObject.Find("/Canvas").transform.Find("Result_PanelList").transform.Find("Result_TextBox").gameObject.GetComponent<Text>();
        catch_text = GameObject.Find("/Canvas").transform.Find("logText").gameObject.GetComponent<Text>();
        SpawnPoint[0] = GameObject.Find("/stage2.1").transform.Find("SpawnPoint").gameObject;
        SpawnPoint[1] = GameObject.Find("/stage2.1").transform.Find("SpawnPoint_01").gameObject;
        SpawnPoint[2] = GameObject.Find("/stage2.1").transform.Find("SpawnPoint_02").gameObject;
        SpawnPoint[3] = GameObject.Find("/stage2.1").transform.Find("SpawnPoint_03").gameObject;
    }

    void Update(){
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        if(!photonView.IsMine){
            return;
        }
        Character_Spawn();
        //ゲーム中かどうか
        if(!RandomMatchMaker.GameStartFlg){
            return;
        }

        Timen -= Time.deltaTime;                           //鬼の残り時間
        Text.text= Mathf.Floor(Timen).ToString();          //stringにキャストしtextに代入
        if(Timen <= 0 && PhotonNetwork.PlayerList.Length > 1){
            result_text.text = "全員捕まえられなかった...";
            Oni_Game_End();
        }
        if(PhotonNetwork.PlayerList.Length==1){
            result_text.text = "全員捕まえられた！";
            Oni_Game_End();
        }
    }
    void Character_Spawn(){
        Debug.Log("ONI"+RandomMatchMaker.GameStartFlg);
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
    void Oni_Game_End(){
        Timen = 0;
        Text.text = (0).ToString();
        Panels.SetActive(true);
        PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
        PhotonNetwork.Disconnect();//ルームから退出
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.GetComponent<PhotonView>() == false){
            return;
        }

        var p = col.gameObject.GetComponent<PhotonView>().Owner.NickName;
        RpcSendMessage(p);

    }

    void RpcSendMessage(string p){
        if(p == PhotonNetwork.NickName){
            return;
        }
        catch_text.enabled = true;
        catch_text.text = p + "を捕まえた！";
        //Debug.Log(p + "を捕まえた！");
        StartCoroutine("textwait",5f);
    }
    //
    IEnumerator textwait(float time)
    {
        yield return new WaitForSeconds(time);
        catch_text.enabled = false;
    }

    void FixedUpdate(){
        if(!photonView.IsMine){
            return;
        }
        PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
        inputHorizontal = Input.GetAxis ("Horizontal");			// 入力デバイスの水平軸をhで定義
        inputVertical = Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
        if(inputHorizontal==0 && inputVertical==0){
            anim.SetFloat ("Speed", 0);//プレイヤーが移動してないときは走るアニメーションを止める
        }else{
            anim.SetFloat ("Speed", 1);//プレイヤーが移動しているときは走るアニメーションを再生する
        }

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
    
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
        float nowspeed;
        if(Timen < 30f){
            nowspeed = speed * 2;
        }
        else{
            nowspeed = speed;
        }
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * nowspeed + new Vector3(0, rb.velocity.y, 0);
    
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }
    void Out_After_Delay(){
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
        isExitCountA += 1;		// 毎フレームカウントアップ
        if(isExitCountA >= 60){	// 60フレーム経過したら
            isExitCountA = 0;	// カウントをリセット
            isExitCountB += 1;	// 1秒経過したことにする
        }
        if(isExitCountB >= isExitCountMax){	// 10秒経過したら
            isExitCountB = 0;				// カウントをリセット
            isExitCountA = 0;				// カウントをリセット
            PhotonNetwork.LeaveRoom();		// ルームから退出
            GotoTitleScene.exeend();		// プログラムを終了する
        }
    }

    [PunRPC]
    void Game_Now_Update(){
        RandomMatchMaker.GameStartFlg = true;
    }

}