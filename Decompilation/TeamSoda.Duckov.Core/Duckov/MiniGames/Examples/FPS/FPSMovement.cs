using System;
using ECM2;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D8 RID: 728
	public class FPSMovement : Character
	{
		// Token: 0x060016EB RID: 5867 RVA: 0x00053CAC File Offset: 0x00051EAC
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00053CB4 File Offset: 0x00051EB4
		protected override void Start()
		{
			base.Start();
			if (this.game == null)
			{
				this.game.GetComponentInParent<MiniGame>();
			}
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00053CD6 File Offset: 0x00051ED6
		public void SetGame(MiniGame game)
		{
			this.game = game;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00053CDF File Offset: 0x00051EDF
		private void Update()
		{
			this.UpdateRotation();
			this.UpdateMovement();
			if (this.game.GetButtonDown(MiniGame.Button.B))
			{
				this.Jump();
				return;
			}
			if (this.game.GetButtonUp(MiniGame.Button.B))
			{
				this.StopJumping();
			}
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00053D18 File Offset: 0x00051F18
		private void UpdateMovement()
		{
			Vector2 axis = this.game.GetAxis(0);
			Vector3 vector = Vector3.zero;
			vector += Vector3.right * axis.x;
			vector += Vector3.forward * axis.y;
			if (base.camera)
			{
				vector = vector.relativeTo(base.cameraTransform, true);
			}
			base.SetMovementDirection(vector);
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00053D88 File Offset: 0x00051F88
		private void UpdateRotation()
		{
			Vector2 axis = this.game.GetAxis(1);
			this.AddYawInput(axis.x * this.lookSensitivity.x);
			if (axis.y == 0f)
			{
				return;
			}
			float num = MathLib.ClampAngle(-base.cameraTransform.localRotation.eulerAngles.x + axis.y * this.lookSensitivity.y, -80f, 80f);
			base.cameraTransform.localRotation = Quaternion.Euler(-num, 0f, 0f);
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00053E20 File Offset: 0x00052020
		public void AddControlYawInput(float value)
		{
			this.AddYawInput(value);
		}

		// Token: 0x040010C4 RID: 4292
		[SerializeField]
		private MiniGame game;

		// Token: 0x040010C5 RID: 4293
		[SerializeField]
		private Vector2 lookSensitivity;
	}
}
