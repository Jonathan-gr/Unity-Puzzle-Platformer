using UnityEngine;

public class SpaceShipEjection : MonoBehaviour
{


    private GameObject storedPlayer;
    public GameUIManager uiManager;
    public bool allowEjection = false;


    // Update is called once per frame
    void Update()
    {

        if (storedPlayer != null && Input.GetKeyDown(KeyCode.Space) && allowEjection)
        {
            storedPlayer.SetActive(true);

            // Optional: place player back near spaceship
            storedPlayer.transform.position = transform.position + Vector3.up * 1.5f;

            uiManager.HideAll();

            storedPlayer = null;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {

                storedPlayer = collision.gameObject;

                storedPlayer.SetActive(false);
            }
        }
    }
}
