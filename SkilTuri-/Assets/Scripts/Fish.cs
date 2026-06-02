using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSize size;

    public float speed = 2f;

    public float catchRange = 2.5f;

    private Vector3 startPos;

    private bool movingRight = true;

    public bool isCaught = false;

    public bool isLaunching = false;

    public float detectRange = 3f;

    void Start()
    {
        startPos = transform.position;
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
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (transform.position.x > startPos.x + 3f)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (transform.position.x < startPos.x - 3f)
            {
                movingRight = true;
                Flip();
            }
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
}