using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initialSpeed = 2200;
    public Rigidbody rb;
    private void Start()
    {
        rb.velocity = transform.forward * initialSpeed;
    }
    void Update()
    {
    }
}
