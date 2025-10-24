using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x02000285 RID: 645
	public class ControllerAnimator : MonoBehaviour
	{
		// Token: 0x060014A8 RID: 5288 RVA: 0x0004C6E2 File Offset: 0x0004A8E2
		private void OnEnable()
		{
			MiniGame.OnInput += this.OnMiniGameInput;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0004C6F5 File Offset: 0x0004A8F5
		private void OnDisable()
		{
			MiniGame.OnInput -= this.OnMiniGameInput;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0004C708 File Offset: 0x0004A908
		private void OnMiniGameInput(MiniGame game, MiniGame.MiniGameInputEventContext context)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.Game != game)
			{
				return;
			}
			this.HandleInput(context);
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0004C734 File Offset: 0x0004A934
		private void HandleInput(MiniGame.MiniGameInputEventContext context)
		{
			if (context.isButtonEvent)
			{
				this.HandleButtonEvent(context);
				return;
			}
			if (context.isAxisEvent)
			{
				this.HandleAxisEvent(context);
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0004C755 File Offset: 0x0004A955
		private void HandleAxisEvent(MiniGame.MiniGameInputEventContext context)
		{
			if (context.axisIndex != 0)
			{
				return;
			}
			this.SetAxis(context.axisValue);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0004C76C File Offset: 0x0004A96C
		private void HandleButtonEvent(MiniGame.MiniGameInputEventContext context)
		{
			switch (context.button)
			{
			case MiniGame.Button.A:
				this.HandleBtnPushRest(this.btn_A, context.pressing);
				break;
			case MiniGame.Button.B:
				this.HandleBtnPushRest(this.btn_B, context.pressing);
				break;
			case MiniGame.Button.Start:
				this.HandleBtnPushRest(this.btn_Start, context.pressing);
				break;
			case MiniGame.Button.Select:
				this.HandleBtnPushRest(this.btn_Select, context.pressing);
				break;
			case MiniGame.Button.Left:
			case MiniGame.Button.Right:
			case MiniGame.Button.Up:
			case MiniGame.Button.Down:
				this.PlayAxisPressReleaseFX(context.button, context.pressing);
				break;
			}
			if (context.pressing)
			{
				switch (context.button)
				{
				case MiniGame.Button.None:
					break;
				case MiniGame.Button.A:
					this.ApplyTorque(1f, -0.5f);
					return;
				case MiniGame.Button.B:
					this.ApplyTorque(1f, --0f);
					return;
				case MiniGame.Button.Start:
					this.ApplyTorque(0.5f, -0.5f);
					return;
				case MiniGame.Button.Select:
					this.ApplyTorque(-0.5f, -0.5f);
					return;
				case MiniGame.Button.Left:
					this.ApplyTorque(-1f, 0f);
					return;
				case MiniGame.Button.Right:
					this.ApplyTorque(-0.5f, 0f);
					return;
				case MiniGame.Button.Up:
					this.ApplyTorque(-1f, 0.5f);
					return;
				case MiniGame.Button.Down:
					this.ApplyTorque(-1f, -0.5f);
					return;
				default:
					return;
				}
			}
			else
			{
				this.ApplyTorque(UnityEngine.Random.insideUnitCircle * 0.25f);
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004C8E8 File Offset: 0x0004AAE8
		private void PlayAxisPressReleaseFX(MiniGame.Button button, bool pressing)
		{
			Transform transform = null;
			switch (button)
			{
			case MiniGame.Button.Left:
				transform = this.fxPos_Left;
				break;
			case MiniGame.Button.Right:
				transform = this.fxPos_Right;
				break;
			case MiniGame.Button.Up:
				transform = this.fxPos_Up;
				break;
			case MiniGame.Button.Down:
				transform = this.fxPos_Down;
				break;
			}
			if (transform == null)
			{
				return;
			}
			if (pressing)
			{
				FXPool.Play(this.buttonPressFX, transform.position, transform.rotation);
				return;
			}
			FXPool.Play(this.buttonRestFX, transform.position, transform.rotation);
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0004C974 File Offset: 0x0004AB74
		private void ApplyTorque(float x, float y)
		{
			if (this.mainTransform == null)
			{
				return;
			}
			this.mainTransform.DOKill(false);
			Vector3 punch = new Vector3(-y, -x, 0f) * this.torqueStrength;
			this.mainTransform.localRotation = Quaternion.identity;
			this.mainTransform.DOPunchRotation(punch, this.torqueDuration, this.torqueVibrato, this.torqueElasticity);
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0004C9E6 File Offset: 0x0004ABE6
		private void ApplyTorque(Vector2 torque)
		{
			this.ApplyTorque(torque.x, torque.y);
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0004C9FA File Offset: 0x0004ABFA
		private void HandleBtnPushRest(Transform btnTrans, bool pressed)
		{
			if (pressed)
			{
				this.Push(btnTrans);
				return;
			}
			this.Rest(btnTrans);
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0004CA0E File Offset: 0x0004AC0E
		internal void SetConsole(GamingConsole master)
		{
			this.master = master;
			this.RefreshAll();
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0004CA20 File Offset: 0x0004AC20
		private void RefreshAll()
		{
			this.RestAll();
			if (this.master == null)
			{
				return;
			}
			MiniGame game = this.master.Game;
			if (game == null)
			{
				return;
			}
			if (game.GetButton(MiniGame.Button.A))
			{
				this.Push(this.btn_A);
			}
			if (game.GetButton(MiniGame.Button.B))
			{
				this.Push(this.btn_B);
			}
			if (game.GetButton(MiniGame.Button.Select))
			{
				this.Push(this.btn_Select);
			}
			if (game.GetButton(MiniGame.Button.Start))
			{
				this.Push(this.btn_Start);
			}
			this.SetAxis(game.GetAxis(0));
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0004CABC File Offset: 0x0004ACBC
		private void RestAll()
		{
			this.Rest(this.btn_A);
			this.Rest(this.btn_B);
			this.Rest(this.btn_Start);
			this.Rest(this.btn_Select);
			this.Rest(this.btn_Axis);
			this.SetAxis(Vector2.zero);
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0004CB10 File Offset: 0x0004AD10
		private void SetAxis(Vector2 axis)
		{
			if (this.btn_Axis == null)
			{
				return;
			}
			axis = axis.normalized;
			Vector3 euler = new Vector3(0f, -axis.x * this.axisAmp, axis.y * this.axisAmp);
			Quaternion localRotation = this.btn_Axis.localRotation;
			Quaternion quaternion = Quaternion.Euler(euler);
			quaternion * Quaternion.Inverse(localRotation);
			this.btn_Axis.localRotation = quaternion;
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0004CB88 File Offset: 0x0004AD88
		private void Push(Transform btnTransform)
		{
			if (btnTransform == null)
			{
				return;
			}
			btnTransform.DOKill(false);
			btnTransform.DOLocalMoveX(-this.btnDepth, this.transitionDuration, false).SetEase(Ease.OutElastic);
			if (this.buttonPressFX)
			{
				FXPool.Play(this.buttonPressFX, btnTransform.position, btnTransform.rotation);
			}
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004CBE8 File Offset: 0x0004ADE8
		private void Rest(Transform btnTransform)
		{
			if (btnTransform == null)
			{
				return;
			}
			btnTransform.DOKill(false);
			btnTransform.DOLocalMoveX(0f, this.transitionDuration, false).SetEase(Ease.OutElastic);
			if (this.buttonRestFX)
			{
				FXPool.Play(this.buttonRestFX, btnTransform.position, btnTransform.rotation);
			}
		}

		// Token: 0x04000F18 RID: 3864
		private GamingConsole master;

		// Token: 0x04000F19 RID: 3865
		public Transform mainTransform;

		// Token: 0x04000F1A RID: 3866
		public Transform btn_A;

		// Token: 0x04000F1B RID: 3867
		public Transform btn_B;

		// Token: 0x04000F1C RID: 3868
		public Transform btn_Start;

		// Token: 0x04000F1D RID: 3869
		public Transform btn_Select;

		// Token: 0x04000F1E RID: 3870
		public Transform btn_Axis;

		// Token: 0x04000F1F RID: 3871
		public Transform fxPos_Up;

		// Token: 0x04000F20 RID: 3872
		public Transform fxPos_Right;

		// Token: 0x04000F21 RID: 3873
		public Transform fxPos_Down;

		// Token: 0x04000F22 RID: 3874
		public Transform fxPos_Left;

		// Token: 0x04000F23 RID: 3875
		[SerializeField]
		private float transitionDuration = 0.2f;

		// Token: 0x04000F24 RID: 3876
		[SerializeField]
		private float axisAmp = 10f;

		// Token: 0x04000F25 RID: 3877
		[SerializeField]
		private float btnDepth = 0.003f;

		// Token: 0x04000F26 RID: 3878
		[SerializeField]
		private float torqueStrength = 5f;

		// Token: 0x04000F27 RID: 3879
		[SerializeField]
		private float torqueDuration = 0.5f;

		// Token: 0x04000F28 RID: 3880
		[SerializeField]
		private int torqueVibrato = 1;

		// Token: 0x04000F29 RID: 3881
		[SerializeField]
		private float torqueElasticity = 1f;

		// Token: 0x04000F2A RID: 3882
		[SerializeField]
		private ParticleSystem buttonPressFX;

		// Token: 0x04000F2B RID: 3883
		[SerializeField]
		private ParticleSystem buttonRestFX;
	}
}
