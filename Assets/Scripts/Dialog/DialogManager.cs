using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using RedBlueGames.Tools.TextTyper;
using TMPro;

public class DialogManager : MonoBehaviour {

    [Header("Settings")]
    [Header("Assets")]
    [SerializeField]
	private TextAsset inkAssetPortuguese;
    [SerializeField]
	private TextAsset inkAssetEnglish;
    [Header("References")]
    [SerializeField]
	private TextMeshProUGUI text;
    [SerializeField]
    private GameObject container;
    [SerializeField]
	private Animator animator;
    [SerializeField]
	private AudioSource audioSource;

	private TextTyper typer;

	private Story _story;
	private string savedJson = "";
	private bool printCompleted;
	private bool dialogActive;
	private bool dialogKeyPressed;

	private DialogCharacter currDialog;

	public delegate void SetActiveDelegate(bool value);
	public event SetActiveDelegate setActiveEvent;

	public static DialogManager Get() {
        var gc = GameObject.FindGameObjectWithTag("GameController");
        if (gc == null) { Debug.LogError("GameController not found."); return null; }
        var o = gc.GetComponent<DialogManager>();
        if (o == null) { Debug.LogError("DialogManager not found in GameController."); return null; }
		return (DialogManager) HushPuppy.safeFindComponent("GameController", "DialogManager");
	}

	void Start() {
        typer = text.GetComponent<TextTyper>();
		typer.CharacterPrinted.AddListener(EmitSoundbite);
		typer.PrintCompleted.AddListener(() => {
            this.printCompleted = true;
        });
	}

	void Toggle(bool value) {
		this.dialogActive = value;
		if (setActiveEvent != null) {
			setActiveEvent(value);
		}
	}

	public void RunDialog(DialogCharacter character) {
		this.currDialog = character;
		if (PlayerPrefs.GetString("language") == "pt-BR") {
			this._story = new Story(inkAssetPortuguese.text);
		} else if (PlayerPrefs.GetString("language") == "en") {
			this._story = new Story(inkAssetEnglish.text);
		} else {
			Debug.Break();
			Debug.LogError("This should not be happening.");
			this._story = new Story(inkAssetPortuguese.text);
		}

		if (savedJson != "") {
			this._story.state.LoadJson(savedJson);
		}

		this._story.ChoosePathString(character.dialogID);

		Toggle(true);
		StartCoroutine(Text());
	}
	
	IEnumerator Text() {
		container.SetActive(true);
		if (animator) animator.SetBool("active", true);

		while (_story.canContinue) {
			string str = _story.Continue();
			this.printCompleted = false;

            // start typing
			typer.TypeText(str, 0.02f);
			if (animator) animator.SetBool("idle", false);

            // typing ends when player tries to skip it, or if the print was completed naturally
			yield return new WaitForEndOfFrame();
			yield return new WaitUntil(() => this.dialogKeyPressed || this.printCompleted);
			this.dialogKeyPressed = false;

			typer.Skip();
			if (animator) animator.SetBool("idle", true);

            // end this line after player confirmation
			yield return new WaitForEndOfFrame();
			yield return new WaitUntil(() => this.dialogKeyPressed);
			this.dialogKeyPressed = false;
		}		
		
		if (animator) animator.SetBool("active", false);
        container.SetActive(false);
		
		savedJson = _story.state.ToJson();
        this.printCompleted = false;
		this.currDialog = null;
		Toggle(false);
	}

	int k = 0;
	void EmitSoundbite(string str) {
		if (new List<string>() { ".", "!", "?", "," }.Contains(str)) {
			k = 0;
			return;
		}

		if (currDialog.soundbite != null) {
			k++;
			if (k % 3 == 0)
			audioSource.PlayOneShot(currDialog.soundbite);
		}
	}

	public void PressDialogKey() {
		this.dialogKeyPressed = true;
	}
}
