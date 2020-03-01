using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChemExplosion : ChemElement {
    
    GameObject firePrefab;
    Vector3 originalScale;
    
    [SerializeField]
    AudioClip explosionClip;

    protected new void Start() {
        base.Start();
        
        originalScale = this.transform.localScale;

        Explode();
    }

    void Explode() {
        EasyAudio.Get().audio.PlayOneShot(explosionClip, 0.4f);

        this.transform.localScale = Vector2.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(this.transform.DOScale(originalScale * 2, 0.2f).OnComplete(() =>
            ExplosiveSideEffects()));
        sequence.Append(this.transform.DOScale(originalScale * 0f, 0.8f).OnPlay(() => 
            this.sr.DOColor(Color.white, 0.8f)).SetDelay(0.1f));
        sequence.OnComplete(() => barreira());
        sequence.Play();

        Camera.main.GetComponent<SpecialCamera>().ScreenShake(0.1f, 30);
    }

    void ExplosiveSideEffects() {
        Bounds bounds = new Bounds(this.transform.position, Vector2.one * 2f);
        var hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max);

        foreach (var hit in hits) {
            var material = hit.GetComponentInChildren<ChemMaterial>();
            if (material != null) {
                material.React(this);
            }
        }

        barreira();
    }

    int k = 0;

    void barreira() {
        k++;
        if (k == 2) {
            Destroy(this.gameObject);
        }
    }
}
