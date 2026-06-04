using System.Transactions;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSize size;

    public float speed = 2f;

    public float catchRange = 2.5f;

    private Vector3 startPos;
    private Vector2 targetPos;

    private bool movingRight = true;

    public bool isCaught = false;

    public bool isLaunching = false;

    public float detectRange = 3f;

    public float swimRangeX = 3f;
    public float swimRangeY = 3f;

    private BoxCollider2D seaCollider;

    void Start()
    {
        GameObject sea = GameObject.FindWithTag("Sea");

        if (sea != null)
        {
            seaCollider = sea.GetComponent<BoxCollider2D>();
        }

        ChooseNewTarget();

        startPos = transform.position;

        // ‚»‚ê‚¼‚ê‚Ì‹›‚Ì‘å‚«‚³
        if (size == FishSize.Small)
        {
            transform.localScale = Vector3.one * 0.2f;
        }
        else if (size == FishSize.Medium)
        {
            transform.localScale = Vector3.one * 0.4f;
        }
        else if (size == FishSize.Large)
        {
            transform.localScale = Vector3.one * 0.8f;
        }
    }

    void Update()
    {
        GameObject lure = GameObject.FindWithTag("Lure");

        if (isLaunching)
        {
            transform.position += Vector3.up * 10f * Time.deltaTime;

            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

            if (viewPos.y > 1.1f)
            {
                Destroy(gameObject);
            }

            return;
        }

        if (isCaught) return;

        if (lure != null)
        {
            float distance = Vector2.Distance(transform.position, lure.transform.position);

            if (distance < detectRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, lure.transform.position, speed * Time.deltaTime);

                return;
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

    }
}