using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningArea : MonoBehaviour
{
    private Collider2D currentBlock;
    //範囲に入ったブロックの位置情報取得
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Block"))
        {
            currentBlock = col;
        }
    }
    //範囲から離れたら破壊時間リセット
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col == currentBlock)
        {
            currentBlock = null;
        }
    }

    private void Update()
    {
        //Zを押しているかの判定
        if (Input.GetKey(KeyCode.Z))
        {
            //Zを押しているときに採掘範囲に入ると一定時間で壊せる
            if (currentBlock != null)
            {
                Destroy(currentBlock.gameObject);
            }
        }
    }
}
//HasTile　指定した座標にタイルが存在するか確認する関数

//2
//colは触れている相手が自動で渡される
//↓入っている情報
//col.gameObject → 相手のオブジェクト
//col.transform → 相手の位置
//col.tag → タグ（"Player" など）
//col.GetComponent<>() → 相手のスクリプト取得

//col.ClosestPoint(調べたい位置)でコライダー上で一番近い点を指す
