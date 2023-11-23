using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
   public Gradient gradient;
   public Image fill;
   public Text healthNum;
    public void SetMaxHealth(int health){
        slider.maxValue = health;
        slider.value = health; //血量初始最大值
   
     fill.color=gradient.Evaluate(1f);
    }

    public void SetHealth(int health){
        slider.value = health;

        fill.color=gradient.Evaluate(slider.normalizedValue);
        healthNum.text = health.ToString();
    }



    
}
