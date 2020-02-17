using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    AudioClip footsteps;

    AudioSource audio;
    PlayerMovement movement;

    void Start() {
        movement = this.GetComponent<PlayerMovement>();
        audio = this.GetComponent<AudioSource>();

        StartCoroutine(EmitFootsteps());
    }

    IEnumerator EmitFootsteps() {
        while (true) {
            yield return new WaitUntil(() => movement.isWalking);
            
            while (movement.isWalking) {
                audio.PlayOneShot(footsteps);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
