using UnityEngine;

/// <summary>
/// 武器的统一父类，存储武器的通用属性
/// </summary>
public class Weapon : MonoBehaviour
{
    [Header("统一武器参数")]
    public float weaponDamage;          // 武器基础伤害
    public float hitDistance;           // 武器击退格数（策划案内要求）
    public float attackCheckRadius;     // 攻击检测半径（用于通用的OverlapCircle检测）

    [Header("基础组件")]
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected Animator anim;

    //public bool isTriggerCalled = false;
    public string groundAnimName;
    protected string boolName;

    protected virtual void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    protected void AnimPlay(string boolName)
    {
        anim.SetBool(boolName, true);
    }

    public void EndAnim(string boolName)
    {
        anim.SetBool(boolName, false);
    }
}
