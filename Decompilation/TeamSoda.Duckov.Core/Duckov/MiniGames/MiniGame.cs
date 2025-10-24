using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000282 RID: 642
	public class MiniGame : MonoBehaviour
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0004BE05 File Offset: 0x0004A005
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x0004BE0D File Offset: 0x0004A00D
		public Camera Camera
		{
			get
			{
				return this.camera;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0004BE15 File Offset: 0x0004A015
		public Camera UICamera
		{
			get
			{
				return this.uiCamera;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0004BE1D File Offset: 0x0004A01D
		public RenderTexture RenderTexture
		{
			get
			{
				return this.renderTexture;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0004BE25 File Offset: 0x0004A025
		public GamingConsole Console
		{
			get
			{
				return this.console;
			}
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x0600147D RID: 5245 RVA: 0x0004BE30 File Offset: 0x0004A030
		// (remove) Token: 0x0600147E RID: 5246 RVA: 0x0004BE64 File Offset: 0x0004A064
		public static event Action<MiniGame, MiniGame.MiniGameInputEventContext> OnInput;

		// Token: 0x0600147F RID: 5247 RVA: 0x0004BE97 File Offset: 0x0004A097
		public void SetRenderTexture(RenderTexture texture)
		{
			this.camera.targetTexture = texture;
			if (this.uiCamera)
			{
				this.uiCamera.targetTexture = texture;
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0004BEC0 File Offset: 0x0004A0C0
		public RenderTexture CreateAndSetRenderTexture(int width, int height)
		{
			RenderTexture result = new RenderTexture(width, height, 32);
			this.SetRenderTexture(result);
			return result;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0004BEDF File Offset: 0x0004A0DF
		private void Awake()
		{
			if (this.renderTexture != null)
			{
				this.SetRenderTexture(this.renderTexture);
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0004BEFC File Offset: 0x0004A0FC
		public void SetInputAxis(Vector2 axis, int index = 0)
		{
			Vector2 vector = this.inputAxis_0;
			if (index == 0)
			{
				this.inputAxis_0 = axis;
			}
			if (index == 1)
			{
				this.inputAxis_1 = axis;
			}
			if (index == 0)
			{
				bool flag = axis.x < -0.1f;
				bool flag2 = axis.x > 0.1f;
				bool flag3 = axis.y > 0.1f;
				bool flag4 = axis.y < -0.1f;
				bool flag5 = vector.x < -0.1f;
				bool flag6 = vector.x > 0.1f;
				bool flag7 = vector.y > 0.1f;
				bool flag8 = vector.y < -0.1f;
				if (flag != flag5)
				{
					this.SetButton(MiniGame.Button.Left, flag);
				}
				if (flag2 != flag6)
				{
					this.SetButton(MiniGame.Button.Right, flag2);
				}
				if (flag3 != flag7)
				{
					this.SetButton(MiniGame.Button.Up, flag3);
				}
				if (flag4 != flag8)
				{
					this.SetButton(MiniGame.Button.Down, flag4);
				}
			}
			Action<MiniGame, MiniGame.MiniGameInputEventContext> onInput = MiniGame.OnInput;
			if (onInput == null)
			{
				return;
			}
			onInput(this, new MiniGame.MiniGameInputEventContext
			{
				isAxisEvent = true,
				axisIndex = index,
				axisValue = axis
			});
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0004C008 File Offset: 0x0004A208
		public void SetButton(MiniGame.Button button, bool down)
		{
			MiniGame.ButtonStatus buttonStatus;
			if (!this.buttons.TryGetValue(button, out buttonStatus))
			{
				buttonStatus = new MiniGame.ButtonStatus();
				this.buttons[button] = buttonStatus;
			}
			if (down)
			{
				buttonStatus.justPressed = true;
				buttonStatus.pressed = true;
			}
			else
			{
				buttonStatus.pressed = false;
				buttonStatus.justReleased = true;
			}
			this.buttons[button] = buttonStatus;
			Action<MiniGame, MiniGame.MiniGameInputEventContext> onInput = MiniGame.OnInput;
			if (onInput == null)
			{
				return;
			}
			onInput(this, new MiniGame.MiniGameInputEventContext
			{
				isButtonEvent = true,
				button = button,
				pressing = buttonStatus.pressed,
				buttonDown = buttonStatus.justPressed,
				buttonUp = buttonStatus.justReleased
			});
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004C0B8 File Offset: 0x0004A2B8
		public bool GetButton(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.pressed;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0004C0E0 File Offset: 0x0004A2E0
		public bool GetButtonDown(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.justPressed;
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0004C108 File Offset: 0x0004A308
		public bool GetButtonUp(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.justReleased;
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0004C130 File Offset: 0x0004A330
		public Vector2 GetAxis(int index = 0)
		{
			if (index == 0)
			{
				return this.inputAxis_0;
			}
			if (index == 1)
			{
				return this.inputAxis_1;
			}
			return default(Vector2);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0004C15B File Offset: 0x0004A35B
		private void Tick(float deltaTime)
		{
			this.UpdateLogic(deltaTime);
			this.Cleanup();
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0004C16A File Offset: 0x0004A36A
		private void UpdateLogic(float deltaTime)
		{
			Action<MiniGame, float> action = MiniGame.onUpdateLogic;
			if (action == null)
			{
				return;
			}
			action(this, deltaTime);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0004C180 File Offset: 0x0004A380
		private void Cleanup()
		{
			foreach (MiniGame.ButtonStatus buttonStatus in this.buttons.Values)
			{
				buttonStatus.justPressed = false;
				buttonStatus.justReleased = false;
			}
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0004C1E0 File Offset: 0x0004A3E0
		private void Update()
		{
			if (this.tickTiming == MiniGame.TickTiming.Update)
			{
				this.Tick(Time.deltaTime);
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0004C1F6 File Offset: 0x0004A3F6
		private void FixedUpdate()
		{
			if (this.tickTiming == MiniGame.TickTiming.FixedUpdate)
			{
				this.Tick(Time.fixedDeltaTime);
			}
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0004C20C File Offset: 0x0004A40C
		private void LateUpdate()
		{
			if (this.tickTiming == MiniGame.TickTiming.FixedUpdate)
			{
				this.Tick(Time.deltaTime);
			}
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0004C224 File Offset: 0x0004A424
		public void ClearInput()
		{
			foreach (MiniGame.ButtonStatus buttonStatus in this.buttons.Values)
			{
				if (buttonStatus.pressed)
				{
					buttonStatus.justReleased = true;
				}
				buttonStatus.pressed = false;
			}
			this.SetInputAxis(default(Vector2), 0);
			this.SetInputAxis(default(Vector2), 1);
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0004C2AC File Offset: 0x0004A4AC
		internal void SetConsole(GamingConsole console)
		{
			this.console = console;
		}

		// Token: 0x04000F04 RID: 3844
		[SerializeField]
		private string id;

		// Token: 0x04000F05 RID: 3845
		public MiniGame.TickTiming tickTiming;

		// Token: 0x04000F06 RID: 3846
		[SerializeField]
		private Camera camera;

		// Token: 0x04000F07 RID: 3847
		[SerializeField]
		private Camera uiCamera;

		// Token: 0x04000F08 RID: 3848
		[SerializeField]
		private RenderTexture renderTexture;

		// Token: 0x04000F09 RID: 3849
		public static Action<MiniGame, float> onUpdateLogic;

		// Token: 0x04000F0A RID: 3850
		private GamingConsole console;

		// Token: 0x04000F0B RID: 3851
		private Vector2 inputAxis_0;

		// Token: 0x04000F0C RID: 3852
		private Vector2 inputAxis_1;

		// Token: 0x04000F0D RID: 3853
		private Dictionary<MiniGame.Button, MiniGame.ButtonStatus> buttons = new Dictionary<MiniGame.Button, MiniGame.ButtonStatus>();

		// Token: 0x02000555 RID: 1365
		public enum TickTiming
		{
			// Token: 0x04001EE2 RID: 7906
			Manual,
			// Token: 0x04001EE3 RID: 7907
			Update,
			// Token: 0x04001EE4 RID: 7908
			FixedUpdate,
			// Token: 0x04001EE5 RID: 7909
			LateUpdate
		}

		// Token: 0x02000556 RID: 1366
		public enum Button
		{
			// Token: 0x04001EE7 RID: 7911
			None,
			// Token: 0x04001EE8 RID: 7912
			A,
			// Token: 0x04001EE9 RID: 7913
			B,
			// Token: 0x04001EEA RID: 7914
			Start,
			// Token: 0x04001EEB RID: 7915
			Select,
			// Token: 0x04001EEC RID: 7916
			Left,
			// Token: 0x04001EED RID: 7917
			Right,
			// Token: 0x04001EEE RID: 7918
			Up,
			// Token: 0x04001EEF RID: 7919
			Down
		}

		// Token: 0x02000557 RID: 1367
		public class ButtonStatus
		{
			// Token: 0x04001EF0 RID: 7920
			public bool pressed;

			// Token: 0x04001EF1 RID: 7921
			public bool justPressed;

			// Token: 0x04001EF2 RID: 7922
			public bool justReleased;
		}

		// Token: 0x02000558 RID: 1368
		public struct MiniGameInputEventContext
		{
			// Token: 0x04001EF3 RID: 7923
			public bool isButtonEvent;

			// Token: 0x04001EF4 RID: 7924
			public MiniGame.Button button;

			// Token: 0x04001EF5 RID: 7925
			public bool pressing;

			// Token: 0x04001EF6 RID: 7926
			public bool buttonDown;

			// Token: 0x04001EF7 RID: 7927
			public bool buttonUp;

			// Token: 0x04001EF8 RID: 7928
			public bool isAxisEvent;

			// Token: 0x04001EF9 RID: 7929
			public int axisIndex;

			// Token: 0x04001EFA RID: 7930
			public Vector2 axisValue;
		}
	}
}
