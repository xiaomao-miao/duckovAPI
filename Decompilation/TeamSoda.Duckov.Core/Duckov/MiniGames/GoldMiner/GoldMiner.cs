using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using Saves;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200028F RID: 655
	public class GoldMiner : MiniGameBehaviour
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x0004EA22 File Offset: 0x0004CC22
		public Hook Hook
		{
			get
			{
				return this.hook;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x0004EA2A File Offset: 0x0004CC2A
		public Bounds Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x0004EA32 File Offset: 0x0004CC32
		public int Money
		{
			get
			{
				if (this.run == null)
				{
					return 0;
				}
				return this.run.money;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x0004EA49 File Offset: 0x0004CC49
		public ReadOnlyCollection<GoldMinerArtifact> ArtifactPrefabs
		{
			get
			{
				if (this.artifactPrefabs_ReadOnly == null)
				{
					this.artifactPrefabs_ReadOnly = new ReadOnlyCollection<GoldMinerArtifact>(this.artifactPrefabs);
				}
				return this.artifactPrefabs_ReadOnly;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001549 RID: 5449 RVA: 0x0004EA6A File Offset: 0x0004CC6A
		// (set) Token: 0x0600154A RID: 5450 RVA: 0x0004EA76 File Offset: 0x0004CC76
		public static int HighLevel
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/GoldMiner/HighLevel");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/GoldMiner/HighLevel", value);
			}
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0004EA84 File Offset: 0x0004CC84
		private void Awake()
		{
			this.Hook.OnBeginRetrieve += this.OnHookBeginRetrieve;
			this.Hook.OnEndRetrieve += this.OnHookEndRetrieve;
			this.Hook.OnLaunch += this.OnHookLaunch;
			this.Hook.OnResolveTarget += this.OnHookResolveEntity;
			this.Hook.OnAttach += this.OnHookAttach;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0004EB04 File Offset: 0x0004CD04
		protected override void Start()
		{
			base.Start();
			this.hook.BeginSwing();
			this.Main().Forget();
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0004EB22 File Offset: 0x0004CD22
		internal bool PayMoney(int price)
		{
			if (this.run.money < price)
			{
				return false;
			}
			this.run.money -= price;
			return true;
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0004EB48 File Offset: 0x0004CD48
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x0004EB50 File Offset: 0x0004CD50
		public GoldMinerRunData run { get; private set; }

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06001550 RID: 5456 RVA: 0x0004EB5C File Offset: 0x0004CD5C
		// (remove) Token: 0x06001551 RID: 5457 RVA: 0x0004EB90 File Offset: 0x0004CD90
		public static event Action<int> OnLevelClear;

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0004EBC3 File Offset: 0x0004CDC3
		private bool ShouldQuit
		{
			get
			{
				return base.gameObject == null;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x0004EBD6 File Offset: 0x0004CDD6
		public float GlobalPriceFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0004EBE0 File Offset: 0x0004CDE0
		private UniTask Main()
		{
			GoldMiner.<Main>d__51 <Main>d__;
			<Main>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Main>d__.<>4__this = this;
			<Main>d__.<>1__state = -1;
			<Main>d__.<>t__builder.Start<GoldMiner.<Main>d__51>(ref <Main>d__);
			return <Main>d__.<>t__builder.Task;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0004EC24 File Offset: 0x0004CE24
		private UniTask DoTitleScreen()
		{
			GoldMiner.<DoTitleScreen>d__53 <DoTitleScreen>d__;
			<DoTitleScreen>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoTitleScreen>d__.<>4__this = this;
			<DoTitleScreen>d__.<>1__state = -1;
			<DoTitleScreen>d__.<>t__builder.Start<GoldMiner.<DoTitleScreen>d__53>(ref <DoTitleScreen>d__);
			return <DoTitleScreen>d__.<>t__builder.Task;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0004EC68 File Offset: 0x0004CE68
		private UniTask DoGameOver()
		{
			GoldMiner.<DoGameOver>d__55 <DoGameOver>d__;
			<DoGameOver>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoGameOver>d__.<>4__this = this;
			<DoGameOver>d__.<>1__state = -1;
			<DoGameOver>d__.<>t__builder.Start<GoldMiner.<DoGameOver>d__55>(ref <DoGameOver>d__);
			return <DoGameOver>d__.<>t__builder.Task;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0004ECAB File Offset: 0x0004CEAB
		public void Cleanup()
		{
			if (this.run != null)
			{
				this.run.Cleanup();
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0004ECC0 File Offset: 0x0004CEC0
		private void GenerateLevel()
		{
			GoldMiner.<>c__DisplayClass58_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			for (int i = 0; i < this.activeEntities.Count; i++)
			{
				GoldMinerEntity goldMinerEntity = this.activeEntities[i];
				if (!(goldMinerEntity == null))
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(goldMinerEntity.gameObject);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(goldMinerEntity.gameObject);
					}
				}
			}
			this.activeEntities.Clear();
			for (int j = 0; j < this.resolvedEntities.Count; j++)
			{
				GoldMinerEntity goldMinerEntity2 = this.activeEntities[j];
				if (!(goldMinerEntity2 == null))
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(goldMinerEntity2.gameObject);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(goldMinerEntity2.gameObject);
					}
				}
			}
			this.resolvedEntities.Clear();
			int seed = this.run.levelRandom.Next();
			CS$<>8__locals1.levelGenRandom = new System.Random(seed);
			int minValue = 10;
			int maxValue = 20;
			int num = CS$<>8__locals1.levelGenRandom.Next(minValue, maxValue);
			for (int k = 0; k < num; k++)
			{
				GoldMinerEntity random = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, 0f);
				this.<GenerateLevel>g__Generate|58_0(random, ref CS$<>8__locals1);
			}
			for (float num2 = this.run.extraRocks; num2 > 0f; num2 -= 1f)
			{
				if (num2 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num2)
				{
					GoldMinerEntity random2 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Rock), 0f);
					this.<GenerateLevel>g__Generate|58_0(random2, ref CS$<>8__locals1);
				}
			}
			for (float num3 = this.run.extraGold; num3 > 0f; num3 -= 1f)
			{
				if (num3 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num3)
				{
					GoldMinerEntity random3 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Gold), 0f);
					this.<GenerateLevel>g__Generate|58_0(random3, ref CS$<>8__locals1);
				}
			}
			for (float num4 = this.run.extraDiamond; num4 > 0f; num4 -= 1f)
			{
				if (num4 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num4)
				{
					GoldMinerEntity random4 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Diamond), 0f);
					this.<GenerateLevel>g__Generate|58_0(random4, ref CS$<>8__locals1);
				}
			}
			this.run.shopRandom = new System.Random(this.run.seed + CS$<>8__locals1.levelGenRandom.Next());
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0004EF94 File Offset: 0x0004D194
		private Vector3 NormalizedPosToLocalPos(Vector2 posNormalized)
		{
			float x = Mathf.Lerp(this.bounds.min.x, this.bounds.max.x, posNormalized.x);
			float y = Mathf.Lerp(this.bounds.min.y, this.bounds.max.y, posNormalized.y);
			return new Vector3(x, y, 0f);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0004F003 File Offset: 0x0004D203
		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = this.levelLayout.localToWorldMatrix;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.extents * 2f);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0004F03C File Offset: 0x0004D23C
		private UniTask Run(int seed = 0)
		{
			GoldMiner.<Run>d__61 <Run>d__;
			<Run>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Run>d__.<>4__this = this;
			<Run>d__.seed = seed;
			<Run>d__.<>1__state = -1;
			<Run>d__.<>t__builder.Start<GoldMiner.<Run>d__61>(ref <Run>d__);
			return <Run>d__.<>t__builder.Task;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0004F088 File Offset: 0x0004D288
		private UniTask<bool> SettleLevel()
		{
			GoldMiner.<SettleLevel>d__62 <SettleLevel>d__;
			<SettleLevel>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SettleLevel>d__.<>4__this = this;
			<SettleLevel>d__.<>1__state = -1;
			<SettleLevel>d__.<>t__builder.Start<GoldMiner.<SettleLevel>d__62>(ref <SettleLevel>d__);
			return <SettleLevel>d__.<>t__builder.Task;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0004F0CC File Offset: 0x0004D2CC
		private UniTask DoLevel()
		{
			GoldMiner.<DoLevel>d__65 <DoLevel>d__;
			<DoLevel>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoLevel>d__.<>4__this = this;
			<DoLevel>d__.<>1__state = -1;
			<DoLevel>d__.<>t__builder.Start<GoldMiner.<DoLevel>d__65>(ref <DoLevel>d__);
			return <DoLevel>d__.<>t__builder.Task;
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0004F10F File Offset: 0x0004D30F
		protected override void OnUpdate(float deltaTime)
		{
			if (this.levelPlaying)
			{
				this.UpdateLevelPlaying(deltaTime);
			}
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0004F120 File Offset: 0x0004D320
		private void UpdateLevelPlaying(float deltaTime)
		{
			Action<GoldMiner> action = this.onEarlyLevelPlayTick;
			if (action != null)
			{
				action(this);
			}
			this.Hook.SetParameters(this.run.GameSpeedFactor, this.run.emptySpeed.Value, this.run.strength.Value);
			this.Hook.Tick(deltaTime);
			Hook.HookStatus status = this.Hook.Status;
			if (status != Hook.HookStatus.Swinging)
			{
				if (status == Hook.HookStatus.Retrieving)
				{
					this.run.stamina -= deltaTime * this.run.staminaDrain.Value;
				}
			}
			else if (this.launchHook)
			{
				this.Hook.Launch();
			}
			Action<GoldMiner> action2 = this.onLateLevelPlayTick;
			if (action2 != null)
			{
				action2(this);
			}
			this.launchHook = false;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0004F1E9 File Offset: 0x0004D3E9
		public void LaunchHook()
		{
			this.launchHook = true;
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0004F1F4 File Offset: 0x0004D3F4
		private bool IsLevelOver()
		{
			this.activeEntities.RemoveAll((GoldMinerEntity e) => e == null);
			return this.activeEntities.Count <= 0 || (this.hook.Status == Hook.HookStatus.Swinging && this.run.stamina <= 0f) || (this.Hook.Status == Hook.HookStatus.Retrieving && this.run.stamina < -this.run.extraStamina.Value);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0004F290 File Offset: 0x0004D490
		private UniTask DoShop()
		{
			GoldMiner.<DoShop>d__71 <DoShop>d__;
			<DoShop>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoShop>d__.<>4__this = this;
			<DoShop>d__.<>1__state = -1;
			<DoShop>d__.<>t__builder.Start<GoldMiner.<DoShop>d__71>(ref <DoShop>d__);
			return <DoShop>d__.<>t__builder.Task;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0004F2D4 File Offset: 0x0004D4D4
		private void OnHookResolveEntity(Hook hook, GoldMinerEntity entity)
		{
			entity.NotifyResolved(this);
			entity.gameObject.SetActive(false);
			this.activeEntities.Remove(entity);
			this.resolvedEntities.Add(entity);
			if (this.run.IsRock(entity))
			{
				entity.Value = Mathf.CeilToInt((float)entity.Value * this.run.rockValueFactor.Value);
			}
			if (this.run.IsGold(entity))
			{
				entity.Value = Mathf.CeilToInt((float)entity.Value * this.run.goldValueFactor.Value);
			}
			this.popText.Pop(string.Format("${0}", entity.Value), hook.Axis.position);
			Action<GoldMiner, GoldMinerEntity> action = this.onResolveEntity;
			if (action != null)
			{
				action(this, entity);
			}
			Action<GoldMiner, GoldMinerEntity> action2 = this.onAfterResolveEntity;
			if (action2 == null)
			{
				return;
			}
			action2(this, entity);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0004F3BF File Offset: 0x0004D5BF
		private void OnHookBeginRetrieve(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookBeginRetrieve;
			if (action == null)
			{
				return;
			}
			action(this, hook);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0004F3D3 File Offset: 0x0004D5D3
		private void OnHookEndRetrieve(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookEndRetrieve;
			if (action != null)
			{
				action(this, hook);
			}
			if (this.run.StrengthPotionActivated)
			{
				this.run.DeactivateStrengthPotion();
			}
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0004F400 File Offset: 0x0004D600
		private void OnHookLaunch(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookLaunch;
			if (action != null)
			{
				action(this, hook);
			}
			if (this.run.EagleEyeActivated)
			{
				this.run.DeactivateEagleEye();
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0004F42D File Offset: 0x0004D62D
		private void OnHookAttach(Hook hook, GoldMinerEntity entity)
		{
			Action<GoldMiner, Hook, GoldMinerEntity> action = this.onHookAttach;
			if (action == null)
			{
				return;
			}
			action(this, hook, entity);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0004F442 File Offset: 0x0004D642
		public bool UseStrengthPotion()
		{
			if (this.run.strengthPotion <= 0)
			{
				return false;
			}
			if (this.run.StrengthPotionActivated)
			{
				return false;
			}
			this.run.strengthPotion--;
			this.run.ActivateStrengthPotion();
			return true;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0004F482 File Offset: 0x0004D682
		public bool UseEagleEyePotion()
		{
			if (this.run.eagleEyePotion <= 0)
			{
				return false;
			}
			if (this.run.EagleEyeActivated)
			{
				return false;
			}
			this.run.eagleEyePotion--;
			this.run.ActivateEagleEye();
			return true;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0004F4C4 File Offset: 0x0004D6C4
		public GoldMinerArtifact GetArtifactPrefab(string id)
		{
			return this.artifactPrefabs.Find((GoldMinerArtifact e) => e != null && e.ID == id);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0004F4F8 File Offset: 0x0004D6F8
		internal bool UseBomb()
		{
			if (this.run.bomb <= 0)
			{
				return false;
			}
			this.run.bomb--;
			UnityEngine.Object.Instantiate<Bomb>(this.bombPrefab, this.hook.Axis.transform.position, Quaternion.FromToRotation(Vector3.up, -this.hook.Axis.transform.up), base.transform);
			return true;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0004F574 File Offset: 0x0004D774
		internal void NotifyArtifactChange()
		{
			Action<GoldMiner> action = this.onArtifactChange;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0004F5B0 File Offset: 0x0004D7B0
		[CompilerGenerated]
		private void <GenerateLevel>g__Generate|58_0(GoldMinerEntity entityPrefab, ref GoldMiner.<>c__DisplayClass58_0 A_2)
		{
			if (entityPrefab == null)
			{
				return;
			}
			Vector2 posNormalized = new Vector2((float)A_2.levelGenRandom.NextDouble(), (float)A_2.levelGenRandom.NextDouble());
			GoldMinerEntity goldMinerEntity = UnityEngine.Object.Instantiate<GoldMinerEntity>(entityPrefab, this.levelLayout);
			Vector3 localPosition = this.NormalizedPosToLocalPos(posNormalized);
			Quaternion localRotation = Quaternion.AngleAxis((float)A_2.levelGenRandom.NextDouble() * 360f, Vector3.forward);
			goldMinerEntity.transform.localPosition = localPosition;
			goldMinerEntity.transform.localRotation = localRotation;
			goldMinerEntity.SetMaster(this);
			this.activeEntities.Add(goldMinerEntity);
		}

		// Token: 0x04000FAE RID: 4014
		[SerializeField]
		private Hook hook;

		// Token: 0x04000FAF RID: 4015
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x04000FB0 RID: 4016
		[SerializeField]
		private LevelSettlementUI settlementUI;

		// Token: 0x04000FB1 RID: 4017
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x04000FB2 RID: 4018
		[SerializeField]
		private GameObject gameoverScreen;

		// Token: 0x04000FB3 RID: 4019
		[SerializeField]
		private GoldMiner_PopText popText;

		// Token: 0x04000FB4 RID: 4020
		[SerializeField]
		private Transform levelLayout;

		// Token: 0x04000FB5 RID: 4021
		[SerializeField]
		private Bounds bounds;

		// Token: 0x04000FB6 RID: 4022
		[SerializeField]
		private Bomb bombPrefab;

		// Token: 0x04000FB7 RID: 4023
		[SerializeField]
		private RandomContainer<GoldMinerEntity> entities;

		// Token: 0x04000FB8 RID: 4024
		[SerializeField]
		private List<GoldMinerArtifact> artifactPrefabs = new List<GoldMinerArtifact>();

		// Token: 0x04000FB9 RID: 4025
		private ReadOnlyCollection<GoldMinerArtifact> artifactPrefabs_ReadOnly;

		// Token: 0x04000FBA RID: 4026
		public Action<GoldMiner> onLevelBegin;

		// Token: 0x04000FBB RID: 4027
		public Action<GoldMiner> onLevelEnd;

		// Token: 0x04000FBC RID: 4028
		public Action<GoldMiner> onShopBegin;

		// Token: 0x04000FBD RID: 4029
		public Action<GoldMiner> onShopEnd;

		// Token: 0x04000FBE RID: 4030
		public Action<GoldMiner> onEarlyLevelPlayTick;

		// Token: 0x04000FBF RID: 4031
		public Action<GoldMiner> onLateLevelPlayTick;

		// Token: 0x04000FC0 RID: 4032
		public Action<GoldMiner, Hook> onHookLaunch;

		// Token: 0x04000FC1 RID: 4033
		public Action<GoldMiner, Hook> onHookBeginRetrieve;

		// Token: 0x04000FC2 RID: 4034
		public Action<GoldMiner, Hook> onHookEndRetrieve;

		// Token: 0x04000FC3 RID: 4035
		public Action<GoldMiner, Hook, GoldMinerEntity> onHookAttach;

		// Token: 0x04000FC4 RID: 4036
		public Action<GoldMiner, GoldMinerEntity> onResolveEntity;

		// Token: 0x04000FC5 RID: 4037
		public Action<GoldMiner, GoldMinerEntity> onAfterResolveEntity;

		// Token: 0x04000FC6 RID: 4038
		public Action<GoldMiner> onArtifactChange;

		// Token: 0x04000FC7 RID: 4039
		private const string HighLevelSaveKey = "MiniGame/GoldMiner/HighLevel";

		// Token: 0x04000FCA RID: 4042
		private bool titleConfirmed;

		// Token: 0x04000FCB RID: 4043
		private bool gameOverConfirmed;

		// Token: 0x04000FCC RID: 4044
		public List<GoldMinerEntity> activeEntities = new List<GoldMinerEntity>();

		// Token: 0x04000FCD RID: 4045
		private bool levelPlaying;

		// Token: 0x04000FCE RID: 4046
		public List<GoldMinerEntity> resolvedEntities = new List<GoldMinerEntity>();

		// Token: 0x04000FCF RID: 4047
		private bool launchHook;
	}
}
