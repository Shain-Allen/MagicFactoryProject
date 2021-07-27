using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGeneration
{
    // Determines the maximum radial distance of the ores from the spawn center
    private const int MAX_RADIUS = 3;
    // Essentially translates to % of all tiles that are the center of an ore vein.
    private const float SPAWN_CHANCE = 0.02f;

    public void GenerateOres(GridControl grid, int seed, int height, int width)
    {
        System.Random randGen = new System.Random(seed);
        GenerateOres(grid, randGen, -width/2, width/2, -height/2, height/2);
    }

    private void GenerateOres(GridControl grid, System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        List<Vector2> oreSpawns = GetSpawnLocations(randGen, leftEdge, rightEdge, topEdge, bottomEdge);

        foreach(Vector2 oreCenter in oreSpawns)
            SpawnVein(grid, oreCenter, randGen, leftEdge, rightEdge, topEdge, bottomEdge);
    }

    private List<Vector2> GetSpawnLocations(System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        List<Vector2> OreSpawns = new List<Vector2>();

        for (int x = leftEdge; x <= rightEdge; x++)
            for (int y = topEdge; y <= bottomEdge; y++)
                if(randGen.Next() <= SPAWN_CHANCE)
                    OreSpawns.Add(new Vector2(x, y));

        return OreSpawns;
    }

    private void SpawnVein(GridControl grid, Vector2 center, System.Random randGen, int leftEdge, int rightEdge, int topEdge, int bottomEdge)
    {
        //grid.placeObjects.Add(center, Instantiate(brushItem, grid.CellToWorld(grid.WorldToCell(center)), itemPreview.transform.rotation, grid.transform));
    }
}