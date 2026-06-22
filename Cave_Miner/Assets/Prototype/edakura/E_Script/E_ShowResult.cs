using UnityEngine;
using UnityEngine.UI;

public class E_ShowResult : MonoBehaviour
{
    public Text showScoreText;

    public Text showMiningCountText;

    // Update is called once per frame
    void Update()
    {
        showScoreText.text = $"¡‰ñ‚ÌŠl“¾‹àŠz:{ScoreManager.Instance.dayScore}‰~ \n" +
                             $"Œ»İ‚Ì‡Œv‹àŠz:{ScoreManager.Instance.score}‰~";
        showMiningCountText.text = $"¡‰ñ‚ÌÌŒ@ŒÂ”:{ScoreManager.Instance.dayMiningCount}\n" +
                                   $"Œ»İ‚Ì‡ŒvÌŒ@ŒÂ”:{ScoreManager.Instance.miningCount}";
    }
 }
