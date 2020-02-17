using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCharacter : MonoBehaviour, PlayerInteractive {
	public string dialogID = "";
	public AudioClip soundbite;

	DialogManager dialogManager;
	bool dialogActive = false;

	void Start() {
		dialogManager = DialogManager.Get();
		dialogManager.setActiveEvent += ((v) => this.dialogActive = v);
	}

    public void OnInteraction(GameObject player) {
		if (this.dialogActive) {
			dialogManager.PressDialogKey();
		} else {
			dialogManager.RunDialog(this);
		}
    }
}
