using Unity.Burst.CompilerServices;
using UnityEngine;

public class BrushWeapon : Weapon, IWeapon
{
    public string weaponName = "动院画笔";
    public int damage = 3;
    public float knockbackForce = 2f;
    public Vector2 attackSize = new Vector2(2f, 2f);

    public int ownerID = -1;
    public bool isHeld = false;

    public int OwnerID => ownerID;
    public bool IsHeld => isHeld;

    private Transform holder;
    private Transform attackOrigin;
    private Player player;

    protected override void Awake()
    {
        base.Awake(); // 先调用Weapon类的Awake()

        weaponDamage = 3f;            // 画笔伤害为3
        hitDistance = 4f;             // 画笔击退为4格（策划案要求）
        attackCheckRadius = 2f;       // 根据武器范围2x2格设定一个半径即可
    }

    void Update()
    {
        if (!isHeld) return;

        if ((ownerID == 1 && Input.GetKeyDown(KeyCode.J)) ||
            (ownerID == 2 && Input.GetKeyDown(KeyCode.Keypad1)))
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(attackOrigin.position, attackSize, 0f);
            if (hits.Length > 0 )
            {
               foreach (Collider2D hit in hits)
               {
                    if(hit.gameObject.tag == "Player")
                    {
                        AnimPlay("Special");
                        return;
                    }
               }
            }
            AnimPlay("Attack");
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
        player = holder.gameObject.GetComponent<Player>();
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

    public void PerformAttack() // 确认方法已存在且正确，不需额外修改
    {
        if (attackOrigin == null) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackOrigin.position, attackSize, 0f);
        foreach (Collider2D hit in hits)
        {
            Fighter fighter = hit.GetComponent<Fighter>();
            if (fighter != null && fighter.playerID != ownerID)
            {
                EnemyHealth hp = hit.GetComponent<EnemyHealth>();
                if (hp != null) hp.TakeDamage(damage);
                Transform enemyTf = hit.transform;
                Vector2 knockDir = (enemyTf.position - attackOrigin.position).normalized;
                enemyTf.position += (Vector3)(knockDir * knockbackForce);
                //hit.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackForce*holder.parent.GetComponent<Player>().facingDir, knockbackForce));
                //Debug.Log($"【画笔击退】{hit.name} 击退 {knockDir} → {enemyTf.position}");
            }
            //if(hit.gameObject.tag == "Player")
            //{
            //    hit.GetComponent<Player>().isHit = true;
            //    hit.GetComponent<Player>().Damage(damage);
            //    hit.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.GetComponent<Player>().facingDir*knockbackForce, knockbackForce), ForceMode2D.Impulse);

            //}
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackOrigin.position, attackSize);
        }
    }
}
