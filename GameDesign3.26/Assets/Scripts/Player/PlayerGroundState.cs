using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //可以调整
        if(player.payerID==1 && Input.GetKeyDown(KeyCode.W))
        {
            stateMachine.ChangeState(player.jumpState);
        }
        else if(player.payerID==2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            stateMachine.ChangeState(player.jumpState);
        }
        //攻击
        if (player.weapon == null)
        {
            if (player.payerID == 1 && Input.GetKeyDown(KeyCode.J))
            {
                stateMachine.ChangeState(player.attackState);
            }
            else if (player.payerID == 2 && Input.GetKeyDown(KeyCode.Keypad1))
            {
                stateMachine.ChangeState(player.attackState);
            }
        }
    }
}
