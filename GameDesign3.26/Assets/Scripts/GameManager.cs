using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player1;
    public Player player2;

    public GameObject[] arsenal;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject weapon4;
    public GameObject weapon5;
    public GameObject weapon6;
    private int weapon1Number;//随机数决定武器
    private int weapon2Number;
    public Transform weapon1Point;
    public Transform weapon2Point;

    private void Start()
    {
        CreateWeapon();
    }

    private void CreateWeapon()
    {
        weapon1Number = UnityEngine.Random.Range(0, 6);
        do
        {
            weapon2Number = UnityEngine.Random.Range(0, 6);
        }
        while (weapon1Number == weapon2Number);
        arsenal = new GameObject[6];
        arsenal[0] = weapon1;
        arsenal[1] = weapon2;
        arsenal[2] = weapon3;
        arsenal[3] = weapon4;
        arsenal[4] = weapon5;
        arsenal[5] = weapon6;

        //记得给武器预制体加rigidbody2d
        Instantiate(arsenal[weapon1Number], weapon1Point.position, Quaternion.identity);
        Instantiate(arsenal[weapon2Number], weapon2Point.position, Quaternion.identity);
    }

    private void Update()
    {
        if(player1.health <= 0 || player2.health <= 0)
        {
            //弹出暂停界面
        }
    }
}
