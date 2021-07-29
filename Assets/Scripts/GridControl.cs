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
		
		int temp = 7;
		for (int x = -temp; x <= temp; x++)
		{
			for (int y = -temp; y <= temp; y++)
			{
				OreGeneration.GenerateOres(this, oreName, 0, x, y);
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
