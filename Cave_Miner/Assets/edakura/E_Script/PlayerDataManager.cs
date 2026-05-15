using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    //アップグレードに必要なスコア変数(強化毎に上昇)
    //初期値は100でおいておく
    public static int DiggingScore = 100;
    public static int SpeedScore   = 100;
    public static int RangeScore   = 100;
    //各種強化レベル
    //初期値はLv1でおいておく
    public static int DiggingLevel = 1;
    public static int SpeedLevel   = 1;
    public static int RangeLevel   = 1;

    //プレイヤーステータス
    public static float playerDigSpeed;
    public static float playerSpeed;

    public static float squareSizex = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        playerDigSpeed = 0f;
        playerSpeed = 5f;
    }
}
