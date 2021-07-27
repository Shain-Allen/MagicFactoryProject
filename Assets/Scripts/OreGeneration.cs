using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGeneration : MonoBehaviour
{
    // Determines the maximum radial distance of the ores from the spawn center
    private const int MAX_RADIUS = 3;
    // Essentially translates to % of all tiles that are the center of an ore vein.
    private const float SPAWN_CHANCE = 0.003f;

    public static void GenerateOres(GridControl grid, GameObject oreName, int seed, int width, int height)
    {
        System.Random randGen = new System.Random(seed);
        GenerateOres(grid, oreName, randGen, -width/2, width/2, -height/2, height/2);
    }

    private static void GenerateOres(GridControl grid, GameObject oreName, System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        List<Vector3> oreSpawns = GetSpawnLocations(randGen, leftEdge, rightEdge, topEdge, bottomEdge);

        foreach(Vector3 oreCenter in oreSpawns)
            SpawnVein(grid, oreName, oreCenter, randGen, leftEdge, rightEdge, topEdge, bottomEdge);
    }

    private static List<Vector3> GetSpawnLocations(System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        List<Vector3> OreSpawns = new List<Vector3>();

        for (int x = leftEdge; x <= rightEdge; x++)
            for (int y = topEdge; y <= bottomEdge; y++)
                if(randGen.NextDouble() <= SPAWN_CHANCE)
                    OreSpawns.Add(new Vector3(x, y, 0));

        return OreSpawns;
    }

    private static void SpawnVein(GridControl grid, GameObject oreName, Vector3 center, System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        grid.placeObjects.Add(center, Instantiate(oreName, center, Quaternion.identity, grid.transform));
    }
}