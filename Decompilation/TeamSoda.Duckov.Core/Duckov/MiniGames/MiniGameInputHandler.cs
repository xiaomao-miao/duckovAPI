using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.MiniGames
{
	// Token: 0x02000284 RID: 644
	public class MiniGameInputHandler : MonoBehaviour
	{
		// Token: 0x0600149A RID: 5274 RVA: 0x0004C3AC File Offset: 0x0004A5AC
		private void Awake()
		{
			InputActionAsset actions = GameManager.MainPlayerInput.actions;
			this.inputActionMove = actions["MoveAxis"];
			this.inputActionButtonA = actions["MiniGameA"];
			this.inputActionButtonB = actions["MiniGameB"];
			this.inputActionSelect = actions["MiniGameSelect"];
			this.inputActionStart = actions["MiniGameStart"];
			this.inputActionMouseDelta = actions["MouseDelta"];
			this.inputActionButtonA.actionMap.Enable();
			this.Bind(this.inputActionMove, new Action<InputAction.CallbackContext>(this.OnMove));
			this.Bind(this.inputActionButtonA, new Action<InputAction.CallbackContext>(this.OnButtonA));
			this.Bind(this.inputActionButtonB, new Action<InputAction.CallbackContext>(this.OnButtonB));
			this.Bind(this.inputActionSelect, new Action<InputAction.CallbackContext>(this.OnSelect));
			this.Bind(this.inputActionStart, new Action<InputAction.CallbackContext>(this.OnStart));
			this.Bind(this.inputActionMouseDelta, new Action<InputAction.CallbackContext>(this.OnMouseDelta));
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0004C4CA File Offset: 0x0004A6CA
		private void OnMouseDelta(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.game.SetInputAxis(context.ReadValue<Vector2>(), 1);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0004C4F7 File Offset: 0x0004A6F7
		public void ClearInput()
		{
			MiniGame miniGame = this.game;
			if (miniGame == null)
			{
				return;
			}
			miniGame.ClearInput();
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0004C509 File Offset: 0x0004A709
		private void OnDisable()
		{
			this.ClearInput();
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0004C511 File Offset: 0x0004A711
		private void SetGameButtonByContext(MiniGame.Button button, InputAction.CallbackContext context)
		{
			if (context.started)
			{
				this.game.SetButton(button, true);
				return;
			}
			if (context.canceled)
			{
				this.game.SetButton(button, false);
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0004C540 File Offset: 0x0004A740
		private void OnStart(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.Start, context);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004C562 File Offset: 0x0004A762
		private void OnSelect(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.Select, context);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0004C584 File Offset: 0x0004A784
		private void OnButtonB(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.B, context);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0004C5A6 File Offset: 0x0004A7A6
		private void OnButtonA(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.A, context);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0004C5C8 File Offset: 0x0004A7C8
		private void OnMove(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.game.SetInputAxis(context.ReadValue<Vector2>(), 0);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0004C5F8 File Offset: 0x0004A7F8
		private void OnDestroy()
		{
			foreach (Action action in this.unbindCommands)
			{
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0004C650 File Offset: 0x0004A850
		private void Bind(InputAction inputAction, Action<InputAction.CallbackContext> action)
		{
			inputAction.Enable();
			inputAction.started += action;
			inputAction.performed += action;
			inputAction.canceled += action;
			this.unbindCommands.Add(delegate
			{
				inputAction.started -= action;
				inputAction.performed -= action;
				inputAction.canceled -= action;
			});
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0004C6C6 File Offset: 0x0004A8C6
		internal void SetGame(MiniGame game)
		{
			this.game = game;
		}

		// Token: 0x04000F10 RID: 3856
		[SerializeField]
		private MiniGame game;

		// Token: 0x04000F11 RID: 3857
		private InputAction inputActionMove;

		// Token: 0x04000F12 RID: 3858
		private InputAction inputActionButtonA;

		// Token: 0x04000F13 RID: 3859
		private InputAction inputActionButtonB;

		// Token: 0x04000F14 RID: 3860
		private InputAction inputActionSelect;

		// Token: 0x04000F15 RID: 3861
		private InputAction inputActionStart;

		// Token: 0x04000F16 RID: 3862
		private InputAction inputActionMouseDelta;

		// Token: 0x04000F17 RID: 3863
		private List<Action> unbindCommands = new List<Action>();
	}
}
