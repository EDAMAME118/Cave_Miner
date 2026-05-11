using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;


    private int totalScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        score = 10000;
        totalScore = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
