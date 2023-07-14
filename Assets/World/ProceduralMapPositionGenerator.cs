using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomPosition {
    public Vector2Int position;
    public Vector2Int dimensions;
    public bool playerAndBossRoom;

    HashSet<Vector2Int> ToPositions() {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

        int upperX = position.x + dimensions.x;
        int upperY = position.y + dimensions.y;
        for(int x = position.x; x < upperX; x++) {
            for(int y = position.y; y < upperY; y++) {
                positions.Add(new Vector2Int(x, y));
            }
        }

        return positions;
    }
}

public class ProceduralMapPositionGenerator : MonoBehaviour
{
    public Vector2Int startRoom;
    public Vector2Int numRooms;

    [HideInInspector]
    public HashSet<Vector2Int> AllPositions = new HashSet<Vector2Int>();


    // Start is called before the first frame update
    void Start()
    {
    }

    public void Generate() {
        
    }
}
