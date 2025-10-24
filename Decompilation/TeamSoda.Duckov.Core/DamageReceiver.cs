using System;
using Duckov.Buffs;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200006F RID: 111
public class DamageReceiver : MonoBehaviour
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000424 RID: 1060 RVA: 0x00012594 File Offset: 0x00010794
	public Teams Team
	{
		get
		{
			if (!this.useSimpleHealth && this.health)
			{
				return this.health.team;
			}
			if (this.useSimpleHealth && this.simpleHealth)
			{
				return this.simpleHealth.team;
			}
			return Teams.all;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000425 RID: 1061 RVA: 0x000125E4 File Offset: 0x000107E4
	public bool IsMainCharacter
	{
		get
		{
			return !this.useSimpleHealth && this.health && this.health.IsMainCharacterHealth;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000426 RID: 1062 RVA: 0x00012608 File Offset: 0x00010808
	public bool IsDead
	{
		get
		{
			return this.health && this.health.IsDead;
		}
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00012624 File Offset: 0x00010824
	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("DamageReceiver");
		if (this.health)
		{
			this.health.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00012664 File Offset: 0x00010864
	private void OnDestroy()
	{
		if (this.health)
		{
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0001268F File Offset: 0x0001088F
	public bool Hurt(DamageInfo damageInfo)
	{
		damageInfo.toDamageReceiver = this;
		UnityEvent<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent.Invoke(damageInfo);
		}
		if (this.health)
		{
			this.health.Hurt(damageInfo);
		}
		return true;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000126C8 File Offset: 0x000108C8
	public bool AddBuff(Buff buffPfb, CharacterMainControl fromWho)
	{
		if (this.useSimpleHealth)
		{
			return false;
		}
		if (!this.health)
		{
			return false;
		}
		CharacterMainControl characterMainControl = this.health.TryGetCharacter();
		if (!characterMainControl)
		{
			return false;
		}
		characterMainControl.AddBuff(buffPfb, fromWho, 0);
		return true;
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0001270E File Offset: 0x0001090E
	public void OnDead(DamageInfo dmgInfo)
	{
		base.gameObject.SetActive(false);
		UnityEvent<DamageInfo> onDeadEvent = this.OnDeadEvent;
		if (onDeadEvent == null)
		{
			return;
		}
		onDeadEvent.Invoke(dmgInfo);
	}

	// Token: 0x04000333 RID: 819
	public bool useSimpleHealth;

	// Token: 0x04000334 RID: 820
	public Health health;

	// Token: 0x04000335 RID: 821
	public HealthSimpleBase simpleHealth;

	// Token: 0x04000336 RID: 822
	public bool isHalfObsticle;

	// Token: 0x04000337 RID: 823
	public UnityEvent<DamageInfo> OnHurtEvent;

	// Token: 0x04000338 RID: 824
	public UnityEvent<DamageInfo> OnDeadEvent;
}
