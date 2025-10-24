using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D6 RID: 726
	public class FPSGunControl : MiniGameBehaviour
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x000539E6 File Offset: 0x00051BE6
		public FPSGun Gun
		{
			get
			{
				return this.gun;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060016DA RID: 5850 RVA: 0x000539EE File Offset: 0x00051BEE
		public float ScatterAngle
		{
			get
			{
				if (this.Gun)
				{
					return this.Gun.ScatterAngle;
				}
				return 0f;
			}
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00053A0E File Offset: 0x00051C0E
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.gun != null)
			{
				this.SetGun(this.gun);
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00053A30 File Offset: 0x00051C30
		protected override void OnUpdate(float deltaTime)
		{
			bool buttonDown = base.Game.GetButtonDown(MiniGame.Button.A);
			bool buttonUp = base.Game.GetButtonUp(MiniGame.Button.A);
			if (buttonDown)
			{
				this.gun.SetTrigger(true);
			}
			if (buttonUp)
			{
				this.gun.SetTrigger(false);
			}
			this.UpdateGunPhysicsStatus(deltaTime);
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00053A7A File Offset: 0x00051C7A
		private void UpdateGunPhysicsStatus(float deltaTime)
		{
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00053A7C File Offset: 0x00051C7C
		private void SetGun(FPSGun gunInstance)
		{
			if (gunInstance != this.gun)
			{
				UnityEngine.Object.Destroy(this.gun);
			}
			this.gun = gunInstance;
			this.SetupGunData();
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00053AA4 File Offset: 0x00051CA4
		private void SetupGunData()
		{
			this.gun.Setup(this.mainCamera, this.gunParent);
		}

		// Token: 0x040010B8 RID: 4280
		[SerializeField]
		private Camera mainCamera;

		// Token: 0x040010B9 RID: 4281
		[SerializeField]
		private Transform gunParent;

		// Token: 0x040010BA RID: 4282
		[SerializeField]
		private FPSGun gun;
	}
}
