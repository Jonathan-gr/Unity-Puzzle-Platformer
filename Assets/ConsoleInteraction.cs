using UnityEngine;
using TMPro;

public class MachineInteraction : MonoBehaviour
{
    public GameObject textUI; // drag MachineText here

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleText();
        }
    }

    void ToggleText()
    {
        textUI.SetActive(!textUI.activeSelf);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            textUI.SetActive(false); // hide when leaving
        }
    }
}