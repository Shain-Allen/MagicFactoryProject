using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
	public event EventHandler OnBeltTimerCycle;
	public Dictionary<Vector2, GameObject> placeObjects = new Dictionary<Vector2, GameObject>();

	[SerializeField] float beltCycleTime = 5f;
	float beltCycleTimeLeft = 0f;

	private void Start()
	{
		beltCycleTimeLeft = beltCycleTime;
	}

	private void Update()
	{
		beltCycleTimeLeft -= Time.deltaTime;

		if (beltCycleTimeLeft <= 0)
		{
			beltCycleTimeLeft = beltCycleTime;
			OnBeltTimerCycle?.Invoke(this, EventArgs.Empty);
		}
	}
}
