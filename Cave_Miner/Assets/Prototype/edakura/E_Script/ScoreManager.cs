using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int dayScore;
    public static int score;
    public static int dayMiningCount;
    public static int miningCount;

    public static int totalScore;
    public static int totalMiningCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        dayScore = 0;
        dayMiningCount = 0;

        score = 0;
        totalScore = 0;

        miningCount = 0;
        totalMiningCount = 0;
    }
}
