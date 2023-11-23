using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f ;
    public float currentHealth ;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth ;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth<=0)
        {
           PlayerDie();
        }
            
    }
    void PlayerDie()
    {
         SceneManager.LoadScene(2);
    }
    public void TakeDamage(int damage){//調用上述Takedamage(2)，把2轉接成damage變數
        currentHealth -=damage ;//currenthealth初始直=100,受到傷害-2
        healthBar.SetHealth(currentHealth);
    }
}
