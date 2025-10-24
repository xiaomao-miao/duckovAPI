using System;
using Duckov.Buffs;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000072 RID: 114
public struct ProjectileContext
{
	// Token: 0x04000371 RID: 881
	public Vector3 direction;

	// Token: 0x04000372 RID: 882
	public bool firstFrameCheck;

	// Token: 0x04000373 RID: 883
	public Vector3 firstFrameCheckStartPoint;

	// Token: 0x04000374 RID: 884
	public float halfDamageDistance;

	// Token: 0x04000375 RID: 885
	public float distance;

	// Token: 0x04000376 RID: 886
	public float speed;

	// Token: 0x04000377 RID: 887
	public Teams team;

	// Token: 0x04000378 RID: 888
	public int penetrate;

	// Token: 0x04000379 RID: 889
	public float damage;

	// Token: 0x0400037A RID: 890
	public float critDamageFactor;

	// Token: 0x0400037B RID: 891
	public float critRate;

	// Token: 0x0400037C RID: 892
	public float armorPiercing;

	// Token: 0x0400037D RID: 893
	public float armorBreak;

	// Token: 0x0400037E RID: 894
	public float element_Physics;

	// Token: 0x0400037F RID: 895
	public float element_Fire;

	// Token: 0x04000380 RID: 896
	public float element_Poison;

	// Token: 0x04000381 RID: 897
	public float element_Electricity;

	// Token: 0x04000382 RID: 898
	public float element_Space;

	// Token: 0x04000383 RID: 899
	public CharacterMainControl fromCharacter;

	// Token: 0x04000384 RID: 900
	public float gravity;

	// Token: 0x04000385 RID: 901
	public float explosionRange;

	// Token: 0x04000386 RID: 902
	public float explosionDamage;

	// Token: 0x04000387 RID: 903
	public float buffChance;

	// Token: 0x04000388 RID: 904
	public Buff buff;

	// Token: 0x04000389 RID: 905
	public float bleedChance;

	// Token: 0x0400038A RID: 906
	public bool ignoreHalfObsticle;

	// Token: 0x0400038B RID: 907
	[ItemTypeID]
	public int fromWeaponItemID;
}
