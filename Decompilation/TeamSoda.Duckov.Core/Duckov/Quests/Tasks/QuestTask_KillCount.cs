using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000352 RID: 850
	public class QuestTask_KillCount : Task
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x00069417 File Offset: 0x00067617
		// (set) Token: 0x06001D7D RID: 7549 RVA: 0x0006941E File Offset: 0x0006761E
		[LocalizationKey("TasksAndRewards")]
		private string defaultEnemyNameKey
		{
			get
			{
				return "Task_Desc_AnyEnemy";
			}
			set
			{
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x00069420 File Offset: 0x00067620
		// (set) Token: 0x06001D7F RID: 7551 RVA: 0x00069427 File Offset: 0x00067627
		[LocalizationKey("TasksAndRewards")]
		private string defaultWeaponNameKey
		{
			get
			{
				return "Task_Desc_AnyWeapon";
			}
			set
			{
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x0006942C File Offset: 0x0006762C
		private string weaponName
		{
			get
			{
				if (this.withWeapon)
				{
					return ItemAssetsCollection.GetMetaData(this.weaponTypeID).DisplayName;
				}
				return this.defaultWeaponNameKey.ToPlainText();
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x00069460 File Offset: 0x00067660
		private string enemyName
		{
			get
			{
				if (this.requireEnemyType == null)
				{
					return this.defaultEnemyNameKey.ToPlainText();
				}
				return this.requireEnemyType.DisplayName;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x00069487 File Offset: 0x00067687
		// (set) Token: 0x06001D83 RID: 7555 RVA: 0x0006948E File Offset: 0x0006768E
		[LocalizationKey("TasksAndRewards")]
		private string descriptionFormatKey
		{
			get
			{
				return "Task_KillCount";
			}
			set
			{
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x00069490 File Offset: 0x00067690
		// (set) Token: 0x06001D85 RID: 7557 RVA: 0x00069497 File Offset: 0x00067697
		[LocalizationKey("TasksAndRewards")]
		private string withWeaponDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_WithWeapon";
			}
			set
			{
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x00069499 File Offset: 0x00067699
		// (set) Token: 0x06001D87 RID: 7559 RVA: 0x000694A0 File Offset: 0x000676A0
		[LocalizationKey("TasksAndRewards")]
		private string requireSceneDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_RequireScene";
			}
			set
			{
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x000694A2 File Offset: 0x000676A2
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x000694A9 File Offset: 0x000676A9
		[LocalizationKey("TasksAndRewards")]
		private string RequireHeadShotDescriptionKey
		{
			get
			{
				return "Task_Desc_RequireHeadShot";
			}
			set
			{
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x000694AB File Offset: 0x000676AB
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x000694B2 File Offset: 0x000676B2
		[LocalizationKey("TasksAndRewards")]
		private string WithoutHeadShotDescriptionKey
		{
			get
			{
				return "Task_Desc_WithoutHeadShot";
			}
			set
			{
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x000694B4 File Offset: 0x000676B4
		// (set) Token: 0x06001D8D RID: 7565 RVA: 0x000694BB File Offset: 0x000676BB
		[LocalizationKey("TasksAndRewards")]
		private string RequireBuffDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_WithBuff";
			}
			set
			{
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x000694BD File Offset: 0x000676BD
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x000694CC File Offset: 0x000676CC
		public override string[] ExtraDescriptsions
		{
			get
			{
				List<string> list = new List<string>();
				if (this.withWeapon)
				{
					list.Add(this.WithWeaponDescription);
				}
				if (!string.IsNullOrEmpty(this.requireSceneID))
				{
					list.Add(this.RequireSceneDescription);
				}
				if (this.requireHeadShot)
				{
					list.Add(this.RequireHeadShotDescription);
				}
				if (this.withoutHeadShot)
				{
					list.Add(this.WithoutHeadShotDescription);
				}
				if (this.requireBuff)
				{
					list.Add(this.RequireBuffDescription);
				}
				return list.ToArray();
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x0006954E File Offset: 0x0006774E
		private string WithWeaponDescription
		{
			get
			{
				return this.withWeaponDescriptionFormatKey.ToPlainText().Format(new
				{
					this.weaponName
				});
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x0006956B File Offset: 0x0006776B
		private string RequireSceneDescription
		{
			get
			{
				return this.requireSceneDescriptionFormatKey.ToPlainText().Format(new
				{
					this.requireSceneName
				});
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x00069588 File Offset: 0x00067788
		private string RequireHeadShotDescription
		{
			get
			{
				return this.RequireHeadShotDescriptionKey.ToPlainText();
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001D93 RID: 7571 RVA: 0x00069595 File Offset: 0x00067795
		private string WithoutHeadShotDescription
		{
			get
			{
				return this.WithoutHeadShotDescriptionKey.ToPlainText();
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x000695A4 File Offset: 0x000677A4
		private string RequireBuffDescription
		{
			get
			{
				string buffDisplayName = GameplayDataSettings.Buffs.GetBuffDisplayName(this.requireBuffID);
				return this.RequireBuffDescriptionFormatKey.ToPlainText().Format(new
				{
					buffName = buffDisplayName
				});
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x000695D8 File Offset: 0x000677D8
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.weaponName,
					this.enemyName,
					this.requireAmount,
					this.amount,
					this.requireSceneName
				});
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001D96 RID: 7574 RVA: 0x00069608 File Offset: 0x00067808
		public SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001D97 RID: 7575 RVA: 0x00069618 File Offset: 0x00067818
		public SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001D98 RID: 7576 RVA: 0x00069637 File Offset: 0x00067837
		public string requireSceneName
		{
			get
			{
				if (string.IsNullOrEmpty(this.requireSceneID))
				{
					return "Task_Desc_AnyScene".ToPlainText();
				}
				return this.RequireSceneInfo.DisplayName;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001D99 RID: 7577 RVA: 0x0006965C File Offset: 0x0006785C
		public bool SceneRequirementSatisfied
		{
			get
			{
				if (string.IsNullOrEmpty(this.requireSceneID))
				{
					return true;
				}
				SceneReference requireScene = this.RequireScene;
				return requireScene == null || requireScene.UnsafeReason == SceneReferenceUnsafeReason.Empty || requireScene.UnsafeReason != SceneReferenceUnsafeReason.None || requireScene.LoadedScene.isLoaded;
			}
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x000696A7 File Offset: 0x000678A7
		private void OnEnable()
		{
			Health.OnDead += this.Health_OnDead;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x000696CB File Offset: 0x000678CB
		private void OnDisable()
		{
			Health.OnDead -= this.Health_OnDead;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000696EF File Offset: 0x000678EF
		private void OnLevelInitialized()
		{
			if (this.resetOnLevelInitialized)
			{
				this.amount = 0;
			}
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x00069700 File Offset: 0x00067900
		private void Health_OnDead(Health health, DamageInfo info)
		{
			if (health.team == Teams.player)
			{
				return;
			}
			bool flag = false;
			CharacterMainControl fromCharacter = info.fromCharacter;
			if (fromCharacter != null && info.fromCharacter.IsMainCharacter())
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (this.withWeapon && info.fromWeaponItemID != this.weaponTypeID)
			{
				return;
			}
			if (!this.SceneRequirementSatisfied)
			{
				return;
			}
			if (this.requireHeadShot && info.crit <= 0)
			{
				return;
			}
			if (this.withoutHeadShot && info.crit > 0)
			{
				return;
			}
			if (this.requireBuff && !fromCharacter.HasBuff(this.requireBuffID))
			{
				return;
			}
			if (this.requireEnemyType != null)
			{
				CharacterMainControl characterMainControl = health.TryGetCharacter();
				if (characterMainControl == null)
				{
					return;
				}
				CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
				if (characterPreset == null)
				{
					return;
				}
				if (characterPreset.nameKey != this.requireEnemyType.nameKey)
				{
					return;
				}
			}
			this.AddCount();
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000697E5 File Offset: 0x000679E5
		private void AddCount()
		{
			if (this.amount < this.requireAmount)
			{
				this.amount++;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00069809 File Offset: 0x00067A09
		public override object GenerateSaveData()
		{
			return this.amount;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x00069816 File Offset: 0x00067A16
		protected override bool CheckFinished()
		{
			return this.amount >= this.requireAmount;
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0006982C File Offset: 0x00067A2C
		public override void SetupSaveData(object data)
		{
			if (data is int)
			{
				int num = (int)data;
				this.amount = num;
			}
		}

		// Token: 0x0400146D RID: 5229
		[SerializeField]
		private int requireAmount = 1;

		// Token: 0x0400146E RID: 5230
		[SerializeField]
		private bool resetOnLevelInitialized;

		// Token: 0x0400146F RID: 5231
		[SerializeField]
		private int amount;

		// Token: 0x04001470 RID: 5232
		[SerializeField]
		private bool withWeapon;

		// Token: 0x04001471 RID: 5233
		[SerializeField]
		[ItemTypeID]
		private int weaponTypeID;

		// Token: 0x04001472 RID: 5234
		[SerializeField]
		private bool requireHeadShot;

		// Token: 0x04001473 RID: 5235
		[SerializeField]
		private bool withoutHeadShot;

		// Token: 0x04001474 RID: 5236
		[SerializeField]
		private bool requireBuff;

		// Token: 0x04001475 RID: 5237
		[SerializeField]
		private int requireBuffID;

		// Token: 0x04001476 RID: 5238
		[SerializeField]
		private CharacterRandomPreset requireEnemyType;

		// Token: 0x04001477 RID: 5239
		[SceneID]
		[SerializeField]
		private string requireSceneID;
	}
}
