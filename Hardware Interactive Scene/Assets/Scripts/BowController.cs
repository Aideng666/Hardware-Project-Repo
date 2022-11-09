using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BowController : MonoBehaviour
{
    public GameObject led;
    public Transform topPos;
    public Transform botPos;
    public Color lightOnColor;
    public Color lightOffColor;

    float maxDrawDistance = 2;
    float equalTensionThreshold = 1;
    bool isDrawing;

    Rigidbody2D body;
    LineRenderer bowstring;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bowstring = GetComponent<LineRenderer>();

        bowstring.SetPosition(0, topPos.position);
        bowstring.SetPosition(1, new Vector3(topPos.position.x, 0, topPos.position.z));
        bowstring.SetPosition(2, topPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = Vector2.zero;

        bowstring.SetPosition(0, topPos.position);
        bowstring.SetPosition(2, botPos.position);

        if (Input.GetKey(KeyCode.W))
        {
            body.velocity = Vector2.up * 5;
            bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            body.velocity = Vector2.down * 5;
            bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            body.velocity = Vector2.left * 5;
            bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            body.velocity = Vector2.right * 5;
            bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        }

        if (body.velocity.magnitude > 3)
        {
            led.GetComponent<SpriteRenderer>().color = lightOffColor;
        }
        else
        {
            led.GetComponent<SpriteRenderer>().color = lightOnColor;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.right * 5 * Time.deltaTime));
            isDrawing = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.left * 5 * Time.deltaTime));
            isDrawing = true;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.up * 5 * Time.deltaTime));
            isDrawing = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.down * 5 * Time.deltaTime));
            isDrawing = true;
        }
        else
        {
            isDrawing = false;
        }

        if (bowstring.GetPosition(1).x < bowstring.GetPosition(0).x)
        {
            bowstring.SetPosition(1, new Vector3(bowstring.GetPosition(0).x, bowstring.GetPosition(1).y, 0));
        }
        if (bowstring.GetPosition(1).x > bowstring.GetPosition(0).x + maxDrawDistance)
        {
            bowstring.SetPosition(1, new Vector3(bowstring.GetPosition(0).x + maxDrawDistance, bowstring.GetPosition(1).y, 0));
        }

        float topDist = Vector3.Distance(bowstring.GetPosition(0), bowstring.GetPosition(1));
        float botDist = Vector3.Distance(bowstring.GetPosition(2), bowstring.GetPosition(1));

        print($"{bowstring.GetPosition(1).x} | {bowstring.GetPosition(0).x + (maxDrawDistance / 2)}");

        if (Mathf.Max(topDist, botDist) - Mathf.Min(topDist, botDist) < equalTensionThreshold && bowstring.GetPosition(1).x >= bowstring.GetPosition(0).x + (maxDrawDistance / 2))
        {
            print("HI");

            Camera.main.transform.DOShakePosition(0.1f, 0.1f, 2);
        }
    }

    public void InterpretMessage(string msg)
    {
        body.velocity = Vector2.zero;

        string[] messageSplit = msg.Split(':');

        float xAccel = float.Parse(messageSplit[0]);
        float yAccel = float.Parse(messageSplit[2]);

        if (xAccel > 1f || yAccel > 1f || xAccel < -1f || yAccel < -1f)
        {
            float xVel = body.velocity.x + xAccel * Time.deltaTime;
            float yVel = body.velocity.y + yAccel * Time.deltaTime;

            print(xVel * 1000 + " " + yVel * 1000);

            body.velocity = new Vector2(xVel, yVel) * 1000;
        }
        else
        {
            body.velocity = Vector2.zero;
        }

        float topFlexAmount = float.Parse(messageSplit[3]);
        float botFlexAmount = float.Parse(messageSplit[4]);

        float averageFlexAmount = (topFlexAmount + botFlexAmount) / 2;

        bowstring.SetPosition(1, new Vector3(Mathf.Lerp(topPos.position.x, topPos.position.x + 1.75f, averageFlexAmount / 1000), 0, topPos.position.z));
    }
}
