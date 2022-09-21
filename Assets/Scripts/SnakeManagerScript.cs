using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManagerScript : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] GameObject Segment;
    List<GameObject> snakeBody = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Segment head = Instantiate(Segment, transform.position, transform);
    }

    // Update is called once per frame
    void Update()
    {
        SnakeMovement();
    }

    void SnakeMovement()
    {
        GameObject head = snakeBody[0];
        Vector2 pos = head.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos += MoveSpeed * Time.deltaTime * (Vector2)head.transform.right;
        head.transform.right = mousePos - pos;
        if (Vector2.Distance(pos, mousePos) >= 0.05f) head.transform.position = pos;
    }
}
