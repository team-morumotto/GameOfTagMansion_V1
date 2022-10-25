using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playersample : MonoBehaviourPunCallbacks
{
    private CapsuleCollider col;
    private AnimatorStateInfo currentBaseState;
    public float animSpeed = 1.5f;
    private Rigidbody rb;
    private Animator anim;
    private GameObject cameraObject;	// メインカメラへの参照

    float inputHorizontal;
    float inputVertical;
    [SerializeField] private float initSpeed = 0.1f;
    private float speed;
    private float moveSpeed = 5.0f;
    private Vector3 rv; //入力値の格納用
    private float v;
    private float h;
    static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash ("Base Layer.Jump");
	static int restState = Animator.StringToHash ("Base Layer.Rest");

    private GameObject MySpawnPoint;//キャラクターのステージスポーンポイント
    void Start () {
        anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider> ();
        speed = initSpeed;
    }

    void Update () {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        if(RandomMatchMaker.CharacterSpawnFlg){
            GameSpawn(); //プレイヤーが移動
        }
    }

    void FixedUpdate(){
        if(photonView.IsMine){
            string aho = PhotonNetwork.CurrentRoom.Name;
            string gomi = PhotonNetwork.LocalPlayer.UserId.ToString();
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
            
            if(inputHorizontal==0 && inputVertical==0){
                anim.SetFloat ("Speed", 0);//プレイヤーが移動してないときは走るアニメーションを止める
            }else{
                anim.SetFloat ("Speed", 1);//プレイヤーが移動しているときは走るアニメーションを再生する
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
        if(col.gameObject.GetComponent<oni_sample>() == true){
            Debug.Log("nenenennenen");
            PhotonNetwork.Disconnect();
        }
    }

    private void GameSpawn(){
        transform.position = MySpawnPoint.transform.position; //スポーンポイントの位置に移動
        transform.rotation = MySpawnPoint.transform.rotation; //スポーンポイントの向きに向く
    }
}
