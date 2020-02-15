using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomDoor : RoomButtonObject
{
    SpriteRenderer sr;

    public bool invert = false;

    void Start() {
        if (invert) {
            this.gameObject.SetActive(false);
            sr.color = HushPuppy.getColorWithOpacity(sr.color, 0f);
        }
    }

    void OnEnable() {
        sr = this.GetComponent<SpriteRenderer>();
        // sr.color = HushPuppy.getColorWithOpacity(sr.color, 1f);
    }

    public override void Toggle(bool value) {
        if (value && !IsEveryButtonActive()) {
            return;
        }

        if (invert) {
            value = !value;
        }

        if (value == true) {
            sr.DOFade(0f, 0.2f).OnComplete(() => this.gameObject.SetActive(false));
        } else if (value == false) {
            sr.DOFade(1f, 0.2f);
        }
    }
}
