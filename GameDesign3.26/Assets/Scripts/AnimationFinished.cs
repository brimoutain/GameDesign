using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        player.isTriggerCalled = true;
        
        if (player.weapon != null
    && player.weapon.TryGetComponent<IWeapon>(out var weaponLogic)
    && !(player.weapon is CameraWeapon cameraWeapon && cameraWeapon.isFrozen))
        {
            weapon.EndAnim("Attack");
            weapon.EndAnim("Special");
            weaponLogic.PerformAttack(); // ���õ�ǰ����ͳһ��������
        }
        else
        {
            AttackTrigger(1, 1f);
        }
    }

    public void AttackTrigger(int damage, float hitDistance)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.attackCheckPoint.position, 1);
        foreach (Collider2D hit in hits)
        {
            Fighter fighter = hit.GetComponent<Fighter>();
            if (fighter != null && fighter.playerID != player.payerID)
            {
                EnemyHealth hp = hit.GetComponent<EnemyHealth>();
                if (hp != null) hp.TakeDamage(damage);
                //Transform enemyTf = hit.transform;
                //Vector2 knockDir = (enemyTf.position - attackOrigin.position).normalized;
                //enemyTf.position += (Vector3)(knockDir * knockbackForce);
                hit.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(hitDistance*player.facingDir, hitDistance));
                //Debug.Log($"�����ʻ��ˡ�{hit.name} ���� {knockDir} �� {enemyTf.position}");
            }
        }
    }

}

public delegate void AnimationFinishedDelegate(int damage,float HitDistance);
