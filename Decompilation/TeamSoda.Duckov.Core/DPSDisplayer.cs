using System;
using TMPro;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class DPSDisplayer : MonoBehaviour
{
	// Token: 0x06000B6D RID: 2925 RVA: 0x000305F5 File Offset: 0x0002E7F5
	private void Awake()
	{
		Health.OnHurt += this.OnHurt;
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00030608 File Offset: 0x0002E808
	private void Update()
	{
		if (Time.time - this.lastTimeMarker > 3f)
		{
			this.empty = true;
			this.totalDamage = 0f;
			this.RefreshDisplay();
		}
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00030635 File Offset: 0x0002E835
	private void OnDestroy()
	{
		Health.OnHurt -= this.OnHurt;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00030648 File Offset: 0x0002E848
	private void OnHurt(Health health, DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		this.totalDamage += dmgInfo.finalDamage;
		if (this.empty)
		{
			this.firstTimeMarker = Time.time;
			this.lastTimeMarker = Time.time;
			this.empty = false;
			return;
		}
		this.lastTimeMarker = Time.time;
		this.RefreshDisplay();
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x000306BC File Offset: 0x0002E8BC
	private void RefreshDisplay()
	{
		float num = this.CalculateDPS();
		this.dpsText.text = num.ToString("00000");
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x000306E8 File Offset: 0x0002E8E8
	private float CalculateDPS()
	{
		if (this.empty)
		{
			return 0f;
		}
		float num = this.lastTimeMarker - this.firstTimeMarker;
		if (num <= 0f)
		{
			return 0f;
		}
		return this.totalDamage / num;
	}

	// Token: 0x040009BB RID: 2491
	[SerializeField]
	private TextMeshPro dpsText;

	// Token: 0x040009BC RID: 2492
	private bool empty;

	// Token: 0x040009BD RID: 2493
	private float totalDamage;

	// Token: 0x040009BE RID: 2494
	private float firstTimeMarker;

	// Token: 0x040009BF RID: 2495
	private float lastTimeMarker;
}
