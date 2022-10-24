using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RobbyScripts : MonoBehaviourPunCallbacks
{
    public GameObject PhotonObject;
    void Start()
    {
        Instantiate(PhotonObject, new Vector3(0f, 1f, 0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
