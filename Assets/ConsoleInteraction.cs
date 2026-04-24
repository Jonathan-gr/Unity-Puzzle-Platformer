using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleInteractable : MonoBehaviour
{
    [Header("Interaction")]

    [Header("UI")]
    public TextMeshProUGUI consoleText;         // drag your UI Text here
    public GameObject textCanvas;    // drag your World Space Canvas here
    public bool firstMessage = true;
    public string startingMessage = "Press E to interact";

    [Header("Messages")]
    [TextArea]
    public string[] messages = {
        "SYSTEM BOOTING...",
        "SYSTEM BOOTING....",
                "SYSTEM BOOTING....",
        "SYSTEM BOOTING......",        "SYSTEM BOOTING......",
        "SYSTEM BOOTING.........",        "SYSTEM BOOTING...........",
        "SYSTEM BOOTING..............."
    };

    public void Start()
    {
        consoleText.text = startingMessage;
    }

    private int currentIndex = -1;
    private bool playerInRange = false;

    void Update()
    {


        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            firstMessage = false;
            currentIndex = (currentIndex + 1) % messages.Length;
            consoleText.text = messages[currentIndex];


            consoleText.lineSpacing -= 10;


        }

        // Optional: hide text when player walks away
        if (!playerInRange && firstMessage)
            textCanvas.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            playerInRange = true;
            textCanvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            currentIndex = -1;
            if (firstMessage)
            {
                consoleText.text = startingMessage;
            }


        }
    }
}