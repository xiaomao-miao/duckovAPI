using System;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class SetActiveOnAwake : MonoBehaviour
{
	// Token: 0x06000A64 RID: 2660 RVA: 0x0002DAC8 File Offset: 0x0002BCC8
	private void Awake()
	{
		this.target.SetActive(true);
	}

	// Token: 0x04000918 RID: 2328
	public GameObject target;
}
