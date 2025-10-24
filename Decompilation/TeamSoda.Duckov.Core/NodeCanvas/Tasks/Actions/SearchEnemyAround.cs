using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000413 RID: 1043
	public class SearchEnemyAround : ActionTask<AICharacterController>
	{
		// Token: 0x06002598 RID: 9624 RVA: 0x00081903 File Offset: 0x0007FB03
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x00081908 File Offset: 0x0007FB08
		protected override void OnExecute()
		{
			DamageInfo damageInfo = default(DamageInfo);
			this.isHurtSearch = false;
			if (base.agent.IsHurt(1.5f, 1, ref damageInfo) && damageInfo.fromCharacter && damageInfo.fromCharacter.mainDamageReceiver)
			{
				this.isHurtSearch = true;
			}
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x00081960 File Offset: 0x0007FB60
		private void Search()
		{
			this.waitingSearchResult = true;
			float num = this.useSight ? base.agent.sightAngle : this.searchAngle.value;
			float num2 = this.useSight ? (base.agent.sightDistance * this.sightDistanceMultiplier.value) : this.searchDistance.value;
			if (this.isHurtSearch)
			{
				num2 *= 2f;
			}
			if (this.affactByNightVisionAbility && base.agent.CharacterMainControl)
			{
				float nightVisionAbility = base.agent.CharacterMainControl.NightVisionAbility;
				num *= Mathf.Lerp(TimeOfDayController.NightViewAngleFactor, 1f, nightVisionAbility);
			}
			bool flag = this.useSight || this.checkObsticle;
			this.searchStartTimeMarker = Time.time;
			bool thermalOn = base.agent.CharacterMainControl.ThermalOn;
			LevelManager.Instance.AIMainBrain.AddSearchTask(base.agent.transform.position + Vector3.up * 1.5f, base.agent.CharacterMainControl.CurrentAimDirection, num, num2, base.agent.CharacterMainControl.Team, flag, thermalOn, this.isHurtSearch, this.searchPickup ? base.agent.wantItem : -1, new Action<DamageReceiver, InteractablePickup>(this.OnSearchFinished));
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00081AC0 File Offset: 0x0007FCC0
		private void OnSearchFinished(DamageReceiver dmgReceiver, InteractablePickup pickup)
		{
			if (base.agent.gameObject == null)
			{
				return;
			}
			float time = Time.time;
			float num = this.searchStartTimeMarker;
			if (dmgReceiver != null)
			{
				this.result.value = dmgReceiver;
			}
			else if (this.setNullIfNotFound)
			{
				this.result.value = null;
			}
			if (pickup != null)
			{
				this.pickupResult.value = pickup;
			}
			this.waitingSearchResult = false;
			if (base.isRunning)
			{
				base.EndAction(this.alwaysSuccess || this.result.value != null || this.pickupResult != null);
			}
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00081B6D File Offset: 0x0007FD6D
		protected override void OnUpdate()
		{
			if (!this.waitingSearchResult)
			{
				this.Search();
			}
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00081B7D File Offset: 0x0007FD7D
		protected override void OnStop()
		{
			this.waitingSearchResult = false;
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00081B86 File Offset: 0x0007FD86
		protected override void OnPause()
		{
		}

		// Token: 0x0400198F RID: 6543
		public bool useSight;

		// Token: 0x04001990 RID: 6544
		public bool affactByNightVisionAbility;

		// Token: 0x04001991 RID: 6545
		[ShowIf("useSight", 0)]
		public BBParameter<float> searchAngle = 180f;

		// Token: 0x04001992 RID: 6546
		[ShowIf("useSight", 0)]
		public BBParameter<float> searchDistance;

		// Token: 0x04001993 RID: 6547
		[ShowIf("useSight", 1)]
		public BBParameter<float> sightDistanceMultiplier = 1f;

		// Token: 0x04001994 RID: 6548
		[ShowIf("useSight", 0)]
		public bool checkObsticle = true;

		// Token: 0x04001995 RID: 6549
		public BBParameter<DamageReceiver> result;

		// Token: 0x04001996 RID: 6550
		public BBParameter<InteractablePickup> pickupResult;

		// Token: 0x04001997 RID: 6551
		public bool searchPickup;

		// Token: 0x04001998 RID: 6552
		public bool alwaysSuccess;

		// Token: 0x04001999 RID: 6553
		public bool setNullIfNotFound;

		// Token: 0x0400199A RID: 6554
		private bool waitingSearchResult;

		// Token: 0x0400199B RID: 6555
		private float searchStartTimeMarker;

		// Token: 0x0400199C RID: 6556
		private bool isHurtSearch;
	}
}
