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

    public static void GenerateOres(GridControl grid, GameObject oreName, int seed, int width, int height)
    {
        System.Random randGen = new System.Random(seed);
        GenerateOres(grid, oreName, randGen, -width/2, width/2, -height/2, height/2);
    }

    private static void GenerateOres(GridControl grid, GameObject oreName, System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        List<Vector3Int> oreSpawns = GetSpawnLocations(randGen, leftEdge, rightEdge, topEdge, bottomEdge);

        foreach(Vector3Int oreCenter in oreSpawns)
            SpawnVein(grid, oreName, oreCenter, randGen, leftEdge, rightEdge, topEdge, bottomEdge);
    }

    private static List<Vector3Int> GetSpawnLocations(System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        List<Vector3Int> OreSpawns = new List<Vector3Int>();

        for (int x = leftEdge; x <= rightEdge; x++)
            for (int y = topEdge; y <= bottomEdge; y++)
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
                bool empty = !grid.placeObjects.TryGetValue((new Vector2(x, y)), out temp);
                if (dist <= rad && empty)
                    grid.placeObjects.Add(new Vector2(x, y), Instantiate(oreName, new Vector3(x, y, 0), Quaternion.identity, grid.transform));
            }
    }
}