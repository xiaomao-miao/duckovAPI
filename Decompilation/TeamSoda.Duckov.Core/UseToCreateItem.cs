using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class UseToCreateItem : UsageBehavior
{
	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x0002509C File Offset: 0x0002329C
	public override UsageBehavior.DisplaySettingsData DisplaySettings
	{
		get
		{
			return new UsageBehavior.DisplaySettingsData
			{
				display = true,
				description = this.descKey.ToPlainText()
			};
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x000250CC File Offset: 0x000232CC
	public override bool CanBeUsed(Item item, object user)
	{
		return user as CharacterMainControl;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x000250E0 File Offset: 0x000232E0
	protected override void OnUse(Item item, object user)
	{
		CharacterMainControl characterMainControl = user as CharacterMainControl;
		if (!characterMainControl)
		{
			return;
		}
		if (this.entries.entries.Count == 0)
		{
			return;
		}
		UseToCreateItem.Entry random = this.entries.GetRandom(0f);
		this.Generate(random.itemTypeID, characterMainControl).Forget();
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00025134 File Offset: 0x00023334
	private UniTask Generate(int typeID, CharacterMainControl character)
	{
		UseToCreateItem.<Generate>d__9 <Generate>d__;
		<Generate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Generate>d__.<>4__this = this;
		<Generate>d__.typeID = typeID;
		<Generate>d__.character = character;
		<Generate>d__.<>1__state = -1;
		<Generate>d__.<>t__builder.Start<UseToCreateItem.<Generate>d__9>(ref <Generate>d__);
		return <Generate>d__.<>t__builder.Task;
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00025187 File Offset: 0x00023387
	private void OnValidate()
	{
		this.entries.RefreshPercent();
	}

	// Token: 0x0400078C RID: 1932
	[SerializeField]
	private RandomContainer<UseToCreateItem.Entry> entries;

	// Token: 0x0400078D RID: 1933
	[LocalizationKey("Items")]
	public string descKey;

	// Token: 0x0400078E RID: 1934
	[LocalizationKey("Default")]
	public string notificationKey;

	// Token: 0x0400078F RID: 1935
	private bool running;

	// Token: 0x02000481 RID: 1153
	[Serializable]
	private struct Entry
	{
		// Token: 0x04001B81 RID: 7041
		[ItemTypeID]
		[SerializeField]
		public int itemTypeID;
	}
}
