using UnityEngine;
using System.Collections.Generic;

public class GameLevelData
{
    public MimicTypeEnum mimicType;
    public Vector3 playerStartPosition;
    public Vector3 playerExitPosition;
    public int totalBatteriesToCollect;
    public List<EnemyData> enemies;
    public List<EdibleData> edibles;

    public GameLevelData(
        MimicTypeEnum mimicType,
        Vector3 playerStartPosition, 
        Vector3 playerExitPosition, 
        int totalBatteriesToCollect,
        List<EnemyData> enemies, 
        List<EdibleData> edibles
    ){
        this.mimicType = mimicType;
        this.playerStartPosition = playerStartPosition;
        this.playerExitPosition = playerExitPosition;
        this.enemies = enemies;
        this.edibles = edibles;
        this.totalBatteriesToCollect = totalBatteriesToCollect;
    }
}
