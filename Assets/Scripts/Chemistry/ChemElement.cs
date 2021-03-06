﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ElementKind {
    NONE, FIRE, EXPLOSION
}

public class ChemElement : MonoBehaviour
{
    public ElementKind kind;
    public float maxHealth = 5;
    public float health = 0;

    bool isDead = false;

    protected RoomManager roomManager;
    protected SpriteRenderer sr;

    protected void Start() {
        roomManager = RoomManager.Get();
        sr = this.GetComponent<SpriteRenderer>();

        this.health = this.maxHealth;

        StartAnimations();
    }

    protected void FixedUpdate() {
        HandleLife();
        HandleReactions();
    }

    protected virtual void HandleLife() {
        if (health <= 0) {
            Die();
        }
    }

    void HandleReactions() {
        var materials = new List<ChemMaterial>(
            GameObject.FindObjectsOfType<ChemMaterial>());
        materials = materials.FindAll((material) => 
            material.transform.position == this.transform.position);
        React(materials);
    }

    protected virtual void React(List<ChemMaterial> materials) {
        //do nothing
    }

    public virtual void Die() {
        if (isDead) return;

        isDead = true;
        sr.DOFade(0, 0.5f).OnComplete(() => 
            Destroy(this.gameObject));
    }

    protected virtual void StartAnimations() {
        //do nothing
    }

    protected virtual void StopAnimations() {
        //do nothing
    }
}
