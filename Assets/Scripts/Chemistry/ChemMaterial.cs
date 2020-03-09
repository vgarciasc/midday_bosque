using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;

public class ChemMaterial : MonoBehaviour {

    public float health = 0;
    public float maxHealth = 5;
    public bool inflammable = false;
    [SerializeField]
    GameObject firePrefab;

    protected bool isDead = false;
    protected SpriteRenderer sr;

    public UnityEvent catchFire;
    public UnityEvent death;

    bool spawnedFire;

    public void OnEnable() {
        sr = this.GetComponent<SpriteRenderer>();
        
        this.health = this.maxHealth;
    }

    void Update() {
        if (health <= 0) {
            Die();
        }
    }

    public virtual void React(ChemElement element) {
        if (element.kind == ElementKind.FIRE) {
            catchFire.Invoke();
            if (inflammable) {
                health -= Time.deltaTime;
            }
        }
        else if (element.kind == ElementKind.EXPLOSION) {
            if (inflammable && !spawnedFire) {
                spawnedFire = true;
                Instantiate(firePrefab,
                    this.transform.position,
                    Quaternion.identity,
                    this.transform.parent);
            }
        }
    }

    protected virtual void Die() {
        if (isDead) return;
        
        isDead = true;
        sr.DOFade(0, 0.5f).OnComplete(() => {
            death.Invoke(); 
            Destroy(this.gameObject);
        });
    }
}
