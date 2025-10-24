using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000B7 RID: 183
[RequireComponent(typeof(Zone))]
public class ZoneDamage : MonoBehaviour
{
	// Token: 0x060005F3 RID: 1523 RVA: 0x0001A6E9 File Offset: 0x000188E9
	private void Start()
	{
		if (this.zone == null)
		{
			this.zone = base.GetComponent<Zone>();
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0001A708 File Offset: 0x00018908
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > this.timeSpace)
		{
			this.timer %= this.timeSpace;
			this.Damage();
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0001A758 File Offset: 0x00018958
	private void Damage()
	{
		foreach (Health health in this.zone.Healths)
		{
			CharacterMainControl characterMainControl = health.TryGetCharacter();
			if (!(characterMainControl == null))
			{
				if (this.checkGasMask && characterMainControl.HasGasMask)
				{
					Item faceMaskItem = characterMainControl.GetFaceMaskItem();
					if (faceMaskItem && faceMaskItem.GetStat(this.hasMaskHash) != null)
					{
						faceMaskItem.Durability -= 0.1f * this.timeSpace;
					}
				}
				else if ((!this.checkElecProtection || characterMainControl.CharacterItem.GetStat(this.elecProtectionHash).Value <= 0.99f) && (!this.checkFireProtection || characterMainControl.CharacterItem.GetStat(this.fireProtectionHash).Value <= 0.99f))
				{
					this.damageInfo.fromCharacter = null;
					this.damageInfo.damagePoint = health.transform.position + Vector3.up * 0.5f;
					this.damageInfo.damageNormal = Vector3.up;
					health.Hurt(this.damageInfo);
				}
			}
		}
	}

	// Token: 0x04000576 RID: 1398
	public Zone zone;

	// Token: 0x04000577 RID: 1399
	public float timeSpace = 0.5f;

	// Token: 0x04000578 RID: 1400
	private float timer;

	// Token: 0x04000579 RID: 1401
	public DamageInfo damageInfo;

	// Token: 0x0400057A RID: 1402
	public bool checkGasMask;

	// Token: 0x0400057B RID: 1403
	public bool checkElecProtection;

	// Token: 0x0400057C RID: 1404
	public bool checkFireProtection;

	// Token: 0x0400057D RID: 1405
	private int hasMaskHash = "GasMask".GetHashCode();

	// Token: 0x0400057E RID: 1406
	private int elecProtectionHash = "ElecProtection".GetHashCode();

	// Token: 0x0400057F RID: 1407
	private int fireProtectionHash = "FireProtection".GetHashCode();
}
