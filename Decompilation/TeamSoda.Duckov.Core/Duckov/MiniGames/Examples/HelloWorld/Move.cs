using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.Examples.HelloWorld
{
	// Token: 0x020002CF RID: 719
	public class Move : MiniGameBehaviour
	{
		// Token: 0x060016B5 RID: 5813 RVA: 0x00053277 File Offset: 0x00051477
		private void Awake()
		{
			if (this.rigidbody == null)
			{
				this.rigidbody = base.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00053294 File Offset: 0x00051494
		protected override void OnUpdate(float deltaTime)
		{
			bool flag = this.CanJump();
			Vector2 vector = base.Game.GetAxis(0) * this.speed;
			float y = this.rigidbody.velocity.y;
			if (base.Game.GetButtonDown(MiniGame.Button.A) && flag)
			{
				y = this.jumpSpeed;
			}
			this.rigidbody.velocity = new Vector3(vector.x, y, vector.y);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00053305 File Offset: 0x00051505
		private bool CanJump()
		{
			return this.touchingColliders.Count > 0;
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00053318 File Offset: 0x00051518
		private void OnCollisionEnter(Collision collision)
		{
			this.touchingColliders.Add(collision.collider);
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x0005332B File Offset: 0x0005152B
		private void OnCollisionExit(Collision collision)
		{
			this.touchingColliders.Remove(collision.collider);
		}

		// Token: 0x04001093 RID: 4243
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x04001094 RID: 4244
		[SerializeField]
		private float speed = 10f;

		// Token: 0x04001095 RID: 4245
		[SerializeField]
		private float jumpSpeed = 5f;

		// Token: 0x04001096 RID: 4246
		private List<Collider> touchingColliders = new List<Collider>();
	}
}
