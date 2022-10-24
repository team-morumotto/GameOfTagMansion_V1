//
// Unityちゃん用の三人称カメラ
//
// 2013/06/07 N.Kobyasahi
//


/*#################ここはGitHubで共有しているプロジェクトのスクリプト####################*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace UnityChan
{
	public class ThirdPersonCamera : MonoBehaviourPunCallbacks
	{
		public float smooth = 3f;		// カメラモーションのスムーズ化用変数
		Transform standardPos;			// the usual position for the camera, specified by a transform in the game
		Transform frontPos;			// Front Camera locater
		Transform jumpPos;			// Jump Camera locater

		// スムーズに繋がない時（クイック切り替え）用のブーリアンフラグ
		bool bQuickSwitch = false;	//Change Camera Position Quickly


		private GameObject targetObj;
		private Vector3 targetPos;
		[SerializeField]private float rotatespeed = 400f;

		void Start ()
		{
			// 各参照の初期化
			standardPos = GameObject.Find ("CamPos").transform;

			if (GameObject.Find ("FrontPos"))
				frontPos = GameObject.Find ("FrontPos").transform;

			if (GameObject.Find ("JumpPos"))
				jumpPos = GameObject.Find ("JumpPos").transform;

			//カメラをスタートする
			transform.position = standardPos.position;
			transform.forward = standardPos.forward;
		}

		void FixedUpdate ()	// このカメラ切り替えはFixedUpdate()内でないと正常に動かない
		{

			/*if (Input.GetButton ("Fire1")) {	// left Ctlr
				// Change Front Camera
				setCameraPositionFrontView ();
			} else if (Input.GetButton ("Fire2")) {	//Alt
				// Change Jump Camera
				setCameraPositionJumpView ();
			} else {
				// return the camera to standard position and direction
				setCameraPositionNormalView ();
			}*/

			if(targetObj == null){
                if(GameObject.Find("UCAkiTest(Clone)")) targetObj = GameObject.Find("UCAkiTest(Clone)");
                else if(GameObject.Find("NoranekoSeven_Variant(Clone)")) targetObj = GameObject.Find("NoranekoSeven_Variant(Clone)");
                else if(GameObject.Find("水鏡こよみ_Variant(Clone)")) targetObj = GameObject.Find("水鏡こよみ_Variant(Clone)");
                else if(GameObject.Find("UCTest(Clone)")) targetObj = GameObject.Find("UCTest(Clone)");
				else if(GameObject.Find("UCTest_DB(Clone)")) targetObj = GameObject.Find("UCTest_DB(Clone)");
                //targetObj = GameObject.Find("UnityChan Variant(Clone)");
                if(PhotonNetwork.LocalPlayer.IsLocal) targetPos = targetObj.transform.position;
                //initCameraposition = transform.position;
            }
            // targetの移動量分、自分（カメラ）も移動する
            transform.position += targetObj.transform.position - targetPos;
            targetPos = targetObj.transform.position;
            camerarotate();

		}

		void setCameraPositionNormalView ()
		{
			if (bQuickSwitch == false) {
				// the camera to standard position and direction
				transform.position = Vector3.Lerp (transform.position, standardPos.position, Time.fixedDeltaTime * smooth);
				transform.forward = Vector3.Lerp (transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
			} else {
				// the camera to standard position and direction / Quick Change
				transform.position = standardPos.position;
				transform.forward = standardPos.forward;
				bQuickSwitch = false;
			}
		}

		void setCameraPositionFrontView ()
		{
			// Change Front Camera
			bQuickSwitch = true;
			transform.position = frontPos.position;
			transform.forward = frontPos.forward;
		}

		void setCameraPositionJumpView ()
		{
			// Change Jump Camera
			bQuickSwitch = false;
			transform.position = Vector3.Lerp (transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);
			transform.forward = Vector3.Lerp (transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);
		}

		private void camerarotate(){
        // マウスの右クリックを押している間
        if (Input.GetMouseButton(0)) {
            // マウスの移動量
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            // targetの位置のY軸を中心に、回転（公転）する
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * rotatespeed);
            // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
            transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * rotatespeed);
        }
    }
	}
}