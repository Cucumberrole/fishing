using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishData data;

    private SpriteRenderer spriteRenderer;

    private Vector3 startPos;

    private bool movingRight = true;

    public bool isCaught = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = data.fishSprite;

        startPos = transform.position;
    }

    void Update()
    {
        Swim();
    }

    void Swim()
    {
        float speed = data.speed;

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
}