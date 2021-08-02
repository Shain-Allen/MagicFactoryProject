using UnityEngine;

[CreateAssetMenu(fileName = "GasInfo", menuName = "GasInfoAsset")]
public class GasInfoSO : ScriptableObject
{
	public int baseGasAmount = 1;
	public int DistanceMultiplyer = 1;
	public int returnObjectIndex;
	public ObjectDictionary itemDictionary;

	private void OnEnable()
	{
		itemDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/ItemDictionary");
	}
}
