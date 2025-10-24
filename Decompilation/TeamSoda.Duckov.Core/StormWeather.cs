using System;
using Duckov.Buffs;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class StormWeather : MonoBehaviour
{
	// Token: 0x06000BCE RID: 3022 RVA: 0x000320AC File Offset: 0x000302AC
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
		if (this.onlyOutDoor && subSceneInfo.IsInDoor)
		{
			return;
		}
		if (!this.target)
		{
			this.target = CharacterMainControl.Main;
			if (!this.target)
			{
				return;
			}
		}
		this.addBuffTimer -= Time.deltaTime;
		if (this.addBuffTimer <= 0f)
		{
			this.addBuffTimer = this.addBuffTimeSpace;
			if (this.target.StormProtection > this.stormProtectionThreshold)
			{
				return;
			}
			this.target.AddBuff(this.buff, null, 0);
		}
	}

	// Token: 0x04000A21 RID: 2593
	public Buff buff;

	// Token: 0x04000A22 RID: 2594
	public float addBuffTimeSpace = 1f;

	// Token: 0x04000A23 RID: 2595
	private float addBuffTimer;

	// Token: 0x04000A24 RID: 2596
	private CharacterMainControl target;

	// Token: 0x04000A25 RID: 2597
	private bool onlyOutDoor = true;

	// Token: 0x04000A26 RID: 2598
	public float stormProtectionThreshold = 0.9f;
}
