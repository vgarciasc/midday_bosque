using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlorgCave : MonoBehaviour
{
    [SerializeField]
    bool startGoingUp;
    [SerializeField]
    float duration;

    void Start()
    {
        StartCoroutine(Move());    
    }

    IEnumerator Move()
    {
        bool toggle = startGoingUp;
        while (true) {
            toggle = !toggle;
            float newPos = this.transform.position.y + 1f * (toggle ? -1f : 1f);

            yield return new WaitForSeconds(duration);
            this.transform.DOMoveY(newPos, duration)
                .SetEase(Ease.Linear);
            yield return new WaitForSeconds(duration);
        }
    }
}
