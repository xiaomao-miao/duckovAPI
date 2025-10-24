using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Buffs;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200008F RID: 143
[CreateAssetMenu(fileName = "New Character Random Preset", menuName = "Character Random Preset", order = 51)]
public class CharacterRandomPreset : ScriptableObject
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060004EF RID: 1263 RVA: 0x000162F7 File Offset: 0x000144F7
	public string Name
	{
		get
		{
			return this.nameKey.ToPlainText();
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00016304 File Offset: 0x00014504
	public string DisplayName
	{
		get
		{
			return this.nameKey.ToPlainText();
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00016311 File Offset: 0x00014511
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00016320 File Offset: 0x00014520
	public Sprite GetCharacterIcon()
	{
		switch (this.characterIconType)
		{
		case CharacterIconTypes.none:
			return null;
		case CharacterIconTypes.elete:
			return GameplayDataSettings.UIStyle.EleteCharacterIcon;
		case CharacterIconTypes.pmc:
			return GameplayDataSettings.UIStyle.PmcCharacterIcon;
		case CharacterIconTypes.boss:
			return GameplayDataSettings.UIStyle.BossCharacterIcon;
		case CharacterIconTypes.merchant:
			return GameplayDataSettings.UIStyle.MerchantCharacterIcon;
		case CharacterIconTypes.pet:
			return GameplayDataSettings.UIStyle.PetCharacterIcon;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00016394 File Offset: 0x00014594
	public UniTask<CharacterMainControl> CreateCharacterAsync(Vector3 pos, Vector3 dir, int relatedScene, CharacterSpawnerGroup group, bool isLeader)
	{
		CharacterRandomPreset.<CreateCharacterAsync>d__80 <CreateCharacterAsync>d__;
		<CreateCharacterAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateCharacterAsync>d__.<>4__this = this;
		<CreateCharacterAsync>d__.pos = pos;
		<CreateCharacterAsync>d__.dir = dir;
		<CreateCharacterAsync>d__.relatedScene = relatedScene;
		<CreateCharacterAsync>d__.group = group;
		<CreateCharacterAsync>d__.isLeader = isLeader;
		<CreateCharacterAsync>d__.<>1__state = -1;
		<CreateCharacterAsync>d__.<>t__builder.Start<CharacterRandomPreset.<CreateCharacterAsync>d__80>(ref <CreateCharacterAsync>d__);
		return <CreateCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00016404 File Offset: 0x00014604
	private UniTask<List<Item>> GenerateItems()
	{
		CharacterRandomPreset.<GenerateItems>d__81 <GenerateItems>d__;
		<GenerateItems>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<GenerateItems>d__.<>4__this = this;
		<GenerateItems>d__.<>1__state = -1;
		<GenerateItems>d__.<>t__builder.Start<CharacterRandomPreset.<GenerateItems>d__81>(ref <GenerateItems>d__);
		return <GenerateItems>d__.<>t__builder.Task;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00016448 File Offset: 0x00014648
	private UniTask AddBullet(CharacterMainControl character)
	{
		CharacterRandomPreset.<AddBullet>d__82 <AddBullet>d__;
		<AddBullet>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<AddBullet>d__.<>4__this = this;
		<AddBullet>d__.character = character;
		<AddBullet>d__.<>1__state = -1;
		<AddBullet>d__.<>t__builder.Start<CharacterRandomPreset.<AddBullet>d__82>(ref <AddBullet>d__);
		return <AddBullet>d__.<>t__builder.Task;
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x000166AC File Offset: 0x000148AC
	[CompilerGenerated]
	internal static void <CreateCharacterAsync>g__SetCharacterStat|80_0(string statName, float value, ref CharacterRandomPreset.<>c__DisplayClass80_0 A_2)
	{
		Stat stat = A_2.characterItemInstance.GetStat(statName.GetHashCode());
		if (stat == null)
		{
			return;
		}
		stat.BaseValue = value;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000166D8 File Offset: 0x000148D8
	[CompilerGenerated]
	internal static void <CreateCharacterAsync>g__MultiplyCharacterStat|80_1(string statName, float multiplier, ref CharacterRandomPreset.<>c__DisplayClass80_0 A_2)
	{
		Stat stat = A_2.characterItemInstance.GetStat(statName.GetHashCode());
		if (stat == null)
		{
			return;
		}
		stat.BaseValue *= multiplier;
	}

	// Token: 0x04000428 RID: 1064
	[LocalizationKey("Characters")]
	public string nameKey;

	// Token: 0x04000429 RID: 1065
	public AudioManager.VoiceType voiceType;

	// Token: 0x0400042A RID: 1066
	public AudioManager.FootStepMaterialType footstepMaterialType;

	// Token: 0x0400042B RID: 1067
	public InteractableLootbox lootBoxPrefab;

	// Token: 0x0400042C RID: 1068
	public List<AISpecialAttachmentBase> specialAttachmentBases;

	// Token: 0x0400042D RID: 1069
	public Teams team = Teams.scav;

	// Token: 0x0400042E RID: 1070
	public bool showName;

	// Token: 0x0400042F RID: 1071
	[FormerlySerializedAs("iconType")]
	[SerializeField]
	private CharacterIconTypes characterIconType;

	// Token: 0x04000430 RID: 1072
	public float health;

	// Token: 0x04000431 RID: 1073
	public bool hasSoul = true;

	// Token: 0x04000432 RID: 1074
	public bool showHealthBar = true;

	// Token: 0x04000433 RID: 1075
	public int exp = 100;

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x04000435 RID: 1077
	[SerializeField]
	private bool usePlayerPreset;

	// Token: 0x04000436 RID: 1078
	[SerializeField]
	private CustomFacePreset facePreset;

	// Token: 0x04000437 RID: 1079
	[SerializeField]
	private AICharacterController aiController;

	// Token: 0x04000438 RID: 1080
	public bool setActiveByPlayerDistance = true;

	// Token: 0x04000439 RID: 1081
	public float forceTracePlayerDistance;

	// Token: 0x0400043A RID: 1082
	public bool shootCanMove;

	// Token: 0x0400043B RID: 1083
	public float sightDistance = 17f;

	// Token: 0x0400043C RID: 1084
	public float sightAngle = 100f;

	// Token: 0x0400043D RID: 1085
	public float reactionTime = 0.2f;

	// Token: 0x0400043E RID: 1086
	public float nightReactionTimeFactor = 1.5f;

	// Token: 0x0400043F RID: 1087
	public float shootDelay = 0.2f;

	// Token: 0x04000440 RID: 1088
	public Vector2 shootTimeRange = new Vector2(0.4f, 1.5f);

	// Token: 0x04000441 RID: 1089
	public Vector2 shootTimeSpaceRange = new Vector2(2f, 3f);

	// Token: 0x04000442 RID: 1090
	public Vector2 combatMoveTimeRange = new Vector2(1f, 3f);

	// Token: 0x04000443 RID: 1091
	public float hearingAbility = 1f;

	// Token: 0x04000444 RID: 1092
	public float patrolRange = 8f;

	// Token: 0x04000445 RID: 1093
	[FormerlySerializedAs("combatRange")]
	public float combatMoveRange = 8f;

	// Token: 0x04000446 RID: 1094
	public bool canDash;

	// Token: 0x04000447 RID: 1095
	public Vector2 dashCoolTimeRange = new Vector2(2f, 4f);

	// Token: 0x04000448 RID: 1096
	[Range(0f, 1f)]
	public float minTraceTargetChance = 1f;

	// Token: 0x04000449 RID: 1097
	[Range(0f, 1f)]
	public float maxTraceTargetChance = 1f;

	// Token: 0x0400044A RID: 1098
	public float forgetTime = 8f;

	// Token: 0x0400044B RID: 1099
	public bool defaultWeaponOut = true;

	// Token: 0x0400044C RID: 1100
	public bool canTalk = true;

	// Token: 0x0400044D RID: 1101
	public float patrolTurnSpeed = 180f;

	// Token: 0x0400044E RID: 1102
	public float combatTurnSpeed = 1200f;

	// Token: 0x0400044F RID: 1103
	[ItemTypeID]
	public int wantItem = -1;

	// Token: 0x04000450 RID: 1104
	public float moveSpeedFactor = 1f;

	// Token: 0x04000451 RID: 1105
	public float bulletSpeedMultiplier = 1f;

	// Token: 0x04000452 RID: 1106
	[Range(1f, 2f)]
	public float gunDistanceMultiplier = 1f;

	// Token: 0x04000453 RID: 1107
	public float nightVisionAbility = 0.5f;

	// Token: 0x04000454 RID: 1108
	public float gunScatterMultiplier = 1f;

	// Token: 0x04000455 RID: 1109
	public float scatterMultiIfTargetRunning = 3f;

	// Token: 0x04000456 RID: 1110
	public float scatterMultiIfOffScreen = 4f;

	// Token: 0x04000457 RID: 1111
	[FormerlySerializedAs("gunDamageMultiplier")]
	public float damageMultiplier = 1f;

	// Token: 0x04000458 RID: 1112
	public float gunCritRateGain;

	// Token: 0x04000459 RID: 1113
	[Tooltip("用来决定双方造成伤害缩放")]
	public float aiCombatFactor = 1f;

	// Token: 0x0400045A RID: 1114
	public bool hasSkill;

	// Token: 0x0400045B RID: 1115
	public SkillBase skillPfb;

	// Token: 0x0400045C RID: 1116
	[Range(0.01f, 1f)]
	public float hasSkillChance = 1f;

	// Token: 0x0400045D RID: 1117
	public Vector2 skillCoolTimeRange = Vector2.one;

	// Token: 0x0400045E RID: 1118
	[Range(0.01f, 1f)]
	public float skillSuccessChance = 1f;

	// Token: 0x0400045F RID: 1119
	private float tryReleaseSkillTimeMarker = -1f;

	// Token: 0x04000460 RID: 1120
	[Range(0f, 1f)]
	public float itemSkillChance = 0.3f;

	// Token: 0x04000461 RID: 1121
	public float itemSkillCoolTime = 6f;

	// Token: 0x04000462 RID: 1122
	public List<Buff> buffs;

	// Token: 0x04000463 RID: 1123
	public List<Buff.BuffExclusiveTags> buffResist;

	// Token: 0x04000464 RID: 1124
	public float elementFactor_Physics = 1f;

	// Token: 0x04000465 RID: 1125
	public float elementFactor_Fire = 1f;

	// Token: 0x04000466 RID: 1126
	public float elementFactor_Poison = 1f;

	// Token: 0x04000467 RID: 1127
	public float elementFactor_Electricity = 1f;

	// Token: 0x04000468 RID: 1128
	public float elementFactor_Space = 1f;

	// Token: 0x04000469 RID: 1129
	[SerializeField]
	private List<CharacterRandomPreset.SetCharacterStatInfo> setStats;

	// Token: 0x0400046A RID: 1130
	[Range(0f, 1f)]
	public float hasCashChance;

	// Token: 0x0400046B RID: 1131
	public Vector2Int cashRange;

	// Token: 0x0400046C RID: 1132
	[SerializeField]
	private List<RandomItemGenerateDescription> itemsToGenerate;

	// Token: 0x0400046D RID: 1133
	[Space(12f)]
	[SerializeField]
	private RandomContainer<int> bulletQualityDistribution;

	// Token: 0x0400046E RID: 1134
	[SerializeField]
	private Tag[] bulletExclusiveTags;

	// Token: 0x0400046F RID: 1135
	[HideInInspector]
	[SerializeField]
	private ItemFilter bulletFilter;

	// Token: 0x04000470 RID: 1136
	[SerializeField]
	private Vector2 bulletCountRange = Vector2.one;

	// Token: 0x02000441 RID: 1089
	[Serializable]
	private struct SetCharacterStatInfo
	{
		// Token: 0x04001A77 RID: 6775
		public string statName;

		// Token: 0x04001A78 RID: 6776
		public Vector2 statBaseValue;
	}
}
