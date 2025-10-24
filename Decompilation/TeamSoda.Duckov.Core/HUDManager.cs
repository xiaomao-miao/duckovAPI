using System;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class HUDManager : MonoBehaviour
{
	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06000EE3 RID: 3811 RVA: 0x0003B2DC File Offset: 0x000394DC
	// (remove) Token: 0x06000EE4 RID: 3812 RVA: 0x0003B310 File Offset: 0x00039510
	private static event Action onHideTokensChanged;

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0003B344 File Offset: 0x00039544
	private bool ShouldDisplay
	{
		get
		{
			bool flag = HUDManager.hideTokens.Any((UnityEngine.Object e) => e != null);
			bool flag2 = View.ActiveView != null;
			bool active = DialogueUI.Active;
			bool flag3 = CustomFaceUI.ActiveView != null;
			bool active2 = CameraMode.Active;
			return !flag && !flag2 && !active && !flag3 && !active2;
		}
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0003B3B0 File Offset: 0x000395B0
	private void Awake()
	{
		View.OnActiveViewChanged += this.OnActiveViewChanged;
		DialogueUI.OnDialogueStatusChanged += this.OnDialogueStatusChanged;
		CustomFaceUI.OnCustomUIViewChanged += this.OnCustomFaceViewChange;
		CameraMode.OnCameraModeChanged = (Action<bool>)Delegate.Combine(CameraMode.OnCameraModeChanged, new Action<bool>(this.OnCameraModeChanged));
		HUDManager.onHideTokensChanged += this.OnHideTokensChanged;
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0003B424 File Offset: 0x00039624
	private void OnDestroy()
	{
		View.OnActiveViewChanged -= this.OnActiveViewChanged;
		DialogueUI.OnDialogueStatusChanged -= this.OnDialogueStatusChanged;
		CustomFaceUI.OnCustomUIViewChanged -= this.OnCustomFaceViewChange;
		CameraMode.OnCameraModeChanged = (Action<bool>)Delegate.Remove(CameraMode.OnCameraModeChanged, new Action<bool>(this.OnCameraModeChanged));
		HUDManager.onHideTokensChanged -= this.OnHideTokensChanged;
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0003B495 File Offset: 0x00039695
	private void OnHideTokensChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0003B49D File Offset: 0x0003969D
	private void OnCameraModeChanged(bool value)
	{
		this.Refresh();
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0003B4A5 File Offset: 0x000396A5
	private void OnDialogueStatusChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0003B4AD File Offset: 0x000396AD
	private void OnActiveViewChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0003B4B5 File Offset: 0x000396B5
	private void OnCustomFaceViewChange()
	{
		this.Refresh();
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0003B4C0 File Offset: 0x000396C0
	private void Refresh()
	{
		if (this.ShouldDisplay)
		{
			this.canvasGroup.blocksRaycasts = true;
			if (this.fadeGroup.IsShown)
			{
				return;
			}
			this.fadeGroup.Show();
			return;
		}
		else
		{
			this.canvasGroup.blocksRaycasts = false;
			if (this.fadeGroup.IsHidden)
			{
				return;
			}
			this.fadeGroup.Hide();
			return;
		}
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0003B520 File Offset: 0x00039720
	public static void RegisterHideToken(UnityEngine.Object obj)
	{
		HUDManager.hideTokens.Add(obj);
		Action action = HUDManager.onHideTokensChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0003B53C File Offset: 0x0003973C
	public static void UnregisterHideToken(UnityEngine.Object obj)
	{
		HUDManager.hideTokens.Remove(obj);
		Action action = HUDManager.onHideTokensChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x04000C45 RID: 3141
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000C46 RID: 3142
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000C47 RID: 3143
	private static List<UnityEngine.Object> hideTokens = new List<UnityEngine.Object>();
}
