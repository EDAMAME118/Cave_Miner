using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class E_TimeLimit : MonoBehaviour
{
    //時間制限
    public int TimeLimit;
    private int secondsTime;
    private int minutesTime;

    //1秒計測
    private float timeInterval;

    public Text timeText;

    //制限時間が０になったらアップグレードシーンへ移動する用
    public string nextSceneBuy;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeInterval = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //分と秒に変換
        minutesTime = TimeLimit / 60;
        secondsTime = TimeLimit % 60;

        //現在の制限時間表示
        timeText.text = $"残り時間 {minutesTime}:{secondsTime:D2}";

        timeInterval += Time.deltaTime;

        //1秒経過時
        if(timeInterval >= 1.0f)
        {
            //制限時間１減らす
            TimeLimit--;
            //１秒計測変数をもとに戻す
            timeInterval -= 1.0f;
        }

        if(TimeLimit < 0)
        {
            SceneManager.LoadScene(nextSceneBuy);
        }


    }
}
