using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject firePrefab;
    public float speed = 1f;
    public AudioClip fireballDeath;
    public GameObject fireballFailParticlesPrefab;

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

            Die();
            return;
        }
    }

    List<ChemMaterial> reacted = new List<ChemMaterial>();
    void React(ChemMaterial material) {
        if (reacted.Contains(material)) return;
        if (!material.inflammable) return;

        reacted.Add(material);
        Instantiate(firePrefab,
            material.transform.position,
            Quaternion.identity,
            this.transform.parent);
        Die();
    }

    void Die() {
        EasyAudio.Get().audio.PlayOneShot(this.fireballDeath, 0.05f);
        // Instantiate(fireballFailParticlesPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
        Destroy(this.gameObject);
    }
}
