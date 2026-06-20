using System.Transactions;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSize size;

    public float speed = 2f;
    public float catchRange = 2.5f;

    private Vector3 startPos;
    private Vector2 targetPos;

    // private bool movingRight = true;

    public bool isCaught = false;
    public bool isLaunching = false;

    public bool isReturning = false;
    public bool reachedPlayer = false;

    public float returnSpeed = 8f;

    public FishData caughtData;

    private Transform returnTarget;

    public float detectRange = 3f;

    public float swimRangeX = 3f;
    public float swimRangeY = 3f;

    private BoxCollider2D seaCollider;
    private GameObject lure;

    public bool isInterested = false;

    private SpriteRenderer sr; // デバッグ用




    void Start()
    {
        GameObject sea = GameObject.FindWithTag("Sea");

        lure = GameObject.FindWithTag("Lure");
        if (lure == null)
        {
            Debug.Log("ルアーが見つかりません！");
        }
        else
        {
            Debug.Log("ルアー発見！");
        }

        sr = GetComponent<SpriteRenderer>(); // デバッグ用

        if (sea != null)
        {
            seaCollider = sea.GetComponent<BoxCollider2D>();
        }

        startPos = transform.position;

        ChooseNewTarget();


        // それぞれの魚の大きさ
        if (size == FishSize.Small)
        {
            transform.localScale = Vector3.one * 0.3f;
        }
        else if (size == FishSize.Medium)
        {
            transform.localScale = Vector3.one * 0.5f;
        }
        else if (size == FishSize.Large)
        {
            transform.localScale = Vector3.one * 0.7f;
        }
    }




    void Update()
    {
        if (isReturning)
        {
            if (returnTarget == null)
            {
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, returnTarget.position, returnSpeed * Time.deltaTime);

            // プレイヤーの方向を向かせる
            Vector3 scale = transform.localScale;

            if (returnTarget.position.x > transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x);
            }
            else
            {
                scale.x = -Mathf.Abs(scale.x);
            }

            transform.localScale = scale;

            // プレイヤーへ到着したか
            if (Vector2.Distance(
                transform.position,
                returnTarget.position
            ) < 0.3f)
            {
                reachedPlayer = true;
            }

            return;
        }

        if (isLaunching)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;

            transform.rotation = Quaternion.Euler(0, 0, 90);

            transform.position += Vector3.up * 10f * Time.deltaTime;

            return;
        }

        if (isCaught)
        {
            if (lure != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, lure.transform.position, speed * 2f * Time.deltaTime);
            }
            return;
        }

        if (lure != null)
        {
            float distance = Vector2.Distance(transform.position, lure.transform.position);

            // Debug.Log(distance);

            if (distance < detectRange)
            {
                Debug.Log(gameObject.name + " がルアーを発見");
                sr.color = Color.red;

                isInterested = true;

                transform.position = Vector2.MoveTowards(transform.position, lure.transform.position, speed * Time.deltaTime);

                if (lure.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }

                if (distance < 0.3f)
                {
                    isCaught = true;
                    Debug.Log("食いついた！");
                }

                return;
            }
            else
            {
                sr.color = Color.white;
                isInterested = false;
            }
        }

        Swim();
    }




    void Swim()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            ChooseNewTarget();
        }

        if (targetPos.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }




    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }




    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }




    void ChooseNewTarget()
    {
        if (seaCollider == null) return;

        Bounds bounds = seaCollider.bounds;

        float margin = 1f;

        float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);

        float y = Random.Range(bounds.min.y + margin, bounds.max.y - margin);

        targetPos = new Vector2(x, y);
    }



    public void BeginReturn(Transform target, FishData data)
    {
        if (target == null || data == null)
        {
            return;
        }

        isLaunching = false;
        isReturning = true;
        reachedPlayer = false;

        returnTarget = target;
        caughtData = data;

        // 飛び上がるときの回転を元に戻す
        transform.rotation = Quaternion.identity;

        // 魚影から、実際に獲得した魚の画像へ変更
        if (sr != null && data.fishSprite != null)
        {
            sr.sprite = data.fishSprite;
            sr.color = Color.white;
        }
    }
}