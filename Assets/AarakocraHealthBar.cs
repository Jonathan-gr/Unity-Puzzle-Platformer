using UnityEngine;
using UnityEngine.UI;

public class AarakocraHealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;

    public void AarakocraHit(float damage)
    {
        slider.value -= damage;

        if (slider.value <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        slider.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("aarakocra started");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
