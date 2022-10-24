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
    private Vector3 rv; //入力値の格納用
    private float v;
    private float h;
    static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash ("Base Layer.Jump");
	static int restState = Animator.StringToHash ("Base Layer.Rest");

    void Start () {
        anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider> ();
        speed = initSpeed;
        
    }

    void Update () {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate(){
        if(photonView.IsMine){
            string aho = PhotonNetwork.CurrentRoom.Name;
            string gomi = PhotonNetwork.LocalPlayer.UserId.ToString();
            
            anim.SetFloat ("Speed", v);							// Animator側で設定している"Speed"パラメタにvを渡す
            anim.SetFloat ("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
            
            anim.speed = animSpeed;
            currentBaseState = anim.GetCurrentAnimatorStateInfo (0);


            
            rv = new Vector3();        
            
            v = 0;h = 0;
            var dn = new Vector3();
            //WASD押したら方向を代入
            if (Input.GetKey(KeyCode.A)) {
                    rv.x = -speed;
                    v = 1;
                    dn += transform.forward * speed;
                }
            if (Input.GetKey(KeyCode.D)) {
                    rv.x = speed;
                    v = 1;
                    dn += transform.forward * speed;
                }
            if (Input.GetKey(KeyCode.W)) {
                    rv.z = speed;
                    v = 1;
                    dn += transform.forward * speed;
                }
            if (Input.GetKey(KeyCode.S)) {
                    rv.z = -speed;
                    v = 1;
                    //transform.position += transform.forward * speed;
                    dn += transform.forward * speed;
                }
            
            //カメラのxとzのベクトルを抽出
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
    
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
    
            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rb.velocity = moveForward * speed + new Vector3(0, rb.velocity.y, 0);
    
            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }

            // IDLE中の処理
            // 現在のベースレイヤーがidleStateの時
            else if (currentBaseState.nameHash == idleState) {
                    // スペースキーを入力したらRest状態になる
                    if (Input.GetButtonDown ("Jump")) {
                        anim.SetBool ("Rest", true);
                    }
                }
            // REST中の処理
            // 現在のベースレイヤーがrestStateの時
            else if (currentBaseState.nameHash == restState) {
                //cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
                // ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
                if (!anim.IsInTransition (0)) {
                    anim.SetBool ("Rest", false);
                }
            }
        }
    }
}
