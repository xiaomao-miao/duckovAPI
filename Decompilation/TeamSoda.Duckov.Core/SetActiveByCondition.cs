using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Quests;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class SetActiveByCondition : MonoBehaviour
{
	// Token: 0x060005BE RID: 1470 RVA: 0x00019AC8 File Offset: 0x00017CC8
	private void Update()
	{
		if (!LevelManager.LevelInited && this.requireLevelInited)
		{
			return;
		}
		this.Set();
		if (this.update)
		{
			this.CheckAndLoop().Forget();
		}
		base.enabled = false;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00019B08 File Offset: 0x00017D08
	public void Set()
	{
		if (this.targetObject)
		{
			bool flag = this.conditions.Satisfied();
			if (this.inverse)
			{
				flag = !flag;
			}
			this.targetObject.SetActive(flag);
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00019B48 File Offset: 0x00017D48
	private UniTaskVoid CheckAndLoop()
	{
		SetActiveByCondition.<CheckAndLoop>d__8 <CheckAndLoop>d__;
		<CheckAndLoop>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CheckAndLoop>d__.<>4__this = this;
		<CheckAndLoop>d__.<>1__state = -1;
		<CheckAndLoop>d__.<>t__builder.Start<SetActiveByCondition.<CheckAndLoop>d__8>(ref <CheckAndLoop>d__);
		return <CheckAndLoop>d__.<>t__builder.Task;
	}

	// Token: 0x0400053D RID: 1341
	public GameObject targetObject;

	// Token: 0x0400053E RID: 1342
	public bool inverse;

	// Token: 0x0400053F RID: 1343
	public bool requireLevelInited = true;

	// Token: 0x04000540 RID: 1344
	public List<Condition> conditions;

	// Token: 0x04000541 RID: 1345
	public bool update;

	// Token: 0x04000542 RID: 1346
	private float checkTimeSpace = 1f;
}
