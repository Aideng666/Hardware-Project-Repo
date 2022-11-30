using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] Transform defaultPos;
    [SerializeField] Transform drawnPos;
    [SerializeField] Transform bullseyePos;
    [SerializeField] Transform badShotPos;

    public List<Vector3> positionList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        positionList.Add(defaultPos.position);
        positionList.Add(drawnPos.position);
        positionList.Add(bullseyePos.position);
        positionList.Add(badShotPos.position);
    }

    public void SetPosition(int posIndex)
    {
         transform.position = positionList[posIndex];
    }
}
