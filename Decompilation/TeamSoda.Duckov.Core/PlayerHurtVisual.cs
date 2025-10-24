using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x0200018A RID: 394
public class PlayerHurtVisual : MonoBehaviour
{
	// Token: 0x06000BBF RID: 3007 RVA: 0x00031D4C File Offset: 0x0002FF4C
	private void Update()
	{
		if (!this.inited)
		{
			this.TryInit();
			return;
		}
		this.value = Mathf.MoveTowards(this.value, 0f, Time.deltaTime * this.speed);
		if (this.volume.weight != this.value)
		{
			this.volume.weight = this.value;
		}
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00031DB0 File Offset: 0x0002FFB0
	private void TryInit()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (!main)
		{
			return;
		}
		this.mainCharacterHealth = main.Health;
		if (!this.mainCharacterHealth)
		{
			return;
		}
		this.mainCharacterHealth.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
		this.inited = true;
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00031E11 File Offset: 0x00030011
	private void OnDestroy()
	{
		if (this.mainCharacterHealth)
		{
			this.mainCharacterHealth.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
		}
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00031E3C File Offset: 0x0003003C
	private void OnHurt(DamageInfo dmgInfo)
	{
		if (dmgInfo.damageValue < 1.5f)
		{
			return;
		}
		if (!this.mainCharacterHealth || !PlayerHurtVisual.hurtVisualOn)
		{
			this.value = 0f;
		}
		else if (this.mainCharacterHealth.CurrentHealth / this.mainCharacterHealth.MaxHealth > 0.5f)
		{
			this.value = 0.5f;
		}
		else
		{
			this.value = 1f;
		}
		if (this.volume.weight != this.value)
		{
			this.volume.weight = this.value;
		}
	}

	// Token: 0x04000A15 RID: 2581
	[SerializeField]
	private Volume volume;

	// Token: 0x04000A16 RID: 2582
	[SerializeField]
	private float speed = 4f;

	// Token: 0x04000A17 RID: 2583
	private Health mainCharacterHealth;

	// Token: 0x04000A18 RID: 2584
	private bool inited;

	// Token: 0x04000A19 RID: 2585
	public static bool hurtVisualOn = true;

	// Token: 0x04000A1A RID: 2586
	private float value;
}
