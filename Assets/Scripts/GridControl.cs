using System;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
	public event EventHandler OnBeltTimerCycle;
	public Dictionary<Vector2, GameObject> placeObjects = new Dictionary<Vector2, GameObject>();
	public Dictionary<Vector2, GameObject> oreObjects = new Dictionary<Vector2, GameObject>();
	// Vector2Int = Chunk coordinates, and GameObject is an empty object to be the parent of all ores in that chunk
	public Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();

	public float beltCycleTime { get; } = 2f;
	float beltCycleTimeLeft = 0f;
	public int worldSeed = 0;

	public List<GameObject> oreNames;

	private void Start()
	{
		beltCycleTimeLeft = beltCycleTime;
		
		int initialChunkSpawningRadius = 2;
		for (int x = -initialChunkSpawningRadius; x <= initialChunkSpawningRadius; x++)
		{
			for (int y = -initialChunkSpawningRadius; y <= initialChunkSpawningRadius; y++)
			{
				OreGeneration.LoadChunkOres(this, worldSeed, x, y);
			}
		}
	}

	private void Update()
	{
		beltCycleTimeLeft -= Time.deltaTime;

		if (beltCycleTimeLeft <= 0)
		{
			//Debug.Log("Belt pulse");
			beltCycleTimeLeft = beltCycleTime;
			OnBeltTimerCycle?.Invoke(this, EventArgs.Empty);
		}
	}
}
