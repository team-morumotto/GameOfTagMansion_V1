using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 1, 0);
    }
    void OnTriggerEnter(Collider c)
    {
        Destroy(gameObject);
        playersample.moveSpeed = 10.0f;
    }
}
