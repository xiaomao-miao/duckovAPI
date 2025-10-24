using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class InteractMarker : MonoBehaviour
{
	// Token: 0x0600070B RID: 1803 RVA: 0x0001FBE4 File Offset: 0x0001DDE4
	public void MarkAsUsed()
	{
		if (this.markedAsUsed)
		{
			return;
		}
		this.markedAsUsed = true;
		if (this.hideIfUsedObject)
		{
			this.hideIfUsedObject.SetActive(false);
		}
		if (this.showIfUsedObject)
		{
			this.showIfUsedObject.SetActive(true);
		}
	}

	// Token: 0x040006AE RID: 1710
	private bool markedAsUsed;

	// Token: 0x040006AF RID: 1711
	public GameObject showIfUsedObject;

	// Token: 0x040006B0 RID: 1712
	public GameObject hideIfUsedObject;
}
