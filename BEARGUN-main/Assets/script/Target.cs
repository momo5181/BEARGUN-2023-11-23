using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("位置:")]
    public GameObject Killstatment;
    public GameObject KillfeedContainer;
    
    [Space(10)]
    public float health = 50f;//設定血量
    public void TakeDamage (float amount)
    {
        health -=amount;
        if(health <= 0f) 
        {
             Instantiate(Killstatment, transform.position, Quaternion.identity, KillfeedContainer.transform);
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);//銷毀物體
    }
}
