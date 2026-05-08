using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningArea : MonoBehaviour
{
    public Tilemap tilemap;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (Input.GetMouseButton(0)) //クリック中に採掘
        {
            //接触している位置を取得
            Vector3 hitPos = col.ClosestPoint(transform.position);

            //タイル座標に変換
            Vector3Int cellPos = tilemap.WorldToCell(hitPos);

            //タイルがあれば削除
            if (tilemap.HasTile(cellPos))
            {
                tilemap.SetTile(cellPos, null);
            }
        }
    }
}
//HasTile　指定した座標にタイルが存在するか確認する関数
//