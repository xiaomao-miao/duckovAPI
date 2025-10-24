using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000417 RID: 1047
	public class Shoot : ActionTask<AICharacterController>
	{
		// Token: 0x060025B3 RID: 9651 RVA: 0x00081D49 File Offset: 0x0007FF49
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x00081D4C File Offset: 0x0007FF4C
		protected override string info
		{
			get
			{
				return string.Format("Shoot {0}to{1} sec.", this.shootTimeRange.value.x, this.shootTimeRange.value.y);
			}
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x00081D84 File Offset: 0x0007FF84
		protected override void OnExecute()
		{
			this.semiTimer = this.semiTimeSpace;
			base.agent.CharacterMainControl.Trigger(true, true, false);
			if (!base.agent.shootCanMove)
			{
				base.agent.StopMove();
			}
			this.shootTime = UnityEngine.Random.Range(this.shootTimeRange.value.x, this.shootTimeRange.value.y);
			if (this.shootTime <= 0f)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x00081E08 File Offset: 0x00080008
		protected override void OnUpdate()
		{
			bool triggerThisFrame = false;
			this.semiTimer += Time.deltaTime;
			if (!base.agent.shootCanMove)
			{
				base.agent.StopMove();
			}
			if (this.semiTimer >= this.semiTimeSpace)
			{
				this.semiTimer = 0f;
				triggerThisFrame = true;
			}
			base.agent.CharacterMainControl.Trigger(true, triggerThisFrame, false);
			if (base.elapsedTime >= this.shootTime)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x00081E84 File Offset: 0x00080084
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x00081E99 File Offset: 0x00080099
		protected override void OnPause()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x040019A3 RID: 6563
		public BBParameter<Vector2> shootTimeRange;

		// Token: 0x040019A4 RID: 6564
		private float shootTime;

		// Token: 0x040019A5 RID: 6565
		public float semiTimeSpace = 0.35f;

		// Token: 0x040019A6 RID: 6566
		private float semiTimer;
	}
}
