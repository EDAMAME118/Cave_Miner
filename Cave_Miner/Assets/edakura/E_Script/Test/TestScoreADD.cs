using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestScoreADD : MonoBehaviour
{
    public Text AddTextTest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AddTextTest.text = $"åªç›ÇÃÉXÉRÉA:{ScoreManager.score}";

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ScoreManager.dayScore += 100;
            ScoreManager.score += 100;
            ScoreManager.totalScore += 100;
        }
    }
}
