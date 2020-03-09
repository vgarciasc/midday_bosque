using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    public float duration;

    void Start()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
