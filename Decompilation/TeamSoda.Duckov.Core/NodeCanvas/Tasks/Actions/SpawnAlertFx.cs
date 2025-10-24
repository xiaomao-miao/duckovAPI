using System;
using Duckov.Utilities;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000418 RID: 1048
	public class SpawnAlertFx : ActionTask<AICharacterController>
	{
		// Token: 0x060025BA RID: 9658 RVA: 0x00081EC1 File Offset: 0x000800C1
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x00081EC4 File Offset: 0x000800C4
		protected override string info
		{
			get
			{
				return string.Format("AlertFx", Array.Empty<object>());
			}
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x00081ED8 File Offset: 0x000800D8
		protected override void OnExecute()
		{
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				base.EndAction(true);
			}
			Transform rightHandSocket = base.agent.CharacterMainControl.RightHandSocket;
			if (!rightHandSocket)
			{
				base.EndAction(true);
			}
			UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.AlertFxPrefab, rightHandSocket).transform.localPosition = Vector3.zero;
			if (this.alertTime.value <= 0f)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x00081F63 File Offset: 0x00080163
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.alertTime.value)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x00081F7F File Offset: 0x0008017F
		protected override void OnStop()
		{
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x00081F81 File Offset: 0x00080181
		protected override void OnPause()
		{
		}

		// Token: 0x040019A7 RID: 6567
		public BBParameter<float> alertTime = 0.2f;
	}
}
