using System;
using Duckov.Buffs;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class ModifierAction : EffectAction
{
	// Token: 0x060004C9 RID: 1225 RVA: 0x00015C08 File Offset: 0x00013E08
	protected override void Awake()
	{
		base.Awake();
		this.modifier = new Modifier(this.ModifierType, this.modifierValue, this.overrideOrder, this.overrideOrderValue, base.Master);
		this.targetStatHash = this.targetStatKey.GetHashCode();
		if (this.buff)
		{
			this.buff.OnLayerChangedEvent += this.OnBuffLayerChanged;
		}
		this.OnBuffLayerChanged();
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00015C7F File Offset: 0x00013E7F
	private void OnBuffLayerChanged()
	{
		if (!this.buff)
		{
			return;
		}
		if (this.modifier == null)
		{
			return;
		}
		this.modifier.Value = this.modifierValue * (float)this.buff.CurrentLayers;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00015CB8 File Offset: 0x00013EB8
	protected override void OnTriggered(bool positive)
	{
		if (base.Master.Item == null)
		{
			return;
		}
		Item characterItem = base.Master.Item.GetCharacterItem();
		if (characterItem == null)
		{
			return;
		}
		if (positive)
		{
			if (this.targetStat != null)
			{
				this.targetStat.RemoveModifier(this.modifier);
				this.targetStat = null;
			}
			this.targetStat = characterItem.GetStat(this.targetStatHash);
			this.targetStat.AddModifier(this.modifier);
			return;
		}
		if (this.targetStat != null)
		{
			this.targetStat.RemoveModifier(this.modifier);
			this.targetStat = null;
		}
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00015D60 File Offset: 0x00013F60
	private void OnDestroy()
	{
		if (this.targetStat != null)
		{
			this.targetStat.RemoveModifier(this.modifier);
			this.targetStat = null;
		}
		if (this.buff)
		{
			this.buff.OnLayerChangedEvent -= this.OnBuffLayerChanged;
		}
	}

	// Token: 0x04000402 RID: 1026
	[SerializeField]
	private Buff buff;

	// Token: 0x04000403 RID: 1027
	public string targetStatKey;

	// Token: 0x04000404 RID: 1028
	private int targetStatHash;

	// Token: 0x04000405 RID: 1029
	public ModifierType ModifierType;

	// Token: 0x04000406 RID: 1030
	public float modifierValue;

	// Token: 0x04000407 RID: 1031
	public bool overrideOrder;

	// Token: 0x04000408 RID: 1032
	public int overrideOrderValue;

	// Token: 0x04000409 RID: 1033
	private Modifier modifier;

	// Token: 0x0400040A RID: 1034
	private Stat targetStat;
}
