using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFinished : MonoBehaviour
{
    private Player player =>GetComponentInParent<Player>();
    private Weapon weapon => player.weapon;

    public event AnimationFinishedDelegate animFinishHealder;

    public void Awake()
    {
        //������Ƿ���ε���
        animFinishHealder += AttackTrigger;
    }

    public void AttackFinish()
    {
        if(weapon != null)
        {
            //���������������������������˺��ͻ���
            animFinishHealder?.Invoke(weapon.weaponDamage,weapon.hitDistance); 
        }else
        {
            //������������Ϊ����
            animFinishHealder?.Invoke(1f,0);
        }
        player.isTriggerCalled = true;
    }

    public void AttackTrigger(float damage,float hitDistance)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheckPoint.position, weapon.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage(damage, weapon.hitDistance);
                hit.GetComponent<Player>().rb.AddForce(new Vector2(player.facingDir * hitDistance, hitDistance));
            }
        }
    }

}

public delegate void AnimationFinishedDelegate(float damage,float HitDistance);
