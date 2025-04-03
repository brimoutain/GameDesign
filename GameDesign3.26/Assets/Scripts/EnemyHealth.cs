using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} �ܵ��� {amount} ���˺���ʣ��Ѫ����{health}");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
