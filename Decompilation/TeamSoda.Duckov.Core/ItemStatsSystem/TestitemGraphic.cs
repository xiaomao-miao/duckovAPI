using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItemStatsSystem
{
	// Token: 0x0200022B RID: 555
	public class TestitemGraphic : MonoBehaviour
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x0004230B File Offset: 0x0004050B
		private void Start()
		{
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00042310 File Offset: 0x00040510
		private void Update()
		{
			if (Keyboard.current.gKey.wasPressedThisFrame)
			{
				if (this.instance)
				{
					UnityEngine.Object.Destroy(this.instance.gameObject);
				}
				DuckovItemAgent currentHoldItemAgent = CharacterMainControl.Main.CurrentHoldItemAgent;
				if (!currentHoldItemAgent)
				{
					return;
				}
				this.instance = ItemGraphicInfo.CreateAGraphic(currentHoldItemAgent.Item, base.transform);
			}
		}

		// Token: 0x04000D52 RID: 3410
		private ItemGraphicInfo instance;
	}
}
