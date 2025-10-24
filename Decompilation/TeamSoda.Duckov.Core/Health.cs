using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Buffs;
using Duckov.Scenes;
using Duckov.Utilities;
using Duckov.Weathers;
using FX;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000065 RID: 101
public class Health : MonoBehaviour
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000FF4E File Offset: 0x0000E14E
	// (set) Token: 0x060003A6 RID: 934 RVA: 0x0000FF45 File Offset: 0x0000E145
	public bool showHealthBar
	{
		get
		{
			return this._showHealthBar;
		}
		set
		{
			this._showHealthBar = value;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000FF56 File Offset: 0x0000E156
	public bool Hidden
	{
		get
		{
			return this.TryGetCharacter() && this.characterCached.Hidden;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060003A9 RID: 937 RVA: 0x0000FF74 File Offset: 0x0000E174
	public float MaxHealth
	{
		get
		{
			float num;
			if (this.item)
			{
				num = this.item.GetStatValue(this.maxHealthHash);
			}
			else
			{
				num = (float)this.defaultMaxHealth;
			}
			if (!Mathf.Approximately(this.lastMaxHealth, num))
			{
				this.lastMaxHealth = num;
				UnityEvent<Health> onMaxHealthChange = this.OnMaxHealthChange;
				if (onMaxHealthChange != null)
				{
					onMaxHealthChange.Invoke(this);
				}
			}
			return num;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060003AA RID: 938 RVA: 0x0000FFD8 File Offset: 0x0000E1D8
	public bool IsMainCharacterHealth
	{
		get
		{
			return !(LevelManager.Instance == null) && !(LevelManager.Instance.MainCharacter == null) && !(LevelManager.Instance.MainCharacter != this.TryGetCharacter());
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060003AB RID: 939 RVA: 0x00010017 File Offset: 0x0000E217
	// (set) Token: 0x060003AC RID: 940 RVA: 0x00010020 File Offset: 0x0000E220
	public float CurrentHealth
	{
		get
		{
			return this._currentHealth;
		}
		set
		{
			float currentHealth = this._currentHealth;
			this._currentHealth = value;
			if (this._currentHealth != currentHealth)
			{
				UnityEvent<Health> onHealthChange = this.OnHealthChange;
				if (onHealthChange == null)
				{
					return;
				}
				onHealthChange.Invoke(this);
			}
		}
	}

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x060003AD RID: 941 RVA: 0x00010058 File Offset: 0x0000E258
	// (remove) Token: 0x060003AE RID: 942 RVA: 0x0001008C File Offset: 0x0000E28C
	public static event Action<Health, DamageInfo> OnHurt;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x060003AF RID: 943 RVA: 0x000100C0 File Offset: 0x0000E2C0
	// (remove) Token: 0x060003B0 RID: 944 RVA: 0x000100F4 File Offset: 0x0000E2F4
	public static event Action<Health, DamageInfo> OnDead;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x060003B1 RID: 945 RVA: 0x00010128 File Offset: 0x0000E328
	// (remove) Token: 0x060003B2 RID: 946 RVA: 0x0001015C File Offset: 0x0000E35C
	public static event Action<Health> OnRequestHealthBar;

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060003B3 RID: 947 RVA: 0x0001018F File Offset: 0x0000E38F
	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060003B4 RID: 948 RVA: 0x00010197 File Offset: 0x0000E397
	public bool Invincible
	{
		get
		{
			return this.invincible;
		}
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x000101A0 File Offset: 0x0000E3A0
	public CharacterMainControl TryGetCharacter()
	{
		if (this.characterCached != null)
		{
			return this.characterCached;
		}
		if (!this.hasCharacter)
		{
			return null;
		}
		if (!this.item)
		{
			this.hasCharacter = false;
			return null;
		}
		this.characterCached = this.item.GetCharacterMainControl();
		if (!this.characterCached)
		{
			this.hasCharacter = true;
		}
		return this.characterCached;
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001020D File Offset: 0x0000E40D
	public float BodyArmor
	{
		get
		{
			if (this.item)
			{
				return this.item.GetStatValue(this.bodyArmorHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060003B7 RID: 951 RVA: 0x00010233 File Offset: 0x0000E433
	public float HeadArmor
	{
		get
		{
			if (this.item)
			{
				return this.item.GetStatValue(this.headArmorHash);
			}
			return 0f;
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001025C File Offset: 0x0000E45C
	public float ElementFactor(ElementTypes type)
	{
		float num = 1f;
		if (!this.item)
		{
			return num;
		}
		Weather currentWeather = TimeOfDayController.Instance.CurrentWeather;
		bool isBaseLevel = LevelManager.Instance.IsBaseLevel;
		switch (type)
		{
		case ElementTypes.physics:
			num = this.item.GetStat(this.Hash_ElementFactor_Physics).Value;
			break;
		case ElementTypes.fire:
			num = this.item.GetStat(this.Hash_ElementFactor_Fire).Value;
			if (!isBaseLevel && currentWeather == Weather.Rainy)
			{
				num -= 0.15f;
			}
			break;
		case ElementTypes.poison:
			num = this.item.GetStat(this.Hash_ElementFactor_Poison).Value;
			break;
		case ElementTypes.electricity:
			num = this.item.GetStat(this.Hash_ElementFactor_Electricity).Value;
			if (!isBaseLevel && currentWeather == Weather.Rainy)
			{
				num += 0.2f;
			}
			break;
		case ElementTypes.space:
			num = this.item.GetStat(this.Hash_ElementFactor_Space).Value;
			break;
		}
		return num;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00010350 File Offset: 0x0000E550
	private void Start()
	{
		if (this.autoInit)
		{
			this.Init();
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00010360 File Offset: 0x0000E560
	public void SetItemAndCharacter(Item _item, CharacterMainControl _character)
	{
		this.item = _item;
		if (_character)
		{
			this.hasCharacter = true;
			this.characterCached = _character;
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0001037F File Offset: 0x0000E57F
	public void Init()
	{
		if (this.CurrentHealth <= 0f)
		{
			this.CurrentHealth = this.MaxHealth;
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001039A File Offset: 0x0000E59A
	public void AddBuff(Buff buffPfb, CharacterMainControl fromWho, int overrideFromWeaponID = 0)
	{
		CharacterMainControl characterMainControl = this.TryGetCharacter();
		if (characterMainControl == null)
		{
			return;
		}
		characterMainControl.AddBuff(buffPfb, fromWho, overrideFromWeaponID);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x000103AF File Offset: 0x0000E5AF
	private void Update()
	{
	}

	// Token: 0x060003BE RID: 958 RVA: 0x000103B4 File Offset: 0x0000E5B4
	public bool Hurt(DamageInfo damageInfo)
	{
		if (MultiSceneCore.Instance != null && MultiSceneCore.Instance.IsLoading)
		{
			return false;
		}
		if (this.invincible)
		{
			return false;
		}
		if (this.isDead)
		{
			return false;
		}
		if (damageInfo.buff != null && UnityEngine.Random.Range(0f, 1f) < damageInfo.buffChance)
		{
			this.AddBuff(damageInfo.buff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		bool flag = LevelManager.Rule.AdvancedDebuffMode;
		if (LevelManager.Instance.IsBaseLevel)
		{
			flag = false;
		}
		float num = 0.2f;
		float num2 = 0.12f;
		CharacterMainControl characterMainControl = this.TryGetCharacter();
		if (!this.IsMainCharacterHealth)
		{
			num = 0.1f;
			num2 = 0.1f;
		}
		if (flag && UnityEngine.Random.Range(0f, 1f) < damageInfo.bleedChance * num)
		{
			this.AddBuff(GameplayDataSettings.Buffs.BoneCrackBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		else if (flag && UnityEngine.Random.Range(0f, 1f) < damageInfo.bleedChance * num2)
		{
			this.AddBuff(GameplayDataSettings.Buffs.WoundBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		else if (UnityEngine.Random.Range(0f, 1f) < damageInfo.bleedChance)
		{
			if (flag)
			{
				this.AddBuff(GameplayDataSettings.Buffs.UnlimitBleedBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
			}
			else
			{
				this.AddBuff(GameplayDataSettings.Buffs.BleedSBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
			}
		}
		bool flag2 = UnityEngine.Random.Range(0f, 1f) < damageInfo.critRate;
		damageInfo.crit = (flag2 ? 1 : 0);
		if (!damageInfo.ignoreDifficulty && this.team == Teams.player)
		{
			damageInfo.damageValue *= LevelManager.Rule.DamageFactor_ToPlayer;
		}
		float num3 = damageInfo.damageValue * (flag2 ? damageInfo.critDamageFactor : 1f);
		if (damageInfo.damageType != DamageTypes.realDamage && !damageInfo.ignoreArmor)
		{
			float num4 = flag2 ? this.HeadArmor : this.BodyArmor;
			if (characterMainControl && LevelManager.Instance.IsRaidMap)
			{
				Item item = flag2 ? characterMainControl.GetHelmatItem() : characterMainControl.GetArmorItem();
				if (item)
				{
					item.Durability = Mathf.Max(0f, item.Durability - damageInfo.armorBreak);
				}
			}
			float num5 = 1f;
			if (num4 > 0f)
			{
				num5 = 2f / (Mathf.Clamp(num4 - damageInfo.armorPiercing, 0f, 999f) + 2f);
			}
			if (characterMainControl && !characterMainControl.IsMainCharacter && damageInfo.fromCharacter && !damageInfo.fromCharacter.IsMainCharacter)
			{
				CharacterRandomPreset characterPreset = damageInfo.fromCharacter.characterPreset;
				CharacterRandomPreset characterPreset2 = characterMainControl.characterPreset;
				if (characterPreset && characterPreset2)
				{
					num5 *= characterPreset.aiCombatFactor / characterPreset2.aiCombatFactor;
				}
			}
			num3 *= num5;
		}
		if (damageInfo.elementFactors.Count <= 0)
		{
			damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.physics, 1f));
		}
		float num6 = 0f;
		foreach (ElementFactor elementFactor in damageInfo.elementFactors)
		{
			float factor = elementFactor.factor;
			float num7 = this.ElementFactor(elementFactor.elementType);
			float num8 = num3 * factor * num7;
			if (num8 < 1f && num8 > 0f && num7 > 0f && factor > 0f)
			{
				num8 = 1f;
			}
			if (num8 > 0f && !this.Hidden && PopText.instance)
			{
				GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook elementDamagePopTextLook = GameplayDataSettings.UIStyle.GetElementDamagePopTextLook(elementFactor.elementType);
				float size = flag2 ? elementDamagePopTextLook.critSize : elementDamagePopTextLook.normalSize;
				Color color = elementDamagePopTextLook.color;
				PopText.Pop(num8.ToString("F1"), damageInfo.damagePoint + Vector3.up * 2f, color, size, flag2 ? GameplayDataSettings.UIStyle.CritPopSprite : null);
			}
			num6 += num8;
		}
		damageInfo.finalDamage = num6;
		if (this.CurrentHealth < damageInfo.finalDamage)
		{
			damageInfo.finalDamage = this.CurrentHealth + 1f;
		}
		this.CurrentHealth -= damageInfo.finalDamage;
		UnityEvent<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent.Invoke(damageInfo);
		}
		Action<Health, DamageInfo> onHurt = Health.OnHurt;
		if (onHurt != null)
		{
			onHurt(this, damageInfo);
		}
		if (this.isDead)
		{
			return true;
		}
		if (this.CurrentHealth <= 0f)
		{
			bool flag3 = true;
			if (!LevelManager.Instance.IsRaidMap)
			{
				flag3 = false;
			}
			if (!flag3)
			{
				this.SetHealth(1f);
			}
		}
		if (this.CurrentHealth <= 0f)
		{
			this.CurrentHealth = 0f;
			this.isDead = true;
			if (LevelManager.Instance.MainCharacter != this.TryGetCharacter())
			{
				this.DestroyOnDelay().Forget();
			}
			if (this.item != null && this.team != Teams.player && damageInfo.fromCharacter && damageInfo.fromCharacter.IsMainCharacter)
			{
				EXPManager.AddExp(this.item.GetInt("Exp", 0));
			}
			UnityEvent<DamageInfo> onDeadEvent = this.OnDeadEvent;
			if (onDeadEvent != null)
			{
				onDeadEvent.Invoke(damageInfo);
			}
			Action<Health, DamageInfo> onDead = Health.OnDead;
			if (onDead != null)
			{
				onDead(this, damageInfo);
			}
			base.gameObject.SetActive(false);
			if (damageInfo.fromCharacter && damageInfo.fromCharacter.IsMainCharacter)
			{
				Debug.Log("Killed by maincharacter");
			}
		}
		return true;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001099C File Offset: 0x0000EB9C
	public void RequestHealthBar()
	{
		if (this.showHealthBar && LevelManager.LevelInited)
		{
			Action<Health> onRequestHealthBar = Health.OnRequestHealthBar;
			if (onRequestHealthBar == null)
			{
				return;
			}
			onRequestHealthBar(this);
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x000109C0 File Offset: 0x0000EBC0
	public UniTask DestroyOnDelay()
	{
		Health.<DestroyOnDelay>d__66 <DestroyOnDelay>d__;
		<DestroyOnDelay>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DestroyOnDelay>d__.<>4__this = this;
		<DestroyOnDelay>d__.<>1__state = -1;
		<DestroyOnDelay>d__.<>t__builder.Start<Health.<DestroyOnDelay>d__66>(ref <DestroyOnDelay>d__);
		return <DestroyOnDelay>d__.<>t__builder.Task;
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00010A03 File Offset: 0x0000EC03
	public void AddHealth(float healthValue)
	{
		this.CurrentHealth = Mathf.Min(this.MaxHealth, this.CurrentHealth + healthValue);
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00010A1E File Offset: 0x0000EC1E
	public void SetHealth(float healthValue)
	{
		this.CurrentHealth = Mathf.Min(this.MaxHealth, healthValue);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00010A32 File Offset: 0x0000EC32
	public void SetInvincible(bool value)
	{
		this.invincible = value;
	}

	// Token: 0x040002C3 RID: 707
	public Teams team;

	// Token: 0x040002C4 RID: 708
	public bool hasSoul = true;

	// Token: 0x040002C5 RID: 709
	private Item item;

	// Token: 0x040002C6 RID: 710
	private int maxHealthHash = "MaxHealth".GetHashCode();

	// Token: 0x040002C7 RID: 711
	private float lastMaxHealth;

	// Token: 0x040002C8 RID: 712
	private bool _showHealthBar;

	// Token: 0x040002C9 RID: 713
	[SerializeField]
	private int defaultMaxHealth;

	// Token: 0x040002CA RID: 714
	private float _currentHealth;

	// Token: 0x040002CB RID: 715
	public UnityEvent<Health> OnHealthChange;

	// Token: 0x040002CC RID: 716
	public UnityEvent<Health> OnMaxHealthChange;

	// Token: 0x040002D2 RID: 722
	public float healthBarHeight = 2f;

	// Token: 0x040002D3 RID: 723
	private bool isDead;

	// Token: 0x040002D4 RID: 724
	public bool autoInit = true;

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	private bool DestroyOnDead = true;

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	private float DeadDestroyDelay = 0.5f;

	// Token: 0x040002D7 RID: 727
	private bool inited;

	// Token: 0x040002D8 RID: 728
	private bool invincible;

	// Token: 0x040002D9 RID: 729
	private bool hasCharacter = true;

	// Token: 0x040002DA RID: 730
	private CharacterMainControl characterCached;

	// Token: 0x040002DB RID: 731
	private int bodyArmorHash = "BodyArmor".GetHashCode();

	// Token: 0x040002DC RID: 732
	private int headArmorHash = "HeadArmor".GetHashCode();

	// Token: 0x040002DD RID: 733
	private int Hash_ElementFactor_Physics = "ElementFactor_Physics".GetHashCode();

	// Token: 0x040002DE RID: 734
	private int Hash_ElementFactor_Fire = "ElementFactor_Fire".GetHashCode();

	// Token: 0x040002DF RID: 735
	private int Hash_ElementFactor_Poison = "ElementFactor_Poison".GetHashCode();

	// Token: 0x040002E0 RID: 736
	private int Hash_ElementFactor_Electricity = "ElementFactor_Electricity".GetHashCode();

	// Token: 0x040002E1 RID: 737
	private int Hash_ElementFactor_Space = "ElementFactor_Space".GetHashCode();
}
