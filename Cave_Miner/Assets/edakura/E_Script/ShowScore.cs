using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    [SerializeField] private Text scoreText;


    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"ÉXÉRÉA:{ScoreManager.dayScore}";        
    }
}
