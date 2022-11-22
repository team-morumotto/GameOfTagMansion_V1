using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class playersample : MonoBehaviourPunCallbacks
{
    //## Unity オブジェクトリスト ##//
    Text Text;
    private Text result_text; //リザルトテキスト
    private GameObject Panels;
    public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
    public GameObject[] ItamSpawnPoint;//アイテムのステージスポーンポイント
    public CinemachineFreeLook CameraFreeLook; //カメラのFreeLook
    public CinemachineCollider CameraCollider; //カメラのFreeLook
    public AudioClip[] SE;
    //## Character系の変数 ##//
    private Rigidbody rb;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    public float animSpeed = 5f;
    float inputHorizontal;
    float inputVertical;
    private float SpeedUpTime;//アイテム取得時のスピードアップ時間
    public static float moveSpeed = 5.0f;//移動速度
    bool GUIFlg = false;//GUIを表示する間のフラグ
    bool CoroutineFlg = true;//コルーチンを一度だけ実行するためのフラグ
    bool CountFlg = true;//カウントダウン中は操作を受け付けないようにするためのフラグ
    GUIStyle style;//GUIのスタイル

    //## ワールド等外部的変数 ##//
	bool SpawnFlg = true;
    public int Nokori_Player=3;
    private float ItemSpawnTime;
    private int isExitCountMax = 10;	// Exitカウントの待機秒数
    private int isExitCountA = 0;		// Exitカウントの秒
    private int isExitCountB = 0;		// Exitカウントのミリ秒(60ms基準)
    private float isTimeMaster = 0.0f;	// MasterConfigから同期するための変数
    private int isTimeCountA = 0;		// 時計の秒カウント
    private int isTimeCountB = 0;		// 時計の分カウント
    private int isTimeCountC = 0;		// 時計のミリ秒カウント(1000ms基準)
    [SerializeField] ParticleSystem particleSystem; // パーティクルシステムを取得
    public static bool isPlayerLose = false; // ゲーム終了フラグ
    int GameStartCount = 5;//メンバーが揃ってからゲーム開始までのカウント(初期値は5秒)
    public AudioClip CountDownClip;//カウントダウンのジングル
    AudioSource audio;//オーディオソース

    //#### ここまで変数置き場 ####//



    void Start () {
        // カウント系の処理
        isExitCountMax = 10;								                // Exitカウントの最大秒数
        isTimeMaster = MasterConfig.GameTimer;				                // ゲームの時間をMasterConfigから同期
        isTimeCountA = Mathf.FloorToInt(isTimeMaster) / 60 - 1;	            // 時計の秒カウントを設定
        isTimeCountB = Mathf.FloorToInt(isTimeMaster) - 60 * (isTimeCountA);// 時計の分カウントを設定

        anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        //-----------------CinemaChineManagerオブジェクトを取得--------------------//
        GameObject CameraObj = GameObject.FindWithTag("MainCameraManager");
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");

        Panels = GameObject.Find("/Canvas").transform.Find("Result_PanelList").gameObject;
        Text = GameObject.Find("/Canvas").transform.Find("Time").gameObject.GetComponent<Text>();

        result_text = GameObject.Find("/Canvas").transform.Find("Result_PanelList").transform.Find("Result_TextBox").gameObject.GetComponent<Text>();
        SpawnPoint[0] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint").gameObject;
        SpawnPoint[1] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint_01").gameObject;
        SpawnPoint[2] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint_02").gameObject;
        SpawnPoint[3] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint_03").gameObject;

        ItamSpawnPoint[0] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("ItemSpawnPoint").gameObject;
        ItamSpawnPoint[1] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("ItemSpawnPoint_01").gameObject;
        ItamSpawnPoint[2] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("ItemSpawnPoint_02").gameObject;
        ItamSpawnPoint[3] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("ItemSpawnPoint_03").gameObject;

        particleSystem = mainCamera.transform.Find("Particle System").gameObject.GetComponent<ParticleSystem>();

        audio = GetComponent<AudioSource>();

        style = new GUIStyle();
        style.fontSize = 300;
        style.normal.textColor = Color.white;
    }

    void Update () {
        if(!photonView.IsMine){
            return;
        }
        PhotonNetwork.LocalPlayer.NickName = $"{"Player"}({photonView.Owner.ActorNumber})";
		Character_Spawn();

        //ゲーム中かどうか
        if(!RandomMatchMaker.GameStartFlg){
            return;
        }

        if(CoroutineFlg){
            CoroutineFlg = false;
            StartCoroutine("GameStart");
        }

        photonView.RPC(nameof(Game_Now_Update), RpcTarget.All);
        if(!CountFlg){
            return;
        }

        //アイテムスポーン時間
        ItemSpawnTime += Time.deltaTime;
        if(ItemSpawnTime >= 10.0f){
            ItemSpawnTime = 0.0f;
            ItemSpawn();
        }
        
         //ステージ外に落ちたときy座標が-100以下になったら自分のスパーン位置に戻る
        if(gameObject.transform.position.y <= -100f){
            SpawnFlg = true;
        }
        Player_Win();//プレイヤーが勝つための関数
    }

    void Character_Spawn(){
        if(!SpawnFlg){
            return;
        }

        if(!RandomMatchMaker.GameStartFlg){
            return;
        }

        SpawnFlg = false;//以下の関数内の処理を一回だけ行うための処理

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

    //プレイヤーが勝利した場合の処理
    void Player_Win(){
        isTimeCount(Mathf.FloorToInt(isTimeMaster)); // 時間カウント関数
        if(isTimeMaster <= 0){                                    //残り時間0秒を下回ると
            isTimeMaster = 0;                                     //isTimeMasterに0を代入
            Text.text =("00:00.000").ToString();                     //残り時間を0に上書きし表示
            Panels.SetActive(true);                        //パネルを表示
            result_text.text = "逃げ切った！";
            PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
            PhotonNetwork.Disconnect();//ルームから退出
            isExit();
        }
    }
    void OnCollisionEnter(Collision collision){
        if(!photonView.IsMine){
            return;
        }
        if(collision.gameObject.GetComponent<oni_sample>()){
            Player_Lose();
        }
    }

    void OnTriggerEnter(Collider other){
        if(!photonView.IsMine){
            return;
        }
        if(other.gameObject.tag == "Item"){
            moveSpeed = 10.0f;
            PhotonNetwork.Destroy(other.gameObject);
            OutSE(0);
        }
    }

    //プレイヤーが敗北した場合の処理
    public void Player_Lose() {
        //捕まったとき
        Text.text = ("00:00.000").ToString();
        Panels.SetActive(true);                             //パネルを表示
        result_text.text = "捕まった！！…";
        PhotonNetwork.Destroy(gameObject);                  //自分を全体から破棄
        PhotonNetwork.Disconnect();                         //ルームから退出
        //Invoke("Out_After_Delay", 3.0f);                  //3秒後にOut_After_Delay関数を呼び出す
        isExit();
    }

    void FixedUpdate(){
        if(!photonView.IsMine){
            return;
        }
        if(!CountFlg){
            return;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal");  //横方向の値を入力
        inputVertical = Input.GetAxisRaw("Vertical");      //縦方向の値を入力
        if(inputHorizontal==0 && inputVertical==0){
            anim.SetFloat ("Speed", 0f);                   //プレイヤーが移動してないときは走るアニメーションを止める
        }else{
            if(moveSpeed == 10.0f){
                particleSystem.Play();                         //パーティクルシステムをスタート
                anim.SetFloat("AnimSpeed", 1.5f);
            }else{
                particleSystem.Stop();                         //パーティクルシステムをストップ
                anim.SetFloat("AnimSpeed", 1.0f);
            }
            anim.SetFloat ("Speed", 1.0f);                     //プレイヤーが移動しているときは走るアニメーションを再生する
        }

        if(moveSpeed == 10.0f){
               SpeedUp();                                     //スピードアップの時間を計測し、一定時間を超えたらスピードを戻す.
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

    [PunRPC]
    void Catch_RPC(string p,GameObject g){
        if(!RandomMatchMaker.GameStartFlg){
            return;
        }
        if(p==PhotonNetwork.NickName){
            g.GetComponent<playersample>().Player_Lose();
        }
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

    /// <summary> スピードアップの時間を計測し、一定時間を超えたらスピードを戻す </summary>
    void SpeedUp(){
        SpeedUpTime += Time.deltaTime;
        if(SpeedUpTime >= 5.0f){
            moveSpeed = 5.0f;
            SpeedUpTime = 0.0f;
        }
    }

    /// <summary>  暫定処理として、10秒経過後に強制的にプログラムを終了する </summary>
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

    /// <summary> 自分がマスタークライアントならば、指定の座標にアイテムを出現させる </summary>
    void ItemSpawn(){
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            for(int i=0; i<4; i++){
                PhotonNetwork.Instantiate("Item",ItamSpawnPoint[i].transform.position,Quaternion.identity);
            }
        }
    }

    /* 処理内容
        isTimeCountA = Mathf.FloorToInt(isTimeMaster) / 60;
        isTimeCountB = Mathf.FloorToInt(isTimeMaster) - (60 * isExitCountA);
        isTimeCountC = isTimeMaster - Mathf.FloorToInt(isTimeMaster) * 100; //小数点以下を切り捨て
    */

    // README この順番は計算されつくされた配置です。変更しないでください。
    // ※IntTimeMasterはIntに変換したisTimeMasterです。省略のために使用しています
    void isTimeCount(int IntTimeMaster) {
        isTimeMaster -= Time.deltaTime; //鬼の残り時間を計算

        // 秒数カウントダウン
		if(IntTimeMaster % 60 == 0) isTimeCountB = 0;				// 60秒のときだけ0を表示する
        else isTimeCountB = IntTimeMaster - (60 * (isTimeCountA));	// 60秒未満のときは残り秒数を表示する

        // 分数カウントダウン
        if(IntTimeMaster % 60 == 0 && isTimeCountA == 0) isTimeCountA = 0;		// 1分減算を適用しない置換処理を回す
		else if(IntTimeMaster % 60 == 0) isTimeCountA = IntTimeMaster / 60 - 1;	// 1分減算を適用する置換処理を回す

        // ミリ秒カウントダウン
        float tmp = 0.0f;
        if(isTimeCountB == 0 && isTimeMaster > 0) tmp = (isTimeMaster - (60 + (60 * (isTimeCountA)))) * 1000;	// 秒数が0かつまだ時間が残っている場合毎フレームごとに60秒+αで小数点以下を切り捨て
        else tmp = (isTimeMaster - (isTimeCountB + (60 * (isTimeCountA)))) * 1000;								// 秒数が0でないとき毎フレームごとに残り時間で小数点以下を切り捨て
        isTimeCountC = Mathf.FloorToInt(tmp);																	// 小数点以下を切り捨て
        if(isTimeCountC <= 0) isTimeCountC = 0;																	// マイナスカウントは避けたいので小数点以下が0以下のときは強制的に0で上書き表示する

        // 最後にstringにキャストしtextに代入
        Text.text = (isTimeCountA).ToString("00") + ":" + (isTimeCountB).ToString("00") + "." + (isTimeCountC).ToString("000");
    }

    [PunRPC]
    void Game_Now_Update(){
        RandomMatchMaker.GameStartFlg = true;
    }

    /// <summary> 5秒間待ってゲームを開始する </summary>
    IEnumerator GameStart() {
        audio.PlayOneShot(CountDownClip); //カウントダウンの音を鳴らす
        CountFlg = false;
        for(GameStartCount = 5; GameStartCount > 0; GameStartCount--) {
            GUIFlg = true;
            yield return new WaitForSeconds(1);
        }
        
        CountFlg = true;
        GUIFlg = false;
        var BGMObject = GameObject.Find("BGM");
        BGMObject.GetComponent<AudioSource>().Play();
    }

    private void OnGUI() {
        if(GUIFlg){
            GUI.Label(new Rect(1740, 1080, 1000, 1000), GameStartCount.ToString(), style);
        }else{
            GUI.Label(new Rect(1740, 1080, 100, 20), "");
        }
    }
    public void OutSE(int SEnumber){
        audio.PlayOneShot(SE[SEnumber]);
    }
}