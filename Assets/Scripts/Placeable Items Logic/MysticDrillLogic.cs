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
		//Debug.Log("Event triggered");
		if (!isMining)
		{
			List<GameObject> ores = new List<GameObject>();
			GameObject outputOre = null;

			GameObject chunkParent;
			for (int x = -1; x < 2; x++)
			{
				for (int y = 1; y > -2; y--)
				{
					if (grid.worldChunks.TryGetValue(GetChunk(transform.position + new Vector3(x, y)), out chunkParent))
					{
						//Debug.Log(transform.position);
						//Debug.Log(transform.position + new Vector3(x, y));
						//Debug.Log(chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(transform.position + new Vector3(x, y)).x, PosToPosInChunk(transform.position + new Vector3(x, y)).y].name);
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
		//Debug.Log("IEnumerator Triggered");
		drill.isMining = true;

		//float timeElapsed = 0;
		/*
		while (timeElapsed <= grid.beltCycleTime * 4 * 10)
		{
			timeElapsed += Time.deltaTime;
		}*/

		yield return new WaitForSeconds(grid.beltCycleTime * 4);

		GameObject chunkParent;
		BeltLogic outputBelt;
		if (grid.worldChunks.TryGetValue(GetChunk(drill.transform.position + drill.outputLocation), out chunkParent))
		{
			//Debug.Log($"Chunk name {chunkParent.name}");
			if (chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(drill.transform.position + drill.outputLocation).x, PosToPosInChunk(drill.transform.position + drill.outputLocation).y].TryGetComponent<BeltLogic>(out outputBelt))
			{

				yield return new WaitWhile(() => outputBelt.getItemSlot());

				//Debug.Log($"belt should be here {outputBelt.transform.position}\n belt is here {drill.transform.position + drill.outputLocation}");
				if (!outputBelt.getItemSlot())
				{
					GameObject returnOre;
					outputOre.GetComponent<BaseOre>().MineOre(out returnOre);
					outputBelt.InsertItem(returnOre);
					//Debug.Log("Item mined");
				}
			}
		}

		drill.isMining = false;
		yield return null;
	}
}
