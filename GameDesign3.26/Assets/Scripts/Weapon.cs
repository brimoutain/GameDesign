using UnityEngine;

/// <summary>
/// ������ͳһ���࣬�洢������ͨ������
/// </summary>
public class Weapon : MonoBehaviour
{
    [Header("ͳһ��������")]
    public float weaponDamage;          // ���������˺�
    public float hitDistance;           // �������˸������߻�����Ҫ��
    public float attackCheckRadius;     // �������뾶������ͨ�õ�OverlapCircle��⣩

    [Header("�������")]
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
