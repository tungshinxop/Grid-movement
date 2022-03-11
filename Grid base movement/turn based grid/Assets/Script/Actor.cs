using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public float perTileMoveSpeed = 0.2f;

    private Vector3 currentPosition;
    public IEnumerator MoveSequence(List<Vector3> wayPoints)
    {
        for(int i = 0; i < wayPoints.Count; i ++)
        {
            yield return StartCoroutine(MoveRoutine(wayPoints[i]));
        }
    }

    IEnumerator MoveRoutine(Vector3 end)
    {
        print("Move routine");
        var elapsedTime = 0f;
        while(elapsedTime < perTileMoveSpeed){
            transform.position = Vector3.Lerp(currentPosition ,end , elapsedTime/perTileMoveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UpdatePosition(end);
    }

    public void UpdatePosition(Vector3 pos)
    {
        transform.position = pos;
        currentPosition = transform.position;
    }
}
