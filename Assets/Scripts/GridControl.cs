using System;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
	public event EventHandler OnBeltTimerCycle;
	public Dictionary<Vector2, GameObject> placeObjects = new Dictionary<Vector2, GameObject>();
	public Dictionary<Vector2, GameObject> oreObjects = new Dictionary<Vector2, GameObject>();
	public Dictionary<Vector2Int, bool> loadedChunks = new Dictionary<Vector2Int, bool>();

	public float beltCycleTime { get; } = 2f;
	float beltCycleTimeLeft = 0f;
	public int worldSeed = 0;

	public GameObject oreName;

	private void Start()
	{
		beltCycleTimeLeft = beltCycleTime;
		
		int initialChunkSpawningRadius = 0;
		for (int x = -initialChunkSpawningRadius; x <= initialChunkSpawningRadius; x++)
		{
			for (int y = -initialChunkSpawningRadius; y <= initialChunkSpawningRadius; y++)
			{
				OreGeneration.LoadChunkOres(this, oreName, worldSeed, x, y);
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
