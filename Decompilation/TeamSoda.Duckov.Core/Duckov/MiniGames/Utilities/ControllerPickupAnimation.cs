using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x02000286 RID: 646
	public class ControllerPickupAnimation : MonoBehaviour
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0004CCA4 File Offset: 0x0004AEA4
		private AnimationCurve pickupRotCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x0004CCAC File Offset: 0x0004AEAC
		private AnimationCurve pickupPosCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x0004CCB4 File Offset: 0x0004AEB4
		private AnimationCurve putDownCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0004CCBC File Offset: 0x0004AEBC
		public UniTask PickUp(Transform endTransform)
		{
			ControllerPickupAnimation.<PickUp>d__11 <PickUp>d__;
			<PickUp>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PickUp>d__.<>4__this = this;
			<PickUp>d__.endTransform = endTransform;
			<PickUp>d__.<>1__state = -1;
			<PickUp>d__.<>t__builder.Start<ControllerPickupAnimation.<PickUp>d__11>(ref <PickUp>d__);
			return <PickUp>d__.<>t__builder.Task;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004CD08 File Offset: 0x0004AF08
		public UniTask PutDown()
		{
			ControllerPickupAnimation.<PutDown>d__12 <PutDown>d__;
			<PutDown>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PutDown>d__.<>4__this = this;
			<PutDown>d__.<>1__state = -1;
			<PutDown>d__.<>t__builder.Start<ControllerPickupAnimation.<PutDown>d__12>(ref <PutDown>d__);
			return <PutDown>d__.<>t__builder.Task;
		}

		// Token: 0x04000F2C RID: 3884
		[SerializeField]
		private Transform restTransform;

		// Token: 0x04000F2D RID: 3885
		[SerializeField]
		private Transform controllerTransform;

		// Token: 0x04000F2E RID: 3886
		[SerializeField]
		private float transitionTime = 1f;

		// Token: 0x04000F2F RID: 3887
		[SerializeField]
		private AnimationCurve pickupCurve;

		// Token: 0x04000F30 RID: 3888
		private int activeToken;
	}
}
