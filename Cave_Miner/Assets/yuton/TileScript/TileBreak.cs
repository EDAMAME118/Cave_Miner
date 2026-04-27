using UnityEngine;
using UnityEngine.Tilemaps;
//public class NewMonoBehaviourScript : MonoBehaviour
//{
//    [SerializeField] Tilemap  tilemap;
//    [SerializeField] TileBase tile;
//    [SerializeField] Vector3Int position;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        tilemap.SetTile(position, null);
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
    public class TilemapController : MonoBehaviour
{
        public Tilemap tilemap;
        public Sprite sprite;

        public void replaceTilemap()
        {
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                // ‰½‚©‚Ìˆ—
            }
        }
        //allPositionsWithin‚±‚±‚É”j‰ó”ÍˆÍ‚ğ“ü‚ê‚ê‚Î‚í‚ñ‚¿‚á‚ñ‚¢‚¯‚é‚©‚à
    }
