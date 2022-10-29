using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerSet : MonoBehaviour
{
    [SerializeField] private Dropdown MemberDD;//Dropdownを入れる変数
    public static int GamePlayers = 2;//ゲームのプレイヤー人数(デフォルトは2人)
    void Update(){
        if(MemberDD.value == 0){
            GamePlayers = 2;
        }
        else if(MemberDD.value == 1){
            GamePlayers = 3;
        }
        else if(MemberDD.value == 2){
            GamePlayers = 4;
        }
    }
}