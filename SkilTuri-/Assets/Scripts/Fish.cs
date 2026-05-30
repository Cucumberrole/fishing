using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed = 2f;

    public float catchRange = 2.5f;

    private Vector3 startPos;

    private bool movingRight = true;

    public bool isCaught = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isCaught) return;

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