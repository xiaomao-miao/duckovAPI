using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000409 RID: 1033
	public class CheckObsticle : ActionTask<AICharacterController>
	{
		// Token: 0x06002559 RID: 9561 RVA: 0x00080AA4 File Offset: 0x0007ECA4
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x00080AA8 File Offset: 0x0007ECA8
		protected override void OnExecute()
		{
			this.isHurtSearch = false;
			DamageInfo damageInfo = default(DamageInfo);
			if (base.agent.IsHurt(1.5f, 1, ref damageInfo) && damageInfo.fromCharacter && damageInfo.fromCharacter.mainDamageReceiver)
			{
				this.isHurtSearch = true;
			}
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x00080B00 File Offset: 0x0007ED00
		private void Check()
		{
			this.waitingResult = true;
			Vector3 vector = this.useTransform ? this.targetTransform.value.position : this.targetPoint.value;
			vector += Vector3.up * 0.4f;
			Vector3 start = base.agent.transform.position + Vector3.up * 0.4f;
			ItemAgent_Gun gun = base.agent.CharacterMainControl.GetGun();
			if (gun && gun.muzzle)
			{
				start = gun.muzzle.position - gun.muzzle.forward * 0.1f;
			}
			LevelManager.Instance.AIMainBrain.AddCheckObsticleTask(start, vector, base.agent.CharacterMainControl.ThermalOn, this.isHurtSearch, new Action<bool>(this.OnCheckFinished));
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x00080BF4 File Offset: 0x0007EDF4
		private void OnCheckFinished(bool result)
		{
			if (base.agent.gameObject == null)
			{
				return;
			}
			base.agent.hasObsticleToTarget = result;
			this.waitingResult = false;
			if (base.isRunning)
			{
				base.EndAction(this.alwaysSuccess || result);
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x00080C42 File Offset: 0x0007EE42
		protected override void OnUpdate()
		{
			if (!this.waitingResult)
			{
				this.Check();
			}
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x00080C52 File Offset: 0x0007EE52
		protected override void OnStop()
		{
			this.waitingResult = false;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x00080C5B File Offset: 0x0007EE5B
		protected override void OnPause()
		{
		}

		// Token: 0x04001965 RID: 6501
		public bool useTransform;

		// Token: 0x04001966 RID: 6502
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x04001967 RID: 6503
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> targetPoint;

		// Token: 0x04001968 RID: 6504
		public bool alwaysSuccess;

		// Token: 0x04001969 RID: 6505
		private bool waitingResult;

		// Token: 0x0400196A RID: 6506
		private bool isHurtSearch;
	}
}
