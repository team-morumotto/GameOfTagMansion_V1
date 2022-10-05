using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Player;
    private Rigidbody _rigidbody;

    void Start () {
        _rigidbody = Player.GetComponent<Rigidbody>();
    }

    void Update () {
         //var speed = 5.0f;
        //var vx = speed * Input.GetAxis("Horizontal");
        //var vz = speed * Input.GetAxis("Vertical");
        var rv = new Vector3();
        if (Input.GetKey(KeyCode.A)) {
                rv = new Vector3(-0.1f,0f,0f);
            }
            if (Input.GetKey(KeyCode.D)) {
                rv = new Vector3(0.1f,0f,0f);
            }
            if (Input.GetKey(KeyCode.W)) {
                rv = new Vector3(0f,0f,0.1f);
            }
            if (Input.GetKey(KeyCode.S)) {
                rv = new Vector3(0f,0f,-0.1f);
            }
            
        //_rigidbody.velocity = new Vector3(rv.x,_rigidbody.velocity.y,rv.z);
        Player.transform.Translate(rv.x,0,rv.z);
    }
    
}
