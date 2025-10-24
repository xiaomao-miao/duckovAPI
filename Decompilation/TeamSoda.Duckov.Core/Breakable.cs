using System;
using Saves;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class Breakable : MonoBehaviour
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060001EC RID: 492 RVA: 0x0000962C File Offset: 0x0000782C
	public string SaveKey
	{
		get
		{
			return "Breakable_" + this.saveKey;
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00009640 File Offset: 0x00007840
	private void Awake()
	{
		this.normalVisual.SetActive(true);
		if (this.dangerVisual)
		{
			this.dangerVisual.SetActive(false);
		}
		if (this.breakedVisual)
		{
			this.breakedVisual.SetActive(false);
		}
		this.simpleHealth.OnHurtEvent += this.OnHurt;
		this.simpleHealth.OnDeadEvent += this.OnDead;
		bool flag = false;
		if (this.save)
		{
			flag = SavesSystem.Load<bool>(this.SaveKey);
		}
		if (flag)
		{
			this.breakableState = Breakable.BreakableStates.danger;
			this.normalVisual.SetActive(false);
			if (this.dangerVisual)
			{
				this.dangerVisual.SetActive(false);
			}
			if (this.breakedVisual)
			{
				this.breakedVisual.SetActive(true);
			}
			if (this.simpleHealth && this.simpleHealth.dmgReceiver)
			{
				this.simpleHealth.dmgReceiver.gameObject.SetActive(false);
				return;
			}
		}
		else if (this.mainCollider)
		{
			this.mainCollider.SetActive(true);
		}
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00009766 File Offset: 0x00007966
	private void OnValidate()
	{
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00009768 File Offset: 0x00007968
	private void OnHurt(DamageInfo dmgInfo)
	{
		switch (this.breakableState)
		{
		case Breakable.BreakableStates.normal:
			if (this.simpleHealth.HealthValue <= (float)this.dangerHealth)
			{
				this.breakableState = Breakable.BreakableStates.danger;
				if (this.dangerVisual)
				{
					this.normalVisual.SetActive(false);
					this.dangerVisual.SetActive(true);
				}
				if (this.dangerFx)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.dangerFx, base.transform.position, base.transform.rotation);
				}
			}
			break;
		case Breakable.BreakableStates.danger:
		case Breakable.BreakableStates.breaked:
			break;
		default:
			return;
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00009800 File Offset: 0x00007A00
	private void OnDead(DamageInfo dmgInfo)
	{
		this.explosionDamageInfo.fromCharacter = dmgInfo.fromCharacter;
		this.normalVisual.SetActive(false);
		if (this.dangerVisual)
		{
			this.dangerVisual.SetActive(false);
		}
		if (this.breakedVisual)
		{
			this.breakedVisual.SetActive(true);
		}
		if (this.mainCollider)
		{
			this.mainCollider.SetActive(false);
		}
		this.breakableState = Breakable.BreakableStates.breaked;
		if (this.createExplosion)
		{
			LevelManager.Instance.ExplosionManager.CreateExplosion(base.transform.position, this.explosionRadius, this.explosionDamageInfo, ExplosionFxTypes.normal, 1f, true);
		}
		if (this.save)
		{
			SavesSystem.Save<bool>("Breakable_", this.saveKey, true);
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x000098CA File Offset: 0x00007ACA
	private void OnDrawGizmosSelected()
	{
		if (this.createExplosion)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, this.explosionRadius);
		}
	}

	// Token: 0x040001A6 RID: 422
	public bool save;

	// Token: 0x040001A7 RID: 423
	public string saveKey;

	// Token: 0x040001A8 RID: 424
	public HealthSimpleBase simpleHealth;

	// Token: 0x040001A9 RID: 425
	public int dangerHealth = 50;

	// Token: 0x040001AA RID: 426
	public bool createExplosion;

	// Token: 0x040001AB RID: 427
	public float explosionRadius;

	// Token: 0x040001AC RID: 428
	public DamageInfo explosionDamageInfo;

	// Token: 0x040001AD RID: 429
	private Breakable.BreakableStates breakableState;

	// Token: 0x040001AE RID: 430
	public GameObject normalVisual;

	// Token: 0x040001AF RID: 431
	public GameObject dangerVisual;

	// Token: 0x040001B0 RID: 432
	public GameObject breakedVisual;

	// Token: 0x040001B1 RID: 433
	public GameObject mainCollider;

	// Token: 0x040001B2 RID: 434
	public GameObject dangerFx;

	// Token: 0x0200042D RID: 1069
	private enum BreakableStates
	{
		// Token: 0x04001A0A RID: 6666
		normal,
		// Token: 0x04001A0B RID: 6667
		danger,
		// Token: 0x04001A0C RID: 6668
		breaked
	}
}
