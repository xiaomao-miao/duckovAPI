using System;
using Duckov.Quests;
using Duckov.Scenes;

// Token: 0x0200011D RID: 285
public class RequireInLevelDataBool : Condition
{
	// Token: 0x0600097A RID: 2426 RVA: 0x00029534 File Offset: 0x00027734
	public override bool Evaluate()
	{
		if (!MultiSceneCore.Instance)
		{
			return false;
		}
		if (!this.keyHashInited)
		{
			this.InitKeyHash();
		}
		object obj;
		return !this.isEmptyString && (MultiSceneCore.Instance.inLevelData.TryGetValue(this.keyHash, out obj) && obj is bool) && (bool)obj;
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00029590 File Offset: 0x00027790
	private void InitKeyHash()
	{
		if (this.keyString == "")
		{
			this.isEmptyString = true;
		}
		this.keyHash = this.keyString.GetHashCode();
		this.keyHashInited = true;
	}

	// Token: 0x0400085C RID: 2140
	public string keyString = "";

	// Token: 0x0400085D RID: 2141
	private int keyHash = -1;

	// Token: 0x0400085E RID: 2142
	private bool keyHashInited;

	// Token: 0x0400085F RID: 2143
	private bool isEmptyString;
}
