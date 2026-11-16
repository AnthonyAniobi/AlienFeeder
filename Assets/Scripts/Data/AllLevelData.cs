using System.Collections.Generic;
using UnityEngine;

class AllLevelData
{
    public Dictionary<int, GameLevelData> levels => new Dictionary<int, GameLevelData>
    {
        { 1, level1 },
    };

    private GameLevelData level1 = new GameLevelData(
        new Vector3(122, 1, -72), // player start position
        new Vector3(50, 1, -8), // player exit position
        new List<EnemyData> {
            new EnemyData(102, -59, 5f, 10f, true),
            new EnemyData(91, -68, 7f, 15f, false),
            new EnemyData(100.3f, -36, 10f, 20f, true),
            new EnemyData(99, -7.5f, 8f, 12f, false)
        },
        new List<EdibleData> {
            new EdibleData(100, -60, EdibleType.Banana),
            new EdibleData(90, -70, EdibleType.Apple),
            new EdibleData(95, -50, EdibleType.Banana),
            new EdibleData(110, -40, EdibleType.Apple),
            new EdibleData(105, -30, EdibleType.Banana)
        },
        new List<RocketPartData> {
            new RocketPartData(123, 36),
        }
    );
}


