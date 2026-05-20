using UnityEngine;
using UnityEngine.UI;

public class E_ShowResult : MonoBehaviour
{
    public Text showScoreText;

    public Text showMiningCountText;

    // Update is called once per frame
    void Update()
    {
        showScoreText.text = $"今回の獲得スコア:{ScoreManager.dayScore}\n" +
                             $"現在の合計スコア:{ScoreManager.score}";
        showMiningCountText.text = $"今回の採掘個数:{ScoreManager.dayMiningCount}\n" +
                                   $"現在の合計採掘個数:{ScoreManager.miningCount}";
    }
}
