using UnityEngine;
using System.Collections;

/// <summary>
/// ����ͬѧ��˷�������Զ����������෢��10�Σ�ֱ�ߴ�͸�˵в�����
/// </summary>
public class MicrophoneWeapon : Weapon, IWeapon
{
    [Header("��������")]
    public int damage = 5;
    public float knockbackForce = 2f;
    public float waveSpeed = 3f;
    public int maxShots = 10;
    public float waveLength = 20f; // ��Զ��ⷶΧ

    [Header("״̬")]
    public int ownerID = -1;
    public bool isHeld = false;
    private int shotsFired = 0;

    [Header("�Ӿ�")]
    public GameObject waveEffectPrefab; // ��ѡ����������Ч
    public Transform attackOrigin;
    public SpriteRenderer spriteRenderer;

    public int OwnerID => ownerID;
    public bool IsHeld => isHeld;

    private Transform holder;

    protected override void Awake()
    {
        base.Awake();

        weaponDamage = 5f;            // ��˷��˺�
        hitDistance = 2f;             // ����2��
        attackCheckRadius = waveLength; // ������̳���
    }


    void Update()
    {
        if (!isHeld || shotsFired >= maxShots) return;

        if ((ownerID == 1 && Input.GetKeyDown(KeyCode.J)) ||
            (ownerID == 2 && Input.GetKeyDown(KeyCode.Keypad1)))
        {
            FireWave();
        }
    }

    public void SetOwner(int playerID, Transform holderTransform, Transform attackPoint)
    {
        ownerID = playerID;
        isHeld = true;
        holder = holderTransform;
        attackOrigin = attackPoint;

        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;

        if (col != null) col.enabled = false;
        if (rb != null) rb.simulated = false;
    }

    public void DropWeapon()
    {
        isHeld = false;
        ownerID = -1;

        transform.SetParent(null);
        transform.position = holder.position + Vector3.right * 1.5f;

        if (col != null) col.enabled = true;
        if (rb != null) rb.simulated = true;

        holder = null;
        attackOrigin = null;
    }

    private void FireWave()
    {
        shotsFired++;

        Vector2 dir = holder.localScale.x >= 0 ? Vector2.right : Vector2.left;

        // ������Ч�����������壬ֻ���Ӿ���
        if (waveEffectPrefab != null && attackOrigin != null)
        {
            GameObject wave = Instantiate(waveEffectPrefab, attackOrigin.position, Quaternion.identity);
            wave.transform.localScale = new Vector3(dir.x, 1, 1);
            Destroy(wave, 1f);
        }

        // �������߼�����
        RaycastHit2D[] hits = Physics2D.RaycastAll(attackOrigin.position, dir, waveLength);
        foreach (RaycastHit2D hit in hits)
        {
            Fighter f = hit.collider.GetComponent<Fighter>();
            if (f != null && f.playerID != ownerID)
            {
                EnemyHealth hp = hit.collider.GetComponent<EnemyHealth>();
                if (hp != null) hp.TakeDamage(damage);
                Rigidbody2D enemyRb = hit.collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Transform enemyTf = hit.collider.transform;
                    Vector2 knockDir = (enemyTf.position - attackOrigin.position).normalized;
                    enemyTf.position += (Vector3)(knockDir * 2f);
                    Debug.Log($"����˷���ˡ�{hit.collider.name} ���������� {knockDir} ǿ���� 2 �� �� {enemyTf.position}");

                }

            }
        }

        if (shotsFired >= maxShots)
        {
            Destroy(gameObject);
        }
    }


    public void PerformAttack()
    {
        if (shotsFired >= maxShots) return; // ��ֹ���޹�������
        shotsFired++;

        Vector2 dir = holder.localScale.x >= 0 ? Vector2.right : Vector2.left;

        if (waveEffectPrefab != null && attackOrigin != null)
        {
            GameObject wave = Instantiate(waveEffectPrefab, attackOrigin.position, Quaternion.identity);
            wave.transform.localScale = new Vector3(dir.x, 1, 1);
            Destroy(wave, 1f);
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(attackOrigin.position, dir, waveLength);
        foreach (RaycastHit2D hit in hits)
        {
            Fighter f = hit.collider.GetComponent<Fighter>();
            if (f != null && f.playerID != ownerID)
            {
                EnemyHealth hp = hit.collider.GetComponent<EnemyHealth>();
                if (hp != null) hp.TakeDamage(damage);

                Transform enemyTf = hit.collider.transform;
                Vector2 knockDir = (enemyTf.position - attackOrigin.position).normalized;
                enemyTf.position += (Vector3)(knockDir * knockbackForce);
                Debug.Log($"����˷���ˡ�{hit.collider.name} ��{knockDir}ǿ����{knockbackForce}�� �� {enemyTf.position}");
            }
        }

        if (shotsFired >= maxShots)
        {
            Debug.Log("����˷硿�������꣬��������");
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackOrigin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(attackOrigin.position, Vector2.right * waveLength);
            Gizmos.DrawRay(attackOrigin.position, Vector2.left * waveLength);
        }
    }
}
