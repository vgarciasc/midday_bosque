using System.Collections;
using System.Collections.Generic;
using RedBlueGames.Tools.TextTyper;
using UnityEngine;
using Ink.Runtime;
using DG.Tweening;
using TMPro;

public class CutsceneTyper : MonoBehaviour
{
    public static CutsceneTyper Get() {
        return (CutsceneTyper) HushPuppy.safeFindComponent("GameController", "CutsceneTyper");
    }

    [SerializeField]
    private TextTyper typer;
    [SerializeField]
	private TextAsset inkAsset;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private CanvasGroup cutsceneCanvasGroup;
    [SerializeField]
    private AudioClip soundbite;

	private Story _story;

    bool printCompleted = false;
    bool dialogKeyPressed = false;

	void Start() {
		typer.CharacterPrinted.AddListener(EmitSoundbite);
		typer.PrintCompleted.AddListener(() => {
            this.printCompleted = true;
        });

        cutsceneCanvasGroup.gameObject.SetActive(true);
        cutsceneCanvasGroup.alpha = 0f;
	}

    public void ToggleCanvasGroup(float value, float duration = 0.5f) {
        cutsceneCanvasGroup.DOFade(value, duration);
    }

    public void PrepareText(string path) {
        this._story = new Story(inkAsset.text);
		this._story.ChoosePathString(path);
    }

    public IEnumerator ShowText() {
        while (_story.canContinue) {
			string str = _story.Continue();
			this.printCompleted = false;

			typer.TypeText(str, 0.04f);

            // typing ends when player tries to skip it, or if the print was completed naturally
			yield return new WaitForEndOfFrame();
			yield return new WaitUntil(() => this.dialogKeyPressed || this.printCompleted);
			this.dialogKeyPressed = false;

			typer.Skip();

            // end this line after player confirmation
			yield return new WaitForEndOfFrame();
			yield return new WaitUntil(() => this.dialogKeyPressed);
			this.dialogKeyPressed = false;
		}
    }

	public void PressDialogKey() {
		this.dialogKeyPressed = true;
	}

	int k = 0;
	void EmitSoundbite(string str) {
		if (new List<string>() { ".", "!", "?", "," }.Contains(str)) {
			k = 0;
			return;
		}

		if (this.soundbite != null) {
			k++;
			if (k % 2 == 0) EasyAudio.Get().audio.PlayOneShot(this.soundbite, 0.2f);
		}
	}
}
