using System;
using Duckov.UI;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class CameraMode : MonoBehaviour
{
	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000C51 RID: 3153 RVA: 0x00033E3C File Offset: 0x0003203C
	// (set) Token: 0x06000C52 RID: 3154 RVA: 0x00033E43 File Offset: 0x00032043
	public static CameraMode Instance { get; private set; }

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000C53 RID: 3155 RVA: 0x00033E4B File Offset: 0x0003204B
	public static bool Active
	{
		get
		{
			return !(CameraMode.Instance == null) && CameraMode.Instance.active;
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00033E68 File Offset: 0x00032068
	private void Awake()
	{
		if (CameraMode.Instance != null)
		{
			Debug.LogError("检测到多个Camera Mode", base.gameObject);
			return;
		}
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		CameraMode.Instance = this;
		UIInputManager.OnToggleCameraMode += this.OnToggleCameraMode;
		UIInputManager.OnCancel += this.OnUICancel;
		ManagedUIElement.onOpen += this.OnViewOpen;
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00033EDC File Offset: 0x000320DC
	private void OnDestroy()
	{
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		UIInputManager.OnToggleCameraMode -= this.OnToggleCameraMode;
		UIInputManager.OnCancel -= this.OnUICancel;
		ManagedUIElement.onOpen -= this.OnViewOpen;
		Shader.SetGlobalFloat("CameraModeOn", 0f);
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00033F3A File Offset: 0x0003213A
	private void OnViewOpen(ManagedUIElement element)
	{
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00033F48 File Offset: 0x00032148
	private void OnUICancel(UIInputEventData data)
	{
		if (data.Used)
		{
			return;
		}
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
			data.Use();
		}
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00033F65 File Offset: 0x00032165
	private void OnToggleCameraMode(UIInputEventData data)
	{
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
		}
		else
		{
			CameraMode.Activate();
		}
		data.Use();
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00033F80 File Offset: 0x00032180
	private void MActivate()
	{
		if (View.ActiveView != null)
		{
			return;
		}
		this.active = true;
		Shader.SetGlobalFloat("CameraModeOn", 1f);
		Action onCameraModeActivated = CameraMode.OnCameraModeActivated;
		if (onCameraModeActivated != null)
		{
			onCameraModeActivated();
		}
		Action<bool> onCameraModeChanged = CameraMode.OnCameraModeChanged;
		if (onCameraModeChanged == null)
		{
			return;
		}
		onCameraModeChanged(this.active);
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00033FD6 File Offset: 0x000321D6
	private void MDeactivate()
	{
		this.active = false;
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		Action onCameraModeDeactivated = CameraMode.OnCameraModeDeactivated;
		if (onCameraModeDeactivated != null)
		{
			onCameraModeDeactivated();
		}
		Action<bool> onCameraModeChanged = CameraMode.OnCameraModeChanged;
		if (onCameraModeChanged == null)
		{
			return;
		}
		onCameraModeChanged(this.active);
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00034013 File Offset: 0x00032213
	public static void Activate()
	{
		if (CameraMode.Instance == null)
		{
			return;
		}
		Shader.SetGlobalFloat("CameraModeOn", 1f);
		CameraMode.Instance.MActivate();
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0003403C File Offset: 0x0003223C
	public static void Deactivate()
	{
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		if (CameraMode.Instance == null)
		{
			return;
		}
		CameraMode.Instance.MDeactivate();
	}

	// Token: 0x04000AA8 RID: 2728
	public static Action OnCameraModeActivated;

	// Token: 0x04000AA9 RID: 2729
	public static Action OnCameraModeDeactivated;

	// Token: 0x04000AAA RID: 2730
	public static Action<bool> OnCameraModeChanged;

	// Token: 0x04000AAB RID: 2731
	private bool active;
}
