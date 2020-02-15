using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject firePrefab;
    public float speed = 1f;

    Rigidbody2D rb;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public void Init(Vector3 direction) {
        Start();
        this.rb.velocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        var obj = coll.gameObject;
        HandleCollision(obj);
    }

    void OnColliderEnter2D(Collider2D coll) {
        var obj = coll.gameObject;
        HandleCollision(obj);
    }

    void HandleCollision(GameObject obj) {
        var material = obj.GetComponent<ChemMaterial>();

        if (material != null) {
            Instantiate(firePrefab, obj.transform.position, Quaternion.identity, this.transform.parent);
        }

        if (obj.tag != "Player") {
            Destroy(this.gameObject);
        }
    }
}
