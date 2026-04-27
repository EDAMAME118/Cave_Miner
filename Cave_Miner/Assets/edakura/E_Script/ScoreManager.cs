using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;


    private int totalScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        totalScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(score);
    }
}
