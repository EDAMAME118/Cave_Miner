using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public static int miningCount;

    private int totalScore;
    private int totalMiningCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        score = 0;
        totalScore = 0;
    }
}
