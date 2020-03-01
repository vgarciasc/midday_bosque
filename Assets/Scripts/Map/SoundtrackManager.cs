using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundtrackManager : MonoBehaviour
{
    public static SoundtrackManager Get() {
        return (SoundtrackManager) HushPuppy.safeFindComponent("AudioManager", "SoundtrackManager");
    }

    AudioSource audio;

    [SerializeField]
    AudioClip soundtrack;
    [SerializeField]
    bool playOnStart;

    float originalVolume;
    bool tonedDown;

    void Start() {
        audio = this.GetComponent<AudioSource>();        
        originalVolume = audio.volume;

        RoomManager.Get().roomChangeEvent += RoomChange;

        if (playOnStart) {
            audio.clip = soundtrack;
            audio.Play();
        }
    }

    void RoomChange(Room room) {
        if (room.volumeToneDown && !tonedDown) {
            audio.DOFade(originalVolume - 0.4f, 1f).OnComplete(() => tonedDown = true);
        } else if (!room.volumeToneDown && tonedDown) {
            audio.DOFade(originalVolume, 1f).OnComplete(() => tonedDown = false);
        }
    }

    public void ToggleSoundtrack(bool value) {
        if (value) {
            audio.clip = soundtrack;
            audio.Play();
        } else {
            audio.Stop();
        }
    }
}
