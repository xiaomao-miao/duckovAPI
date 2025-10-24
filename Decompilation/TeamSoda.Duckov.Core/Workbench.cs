using System;
using Duckov.UI;

// Token: 0x02000197 RID: 407
public class Workbench : InteractableBase
{
	// Token: 0x06000BF3 RID: 3059 RVA: 0x00032C0B File Offset: 0x00030E0B
	protected override void OnInteractFinished()
	{
		ItemCustomizeSelectionView.Show();
	}
}
