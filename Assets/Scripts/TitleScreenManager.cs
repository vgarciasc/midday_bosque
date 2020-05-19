using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public FadeManager fadeManager;
    public AudioClip enterSound;
    public string gameScene;

    bool active = false;

    public void Enter(string language) {
        PlayerPrefs.SetString("language", language);
        PressEnter_();
    }

    public void PressEnter_() {
        StartCoroutine(PressEnter());
    }

    IEnumerator PressEnter() {
        if (active) yield break;
        active = true;
        
        EasyAudio.Get().audio.PlayOneShot(enterSound, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fadeManager.Fade(1f, 1f));
        yield return new WaitForSeconds(5f);

        SceneLoader.LoadScene_(gameScene);
    }
}
