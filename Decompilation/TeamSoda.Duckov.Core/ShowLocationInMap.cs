using System;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class ShowLocationInMap : MonoBehaviour
{
	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0002A24E File Offset: 0x0002844E
	public string DisplayName
	{
		get
		{
			return this.displayName;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0002A256 File Offset: 0x00028456
	public string DisplayNameRaw
	{
		get
		{
			return this.displayName;
		}
	}

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private string displayName;
}
