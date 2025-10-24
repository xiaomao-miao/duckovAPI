using System;
using System.Collections.Generic;
using Duckov.Buffs;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x0200006C RID: 108
[Serializable]
public struct DamageInfo
{
	// Token: 0x06000421 RID: 1057 RVA: 0x000123B8 File Offset: 0x000105B8
	public string GenerateDescription()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		if (this.fromCharacter != null)
		{
			if (this.fromCharacter.IsMainCharacter)
			{
				text = "DeathReason_Self".ToPlainText();
			}
			else if (this.fromCharacter.characterPreset != null)
			{
				text = this.fromCharacter.characterPreset.DisplayName;
			}
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.fromWeaponItemID);
		if (metaData.id > 0)
		{
			text2 = metaData.DisplayName;
		}
		if (this.isExplosion)
		{
			text2 = "DeathReason_Explosion".ToPlainText();
		}
		if (this.crit > 0)
		{
			text3 = "DeathReason_Critical".ToPlainText();
		}
		bool flag = string.IsNullOrEmpty(text);
		bool flag2 = string.IsNullOrEmpty(text2);
		if (flag && flag2)
		{
			return "?";
		}
		if (flag)
		{
			return text2;
		}
		if (flag2)
		{
			return text;
		}
		return string.Concat(new string[]
		{
			text,
			" (",
			text2,
			") ",
			text3
		});
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000124B8 File Offset: 0x000106B8
	public DamageInfo(CharacterMainControl fromCharacter = null)
	{
		this.damageValue = 0f;
		this.critDamageFactor = 1f;
		this.ignoreArmor = false;
		this.critRate = 0f;
		this.armorBreak = 0f;
		this.armorPiercing = 0f;
		this.fromCharacter = fromCharacter;
		this.toDamageReceiver = null;
		this.damagePoint = Vector3.zero;
		this.damageNormal = Vector3.up;
		this.elementFactors = new List<ElementFactor>();
		this.crit = -1;
		this.damageType = DamageTypes.normal;
		this.buffChance = 0f;
		this.buff = null;
		this.finalDamage = 0f;
		this.isFromBuffOrEffect = false;
		this.fromWeaponItemID = 0;
		this.isExplosion = false;
		this.bleedChance = 0f;
		this.ignoreDifficulty = false;
	}

	// Token: 0x04000316 RID: 790
	public DamageTypes damageType;

	// Token: 0x04000317 RID: 791
	public bool isFromBuffOrEffect;

	// Token: 0x04000318 RID: 792
	public float damageValue;

	// Token: 0x04000319 RID: 793
	public bool ignoreArmor;

	// Token: 0x0400031A RID: 794
	public bool ignoreDifficulty;

	// Token: 0x0400031B RID: 795
	public float critDamageFactor;

	// Token: 0x0400031C RID: 796
	public float critRate;

	// Token: 0x0400031D RID: 797
	public float armorPiercing;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	public List<ElementFactor> elementFactors;

	// Token: 0x0400031F RID: 799
	public bool isExplosion;

	// Token: 0x04000320 RID: 800
	public float armorBreak;

	// Token: 0x04000321 RID: 801
	public float finalDamage;

	// Token: 0x04000322 RID: 802
	public CharacterMainControl fromCharacter;

	// Token: 0x04000323 RID: 803
	public DamageReceiver toDamageReceiver;

	// Token: 0x04000324 RID: 804
	[HideInInspector]
	public Vector3 damagePoint;

	// Token: 0x04000325 RID: 805
	[HideInInspector]
	public Vector3 damageNormal;

	// Token: 0x04000326 RID: 806
	public int crit;

	// Token: 0x04000327 RID: 807
	[ItemTypeID]
	public int fromWeaponItemID;

	// Token: 0x04000328 RID: 808
	public float buffChance;

	// Token: 0x04000329 RID: 809
	public Buff buff;

	// Token: 0x0400032A RID: 810
	public float bleedChance;
}
