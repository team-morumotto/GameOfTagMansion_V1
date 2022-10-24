using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

namespace MansionPlayer{
    public class PlayerCameraManager : MonoBehaviourPunCallbacks
    {
        public CinemachineVirtualCamera Camera;    //Cinemachineカメラ

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
    }
}