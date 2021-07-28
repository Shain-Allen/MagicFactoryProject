using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGeneration : MonoBehaviour
{
    // Determines the maximum radial distance of the ores from the spawn center
    private const float MAX_RADIUS = 4.5f;
    // Essentially translates to % of all tiles that are the center of an ore vein.
    private const float SPAWN_CHANCE = 0.003f;
    // The size of chunks!
    private const int chunkSize = 128;

    public static void GenerateOres(GridControl grid, GameObject oreName, int seed, int chunkX, int chunkY)
    {
        int minX = chunkX * chunkSize;
        int minY = chunkY * chunkSize;
        System.Random randGen = new System.Random(seed + GetChunkID(chunkX, chunkY));
        GenerateOres(grid, oreName, randGen, minX, minX + chunkSize, minY, minY + chunkSize);
    }

    private static int GetChunkID(int x, int y)
    {
        return x - y;
    }

    private static void GenerateOres(GridControl grid, GameObject oreName, System.Random randGen, int leftEdge, int rightEdge, int bottomEdge, int topEdge)
    {
        List<Vector3Int> oreSpawns = GetSpawnLocations(randGen, leftEdge, rightEdge, bottomEdge, topEdge);

        foreach(Vector3Int oreCenter in oreSpawns)
            SpawnVein(grid, oreName, oreCenter, randGen, leftEdge, rightEdge, bottomEdge, topEdge);
    }

    private static List<Vector3Int> GetSpawnLocations(System.Random randGen, int leftEdge, int rightEdge, int bottomEdge, int topEdge)
    {
        List<Vector3Int> OreSpawns = new List<Vector3Int>();

        for (int x = leftEdge; x <= rightEdge; x++)
            for (int y = bottomEdge; y <= topEdge; y++)
                if(randGen.NextDouble() <= SPAWN_CHANCE)
                    OreSpawns.Add(new Vector3Int(x, y, 0));

        return OreSpawns;
    }

    private static void SpawnVein(GridControl grid, GameObject oreName, Vector3Int center, System.Random randGen, int left, int right, int top, int bottom)
    {
        double dist;
        double rad = MAX_RADIUS - (randGen.NextDouble() * 3);
        int maxRad = (int)Math.Ceiling(rad);
        GameObject temp;

        // For loops essentially look at the square around the center where MAX_RADIUS is contained, but doesn't go off the edge
        for (int x = (center.x - maxRad < left ? left : center.x - maxRad); x <= (center.x + maxRad > right ? right : center.x + maxRad); x++)
            for (int y = (center.y - maxRad < top ? top : center.y - maxRad); y <= (center.y + maxRad > bottom ? bottom : center.y + maxRad); y++)
            {
                dist = Math.Sqrt( Math.Pow(x - center.x, 2) + Math.Pow(y - center.y, 2) );
                bool empty = !grid.oreObjects.TryGetValue((new Vector2(x, y)), out temp);
                if (dist <= rad && empty)
                    grid.oreObjects.Add(new Vector2(x, y), Instantiate(oreName, new Vector3(x, y, 0), Quaternion.identity, grid.transform));
            }
    }
}