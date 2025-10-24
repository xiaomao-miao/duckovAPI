using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class AISpecialAttachment_Shop : AISpecialAttachmentBase
{
	// Token: 0x060004E8 RID: 1256 RVA: 0x0001618A File Offset: 0x0001438A
	protected override void OnInited()
	{
		base.OnInited();
		this.aiCharacterController.hideIfFoundEnemy = this.shop;
	}

	// Token: 0x0400041C RID: 1052
	public GameObject shop;
}
