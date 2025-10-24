using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200040C RID: 1036
	public class MoveToRandomPos : ActionTask<AICharacterController>
	{
		// Token: 0x0600256D RID: 9581 RVA: 0x00080EA5 File Offset: 0x0007F0A5
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00080EA8 File Offset: 0x0007F0A8
		protected override void OnExecute()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			this.targetPoint = this.RandomPoint();
			base.agent.MoveToPos(this.targetPoint);
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x00080EE0 File Offset: 0x0007F0E0
		protected override void OnUpdate()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			if (base.elapsedTime > this.overTime.value)
			{
				base.EndAction(this.overTimeReturnSuccess);
				return;
			}
			if (this.useTransform && this.centerTransform.value == null)
			{
				base.EndAction(false);
				return;
			}
			if (this.syncDirectionIfNoAimTarget && base.agent.aimTarget == null)
			{
				if (this.setAimToPos && this.aimPos.isDefined)
				{
					base.agent.CharacterMainControl.SetAimPoint(this.aimPos.value);
				}
				else
				{
					Vector3 currentMoveDirection = base.agent.CharacterMainControl.CurrentMoveDirection;
					if (currentMoveDirection.magnitude > 0f)
					{
						base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + currentMoveDirection * 1000f);
					}
				}
			}
			if (!base.agent.WaitingForPathResult())
			{
				if (base.agent.ReachedEndOfPath() || !base.agent.IsMoving())
				{
					base.EndAction(true);
					return;
				}
				if (!base.agent.HasPath())
				{
					if (!this.failIfNoPath && this.retryIfNotFound)
					{
						this.targetPoint = this.RandomPoint();
						base.agent.MoveToPos(this.targetPoint);
						return;
					}
					base.EndAction(!this.failIfNoPath);
					return;
				}
			}
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x00081067 File Offset: 0x0007F267
		protected override void OnStop()
		{
			base.agent.StopMove();
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x00081074 File Offset: 0x0007F274
		protected override void OnPause()
		{
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x00081078 File Offset: 0x0007F278
		private Vector3 RandomPoint()
		{
			Vector3 a = base.agent.CharacterMainControl.transform.position;
			if (this.useTransform)
			{
				if (this.centerTransform.isDefined)
				{
					a = this.centerTransform.value.position;
				}
			}
			else
			{
				a = this.centerPos.value;
			}
			Vector3 a2 = a - base.agent.transform.position;
			a2.y = 0f;
			if (a2.magnitude < 0.1f)
			{
				a2 = UnityEngine.Random.insideUnitSphere;
				a2.y = 0f;
			}
			a2 = a2.normalized;
			float y = UnityEngine.Random.Range(-0.5f * this.randomAngle, 0.5f * this.randomAngle);
			float d = UnityEngine.Random.Range(this.avoidRadius.value, this.radius.value);
			a2 = Quaternion.Euler(0f, y, 0f) * -a2;
			return a + a2 * d;
		}

		// Token: 0x04001972 RID: 6514
		public bool useTransform;

		// Token: 0x04001973 RID: 6515
		public bool setAimToPos;

		// Token: 0x04001974 RID: 6516
		[ShowIf("setAimToPos", 1)]
		public BBParameter<Vector3> aimPos;

		// Token: 0x04001975 RID: 6517
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> centerPos;

		// Token: 0x04001976 RID: 6518
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> centerTransform;

		// Token: 0x04001977 RID: 6519
		public BBParameter<float> radius;

		// Token: 0x04001978 RID: 6520
		public BBParameter<float> avoidRadius;

		// Token: 0x04001979 RID: 6521
		public float randomAngle = 360f;

		// Token: 0x0400197A RID: 6522
		public BBParameter<float> overTime = 8f;

		// Token: 0x0400197B RID: 6523
		public bool overTimeReturnSuccess = true;

		// Token: 0x0400197C RID: 6524
		private Vector3 targetPoint;

		// Token: 0x0400197D RID: 6525
		public bool failIfNoPath;

		// Token: 0x0400197E RID: 6526
		[ShowIf("failIfNoPath", 0)]
		public bool retryIfNotFound;

		// Token: 0x0400197F RID: 6527
		public bool syncDirectionIfNoAimTarget = true;
	}
}
