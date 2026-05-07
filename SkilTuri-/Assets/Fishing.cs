using UnityEngine;

public class Fishing : MonoBehaviour
{
    public GameObject Lure;
    public Transform Rodtip;
    public LineRenderer line;

    public Vector2 throwDirection = new Vector2(1f, 1f);
    //どの角度で投げるか
    public float power = 10f;
    public float reelSpeed = 5f; // 巻き取りの速度
    private bool isReeling = false; // 巻き取り中かどうかを示すフラグ

    private Rigidbody2D LureRigidbody;  //ルアーのRigidbody2Dコンポーネントを格納する変数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Lure != null)//Lureがnullでない場合、Rigidbody2Dコンポーネントを取得
        {
            LureRigidbody = Lure.GetComponent<Rigidbody2D>();
            LureRigidbody.simulated = false;//初めは物理挙動しない
        }
        if (line != null)
        {
            line.positionCount = 2; // LineRendererの頂点数を設定(竿先とルアー)
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Lure == null || Rodtip == null || LureRigidbody == null) return;
        // Lure、Rodtip、LureRigidbodyのいずれかがnullの場合は処理を中断
        //↑これできる男がやるやつ

        if (Input.GetMouseButtonDown(0) && isReeling == false)
        {
            Lure.transform.position = Rodtip.position; // ルアーを竿先の位置に移動
            LureRigidbody.linearVelocity = Vector2.zero; // 一応、ルアーの速度をリセット。
                                                         // LinearVelocityはRigidbody2Dの速度を表すプロパティで、Vector2.zeroは(0, 0)のベクトルを意味する。これにより、ルアーが投げられる前に静止状態になる。
            LureRigidbody.simulated = true; // ルアーの物理挙動をONにする
            LureRigidbody.AddForce(throwDirection.normalized * power, ForceMode2D.Impulse);// ルアーに力を加える。normalizedで方向ベクトルを正規化して、powerで力の大きさを調整。Impulseは瞬間的な力を加えるモード
            //巻き取りはaddforceじゃできない

        }
        if (Input.GetMouseButtonDown(1))
        {
            isReeling = true; // 巻き取り開始
            LureRigidbody.simulated = false;// 巻き取り中は物理挙動をOFFにする
        }
        if (isReeling) 
        { 
            Lure.transform.position = Vector2.MoveTowards(Lure.transform.position, Rodtip.position, reelSpeed * Time.deltaTime);
            //Vector2.MoveTowards(今の位置, 目標位置, 移動距離)らしい
            // ルアーを竿先に向かって一定速度で移動

            //巻くとき連打用
            //  LureRigidbody.linearVelocity = (Rodtip.position - Lure.transform.position).normalized * reelSpeed;//normalizedは方向ベクトルを教えてくれるやつ
            //  // ルアーを竿先に向かって一定速度で移動
            //  if (Vector2.Distance(Lure.transform.position, Rodtip.position) < 0.5f)
            //{
            //      Lure.transform.position = Rodtip.position; // ルアーが竿先に近づいたら位置を完全に合わせる
            //}

            if (Vector2.Distance(Lure.transform.position, Rodtip.position) < 0.5f)
            {
                Lure.transform.position = Rodtip.position; // ルアーが竿先に近づいたら位置を完全に合わせる
                isReeling = false; // 巻き取り終了
                LureRigidbody.simulated = false; // ルアーの物理挙動をOFFにする
            }
        }
        line.SetPosition(0, Rodtip.position);//竿先の位置をLineRendererの始点に設定
        line.SetPosition(1, Lure.transform.position);//ルアーの位置をLineRendererの終点に設定
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sea"))
        {
            LureRigidbody.linearDamping = 5f; // 海に入ったらルアーの動きを減速させる
            LureRigidbody.angularDamping = 5f; // 海に入ったらルアーの回転も減速させる
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sea"))
        {
            LureRigidbody.linearDamping = 0f; // 海から出たらルアーの動きを元に戻す
            LureRigidbody.angularDamping = 0f; // 海から出たらルアーの回転も元に戻す
        }
    }
}
