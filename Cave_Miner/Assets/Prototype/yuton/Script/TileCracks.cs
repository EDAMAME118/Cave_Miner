using UnityEngine;

public class TileCracks : MonoBehaviour
{
    public TileRangeDestroyer tilebreak;

    public float crackscheck = 0.0f;//秒数確認変数 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
  


    }

    // Update is called once per frame
    void Update()
    {
        //使いやすくするために秒数を変数に代入
        crackscheck = tilebreak.crackstime;
        //秒数取得できているか確認用
        Debug.Log(crackscheck);

      // if(crackscheck+=3)

    }
}
