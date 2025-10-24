using System;
using NodeCanvas.Framework;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class GetCurrentPosition : ActionTask<Transform>
{
	// Token: 0x06000182 RID: 386 RVA: 0x00007781 File Offset: 0x00005981
	protected override void OnExecute()
	{
		if (base.agent != null)
		{
			this.positionResult.value = base.agent.position;
		}
		base.EndAction(true);
	}

	// Token: 0x04000114 RID: 276
	public BBParameter<Vector3> positionResult;
}
