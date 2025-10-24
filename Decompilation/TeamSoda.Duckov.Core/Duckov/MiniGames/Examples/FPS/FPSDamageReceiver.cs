using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D3 RID: 723
	public class FPSDamageReceiver : MonoBehaviour
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x0005349E File Offset: 0x0005169E
		public ParticleSystem DamageFX
		{
			get
			{
				if (GameManager.BloodFxOn)
				{
					return this.damageEffectPrefab;
				}
				return this.damageEffectPrefab_Censored;
			}
		}

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x060016CA RID: 5834 RVA: 0x000534B4 File Offset: 0x000516B4
		// (remove) Token: 0x060016CB RID: 5835 RVA: 0x000534EC File Offset: 0x000516EC
		public event Action<FPSDamageReceiver, FPSDamageInfo> onReceiveDamage;

		// Token: 0x060016CC RID: 5836 RVA: 0x00053524 File Offset: 0x00051724
		internal void CastDamage(FPSDamageInfo damage)
		{
			if (this.DamageFX == null)
			{
				return;
			}
			FXPool.Play(this.DamageFX, damage.point, Quaternion.FromToRotation(Vector3.forward, damage.normal));
			Action<FPSDamageReceiver, FPSDamageInfo> action = this.onReceiveDamage;
			if (action == null)
			{
				return;
			}
			action(this, damage);
		}

		// Token: 0x0400109E RID: 4254
		[SerializeField]
		private ParticleSystem damageEffectPrefab;

		// Token: 0x0400109F RID: 4255
		[SerializeField]
		private ParticleSystem damageEffectPrefab_Censored;
	}
}
