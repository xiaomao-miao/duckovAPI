using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x0200032E RID: 814
	public class MultiSceneTeleporter : InteractableBase
	{
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001B9D RID: 7069 RVA: 0x00064421 File Offset: 0x00062621
		protected override bool ShowUnityEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001B9E RID: 7070 RVA: 0x00064424 File Offset: 0x00062624
		[SerializeField]
		public MultiSceneLocation Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001B9F RID: 7071 RVA: 0x0006442C File Offset: 0x0006262C
		private static float TimeSinceTeleportFinished
		{
			get
			{
				return Time.time - MultiSceneTeleporter.timeWhenTeleportFinished;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001BA0 RID: 7072 RVA: 0x00064439 File Offset: 0x00062639
		private static bool Teleportable
		{
			get
			{
				return MultiSceneTeleporter.TimeSinceTeleportFinished > 1f;
			}
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00064447 File Offset: 0x00062647
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00064450 File Offset: 0x00062650
		private void OnDrawGizmosSelected()
		{
			Transform locationTransform = this.target.LocationTransform;
			if (locationTransform)
			{
				Gizmos.DrawLine(base.transform.position, locationTransform.position);
				Gizmos.DrawWireSphere(locationTransform.position, 0.25f);
			}
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00064497 File Offset: 0x00062697
		public void DoTeleport()
		{
			if (!MultiSceneTeleporter.Teleportable)
			{
				Debug.Log("not Teleportable");
				return;
			}
			this.TeleportTask().Forget();
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x000644B6 File Offset: 0x000626B6
		protected override bool IsInteractable()
		{
			return MultiSceneTeleporter.Teleportable;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x000644C0 File Offset: 0x000626C0
		private UniTask TeleportTask()
		{
			MultiSceneTeleporter.<TeleportTask>d__16 <TeleportTask>d__;
			<TeleportTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<TeleportTask>d__.<>4__this = this;
			<TeleportTask>d__.<>1__state = -1;
			<TeleportTask>d__.<>t__builder.Start<MultiSceneTeleporter.<TeleportTask>d__16>(ref <TeleportTask>d__);
			return <TeleportTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x00064503 File Offset: 0x00062703
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			this.coolTime = 2f;
			this.finishWhenTimeOut = true;
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x00064517 File Offset: 0x00062717
		protected override void OnInteractFinished()
		{
			this.DoTeleport();
			base.StopInteract();
		}

		// Token: 0x0400138F RID: 5007
		[SerializeField]
		private MultiSceneLocation target;

		// Token: 0x04001390 RID: 5008
		[SerializeField]
		private bool teleportOnTriggerEnter;

		// Token: 0x04001391 RID: 5009
		private const float CoolDown = 1f;

		// Token: 0x04001392 RID: 5010
		private static float timeWhenTeleportFinished;
	}
}
