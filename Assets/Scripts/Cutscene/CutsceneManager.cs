using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Get() {
        return (CutsceneManager) HushPuppy.safeFindComponent("GameController", "CutsceneManager");
    }

    CutsceneTyper cutsceneTyper;
    PlayerActions playerActions;

    [SerializeField]
    private string beginningCutscenePath;
    [SerializeField]
    CanvasGroup blackscreen;
    [SerializeField]
    EasyAnim sickKid; 
    [SerializeField]
    List<Sprite> healthyKidSprites;
    [SerializeField]
    AudioClip healSpell;
    [SerializeField]
    AudioClip terrorSong;
    [SerializeField]
    GameObject fairyFountainWater;
    [SerializeField]
    bool startAtBeginning;

    void Start() {
        cutsceneTyper = CutsceneTyper.Get();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();

        if (startAtBeginning) {
            StartCoroutine(BeginningGame());
        }
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

    public void EnterFairyFountain() {
        SoundtrackManager.Get().ToggleSoundtrack(false);
        // SoundtrackManager.Get().ToggleSoundtrack(true, terrorSong);
    }

    public IEnumerator TurnFairy(PlayerStorySpecificStuff playerStory) {
        playerActions.SetFreeze(true);

        yield return new WaitForSeconds(1f);
        fairyFountainWater.SetActive(true);

        var player = playerActions.transform;
        player.DOMoveY(player.position.y - 1f, 2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.5f);

        player.GetComponent<SpriteRenderer>().enabled = false;

        blackscreen.GetComponent<Image>().color = Color.white;
        blackscreen.alpha = 0f;
        blackscreen.gameObject.SetActive(true);
        blackscreen.DOFade(1f, 1.5f);

        yield return new WaitForSeconds(1.5f);
        playerStory.ToggleFairy(true);
        SoundtrackManager.Get().ToggleSoundtrack(true, terrorSong);

        player.DOMoveY(player.position.y + 1f, 0f);
        blackscreen.DOFade(0f, 0f);
        yield return new WaitForSeconds(0.5f);
        blackscreen.DOFade(1f, 0f);
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<SpriteRenderer>().enabled = true;
        blackscreen.DOFade(0f, 0f);

        fairyFountainWater.SetActive(false);

        playerActions.SetFreeze(false);
    }

    public IEnumerator EndingGame() {
        playerActions.SetFreeze(true);
        SoundtrackManager.Get().ToggleSoundtrack(false);

        foreach (Collider2D coll in playerActions.GetComponentsInChildren<Collider2D>())
            coll.enabled = false;

        var player = playerActions.transform;
        yield return new WaitForSeconds(1f);
        player.DOMoveY(player.transform.position.y + 1f, 5f);
        yield return new WaitForSeconds(4f);
        player.DOScale(0f, 3f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(3f);

        blackscreen.GetComponent<Image>().color = Color.white;
        blackscreen.alpha = 0f;
        blackscreen.gameObject.SetActive(true);
        blackscreen.DOFade(1f, 3f);

        yield return new WaitForSeconds(1.5f);
        EasyAudio.Get().audio.PlayOneShot(this.healSpell, 0.4f);
        yield return new WaitForSeconds(1.5f);
        
        sickKid.transform.localRotation = Quaternion.identity;
        sickKid.frames = healthyKidSprites;
        sickKid.duration = 1f;

        blackscreen.DOFade(0f, 1.5f);

        yield return new WaitForSeconds(5f);

        cutsceneTyper.PrepareText("ENDING_CUTSCENE");

        cutsceneTyper.ToggleCanvasGroup(1f, 5f);

        yield return cutsceneTyper.ShowText();

        yield return new WaitForSeconds(10f);

        // Application.Quit();
        SceneLoader.LoadScene_("Title Screen");

        yield break;
    }
}
