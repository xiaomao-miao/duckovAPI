using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class ItemAgent_MeleeWeapon : DuckovItemAgent
{
	// Token: 0x17000194 RID: 404
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x0002235E File Offset: 0x0002055E
	public float Damage
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.DamageHash);
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00022370 File Offset: 0x00020570
	public float CritRate
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.CritRateHash);
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00022382 File Offset: 0x00020582
	public float CritDamageFactor
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.CritDamageFactorHash);
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00022394 File Offset: 0x00020594
	public float ArmorPiercing
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.ArmorPiercingHash);
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x000223A6 File Offset: 0x000205A6
	public float AttackSpeed
	{
		get
		{
			return Mathf.Max(0.1f, base.Item.GetStatValue(ItemAgent_MeleeWeapon.AttackSpeedHash));
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060007A4 RID: 1956 RVA: 0x000223C2 File Offset: 0x000205C2
	public float AttackRange
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.AttackRangeHash);
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060007A5 RID: 1957 RVA: 0x000223D4 File Offset: 0x000205D4
	public float DealDamageTime
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.DealDamageTimeHash);
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000223E6 File Offset: 0x000205E6
	public float StaminaCost
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.StaminaCostHash);
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060007A7 RID: 1959 RVA: 0x000223F8 File Offset: 0x000205F8
	public float BleedChance
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.BleedChanceHash);
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0002240A File Offset: 0x0002060A
	public float MoveSpeedMultiplier
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.MoveSpeedMultiplierHash);
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0002241C File Offset: 0x0002061C
	public float CharacterDamageMultiplier
	{
		get
		{
			if (!base.Holder)
			{
				return 1f;
			}
			return base.Holder.MeleeDamageMultiplier;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060007AA RID: 1962 RVA: 0x0002243C File Offset: 0x0002063C
	public float CharacterCritRateGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.MeleeCritRateGain;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060007AB RID: 1963 RVA: 0x0002245C File Offset: 0x0002065C
	public float CharacterCritDamageGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.MeleeCritDamageGain;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x060007AC RID: 1964 RVA: 0x0002247C File Offset: 0x0002067C
	public string SoundKey
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this.soundKey))
			{
				return "Default";
			}
			return this.soundKey;
		}
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00022498 File Offset: 0x00020698
	private int UpdateColliders()
	{
		if (this.colliders == null)
		{
			this.colliders = new Collider[6];
		}
		return Physics.OverlapSphereNonAlloc(base.Holder.transform.position, this.AttackRange, this.colliders, GameplayDataSettings.Layers.damageReceiverLayerMask);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x000224E9 File Offset: 0x000206E9
	public void CheckAndDealDamage()
	{
		this.CheckCollidersInRange(true);
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000224F3 File Offset: 0x000206F3
	public bool AttackableTargetInRange()
	{
		return this.CheckCollidersInRange(false) > 0;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00022500 File Offset: 0x00020700
	private int CheckCollidersInRange(bool dealDamage)
	{
		if (this.colliders == null)
		{
			this.colliders = new Collider[6];
		}
		int num = this.UpdateColliders();
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.colliders[i];
			DamageReceiver component = collider.GetComponent<DamageReceiver>();
			if (!(component == null) && Team.IsEnemy(component.Team, base.Holder.Team))
			{
				Health health = component.health;
				if (health)
				{
					CharacterMainControl characterMainControl = health.TryGetCharacter();
					if (characterMainControl == base.Holder || (characterMainControl && characterMainControl.Dashing))
					{
						goto IL_2B3;
					}
				}
				Vector3 vector = collider.transform.position - base.Holder.transform.position;
				vector.y = 0f;
				vector.Normalize();
				if (Vector3.Angle(vector, base.Holder.CurrentAimDirection) < 90f)
				{
					num2++;
					if (dealDamage)
					{
						DamageInfo damageInfo = new DamageInfo(base.Holder);
						damageInfo.damageValue = this.Damage * this.CharacterDamageMultiplier;
						damageInfo.armorPiercing = this.ArmorPiercing;
						damageInfo.critDamageFactor = this.CritDamageFactor * (1f + this.CharacterCritDamageGain);
						damageInfo.critRate = this.CritRate * (1f + this.CharacterCritRateGain);
						damageInfo.crit = -1;
						damageInfo.damageNormal = -base.Holder.modelRoot.right;
						damageInfo.damagePoint = collider.transform.position - vector * 0.2f;
						damageInfo.damagePoint.y = base.transform.position.y;
						damageInfo.fromWeaponItemID = base.Item.TypeID;
						damageInfo.bleedChance = this.BleedChance;
						if (this.setting)
						{
							damageInfo.isExplosion = this.setting.dealExplosionDamage;
						}
						component.Hurt(damageInfo);
						component.AddBuff(GameplayDataSettings.Buffs.Pain, base.Holder);
						if (this.hitFx)
						{
							UnityEngine.Object.Instantiate<GameObject>(this.hitFx, damageInfo.damagePoint, Quaternion.LookRotation(damageInfo.damageNormal, Vector3.up));
						}
						if (base.Holder && base.Holder == CharacterMainControl.Main)
						{
							Vector3 a = base.Holder.modelRoot.right;
							a += UnityEngine.Random.insideUnitSphere * 0.3f;
							a.Normalize();
							CameraShaker.Shake(a * 0.05f, CameraShaker.CameraShakeTypes.meleeAttackHit);
						}
					}
				}
			}
			IL_2B3:;
		}
		return num2;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x000227CC File Offset: 0x000209CC
	private void Update()
	{
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x000227CE File Offset: 0x000209CE
	protected override void OnInitialize()
	{
		base.OnInitialize();
		this.setting = base.Item.GetComponent<ItemSetting_MeleeWeapon>();
	}

	// Token: 0x0400072F RID: 1839
	public GameObject hitFx;

	// Token: 0x04000730 RID: 1840
	public GameObject slashFx;

	// Token: 0x04000731 RID: 1841
	public float slashFxDelayTime = 0.05f;

	// Token: 0x04000732 RID: 1842
	[SerializeField]
	private string soundKey = "Default";

	// Token: 0x04000733 RID: 1843
	private Collider[] colliders;

	// Token: 0x04000734 RID: 1844
	private ItemSetting_MeleeWeapon setting;

	// Token: 0x04000735 RID: 1845
	private static int DamageHash = "Damage".GetHashCode();

	// Token: 0x04000736 RID: 1846
	private static int CritRateHash = "CritRate".GetHashCode();

	// Token: 0x04000737 RID: 1847
	private static int CritDamageFactorHash = "CritDamageFactor".GetHashCode();

	// Token: 0x04000738 RID: 1848
	private static int ArmorPiercingHash = "ArmorPiercing".GetHashCode();

	// Token: 0x04000739 RID: 1849
	private static int AttackSpeedHash = "AttackSpeed".GetHashCode();

	// Token: 0x0400073A RID: 1850
	private static int AttackRangeHash = "AttackRange".GetHashCode();

	// Token: 0x0400073B RID: 1851
	private static int DealDamageTimeHash = "DealDamageTime".GetHashCode();

	// Token: 0x0400073C RID: 1852
	private static int StaminaCostHash = "StaminaCost".GetHashCode();

	// Token: 0x0400073D RID: 1853
	private static int BleedChanceHash = "BleedChance".GetHashCode();

	// Token: 0x0400073E RID: 1854
	private static int MoveSpeedMultiplierHash = "MoveSpeedMultiplier".GetHashCode();
}
