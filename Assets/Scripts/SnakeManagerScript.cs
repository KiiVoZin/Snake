using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class SnakeManagerScript : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float OffSet;
    [SerializeField] GameObject Segment, Food;
    List<GameObject> snakeBody = new List<GameObject>();
    List<Vector2> MarkerList;
    bool boolFPSIncrease;
    Vector2 HeadPosition;
    // Start is called before the first frame update
    void Start()
    {
        MarkerList = new List<Vector2>();
        GameObject head = Instantiate(Segment, new Vector3(0, 0, 0), transform.rotation);
        snakeBody.Add(head);
        HeadPosition = snakeBody[0].transform.position;
        GenerateFood();
    }

    // Update is called once per frame
    void Update()
    {
        HeadPosition = snakeBody[0].transform.position;
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

        SnakeMovement();
        EatFood();
        for (int i = 1; i < MarkerList.Count; i++)
        {
            Debug.DrawLine(MarkerList[i - 1], MarkerList[i], Color.black);
        }
    }

    void SnakeMovement()
    {
        //Movement for the head
        GameObject head = snakeBody[0];
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        head.transform.right = (mousePos - HeadPosition).normalized;
        Vector2 move = Vector2.ClampMagnitude(MoveSpeed * Time.deltaTime * (Vector2)head.transform.right, Vector2.Distance(HeadPosition, mousePos));
        move += HeadPosition;
        if (Vector2.Distance(HeadPosition, mousePos) <= 0.05f) return;
        head.transform.position = move;
        MarkerList.Insert(0, move);

        //Movement for the body
        if (snakeBody.Count < 2)
        {
            MarkerList.Clear();
            return;
        }
        List<Vector2> newPositions = CalculateNewPositions();
        for (int i = 1; i < snakeBody.Count; i++)
        {
            snakeBody[i].transform.position = newPositions[i];
        }
    }
    List<Vector2> CalculateNewPositions()
    {
        List<Vector2> newPositions = new List<Vector2>
        {
            snakeBody[0].transform.position
        };

        int iterated = 1;
        int closestMarker = 0;
        for (int i = 1; i < snakeBody.Count; i++)
        {
            closestMarker = 0;
            float distance = Vector2.Distance(newPositions[i - 1], MarkerList[iterated]);
            for (int j = iterated; j < MarkerList.Count - 1; j++)
            {
                if (distance > OffSet)
                {
                    closestMarker = j;
                    iterated = j;
                    break;
                }
                else
                {
                    distance += Vector2.Distance(MarkerList[j], MarkerList[j + 1]);
                }
            }
            if (closestMarker == 0)
            {
                for (int k = i; k < snakeBody.Count; k++)
                {
                    newPositions.Add(snakeBody[i].transform.position);
                }
                    return newPositions;
            }
            Vector2 test = MarkerList[closestMarker] + ((MarkerList[closestMarker - 1] - MarkerList[closestMarker]).normalized * (distance - OffSet));
            newPositions.Add(test);
            if (i == snakeBody.Count - 1) MarkerList.RemoveRange(closestMarker + 2, MarkerList.Count - closestMarker - 2);
        }

        return newPositions;
    }
    void AddSegment()
    {
        GameObject tail = Instantiate(Segment, snakeBody[snakeBody.Count - 1].transform.position, snakeBody[snakeBody.Count - 1].transform.rotation);
        MarkerList.Add(tail.transform.position);
        snakeBody.Add(tail);
    }

    void GenerateFood()
    {
        Vector2 position = new Vector2(Random.Range(Camera.main.pixelWidth / 10, Camera.main.pixelWidth * 9 / 10), Random.Range(Camera.main.pixelHeight / 10, Camera.main.pixelHeight * 9 / 10));
        position = Camera.main.ScreenToWorldPoint(position);
        GameObject food = Instantiate(Food, position, transform.rotation);
        CollisionScript.Add(food);
    }

    void EatFood()
    {
        GameObject eaten = CollisionScript.CollideWithIntersect(snakeBody[0], HeadPosition);
        if (eaten == null) return;
        GenerateFood();
        CollisionScript.Remove(eaten);
        Destroy(eaten);
        AddSegment();
        Debug.Log("EAT");
    }
}
