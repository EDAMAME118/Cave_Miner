using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // どこからでもアクセスできる静的なインスタンス
    public static ScoreManager Instance { get; private set; }

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

    //一日のスコアと採掘数
    public int dayScore;
    public int dayMiningCount;

    //現在のスコアと採掘数
    public int score;
    public int miningCount;

    //総合のスコアと採掘数
    public int totalScore;
    public int totalMiningCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //各種スコア初期化
        dayScore = 0;
        dayMiningCount = 0;

        score = 0;
        totalScore = 0;

        miningCount = 0;
        totalMiningCount = 0;
    }
}
