using Unity.VisualScripting;
using UnityEngine;

public class SpaceShipPausesGame : MonoBehaviour
{
    public GameObject WinUi;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                WinUi.SetActive(true);
            }

        }
    }
}
