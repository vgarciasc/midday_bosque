using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogCharacterAnimation {
	public List<Sprite> frames;
}

public class DialogCharacter : MonoBehaviour, PlayerInteractive {
	public string dialogID = "";
	public AudioClip soundbite;

	DialogManager dialogManager;
	bool dialogActive = false;

	public List<DialogCharacterAnimation> animations;

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

	public void ChangeDialog(string newDialogID) {
		this.dialogID = newDialogID;
	}

	public void ChangeAnimation(int animationId) {
		this.GetComponent<EasyAnim>().frames = animations[animationId].frames;
	}
}
