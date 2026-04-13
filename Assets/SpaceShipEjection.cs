using UnityEngine;

public class SpaceShipEjection : MonoBehaviour
{

    public GameObject WinUi;
    private GameObject storedPlayer;

    public bool allowEjection = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {

        if (storedPlayer != null && Input.GetKeyDown(KeyCode.Space) && allowEjection)
        {
            storedPlayer.SetActive(true);

            // Optional: place player back near spaceship
            storedPlayer.transform.position = transform.position + Vector3.up * 1.5f;

            WinUi.SetActive(false);

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
