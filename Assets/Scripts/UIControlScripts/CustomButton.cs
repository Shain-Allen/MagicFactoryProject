using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData_)
	{
		if (eventData_.button == PointerEventData.InputButton.Middle)
		{

		}
	}
}
