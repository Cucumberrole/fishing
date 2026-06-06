using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;

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

    public int segmentCount = 20;
    public float curveHeight = 2f;

    private Rigidbody2D LureRigidbody;  //ルアーのRigidbody2Dコンポーネントを格納する変数

    private List<Fish> caughtFishes = new List<Fish>();
    public FishData[] fishList;
    public FishGetUI fishGetUIPrefab;

    private List<Fish> launchedFishes = new List<Fish>();


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
            line.positionCount = segmentCount; // LineRendererの頂点数を設定(竿先とルアー)
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
            Lure.transform.position = Rodtip.position;

            LureRigidbody.simulated = false;

            if (caughtFishes.Count > 0)
            {
                Debug.Log("魚を飛ばします！");

                foreach (Fish fish in caughtFishes)
                {
                    fish.isLaunching = true;
                    launchedFishes.Add(fish);
                }

                caughtFishes.Clear();
            }
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
            for (int i = 0; i < caughtFishes.Count; i++)
            {
                caughtFishes[i].transform.position = Lure.transform.position + Vector3.down * (0.7f * (i + 1));
            }

            if (Vector2.Distance(Lure.transform.position, Rodtip.position) < 0.5f)
            {
                Lure.transform.position = Rodtip.position;

                isReeling = false;
            }
        }
        //line.SetPosition(0, Rodtip.position);//竿先の位置をLineRendererの始点に設定
        //line.SetPosition(1, Lure.transform.position);//ルアーの位置をLineRendererの終点に設定
        Vector3 start = Rodtip.position;
        Vector3 end = Lure.transform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);

            // 直線補間
            Vector3 point = Vector3.Lerp(start, end, t);

            // 放物線ぽく下げる
            float curve = Mathf.Sin(t * Mathf.PI) * curveHeight;

            point.y -= curve;
            line.SetPosition(i, point);
        }

        Fish[] fishes = FindObjectsByType<Fish>(FindObjectsSortMode.None);

        foreach (Fish fish in fishes)
        {
            if (fish.isCaught)
            {
                if (!caughtFishes.Contains(fish))
                {
                    fish.isCaught = true;

                    caughtFishes.Add(fish);

                    Debug.Log("魚が食いついた！");
                }
            }
        }

        for (int i = launchedFishes.Count - 1; i >= 0; i--)
        {
            Fish fish = launchedFishes[i];

            if (fish == null)
            {
                launchedFishes.RemoveAt(i);
                continue;
            }

            Vector3 viewPos = Camera.main.WorldToViewportPoint(fish.transform.position);

            if (viewPos.y > 1.0f)
            {
                Debug.Log("画面外です");

                FishData result = GetRandomFish(fish.size);

                if (result != null)
                {
                    Debug.Log("獲得：" + result.fishName);
                    GameManager.Instance.AddMoney(result.money);
                    GameManager.Instance.caughtFishList.Add(result);

                    FishGetUI ui = Instantiate(fishGetUIPrefab, GameObject.Find("Canvas").transform);
                    ui.Setup(result);
                }

                Destroy(fish.gameObject);

                launchedFishes.RemoveAt(i);
            }
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sea"))
        {
            LureRigidbody.linearDamping = 5f;
            LureRigidbody.angularDamping = 5f;
        }

        //Fish fish = other.GetComponent<Fish>();

        //if (fish != null)
        //{
        //    caughtFish = fish;
        //    fish.isCaught = true;
        //}
    }




    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sea"))
        {
            LureRigidbody.linearDamping = 0f; // 海から出たらルアーの動きを元に戻す
            LureRigidbody.angularDamping = 0f; // 海から出たらルアーの回転も元に戻す
        }
    }




    FishData GetRandomFish(FishSize size)
    {
        List<FishData> candidates = new List<FishData>();

        foreach (FishData fish in fishList)
        {
            if (fish == null)
            {
                Debug.Log("フィッシュデータが空です！！");
                continue;
            }

            if (fish.size == size)
            {
                candidates.Add(fish);
            }
        }

        if (candidates.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, candidates.Count);

        return candidates[randomIndex];
    }
}