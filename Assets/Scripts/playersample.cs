using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
using System;

public class playersample : MonoBehaviourPunCallbacks
{

    [SerializeField] public static int SpawnFlg = 0;
    private GameObject MySpawnPoint;//キャラクターのステージスポーンポイント
    private CapsuleCollider col;
    private Rigidbody rb;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    private Text result_text; //リザルトテキスト
    public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
    public GameObject ResultPanel;//リザルトパネル
    public GameObject GoToTitleButton;//タイトルに戻るボタン
    private GameObject Panels;
    public Canvas Canvas;
    public CinemachineFreeLook camera;
    public float animSpeed = 1.5f;

    float inputHorizontal;
    float inputVertical;
    private float moveSpeed = 5.0f;

    int time;
    int SpawnCnt = 0;
    public float GameTime=120000;//カウントダウンの時間

	int CNT=0;

	float Timen=10f;

    Text Text;
    void Start () {
                anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider> ();
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
		KASU();
		if(RandomMatchMaker.GameStartFlg){
			Timen -= Time.deltaTime;
			Text.text= Timen.ToString();
			if(Timen <= 0){
				Timen = 0;
				Panels.SetActive(true);//パネルを表示
				result_text.text = "You Win!";
				PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
				PhotonNetwork.Disconnect();//自分をサーバーから切断
			}
		}

    }

    void FixedUpdate(){
      if(!photonView.IsMine){
            return;
        }
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
			inputHorizontal = Input.GetAxisRaw("Horizontal");
        	inputVertical = Input.GetAxisRaw("Vertical");
            if(inputHorizontal==0 && inputVertical==0){
                anim.SetFloat ("Speed", 0);//プレイヤーが移動してないときは走るアニメーションを止める
            }else{
                anim.SetFloat ("Speed", 1f);//プレイヤーが移動しているときは走るアニメーションを再生する
            }

            anim.speed = animSpeed;
            currentBaseState = anim.GetCurrentAnimatorStateInfo (0);
			Debug.Log("Fixed");
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
            result_text.text = "Your Lose…";
            PhotonNetwork.Destroy(gameObject);//自分を全体から破棄
            PhotonNetwork.Disconnect();//自分をサーバーから切断
        }
    }
        void KASU(){
            if(!RandomMatchMaker.GameStartFlg){
                return;
            }
            photonView.RPC(nameof(GOMI),RpcTarget.All);
			if(CNT!=0){
                return;
            }
			CNT++;

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
}