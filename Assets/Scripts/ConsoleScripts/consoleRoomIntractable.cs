using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleRoomInteractable : MonoBehaviour
{
    [Header("Interaction")]

    [Header("UI")]
    public TextMeshProUGUI consoleText;
    public GameObject textCanvas;

    public GameObject wordPrefab;



    public string startingMessage = "Press E to interact";

    public bool playerInRange = false;

    [Header("Configure the spawned object")]
    public bool moveUp = true;
    public bool moveLeft = false;
    public bool moveRight = false;
    public float upSpeed = 2f;
    public float horizontalSpeed = 2f;
    public float initPrefabXOffset = 0f;
    public float initPrefabYOffset = 2f;


    [Header("Sprite & Collider")]

    public Sprite wordSprite;

    public Vector2 colliderSize = new Vector2(1f, 1f);
    public Vector2 colliderOffset = new Vector2(0f, 0f);

    private Vector3 wordScale;
    public float xScaleOfPrefab = 1f;
    public float ySacaleOfPrefab = 1f;

    [Header("Lifetime")]
    public float destroyAfterSeconds = 3f;

    public void Start()
    {
        consoleText.text = startingMessage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            Vector3 offset = new Vector3(initPrefabXOffset, initPrefabYOffset, 0f);
            GameObject spawned = Instantiate(wordPrefab, transform.position + offset, Quaternion.identity);

            ConsolePrefabManager movement = spawned.GetComponent<ConsolePrefabManager>();
            if (movement != null)
            {
                movement.moveUp = moveUp;
                movement.moveLeft = moveLeft;
                movement.moveRight = moveRight;
                movement.upSpeed = upSpeed;
                movement.horizontalSpeed = horizontalSpeed;
            }
            // Sprite
            SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
            if (sr != null && wordSprite != null)
                sr.sprite = wordSprite;

            // Collider
            BoxCollider2D col = spawned.GetComponent<BoxCollider2D>();
            if (col != null)
            {
                col.size = colliderSize;
                col.offset = colliderOffset;
            }
            wordScale = new Vector3(xScaleOfPrefab, ySacaleOfPrefab, 1f);
            spawned.transform.localScale = wordScale;

            // Destroy the spawned instance after X seconds
            Destroy(spawned, destroyAfterSeconds);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        textCanvas.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        textCanvas.SetActive(false);
    }
}