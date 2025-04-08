using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public int health = 50;
    private Hit FX;
    public float healthSlider;

    private void Start()
    {
        FX = GetComponentInChildren<Hit>();
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} 受到了 {amount} 点伤害，剩余血量：{health}");
        FX.StartCoroutine(FX.FlashFx());
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GetComponent<Player>().payerID == 1)
        {
            healthSlider = -6.47f - (50 - health) * .1f;
        }else if(GetComponent<Player>().payerID == 2)
        {
            healthSlider = 6.43f + (50 - health) * .1f;
        }
    }
}
