using System;
using System.Collections.Generic;
using Duckov;
using Duckov.ItemUsage;
using Duckov.Scenes;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000042 RID: 66
public class AICharacterController : MonoBehaviour
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000184 RID: 388 RVA: 0x000077B6 File Offset: 0x000059B6
	public CharacterMainControl CharacterMainControl
	{
		get
		{
			return this.characterMainControl;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000185 RID: 389 RVA: 0x000077BE File Offset: 0x000059BE
	public Vector3 NoticeFromPos
	{
		get
		{
			return this.noticeFromPos;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000186 RID: 390 RVA: 0x000077C6 File Offset: 0x000059C6
	public Vector3 NoticeFromDirection
	{
		get
		{
			return this.noticeFromDirection;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000187 RID: 391 RVA: 0x000077CE File Offset: 0x000059CE
	public CharacterMainControl NoticeFromCharacter
	{
		get
		{
			if (!this.noticed)
			{
				return null;
			}
			return this.noticeFromCharacter;
		}
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000077E0 File Offset: 0x000059E0
	public void Init(CharacterMainControl _characterMainControl, Vector3 patrolCenter, AudioManager.VoiceType voiceType = AudioManager.VoiceType.Duck, AudioManager.FootStepMaterialType footStepMatType = AudioManager.FootStepMaterialType.organic)
	{
		this.patrolPosition = patrolCenter;
		this.characterMainControl = _characterMainControl;
		this.pathControl.controller = this.characterMainControl;
		base.transform.SetParent(this.characterMainControl.transform, false);
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		this.characterMainControl.Health.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
		if (_characterMainControl)
		{
			_characterMainControl.AudioVoiceType = voiceType;
			_characterMainControl.FootStepMaterialType = footStepMatType;
		}
		this.scatterMultiplierStat = _characterMainControl.CharacterItem.GetStat("GunScatterMultiplier");
		this.rotateSpeedStat = this.characterMainControl.CharacterItem.GetStat("TurnSpeed");
		this.scatterMultiplierModifier = new Modifier(ModifierType.PercentageMultiply, this.scatterModifierMultiplier, this);
		this.scatterMultiplierStat.AddModifier(this.scatterMultiplierModifier);
		if (this.hasSkill && !this.skillInstance && this.skillPfb)
		{
			this.skillInstance = UnityEngine.Object.Instantiate<SkillBase>(this.skillPfb, base.transform);
			this.skillInstance.transform.localPosition = Vector3.zero;
			this.characterMainControl.SetSkill(SkillTypes.characterSkill, this.skillInstance, this.skillInstance.gameObject);
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00007940 File Offset: 0x00005B40
	public float NightReactionTimeMultiplier()
	{
		return 1f;
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00007947 File Offset: 0x00005B47
	public void AddItemSkill(ItemSetting_Skill skill)
	{
		this.skillsOnItem.Add(skill);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00007958 File Offset: 0x00005B58
	public ItemSetting_Skill GetItemSkill(bool random)
	{
		if (this.skillsOnItem.Count > 0 && random)
		{
			int index = UnityEngine.Random.Range(0, this.skillsOnItem.Count);
			ItemSetting_Skill itemSetting_Skill = this.skillsOnItem[index];
			if (itemSetting_Skill)
			{
				return itemSetting_Skill;
			}
			this.skillsOnItem.RemoveAt(index);
		}
		if (this.skillsOnItem.Count > 0)
		{
			int num = 0;
			if (num < this.skillsOnItem.Count)
			{
				ItemSetting_Skill itemSetting_Skill2 = this.skillsOnItem[num];
				if (itemSetting_Skill2 == null || itemSetting_Skill2.Item == null)
				{
					this.skillsOnItem.RemoveAt(num);
					num--;
				}
				return itemSetting_Skill2;
			}
		}
		return null;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00007A02 File Offset: 0x00005C02
	public void CheckAndAddDrugItem(Item targetItem)
	{
		if (!targetItem.GetComponent<Drug>())
		{
			return;
		}
		this.drugItems.Add(targetItem);
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00007A20 File Offset: 0x00005C20
	public Item GetDrugItem()
	{
		if (this.drugItems.Count > 0)
		{
			int num = 0;
			if (num < this.drugItems.Count)
			{
				Item item = this.drugItems[num];
				if (item == null)
				{
					this.drugItems.RemoveAt(num);
					num--;
				}
				return item;
			}
		}
		return null;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00007A74 File Offset: 0x00005C74
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		bool flag = this.searchedEnemy != null;
		if (this.aimTarget)
		{
			this.characterMainControl.SetAimPoint(this.aimTarget.transform.position + Vector3.up * 0.5f);
		}
		if (!this.gameCamera)
		{
			this.gameCamera = GameCamera.Instance;
		}
		this.updateValueTimer -= Time.deltaTime;
		if (this.updateValueTimer <= 0f)
		{
			this.updateValueTimer = 1f;
			bool isInDoor = MultiSceneCore.Instance.GetSubSceneInfo().IsInDoor;
			if (TimeOfDayController.Instance.AtNight && !isInDoor)
			{
				this.reactionTime = this.baseReactionTime * this.nightReactionTimeFactor;
			}
			else
			{
				this.reactionTime = this.baseReactionTime;
			}
			if (this.rotateSpeedStat != null)
			{
				if (flag)
				{
					this.rotateSpeedStat.BaseValue = this.combatTurnSpeed;
				}
				else
				{
					this.rotateSpeedStat.BaseValue = this.patrolTurnSpeed;
				}
			}
		}
		float num = 1f;
		if (this.aimTarget && CharacterMainControl.Main && this.aimTarget.gameObject == CharacterMainControl.Main.mainDamageReceiver.gameObject)
		{
			if (CharacterMainControl.Main.Running)
			{
				num = this.scatterMultiIfTargetRunning;
			}
			if (this.characterMainControl && this.gameCamera && this.gameCamera.IsOffScreen(this.characterMainControl.transform.position))
			{
				num = Mathf.Max(num, this.scatterMultiIfOffScreen);
			}
		}
		if (num != this.scatterModifierMultiplier)
		{
			this.scatterModifierMultiplier = num;
			this.scatterMultiplierModifier.Value = num;
		}
		if (this.group != null && this.group.hasLeader)
		{
			this.leaderAI = this.group.LeaderAI;
			if (this.leaderAI)
			{
				this.leader = this.leaderAI.characterMainControl;
			}
		}
		if (this.leader != null)
		{
			this.patrolPosition = this.leader.transform.position;
			Debug.DrawLine(base.transform.position, this.patrolPosition + Vector3.up * 2f, Color.magenta);
		}
		if (this.forceTracePlayerDistance > 0.5f && CharacterMainControl.Main != null && Vector3.Distance(base.transform.position, CharacterMainControl.Main.transform.position) < this.forceTracePlayerDistance)
		{
			this.searchedEnemy = CharacterMainControl.Main.mainDamageReceiver;
		}
		if (this.leaderAI)
		{
			if (this.leaderAI.searchedEnemy && !flag)
			{
				this.searchedEnemy = this.leaderAI.searchedEnemy;
				flag = true;
			}
			else if (!this.leaderAI.searchedEnemy && this.searchedEnemy)
			{
				this.leaderAI.searchedEnemy = this.searchedEnemy;
			}
		}
		if (flag && this.characterMainControl != null && this.searchedEnemy.Team == this.characterMainControl.Team)
		{
			this.searchedEnemy = null;
			flag = false;
		}
		if (this.searchedEnemy != this.cachedSearchedEnemy)
		{
			this.combatWithTargetTimer = 0f;
			this.cachedSearchedEnemy = this.searchedEnemy;
		}
		else
		{
			this.combatWithTargetTimer += Time.deltaTime;
		}
		if ((this.defaultWeaponOut || flag) && !this.weaponOut)
		{
			this.TakeOutWeapon();
		}
		if (this.hideIfFoundEnemy != null && !flag != this.hideIfFoundEnemy.activeSelf)
		{
			this.hideIfFoundEnemy.SetActive(!flag);
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00007E44 File Offset: 0x00006044
	public void SetNoticedToTarget(DamageReceiver target)
	{
		if (!target)
		{
			return;
		}
		this.noticeFromCharacter = target.health.TryGetCharacter();
		this.noticeTimeMarker = Time.time;
		this.noticeFromDirection = (target.transform.position - base.transform.position).normalized;
		this.noticeFromPos = target.transform.position;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00007EB0 File Offset: 0x000060B0
	private void OnEnable()
	{
		AIMainBrain.OnSoundSpawned += this.OnSound;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00007EC3 File Offset: 0x000060C3
	private void OnDisable()
	{
		AIMainBrain.OnSoundSpawned -= this.OnSound;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00007ED8 File Offset: 0x000060D8
	private void OnDestroy()
	{
		AIMainBrain.OnSoundSpawned -= this.OnSound;
		if (this.characterMainControl)
		{
			this.characterMainControl.Health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
		}
		if (this.scatterMultiplierStat != null)
		{
			this.scatterMultiplierStat.RemoveAllModifiersFromSource(this);
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00007F3C File Offset: 0x0000613C
	private void OnSound(AISound sound)
	{
		switch (sound.soundType)
		{
		case SoundTypes.unknowNoise:
			if (sound.fromTeam == this.characterMainControl.Team)
			{
				return;
			}
			break;
		case SoundTypes.combatSound:
			if (sound.fromTeam == this.characterMainControl.Team)
			{
				return;
			}
			break;
		case SoundTypes.grenadeDropSound:
			if (sound.fromObject)
			{
				this.foundDangerObject = sound.fromObject;
			}
			break;
		}
		Vector3 pos = sound.pos;
		pos.y = 0f;
		Vector3 position = base.transform.position;
		position.y = 0f;
		if (Vector3.Distance(position, pos) < sound.radius * this.hearingAbility)
		{
			this.noticed = true;
			this.noticeFromCharacter = sound.fromCharacter;
			this.noticeTimeMarker = Time.time;
			this.noticeFromDirection = (pos - position).normalized;
			this.noticeFromPos = sound.pos;
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000802C File Offset: 0x0000622C
	private void OnHurt(DamageInfo dmgInfo)
	{
		if (dmgInfo.isFromBuffOrEffect)
		{
			return;
		}
		this.noticed = true;
		this.lastDamageInfo = dmgInfo;
		this.noticeFromCharacter = dmgInfo.fromCharacter;
		this.noticeTimeMarker = Time.time;
		this.hurtTimeMarker = Time.time;
		this.noticeFromDirection = dmgInfo.damageNormal;
		if (this.noticeFromCharacter)
		{
			this.noticeFromPos = this.noticeFromCharacter.transform.position;
			return;
		}
		this.noticeFromPos = base.transform.position + this.noticeFromDirection * 3f;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x000080C8 File Offset: 0x000062C8
	public bool IsHurt(float timeThreshold, int damageThreshold, ref DamageInfo dmgInfo)
	{
		dmgInfo = this.lastDamageInfo;
		return Time.time - this.hurtTimeMarker < timeThreshold && this.lastDamageInfo.finalDamage >= (float)damageThreshold;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x000080F9 File Offset: 0x000062F9
	public bool isNoticing(float timeThreshold)
	{
		return this.noticed && Time.time - this.noticeTimeMarker < timeThreshold;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00008114 File Offset: 0x00006314
	public void MoveToPos(Vector3 pos)
	{
		if (!this.pathControl || !this.pathControl.controller)
		{
			return;
		}
		this.pathControl.MoveToPos(pos);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00008142 File Offset: 0x00006342
	public bool HasPath()
	{
		return this.pathControl.path != null;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00008152 File Offset: 0x00006352
	public bool WaitingForPathResult()
	{
		return this.pathControl.WaitingForPathResult;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000815F File Offset: 0x0000635F
	public void StopMove()
	{
		this.pathControl.StopMove();
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000816C File Offset: 0x0000636C
	public bool IsMoving()
	{
		return this.pathControl.Moving;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00008179 File Offset: 0x00006379
	public bool ReachedEndOfPath()
	{
		return this.pathControl.ReachedEndOfPath;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00008186 File Offset: 0x00006386
	public void SetTarget(Transform _aimTarget)
	{
		this.aimTarget = _aimTarget;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000818F File Offset: 0x0000638F
	public void SetAimInput(Vector3 aimInput, AimTypes aimType)
	{
		this.characterMainControl.SetAimPoint(this.characterMainControl.transform.position + aimInput * 1000f);
		this.characterMainControl.SetAimType(aimType);
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000081C8 File Offset: 0x000063C8
	public void PutBackWeapon()
	{
		if (this.characterMainControl.CurrentHoldItemAgent == null)
		{
			return;
		}
		this.characterMainControl.agentHolder.ChangeHoldItem(null);
		this.weaponOut = false;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000081F8 File Offset: 0x000063F8
	public void TakeOutWeapon()
	{
		bool flag = this.characterMainControl.SwitchToFirstAvailableWeapon();
		this.weaponOut = flag;
	}

	// Token: 0x04000115 RID: 277
	public DamageReceiver searchedEnemy;

	// Token: 0x04000116 RID: 278
	public InteractablePickup searchedPickup;

	// Token: 0x04000117 RID: 279
	private DamageReceiver cachedSearchedEnemy;

	// Token: 0x04000118 RID: 280
	private CharacterMainControl characterMainControl;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	private AI_PathControl pathControl;

	// Token: 0x0400011A RID: 282
	public CharacterSpawnerGroup group;

	// Token: 0x0400011B RID: 283
	public CharacterMainControl leader;

	// Token: 0x0400011C RID: 284
	public AICharacterController leaderAI;

	// Token: 0x0400011D RID: 285
	public bool shootCanMove;

	// Token: 0x0400011E RID: 286
	public bool defaultWeaponOut;

	// Token: 0x0400011F RID: 287
	private float updateValueTimer = 1f;

	// Token: 0x04000120 RID: 288
	public float patrolTurnSpeed = 200f;

	// Token: 0x04000121 RID: 289
	public float combatTurnSpeed = 1000f;

	// Token: 0x04000122 RID: 290
	private Stat rotateSpeedStat;

	// Token: 0x04000123 RID: 291
	public bool hasSkill;

	// Token: 0x04000124 RID: 292
	public SkillBase skillPfb;

	// Token: 0x04000125 RID: 293
	public SkillBase skillInstance;

	// Token: 0x04000126 RID: 294
	public Vector2 skillCoolTimeRange;

	// Token: 0x04000127 RID: 295
	[Range(0.01f, 1f)]
	public float skillSuccessChance = 1f;

	// Token: 0x04000128 RID: 296
	public float nextReleaseSkillTimeMarker = -1f;

	// Token: 0x04000129 RID: 297
	private float noticeTimeMarker;

	// Token: 0x0400012A RID: 298
	public bool noticed;

	// Token: 0x0400012B RID: 299
	public float sightDistance = 20f;

	// Token: 0x0400012C RID: 300
	public float forceTracePlayerDistance;

	// Token: 0x0400012D RID: 301
	public float sightAngle = 100f;

	// Token: 0x0400012E RID: 302
	public float baseReactionTime = 0.2f;

	// Token: 0x0400012F RID: 303
	public float nightReactionTimeFactor = 1.5f;

	// Token: 0x04000130 RID: 304
	public float reactionTime = 0.2f;

	// Token: 0x04000131 RID: 305
	public float shootDelay = 0.2f;

	// Token: 0x04000132 RID: 306
	public float scatterMultiIfTargetRunning = 4f;

	// Token: 0x04000133 RID: 307
	public float scatterMultiIfOffScreen = 4f;

	// Token: 0x04000134 RID: 308
	public Vector2 shootTimeRange = Vector2.one;

	// Token: 0x04000135 RID: 309
	public Vector2 shootTimeSpaceRange = Vector2.one;

	// Token: 0x04000136 RID: 310
	public Vector2 combatMoveTimeRange = new Vector2(1f, 3f);

	// Token: 0x04000137 RID: 311
	public bool canDash;

	// Token: 0x04000138 RID: 312
	public Vector2 dashCoolTimeRange;

	// Token: 0x04000139 RID: 313
	private Vector3 noticeFromPos;

	// Token: 0x0400013A RID: 314
	[ItemTypeID]
	public int wantItem;

	// Token: 0x0400013B RID: 315
	private Vector3 noticeFromDirection;

	// Token: 0x0400013C RID: 316
	private float combatWithTargetTimer;

	// Token: 0x0400013D RID: 317
	private bool weaponOut;

	// Token: 0x0400013E RID: 318
	private CharacterMainControl noticeFromCharacter;

	// Token: 0x0400013F RID: 319
	public float hearingAbility = 1f;

	// Token: 0x04000140 RID: 320
	public float traceTargetChance = 1f;

	// Token: 0x04000141 RID: 321
	public Transform aimTarget;

	// Token: 0x04000142 RID: 322
	private bool aimingRuningMainCharacter;

	// Token: 0x04000143 RID: 323
	public float patrolRange;

	// Token: 0x04000144 RID: 324
	public Vector3 patrolPosition;

	// Token: 0x04000145 RID: 325
	public float combatMoveRange;

	// Token: 0x04000146 RID: 326
	public float forgetTime = 8f;

	// Token: 0x04000147 RID: 327
	public bool canTalk = true;

	// Token: 0x04000148 RID: 328
	public bool alert;

	// Token: 0x04000149 RID: 329
	public BehaviourTree patrolTree;

	// Token: 0x0400014A RID: 330
	public BehaviourTree alertTree;

	// Token: 0x0400014B RID: 331
	public BehaviourTree combatTree;

	// Token: 0x0400014C RID: 332
	public BehaviourTree combat_Attack_Tree;

	// Token: 0x0400014D RID: 333
	[HideInInspector]
	public float hurtTimeMarker;

	// Token: 0x0400014E RID: 334
	[HideInInspector]
	public DamageInfo lastDamageInfo;

	// Token: 0x0400014F RID: 335
	public bool hasObsticleToTarget;

	// Token: 0x04000150 RID: 336
	public GameObject hideIfFoundEnemy;

	// Token: 0x04000151 RID: 337
	private Modifier scatterMultiplierModifier;

	// Token: 0x04000152 RID: 338
	private Stat scatterMultiplierStat;

	// Token: 0x04000153 RID: 339
	private float scatterModifierMultiplier = 1f;

	// Token: 0x04000154 RID: 340
	public float itemSkillChance = 0.3f;

	// Token: 0x04000155 RID: 341
	public float itemSkillCoolTime = 6f;

	// Token: 0x04000156 RID: 342
	private GameCamera gameCamera;

	// Token: 0x04000157 RID: 343
	public GameObject foundDangerObject;

	// Token: 0x04000158 RID: 344
	private List<ItemSetting_Skill> skillsOnItem = new List<ItemSetting_Skill>();

	// Token: 0x04000159 RID: 345
	private List<Item> drugItems = new List<Item>();
}
