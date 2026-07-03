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

    private float currentChargeTime;
    private bool isCharging;
    private bool isReeling;
    private Rigidbody2D lureRigidbody;

    private readonly List<Fish> caughtFishes = new();
    private readonly List<Fish> launchedFishes = new();

    private float MinPower =>
        GManager.instance != null
            ? GManager.instance.minThrowPower
            : 5f;

    private float MaxPower =>
        GManager.instance != null
            ? GManager.instance.maxThrowPower
            : 20f;

    private float MaxChargeTime =>
        GManager.instance != null
            ? GManager.instance.maxChargeTime
            : 2f;

    private float ReelSpeed =>
        GManager.instance != null
            ? GManager.instance.reelSpeed
            : 5f;

    private void Start()
    {
        if (Lure != null)
        {
            lureRigidbody =
                Lure.GetComponent<Rigidbody2D>();

            if (lureRigidbody != null)
            {
                lureRigidbody.simulated = false;
            }
        }

        if (line != null)
        {
            line.positionCount = segmentCount;
        }
    }

    private void Update()
    {
        if (Lure == null ||
            Rodtip == null ||
            lureRigidbody == null)
        {
            return;
        }

        UpdateThrowInput();
        UpdateReeling();
        UpdateFishingLine();
        RegisterCaughtFishes();
        UpdateLaunchedFishes();
    }

    private void UpdateThrowInput()
    {
        // 左クリックを押した瞬間
        if (Input.GetMouseButtonDown(0) &&
            !isReeling)
        {
            isCharging = true;
            currentChargeTime = 0f;

            Lure.transform.position =
                Rodtip.position;

            lureRigidbody.linearVelocity =
                Vector2.zero;

            lureRigidbody.angularVelocity =
                0f;

            lureRigidbody.simulated =
                false;
        }

        // 左クリックを押している間
        if (Input.GetMouseButton(0) &&
            isCharging)
        {
            currentChargeTime =
                Mathf.Min(
                    currentChargeTime +
                    Time.deltaTime,
                    MaxChargeTime
                );

            Lure.transform.position =
                Rodtip.position;
        }

        // 左クリックを離した瞬間
        if (Input.GetMouseButtonUp(0) &&
            isCharging)
        {
            isCharging = false;

            float chargeRate =
                Mathf.Clamp01(
                    currentChargeTime /
                    Mathf.Max(
                        MaxChargeTime,
                        0.01f
                    )
                );

            float throwPower =
                Mathf.Lerp(
                    MinPower,
                    MaxPower,
                    chargeRate
                );

            lureRigidbody.simulated =
                true;

            lureRigidbody.AddForce(
                throwDirection.normalized *
                throwPower,
                ForceMode2D.Impulse
            );

            currentChargeTime = 0f;
        }

        // 右クリックで釣り上げる
        if (Input.GetMouseButtonDown(1))
        {
            isCharging = false;
            currentChargeTime = 0f;

            Lure.transform.position =
                Rodtip.position;

            lureRigidbody.linearVelocity =
                Vector2.zero;

            lureRigidbody.angularVelocity =
                0f;

            lureRigidbody.simulated =
                false;

            foreach (Fish fish in caughtFishes)
            {
                if (fish == null)
                {
                    continue;
                }

                // 魚影の弧を描く釣り上げを開始
                fish.BeginLaunch();

                // 同じ魚を重複登録しない
                if (!launchedFishes.Contains(fish))
                {
                    launchedFishes.Add(fish);
                }
            }

            caughtFishes.Clear();
        }
    }

    private void UpdateReeling()
    {
        if (!isReeling)
        {
            return;
        }

        Lure.transform.position =
            Vector2.MoveTowards(
                Lure.transform.position,
                Rodtip.position,
                ReelSpeed *
                Time.deltaTime
            );

        for (int i = 0;
             i < caughtFishes.Count;
             i++)
        {
            if (caughtFishes[i] != null)
            {
                caughtFishes[i].transform.position =
                    Lure.transform.position +
                    Vector3.down *
                    (0.7f * (i + 1));
            }
        }

        if (Vector2.Distance(
                Lure.transform.position,
                Rodtip.position
            ) < 0.5f)
        {
            Lure.transform.position =
                Rodtip.position;

            isReeling = false;
        }
    }

    private void UpdateFishingLine()
    {
        if (line == null ||
            segmentCount < 2)
        {
            return;
        }

        Vector3 start =
            Rodtip.position;

        Vector3 end =
            Lure.transform.position;

        for (int i = 0;
             i < segmentCount;
             i++)
        {
            float t =
                i /
                (float)(segmentCount - 1);

            Vector3 point =
                Vector3.Lerp(
                    start,
                    end,
                    t
                );

            point.y -=
                Mathf.Sin(
                    t * Mathf.PI
                ) * curveHeight;

            line.SetPosition(
                i,
                point
            );
        }
    }

    private void RegisterCaughtFishes()
    {
        Fish[] fishes =
            FindObjectsByType<Fish>(
                FindObjectsSortMode.None
            );

        foreach (Fish fish in fishes)
        {
            if (fish != null &&
                fish.isCaught &&
                !caughtFishes.Contains(fish))
            {
                caughtFishes.Add(fish);

                Debug.Log(
                    "魚が食いついた！"
                );
            }
        }
    }

    private void UpdateLaunchedFishes()
    {
        for (int i =
                 launchedFishes.Count - 1;
             i >= 0;
             i--)
        {
            Fish fish =
                launchedFishes[i];

            if (fish == null)
            {
                launchedFishes.RemoveAt(i);
                continue;
            }

            // 魚影が釣り上げ用の弧を移動している間
            if (fish.isLaunching)
            {
                continue;
            }

            // 魚影が画面外へ到着した
            if (fish.launchFinished)
            {
                if (GManager.instance == null)
                {
                    Debug.LogError(
                        "GManagerが見つかりません！"
                    );

                    Destroy(
                        fish.gameObject
                    );

                    launchedFishes.RemoveAt(i);
                    continue;
                }

                // 魚影のサイズに合った魚を抽選
                FishData result =
                    GManager.instance.GetRandomFish(
                        fish.size
                    );

                if (result == null)
                {
                    Destroy(
                        fish.gameObject
                    );

                    launchedFishes.RemoveAt(i);
                    continue;
                }

                Debug.Log(
                    "獲得した魚：" +
                    result.fishName
                );

                Transform target =
                    playerTarget != null
                        ? playerTarget
                        : Rodtip;

                // 魚画像へ変化させて、
                // プレイヤーへの帰還を開始
                fish.BeginReturn(
                    target,
                    result
                );

                continue;
            }

            // プレイヤーへ到着した
            if (fish.reachedPlayer)
            {
                GiveFishReward(fish);

                Destroy(
                    fish.gameObject
                );

                launchedFishes.RemoveAt(i);
            }
        }
    }

    private void GiveFishReward(Fish fish)
    {
        FishData result =
            fish.caughtData;

        if (result == null)
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMoney(
                result.money
            );

            GameManager.Instance.AddFish(
                result
            );
        }

        GameObject canvas =
            GameObject.Find("Canvas");

        if (fishGetUIPrefab != null &&
            canvas != null)
        {
            // FishGetUIを表示したい場合は、
            // 次の2行のコメントを外す

            // FishGetUI ui = Instantiate(
            //     fishGetUIPrefab,
            //     canvas.transform
            // );

            // ui.Setup(result);
        }
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (other.CompareTag("Sea"))
        {
            lureRigidbody.linearDamping =
                5f;

            lureRigidbody.angularDamping =
                5f;
        }
    }

    private void OnTriggerExit2D(
        Collider2D other
    )
    {
        if (other.CompareTag("Sea"))
        {
            lureRigidbody.linearDamping =
                0f;

            lureRigidbody.angularDamping =
                0f;
        }
    }
}