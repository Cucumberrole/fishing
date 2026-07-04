using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    public GameObject Lure;
    public Transform Rodtip;
    public LineRenderer line;
    public Transform playerTarget;
    public FishGetUI fishGetUIPrefab;

    [Header("投げる方向")]
    public Vector2 throwDirection = new(1f, 1f);

    [Header("糸の見た目")]
    public int segmentCount = 20;
    public float curveHeight = 2f;

    [Header("プレイヤー画像")]
    public SpriteRenderer playerSpriteRenderer;
    public Sprite idleSprite;
    public Sprite chargeSprite;
    public Sprite castSprite;
    public Sprite pullSprite;
    public Sprite getSprite;
    public float castSpriteDuration = 0.2f;
    public float pullSpriteDuration = 0.5f;
    public float getSpriteDuration = 0.5f;

    private float currentChargeTime;
    private bool isCharging;
    private bool isReeling;
    private Rigidbody2D lureRigidbody;
    private float playerSpriteTimer;
    private Sprite nextSprite;
    private Coroutine playerSpriteCoroutine;

    private readonly List<Fish> caughtFishes = new();
    private readonly List<Fish> launchedFishes = new();

    private float MinPower => GManager.instance != null ? GManager.instance.minThrowPower : 5f;
    private float MaxPower => GManager.instance != null ? GManager.instance.maxThrowPower : 20f;
    private float MaxChargeTime => GManager.instance != null ? GManager.instance.maxChargeTime : 2f;
    private float ReelSpeed => GManager.instance != null ? GManager.instance.reelSpeed : 5f;

    private void Start()
    {
        if (Lure != null) { lureRigidbody = Lure.GetComponent<Rigidbody2D>(); if (lureRigidbody != null) lureRigidbody.simulated = false; }
        if (line != null) { line.positionCount = segmentCount; line.enabled = false; }
        if (playerSpriteRenderer == null) playerSpriteRenderer = GetComponent<SpriteRenderer>();
        SetPlayerSprite(idleSprite);
    }

    private void Update()
    {
        if (Lure == null || Rodtip == null || lureRigidbody == null) return;
        UpdateThrowInput();
        UpdateReeling();
        UpdateFishingLine();
        RegisterCaughtFishes();
        UpdateLaunchedFishes();
        UpdatePlayerSpriteTimer();
    }

    private void UpdateThrowInput()
    {
        if (Input.GetMouseButtonDown(0) && !isReeling && (line == null || !line.enabled))
        {
            SetPlayerSprite(chargeSprite);
            isCharging = true;
            currentChargeTime = 0f;
            Lure.transform.position = Rodtip.position;
            lureRigidbody.linearVelocity = Vector2.zero;
            lureRigidbody.angularVelocity = 0f;
            lureRigidbody.simulated = false;
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, MaxChargeTime);
            Lure.transform.position = Rodtip.position;
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            if (line != null) line.enabled = true;
            if (AudioManager.Instance != null) AudioManager.Instance.PlayRodCastSE();
            SetPlayerSpriteForSeconds(castSprite, castSpriteDuration, idleSprite);
            isCharging = false;
            float chargeRate = Mathf.Clamp01(currentChargeTime / Mathf.Max(MaxChargeTime, 0.01f));
            float throwPower = Mathf.Lerp(MinPower, MaxPower, chargeRate);
            lureRigidbody.simulated = true;
            lureRigidbody.AddForce(throwDirection.normalized * throwPower, ForceMode2D.Impulse);
            currentChargeTime = 0f;
        }

        if (Input.GetMouseButtonDown(1) && !isReeling)
        {
            Vector3 launchOrigin = Lure.transform.position;
            if (line != null) line.enabled = false;
            if (caughtFishes.Count > 0) { if (AudioManager.Instance != null) AudioManager.Instance.PlayCatchSplashSE(); PlayPullGetSpriteSequence(); }
            isCharging = false;
            currentChargeTime = 0f;
            Lure.transform.position = Rodtip.position;
            lureRigidbody.linearVelocity = Vector2.zero;
            lureRigidbody.angularVelocity = 0f;
            lureRigidbody.simulated = false;
            LaunchCaughtFishes(launchOrigin);
        }
    }

    private void UpdateReeling()
    {
        if (!isReeling) return;
        Lure.transform.position = Vector2.MoveTowards(Lure.transform.position, Rodtip.position, ReelSpeed * Time.deltaTime);

        for (int i = 0; i < caughtFishes.Count; i++)
        {
            if (caughtFishes[i] != null) caughtFishes[i].transform.position = Lure.transform.position + Vector3.down * (0.7f * (i + 1));
        }

        if (Vector2.Distance(Lure.transform.position, Rodtip.position) < 0.5f)
        {
            Vector3 launchOrigin = Lure.transform.position;
            Lure.transform.position = Rodtip.position;
            isReeling = false;
            if (line != null) line.enabled = false;
            LaunchCaughtFishes(launchOrigin);
        }
    }

    private void LaunchCaughtFishes(Vector3 launchOrigin)
    {
        Transform target = playerTarget != null ? playerTarget : Rodtip;

        for (int i = 0; i < caughtFishes.Count; i++)
        {
            Fish fish = caughtFishes[i];
            if (fish == null) continue;
            fish.transform.position = launchOrigin + Vector3.down * (0.6f * i);
            fish.BeginLaunch(target, i);
            if (!launchedFishes.Contains(fish)) launchedFishes.Add(fish);
        }

        caughtFishes.Clear();
    }

    private void UpdateFishingLine()
    {
        if (line == null || !line.enabled || segmentCount < 2) return;
        Vector3 start = Rodtip.position;
        Vector3 end = Lure.transform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector3 point = Vector3.Lerp(start, end, t);
            point.y -= Mathf.Sin(t * Mathf.PI) * curveHeight;
            line.SetPosition(i, point);
        }
    }

    private void RegisterCaughtFishes()
    {
        Fish[] fishes = FindObjectsByType<Fish>(FindObjectsSortMode.None);

        foreach (Fish fish in fishes)
        {
            if (fish == null || !fish.isCaught || caughtFishes.Contains(fish)) continue;
            caughtFishes.Add(fish);
            Debug.Log("魚が食いついた！");
        }
    }

    private void UpdateLaunchedFishes()
    {
        for (int i = launchedFishes.Count - 1; i >= 0; i--)
        {
            Fish fish = launchedFishes[i];

            if (fish == null)
            {
                launchedFishes.RemoveAt(i);
                continue;
            }

            if (fish.isLaunching) continue;

            if (fish.launchFinished)
            {
                if (GManager.instance == null)
                {
                    Debug.LogError("GManagerが見つかりません！");
                    Destroy(fish.gameObject);
                    launchedFishes.RemoveAt(i);
                    continue;
                }

                FishData result = GManager.instance.GetRandomFish(fish.size);

                if (result == null)
                {
                    Destroy(fish.gameObject);
                    launchedFishes.RemoveAt(i);
                    continue;
                }

                Debug.Log("獲得した魚：" + result.fishName);
                Transform target = playerTarget != null ? playerTarget : Rodtip;
                fish.BeginReturn(target, result);
                continue;
            }

            if (fish.reachedPlayer)
            {
                GiveFishReward(fish);
                Destroy(fish.gameObject);
                launchedFishes.RemoveAt(i);
            }
        }
    }

    private void GiveFishReward(Fish fish)
    {
        FishData result = fish.caughtData;
        if (result == null) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMoney(result.money);
            GameManager.Instance.AddFish(result);
        }

        GameObject canvas = GameObject.Find("Canvas");

        if (fishGetUIPrefab != null && canvas != null)
        {
            // FishGetUI ui = Instantiate(fishGetUIPrefab, canvas.transform);
            // ui.Setup(result);
        }
    }

    private void SetPlayerSprite(Sprite sprite)
    {
        if (playerSpriteRenderer == null || sprite == null) return;
        playerSpriteRenderer.sprite = sprite;
    }

    private void SetPlayerSpriteForSeconds(Sprite sprite, float duration, Sprite afterSprite)
    {
        SetPlayerSprite(sprite);
        playerSpriteTimer = duration;
        nextSprite = afterSprite;
    }

    private void UpdatePlayerSpriteTimer()
    {
        if (playerSpriteTimer <= 0f) return;
        playerSpriteTimer -= Time.deltaTime;

        if (playerSpriteTimer <= 0f)
        {
            playerSpriteTimer = 0f;
            if (!isCharging && caughtFishes.Count == 0) SetPlayerSprite(nextSprite);
        }
    }

    private void PlayPullGetSpriteSequence()
    {
        playerSpriteTimer = 0f;
        if (playerSpriteCoroutine != null) StopCoroutine(playerSpriteCoroutine);
        playerSpriteCoroutine = StartCoroutine(PullGetSpriteCoroutine());
    }

    private IEnumerator PullGetSpriteCoroutine()
    {
        SetPlayerSprite(pullSprite);
        yield return new WaitForSeconds(pullSpriteDuration);
        SetPlayerSprite(getSprite);
        yield return new WaitForSeconds(getSpriteDuration);
        if (!isCharging) SetPlayerSprite(idleSprite);
        playerSpriteCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Sea") || lureRigidbody == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayLureSplashSE();
        lureRigidbody.linearDamping = 5f;
        lureRigidbody.angularDamping = 5f;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Sea") || lureRigidbody == null) return;
        lureRigidbody.linearDamping = 0f;
        lureRigidbody.angularDamping = 0f;
    }
}
