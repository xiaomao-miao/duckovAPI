using System;
using System.Collections.Generic;
using System.Linq;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200028E RID: 654
	[Serializable]
	public class GoldMinerRunData
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0004E45D File Offset: 0x0004C65D
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x0004E465 File Offset: 0x0004C665
		public int seed { get; private set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0004E46E File Offset: 0x0004C66E
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x0004E476 File Offset: 0x0004C676
		public System.Random shopRandom { get; set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x0004E47F File Offset: 0x0004C67F
		// (set) Token: 0x0600152C RID: 5420 RVA: 0x0004E487 File Offset: 0x0004C687
		public System.Random levelRandom { get; private set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x0004E490 File Offset: 0x0004C690
		public float GameSpeedFactor
		{
			get
			{
				return this.gameSpeedFactor.Value;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x0004E49D File Offset: 0x0004C69D
		// (set) Token: 0x0600152F RID: 5423 RVA: 0x0004E4A5 File Offset: 0x0004C6A5
		public float stamina { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x0004E4AE File Offset: 0x0004C6AE
		// (set) Token: 0x06001531 RID: 5425 RVA: 0x0004E4B6 File Offset: 0x0004C6B6
		public bool gameOver { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x0004E4BF File Offset: 0x0004C6BF
		// (set) Token: 0x06001533 RID: 5427 RVA: 0x0004E4C7 File Offset: 0x0004C6C7
		public int level { get; set; }

		// Token: 0x06001534 RID: 5428 RVA: 0x0004E4D0 File Offset: 0x0004C6D0
		public GoldMinerArtifact AttachArtifactFromPrefab(GoldMinerArtifact prefab)
		{
			if (prefab == null)
			{
				return null;
			}
			GoldMinerArtifact goldMinerArtifact = UnityEngine.Object.Instantiate<GoldMinerArtifact>(prefab, this.master.transform);
			this.AttachArtifact(goldMinerArtifact);
			return goldMinerArtifact;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0004E504 File Offset: 0x0004C704
		private void AttachArtifact(GoldMinerArtifact artifact)
		{
			if (this.artifactCount.ContainsKey(artifact.ID))
			{
				Dictionary<string, int> dictionary = this.artifactCount;
				string id = artifact.ID;
				dictionary[id]++;
			}
			else
			{
				this.artifactCount[artifact.ID] = 1;
			}
			this.artifacts.Add(artifact);
			artifact.Attach(this.master);
			this.master.NotifyArtifactChange();
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0004E57C File Offset: 0x0004C77C
		public bool DetachArtifact(GoldMinerArtifact artifact)
		{
			bool result = this.artifacts.Remove(artifact);
			artifact.Detatch(this.master);
			if (this.artifactCount.ContainsKey(artifact.ID))
			{
				Dictionary<string, int> dictionary = this.artifactCount;
				string id = artifact.ID;
				dictionary[id]--;
			}
			else
			{
				Debug.LogError("Artifact counter error.", this.master);
			}
			this.master.NotifyArtifactChange();
			return result;
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0004E5F0 File Offset: 0x0004C7F0
		public int GetArtifactCount(string id)
		{
			int result;
			if (this.artifactCount.TryGetValue(id, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0004E610 File Offset: 0x0004C810
		public GoldMinerRunData(GoldMiner master, int seed = 0)
		{
			this.master = master;
			if (seed == 0)
			{
				seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
			this.seed = seed;
			this.levelRandom = new System.Random(seed);
			this.strengthPotionModifier = new Modifier(ModifierType.Add, 100f, this);
			this.eagleEyeModifier = new Modifier(ModifierType.PercentageMultiply, -0.5f, this);
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x0004E81F File Offset: 0x0004CA1F
		// (set) Token: 0x0600153A RID: 5434 RVA: 0x0004E827 File Offset: 0x0004CA27
		public bool StrengthPotionActivated { get; private set; }

		// Token: 0x0600153B RID: 5435 RVA: 0x0004E830 File Offset: 0x0004CA30
		public void ActivateStrengthPotion()
		{
			if (this.StrengthPotionActivated)
			{
				return;
			}
			this.strength.AddModifier(this.strengthPotionModifier);
			this.StrengthPotionActivated = true;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0004E853 File Offset: 0x0004CA53
		public void DeactivateStrengthPotion()
		{
			this.strength.RemoveModifier(this.strengthPotionModifier);
			this.StrengthPotionActivated = false;
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x0004E86E File Offset: 0x0004CA6E
		// (set) Token: 0x0600153E RID: 5438 RVA: 0x0004E876 File Offset: 0x0004CA76
		public bool EagleEyeActivated { get; private set; }

		// Token: 0x0600153F RID: 5439 RVA: 0x0004E87F File Offset: 0x0004CA7F
		public void ActivateEagleEye()
		{
			if (this.EagleEyeActivated)
			{
				return;
			}
			this.gameSpeedFactor.AddModifier(this.eagleEyeModifier);
			this.EagleEyeActivated = true;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0004E8A2 File Offset: 0x0004CAA2
		public void DeactivateEagleEye()
		{
			this.gameSpeedFactor.RemoveModifier(this.eagleEyeModifier);
			this.EagleEyeActivated = false;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0004E8C0 File Offset: 0x0004CAC0
		internal void Cleanup()
		{
			foreach (GoldMinerArtifact goldMinerArtifact in this.artifacts)
			{
				if (!(goldMinerArtifact == null))
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(goldMinerArtifact.gameObject);
					}
					else
					{
						UnityEngine.Object.Destroy(goldMinerArtifact.gameObject);
					}
				}
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0004E934 File Offset: 0x0004CB34
		public bool IsGold(GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			using (List<Func<GoldMinerEntity, bool>>.Enumerator enumerator = this.isGoldPredicators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current(entity))
					{
						return true;
					}
				}
			}
			return entity.tags.Contains(GoldMinerEntity.Tag.Gold);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0004E9A4 File Offset: 0x0004CBA4
		public bool IsRock(GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			using (List<Func<GoldMinerEntity, bool>>.Enumerator enumerator = this.isGoldPredicators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current(entity))
					{
						return true;
					}
				}
			}
			return entity.tags.Contains(GoldMinerEntity.Tag.Rock);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0004EA14 File Offset: 0x0004CC14
		internal bool IsPig(GoldMinerEntity entity)
		{
			return entity.tags.Contains(GoldMinerEntity.Tag.Pig);
		}

		// Token: 0x04000F7F RID: 3967
		public readonly GoldMiner master;

		// Token: 0x04000F83 RID: 3971
		public int money;

		// Token: 0x04000F84 RID: 3972
		public int bomb;

		// Token: 0x04000F85 RID: 3973
		public int strengthPotion;

		// Token: 0x04000F86 RID: 3974
		public int eagleEyePotion;

		// Token: 0x04000F87 RID: 3975
		public int shopTicket;

		// Token: 0x04000F88 RID: 3976
		public const int shopDefaultItemAmount = 3;

		// Token: 0x04000F89 RID: 3977
		public const int shopMaxItemAmount = 3;

		// Token: 0x04000F8A RID: 3978
		public int shopCapacity = 3;

		// Token: 0x04000F8B RID: 3979
		public float levelScoreFactor;

		// Token: 0x04000F8C RID: 3980
		public Stat maxStamina = new Stat("maxStamina", 15f, false);

		// Token: 0x04000F8D RID: 3981
		public Stat extraStamina = new Stat("extraStamina", 2f, false);

		// Token: 0x04000F8E RID: 3982
		public Stat staminaDrain = new Stat("staminaDrain", 1f, false);

		// Token: 0x04000F8F RID: 3983
		public Stat gameSpeedFactor = new Stat("gameSpeedFactor", 1f, false);

		// Token: 0x04000F90 RID: 3984
		public Stat emptySpeed = new Stat("emptySpeed", 300f, false);

		// Token: 0x04000F91 RID: 3985
		public Stat strength = new Stat("strength", 0f, false);

		// Token: 0x04000F92 RID: 3986
		public Stat scoreFactorBase = new Stat("scoreFactor", 1f, false);

		// Token: 0x04000F93 RID: 3987
		public Stat rockValueFactor = new Stat("rockValueFactor", 1f, false);

		// Token: 0x04000F94 RID: 3988
		public Stat goldValueFactor = new Stat("goldValueFactor", 1f, false);

		// Token: 0x04000F95 RID: 3989
		public Stat charm = new Stat("charm", 1f, false);

		// Token: 0x04000F96 RID: 3990
		public Stat shopRefreshPrice = new Stat("shopRefreshPrice", 100f, false);

		// Token: 0x04000F97 RID: 3991
		public Stat shopRefreshPriceIncrement = new Stat("shopRefreshPriceIncrement", 50f, false);

		// Token: 0x04000F98 RID: 3992
		public Stat shopRefreshChances = new Stat("shopRefreshChances", 2f, false);

		// Token: 0x04000F99 RID: 3993
		public Stat shopPriceCut = new Stat("shopPriceCut", 0.7f, false);

		// Token: 0x04000F9A RID: 3994
		public Stat defuse = new Stat("defuse", 0f, false);

		// Token: 0x04000F9B RID: 3995
		public float extraRocks;

		// Token: 0x04000F9C RID: 3996
		public float extraGold;

		// Token: 0x04000F9D RID: 3997
		public float extraDiamond;

		// Token: 0x04000F9E RID: 3998
		public List<GoldMinerArtifact> artifacts = new List<GoldMinerArtifact>();

		// Token: 0x04000FA2 RID: 4002
		private Dictionary<string, int> artifactCount = new Dictionary<string, int>();

		// Token: 0x04000FA3 RID: 4003
		private Modifier strengthPotionModifier;

		// Token: 0x04000FA5 RID: 4005
		private Modifier eagleEyeModifier;

		// Token: 0x04000FA6 RID: 4006
		internal int targetScore = 100;

		// Token: 0x04000FA8 RID: 4008
		public List<Func<GoldMinerEntity, bool>> isGoldPredicators = new List<Func<GoldMinerEntity, bool>>();

		// Token: 0x04000FA9 RID: 4009
		public List<Func<GoldMinerEntity, bool>> isRockPredicators = new List<Func<GoldMinerEntity, bool>>();

		// Token: 0x04000FAA RID: 4010
		public List<Func<float>> additionalFactorFuncs = new List<Func<float>>();

		// Token: 0x04000FAB RID: 4011
		public List<Func<int, int>> settleValueProcessor = new List<Func<int, int>>();

		// Token: 0x04000FAC RID: 4012
		public List<Func<bool>> forceLevelSuccessFuncs = new List<Func<bool>>();

		// Token: 0x04000FAD RID: 4013
		internal int minMoneySum;
	}
}
