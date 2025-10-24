using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Bilibili.BDS;
using Duckov;
using Duckov.Buffs;
using Duckov.Buildings;
using Duckov.Economy;
using Duckov.MasterKeys;
using Duckov.PerkTrees;
using Duckov.Quests;
using Duckov.Rules;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using Saves;
using Steamworks;
using UnityEngine;

namespace EventReports
{
	// Token: 0x02000228 RID: 552
	public class BDSManager : MonoBehaviour
	{
		// Token: 0x060010DD RID: 4317 RVA: 0x000413FC File Offset: 0x0003F5FC
		private void Awake()
		{
			if (PlatformInfo.Platform == Platform.Steam)
			{
				if (SteamManager.Initialized && SteamUtils.IsSteamChinaLauncher())
				{
				}
			}
			else
			{
				string.Format("{0}", PlatformInfo.Platform);
			}
			Debug.Log("Player Info:\n" + BDSManager.PlayerInfo.GetCurrent().ToJson());
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00041452 File Offset: 0x0003F652
		private void Start()
		{
			this.OnGameStarted();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0004145A File Offset: 0x0003F65A
		private void OnDestroy()
		{
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0004145C File Offset: 0x0003F65C
		private float TimeSinceLastHeartbeat
		{
			get
			{
				return Time.unscaledTime - this.lastTimeHeartbeat;
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0004146A File Offset: 0x0003F66A
		private void Update()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00041472 File Offset: 0x0003F672
		private void UpdateHeartbeat()
		{
			if (this.TimeSinceLastHeartbeat > 60f)
			{
				this.ReportCustomEvent(BDSManager.EventName.heartbeat, "");
				this.lastTimeHeartbeat = Time.unscaledTime;
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0004149C File Offset: 0x0003F69C
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			SavesSystem.OnSaveDeleted += this.OnSaveDeleted;
			RaidUtilities.OnNewRaid = (Action<RaidUtilities.RaidInfo>)Delegate.Combine(RaidUtilities.OnNewRaid, new Action<RaidUtilities.RaidInfo>(this.OnNewRaid));
			RaidUtilities.OnRaidEnd = (Action<RaidUtilities.RaidInfo>)Delegate.Combine(RaidUtilities.OnRaidEnd, new Action<RaidUtilities.RaidInfo>(this.OnRaidEnd));
			SceneLoader.onStartedLoadingScene += this.OnSceneLoadingStart;
			SceneLoader.onFinishedLoadingScene += this.OnSceneLoadingFinish;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			LevelManager.OnEvacuated += this.OnEvacuated;
			LevelManager.OnMainCharacterDead += this.OnMainCharacterDead;
			Quest.onQuestActivated += this.OnQuestActivated;
			Quest.onQuestCompleted += this.OnQuestCompleted;
			EconomyManager.OnCostPaid += this.OnCostPaid;
			EconomyManager.OnMoneyPaid += this.OnMoneyPaid;
			ItemUtilities.OnItemSentToPlayerInventory += this.OnItemSentToPlayerInventory;
			ItemUtilities.OnItemSentToPlayerStorage += this.OnItemSentToPlayerStorage;
			StockShop.OnItemPurchased += this.OnItemPurchased;
			CraftingManager.OnItemCrafted = (Action<CraftingFormula, Item>)Delegate.Combine(CraftingManager.OnItemCrafted, new Action<CraftingFormula, Item>(this.OnItemCrafted));
			CraftingManager.OnFormulaUnlocked = (Action<string>)Delegate.Combine(CraftingManager.OnFormulaUnlocked, new Action<string>(this.OnFormulaUnlocked));
			Health.OnDead += this.OnHealthDead;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Combine(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			BuildingManager.OnBuildingBuiltComplex += this.OnBuildingBuilt;
			BuildingManager.OnBuildingDestroyedComplex += this.OnBuildingDestroyed;
			Perk.OnPerkUnlockConfirmed += this.OnPerkUnlockConfirmed;
			MasterKeysManager.OnMasterKeyUnlocked += this.OnMasterKeyUnlocked;
			CharacterMainControl.OnMainCharacterSlotContentChangedEvent = (Action<CharacterMainControl, Slot>)Delegate.Combine(CharacterMainControl.OnMainCharacterSlotContentChangedEvent, new Action<CharacterMainControl, Slot>(this.OnMainCharacterSlotContentChanged));
			StockShop.OnItemSoldByPlayer += this.OnItemSold;
			Reward.OnRewardClaimed += this.OnRewardClaimed;
			UsageUtilities.OnItemUsedStaticEvent += this.OnItemUsed;
			InteractableBase.OnInteractStartStaticEvent += this.OnInteractStart;
			LevelManager.OnNewGameReport += this.OnNewGameReport;
			Interact_CustomFace.OnCustomFaceStartEvent += this.OnCustomFaceStart;
			Interact_CustomFace.OnCustomFaceFinishedEvent += this.OnCustomFaceFinish;
			CheatMode.OnCheatModeStatusChanged += this.OnCheatModeStatusChanged;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0004172C File Offset: 0x0003F92C
		private void UnregisterEvents()
		{
			SavesSystem.OnSaveDeleted -= this.OnSaveDeleted;
			RaidUtilities.OnNewRaid = (Action<RaidUtilities.RaidInfo>)Delegate.Remove(RaidUtilities.OnNewRaid, new Action<RaidUtilities.RaidInfo>(this.OnNewRaid));
			RaidUtilities.OnRaidEnd = (Action<RaidUtilities.RaidInfo>)Delegate.Remove(RaidUtilities.OnRaidEnd, new Action<RaidUtilities.RaidInfo>(this.OnRaidEnd));
			SceneLoader.onStartedLoadingScene -= this.OnSceneLoadingStart;
			SceneLoader.onFinishedLoadingScene -= this.OnSceneLoadingFinish;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			LevelManager.OnEvacuated -= this.OnEvacuated;
			LevelManager.OnMainCharacterDead -= this.OnMainCharacterDead;
			Quest.onQuestActivated -= this.OnQuestActivated;
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			EconomyManager.OnCostPaid -= this.OnCostPaid;
			EconomyManager.OnMoneyPaid -= this.OnMoneyPaid;
			ItemUtilities.OnItemSentToPlayerInventory -= this.OnItemSentToPlayerInventory;
			ItemUtilities.OnItemSentToPlayerStorage -= this.OnItemSentToPlayerStorage;
			StockShop.OnItemPurchased -= this.OnItemPurchased;
			CraftingManager.OnItemCrafted = (Action<CraftingFormula, Item>)Delegate.Remove(CraftingManager.OnItemCrafted, new Action<CraftingFormula, Item>(this.OnItemCrafted));
			CraftingManager.OnFormulaUnlocked = (Action<string>)Delegate.Remove(CraftingManager.OnFormulaUnlocked, new Action<string>(this.OnFormulaUnlocked));
			Health.OnDead -= this.OnHealthDead;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Remove(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			BuildingManager.OnBuildingBuiltComplex -= this.OnBuildingBuilt;
			BuildingManager.OnBuildingDestroyedComplex -= this.OnBuildingDestroyed;
			Perk.OnPerkUnlockConfirmed -= this.OnPerkUnlockConfirmed;
			MasterKeysManager.OnMasterKeyUnlocked -= this.OnMasterKeyUnlocked;
			CharacterMainControl.OnMainCharacterSlotContentChangedEvent = (Action<CharacterMainControl, Slot>)Delegate.Remove(CharacterMainControl.OnMainCharacterSlotContentChangedEvent, new Action<CharacterMainControl, Slot>(this.OnMainCharacterSlotContentChanged));
			StockShop.OnItemSoldByPlayer -= this.OnItemSold;
			Reward.OnRewardClaimed -= this.OnRewardClaimed;
			UsageUtilities.OnItemUsedStaticEvent -= this.OnItemUsed;
			InteractableBase.OnInteractStartStaticEvent -= this.OnInteractStart;
			LevelManager.OnNewGameReport -= this.OnNewGameReport;
			Interact_CustomFace.OnCustomFaceStartEvent -= this.OnCustomFaceStart;
			Interact_CustomFace.OnCustomFaceFinishedEvent -= this.OnCustomFaceFinish;
			CheatMode.OnCheatModeStatusChanged -= this.OnCheatModeStatusChanged;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000419B4 File Offset: 0x0003FBB4
		private void OnCheatModeStatusChanged(bool value)
		{
			this.ReportCustomEvent<BDSManager.CheatModeStatusChangeContext>(BDSManager.EventName.cheat_mode_changed, new BDSManager.CheatModeStatusChangeContext
			{
				cheatModeActive = value
			});
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x000419DA File Offset: 0x0003FBDA
		private void OnCustomFaceFinish()
		{
			this.ReportCustomEvent(BDSManager.EventName.face_customize_finish, "");
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x000419E9 File Offset: 0x0003FBE9
		private void OnCustomFaceStart()
		{
			this.ReportCustomEvent(BDSManager.EventName.face_customize_begin, "");
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x000419F8 File Offset: 0x0003FBF8
		private void OnNewGameReport()
		{
			this.ReportCustomEvent(BDSManager.EventName.begin_new_game, "");
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00041A08 File Offset: 0x0003FC08
		private void OnInteractStart(InteractableBase target)
		{
			if (target == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.InteractEventContext>(BDSManager.EventName.interact_start, new BDSManager.InteractEventContext
			{
				interactGameObjectName = target.name,
				typeName = target.GetType().Name
			});
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00041A50 File Offset: 0x0003FC50
		private void OnItemUsed(Item item)
		{
			this.ReportCustomEvent<BDSManager.ItemUseEventContext>(BDSManager.EventName.item_use, new BDSManager.ItemUseEventContext
			{
				itemTypeID = item.TypeID
			});
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00041A7C File Offset: 0x0003FC7C
		private void OnRewardClaimed(Reward reward)
		{
			int questID = (reward.Master != null) ? reward.Master.ID : -1;
			this.ReportCustomEvent<BDSManager.RewardClaimEventContext>(BDSManager.EventName.reward_claimed, new BDSManager.RewardClaimEventContext
			{
				questID = questID,
				rewardID = reward.ID
			});
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00041ACC File Offset: 0x0003FCCC
		private void OnItemSold(StockShop shop, Item item, int price)
		{
			if (item == null)
			{
				return;
			}
			string stockShopID = (shop != null) ? shop.MerchantID : null;
			this.ReportCustomEvent<BDSManager.ItemSoldEventContext>(BDSManager.EventName.item_sold, new BDSManager.ItemSoldEventContext
			{
				stockShopID = stockShopID,
				itemID = item.TypeID,
				price = price
			});
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00041B20 File Offset: 0x0003FD20
		private void OnMainCharacterSlotContentChanged(CharacterMainControl control, Slot slot)
		{
			if (control == null || slot == null)
			{
				return;
			}
			if (slot.Content == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.EquipEventContext>(BDSManager.EventName.role_equip, new BDSManager.EquipEventContext
			{
				slotKey = slot.Key,
				contentItemTypeID = slot.Content.TypeID
			});
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00041B7C File Offset: 0x0003FD7C
		private void OnMasterKeyUnlocked(int id)
		{
			this.ReportCustomEvent<BDSManager.MasterKeyUnlockContext>(BDSManager.EventName.masterkey_unlocked, new BDSManager.MasterKeyUnlockContext
			{
				keyID = id
			});
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00041BA4 File Offset: 0x0003FDA4
		private void OnPerkUnlockConfirmed(Perk perk)
		{
			if (perk == null)
			{
				return;
			}
			BDSManager.EventName eventName = BDSManager.EventName.perk_unlocked;
			BDSManager.PerkInfo customParameters = default(BDSManager.PerkInfo);
			PerkTree master = perk.Master;
			customParameters.perkTreeID = ((master != null) ? master.ID : null);
			customParameters.perkName = perk.name;
			this.ReportCustomEvent<BDSManager.PerkInfo>(eventName, customParameters);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00041BF4 File Offset: 0x0003FDF4
		private void OnBuildingBuilt(int guid, BuildingInfo info)
		{
			this.ReportCustomEvent<BDSManager.BuildingEventContext>(BDSManager.EventName.building_built, new BDSManager.BuildingEventContext
			{
				buildingID = info.id
			});
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00041C20 File Offset: 0x0003FE20
		private void OnBuildingDestroyed(int guid, BuildingInfo info)
		{
			this.ReportCustomEvent<BDSManager.BuildingEventContext>(BDSManager.EventName.building_destroyed, new BDSManager.BuildingEventContext
			{
				buildingID = info.id
			});
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00041C4B File Offset: 0x0003FE4B
		private void OnLevelChanged(int from, int to)
		{
			this.ReportCustomEvent<BDSManager.LevelChangedEventContext>(BDSManager.EventName.role_level_changed, new BDSManager.LevelChangedEventContext(from, to));
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00041C5C File Offset: 0x0003FE5C
		private void OnHealthDead(Health health, DamageInfo info)
		{
			if (health == null)
			{
				return;
			}
			Teams team = health.team;
			bool flag = false;
			if (info.fromCharacter != null && info.fromCharacter.IsMainCharacter())
			{
				flag = true;
			}
			if (flag)
			{
				this.ReportCustomEvent<BDSManager.EnemyKillInfo>(BDSManager.EventName.enemy_kill, new BDSManager.EnemyKillInfo
				{
					enemyPresetName = BDSManager.<OnHealthDead>g__GetPresetName|36_0(health),
					damageInfo = info
				});
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00041CC2 File Offset: 0x0003FEC2
		private void OnFormulaUnlocked(string formulaID)
		{
			this.ReportCustomEvent(BDSManager.EventName.craft_formula_unlock, StrJson.Create(new string[]
			{
				"id",
				formulaID
			}));
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00041CE3 File Offset: 0x0003FEE3
		private void OnItemCrafted(CraftingFormula formula, Item item)
		{
			this.ReportCustomEvent<CraftingFormula>(BDSManager.EventName.craft_craft, formula);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00041CF0 File Offset: 0x0003FEF0
		private void OnItemPurchased(StockShop shop, Item item)
		{
			if (shop == null || item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.PurchaseInfo>(BDSManager.EventName.shop_purchased, new BDSManager.PurchaseInfo
			{
				shopID = shop.MerchantID,
				itemTypeID = item.TypeID,
				itemAmount = item.StackCount
			});
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00041D48 File Offset: 0x0003FF48
		private void OnItemSentToPlayerStorage(Item item)
		{
			if (item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.ItemInfo>(BDSManager.EventName.item_to_storage, new BDSManager.ItemInfo
			{
				itemId = item.TypeID,
				amount = item.StackCount
			});
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00041D8C File Offset: 0x0003FF8C
		private void OnItemSentToPlayerInventory(Item item)
		{
			if (item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.ItemInfo>(BDSManager.EventName.item_to_inventory, new BDSManager.ItemInfo
			{
				itemId = item.TypeID,
				amount = item.StackCount
			});
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00041DD0 File Offset: 0x0003FFD0
		private void OnMoneyPaid(long money)
		{
			this.ReportCustomEvent<Cost>(BDSManager.EventName.pay_money, new Cost
			{
				money = money,
				items = new Cost.ItemEntry[0]
			});
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00041E03 File Offset: 0x00040003
		private void OnCostPaid(Cost cost)
		{
			this.ReportCustomEvent<Cost>(BDSManager.EventName.pay_cost, cost);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00041E0E File Offset: 0x0004000E
		private void OnQuestActivated(Quest quest)
		{
			if (quest == null)
			{
				return;
			}
			this.ReportCustomEvent<Quest.QuestInfo>(BDSManager.EventName.quest_activate, quest.GetInfo());
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00041E28 File Offset: 0x00040028
		private void OnQuestCompleted(Quest quest)
		{
			if (quest == null)
			{
				return;
			}
			this.ReportCustomEvent<Quest.QuestInfo>(BDSManager.EventName.quest_complete, quest.GetInfo());
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00041E44 File Offset: 0x00040044
		private void OnMainCharacterDead(DamageInfo info)
		{
			string fromCharacterPresetName = "None";
			string fromCharacterNameKey = "None";
			if (info.fromCharacter)
			{
				CharacterRandomPreset characterPreset = info.fromCharacter.characterPreset;
				if (characterPreset != null)
				{
					fromCharacterPresetName = characterPreset.name;
					fromCharacterNameKey = characterPreset.nameKey;
				}
			}
			this.ReportCustomEvent<BDSManager.CharacterDeathContext>(BDSManager.EventName.main_character_dead, new BDSManager.CharacterDeathContext
			{
				damageInfo = info,
				levelInfo = LevelManager.GetCurrentLevelInfo(),
				fromCharacterPresetName = fromCharacterPresetName,
				fromCharacterNameKey = fromCharacterNameKey
			});
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00041EC4 File Offset: 0x000400C4
		private void OnEvacuated(EvacuationInfo evacuationInfo)
		{
			LevelManager.LevelInfo currentLevelInfo = LevelManager.GetCurrentLevelInfo();
			RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
			BDSManager.PlayerStatus playerStatus = BDSManager.PlayerStatus.CreateFromCurrent();
			this.ReportCustomEvent<BDSManager.EvacuationEventData>(BDSManager.EventName.level_evacuated, new BDSManager.EvacuationEventData
			{
				evacuationInfo = evacuationInfo,
				mapID = currentLevelInfo.activeSubSceneID,
				raidInfo = currentRaid,
				playerStatus = playerStatus
			});
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00041F19 File Offset: 0x00040119
		private void OnLevelInitialized()
		{
			this.ReportCustomEvent<LevelManager.LevelInfo>(BDSManager.EventName.level_initialized, LevelManager.GetCurrentLevelInfo());
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00041F27 File Offset: 0x00040127
		private void OnSceneLoadingFinish(SceneLoadingContext context)
		{
			this.ReportCustomEvent<SceneLoadingContext>(BDSManager.EventName.scene_load_start, context);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00041F31 File Offset: 0x00040131
		private void OnSceneLoadingStart(SceneLoadingContext context)
		{
			this.ReportCustomEvent<SceneLoadingContext>(BDSManager.EventName.scene_load_finish, context);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00041F3B File Offset: 0x0004013B
		private void OnRaidEnd(RaidUtilities.RaidInfo info)
		{
			this.ReportCustomEvent<RaidUtilities.RaidInfo>(BDSManager.EventName.raid_end, info);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00041F45 File Offset: 0x00040145
		private void OnNewRaid(RaidUtilities.RaidInfo info)
		{
			this.ReportCustomEvent<RaidUtilities.RaidInfo>(BDSManager.EventName.raid_new, info);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00041F4F File Offset: 0x0004014F
		private void OnSaveDeleted()
		{
			this.ReportCustomEvent(BDSManager.EventName.delete_save_data, StrJson.Create(new string[]
			{
				"slot",
				string.Format("{0}", SavesSystem.CurrentSlot)
			}));
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00041F84 File Offset: 0x00040184
		private void OnGameStarted()
		{
			int @int = PlayerPrefs.GetInt("AppStartCount", 0);
			this.sessionInfo = new BDSManager.SessionInfo
			{
				startCount = @int,
				isFirstTimeStart = (@int <= 0),
				session_id = DateTime.Now.ToBinary().GetHashCode()
			};
			this.sessionStartTime = DateTime.Now;
			this.ReportCustomEvent<BDSManager.SessionInfo>(BDSManager.EventName.app_start, this.sessionInfo);
			PlayerPrefs.SetInt("AppStartCount", @int + 1);
			PlayerPrefs.Save();
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0004200A File Offset: 0x0004020A
		private void ReportCustomEvent(BDSManager.EventName eventName, StrJson customParameters)
		{
			this.ReportCustomEvent(eventName, customParameters.ToString());
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0004201C File Offset: 0x0004021C
		private void ReportCustomEvent<T>(BDSManager.EventName eventName, T customParameters)
		{
			string customParameters2 = (customParameters != null) ? JsonUtility.ToJson(customParameters) : "";
			this.ReportCustomEvent(eventName, customParameters2);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0004204C File Offset: 0x0004024C
		private void ReportCustomEvent(BDSManager.EventName eventName, string customParameters = "")
		{
			string strPlayerInfo = BDSManager.PlayerInfo.GetCurrent().ToJson();
			SDK.ReportCustomEvent(eventName.ToString(), strPlayerInfo, "", customParameters);
			try
			{
				Action<string, string> onReportCustomEvent = BDSManager.OnReportCustomEvent;
				if (onReportCustomEvent != null)
				{
					onReportCustomEvent(eventName.ToString(), customParameters);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000420C0 File Offset: 0x000402C0
		[CompilerGenerated]
		internal static string <OnHealthDead>g__GetPresetName|36_0(Health health)
		{
			CharacterMainControl characterMainControl = health.TryGetCharacter();
			if (characterMainControl == null)
			{
				return "None";
			}
			CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
			if (characterPreset == null)
			{
				return "None";
			}
			return characterPreset.Name;
		}

		// Token: 0x04000D48 RID: 3400
		private float lastTimeHeartbeat;

		// Token: 0x04000D49 RID: 3401
		private int sessionID;

		// Token: 0x04000D4A RID: 3402
		private DateTime sessionStartTime;

		// Token: 0x04000D4B RID: 3403
		private BDSManager.SessionInfo sessionInfo;

		// Token: 0x04000D4C RID: 3404
		public static Action<string, string> OnReportCustomEvent;

		// Token: 0x0200050F RID: 1295
		private enum EventName
		{
			// Token: 0x04001DE1 RID: 7649
			none,
			// Token: 0x04001DE2 RID: 7650
			app_start,
			// Token: 0x04001DE3 RID: 7651
			begin_new_game,
			// Token: 0x04001DE4 RID: 7652
			delete_save_data,
			// Token: 0x04001DE5 RID: 7653
			raid_new,
			// Token: 0x04001DE6 RID: 7654
			raid_end,
			// Token: 0x04001DE7 RID: 7655
			scene_load_start,
			// Token: 0x04001DE8 RID: 7656
			scene_load_finish,
			// Token: 0x04001DE9 RID: 7657
			level_initialized,
			// Token: 0x04001DEA RID: 7658
			level_evacuated,
			// Token: 0x04001DEB RID: 7659
			main_character_dead,
			// Token: 0x04001DEC RID: 7660
			quest_activate,
			// Token: 0x04001DED RID: 7661
			quest_complete,
			// Token: 0x04001DEE RID: 7662
			pay_money,
			// Token: 0x04001DEF RID: 7663
			pay_cost,
			// Token: 0x04001DF0 RID: 7664
			item_to_inventory,
			// Token: 0x04001DF1 RID: 7665
			item_to_storage,
			// Token: 0x04001DF2 RID: 7666
			shop_purchased,
			// Token: 0x04001DF3 RID: 7667
			craft_craft,
			// Token: 0x04001DF4 RID: 7668
			craft_formula_unlock,
			// Token: 0x04001DF5 RID: 7669
			enemy_kill,
			// Token: 0x04001DF6 RID: 7670
			role_level_changed,
			// Token: 0x04001DF7 RID: 7671
			building_built,
			// Token: 0x04001DF8 RID: 7672
			building_destroyed,
			// Token: 0x04001DF9 RID: 7673
			perk_unlocked,
			// Token: 0x04001DFA RID: 7674
			masterkey_unlocked,
			// Token: 0x04001DFB RID: 7675
			role_equip,
			// Token: 0x04001DFC RID: 7676
			item_sold,
			// Token: 0x04001DFD RID: 7677
			reward_claimed,
			// Token: 0x04001DFE RID: 7678
			item_use,
			// Token: 0x04001DFF RID: 7679
			interact_start,
			// Token: 0x04001E00 RID: 7680
			face_customize_begin,
			// Token: 0x04001E01 RID: 7681
			face_customize_finish,
			// Token: 0x04001E02 RID: 7682
			heartbeat,
			// Token: 0x04001E03 RID: 7683
			cheat_mode_changed,
			// Token: 0x04001E04 RID: 7684
			app_end
		}

		// Token: 0x02000510 RID: 1296
		private struct CheatModeStatusChangeContext
		{
			// Token: 0x04001E05 RID: 7685
			public bool cheatModeActive;
		}

		// Token: 0x02000511 RID: 1297
		private struct InteractEventContext
		{
			// Token: 0x04001E06 RID: 7686
			public string interactGameObjectName;

			// Token: 0x04001E07 RID: 7687
			public string typeName;
		}

		// Token: 0x02000512 RID: 1298
		private struct ItemUseEventContext
		{
			// Token: 0x04001E08 RID: 7688
			public int itemTypeID;
		}

		// Token: 0x02000513 RID: 1299
		private struct RewardClaimEventContext
		{
			// Token: 0x04001E09 RID: 7689
			public int questID;

			// Token: 0x04001E0A RID: 7690
			public int rewardID;
		}

		// Token: 0x02000514 RID: 1300
		private struct ItemSoldEventContext
		{
			// Token: 0x04001E0B RID: 7691
			public string stockShopID;

			// Token: 0x04001E0C RID: 7692
			public int itemID;

			// Token: 0x04001E0D RID: 7693
			public int price;
		}

		// Token: 0x02000515 RID: 1301
		private struct EquipEventContext
		{
			// Token: 0x04001E0E RID: 7694
			public string slotKey;

			// Token: 0x04001E0F RID: 7695
			public int contentItemTypeID;
		}

		// Token: 0x02000516 RID: 1302
		private struct MasterKeyUnlockContext
		{
			// Token: 0x04001E10 RID: 7696
			public int keyID;
		}

		// Token: 0x02000517 RID: 1303
		private struct PerkInfo
		{
			// Token: 0x04001E11 RID: 7697
			public string perkTreeID;

			// Token: 0x04001E12 RID: 7698
			public string perkName;
		}

		// Token: 0x02000518 RID: 1304
		private struct BuildingEventContext
		{
			// Token: 0x04001E13 RID: 7699
			public string buildingID;
		}

		// Token: 0x02000519 RID: 1305
		private struct LevelChangedEventContext
		{
			// Token: 0x06002785 RID: 10117 RVA: 0x00090732 File Offset: 0x0008E932
			public LevelChangedEventContext(int from, int to)
			{
				this.from = from;
				this.to = to;
			}

			// Token: 0x04001E14 RID: 7700
			public int from;

			// Token: 0x04001E15 RID: 7701
			public int to;
		}

		// Token: 0x0200051A RID: 1306
		private struct EnemyKillInfo
		{
			// Token: 0x04001E16 RID: 7702
			public string enemyPresetName;

			// Token: 0x04001E17 RID: 7703
			public DamageInfo damageInfo;
		}

		// Token: 0x0200051B RID: 1307
		[Serializable]
		public struct PurchaseInfo
		{
			// Token: 0x04001E18 RID: 7704
			public string shopID;

			// Token: 0x04001E19 RID: 7705
			public int itemTypeID;

			// Token: 0x04001E1A RID: 7706
			public int itemAmount;
		}

		// Token: 0x0200051C RID: 1308
		private struct ItemInfo
		{
			// Token: 0x04001E1B RID: 7707
			public int itemId;

			// Token: 0x04001E1C RID: 7708
			public int amount;
		}

		// Token: 0x0200051D RID: 1309
		public struct CharacterDeathContext
		{
			// Token: 0x04001E1D RID: 7709
			public DamageInfo damageInfo;

			// Token: 0x04001E1E RID: 7710
			public string fromCharacterPresetName;

			// Token: 0x04001E1F RID: 7711
			public string fromCharacterNameKey;

			// Token: 0x04001E20 RID: 7712
			public LevelManager.LevelInfo levelInfo;
		}

		// Token: 0x0200051E RID: 1310
		[Serializable]
		private struct PlayerStatus
		{
			// Token: 0x06002786 RID: 10118 RVA: 0x00090744 File Offset: 0x0008E944
			public static BDSManager.PlayerStatus CreateFromCurrent()
			{
				CharacterMainControl main = CharacterMainControl.Main;
				if (main == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				Health health = main.Health;
				if (health == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				CharacterBuffManager buffManager = main.GetBuffManager();
				if (buffManager == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				if (main.CharacterItem == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				string[] array = new string[buffManager.Buffs.Count];
				for (int i = 0; i < buffManager.Buffs.Count; i++)
				{
					Buff buff = buffManager.Buffs[i];
					if (!(buff == null))
					{
						array[i] = string.Format("{0} {1}", buff.ID, buff.DisplayNameKey);
					}
				}
				int totalRawValue = main.CharacterItem.GetTotalRawValue();
				return new BDSManager.PlayerStatus
				{
					valid = true,
					healthMax = health.MaxHealth,
					health = main.CurrentEnergy,
					water = main.CurrentWater,
					food = main.CurrentEnergy,
					waterMax = main.MaxWater,
					foodMax = main.MaxEnergy,
					totalItemValue = totalRawValue
				};
			}

			// Token: 0x04001E21 RID: 7713
			public bool valid;

			// Token: 0x04001E22 RID: 7714
			public float healthMax;

			// Token: 0x04001E23 RID: 7715
			public float health;

			// Token: 0x04001E24 RID: 7716
			public float waterMax;

			// Token: 0x04001E25 RID: 7717
			public float foodMax;

			// Token: 0x04001E26 RID: 7718
			public float water;

			// Token: 0x04001E27 RID: 7719
			public float food;

			// Token: 0x04001E28 RID: 7720
			public string[] activeEffects;

			// Token: 0x04001E29 RID: 7721
			public int totalItemValue;
		}

		// Token: 0x0200051F RID: 1311
		private struct EvacuationEventData
		{
			// Token: 0x04001E2A RID: 7722
			public EvacuationInfo evacuationInfo;

			// Token: 0x04001E2B RID: 7723
			public string mapID;

			// Token: 0x04001E2C RID: 7724
			public RaidUtilities.RaidInfo raidInfo;

			// Token: 0x04001E2D RID: 7725
			public BDSManager.PlayerStatus playerStatus;
		}

		// Token: 0x02000520 RID: 1312
		[Serializable]
		private struct SessionInfo
		{
			// Token: 0x04001E2E RID: 7726
			public int startCount;

			// Token: 0x04001E2F RID: 7727
			public bool isFirstTimeStart;

			// Token: 0x04001E30 RID: 7728
			public int session_id;

			// Token: 0x04001E31 RID: 7729
			public int session_duration_seconds;
		}

		// Token: 0x02000521 RID: 1313
		public struct PlayerInfo
		{
			// Token: 0x06002787 RID: 10119 RVA: 0x00090898 File Offset: 0x0008EA98
			public PlayerInfo(int level, string steamAccountID, int saveSlot, string location, string language, string displayName, string difficulty, string platform, string version, string system)
			{
				this.role_name = displayName;
				this.profession_type = language;
				this.gender = version;
				this.level = string.Format("{0}", level);
				this.b_account_id = steamAccountID;
				this.b_role_id = string.Format("{0}|{1}", saveSlot, difficulty);
				this.b_tour_indicator = "0";
				this.b_zone_id = location;
				this.b_sdk_uid = platform + "|" + system;
			}

			// Token: 0x06002788 RID: 10120 RVA: 0x0009091C File Offset: 0x0008EB1C
			public static BDSManager.PlayerInfo GetCurrent()
			{
				string id = PlatformInfo.GetID();
				string displayName = PlatformInfo.GetDisplayName();
				return new BDSManager.PlayerInfo(EXPManager.Level, id, SavesSystem.CurrentSlot, RegionInfo.CurrentRegion.Name, Application.systemLanguage.ToString(), displayName, GameRulesManager.Current.displayNameKey, PlatformInfo.Platform.ToString(), GameMetaData.Instance.Version.ToString(), Environment.OSVersion.Platform.ToString())
				{
					gender = GameMetaData.Instance.Version.ToString()
				};
			}

			// Token: 0x06002789 RID: 10121 RVA: 0x000909D8 File Offset: 0x0008EBD8
			public static string GetCurrentJson()
			{
				return BDSManager.PlayerInfo.GetCurrent().ToJson();
			}

			// Token: 0x0600278A RID: 10122 RVA: 0x000909F2 File Offset: 0x0008EBF2
			public string ToJson()
			{
				return JsonUtility.ToJson(this);
			}

			// Token: 0x04001E32 RID: 7730
			public string role_name;

			// Token: 0x04001E33 RID: 7731
			public string profession_type;

			// Token: 0x04001E34 RID: 7732
			public string gender;

			// Token: 0x04001E35 RID: 7733
			public string level;

			// Token: 0x04001E36 RID: 7734
			public string b_account_id;

			// Token: 0x04001E37 RID: 7735
			public string b_role_id;

			// Token: 0x04001E38 RID: 7736
			public string b_tour_indicator;

			// Token: 0x04001E39 RID: 7737
			public string b_zone_id;

			// Token: 0x04001E3A RID: 7738
			public string b_sdk_uid;
		}
	}
}
