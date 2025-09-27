using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour{

    public TMP_Text TxThealth;
    
    public Slider slider;

    public void SetMaxHealth(int health){

        slider.maxValue = health;
        slider.value = health;
        TxThealth.text = health.ToString();
    }

    public void SetHealth(int health){

        slider.value = health;
        TxThealth.text = health.ToString();
    }
}
