using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;
using static HelpFuncs;

public class MysticDrillLogic : Placeable
{
	private GridControl grid;

	private Vector3 outputLocation;

	private bool isMining = false;

	public override void PlacedAction(GridControl _grid)
	{
		grid = _grid;
		outputLocation = transform.up * 2;

		//Debug.Log(gameObject.transform.position + outputLocation);

		AddToWorld(grid, this, Vector3.up + Vector3.left); AddToWorld(grid, this, Vector3.up); AddToWorld(grid, this, Vector3.up + Vector3.right);
		AddToWorld(grid, this, Vector3.left); AddToWorld(grid, this); AddToWorld(grid, this, Vector3.right);
		AddToWorld(grid, this, Vector3.down + Vector3.left); AddToWorld(grid, this, Vector3.down); AddToWorld(grid, this, Vector3.down + Vector3.right);

		grid.Tick += TryMineOre;
	}

	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this, Vector3.up + Vector3.left); RemoveFromWorld(grid, this, Vector3.up); RemoveFromWorld(grid, this, Vector3.up + Vector3.right);
		RemoveFromWorld(grid, this, Vector3.left); RemoveFromWorld(grid, this); RemoveFromWorld(grid, this, Vector3.right);
		RemoveFromWorld(grid, this, Vector3.down + Vector3.left); RemoveFromWorld(grid, this, Vector3.down); RemoveFromWorld(grid, this, Vector3.down + Vector3.right);

		grid.Tick -= TryMineOre;

		Destroy(gameObject);
	}

	public void TryMineOre(object sender, EventArgs e)
	{
		if (!isMining)
		{
			List<GameObject> ores = new List<GameObject>();
			GameObject outputOre = null;

			GameObject chunkParent;
			for (int x = -2; x <= 2; x++)
			{
				for (int y = 2; y >= -2; y--)
				{
					chunkParent = GetChunkParentByPos(grid, transform.position + new Vector3(x, y));
					if (chunkParent)
					{
						if (chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(transform.position + new Vector3(x, y)).x, PosToPosInChunk(transform.position + new Vector3(x, y)).y])
						{
							ores.Add(chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(transform.position + new Vector3(x, y)).x, PosToPosInChunk(transform.position + new Vector3(x, y)).y]);
						}
					}
				}
			}

			if (ores.Count != 0)
			{
				outputOre = ores[UnityEngine.Random.Range(0, ores.Count)];

				StartCoroutine(Mining(this, outputOre));
			}
		}
	}

	private IEnumerator Mining(MysticDrillLogic drill, GameObject outputOre)
	{
		drill.isMining = true;
		yield return new WaitForSeconds(grid.beltCycleTime * 4);

		BeltLogic outputBelt;
		GameObject chunkParent = GetChunkParentByPos(grid, drill.transform.position + drill.outputLocation);
		if (chunkParent && GetPlaceableAt<ItemControl>(grid, drill.transform.position + drill.outputLocation).TryGetComponent<BeltLogic>(out outputBelt))
		{
			yield return new WaitWhile(() => outputBelt.getItemSlot());

			// This should never be false, but it's just in case because this is a coroutine
			if (!outputBelt.getItemSlot())
			{
				GameObject returnOre;
				outputOre.GetComponent<BaseOre>().MineOre(out returnOre);
				outputBelt.InsertItem(returnOre);
			}

		}

		drill.isMining = false;
		yield return null;
	}
}
