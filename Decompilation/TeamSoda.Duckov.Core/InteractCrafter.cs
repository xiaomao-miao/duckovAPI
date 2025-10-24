using System;
using System.Linq;
using Sirenix.Utilities;

// Token: 0x020001A9 RID: 425
public class InteractCrafter : InteractableBase
{
	// Token: 0x06000C8A RID: 3210 RVA: 0x00034C2D File Offset: 0x00032E2D
	protected override void Awake()
	{
		base.Awake();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x00034C3C File Offset: 0x00032E3C
	protected override void OnInteractFinished()
	{
		base.OnInteractFinished();
		CraftView.SetupAndOpenView(new Predicate<CraftingFormula>(this.FilterCraft));
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x00034C55 File Offset: 0x00032E55
	private bool FilterCraft(CraftingFormula formula)
	{
		return this.requireTag.IsNullOrWhitespace() || formula.tags.Contains(this.requireTag);
	}

	// Token: 0x04000ADE RID: 2782
	public string requireTag;
}
