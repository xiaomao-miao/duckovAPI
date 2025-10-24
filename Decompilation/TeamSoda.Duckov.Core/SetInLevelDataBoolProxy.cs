using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class SetInLevelDataBoolProxy : MonoBehaviour
{
	// Token: 0x060005A6 RID: 1446 RVA: 0x00019481 File Offset: 0x00017681
	public void SetToTarget()
	{
		this.SetTo(this.targetValue);
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00019490 File Offset: 0x00017690
	public void SetTo(bool target)
	{
		if (this.keyString == "")
		{
			return;
		}
		if (!this.keyInited)
		{
			this.InitKey();
		}
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.Instance.inLevelData[this.keyHash] = target;
		}
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x000194E5 File Offset: 0x000176E5
	private void InitKey()
	{
		this.keyHash = this.keyString.GetHashCode();
		this.keyInited = true;
	}

	// Token: 0x04000522 RID: 1314
	public bool targetValue = true;

	// Token: 0x04000523 RID: 1315
	public string keyString = "";

	// Token: 0x04000524 RID: 1316
	private int keyHash;

	// Token: 0x04000525 RID: 1317
	private bool keyInited;
}
