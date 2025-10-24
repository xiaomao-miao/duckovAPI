using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
[Serializable]
public struct EvacuationInfo
{
	// Token: 0x06000914 RID: 2324 RVA: 0x0002859A File Offset: 0x0002679A
	public EvacuationInfo(string subsceneID, Vector3 position)
	{
		this.subsceneID = subsceneID;
		this.position = position;
	}

	// Token: 0x04000829 RID: 2089
	public string subsceneID;

	// Token: 0x0400082A RID: 2090
	public Vector3 position;
}
