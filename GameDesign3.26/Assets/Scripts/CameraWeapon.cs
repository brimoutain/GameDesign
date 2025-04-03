using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 影院摄像机武器：使用后截屏，所有玩家冻结10秒（包括自己）
/// </summary>
public class CameraWeapon : Weapon, IWeapon
{
    [Header("冻结设置")]
    public float freezeDuration = 10f;

    [Header("状态")]
    public int ownerID = -1;
    public bool isHeld = false;

    public int OwnerID => ownerID;
    public bool IsHeld => isHeld;

    private Transform holder;
    private bool hasUsed = false;
    public bool isFrozen = false;

    protected override void Awake()
    {
        base.Awake();
        weaponDamage = 0;
        hitDistance = 0;
        attackCheckRadius = 0;
    }

    void Update()
    {
        if (!isHeld) return;

        if ((ownerID == 1 && Input.GetKeyDown(KeyCode.J)) ||
            (ownerID == 2 && Input.GetKeyDown(KeyCode.Keypad1)))
        {
            if (!isFrozen)
            {
                AnimPlay("Special");
            }
        }
    }

    public void SetOwner(int playerID, Transform holderTransform, Transform _)
    {
        ownerID = playerID;
        isHeld = true;
        holder = holderTransform;
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;

        if (col != null) col.enabled = false;
        if (rb != null) rb.simulated = false;
    }

    public void DropWeapon()
    {
        isHeld = false;
        ownerID = -1;
        transform.SetParent(null);
        transform.position = holder.position + Vector3.right * 1.5f;

        if (col != null) col.enabled = true;
        if (rb != null) rb.simulated = true;
        holder = null;
    }

    public void PerformAttack()
    {       
        if (hasUsed) return;
        hasUsed = true;
        isHeld = false;
        transform.SetParent(null);

        // 冻结玩家
        Player[] allPlayers = FindObjectsOfType<Player>();
        foreach (Player player in allPlayers)
        {
            if (player.payerID != ownerID)
            {
                player.FreezePlayer();
            }
        }

        StartCoroutine(FreezeWithScreenshotAndDestroy());
    }

    private IEnumerator FreezeWithScreenshotAndDestroy()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        GameObject canvasGO = new GameObject("FreezePhotoCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject imageGO = new GameObject("FreezeImage");
        imageGO.transform.SetParent(canvasGO.transform);
        RawImage img = imageGO.AddComponent<RawImage>();
        img.texture = screenshot;
        img.color = new Color(0.7f, 0.7f, 0.7f, 1f);

        RectTransform rt = imageGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        yield return new WaitForSeconds(freezeDuration);

        // 解冻玩家
        Player[] allPlayers = FindObjectsOfType<Player>();
        foreach (Player player in allPlayers)
            player.UnfreezePlayer();

        Destroy(screenshot);
        Destroy(canvasGO);
        Destroy(gameObject);

        Debug.Log("✅ 所有玩家已解冻");
    }
}
