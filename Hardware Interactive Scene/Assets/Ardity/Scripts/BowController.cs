using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    public Transform topPos;
    public Transform botPos;

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
        bowstring.SetPosition(0, topPos.position);
        bowstring.SetPosition(2, botPos.position);
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
