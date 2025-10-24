using System;
using Duckov.Quests;
using Saves;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class RequireSaveDataBool : Condition
{
	// Token: 0x0600097D RID: 2429 RVA: 0x000295E0 File Offset: 0x000277E0
	public override bool Evaluate()
	{
		bool flag = SavesSystem.Load<bool>(this.key);
		Debug.Log(string.Format("Load bool:{0}  value:{1}", this.key, flag));
		return flag == this.requireValue;
	}

	// Token: 0x04000860 RID: 2144
	[SerializeField]
	private string key;

	// Token: 0x04000861 RID: 2145
	[SerializeField]
	private bool requireValue;
}
