using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Get() {
        return (FadeManager) HushPuppy.safeFindComponent("GameController", "FadeManager");
    }

    [SerializeField]
    CanvasGroup screen;

    public IEnumerator Fade(float target) {
        float duration = 0.25f;
        screen.DOFade(target, duration);
        yield return new WaitForSeconds(duration);
    }
}
