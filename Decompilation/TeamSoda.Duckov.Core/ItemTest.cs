using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class ItemTest : MonoBehaviour
{
	// Token: 0x06000CB8 RID: 3256 RVA: 0x000354F1 File Offset: 0x000336F1
	public void DoInstantiate()
	{
		this.characterInstance = this.characterTemplate.CreateInstance();
		this.swordInstance = this.swordTemplate.CreateInstance();
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x00035518 File Offset: 0x00033718
	public void EquipSword()
	{
		Item item;
		this.characterInstance.Slots["Weapon"].Plug(this.swordInstance, out item);
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00035548 File Offset: 0x00033748
	public void UequipSword()
	{
		this.characterInstance.Slots["Weapon"].Unplug();
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00035565 File Offset: 0x00033765
	public void DestroyInstances()
	{
		if (this.characterInstance)
		{
			this.characterInstance.DestroyTreeImmediate();
		}
		if (this.swordInstance)
		{
			this.swordInstance.DestroyTreeImmediate();
		}
	}

	// Token: 0x04000B03 RID: 2819
	public Item characterTemplate;

	// Token: 0x04000B04 RID: 2820
	public Item swordTemplate;

	// Token: 0x04000B05 RID: 2821
	public Item characterInstance;

	// Token: 0x04000B06 RID: 2822
	public Item swordInstance;
}
