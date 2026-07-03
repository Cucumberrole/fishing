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
    private Transform arcTarget;
    private bool isFacingRight = true;

    private float detectRange = 3f;
    private float biteDistance = 0.3f;
    private float launchDuration = 0.8f;
    private float launchCurveHeight = 2f;
    private float returnDuration = 1.2f;

    private Vector3 launchStartPosition;
    private Vector3 launchControlPosition;
    private Vector3 arcApexPosition;
    private float launchElapsedTime;

    private Vector3 returnStartPosition;
    private Vector3 returnControlPosition;
    private float returnElapsedTime;

    private void Start()
    {
        GameObject sea = GameObject.FindWithTag("Sea");
        lure = GameObject.FindWithTag("Lure");
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (lure == null) Debug.LogWarning("ルアーが見つかりません！");
        if (sea != null) seaCollider = sea.GetComponent<BoxCollider2D>();

        LoadGlobalSettings();
        ApplySizeScale();
        ChooseNewTarget();

        isFacingRight = transform.localScale.x >= 0f;
    }

    private void Update()
    {
        if (reachedPlayer) return;

        if (isReturning)
        {
            UpdateReturnMovement();
            return;
        }

        if (isLaunching)
        {
            UpdateLaunchMovement();
            return;
        }

        if (isCaught)
        {
            if (lure != null) transform.position = Vector2.MoveTowards(transform.position, lure.transform.position, speed * 2f * Time.deltaTime);
            return;
        }

        if (lure != null)
        {
            float distance = Vector2.Distance(transform.position, lure.transform.position);

            if (distance < detectRange)
            {
                isInterested = true;
                if (spriteRenderer != null) spriteRenderer.color = Color.red;

                SetFacing(lure.transform.position.x > transform.position.x);
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
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
        }

        Swim();
    }

    private void LoadGlobalSettings()
    {
        if (GManager.instance == null)
        {
            Debug.LogWarning("GManagerが見つからないため、Fishの初期値を使います");
            return;
        }

        detectRange = GManager.instance.detectRange;
        biteDistance = GManager.instance.biteDistance;
        launchDuration = GManager.instance.fishLaunchDuration;
        launchCurveHeight = GManager.instance.fishLaunchCurveHeight;
        returnDuration = GManager.instance.fishReturnDuration;
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

        if (Vector2.Distance(transform.position, targetPos) < 0.2f) ChooseNewTarget();

        SetFacing(targetPos.x > transform.position.x);
    }

    private void SetFacing(bool faceRight)
    {
        if (spriteRenderer == null || isFacingRight == faceRight) return;

        Vector3 centerBefore = spriteRenderer.bounds.center;
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        Vector3 centerAfter = spriteRenderer.bounds.center;
        transform.position += centerBefore - centerAfter;
        isFacingRight = faceRight;
    }

    private void ChooseNewTarget()
    {
        if (seaCollider == null) return;

        Bounds bounds = seaCollider.bounds;
        const float margin = 1f;

        float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float y = Random.Range(bounds.min.y + margin, bounds.max.y - margin);

        targetPos = new Vector2(x, y);
    }

    public void BeginLaunch(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("弧の終点となるプレイヤーが設定されていません！");
            return;
        }

        isCaught = false;
        isReturning = false;
        reachedPlayer = false;
        isLaunching = true;
        launchFinished = false;

        arcTarget = target;
        returnTarget = target;
        launchElapsedTime = 0f;
        launchStartPosition = transform.position;

        Vector3 endPosition = arcTarget.position;
        float apexY = Mathf.Max(launchStartPosition.y, endPosition.y) + launchCurveHeight;
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            float cameraDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
            Vector3 cameraTopPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, cameraDistance));
            apexY = Mathf.Max(apexY, cameraTopPosition.y + launchCurveHeight);
        }

        arcApexPosition = new Vector3((launchStartPosition.x + endPosition.x) / 2f, apexY, transform.position.z);
        launchControlPosition = new Vector3((launchStartPosition.x + arcApexPosition.x) / 2f, arcApexPosition.y, transform.position.z);

        SetFacing(arcApexPosition.x >= launchStartPosition.x);
        transform.rotation = Quaternion.identity;
    }

    private void UpdateLaunchMovement()
    {
        launchElapsedTime += Time.deltaTime;

        float duration = Mathf.Max(launchDuration, 0.01f);
        float t = Mathf.Clamp01(launchElapsedTime / duration);

        Vector3 pointA = Vector3.Lerp(launchStartPosition, launchControlPosition, t);
        Vector3 pointB = Vector3.Lerp(launchControlPosition, arcApexPosition, t);
        Vector3 nextPosition = Vector3.Lerp(pointA, pointB, t);
        Vector3 moveDirection = nextPosition - transform.position;

        transform.position = nextPosition;

        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            SetFacing(moveDirection.x >= 0f);

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            if (!isFacingRight) angle -= 180f;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (t >= 1f)
        {
            transform.position = arcApexPosition;
            transform.rotation = Quaternion.identity;
            isLaunching = false;
            launchFinished = true;
        }
    }

    public void BeginReturn(Transform target, FishData data)
    {
        if (target == null || data == null) return;

        isLaunching = false;
        launchFinished = false;
        isReturning = true;
        isCaught = false;
        reachedPlayer = false;

        returnTarget = target;
        caughtData = data;
        returnElapsedTime = 0f;
        returnStartPosition = transform.position;

        Vector3 endPosition = returnTarget.position;
        returnControlPosition = new Vector3((returnStartPosition.x + endPosition.x) / 2f, returnStartPosition.y, transform.position.z);

        transform.rotation = Quaternion.identity;

        if (spriteRenderer != null && data.fishSprite != null)
        {
            spriteRenderer.sprite = data.fishSprite;
            spriteRenderer.color = Color.white;
        }
    }

    private void UpdateReturnMovement()
    {
        if (returnTarget == null) return;

        returnElapsedTime += Time.deltaTime;

        float duration = Mathf.Max(returnDuration, 0.01f);
        float t = Mathf.Clamp01(returnElapsedTime / duration);
        float easedT = t;

        Vector3 endPosition = returnTarget.position;
        Vector3 pointA = Vector3.Lerp(returnStartPosition, returnControlPosition, easedT);
        Vector3 pointB = Vector3.Lerp(returnControlPosition, endPosition, easedT);
        Vector3 nextPosition = Vector3.Lerp(pointA, pointB, easedT);
        Vector3 moveDirection = nextPosition - transform.position;

        transform.position = nextPosition;

        if (moveDirection.x > 0.01f) SetFacing(true);
        else if (moveDirection.x < -0.01f) SetFacing(false);

        float shakeAngle = Mathf.Sin(t * Mathf.PI * 4f) * 12f * (1f - t);
        transform.rotation = Quaternion.Euler(0f, 0f, shakeAngle);

        if (t >= 1f)
        {
            transform.position = endPosition;
            transform.rotation = Quaternion.identity;
            isReturning = false;
            reachedPlayer = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}