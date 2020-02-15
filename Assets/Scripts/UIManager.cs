using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject fireIcon;
    public TextMeshProUGUI fireText;

    PlayerInventory inventory;

    void Start()
    {
        inventory = GameObject.FindObjectOfType<PlayerInventory>();
    }

    void Update()
    {
        if (inventory.fireballNum <= 0) {
            fireIcon.SetActive(false);
        }
        else {
            fireIcon.SetActive(true);
            fireText.text = "x" + inventory.fireballNum;
        }
    }
}
