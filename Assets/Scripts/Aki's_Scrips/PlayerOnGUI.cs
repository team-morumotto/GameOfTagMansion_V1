/*
2022/11/21 Atsuki Kobayashi
にげ/おにキャラクターにアタッチされているスクリプト。
操作説明の表示、スピードアップの表示、及び点滅を行う。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerOnGUI : MonoBehaviourPunCallbacks
{
    private GUIStyle ControlStyle,SpeedUpStyle;//操作とスピードアップで使用するGUIStyle
    private float PlayerRun = 10;//プレイヤーが走っているときの速度
    private float OniRun = 13.0f;//おにが走っているときの速度

	void Start()
	{
        if(!photonView.IsMine) return;//自分のオブジェクトでなければ処理を行わない
		ControlStyle = new GUIStyle();//操作説明のGUIStyle
        SpeedUpStyle = new GUIStyle();//スピードアップのGUIStyle
        //どちらもフォントサイズを20に設定
		ControlStyle.fontSize = 30;
        SpeedUpStyle.fontSize = 80;
        ControlStyle.normal.textColor = Color.white;//操作説明GUIのフォントの色を白に設定
	}

    void Update(){
        if(!photonView.IsMine) return;//自分のオブジェクトでなければ処理を行わない
        //プレイヤーの移動速度がスピードアップ状態の時
        if(playersample.moveSpeed == PlayerRun || oni_sample.speed == OniRun){
            StartCoroutine(Text_Color_Frash());
        }
    }

    /// <summary> スピードアップの表記を点滅させる </summary>
    IEnumerator Text_Color_Frash(){
        SpeedUpStyle.normal.textColor = Color.red;
        yield return new WaitForSeconds(0.5f);//0.5秒ごとに点滅
        SpeedUpStyle.normal.textColor = Color.blue;
    }

    void OnGUI(){
        if(!photonView.IsMine){return;}
        GUI.Label(new Rect(0,800,500,400),"・左スティックで移動",ControlStyle);
        GUI.Label(new Rect(0,850,500,400),"・右スティックでカメラ操作",ControlStyle);
        GUI.Label(new Rect(0,900,500,400),"・アイテムをとるとスピードアップ",ControlStyle);
        if(playersample.moveSpeed == PlayerRun || oni_sample.speed == OniRun ){
            GUI.Label(new Rect(480,300,500,400),"スピードアップ中！！",SpeedUpStyle);
        }else{
            GUI.Label(new Rect(250,100,500,400),"",ControlStyle);
        }
    }
}
