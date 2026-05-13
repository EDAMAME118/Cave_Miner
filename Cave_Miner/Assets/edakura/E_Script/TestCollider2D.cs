using UnityEngine;

public class TestCollider2D : MonoBehaviour
{
    BoxCollider2D squareCollider;
    Vector2 testvector;
    Vector2 testOffset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        squareCollider = GetComponent<BoxCollider2D>();

        testvector.x = 5f;
        testvector.y = 5f;

        testOffset.x = 0f;
        testOffset.y = -2f;
    }

    // Update is called once per frame
    void Update()
    {


    }
}
