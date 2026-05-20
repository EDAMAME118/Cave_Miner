using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField]Transform Player;
    void Update()
    {
        transform.position = new Vector3(Player.position.x,
        Player.position.y, -10f);
        
    }
}
