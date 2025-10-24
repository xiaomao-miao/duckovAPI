using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200040A RID: 1034
	public class Dash : ActionTask<AICharacterController>
	{
		// Token: 0x06002561 RID: 9569 RVA: 0x00080C65 File Offset: 0x0007EE65
		protected override string OnInit()
		{
			this.dashTimeSpace = UnityEngine.Random.Range(this.dashTimeSpaceRange.value.x, this.dashTimeSpaceRange.value.y);
			return null;
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002562 RID: 9570 RVA: 0x00080C93 File Offset: 0x0007EE93
		protected override string info
		{
			get
			{
				return string.Format("Dash", Array.Empty<object>());
			}
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x00080CA4 File Offset: 0x0007EEA4
		protected override void OnExecute()
		{
			if (Time.time - this.lastDashTime < this.dashTimeSpace)
			{
				base.EndAction();
				return;
			}
			this.lastDashTime = Time.time;
			this.dashTimeSpace = UnityEngine.Random.Range(this.dashTimeSpaceRange.value.x, this.dashTimeSpaceRange.value.y);
			Vector3 vector = Vector3.forward;
			Dash.DashDirectionModes dashDirectionModes = this.directionMode;
			if (dashDirectionModes != Dash.DashDirectionModes.random)
			{
				if (dashDirectionModes == Dash.DashDirectionModes.targetTransform)
				{
					if (this.targetTransform.value == null)
					{
						base.EndAction();
						return;
					}
					vector = this.targetTransform.value.position - base.agent.transform.position;
					vector.y = 0f;
					vector.Normalize();
					if (this.verticle)
					{
						vector = Vector3.Cross(vector, Vector3.up) * ((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? 1f : -1f);
					}
				}
			}
			else
			{
				vector = UnityEngine.Random.insideUnitCircle;
				vector.z = vector.y;
				vector.y = 0f;
				vector.Normalize();
			}
			base.agent.CharacterMainControl.SetMoveInput(vector);
			base.agent.CharacterMainControl.Dash();
			base.EndAction(true);
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x00080E01 File Offset: 0x0007F001
		protected override void OnStop()
		{
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x00080E03 File Offset: 0x0007F003
		protected override void OnPause()
		{
		}

		// Token: 0x0400196B RID: 6507
		public Dash.DashDirectionModes directionMode;

		// Token: 0x0400196C RID: 6508
		[ShowIf("directionMode", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x0400196D RID: 6509
		[ShowIf("directionMode", 1)]
		public bool verticle;

		// Token: 0x0400196E RID: 6510
		public BBParameter<Vector2> dashTimeSpaceRange;

		// Token: 0x0400196F RID: 6511
		private float dashTimeSpace;

		// Token: 0x04001970 RID: 6512
		private float lastDashTime = -999f;

		// Token: 0x02000670 RID: 1648
		public enum DashDirectionModes
		{
			// Token: 0x0400232C RID: 9004
			random,
			// Token: 0x0400232D RID: 9005
			targetTransform
		}
	}
}
