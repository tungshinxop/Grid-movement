using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class BreadthFirstAlgorithm : MonoBehaviour
{
    public Queue<Vector3Int> queue;

    public GameObject pref;

    public Dictionary<Vector3Int, Vector3Int> visited;
    private Vector3Int[] searchDirection = new Vector3Int[]{
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
    };
    const int MAX_ITERATION = 10000;
    public MapManager mapManager;
    // Start is called before the first frame update
    

    public List<Vector3> GetPath(Vector3Int startPos, Vector3Int goalPos)
    {
        queue = new Queue<Vector3Int>();
        visited = new Dictionary<Vector3Int, Vector3Int>();
        queue.Enqueue(startPos);
        var numberOfIteration = 0;
        while(queue.Count > 0){
            //Getting the current search tile
            Vector3Int current = queue.Dequeue();

            Instantiate(pref, (Vector3)current + new Vector3(0.5f,0.5f), Quaternion.identity);
            //Finding the neighbours
            SearchNeighbour(current);

            if(ReachedGoal(current, goalPos)) break;

            numberOfIteration++;
            if(numberOfIteration > MAX_ITERATION) {
                print("Cannot find path");
                return null;
            }
        }

        List<Vector3> backtracedPath = new List<Vector3>();
        Vector3Int currentPos = goalPos;
        while(currentPos != startPos)
        {
            backtracedPath.Add(currentPos + mapManager.tilemap.tileAnchor);
            currentPos = visited[currentPos];
        }

        backtracedPath.Reverse();

        return backtracedPath;
    }
 
    void SearchNeighbour(Vector3Int current)
    {
        foreach(var dir in searchDirection){
            var neighbourPos = current + dir;
            if(!mapManager.IsTile(neighbourPos)|| !mapManager.IsTile(current)) continue;
            //Check if neighbour is duplicated, if not, add to the queue for checking later
            if(!visited.ContainsKey(neighbourPos) && mapManager.WalkableTile(neighbourPos)){
                queue.Enqueue(neighbourPos);
                visited.Add(neighbourPos, current);
            }
        }
    }

    bool ReachedGoal(Vector3Int current, Vector3Int goal){
        if(current == goal){
            return true;
        }

        return false;
    }
}
