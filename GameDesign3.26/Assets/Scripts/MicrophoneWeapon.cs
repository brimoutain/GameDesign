using UnityEngine;
using System.Collections;

/// <summary>
/// 播音同学麦克风武器：远程音波，最多发射10次，直线穿透伤敌并击退
/// </summary>
public class MicrophoneWeapon : Weapon, IWeapon
{
    [Header("武器属性")]
    public int damage = 5;
    public float knockbackForce = 2f;
    public float waveSpeed = 3f;
    public int maxShots = 10;
    public float waveLength = 20f; // 最远检测范围

    [Header("状态")]
    public int ownerID = -1;
    public bool isHeld = false;
    private int shotsFired = 0;

    [Header("视觉")]
    public GameObject waveEffectPrefab; // 可选音波动画特效
    public Transform attackOrigin;
    public SpriteRenderer spriteRenderer;

    public int OwnerID => ownerID;
    public bool IsHeld => isHeld;

    private Transform holder;

    protected override void Awake()
    {
        base.Awake();

        weaponDamage = 5f;            // 麦克风伤害
        hitDistance = 2f;             // 击退2格
        attackCheckRadius = waveLength; // 音波射程长度
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

        // 播放特效（不生成物体，只用视觉）
        if (waveEffectPrefab != null && attackOrigin != null)
        {
            GameObject wave = Instantiate(waveEffectPrefab, attackOrigin.position, Quaternion.identity);
            wave.transform.localScale = new Vector3(dir.x, 1, 1);
            Destroy(wave, 1f);
        }

        // 发出射线检测敌人
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
                    Debug.Log($"【麦克风击退】{hit.collider.name} 按音波方向 {knockDir} 强制退 2 格 → {enemyTf.position}");

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
        if (shotsFired >= maxShots) return; // 防止超限攻击次数
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
                Debug.Log($"【麦克风击退】{hit.collider.name} 向{knockDir}强制退{knockbackForce}格 → {enemyTf.position}");
            }
        }

        if (shotsFired >= maxShots)
        {
            Debug.Log("【麦克风】次数用完，武器销毁");
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
