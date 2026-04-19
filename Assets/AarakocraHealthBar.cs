using UnityEngine;
using UnityEngine.UI;

public class AarakocraHealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
        Destroy(gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
