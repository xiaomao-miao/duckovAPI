using System;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000280 RID: 640
	public class GamingConsoleAnimator : MonoBehaviour
	{
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x0004BC44 File Offset: 0x00049E44
		[SerializeField]
		private MiniGame Game
		{
			get
			{
				if (this.console == null)
				{
					return null;
				}
				return this.console.Game;
			}
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0004BC61 File Offset: 0x00049E61
		private void Update()
		{
			this.Tick();
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0004BC6C File Offset: 0x00049E6C
		private void Tick()
		{
			if (this.Game == null)
			{
				this.Clear();
				return;
			}
			if (CameraMode.Active)
			{
				return;
			}
			this.joyStick_Target = this.Game.GetAxis(0);
			this.joyStick_Current = Vector2.Lerp(this.joyStick_Current, this.joyStick_Target, 0.25f);
			Vector2 vector = this.joyStick_Current;
			this.animator.SetFloat("AxisX", vector.x);
			this.animator.SetFloat("AxisY", vector.y);
			this.animator.SetBool("ButtonA", this.Game.GetButton(MiniGame.Button.A));
			this.animator.SetBool("ButtonB", this.Game.GetButton(MiniGame.Button.B));
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0004BD30 File Offset: 0x00049F30
		private void Clear()
		{
			this.animator.SetBool("ButtonA", false);
			this.animator.SetBool("ButtonB", false);
			this.animator.SetFloat("AxisX", 0f);
			this.animator.SetFloat("AxisY", 0f);
		}

		// Token: 0x04000EFE RID: 3838
		[SerializeField]
		private Animator animator;

		// Token: 0x04000EFF RID: 3839
		[SerializeField]
		private GamingConsole console;

		// Token: 0x04000F00 RID: 3840
		private Vector2 joyStick_Current;

		// Token: 0x04000F01 RID: 3841
		private Vector2 joyStick_Target;
	}
}
