using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler
{
	private Transform itemSelector;
	private HotbarMenuControl HotbarController;
	private Button button;

	public void OnPointerClick(PointerEventData eventData_)
	{
		if (eventData_.button == PointerEventData.InputButton.Middle)
		{
			itemSelector.gameObject.SetActive(true);
			HotbarController.UpdateItemSelector(button);
		}
	}

	public void SetButtonReference(Transform itemSelector_, HotbarMenuControl hotbarMenuController_, Button button_)
	{
		itemSelector = itemSelector_;
		HotbarController = hotbarMenuController_;
		button = button_;
	}
}
