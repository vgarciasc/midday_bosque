using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    AudioClip footsteps;
    [SerializeField]
    AudioClip itemUseError;
    [SerializeField]
    AudioClip bottleUseIn;
    [SerializeField]
    AudioClip bottleUseOut;
    [SerializeField]
    AudioClip putPowder;
    [SerializeField]
    AudioClip throwFireball;

    AudioSource audio;
    PlayerMovement movement;
    PlayerItemUse itemUse;

    void Start() {
        movement = this.GetComponent<PlayerMovement>();
        itemUse = this.GetComponent<PlayerItemUse>();
        audio = this.GetComponent<AudioSource>();

        itemUse.itemUseError.AddListener(() => audio.PlayOneShot(this.itemUseError, 0.1f));
        itemUse.bottleUseIn.AddListener(() => audio.PlayOneShot(this.bottleUseIn, 0.1f));
        itemUse.bottleUseOut.AddListener(() => audio.PlayOneShot(this.bottleUseOut, 0.1f));
        itemUse.throwFireball.AddListener(() => audio.PlayOneShot(this.throwFireball, 0.1f));
        itemUse.putPowder.AddListener(() => audio.PlayOneShot(this.putPowder, 0.1f));

        // StartCoroutine(EmitFootsteps());
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
