//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class MiningArea : MonoBehaviour
//{
//    public Tilemap tilemap;
//    //ブロックに判定が触れている間
//    private void OnTriggerStay2D(Collider2D col)
//    {
//        //Ｚキーを押したとき採掘
//        if (Input.GetKey(KeyCode.Z))
//        {
//            //接触している位置を取得
//            Vector3 hitPos = col.ClosestPoint(transform.position);

//            //タイル座標に変換
//            Vector3Int cellPos = tilemap.WorldToCell(hitPos);

//            //タイルがあれば削除
//            if (tilemap.HasTile(cellPos))
//            {
//                tilemap.SetTile(cellPos, null);
//            }
//        }
//    }
//}
////1
////HasTile　指定した座標にタイルが存在するか確認する関数

////2
////colは触れている相手が自動で渡される
////↓入っている情報
////col.gameObject → 相手のオブジェクト
////col.transform → 相手の位置
////col.tag → タグ（"Player" など）
////col.GetComponent<>() → 相手のスクリプト取得

////col.ClosestPoint(調べたい位置)でコライダー上で一番近い点を指す

using UnityEngine;

///using UnityEngine;

public class MiningArea : MonoBehaviour
{
    private Collider2D currentBlock;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Block"))
        {
            currentBlock = col;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col == currentBlock)
        {
            currentBlock = null;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (currentBlock != null)
            {
                Destroy(currentBlock.gameObject);
            }
        }
    }
}