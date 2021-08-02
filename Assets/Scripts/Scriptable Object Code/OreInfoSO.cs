using UnityEngine;

[CreateAssetMenu(fileName = "OreInfo", menuName = "OreInfoAsset")]
public class OreInfoSO : ScriptableObject
{
	public int baseOreAmount = 1;
	public int DistanceMultiplyer = 1;
	public int returnObjectIndex;
	public ObjectDictionary itemDictionary;

	private void OnEnable()
	{
		itemDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/ItemDictionary");
	}
}
