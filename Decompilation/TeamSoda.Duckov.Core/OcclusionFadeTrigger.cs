using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class OcclusionFadeTrigger : MonoBehaviour
{
	// Token: 0x06000BBB RID: 3003 RVA: 0x00031D12 File Offset: 0x0002FF12
	private void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer("VisualOcclusion");
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00031D29 File Offset: 0x0002FF29
	public void Enter()
	{
		this.parent.OnEnter();
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00031D36 File Offset: 0x0002FF36
	public void Leave()
	{
		this.parent.OnLeave();
	}

	// Token: 0x04000A14 RID: 2580
	public OcclusionFadeObject parent;
}
