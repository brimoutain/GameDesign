using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} 受到了 {amount} 点伤害，剩余血量：{health}");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
