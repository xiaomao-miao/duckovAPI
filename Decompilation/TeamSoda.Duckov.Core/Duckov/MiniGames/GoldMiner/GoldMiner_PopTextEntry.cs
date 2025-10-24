using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A3 RID: 675
	public class GoldMiner_PopTextEntry : MonoBehaviour
	{
		// Token: 0x060015FA RID: 5626 RVA: 0x00051364 File Offset: 0x0004F564
		public void Setup(Vector3 pos, string text, Action<GoldMiner_PopTextEntry> releaseAction)
		{
			this.initialized = true;
			this.tmp.text = text;
			this.life = 0f;
			base.transform.position = pos;
			this.releaseAction = releaseAction;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00051398 File Offset: 0x0004F598
		private void Update()
		{
			if (!this.initialized)
			{
				return;
			}
			this.life += Time.deltaTime;
			base.transform.position += Vector3.up * this.moveSpeed * Time.deltaTime;
			if (this.life >= this.lifeTime)
			{
				this.Release();
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00051404 File Offset: 0x0004F604
		private void Release()
		{
			if (this.releaseAction != null)
			{
				this.releaseAction(this);
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001042 RID: 4162
		public TextMeshProUGUI tmp;

		// Token: 0x04001043 RID: 4163
		public float lifeTime;

		// Token: 0x04001044 RID: 4164
		public float moveSpeed = 1f;

		// Token: 0x04001045 RID: 4165
		private bool initialized;

		// Token: 0x04001046 RID: 4166
		private float life;

		// Token: 0x04001047 RID: 4167
		private Action<GoldMiner_PopTextEntry> releaseAction;
	}
}
