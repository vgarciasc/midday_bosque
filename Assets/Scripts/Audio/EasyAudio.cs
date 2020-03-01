using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyAudio : MonoBehaviour
{
    public static EasyAudio Get() {
        return (EasyAudio) HushPuppy.safeFindComponent("EasyAudio", "EasyAudio");
    }

    public AudioSource audio;

    void OnEnable() {
        this.audio = this.GetComponent<AudioSource>();
    }
}
