using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    private void Awake()
    {
        // まだインスタンスがない場合は、自分を登録
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが切り替わっても破棄しない
        }
        else
        {
            // すでに存在する場合は、重複している自分を破棄
            Destroy(gameObject);
        }
    }

    // アップグレードに必要なスコア変数(強化毎に上昇)
    // 初期値は100でおいておく
    public int DiggingScore = 500;
    public int SpeedScore   = 300;
    public int RangeScore   = 10000;

    // 各種強化レベル
    // 初期値はLv1でおいておく
    public int DiggingLevel = 1;
    public int SpeedLevel   = 1;
    public int RangeLevel   = 1;

    // プレイヤーステータス
    public float playerDigSpeed;
    public float playerSpeed;

    //現在選択中のステージ
    public Stage currentStage = Stage.STAGE1;

    public Vector2 miningRange = Vector2.zero;
    public Vector2 miningRangeOffset = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDigSpeed = 1.0f;
        playerSpeed = 5f;

        //miningに(1,1)入れておく miningSizeが1増えたらminingOffsetを-0.5しなければならない
        miningRange = new Vector2(0.7f, 0.7f);
        miningRangeOffset = new Vector2(0, -1f);
        

    }
}
