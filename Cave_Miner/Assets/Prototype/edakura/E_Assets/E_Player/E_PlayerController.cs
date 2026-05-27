using UnityEngine;
using UnityEngine.InputSystem;

public class E_PlayerController : MonoBehaviour
{
    //public Text BlockText;

    public InputAction MoveAction;
    public InputAction DigAction;

    Vector2 PlayerVector;
    Rigidbody2D rbody;

    [SerializeField]
    float currentSpeed;

    public static Vector2 miningRange;
    public static Vector2 miningRangeOffset;

    private BoxCollider2D miningCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        //miningCollider = transform.Find("MiningRange").transform;

        MoveAction.Enable();
        DigAction.Enable();

        currentSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveActionÇÃílÇì«Ç›çûÇÒÇ≈PlayerVectorÇ…ì¸ÇÍÇÈ
        PlayerVector = MoveAction.ReadValue<Vector2>();

        currentSpeed = PlayerDataManager.playerSpeed;
        
        //å¸Ç´ÇÃí≤êÆ
        if (PlayerVector.x > 0.0f)
        {
            //Debug.Log("âEà⁄ìÆ");
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (PlayerVector.x < 0.0f)
        {
            //Debug.Log("ç∂à⁄ìÆ");
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (PlayerVector.y > 0.0f)
        {
            //Debug.Log("è„à⁄ìÆ");
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        else if (PlayerVector.y < 0.0f)
        {
            //Debug.Log("â∫à⁄ìÆ");
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        //RigidbodyÇ…ë¨ìxì¸ÇÍÇÈ
        rbody.linearVelocity = PlayerVector * currentSpeed;
    }
}
