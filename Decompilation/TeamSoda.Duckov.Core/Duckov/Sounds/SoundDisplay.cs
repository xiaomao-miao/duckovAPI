using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.Sounds
{
	// Token: 0x02000248 RID: 584
	public class SoundDisplay : MonoBehaviour
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x00045044 File Offset: 0x00043244
		public float Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x0004504C File Offset: 0x0004324C
		public AISound CurrentSount
		{
			get
			{
				return this.sound;
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00045054 File Offset: 0x00043254
		internal void Trigger(AISound sound)
		{
			this.sound = sound;
			base.gameObject.SetActive(true);
			this.velocity = this.triggerVelocity;
			this.value += this.velocity * Time.deltaTime;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00045090 File Offset: 0x00043290
		private void Update()
		{
			this.velocity -= this.gravity * Time.deltaTime;
			this.value += this.velocity * Time.deltaTime;
			if (this.value > 1f || this.value < 0f)
			{
				this.velocity = 0f;
			}
			this.value = Mathf.Clamp01(this.value);
			this.image.color = new Color(1f, 1f, 1f, this.value);
		}

		// Token: 0x04000DEC RID: 3564
		[SerializeField]
		private Image image;

		// Token: 0x04000DED RID: 3565
		[SerializeField]
		private float removeRecordAfterTime = 1f;

		// Token: 0x04000DEE RID: 3566
		[SerializeField]
		private float triggerVelocity = 10f;

		// Token: 0x04000DEF RID: 3567
		[SerializeField]
		private float gravity = 1f;

		// Token: 0x04000DF0 RID: 3568
		[SerializeField]
		private float untriggerVelocity = 100f;

		// Token: 0x04000DF1 RID: 3569
		private float value;

		// Token: 0x04000DF2 RID: 3570
		private float velocity;

		// Token: 0x04000DF3 RID: 3571
		private AISound sound;
	}
}
