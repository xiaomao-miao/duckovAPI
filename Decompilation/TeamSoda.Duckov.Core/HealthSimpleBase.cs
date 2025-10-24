using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000066 RID: 102
public class HealthSimpleBase : MonoBehaviour
{
	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060003C5 RID: 965 RVA: 0x00010B01 File Offset: 0x0000ED01
	public float HealthValue
	{
		get
		{
			return this.healthValue;
		}
	}

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x060003C6 RID: 966 RVA: 0x00010B0C File Offset: 0x0000ED0C
	// (remove) Token: 0x060003C7 RID: 967 RVA: 0x00010B44 File Offset: 0x0000ED44
	public event Action<DamageInfo> OnHurtEvent;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x060003C8 RID: 968 RVA: 0x00010B7C File Offset: 0x0000ED7C
	// (remove) Token: 0x060003C9 RID: 969 RVA: 0x00010BB0 File Offset: 0x0000EDB0
	public static event Action<HealthSimpleBase, DamageInfo> OnSimpleHealthHit;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060003CA RID: 970 RVA: 0x00010BE4 File Offset: 0x0000EDE4
	// (remove) Token: 0x060003CB RID: 971 RVA: 0x00010C1C File Offset: 0x0000EE1C
	public event Action<DamageInfo> OnDeadEvent;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060003CC RID: 972 RVA: 0x00010C54 File Offset: 0x0000EE54
	// (remove) Token: 0x060003CD RID: 973 RVA: 0x00010C88 File Offset: 0x0000EE88
	public static event Action<HealthSimpleBase, DamageInfo> OnSimpleHealthDead;

	// Token: 0x060003CE RID: 974 RVA: 0x00010CBB File Offset: 0x0000EEBB
	private void Awake()
	{
		this.healthValue = this.maxHealthValue;
		this.dmgReceiver.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
	}

	// Token: 0x060003CF RID: 975 RVA: 0x00010CE8 File Offset: 0x0000EEE8
	private void OnHurt(DamageInfo dmgInfo)
	{
		if (this.onlyReceiveExplosion && !dmgInfo.isExplosion)
		{
			return;
		}
		float num = 1f;
		bool flag = UnityEngine.Random.Range(0f, 1f) <= dmgInfo.critRate;
		dmgInfo.crit = (flag ? 1 : 0);
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			num = this.damageMultiplierIfNotMainCharacter;
		}
		this.healthValue -= (flag ? dmgInfo.critDamageFactor : 1f) * dmgInfo.damageValue * num;
		Action<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent(dmgInfo);
		}
		Action<HealthSimpleBase, DamageInfo> onSimpleHealthHit = HealthSimpleBase.OnSimpleHealthHit;
		if (onSimpleHealthHit != null)
		{
			onSimpleHealthHit(this, dmgInfo);
		}
		if (this.healthValue <= 0f)
		{
			this.Dead(dmgInfo);
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x00010DB4 File Offset: 0x0000EFB4
	private void Dead(DamageInfo dmgInfo)
	{
		this.dmgReceiver.OnDead(dmgInfo);
		Action<DamageInfo> onDeadEvent = this.OnDeadEvent;
		if (onDeadEvent != null)
		{
			onDeadEvent(dmgInfo);
		}
		Action<HealthSimpleBase, DamageInfo> onSimpleHealthDead = HealthSimpleBase.OnSimpleHealthDead;
		if (onSimpleHealthDead == null)
		{
			return;
		}
		onSimpleHealthDead(this, dmgInfo);
	}

	// Token: 0x040002E2 RID: 738
	public Teams team;

	// Token: 0x040002E3 RID: 739
	public bool onlyReceiveExplosion;

	// Token: 0x040002E4 RID: 740
	public float maxHealthValue = 250f;

	// Token: 0x040002E5 RID: 741
	private float healthValue;

	// Token: 0x040002E6 RID: 742
	public DamageReceiver dmgReceiver;

	// Token: 0x040002EA RID: 746
	public float damageMultiplierIfNotMainCharacter = 1f;
}
