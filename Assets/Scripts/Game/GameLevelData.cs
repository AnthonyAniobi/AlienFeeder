using UnityEngine;
using System.Collections.Generic;

public class GameLevelData
{
    public Vector3 playerStartPosition;
    public Vector3 playerExitPosition;
    public List<EnemyData> enemies;
    public List<EdibleData> edibles;
    public List<RocketPartData> rocketParts;

    public GameLevelData(
        Vector3 playerStartPosition, 
        Vector3 playerExitPosition, 
        List<EnemyData> enemies, 
        List<EdibleData> edibles, 
        List<RocketPartData> rocketParts
    ){
        this.playerStartPosition = playerStartPosition;
        this.playerExitPosition = playerExitPosition;
        this.enemies = enemies;
        this.edibles = edibles;
        this.rocketParts = rocketParts;
    }
}
