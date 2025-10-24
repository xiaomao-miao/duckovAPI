using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200040D RID: 1037
	public class PickupSearchedItem : ActionTask<AICharacterController>
	{
		// Token: 0x06002574 RID: 9588 RVA: 0x000811AE File Offset: 0x0007F3AE
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000811B4 File Offset: 0x0007F3B4
		protected override void OnExecute()
		{
			if (base.agent == null || base.agent.CharacterMainControl == null || base.agent.searchedPickup == null)
			{
				base.EndAction(false);
				return;
			}
			if (Vector3.Distance(base.agent.transform.position, base.agent.searchedPickup.transform.position) > 1.5f)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.searchedPickup.ItemAgent != null)
			{
				base.agent.CharacterMainControl.PickupItem(base.agent.searchedPickup.ItemAgent.Item);
			}
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00081274 File Offset: 0x0007F474
		protected override void OnUpdate()
		{
		}
	}
}
