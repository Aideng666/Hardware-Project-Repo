using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BowController : MonoBehaviour
{
    public GameObject arrow;
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

    bool isAimSteady;
    bool isTensionEven;
    bool isTensionApplied;

    bool previouslyDrawn;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bowstring = GetComponent<LineRenderer>();

        bowstring.SetPosition(0, topPos.position);
        bowstring.SetPosition(1, new Vector3(topPos.position.x, 0, topPos.position.z));
        bowstring.SetPosition(2, botPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        //body.velocity = Vector2.zero;

        //bowstring.SetPosition(0, topPos.position);
        //bowstring.SetPosition(2, botPos.position);

        //if (Input.GetKey(KeyCode.W))
        //{
        //    body.velocity = Vector2.up * 5;
        //    bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    body.velocity = Vector2.down * 5;
        //    bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    body.velocity = Vector2.left * 5;
        //    bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    body.velocity = Vector2.right * 5;
        //    bowstring.SetPosition(1, new Vector3(topPos.position.x, (topPos.position.y + botPos.position.y) / 2, 0));
        //}

        //if (body.velocity.magnitude > 3)
        //{
        //    led.GetComponent<SpriteRenderer>().color = lightOffColor;
        //}
        //else
        //{
        //    led.GetComponent<SpriteRenderer>().color = lightOnColor;
        //}

        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.right * 5 * Time.deltaTime));
        //    isDrawing = true;
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.left * 5 * Time.deltaTime));
        //    isDrawing = true;
        //}
        //else if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.up * 5 * Time.deltaTime));
        //    isDrawing = true;
        //}
        //else if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    bowstring.SetPosition(1, bowstring.GetPosition(1) + (Vector3.down * 5 * Time.deltaTime));
        //    isDrawing = true;
        //}
        //else
        //{
        //    isDrawing = false;
        //}

        //if (bowstring.GetPosition(1).x < bowstring.GetPosition(0).x)
        //{
        //    bowstring.SetPosition(1, new Vector3(bowstring.GetPosition(0).x, bowstring.GetPosition(1).y, 0));
        //}
        //if (bowstring.GetPosition(1).x > bowstring.GetPosition(0).x + maxDrawDistance)
        //{
        //    bowstring.SetPosition(1, new Vector3(bowstring.GetPosition(0).x + maxDrawDistance, bowstring.GetPosition(1).y, 0));
        //}

        //float topDist = Vector3.Distance(bowstring.GetPosition(0), bowstring.GetPosition(1));
        //float botDist = Vector3.Distance(bowstring.GetPosition(2), bowstring.GetPosition(1));

        //print($"{bowstring.GetPosition(1).x} | {bowstring.GetPosition(0).x + (maxDrawDistance / 2)}");

        //if (Mathf.Max(topDist, botDist) - Mathf.Min(topDist, botDist) < equalTensionThreshold && bowstring.GetPosition(1).x >= bowstring.GetPosition(0).x + (maxDrawDistance / 2))
        //{
        //    print("HI");

        //    Camera.main.transform.DOShakePosition(0.1f, 0.1f, 2);
        //}

        if (isTensionApplied && isTensionEven)
        {
            bowstring.SetPosition(1, new Vector3(topPos.position.x + 1.75f, 0, topPos.position.z));
            Camera.main.transform.DOShakePosition(0.1f, 0.1f, 2);
        }
        else
        {
            bowstring.SetPosition(1, new Vector3(topPos.position.x, 0, topPos.position.z));
        }

        if (isAimSteady)
        {
            led.GetComponent<SpriteRenderer>().color = lightOnColor;
        }
        else
        {
            led.GetComponent<SpriteRenderer>().color = lightOffColor;
        }


        if (isTensionApplied)
        {
            arrow.GetComponent<Arrow>().SetPosition(1);

            previouslyDrawn = true;
        }
    }

    public void InterpretMessage(string msg)
    {
        print($"Interpreting Message: {msg}");

        string[] messageSplit = msg.Split(':');

        switch (messageSplit[0])
        {
            case "Aim":

                if (int.Parse(messageSplit[1]) == 0)
                {
                    isAimSteady = false;
                }
                if (int.Parse(messageSplit[1]) == 1)
                {
                    isAimSteady = true;
                }

                break;

            case "Even":

                if (int.Parse(messageSplit[1]) == 0)
                {
                    isTensionEven = false;
                }
                if (int.Parse(messageSplit[1]) == 1)
                {
                    isTensionEven = true;
                }

                break;

            case "Applied":

                if (int.Parse(messageSplit[1]) == 0)
                {
                    isTensionApplied = false;

                    if (previouslyDrawn)
                    {
                        if (isAimSteady && isTensionEven)
                        {
                            arrow.GetComponent<Arrow>().SetPosition(2);
                        }
                        else
                        {
                            arrow.GetComponent<Arrow>().SetPosition(3);
                        }

                        previouslyDrawn = false;
                    }
                }
                if (int.Parse(messageSplit[1]) == 1)
                {
                    isTensionApplied = true;
                }

                break;
        }
    }
}
