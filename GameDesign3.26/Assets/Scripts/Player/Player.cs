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

    public bool isTriggerCalled = false;
    public Transform attackCheckPoint;
    public Weapon weapon = null;

    #region State
    public Player1IdleState idleState {  get; private set; }
    public Player1MoveState moveState {  get; private set; }
    #endregion


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new Player1IdleState(stateMachine,this,"Idle");
        moveState = new Player1MoveState(stateMachine,this,"Move");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(float xVelocity)
    {
        rb.velocity = new Vector2 (xVelocity, 0);
        CheckFlip(xVelocity);
    }
     //检测地面,不一定加
     public bool isGroundDetected() => Physics2D.Raycast(transform.position, new Vector2(transform.position.x, transform.position.y + 1));

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

    public void Damage(float damage,float forceStrength)
    {
        health -= damage;
        //待修改
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            for (int i = 0; i < 6; i++) 
            { 
                if(collision.gameObject == GameManager.Instance.arsenal[i])
                {
                    weapon = collision.gameObject.GetComponent<Weapon>();
                    break;
                }
            }
            Destroy(collision.gameObject);
            //进入武器状态
        }
    }
}
