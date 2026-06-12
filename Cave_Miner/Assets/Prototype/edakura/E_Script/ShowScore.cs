using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    //スコアUI用テキスト
    [SerializeField] private Text scoreText;

    // Update is called once per frame
    void Update()
    {
        //スコアマネージャーの値を表示する
        scoreText.text = $"金額:{ScoreManager.Instance.dayScore}円";
    }
}
