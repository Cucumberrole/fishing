using UnityEngine;

public class CloudMove : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f; // 移動速度
    [SerializeField] private float startX = -15.0f; // ループ開始位置
    [SerializeField] private float endX = 15.0f; // ループ終了位置

    void Update()
    {
        // 雲を右へ移動（左へ動かす場合は speed をマイナスに）
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // 設定した位置を超えたら開始位置に戻す
        if (transform.position.x > endX)
        {
            transform.position = new Vector2(startX, transform.position.y);
        }
    }
}

