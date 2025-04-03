using UnityEngine;


public interface IWeapon
{
    bool IsHeld { get; }
    int OwnerID { get; }

    void SetOwner(int playerID, Transform holderTransform, Transform attackPoint);
    void DropWeapon();

    void PerformAttack(); // ����ͳһ��������

}
