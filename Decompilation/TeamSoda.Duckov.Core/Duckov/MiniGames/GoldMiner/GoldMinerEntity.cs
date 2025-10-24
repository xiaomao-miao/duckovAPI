using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000290 RID: 656
	public class GoldMinerEntity : MiniGameBehaviour
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x0004F643 File Offset: 0x0004D843
		// (set) Token: 0x06001570 RID: 5488 RVA: 0x0004F64B File Offset: 0x0004D84B
		public GoldMiner master { get; private set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x0004F654 File Offset: 0x0004D854
		public string TypeID
		{
			get
			{
				return this.typeID;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x0004F65C File Offset: 0x0004D85C
		public float Speed
		{
			get
			{
				return this.speed;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x0004F664 File Offset: 0x0004D864
		// (set) Token: 0x06001574 RID: 5492 RVA: 0x0004F66C File Offset: 0x0004D86C
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0004F675 File Offset: 0x0004D875
		public void SetMaster(GoldMiner master)
		{
			this.master = master;
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0004F67E File Offset: 0x0004D87E
		public void NotifyAttached(Hook hook)
		{
			Action<GoldMinerEntity, Hook> onAttached = this.OnAttached;
			if (onAttached != null)
			{
				onAttached(this, hook);
			}
			FXPool.Play(this.contactFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0004F6B5 File Offset: 0x0004D8B5
		public void NotifyBeginRetrieving()
		{
			FXPool.Play(this.beginMoveFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0004F6D9 File Offset: 0x0004D8D9
		internal void Explode(Vector3 origin)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			FXPool.Play(this.explodeFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0004F708 File Offset: 0x0004D908
		internal void NotifyResolved(GoldMiner game)
		{
			Action<GoldMinerEntity, GoldMiner> onResolved = this.OnResolved;
			if (onResolved != null)
			{
				onResolved(this, game);
			}
			FXPool.Play(this.resolveFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x04000FD1 RID: 4049
		[SerializeField]
		private string typeID;

		// Token: 0x04000FD2 RID: 4050
		[SerializeField]
		public GoldMinerEntity.Size size;

		// Token: 0x04000FD3 RID: 4051
		[SerializeField]
		public GoldMinerEntity.Tag[] tags;

		// Token: 0x04000FD4 RID: 4052
		[SerializeField]
		private int value;

		// Token: 0x04000FD5 RID: 4053
		[SerializeField]
		private float speed = 1f;

		// Token: 0x04000FD6 RID: 4054
		[SerializeField]
		private ParticleSystem contactFX;

		// Token: 0x04000FD7 RID: 4055
		[SerializeField]
		private ParticleSystem beginMoveFX;

		// Token: 0x04000FD8 RID: 4056
		[SerializeField]
		private ParticleSystem resolveFX;

		// Token: 0x04000FD9 RID: 4057
		[SerializeField]
		private ParticleSystem explodeFX;

		// Token: 0x04000FDA RID: 4058
		public Action<GoldMinerEntity, Hook> OnAttached;

		// Token: 0x04000FDB RID: 4059
		public Action<GoldMinerEntity, GoldMiner> OnResolved;

		// Token: 0x02000569 RID: 1385
		public enum Size
		{
			// Token: 0x04001F49 RID: 8009
			XS = -2,
			// Token: 0x04001F4A RID: 8010
			S,
			// Token: 0x04001F4B RID: 8011
			M,
			// Token: 0x04001F4C RID: 8012
			L,
			// Token: 0x04001F4D RID: 8013
			XL
		}

		// Token: 0x0200056A RID: 1386
		public enum Tag
		{
			// Token: 0x04001F4F RID: 8015
			None,
			// Token: 0x04001F50 RID: 8016
			Rock,
			// Token: 0x04001F51 RID: 8017
			Gold,
			// Token: 0x04001F52 RID: 8018
			Diamond,
			// Token: 0x04001F53 RID: 8019
			Mine,
			// Token: 0x04001F54 RID: 8020
			Chest,
			// Token: 0x04001F55 RID: 8021
			Pig,
			// Token: 0x04001F56 RID: 8022
			Cable
		}
	}
}
