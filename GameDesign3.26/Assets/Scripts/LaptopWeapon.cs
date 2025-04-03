using UnityEngine;
using System.Collections;

public class LaptopWeapon : Weapon, IWeapon
{
    public int ownerID = -1;
    public bool isHeld = false;

    public int damage = 20;
    public Vector2 explosionSize = new Vector2(10f, 10f);
    public float explosionDelay = 3f;
    public GameObject explosionEffect;

    public int OwnerID => ownerID;
    public bool IsHeld => isHeld;

    private Transform holder;
    private bool hasPlaced = false;

    protected override void Awake()
    {
        base.Awake();

        weaponDamage = 20f;          // �ʼǱ���ը�˺�
        hitDistance = 0f;            // �޻���Ч������Ϊ0
        attackCheckRadius = 5f;      // ��ը��Χ�뾶��10x10��İ뾶��
    }

    void Update()
    {
        if (!isHeld || hasPlaced) return;

        if ((ownerID == 1 && Input.GetKeyDown(KeyCode.J)) ||
            (ownerID == 2 && Input.GetKeyDown(KeyCode.Keypad1)))
        {
            PlaceAndExplode();
        }
    }

    public void SetOwner(int playerID, Transform holderTransform, Transform _)
    {
        ownerID = playerID;
        isHeld = true;
        holder = holderTransform;

        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        if (col != null) col.enabled = false;
        if (rb != null) rb.simulated = false;
    }

    public void DropWeapon()
    {
        isHeld = false;
        ownerID = -1;
        if (holder != null)
        {
            transform.position = holder.position + Vector3.right * 1.5f;
        }
        else
        {
            transform.position += Vector3.right * 1.5f; // ��ֱ������Ĭ��λ��
        }
        transform.SetParent(null);
        
        if (col != null) col.enabled = true;
        if (rb != null) rb.simulated = true;
        holder = null;
    }

    private void PlaceAndExplode()
    {
        hasPlaced = true;
        isHeld = false;

        transform.SetParent(null);
        transform.position = holder.position;
        holder = null;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (col != null) col.enabled = true;

        StartCoroutine(HandleDelayedExplosion());

    }

    IEnumerator HandleDelayedExplosion()
    {
        // �ȴ�3��
        yield return new WaitForSeconds(explosionDelay);

        // ���ű�ը��Ч�Ͷ���
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // ���ű�ը����
        if (anim != null)
        {
            AnimPlay("Special"); // ȷ������������ȷ

            // �ȴ������������
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
            PerformAttack();
    }

        private void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, explosionSize, 0f);
        foreach (Collider2D hit in hits)
        {
            Fighter fighter = hit.GetComponent<Fighter>();
            if (fighter != null && fighter.playerID != ownerID)
            {
                EnemyHealth hp = hit.GetComponent<EnemyHealth>();
                if (hp != null) hp.TakeDamage(damage);
            }
        }

        Destroy(gameObject);


    }



    public void PerformAttack()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, explosionSize, 0f);
        foreach (Collider2D hit in hits)
        {
            Fighter fighter = hit.GetComponent<Fighter>();
            if (fighter != null && fighter.playerID != ownerID)
            {
                EnemyHealth hp = hit.GetComponent<EnemyHealth>();
                if (hp != null) hp.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

   


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, explosionSize);
    }
}
