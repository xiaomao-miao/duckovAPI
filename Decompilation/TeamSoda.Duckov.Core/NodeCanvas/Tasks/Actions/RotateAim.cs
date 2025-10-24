using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000412 RID: 1042
	public class RotateAim : ActionTask<AICharacterController>
	{
		// Token: 0x06002592 RID: 9618 RVA: 0x000817A6 File Offset: 0x0007F9A6
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000817AC File Offset: 0x0007F9AC
		protected override void OnExecute()
		{
			this.time = UnityEngine.Random.Range(this.timeRange.value.x, this.timeRange.value.y);
			this.startDir = base.agent.CharacterMainControl.CurrentAimDirection;
			base.agent.SetTarget(null);
			if (this.shoot)
			{
				base.agent.CharacterMainControl.Trigger(true, true, false);
			}
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x00081824 File Offset: 0x0007FA24
		protected override void OnUpdate()
		{
			this.currentAngle = this.angle * base.elapsedTime / this.time;
			Vector3 a = Quaternion.Euler(0f, this.currentAngle, 0f) * this.startDir;
			base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + a * 100f);
			if (this.shoot)
			{
				base.agent.CharacterMainControl.Trigger(true, true, false);
			}
			if (base.elapsedTime > this.time)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000818D1 File Offset: 0x0007FAD1
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000818E6 File Offset: 0x0007FAE6
		protected override void OnPause()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x04001989 RID: 6537
		private Vector3 startDir;

		// Token: 0x0400198A RID: 6538
		public float angle;

		// Token: 0x0400198B RID: 6539
		private float currentAngle;

		// Token: 0x0400198C RID: 6540
		public BBParameter<Vector2> timeRange;

		// Token: 0x0400198D RID: 6541
		private float time;

		// Token: 0x0400198E RID: 6542
		public bool shoot;
	}
}
