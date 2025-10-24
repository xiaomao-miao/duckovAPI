using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class FormulasRegisterViewInvoker : InteractableBase
{
	// Token: 0x06000CED RID: 3309 RVA: 0x00035CD1 File Offset: 0x00033ED1
	protected override void Awake()
	{
		base.Awake();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00035CE0 File Offset: 0x00033EE0
	protected override void OnInteractFinished()
	{
		FormulasRegisterView.Show(this.additionalTags);
	}

	// Token: 0x04000B25 RID: 2853
	[SerializeField]
	private List<Tag> additionalTags;
}
