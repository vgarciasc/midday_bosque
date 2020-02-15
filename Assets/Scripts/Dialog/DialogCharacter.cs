﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCharacter : MonoBehaviour {

	public string dialogID = "";
	public Sprite portrait;
	public bool flipSprite = false;

	DialogManager dialogManager;
	bool in_range = false;
	bool talking = false;

	void Start () {
		dialogManager = DialogManager.Get();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		GameObject target = collider.gameObject;
		if (target.tag == "Player") {
			in_range = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		GameObject target = collider.gameObject;
		if (target.tag == "Player") {
			in_range = false;
		}
	}
	
	void Update () {
		handleText();
	}

	void handleText() {
		if (Input.GetKeyDown(KeyCode.Q) && in_range && !dialogManager.dialog_active) {
			dialogManager.start(this);
		}
	}
}
