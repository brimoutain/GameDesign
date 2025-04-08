using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Sprite hitSprite;
    private Sprite originSprite;

    private void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        originSprite = sr.sprite;
    }

    public IEnumerator FlashFx()
    {
        Debug.Log("协程开始执行"); // 添加调试语句
        sr.gameObject.GetComponent<Animator>().enabled = false;
        sr.sprite = hitSprite;

        yield return new WaitForSeconds(.2f);

        sr.sprite = originSprite;
        sr.gameObject.GetComponent<Animator>().enabled = false;
        Debug.Log("协程执行完毕");
    }

}