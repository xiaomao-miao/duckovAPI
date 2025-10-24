using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x020003F1 RID: 1009
	[Serializable]
	public class Ruleset
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x0007E951 File Offset: 0x0007CB51
		// (set) Token: 0x0600246B RID: 9323 RVA: 0x0007E963 File Offset: 0x0007CB63
		[LocalizationKey("UIText")]
		internal string descriptionKey
		{
			get
			{
				return this.displayNameKey + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x0007E965 File Offset: 0x0007CB65
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x0007E972 File Offset: 0x0007CB72
		public string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x0007E97F File Offset: 0x0007CB7F
		public bool SpawnDeadBody
		{
			get
			{
				return this.spawnDeadBody;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x0007E987 File Offset: 0x0007CB87
		public int SaveDeadbodyCount
		{
			get
			{
				return this.saveDeadbodyCount;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x0007E98F File Offset: 0x0007CB8F
		public bool FogOfWar
		{
			get
			{
				return this.fogOfWar;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x0007E997 File Offset: 0x0007CB97
		public bool AdvancedDebuffMode
		{
			get
			{
				return this.advancedDebuffMode;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x0007E99F File Offset: 0x0007CB9F
		public float RecoilMultiplier
		{
			get
			{
				return this.recoilMultiplier;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x0007E9A7 File Offset: 0x0007CBA7
		public float DamageFactor_ToPlayer
		{
			get
			{
				return this.damageFactor_ToPlayer;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x0007E9AF File Offset: 0x0007CBAF
		public float EnemyHealthFactor
		{
			get
			{
				return this.enemyHealthFactor;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x0007E9B7 File Offset: 0x0007CBB7
		public float EnemyReactionTimeFactor
		{
			get
			{
				return this.enemyReactionTimeFactor;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002476 RID: 9334 RVA: 0x0007E9BF File Offset: 0x0007CBBF
		public float EnemyAttackTimeSpaceFactor
		{
			get
			{
				return this.enemyAttackTimeSpaceFactor;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x0007E9C7 File Offset: 0x0007CBC7
		public float EnemyAttackTimeFactor
		{
			get
			{
				return this.enemyAttackTimeFactor;
			}
		}

		// Token: 0x040018C9 RID: 6345
		[LocalizationKey("UIText")]
		[SerializeField]
		internal string displayNameKey;

		// Token: 0x040018CA RID: 6346
		[SerializeField]
		private float damageFactor_ToPlayer = 1f;

		// Token: 0x040018CB RID: 6347
		[SerializeField]
		private float enemyHealthFactor = 1f;

		// Token: 0x040018CC RID: 6348
		[SerializeField]
		private bool spawnDeadBody = true;

		// Token: 0x040018CD RID: 6349
		[SerializeField]
		private bool fogOfWar = true;

		// Token: 0x040018CE RID: 6350
		[SerializeField]
		private bool advancedDebuffMode;

		// Token: 0x040018CF RID: 6351
		[SerializeField]
		private int saveDeadbodyCount = 1;

		// Token: 0x040018D0 RID: 6352
		[Range(0f, 1f)]
		[SerializeField]
		private float recoilMultiplier = 1f;

		// Token: 0x040018D1 RID: 6353
		[SerializeField]
		internal float enemyReactionTimeFactor = 1f;

		// Token: 0x040018D2 RID: 6354
		[SerializeField]
		internal float enemyAttackTimeSpaceFactor = 1f;

		// Token: 0x040018D3 RID: 6355
		[SerializeField]
		internal float enemyAttackTimeFactor = 1f;
	}
}
