﻿using System.Collections;
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

    public void Init(Vector3 direction) {
        Start();
        this.rb.velocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        var obj = coll.gameObject;
        HandleCollision(obj, false);
    }

    void OnColliderEnter2D(Collider2D coll) {
        var obj = coll.gameObject;
        HandleCollision(obj, true);
    }

    void HandleCollision(GameObject obj, bool collision) {
        var material = obj.GetComponent<ChemMaterial>();

        if (obj.CompareTag("Player")) {
            return;
        }

        if (material != null) {
            React(material);
            return;
        }
        
        if (material == null) {
            Bounds bounds = new Bounds(this.transform.position, Vector2.one * 0.5f);
            var hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max);
            foreach (var hit in hits) {
                var mat = hit.transform.GetComponent<ChemMaterial>();
                if (mat != null) {
                    React(mat);
                }
            }

            Destroy(this.gameObject);
            return;
        }
    }

    List<ChemMaterial> reacted = new List<ChemMaterial>();
    void React(ChemMaterial material) {
        if (reacted.Contains(material)) return;

        reacted.Add(material);
        Instantiate(firePrefab,
            material.transform.position,
            Quaternion.identity,
            this.transform.parent);
        Destroy(this.gameObject);
    }
}
