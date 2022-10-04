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
        var speed = 5.0f;
        var vx = speed * Input.GetAxis("Horizontal");
        var vz = speed * Input.GetAxis("Vertical");
        _rigidbody.velocity = new Vector3(vx,0, vz);
    }
}
