using System;
using Duckov.UI;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class ViewTabDisplayEntry : MonoBehaviour
{
	// Token: 0x06000F1C RID: 3868 RVA: 0x0003BB88 File Offset: 0x00039D88
	private void Awake()
	{
		ManagedUIElement.onOpen += this.OnViewOpen;
		ManagedUIElement.onClose += this.OnViewClose;
		this.HideIndicator();
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0003BBB2 File Offset: 0x00039DB2
	private void OnDestroy()
	{
		ManagedUIElement.onOpen -= this.OnViewOpen;
		ManagedUIElement.onClose -= this.OnViewClose;
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x0003BBD6 File Offset: 0x00039DD6
	private void Start()
	{
		if (View.ActiveView != null && View.ActiveView.GetType().Name == this.viewTypeName)
		{
			this.ShowIndicator();
		}
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0003BC07 File Offset: 0x00039E07
	private void OnViewClose(ManagedUIElement element)
	{
		if (element.GetType().Name == this.viewTypeName)
		{
			this.HideIndicator();
		}
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x0003BC27 File Offset: 0x00039E27
	private void OnViewOpen(ManagedUIElement element)
	{
		if (element.GetType().Name == this.viewTypeName)
		{
			this.ShowIndicator();
		}
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0003BC47 File Offset: 0x00039E47
	private void ShowIndicator()
	{
		this.indicator.SetActive(true);
		this.punch.Punch();
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0003BC60 File Offset: 0x00039E60
	private void HideIndicator()
	{
		this.indicator.SetActive(false);
	}

	// Token: 0x04000C5D RID: 3165
	[SerializeField]
	private string viewTypeName;

	// Token: 0x04000C5E RID: 3166
	[SerializeField]
	private GameObject indicator;

	// Token: 0x04000C5F RID: 3167
	[SerializeField]
	private PunchReceiver punch;
}
