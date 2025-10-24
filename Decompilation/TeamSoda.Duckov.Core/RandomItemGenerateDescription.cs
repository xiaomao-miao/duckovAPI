using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000F9 RID: 249
[Serializable]
public struct RandomItemGenerateDescription
{
	// Token: 0x06000853 RID: 2131 RVA: 0x00024FBC File Offset: 0x000231BC
	public UniTask<List<Item>> Generate(int count = -1)
	{
		RandomItemGenerateDescription.<Generate>d__12 <Generate>d__;
		<Generate>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Generate>d__.<>4__this = this;
		<Generate>d__.count = count;
		<Generate>d__.<>1__state = -1;
		<Generate>d__.<>t__builder.Start<RandomItemGenerateDescription.<Generate>d__12>(ref <Generate>d__);
		return <Generate>d__.<>t__builder.Task;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0002500C File Offset: 0x0002320C
	private void SetDurabilityIfNeeded(Item targetItem)
	{
		if (targetItem == null)
		{
			return;
		}
		if (this.controlDurability && targetItem.UseDurability)
		{
			float num = UnityEngine.Random.Range(this.durabilityIntegrity.x, this.durabilityIntegrity.y);
			targetItem.DurabilityLoss = 1f - num;
			float num2 = UnityEngine.Random.Range(this.durability.x, this.durability.y);
			if (num2 > num)
			{
				num2 = num;
			}
			targetItem.Durability = targetItem.MaxDurability * num2;
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0002508C File Offset: 0x0002328C
	private void RefreshPercent()
	{
		this.itemPool.RefreshPercent();
	}

	// Token: 0x04000780 RID: 1920
	[TextArea]
	[SerializeField]
	private string comment;

	// Token: 0x04000781 RID: 1921
	[Range(0f, 1f)]
	public float chance;

	// Token: 0x04000782 RID: 1922
	public Vector2Int randomCount;

	// Token: 0x04000783 RID: 1923
	public bool controlDurability;

	// Token: 0x04000784 RID: 1924
	public Vector2 durability;

	// Token: 0x04000785 RID: 1925
	public Vector2 durabilityIntegrity;

	// Token: 0x04000786 RID: 1926
	public bool randomFromPool;

	// Token: 0x04000787 RID: 1927
	[SerializeField]
	public RandomContainer<RandomItemGenerateDescription.Entry> itemPool;

	// Token: 0x04000788 RID: 1928
	public RandomContainer<Tag> tags;

	// Token: 0x04000789 RID: 1929
	public List<Tag> addtionalRequireTags;

	// Token: 0x0400078A RID: 1930
	public List<Tag> excludeTags;

	// Token: 0x0400078B RID: 1931
	public RandomContainer<int> qualities;

	// Token: 0x0200047F RID: 1151
	[Serializable]
	public struct Entry
	{
		// Token: 0x04001B78 RID: 7032
		[ItemTypeID]
		[SerializeField]
		public int itemTypeID;
	}
}
