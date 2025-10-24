using System;
using Duckov.Options.UI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001E0 RID: 480
public class OptionsPanel_TabButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000E43 RID: 3651 RVA: 0x000397F3 File Offset: 0x000379F3
	public void OnPointerClick(PointerEventData eventData)
	{
		Action<OptionsPanel_TabButton, PointerEventData> action = this.onClicked;
		if (action == null)
		{
			return;
		}
		action(this, eventData);
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x00039808 File Offset: 0x00037A08
	internal void NotifySelectionChanged(OptionsPanel optionsPanel, OptionsPanel_TabButton selection)
	{
		bool active = selection == this;
		this.tab.SetActive(active);
		this.selectedIndicator.SetActive(active);
	}

	// Token: 0x04000BC9 RID: 3017
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000BCA RID: 3018
	[SerializeField]
	private GameObject tab;

	// Token: 0x04000BCB RID: 3019
	public Action<OptionsPanel_TabButton, PointerEventData> onClicked;
}
