using System;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class UIInputEventData
{
	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0002FE97 File Offset: 0x0002E097
	public bool Used
	{
		get
		{
			return this.used;
		}
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0002FE9F File Offset: 0x0002E09F
	public void Use()
	{
		this.used = true;
	}

	// Token: 0x0400099B RID: 2459
	private bool used;

	// Token: 0x0400099C RID: 2460
	public Vector2 vector;

	// Token: 0x0400099D RID: 2461
	public bool confirm;

	// Token: 0x0400099E RID: 2462
	public bool cancel;
}
