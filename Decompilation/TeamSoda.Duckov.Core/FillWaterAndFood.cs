using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class FillWaterAndFood : MonoBehaviour
{
	// Token: 0x06000554 RID: 1364 RVA: 0x00017D18 File Offset: 0x00015F18
	public void Fill()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (!main)
		{
			return;
		}
		main.AddWater(this.water);
		main.AddEnergy(this.food);
	}

	// Token: 0x040004C7 RID: 1223
	public float water;

	// Token: 0x040004C8 RID: 1224
	public float food;
}
