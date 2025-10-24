using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class MiningMachineCardDisplay : MonoBehaviour
{
	// Token: 0x06000B8B RID: 2955 RVA: 0x00030D58 File Offset: 0x0002EF58
	public void SetVisualActive(bool active, MiningMachineCardDisplay.CardTypes cardType)
	{
		this.activeVisual.SetActive(active);
		this.deactiveVisual.SetActive(!active);
		if (cardType == MiningMachineCardDisplay.CardTypes.normal)
		{
			this.normalGPU.SetActive(true);
			this.potatoGPU.SetActive(false);
			return;
		}
		if (cardType != MiningMachineCardDisplay.CardTypes.potato)
		{
			throw new ArgumentOutOfRangeException("cardType", cardType, null);
		}
		this.normalGPU.SetActive(false);
		this.potatoGPU.SetActive(true);
	}

	// Token: 0x040009D7 RID: 2519
	public GameObject activeVisual;

	// Token: 0x040009D8 RID: 2520
	public GameObject deactiveVisual;

	// Token: 0x040009D9 RID: 2521
	public GameObject normalGPU;

	// Token: 0x040009DA RID: 2522
	public GameObject potatoGPU;

	// Token: 0x020004B8 RID: 1208
	public enum CardTypes
	{
		// Token: 0x04001C82 RID: 7298
		normal,
		// Token: 0x04001C83 RID: 7299
		potato
	}
}
