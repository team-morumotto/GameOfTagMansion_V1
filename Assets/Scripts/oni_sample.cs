using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

public class oni_sample : MonoBehaviourPunCallbacks
{
    //## Unity オブジェクトリスト ##//
    private Text Text;
    private Text result_text; //リザルトテキスト
    public Text catch_text; //捕まったとき用
    public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
    public GameObject[] ItamSpawnPoint;//アイテムのステージスポーンポイント
    private GameObject Panels;
    //SE鳴らす用関連
    public AudioClip[] SE;
    public AudioSource audioSource;
    //## Character系の変数 ##//
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    private Rigidbody rb;
    [SerializeField] ParticleSystem ps; // パーティクルシステムを取得
    public static float speed = 6.25f;//鬼の移動速度
    public static string RoomTest = "Room";
    public float animSpeed = 1.5f;
    private float inputHorizontal;
    private float inputVertical;
    private float SpeedUpTime;//アイテム取得時のスピードアップ時間
    //## ワールド等外部的変数 ##//
    private int SpawnCnt = 0;
    private float ItemSpawnTime;//アイテムスポーン時間
    private bool playersetflag = false; //人数ifして必要人数になってたらゲームがスタートしたと認識してtrueになる
    bool SpawnFlg = true;
    private int isExitCountMax = 10;	// Exitカウントの待機秒数
    private int isExitCountA = 0;		// Exitカウントの秒
    private int isExitCountB = 0;		// Exitカウントのミリ秒(60ms基準)
    private float isTimeMaster = 0.0f;	// MasterConfigから同期するための変数
    private int isTimeCountA = 0;		// 時計の秒カウント
    private int isTimeCountB = 0;		// 時計の分カウント
    private int isTimeCountC = 0;		// 時計のミリ秒カウント(1000ms基準)
    private bool isSpeedUpStart = false;// スピードアップ開始フラグ
    [SerializeField] ParticleSystem particleSystem; // パーティクルシステムを取得
    
    //#### ここまで変数置き場 ####//
     int GameStartCount = 5;//メンバーが揃ってからゲーム開始までのカウント(初期値は5秒)
    GUIStyle style;//GUIのスタイル
    public AudioClip CountDownClip;
    AudioSource audio;
    bool GUIFlg = false;
    bool CoroutineFlg = true;
    bool CountFlg = true;
    void Start(){
        // カウント系の処理
        isExitCountMax = 10;								                // Exitカウントの最大秒数
        isTimeMaster = MasterConfig.GameTimer;				                // ゲームの時間をMasterConfigから同期
        isTimeCountA = Mathf.FloorToInt(isTimeMaster) / 60 - 1;	            // 時計の秒カウントを設定
        isTimeCountB = Mathf.FloorToInt(isTimeMaster) - 60 * (isTimeCountA);// 時計の分カウントを設定

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GameObject CameraObj = GameObject.FindWithTag("MainCameraManager");
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");

        Panels = GameObject.Find("/Canvas").transform.Find("Result_PanelList").gameObject;
        Text = GameObject.Find("/Canvas").transform.Find("Time").gameObject.GetComponent<Text>();

        result_text = GameObject.Find("/Canvas").transform.Find("Result_PanelList").transform.Find("Result_TextBox").gameObject.GetComponent<Text>();
        catch_text = GameObject.Find("/Canvas").transform.Find("logText").gameObject.GetComponent<Text>();

         SpawnPoint[0] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint").gameObject;
        SpawnPoint[1] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint_01").gameObject;
        SpawnPoint[2] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint_02").gameObject;
        SpawnPoint[3] = GameObject.Find(MasterConfig.SpawnWorld).transform.Find("SpawnPoint_03").gameObject;
        audioSource = GetComponent<AudioSource>(); //SE鳴らすために取得

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

    void Update(){
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

        isTimeCount(Mathf.FloorToInt(isTimeMaster)); // 時間カウント関数

        if(isTimeMaster <= 0 && PhotonNetwork.PlayerList.Length > 1){
            result_text.text = "全員捕まえられなかった...";
            Oni_Game_End();
        }
        if(PhotonNetwork.PlayerList.Length==1 && RandomMatchMaker.GameStartFlg){
            result_text.text = "全員捕まえられた！";
            Oni_Game_End();
        }
    }

    void Character_Spawn(){
        if(!RandomMatchMaker.GameStartFlg){
            return;
        }

        if(!SpawnFlg){
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
    void Oni_Game_End(){
        isTimeMaster = 0;
        Text.text = (0).ToString();
        Panels.SetActive(true);
        PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
        PhotonNetwork.Disconnect();//ルームから退出
    }

    void OnCollisionEnter(Collision col){
        if(!photonView.IsMine){
            return;
        }
        if(!RandomMatchMaker.GameStartFlg){//ゲームスタート後じゃないと動かない
            return;
        }

        if(col.gameObject.GetComponent<PhotonView>() == false){
            return;
        }

        var p = col.gameObject.GetComponent<PhotonView>().Owner.NickName;
        if(p == PhotonNetwork.NickName){
            return;
        }
        photonView.RPC(nameof(Catch_RPC), RpcTarget.All, p,col);
        catch_text.enabled = true;
        catch_text.text = p + "を捕まえた！";
        audioSource.PlayOneShot(SE[0]);
        StartCoroutine("textwait",5f);
    }

    void OnTriggerEnter(Collider other){
        if(!photonView.IsMine){
            return;
        }
        if(other.gameObject.tag == "Item"){
            speed = 13.0f;
            OutSE(1);
        }
    }

    //五秒たったらテキストを非表示にする
    IEnumerator textwait(float time)
    {
        yield return new WaitForSeconds(time);
        catch_text.enabled = false;
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

    void FixedUpdate(){
        if(!photonView.IsMine){
            return;
        }
        if(!CountFlg){
            return;
        }

        inputHorizontal = Input.GetAxis ("Horizontal");			// 入力デバイスの水平軸をhで定義
        inputVertical = Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
        if(inputHorizontal==0 && inputVertical==0){
            anim.SetFloat ("Speed", 0f);                      //プレイヤーが移動してないときは走るアニメーションを止める
        }else{
            if(speed > 10.0f){
                particleSystem.Play(); //パーティクルシステムをスタート
                anim.SetFloat("AnimSpeed", 1.5f);
            }else{
                particleSystem.Stop(); //パーティクルシステムをストップ
                anim.SetFloat("AnimSpeed", 1.0f);
            }
            anim.SetFloat ("Speed", 1f);                     //プレイヤーが移動しているときは走るアニメーションを再生する
        }

        if(speed > 10.0f){
            SpeedUp();//スピードアップの時間を計測し、一定時間を超えたらスピードを戻す.
        }

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        //時間が30秒以下の時に速度を上げる
        float nowspeed;
        //初回
        if(isTimeMaster < 30f&&isSpeedUpStart == false){
            nowspeed = speed * 1.3f;
            ps = GameObject.Find("Particle System").GetComponent<ParticleSystem>();
            ps.transform.localPosition = this.transform.position;
            isSpeedUpStart = true;
        }
        //初回以外
        else if(isSpeedUpStart == true){
            nowspeed = speed * 1.3f;
            ps.transform.localPosition = this.transform.position;
            ps.transform.localRotation = this.transform.localRotation;
        }
        //通常の場合
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
    void ItemSpawn(){
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            for(int i=0; i<4; i++){
                PhotonNetwork.Instantiate("Item",ItamSpawnPoint[i].transform.position,Quaternion.identity);
            }
        }
    }

    //スピードアップの時間を計測し、一定時間を超えたらスピードを戻す.
    void SpeedUp(){
        SpeedUpTime += Time.deltaTime;
        if(SpeedUpTime >= 5.0f){
            speed = 6.25f;
            SpeedUpTime = 0.0f;
        }
    }

    //プレイヤーを捕まえた時の処理
    void Player_Catch(GameObject Player){
        PhotonNetwork.Destroy(Player);
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