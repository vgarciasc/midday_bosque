using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    CutsceneTyper cutsceneTyper;
    PlayerActions playerActions;

    [SerializeField]
    private string beginningCutscenePath;

    void Start() {
        cutsceneTyper = CutsceneTyper.Get();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();

        // StartCoroutine(BeginningGame());
    }

    IEnumerator BeginningGame() {
        playerActions.SetFreeze(true);

        cutsceneTyper.ToggleCanvasGroup(1f, 0f);
        cutsceneTyper.PrepareText(beginningCutscenePath);
        yield return new WaitForSeconds(1f);
        yield return cutsceneTyper.ShowText();
        yield return new WaitForSeconds(1f);
        cutsceneTyper.ToggleCanvasGroup(0f);
        yield return new WaitForSeconds(1f);

        playerActions.SetFreeze(false);
        SoundtrackManager.Get().ToggleSoundtrack(true);
    }
}
