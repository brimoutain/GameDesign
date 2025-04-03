using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    private PlayerStateMachine stateMachine;

    private bool isFaceRight = true;
    public float facingDir = 1;

    public float health = 100;
    public float moveSpeed;

    public bool isTriggerCalled = false;
    public Transform attackCheckPoint;
    public Weapon weapon = null;

    public int payerID;
    [SerializeField] private LayerMask whatIsGround;
    public Transform groundCheckPoint;

    public bool isHit = false;

    #region State
    public PlayerIdleState idleState {  get; private set; }
    public PlayerMoveState moveState {  get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerGroundState groundState { get; private set; }
    public PlayerAirState airState { get; private set; }
    #endregion


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(stateMachine,this,"Idle");
        moveState = new PlayerMoveState(stateMachine,this,"Move");
        jumpState = new PlayerJumpState(stateMachine,this,"Jump");
        attackState = new PlayerAttackState(stateMachine, this, "Attack");
        groundState = new PlayerGroundState(stateMachine, this, "Idle");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        
        stateMachine.InitializeState(idleState);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(payerID == 2)
        {
            isFaceRight = false;
        }
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        //Debug.Log(stateMachine.currentState.ToString());
    }

    public void SetVelocity(float xVelocity)
    {
        rb.velocity = new Vector2 (xVelocity, rb.velocity.y);
        CheckFlip(xVelocity);
    }
     //检测地面,不一定加
     public bool isGroundDetected() => Physics2D.Raycast(groundCheckPoint.position, Vector2.down,.5f,whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckPoint.position, new Vector3(groundCheckPoint.position.x, groundCheckPoint.position.y - .5f));
    }
    #region Flip
    private void CheckFlip(float _x)
    {
        if(_x>0 && !isFaceRight)
        {
            Flip();
        }else if(_x<0 && isFaceRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFaceRight = !isFaceRight;
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
    }
    #endregion

    public void Damage(float damage)
    {
        health -= damage;
        //待修改       
    }

    public void FreezePlayer()
    {
        // 禁用状态机，禁止任何状态更新
        this.enabled = false;

        // 禁用动画播放
        if (anim != null) anim.enabled = false;

        // 停止刚体运动
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;  // 防止任何外力影响
        }

        Debug.Log($"{gameObject.name}已被冻结");
    }

    public void UnfreezePlayer()
    {
        // 启用状态机，恢复状态更新
        this.enabled = true;

        // 启用动画播放
        if (anim != null) anim.enabled = true;

        // 恢复刚体运动状态
        if (rb != null) rb.isKinematic = false;

        Debug.Log($"{gameObject.name}已解除冻结");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            // 丢弃现有武器（如果有）
            if (weapon != null && weapon.TryGetComponent<IWeapon>(out var currentWeapon))
            {
                currentWeapon.DropWeapon();
            }

            // 拾取新武器
            weapon = collision.gameObject.GetComponent<Weapon>();
            if (weapon.TryGetComponent<IWeapon>(out var newWeaponLogic))
            {
                    newWeaponLogic.SetOwner(this.GetComponent<Fighter>().playerID, attackCheckPoint, attackCheckPoint);
            }
            //记得设置bool
            anim.SetBool(collision.ToString(), true);
        }
    }

}
