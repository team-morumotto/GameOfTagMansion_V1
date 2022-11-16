using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    void Update(){
        transform.Rotate(0, 1, 0);
    }
    void OnTriggerEnter(Collider c){
        if(c.gameObject.tag == "Nigeru"){
            Destroy(gameObject);
            playersample.moveSpeed = 10.0f;
        }
        else if(c.gameObject.tag == "Oni"){
            Destroy(gameObject);
            oni_sample.speed = 13.0f;
        }
        else{
            Destroy(gameObject);
        }
    }
}
