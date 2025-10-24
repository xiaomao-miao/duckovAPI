using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001BB RID: 443
public class InputRebinder : MonoBehaviour
{
	// Token: 0x06000D25 RID: 3365 RVA: 0x00036A79 File Offset: 0x00034C79
	public void Rebind()
	{
		InputRebinder.RebindAsync(this.action, this.index, this.excludes, false).Forget<bool>();
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00036A98 File Offset: 0x00034C98
	private static PlayerInput PlayerInput
	{
		get
		{
			return GameManager.MainPlayerInput;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00036A9F File Offset: 0x00034C9F
	private static bool OperationPending
	{
		get
		{
			return InputRebinder.operation.started && !InputRebinder.operation.canceled && !InputRebinder.operation.completed;
		}
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x00036ACA File Offset: 0x00034CCA
	private void Awake()
	{
		InputRebinder.Load();
		UIInputManager.OnCancelEarly += this.OnUICancel;
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00036AE2 File Offset: 0x00034CE2
	private void OnDestroy()
	{
		UIInputManager.OnCancelEarly -= this.OnUICancel;
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00036AF5 File Offset: 0x00034CF5
	private void OnUICancel(UIInputEventData data)
	{
		if (InputRebinder.OperationPending)
		{
			data.Use();
		}
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x00036B04 File Offset: 0x00034D04
	public static void Load()
	{
		string text = SavesSystem.LoadGlobal<string>("InputBinding", null);
		string.IsNullOrEmpty(text);
		try
		{
			InputRebinder.PlayerInput.actions.LoadBindingOverridesFromJson(text, true);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			InputRebinder.PlayerInput.actions.RemoveAllBindingOverrides();
		}
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x00036B60 File Offset: 0x00034D60
	public static void Save()
	{
		string text = InputRebinder.PlayerInput.actions.SaveBindingOverridesAsJson();
		SavesSystem.SaveGlobal<string>("InputBinding", text);
		Debug.Log(text);
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x00036B8E File Offset: 0x00034D8E
	public static void Clear()
	{
		InputRebinder.PlayerInput.actions.RemoveAllBindingOverrides();
		Action onBindingChanged = InputRebinder.OnBindingChanged;
		if (onBindingChanged != null)
		{
			onBindingChanged();
		}
		InputIndicator.NotifyBindingChanged();
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x00036BB4 File Offset: 0x00034DB4
	private static void Rebind(string name, int index, string[] excludes = null)
	{
		if (InputRebinder.OperationPending)
		{
			return;
		}
		InputAction inputAction = InputRebinder.PlayerInput.actions[name];
		if (inputAction == null)
		{
			Debug.LogError("找不到名为 " + name + " 的 action");
			return;
		}
		Action<InputAction> onRebindBegin = InputRebinder.OnRebindBegin;
		if (onRebindBegin != null)
		{
			onRebindBegin(inputAction);
		}
		Debug.Log("Resetting");
		InputRebinder.operation.Reset();
		Debug.Log("Settingup");
		inputAction.actionMap.Disable();
		InputRebinder.operation.WithCancelingThrough("<Keyboard>/escape").WithAction(inputAction).WithTargetBinding(index).OnComplete(new Action<InputActionRebindingExtensions.RebindingOperation>(InputRebinder.OnComplete)).OnCancel(new Action<InputActionRebindingExtensions.RebindingOperation>(InputRebinder.OnCancel));
		if (excludes != null)
		{
			foreach (string path in excludes)
			{
				InputRebinder.operation.WithControlsExcluding(path);
			}
		}
		Debug.Log("Starting");
		InputRebinder.operation.Start();
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x00036CA4 File Offset: 0x00034EA4
	public static UniTask<bool> RebindAsync(string name, int index, string[] excludes = null, bool save = false)
	{
		InputRebinder.<RebindAsync>d__20 <RebindAsync>d__;
		<RebindAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<RebindAsync>d__.name = name;
		<RebindAsync>d__.index = index;
		<RebindAsync>d__.excludes = excludes;
		<RebindAsync>d__.save = save;
		<RebindAsync>d__.<>1__state = -1;
		<RebindAsync>d__.<>t__builder.Start<InputRebinder.<RebindAsync>d__20>(ref <RebindAsync>d__);
		return <RebindAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x00036D00 File Offset: 0x00034F00
	private static void OnCancel(InputActionRebindingExtensions.RebindingOperation operation)
	{
		Debug.Log(operation.action.name + " binding canceled");
		operation.action.actionMap.Enable();
		Action<InputAction> onRebindComplete = InputRebinder.OnRebindComplete;
		if (onRebindComplete == null)
		{
			return;
		}
		onRebindComplete(operation.action);
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x00036D4C File Offset: 0x00034F4C
	private static void OnComplete(InputActionRebindingExtensions.RebindingOperation operation)
	{
		Debug.Log(operation.action.name + " bind to " + operation.selectedControl.name);
		operation.action.actionMap.Enable();
		Action<InputAction> onRebindComplete = InputRebinder.OnRebindComplete;
		if (onRebindComplete != null)
		{
			onRebindComplete(operation.action);
		}
		Action onBindingChanged = InputRebinder.OnBindingChanged;
		if (onBindingChanged != null)
		{
			onBindingChanged();
		}
		InputIndicator.NotifyRebindComplete(operation.action);
	}

	// Token: 0x04000B49 RID: 2889
	[Header("Debug")]
	[SerializeField]
	private string action = "MoveAxis";

	// Token: 0x04000B4A RID: 2890
	[SerializeField]
	private int index = 2;

	// Token: 0x04000B4B RID: 2891
	[SerializeField]
	private string[] excludes = new string[]
	{
		"<Mouse>/leftButton",
		"<Mouse>/rightButton",
		"<Pointer>/position",
		"<Pointer>/delta",
		"<Pointer>/Press"
	};

	// Token: 0x04000B4C RID: 2892
	public static Action<InputAction> OnRebindBegin;

	// Token: 0x04000B4D RID: 2893
	public static Action<InputAction> OnRebindComplete;

	// Token: 0x04000B4E RID: 2894
	public static Action OnBindingChanged;

	// Token: 0x04000B4F RID: 2895
	private static InputActionRebindingExtensions.RebindingOperation operation = new InputActionRebindingExtensions.RebindingOperation();

	// Token: 0x04000B50 RID: 2896
	private const string SaveKey = "InputBinding";
}
