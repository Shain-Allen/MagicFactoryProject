using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
	public event EventHandler OnBeltTimerCycle;
	public Dictionary<Vector2, GameObject> placeObjects = new Dictionary<Vector2, GameObject>();
	public Dictionary<Vector2, GameObject> oreObjects = new Dictionary<Vector2, GameObject>();

	public float beltCycleTime { get; } = 2f;
	float beltCycleTimeLeft = 0f;

	public GameObject oreName;

	private void Start()
	{
		beltCycleTimeLeft = beltCycleTime;
		for (int x = -2; x <= 2; x++)
			for (int y = -2; y <= 2; y++)
			{
				OreGeneration.GenerateOres(this, oreName, 0, x, y);
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
