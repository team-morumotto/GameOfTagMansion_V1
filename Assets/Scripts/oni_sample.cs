using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class oni_sample : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    public float speed = 5f;
    private float inputHorizontal;
    private float inputVertical;
    private Animator anim;

    //プレイヤーキル関連
    [SerializeField] private int PlayerPeople = 2; //プレイヤー人数なんで人数変わったら変えろ
    private bool playersetflag = false; //人数ifして必要人数になってたらゲームがスタートしたと認識してtrueになる
    private GameObject Panels;
    void Start(){
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Panels = GameObject.Find("/Canvas").transform.Find("Result_PanelList").gameObject;
    }

    void Update(){
        if(!photonView.IsMine){
            return;
        }
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
            inputHorizontal = Input.GetAxis ("Horizontal");				// 入力デバイスの水平軸をhで定義
            inputVertical = Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
            
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
}
