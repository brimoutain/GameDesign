using UnityEngine;
using System.Collections;

/// <summary>
/// 信通同学锤子：可切换大小锤，蓄力攻击，击退敌人，统一接口结构
/// </summary>
public class HammerWeapon : Weapon, IWeapon
{
    public enum HammerMode { Small, Large }
    public HammerMode mode = HammerMode.Small;

    [Header("武器状态")]
    public int ownerID = -1;
    public bool isHeld = false;
    public int OwnerID => ownerID;
    public bool IsHeld => isHeld;

    private Transform holder;
    private Transform attackOrigin;

    [Header("锤子参数")]
    public int maxUses = 5;
    private int useCount = 0;

    //被我修改
    public float smallChargeTime = 1f;
    public float largeChargeTime = 2f;
    public int smallDamage = 14;
    public int largeDamage = 18;

    public int smallKnockbackGrids = 1;
    public int largeKnockbackGrids = 2;

    public Vector2 smallRange = new Vector2(3f, 1f); // 横向3格
    public Vector2 largeRange = new Vector2(5f, 1f); // 横向5格

    private float chargeTimer = 0f;
    private bool isCharging = false;

    [Header("视觉")]
    public Sprite smallHammerSprite;
    public Sprite largeHammerSprite;
    public SpriteRenderer spriteRenderer;


    protected override void Awake()
    {
        base.Awake();

        // 以大锤为基准或根据情况动态调整
        weaponDamage = 18f;           // 默认以大锤伤害为基准（后续动态调整）
        hitDistance = 6f;             // 默认以大锤击退为基准（后续动态调整）
        attackCheckRadius = 2.5f;     // 根据大锤范围(5格)取半径2.5

        boolName = "Attack";
    }

    void Update()
    {
        if (!isHeld) return;

        bool pressKey = (ownerID == 1 && Input.GetKey(KeyCode.J)) || (ownerID == 2 && Input.GetKey(KeyCode.Keypad1));
        bool releaseKey = (ownerID == 1 && Input.GetKeyUp(KeyCode.J)) || (ownerID == 2 && Input.GetKeyUp(KeyCode.Keypad1));
        bool switchKey = (ownerID == 1 && Input.GetKeyDown(KeyCode.H)) || (ownerID == 2 && Input.GetKeyDown(KeyCode.Keypad5));
        if (switchKey)
        {
            mode = mode == HammerMode.Small ? HammerMode.Large : HammerMode.Small;

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = (mode == HammerMode.Small) ? smallHammerSprite : largeHammerSprite;
            }

            Debug.Log($"【锤子】[{ownerID}] 切换为 {(mode == HammerMode.Small ? "小锤" : "大锤")}");
        }


        if (pressKey)
        {
            isCharging = true;
            chargeTimer += Time.deltaTime;
            Debug.Log(chargeTimer);

            float required = (mode == HammerMode.Small) ? smallChargeTime : largeChargeTime;
            Debug.Log (required);
            if (chargeTimer >= required)
            {
                AnimPlay("Attack");
                chargeTimer = 0f;
                isCharging = false;
            }
        }
        //有待修改
        if (releaseKey && isCharging)
        {
            chargeTimer = 0f;
            isCharging = false;
            Debug.Log($"【锤子】[{ownerID}] 蓄力未满，攻击取消");
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

    // 在HammerWeapon类中新增或修改：
    public void PerformAttack()
    {
        if (++useCount > maxUses)
        {
            Debug.Log($"【锤子】[{ownerID}] 用完{maxUses}次，武器销毁");
            Destroy(gameObject);
            return;
        }

        Vector2 range = mode == HammerMode.Small ? smallRange : largeRange;
        int damage = mode == HammerMode.Small ? smallDamage : largeDamage;
        float knockGrids = mode == HammerMode.Small ? smallKnockbackGrids : largeKnockbackGrids;

        Vector2 dir = attackOrigin.right.normalized;
        Vector2 center = (Vector2)attackOrigin.position + dir * (range.x / 2f);
        Vector2 size = range;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);
        foreach (Collider2D hit in hits)
        {
            Fighter f = hit.GetComponent<Fighter>();
            if (f != null && f.playerID != ownerID)
            {
                EnemyHealth hp = hit.GetComponent<EnemyHealth>();
                if (hp != null)
                {
                    hp.TakeDamage(damage);
                    Debug.Log($"【锤子】命中 {hit.name}，伤害：{damage}");
                }

                Transform enemyTf = hit.transform;
                Vector2 knockDir = (enemyTf.position - attackOrigin.position).normalized;
                enemyTf.position += (Vector3)(knockDir * knockGrids);
                Debug.Log($"【锤子击退】{hit.name} 向{knockDir}强制退{knockGrids}格 → {enemyTf.position}");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;

        Vector2 range = mode == HammerMode.Small ? smallRange : largeRange;
        Vector2 center = (Vector2)attackOrigin.position + Vector2.right * (range.x / 2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, range);
    }
}
