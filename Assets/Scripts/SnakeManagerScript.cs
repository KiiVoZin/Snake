using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class SnakeManagerScript : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float OffSet;
    [SerializeField] GameObject Segment;
    List<GameObject> snakeBody = new List<GameObject>();
    List<Vector2> MarkerList;
    bool boolFPSIncrease;
    // Start is called before the first frame update
    void Start()
    {
        MarkerList = new List<Vector2>();
        GameObject head = Instantiate(Segment, new Vector3(0, 0, 0), transform.rotation);
        snakeBody.Add(head);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (boolFPSIncrease)
            {
                Application.targetFrameRate = 10;
            }
            else
            {
                Application.targetFrameRate = 120;
            }
            boolFPSIncrease = !boolFPSIncrease;
        }

        AddSegment();
        SnakeMovement();

        for(int i = 0; i < MarkerList.Count-1; i++)
        {
            Debug.DrawLine(MarkerList[i], MarkerList[i + 1]);
        }
    }

    void SnakeMovement()
    {
        //Movement for the head
        GameObject head = snakeBody[0];
        Vector2 pos = head.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        head.transform.right = (mousePos - pos).normalized;
        Vector2 move = Vector2.ClampMagnitude(MoveSpeed * Time.deltaTime * (Vector2)head.transform.right, Vector2.Distance(head.transform.position, mousePos));
        move += pos;
        //pos = Vector2.ClampMagnitude(pos, Vector2.Distance(head.transform.position, mousePos));
        if (Vector2.Distance(pos, mousePos) <= 0.05f) return;
        head.transform.position = move;
        MarkerList.Insert(0, pos);

        //Movement for the body
        if (snakeBody.Count < 2) return;
        int temp = 1;
        for (int i = 1; i < snakeBody.Count; i++)
        {
            int marker = 0;
            float distance = 0;
            //if (temp < MarkerList.Count -2) distance = Vector2.Distance(snakeBody[i-1].transform.position, MarkerList[temp + 2]);
            for (int j = temp; j < MarkerList.Count; j++)
            {
                distance += Vector2.Distance(MarkerList[j - 1], MarkerList[j]);
                if (distance >= OffSet)
                {
                    marker = j;
                    temp = j;
                    break;
                }
            }
            Debug.Log(distance);
            //Vector2.Distance(snakeBody[i - 1].transform.position, MarkerList[j]) >= OffSet
            if (marker == 0) break;
            Vector2 test = MarkerList[marker] - ((MarkerList[marker] - MarkerList[marker - 1]).normalized * (distance - OffSet));
            //test = (MarkerList[marker - 1] - test).normalized * (distance-OffSet);
            snakeBody[i].transform.position = test;
            //snakeBody[i].transform.position = (Vector2)snakeBody[i - 1].transform.position - (((Vector2)snakeBody[i - 1].transform.position - MarkerList[marker]));
            snakeBody[i].transform.right = (snakeBody[i - 1].transform.position - snakeBody[i].transform.position);
            if (i == snakeBody.Count - 1) MarkerList.RemoveRange(marker + 1, MarkerList.Count - marker - 1);
        }
    }

    void AddSegment()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject tail = Instantiate(Segment, snakeBody[snakeBody.Count - 1].transform.position, snakeBody[snakeBody.Count - 1].transform.rotation);
            snakeBody.Add(tail);
        }
    }
}
