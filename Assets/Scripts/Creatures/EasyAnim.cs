using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyAnim : MonoBehaviour
{
    public List<Sprite> frames = new List<Sprite>();
    [Range(0f, 3f)]
    public float duration = 0.05f;

    SpriteRenderer sr;

    void OnEnable()
    {
        sr = this.GetComponent<SpriteRenderer>();
    
        if (frames.Count > 0) {
            StartCoroutine(Anim());
        }
    }

    IEnumerator Anim() {
        int index = 0;
        while (true) {
            index = (index + 1) % frames.Count;
            sr.sprite = frames[index];
            yield return new WaitForSeconds(this.duration);
        }
    }
}
