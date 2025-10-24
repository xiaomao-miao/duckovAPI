using System;
using System.Collections.Generic;
using Duckov.Options;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class HideIfSickFriendly : MonoBehaviour
{
	// Token: 0x060001DE RID: 478 RVA: 0x00009395 File Offset: 0x00007595
	private void Start()
	{
		this.Sync();
		OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x000093AE File Offset: 0x000075AE
	private void OnDestroy()
	{
		OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x000093C1 File Offset: 0x000075C1
	private void OnOptionsChanged(string option)
	{
		this.Sync();
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x000093CC File Offset: 0x000075CC
	private void Sync()
	{
		bool disableCameraOffset = DisableCameraOffset.disableCameraOffset;
		if (this.sickFriendly != disableCameraOffset)
		{
			this.sickFriendly = disableCameraOffset;
		}
		foreach (GameObject gameObject in this.hideList)
		{
			if (gameObject)
			{
				gameObject.SetActive(!this.sickFriendly);
			}
		}
	}

	// Token: 0x0400019B RID: 411
	public List<GameObject> hideList = new List<GameObject>();

	// Token: 0x0400019C RID: 412
	private bool sickFriendly;
}
