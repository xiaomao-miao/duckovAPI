using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041A RID: 1050
	public class TraceTarget : ActionTask<AICharacterController>
	{
		// Token: 0x060025C4 RID: 9668 RVA: 0x00081FBA File Offset: 0x000801BA
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00081FC0 File Offset: 0x000801C0
		protected override void OnExecute()
		{
			if (base.agent == null || (this.traceTargetTransform && this.centerTransform.value == null))
			{
				base.EndAction(false);
				return;
			}
			Vector3 pos = this.traceTargetTransform ? this.centerTransform.value.position : this.centerPosition.value;
			base.agent.MoveToPos(pos);
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x00082030 File Offset: 0x00080230
		protected override void OnUpdate()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			Vector3 vector = (this.traceTargetTransform && this.centerTransform.value != null) ? this.centerTransform.value.position : this.centerPosition.value;
			if (base.elapsedTime > this.overTime.value)
			{
				base.EndAction(this.overTimeReturnSuccess);
				return;
			}
			if (Vector3.Distance(vector, base.agent.transform.position) < this.stopDistance.value)
			{
				base.EndAction(true);
				return;
			}
			this.recalculatePathTimer -= Time.deltaTime;
			if (this.recalculatePathTimer <= 0f)
			{
				this.recalculatePathTimer = this.recalculatePathTimeSpace;
				base.agent.MoveToPos(vector);
			}
			else if (!base.agent.WaitingForPathResult())
			{
				if (!base.agent.IsMoving() || base.agent.ReachedEndOfPath())
				{
					base.EndAction(true);
					return;
				}
				if (!base.agent.HasPath())
				{
					if (!this.failIfNoPath && this.retryIfNotFound)
					{
						base.agent.MoveToPos(vector);
						return;
					}
					base.EndAction(!this.failIfNoPath);
					return;
				}
			}
			if (this.syncDirectionIfNoAimTarget && base.agent.aimTarget == null)
			{
				Vector3 currentMoveDirection = base.agent.CharacterMainControl.CurrentMoveDirection;
				if (currentMoveDirection.magnitude > 0f)
				{
					base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + currentMoveDirection * 1000f);
				}
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000821E8 File Offset: 0x000803E8
		protected override void OnStop()
		{
			base.agent.StopMove();
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000821F5 File Offset: 0x000803F5
		protected override void OnPause()
		{
		}

		// Token: 0x040019A8 RID: 6568
		public bool traceTargetTransform = true;

		// Token: 0x040019A9 RID: 6569
		[ShowIf("traceTargetTransform", 0)]
		public BBParameter<Vector3> centerPosition;

		// Token: 0x040019AA RID: 6570
		[ShowIf("traceTargetTransform", 1)]
		public BBParameter<Transform> centerTransform;

		// Token: 0x040019AB RID: 6571
		public BBParameter<float> stopDistance;

		// Token: 0x040019AC RID: 6572
		public BBParameter<float> overTime = 8f;

		// Token: 0x040019AD RID: 6573
		public bool overTimeReturnSuccess = true;

		// Token: 0x040019AE RID: 6574
		private Vector3 targetPoint;

		// Token: 0x040019AF RID: 6575
		public bool failIfNoPath;

		// Token: 0x040019B0 RID: 6576
		[ShowIf("failIfNoPath", 0)]
		public bool retryIfNotFound;

		// Token: 0x040019B1 RID: 6577
		private float recalculatePathTimeSpace = 0.15f;

		// Token: 0x040019B2 RID: 6578
		private float recalculatePathTimer = 0.15f;

		// Token: 0x040019B3 RID: 6579
		public bool syncDirectionIfNoAimTarget = true;
	}
}
