using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class ViewTabs : MonoBehaviour
{
	// Token: 0x06000F18 RID: 3864 RVA: 0x0003BB44 File Offset: 0x00039D44
	public void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x0003BB51 File Offset: 0x00039D51
	public void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x0003BB5E File Offset: 0x00039D5E
	private void Update()
	{
		if (this.fadeGroup.IsShown && View.ActiveView == null)
		{
			this.Hide();
		}
	}

	// Token: 0x04000C5C RID: 3164
	[SerializeField]
	private FadeGroup fadeGroup;
}
