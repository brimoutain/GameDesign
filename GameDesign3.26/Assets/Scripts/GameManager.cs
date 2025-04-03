using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        weapon1Number = UnityEngine.Random.Range(0, 4);
        do
        {
            weapon2Number = UnityEngine.Random.Range(0, 4);
        }
        while (weapon1Number == weapon2Number);
        arsenal = new GameObject[5];
        arsenal[0] = weapon1;
        arsenal[1] = weapon2;
        arsenal[2] = weapon3;
        arsenal[3] = weapon4;
        arsenal[4] = weapon5;

        //记得给武器预制体加rigidbody2d
        Instantiate(arsenal[weapon1Number], weapon1Point.position, Quaternion.Euler(0, 180, 0));
        Instantiate(arsenal[weapon2Number], weapon2Point.position, Quaternion.identity);
    }

    private void Update()
    {
        if(player1.health <= 0 || player2.health <= 0)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
