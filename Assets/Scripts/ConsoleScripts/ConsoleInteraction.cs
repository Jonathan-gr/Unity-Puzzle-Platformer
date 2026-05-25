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
    public float lineSpaceAmount = 0.5f;
    public float messageFontSize = 100f;
    public float menuFontSize = 50f;


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


    [Header("MainMenuOfCOnsole")]
    [TextArea]
    public string MainMenuMessage = "Press E to view logs, press Q to.. press R to..";
    [Header("Behavior Options")]
    public bool resetOnReEnter = true;     // reset to menu when re-entering
    public bool keepTextVisibleOnExit = false; // keep UI visible after leaving
    private string currentMessage = "";

    public GameObject ladderPrefab;
    public string secretLadderMessage = "";

    public void Start()
    {
        consoleText.text = startingMessage;
    }

    private int currentIndex = -1;
    private bool playerInRange = false;
    public bool secretLadder = false;

    void Update()
    {
        
        if (secretLadder)
            this.enabled = false; 


        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            firstMessage = false;

            consoleText.fontSize = messageFontSize;

            currentIndex = (currentIndex + 1) % messages.Length;
            currentMessage = messages[currentIndex];

            consoleText.text = currentMessage;
        }
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            firstMessage = false;
            consoleText.lineSpacing -= lineSpaceAmount;


        }
        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {

            if(firstMessage)
                return;

            if (currentIndex == messages.Length - 1 && consoleText.lineSpacing < -40 && consoleText.lineSpacing > -55)
            {
                Vector3 offset = new Vector3(-1f, 4.5f, 0f);
                Instantiate(ladderPrefab, transform.position + offset, Quaternion.identity);
                consoleText.text = secretLadderMessage;
                secretLadder = true;

            }
            else
            {
                firstMessage = false;
                consoleText.lineSpacing = 0;
                currentIndex = 0;
                consoleText.text = messages[currentIndex];

            }          

        }

        
        // Optional: hide text when player walks away
        if (!playerInRange && firstMessage)
            textCanvas.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if(secretLadder) return;

        playerInRange = true;
        textCanvas.SetActive(true);

        // If we want to reset every time
        if (resetOnReEnter)
        {
            currentIndex = -1;
            currentMessage = startingMessage;
            consoleText.fontSize = menuFontSize;
            consoleText.text = startingMessage;
            firstMessage = true;
        }
        else
        {
            // Resume previous message
            consoleText.text = string.IsNullOrEmpty(currentMessage) ? startingMessage : currentMessage;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if(secretLadder) return;

        playerInRange = false;


        if (!keepTextVisibleOnExit)
        {
            textCanvas.SetActive(false);
        }

        // Optional: reset only if you want full reset on exit too
        if (resetOnReEnter)
        {
            currentIndex = -1;
            currentMessage = "";
            consoleText.lineSpacing = 0;
        }
    }
}