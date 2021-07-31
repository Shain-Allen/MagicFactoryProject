using System;
using System.Collections.Generic;
using UnityEngine;

public class OreGeneration : MonoBehaviour
{
    // Determines the maximum radial distance of the ores from the spawn center
    private const float MAX_RADIUS = 8.5f;
    // Essentially translates to % of all tiles that are the center of an ore vein.
    private const float SPAWN_CHANCE = 0.0003f;
    // The size of chunks! Chunks are squares, so this is both the height and the width
    public const int chunkSize = 32;

    /* GenerateOres procedurally places oreName in a chunk based off of the seed.
     * PRECONDITIONS: chunkX and chunkY are between INT32MIN/128 and INT32MAX/128
     * cont.: Seed must be between 0 and INT32MAX
     * POSTCONDITIONS: Only the inputted chunk will be altered
     * Ores generated by this function will not overwrite any other ores already present
     * HELPERS: GetChunkID, GenerateOresInChunk
     */
    public static void LoadChunkOres(GridControl grid, int seed, int chunkX, int chunkY)
    {
        GameObject curChunkParent;
        if(grid.loadedChunks.TryGetValue(new Vector2Int(chunkX, chunkY), out curChunkParent))
            return;
        curChunkParent = Instantiate(new GameObject($"({chunkX},{chunkY})"), new Vector3(chunkX * chunkSize, chunkY * chunkSize, 0), Quaternion.identity, grid.transform);
                
        // 1st  4th  7th (This shows the order of chunk loading, therefore meaning it's the priority order)
        // 2nd [5th] 8th ('5th' is the chunk currently generating)
        // 3rd  6th  9th (Only '5th' will have ores placed in it)
        for (int x = -1; x <= 1; x++)
            for (int y = 1; y >= -1; y--)
                GenerateOresInChunk(grid, seed, chunkX, chunkY, chunkX+x, chunkY+y, curChunkParent);
        
        grid.loadedChunks.Add(new Vector2Int(chunkX, chunkY), curChunkParent);
    }

    /* Spawns all ores that have a center within fromChunkX/Y chunk but only places the ones inside chunkX/Y
     * Note: This is done because without this, ore veins will never go over chunk borders
     * cont.: This is the only way to ensure that the world generation will not be affected by the order of chunk generation
     * PRECONDITIONS: All the "Edge" variables are the edges (X and Y) of the chunk
     * cont.: Edges are always inclusive, meaning for Chunk 0,0 they are 0, 127, 0, 127
     * HELPERS: GetChunkID, GetSpawnLocations, SpawnVein
     */
    private static void GenerateOresInChunk(GridControl grid, int seed, int chunkX, int chunkY, int fromChunkX, int fromChunkY, GameObject curChunkParent)
    {
        System.Random randGen = new System.Random(seed - GetChunkID(fromChunkX, fromChunkY));
        List<Vector3Int> oreSpawns = GetSpawnLocations(randGen, fromChunkX * chunkSize, fromChunkY * chunkSize, grid.oreNames.Count);

        randGen = new System.Random(seed - GetChunkID(chunkX, chunkY));
        int minX = chunkX * chunkSize;
        int minY = chunkY * chunkSize;
        foreach(Vector3Int oreCenter in oreSpawns)
            SpawnVein(grid, grid.oreNames[oreCenter.z], oreCenter, randGen, minX, minX + chunkSize - 1, minY, minY + chunkSize - 1, curChunkParent);
    } 
    
    /* GetChunkID converts the chunk X and Y into a single Int
     * PRECONDITIONS: chunkX and chunkY should both be between roughly -30,000 and 30,000 to prevent OverFlows
     * POSTCONDITIONS: Returned int will be bwtween 0 and INT32MAX
     * Recommend seeing SpiralChunkIDs.xlsx to better understand chunk IDs
     */
    public static int GetChunkID(int x, int y)
    {
        int sprialLayer = Math.Max(Math.Abs(x), Math.Abs(y));
        int topLeft = (int)Math.Pow(sprialLayer * 2 + 1, 2) - 1;
        int diff = Math.Abs(x + y);

        if (x < 0 && -x >= Math.Abs(y))
            return topLeft - diff;

        int bottomRight = (int)Math.Pow(sprialLayer * 2, 2);
        if (y < 0 && Math.Abs(y) >= Math.Abs(x))
            return bottomRight + diff;
        if (x > y)
            return bottomRight - diff;
        
        int topRight = bottomRight - (topLeft - bottomRight) / 2;
        diff = Math.Abs(x - y);
        return topRight - diff;
    }

    /* GetSpawnLocations gets the center of each ore vein within the chunk
     * PRECONDITIONS: All the edges are within INT32MIN and INT32MAX
     * POSTCONDITIONS: Returns a List of Vector3s with the positions of all the vein centers
     * cont.: All positions have a 0 in the Z coordinate
     * cont.: ALl positions are within (or equal to) the edges
     */
    private static List<Vector3Int> GetSpawnLocations(System.Random randGen, int minX, int minY, int numTypesOres)
    {
        List<Vector3Int> OreSpawns = new List<Vector3Int>();

        for (int x = minX; x < minX + chunkSize; x++)
            for (int y = minY; y < minY + chunkSize; y++)
                if(randGen.NextDouble() <= SPAWN_CHANCE)
                    OreSpawns.Add(new Vector3Int(x, y, randGen.Next(numTypesOres)));

        return OreSpawns;
    }

    /* SpawnVein takes a Vector3Int the represents the center of a vein, and spawns ores into the world around it
     * PRECONDITIONS: Center is inside of the bounds of the edges
     * POSTCONDITIONS: All ores for the vein will be Instantiated and added to the oreObjects dictionary
     * cont.: Ores will never be placed outside of the edges
     * HELPERS: insideBorder
     */
    private static void SpawnVein(GridControl grid, GameObject oreName, Vector3Int center, System.Random randGen, int left, int right, int bottom, int top, GameObject curChunkParent)
    {
        // rad will determine the radius of the vein by a random amount up to 3
        double rad = MAX_RADIUS - (randGen.NextDouble() * MAX_RADIUS / 2);
        // maxRad will always encompass all ores for the vein, this is used for the for loops
        int maxRad = (int)Math.Ceiling(rad);
        double dist, oddsOfOre;
        bool empty;
        GameObject temp;

        // For loops essentially look at the square around the center where MAX_RADIUS is contained, but doesn't go off the edge
        for (int x = center.x - maxRad; x <= center.x + maxRad; x++)
            for (int y = center.y - maxRad; y <= center.y + maxRad; y++)
            {
                dist = HelpFuncs.GetDistance(x, center.x, y, center.y);
                empty = !grid.oreObjects.TryGetValue((new Vector2(x, y)), out temp);
                oddsOfOre = dist <= rad ? 1 - Math.Pow(dist / rad, 3) : 0;
                if (randGen.NextDouble() <= oddsOfOre && HelpFuncs.insideBorder(x, y, left, right, bottom, top) && dist <= rad && empty)
                    grid.oreObjects.Add(new Vector2(x, y), Instantiate(oreName, new Vector3(x, y, 0), Quaternion.identity, curChunkParent.transform));
            }
    }
}