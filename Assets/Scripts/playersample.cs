using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;

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
    private float moveSpeed = 5.0f;

    private GameObject MySpawnPoint;//キャラクターのステージスポーンポイント
    public GameObject ResultPanel;//リザルトパネル
    public GameObject GoToTitleButton;//タイトルに戻るボタン
    public Canvas Canvas;
    private GameObject Panels;
    private Text result_text; //リザルトテキスト
    public CinemachineFreeLook camera;
    

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
    }

    void Update () {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate(){
        if(photonView.IsMine){
            if(Input.GetMouseButton(0)){
                camera.enabled =true;
            }else{
                camera.enabled =false;
            }
            string aho = PhotonNetwork.CurrentRoom.Name;
            string gomi = PhotonNetwork.LocalPlayer.UserId.ToString();
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)

            if(inputHorizontal==0 && inputVertical==0){
                anim.SetFloat ("Speed", 0);//プレイヤーが移動してないときは走るアニメーションを止める
            }else{
                anim.SetFloat ("Speed", 0.8f);//プレイヤーが移動しているときは走るアニメーションを再生する
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
    }
    public void OnCollisionEnter(Collision col){

        if(!photonView.IsMine){
            return;
        }
        //捕まったとき
        if(col.gameObject.GetComponent<oni_sample>() == true){
            Panels.SetActive(true);
            //textどうにかしたんで確認お願いします
            result_text.text = "Your Lose…";
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Disconnect();
        }
    }
}