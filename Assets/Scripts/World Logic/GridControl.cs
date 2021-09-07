using System;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
	public event EventHandler OnBeltTimerCycle;
	public event EventHandler Tick;
	// Vector2Int = Chunk coordinates, and GameObject is an empty object to be the parent of all ores in that chunk
	public Dictionary<Vector2Int, GameObject> worldChunks = new Dictionary<Vector2Int, GameObject>();

	public float beltCycleTime { get; } = 2f;
	float beltCycleTimeLeft = 0f;
	public int worldSeed = 0;

	[SerializeField]
	private int tps = 10;
	private float tickRate;
	private float tickTimer = 0;

	public List<BaseResource> oreNames;
	public List<GameObject> oreOutputItems;
	public GameObject chunkParentObject;

	private void Start()
	{
		beltCycleTimeLeft = beltCycleTime;

		tickRate = 1f / tps;

		int initialChunkSpawningRadius = 0;
		for (int x = -initialChunkSpawningRadius; x <= initialChunkSpawningRadius; x++)
		{
			for (int y = -initialChunkSpawningRadius; y <= initialChunkSpawningRadius; y++)
			{
				OreGeneration.LoadChunkResources(this, worldSeed, new Vector2Int(x, y));
			}
		}
	}

	private void Update()
	{
		beltCycleTimeLeft -= Time.deltaTime;
		tickTimer += Time.deltaTime;

		if (beltCycleTimeLeft <= 0)
		{
			beltCycleTimeLeft = beltCycleTime;
			OnBeltTimerCycle?.Invoke(this, EventArgs.Empty);
		}

		if (tickTimer >= tickRate)
		{
			tickTimer = 0;
			Tick?.Invoke(this, EventArgs.Empty);
		}
	}
}
