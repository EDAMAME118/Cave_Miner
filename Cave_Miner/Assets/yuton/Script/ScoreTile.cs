using UnityEngine;
using UnityEngine.Tilemaps;

// インスペクターからアセットを作成できるようにする
[CreateAssetMenu(fileName = "NewScoreTile", menuName = "2D/Tiles/ScoreTile")]
public class ScoreTile : Tile
{
    public int scoreValue; // このタイルのスコア
    public int mining_soeed;//採掘速度
    public string typeName; // 種別の名前（"Grass", "Water" など）
}