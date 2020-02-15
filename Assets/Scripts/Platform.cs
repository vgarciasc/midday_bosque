using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 1;

    Rigidbody2D rb;

    void OnEnable() {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.velocity = direction * speed;        
    }
}
