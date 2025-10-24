using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class SetActiveByEnding : MonoBehaviour
{
	// Token: 0x06000BC6 RID: 3014 RVA: 0x00031EF5 File Offset: 0x000300F5
	private void Start()
	{
		this.target.SetActive(this.endingIndexs.Contains(Ending.endingIndex));
	}

	// Token: 0x04000A1C RID: 2588
	public GameObject target;

	// Token: 0x04000A1D RID: 2589
	public List<int> endingIndexs;
}
