using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    // アップグレードに必要なスコア変数(強化毎に上昇)
    // 初期値は100でおいておく
    public static int DiggingScore = 100;
    public static int SpeedScore   = 100;
    public static int RangeScore   = 100;

    // 各種強化レベル
    // 初期値はLv1でおいておく
    public static int DiggingLevel = 1;
    public static int SpeedLevel   = 1;
    public static int RangeLevel   = 1;

    // プレイヤーステータス
    public static float playerDigSpeed;
    public static float playerSpeed;

    public static Vector2 miningRange = Vector2.zero;
    public static Vector2 miningRangeOffset = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        playerDigSpeed = 0f;
        playerSpeed = 5f;

        //miningに(1,1)入れておく miningSizeが1増えたらminingOffsetを-0.5しなければならない
        miningRange = new Vector2(1, 1);
        miningRangeOffset = new Vector2(0, 0);
        

    }
}
