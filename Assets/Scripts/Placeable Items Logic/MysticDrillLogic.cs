using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;

public class MysticDrillLogic : Placeable
{
	private GridControl grid;
	private Vector3 outputLocation;
	private bool isMining = false;
	private List<BaseOre> minableOres = new List<BaseOre>();
	[SerializeField]
	private int miningRadius = 2;

	public override void PlacedAction(GridControl _grid)
	{
		grid = _grid;
		outputLocation = transform.up * 2;

		// Make sure this can be placed, otherwise stop
		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				if (GetPlaceableAt<Placeable>(grid, transform.position + (Vector3.right * x) + (Vector3.up * y)))
					return;

		// Place this drill into the world
		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				AddToWorld(grid, this, (Vector3.right * x) + (Vector3.up * y));

		// Fill the minableOres list with all the minable ores
		BaseOre tempOre;
		for (int x = -miningRadius; x <= miningRadius; x++)
			for (int y = -miningRadius; y <= miningRadius; y++)
				if (tempOre = GetResourceAt<BaseOre>(grid, transform.position + new Vector3(x, y)))
					minableOres.Add(tempOre);

		grid.Tick += TryMineOre;
	}

	public override void RemovedAction()
	{
		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				RemoveFromWorld(grid, this, (Vector3.right * x) + (Vector3.up * y));

		grid.Tick -= TryMineOre;
		Destroy(gameObject);
	}

	public void TryMineOre(object sender, EventArgs e)
	{
		if (!isMining && minableOres.Count != 0)
			StartCoroutine(Mining());
	}

	private IEnumerator Mining()
	{
		int oreInListToMine = UnityEngine.Random.Range(0, minableOres.Count);
		BaseOre outputOre = minableOres[oreInListToMine];
		GameObject returnOre;
		isMining = true;

		yield return new WaitForSeconds(grid.beltCycleTime * 4);

		BeltLogic outputBelt = GetPlaceableAt<BeltLogic>(grid, transform.position + outputLocation);
		if (outputBelt)
		{
			yield return new WaitWhile(() => outputBelt.AllowItem(null));

			outputOre.MineOre(out returnOre);
			outputBelt.InsertItem(returnOre);
			if (outputOre == null)
				minableOres.RemoveAt(oreInListToMine);
		}

		isMining = false;
		yield return null;
	}
}