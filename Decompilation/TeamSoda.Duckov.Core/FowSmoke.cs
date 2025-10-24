using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A5 RID: 165
public class FowSmoke : MonoBehaviour
{
	// Token: 0x060005A2 RID: 1442 RVA: 0x000193F0 File Offset: 0x000175F0
	private void Start()
	{
		this.UpdateSmoke().Forget();
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0001940C File Offset: 0x0001760C
	private UniTaskVoid UpdateSmoke()
	{
		FowSmoke.<UpdateSmoke>d__11 <UpdateSmoke>d__;
		<UpdateSmoke>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<UpdateSmoke>d__.<>4__this = this;
		<UpdateSmoke>d__.<>1__state = -1;
		<UpdateSmoke>d__.<>t__builder.Start<FowSmoke.<UpdateSmoke>d__11>(ref <UpdateSmoke>d__);
		return <UpdateSmoke>d__.<>t__builder.Task;
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0001944F File Offset: 0x0001764F
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}

	// Token: 0x04000518 RID: 1304
	[SerializeField]
	private int res = 8;

	// Token: 0x04000519 RID: 1305
	[SerializeField]
	private float radius;

	// Token: 0x0400051A RID: 1306
	[SerializeField]
	private float height;

	// Token: 0x0400051B RID: 1307
	[SerializeField]
	private float thickness;

	// Token: 0x0400051C RID: 1308
	public Transform colParent;

	// Token: 0x0400051D RID: 1309
	public ParticleSystem[] particles;

	// Token: 0x0400051E RID: 1310
	public float startTime;

	// Token: 0x0400051F RID: 1311
	public float lifeTime;

	// Token: 0x04000520 RID: 1312
	public float particleFadeTime = 3f;

	// Token: 0x04000521 RID: 1313
	public UnityEvent beforeFadeOutEvent;
}
