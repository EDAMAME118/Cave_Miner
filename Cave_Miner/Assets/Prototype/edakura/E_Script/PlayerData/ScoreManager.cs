using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //一日のスコアと採掘数
    public static int dayScore;
    public static int dayMiningCount;

    //現在のスコアと採掘数
    public static int score;
    public static int miningCount;

    //総合のスコアと採掘数
    public static int totalScore;
    public static int totalMiningCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //各種スコア初期化
        dayScore = 0;
        dayMiningCount = 0;

        score = 0;
        totalScore = 0;

        miningCount = 0;
        totalMiningCount = 0;
    }
}
