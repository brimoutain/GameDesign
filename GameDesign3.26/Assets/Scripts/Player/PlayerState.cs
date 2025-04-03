using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public Player player;
    public PlayerStateMachine stateMachine;
    private string animBoolName;
    private string weaponBoolName;

    public float xInput;

    public float stateTimer;
    public bool isWeapon = false;

    public PlayerState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        if (player.payerID == 1)
        {
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                xInput = -1;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                xInput = 1;
            }
            else xInput = 0;
        }
        else if (player.payerID == 2)
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                xInput = -1;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                xInput = 1;
            }
            else xInput = 0;
        }
        player.anim.SetFloat("yVelocity",player.rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
