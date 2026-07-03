using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSize size;

    [Header("魚ごとに設定する値")]
    public float speed = 2f;

    [Header("現在の状態")]
    public bool isCaught = false;
    public bool isLaunching = false;
    public bool launchFinished = false;
    public bool isReturning = false;
    public bool reachedPlayer = false;
    public bool isInterested = false;

    public FishData caughtData;

    private Vector2 targetPos;
    private BoxCollider2D seaCollider;
    private GameObject lure;
    private SpriteRenderer spriteRenderer;
    private Transform returnTarget;

    private bool isFacingRight = true;

    // GManagerから受け取る共通設定
    private float detectRange = 3f;
    private float biteDistance = 0.3f;

    // 釣り上げ演出
    private float launchDuration = 0.8f;
    private float launchCurveHeight = 2f;
    private float launchSideOffset = 3f;

    // プレイヤーへ戻る演出
    private float returnDuration = 1.2f;
    private float returnCurveHeight = 0.5f;
    private float returnSideOffset = 4f;

    // 釣り上げ演出で使う現在値
    private Vector3 launchStartPosition;
    private Vector3 launchControlPosition;
    private Vector3 launchEndPosition;
    private float launchElapsedTime;

    // 戻る演出で使う現在値
    private Vector3 returnStartPosition;
    private Vector3 returnControlPosition;
    private float returnElapsedTime;

    private void Start()
    {
        GameObject sea = GameObject.FindWithTag("Sea");

        lure = GameObject.FindWithTag("Lure");
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (lure == null)
        {
            Debug.LogWarning("ルアーが見つかりません！");
        }

        if (sea != null)
        {
            seaCollider = sea.GetComponent<BoxCollider2D>();
        }

        LoadGlobalSettings();
        ApplySizeScale();
        ChooseNewTarget();

        isFacingRight = transform.localScale.x >= 0f;
    }

    private void Update()
    {
        // プレイヤーへ到着した魚は動かさない
        if (reachedPlayer)
        {
            return;
        }

        // 実際の魚画像がプレイヤーへ戻っている
        if (isReturning)
        {
            UpdateReturnMovement();
            return;
        }

        // 魚影が画面外へ飛んでいる
        if (isLaunching)
        {
            UpdateLaunchMovement();
            return;
        }

        // 魚影がルアーへ食いついている
        if (isCaught)
        {
            if (lure != null)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    lure.transform.position,
                    speed * 2f * Time.deltaTime
                );
            }

            return;
        }

        if (lure != null)
        {
            float distance = Vector2.Distance(transform.position, lure.transform.position);

            if (distance < detectRange)
            {
                isInterested = true;

                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.red;
                }

                SetFacing(
                    lure.transform.position.x >
                    transform.position.x
                );

                transform.position = Vector2.MoveTowards(transform.position, lure.transform.position, speed * Time.deltaTime);

                distance = Vector2.Distance(transform.position, lure.transform.position);

                if (distance < biteDistance)
                {
                    isCaught = true;
                    Debug.Log("食いついた！");
                }

                return;
            }

            isInterested = false;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }

        Swim();
    }

    private void LoadGlobalSettings()
    {
        if (GManager.instance == null)
        {
            Debug.LogWarning(
                "GManagerが見つからないため、Fishの初期値を使います"
            );

            return;
        }

        detectRange = GManager.instance.detectRange;

        biteDistance = GManager.instance.biteDistance;

        launchDuration = GManager.instance.fishLaunchDuration;

        launchCurveHeight = GManager.instance.fishLaunchCurveHeight;

        launchSideOffset = GManager.instance.fishLaunchSideOffset;

        returnDuration = GManager.instance.fishReturnDuration;

        returnCurveHeight = GManager.instance.fishReturnCurveHeight;

        returnSideOffset = GManager.instance.fishReturnSideOffset;
    }

    private void ApplySizeScale()
    {
        float scale = size switch
        {
            FishSize.Small => GManager.instance != null ? GManager.instance.smallFishScale : 0.3f,
            FishSize.Medium => GManager.instance != null ? GManager.instance.mediumFishScale : 0.5f,
            FishSize.Large => GManager.instance != null ? GManager.instance.largeFishScale : 0.7f,
            _ => 1f
        };

        transform.localScale = Vector3.one * scale;
    }

    private void Swim()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            ChooseNewTarget();
        }

        SetFacing(targetPos.x > transform.position.x);
    }

    private void SetFacing(bool faceRight)
    {
        if (spriteRenderer == null ||
            isFacingRight == faceRight)
        {
            return;
        }

        Vector3 centerBefore = spriteRenderer.bounds.center;

        Vector3 scale = transform.localScale;

        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        Vector3 centerAfter = spriteRenderer.bounds.center;

        // Pivotを口にしていても、反転時に体がずれないようにする
        transform.position += centerBefore - centerAfter;

        isFacingRight = faceRight;
    }

    private void ChooseNewTarget()
    {
        if (seaCollider == null)
        {
            return;
        }

        Bounds bounds = seaCollider.bounds;

        const float margin = 1f;

        float x = Random.Range(
            bounds.min.x + margin,
            bounds.max.x - margin
        );

        float y = Random.Range(
            bounds.min.y + margin,
            bounds.max.y - margin
        );

        targetPos = new Vector2(x, y);
    }

    // 魚影の釣り上げ演出を開始する
    public void BeginLaunch()
    {
        isCaught = false;
        isReturning = false;
        reachedPlayer = false;

        isLaunching = true;
        launchFinished = false;

        launchElapsedTime = 0f;
        launchStartPosition = transform.position;

        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 viewportPosition =
                mainCamera.WorldToViewportPoint(
                    transform.position
                );

            // 現在位置より右側を終点にする
            float endX = Mathf.Clamp(viewportPosition.x + 0.2f, 0.15f, 0.85f);

            float cameraDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

            // 画面上端より少し外側を終点にする
            launchEndPosition = mainCamera.ViewportToWorldPoint(new Vector3(endX, 1.15f, cameraDistance));

            launchEndPosition.z = transform.position.z;
        }
        else
        {
            launchEndPosition = launchStartPosition + new Vector3(3f, 10f, 0f);
        }

        Vector3 middlePosition = (launchStartPosition + launchEndPosition) / 2f;

        // 必ず右方向へ弧を膨らませる
        float sideOffset = launchSideOffset;

        launchControlPosition = middlePosition + Vector3.right * sideOffset + Vector3.up * launchCurveHeight;

        SetFacing(true);

        transform.rotation = Quaternion.identity;
    }

    // 魚影を弧に沿って画面外へ移動させる
    private void UpdateLaunchMovement()
    {
        launchElapsedTime += Time.deltaTime;

        float duration = Mathf.Max(launchDuration, 0.01f);

        float t = Mathf.Clamp01(launchElapsedTime / duration);

        // 最初と最後を滑らかにする
        float easedT = Mathf.SmoothStep(0f, 1f, t);

        // 二次ベジェ曲線
        Vector3 pointA = Vector3.Lerp(launchStartPosition, launchControlPosition, easedT);

        Vector3 pointB = Vector3.Lerp(launchControlPosition, launchEndPosition, easedT);

        Vector3 nextPosition = Vector3.Lerp(pointA, pointB, easedT);

        Vector3 moveDirection = nextPosition - transform.position;

        transform.position = nextPosition;

        // 魚影の頭を進行方向へ向ける
        if (moveDirection.sqrMagnitude >
            0.0001f)
        {
            float angle =
                Mathf.Atan2(
                    moveDirection.y,
                    moveDirection.x
                ) * Mathf.Rad2Deg;

            transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    angle
                );
        }

        // 画面外の終点へ到着
        if (t >= 1f)
        {
            transform.position =
                launchEndPosition;

            transform.rotation =
                Quaternion.identity;

            isLaunching = false;
            launchFinished = true;
        }
    }

    // 画面外からプレイヤーへ戻る演出を開始する
    public void BeginReturn(
        Transform target,
        FishData data
    )
    {
        if (target == null ||
            data == null)
        {
            return;
        }

        isLaunching = false;
        launchFinished = false;
        isReturning = true;
        isCaught = false;
        reachedPlayer = false;

        returnTarget = target;
        caughtData = data;

        returnElapsedTime = 0f;
        returnStartPosition =
            transform.position;

        Vector3 endPosition =
            returnTarget.position;

        Vector3 middlePosition =
            (returnStartPosition +
             endPosition) / 2f;

        Vector3 returnDirection =
            (endPosition -
             returnStartPosition).normalized;

        Vector3 perpendicularDirection =
            new Vector3(
                -returnDirection.y,
                returnDirection.x,
                0f
            );

        float sideDirection =
            Random.value < 0.5f
                ? -1f
                : 1f;

        returnControlPosition =
            middlePosition +
            perpendicularDirection *
            returnSideOffset *
            sideDirection +
            Vector3.up *
            returnCurveHeight;

        transform.rotation =
            Quaternion.identity;

        // 魚影から実際の魚画像へ変更する
        if (spriteRenderer != null &&
            data.fishSprite != null)
        {
            spriteRenderer.sprite =
                data.fishSprite;

            spriteRenderer.color =
                Color.white;
        }
    }

    private void UpdateReturnMovement()
    {
        if (returnTarget == null)
        {
            return;
        }

        returnElapsedTime +=
            Time.deltaTime;

        float duration =
            Mathf.Max(
                returnDuration,
                0.01f
            );

        float t =
            Mathf.Clamp01(
                returnElapsedTime /
                duration
            );

        // 最初は速く、到着前に減速する
        float easedT =
            1f -
            Mathf.Pow(1f - t, 3f);

        Vector3 endPosition =
            returnTarget.position;

        Vector3 pointA =
            Vector3.Lerp(
                returnStartPosition,
                returnControlPosition,
                easedT
            );

        Vector3 pointB =
            Vector3.Lerp(
                returnControlPosition,
                endPosition,
                easedT
            );

        Vector3 nextPosition =
            Vector3.Lerp(
                pointA,
                pointB,
                easedT
            );

        Vector3 moveDirection =
            nextPosition -
            transform.position;

        transform.position =
            nextPosition;

        if (moveDirection.x > 0.01f)
        {
            SetFacing(true);
        }
        else if (moveDirection.x < -0.01f)
        {
            SetFacing(false);
        }

        float shakeAngle =
            Mathf.Sin(
                t * Mathf.PI * 4f
            ) *
            12f *
            (1f - t);

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                shakeAngle
            );

        if (t >= 1f)
        {
            transform.position =
                endPosition;

            transform.rotation =
                Quaternion.identity;

            isReturning = false;
            reachedPlayer = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color =
            Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectRange
        );
    }
}