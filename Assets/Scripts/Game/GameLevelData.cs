using UnityEngine;
using System.Collections.Generic;

public class GameLevelData
{
    public Vector3 playerStartPosition;
    public Vector3 playerExitPosition;
    public List<EnemyData> enemies;
    public List<EdibleData> edibles;
    public List<RocketPartData> rocketParts;
}
