using UnityEngine;
using System.Collections;

/// <summary>
/// ��ͨͬѧ���ӣ����л���С�����������������˵��ˣ�ͳһ�ӿڽṹ
/// </summary>
public class HammerWeapon : Weapon, IWeapon
{
    public enum HammerMode { Small, Large }
    public HammerMode mode = HammerMode.Small;

    [Header("����״̬")]
    public int ownerID = -1;
    public bool isHeld = false;
    public int OwnerID => ownerID;
    public bool IsHeld => isHeld;

    private Transform holder;
    private Transform attackOrigin;

    [Header("���Ӳ���")]
    public int maxUses = 5;
    private int useCount = 0;

    //�����޸�
    public float smallChargeTime = 1f;
    public float largeChargeTime = 2f;
    public int smallDamage = 14;
    public int largeDamage = 18;

    public int smallKnockbackGrids = 1;
    public int largeKnockbackGrids = 2;

    public Vector2 smallRange = new Vector2(3f, 1f); // ����3��
    public Vector2 largeRange = new Vector2(5f, 1f); // ����5��

    private float chargeTimer = 0f;
    private bool isCharging = false;

    [Header("�Ӿ�")]
    public Sprite smallHammerSprite;
    public Sprite largeHammerSprite;
    public SpriteRenderer spriteRenderer;


    protected override void Awake()
    {
        base.Awake();

        // �Դ�Ϊ��׼����������̬����
        weaponDamage = 18f;           // Ĭ���Դ��˺�Ϊ��׼��������̬������
        hitDistance = 6f;             // Ĭ���Դ󴸻���Ϊ��׼��������̬������
        attackCheckRadius = 2.5f;     // ���ݴ󴸷�Χ(5��)ȡ�뾶2.5

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

            Debug.Log($"�����ӡ�[{ownerID}] �л�Ϊ {(mode == HammerMode.Small ? "С��" : "��")}");
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
        //�д��޸�
        if (releaseKey && isCharging)
        {
            chargeTimer = 0f;
            isCharging = false;
            Debug.Log($"�����ӡ�[{ownerID}] ����δ��������ȡ��");
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

    // ��HammerWeapon�����������޸ģ�
    public void PerformAttack()
    {
        if (++useCount > maxUses)
        {
            Debug.Log($"�����ӡ�[{ownerID}] ����{maxUses}�Σ���������");
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
                    Debug.Log($"�����ӡ����� {hit.name}���˺���{damage}");
                }

                Transform enemyTf = hit.transform;
                Vector2 knockDir = (enemyTf.position - attackOrigin.position).normalized;
                enemyTf.position += (Vector3)(knockDir * knockGrids);
                Debug.Log($"�����ӻ��ˡ�{hit.name} ��{knockDir}ǿ����{knockGrids}�� �� {enemyTf.position}");
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
