﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChemMaterial : MonoBehaviour
{
    public float health = 0;
    public float maxHealth = 5;
    public bool inflammable = false;

    bool isDead = false;

    SpriteRenderer sr;

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
            if (inflammable) {
                health -= Time.deltaTime;
            }
        }
    }

    protected virtual void Die() {
        if (isDead) return;
        
        isDead = true;
        sr.DOFade(0, 0.5f).OnComplete(() => 
            Destroy(this.gameObject));
    }
}