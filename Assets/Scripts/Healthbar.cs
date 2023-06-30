using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health healthComponent;
    [SerializeField] private Image healthbarBackground;
    [SerializeField] private Image healthbarForeground;

    bool isDisplaying = false;

    // Start is called before the first frame update
    void Start()
    {
        healthbarForeground.enabled = false;
        healthbarBackground.enabled = false;
        isDisplaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool isMaxHealth = healthComponent.currentHealth == healthComponent.GetMaxHealth();
        if(isDisplaying && isMaxHealth) {
            healthbarForeground.enabled = false;
            healthbarBackground.enabled = false;
            isDisplaying = false;
            return;
        }

        float fillAmount = (float)healthComponent.currentHealth / (float)healthComponent.GetMaxHealth();
        if(isDisplaying) {
            healthbarForeground.fillAmount = fillAmount;
        }
        else {
            if(fillAmount < 1) {
                healthbarForeground.enabled = true;
                healthbarBackground.enabled = true;
                isDisplaying = true;
                healthbarForeground.fillAmount = fillAmount;
            }
        }
    }
}
