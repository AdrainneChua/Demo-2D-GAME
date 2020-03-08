using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    public Slider slider;

    public void SetMaxHealth(float hp) {
        int Health = (int) hp;
        slider.maxValue = Health;
        slider.value = Health;
    }

    public void SetHealth(float hp) {
        int Health = (int) hp;
        slider.value = Health;
    }
}