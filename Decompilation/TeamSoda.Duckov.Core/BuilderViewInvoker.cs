using System;
using Duckov.Buildings;
using Duckov.Buildings.UI;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class BuilderViewInvoker : InteractableBase
{
	// Token: 0x06000C40 RID: 3136 RVA: 0x00033A38 File Offset: 0x00031C38
	protected override void OnInteractFinished()
	{
		if (this.buildingArea == null)
		{
			return;
		}
		BuilderView.Show(this.buildingArea);
	}

	// Token: 0x04000A9C RID: 2716
	[SerializeField]
	private BuildingArea buildingArea;
}
