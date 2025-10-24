using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x02000380 RID: 896
	public class Tooltips : MonoBehaviour
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x0006CE96 File Offset: 0x0006B096
		// (set) Token: 0x06001F10 RID: 7952 RVA: 0x0006CE9D File Offset: 0x0006B09D
		public static ITooltipsProvider CurrentProvider { get; private set; }

		// Token: 0x06001F11 RID: 7953 RVA: 0x0006CEA5 File Offset: 0x0006B0A5
		public static void NotifyEnterTooltipsProvider(ITooltipsProvider provider)
		{
			Tooltips.CurrentProvider = provider;
			Action<ITooltipsProvider> onEnterProvider = Tooltips.OnEnterProvider;
			if (onEnterProvider == null)
			{
				return;
			}
			onEnterProvider(provider);
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0006CEBD File Offset: 0x0006B0BD
		public static void NotifyExitTooltipsProvider(ITooltipsProvider provider)
		{
			if (Tooltips.CurrentProvider != provider)
			{
				return;
			}
			Tooltips.CurrentProvider = null;
			Action<ITooltipsProvider> onExitProvider = Tooltips.OnExitProvider;
			if (onExitProvider == null)
			{
				return;
			}
			onExitProvider(provider);
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x0006CEE0 File Offset: 0x0006B0E0
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			Tooltips.OnEnterProvider = (Action<ITooltipsProvider>)Delegate.Combine(Tooltips.OnEnterProvider, new Action<ITooltipsProvider>(this.DoOnEnterProvider));
			Tooltips.OnExitProvider = (Action<ITooltipsProvider>)Delegate.Combine(Tooltips.OnExitProvider, new Action<ITooltipsProvider>(this.DoOnExitProvider));
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0006CF48 File Offset: 0x0006B148
		private void OnDestroy()
		{
			Tooltips.OnEnterProvider = (Action<ITooltipsProvider>)Delegate.Remove(Tooltips.OnEnterProvider, new Action<ITooltipsProvider>(this.DoOnEnterProvider));
			Tooltips.OnExitProvider = (Action<ITooltipsProvider>)Delegate.Remove(Tooltips.OnExitProvider, new Action<ITooltipsProvider>(this.DoOnExitProvider));
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0006CF95 File Offset: 0x0006B195
		private void Update()
		{
			if (this.contents.gameObject.activeSelf)
			{
				this.RefreshPosition();
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0006CFAF File Offset: 0x0006B1AF
		private void DoOnExitProvider(ITooltipsProvider provider)
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0006CFBC File Offset: 0x0006B1BC
		private void DoOnEnterProvider(ITooltipsProvider provider)
		{
			this.text.text = provider.GetTooltipsText();
			this.fadeGroup.Show();
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0006CFDC File Offset: 0x0006B1DC
		private unsafe void RefreshPosition()
		{
			Vector2 screenPoint = *Mouse.current.position.value;
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPoint, null, out v);
			this.contents.localPosition = v;
		}

		// Token: 0x0400153D RID: 5437
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x0400153E RID: 5438
		[SerializeField]
		private RectTransform contents;

		// Token: 0x0400153F RID: 5439
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001540 RID: 5440
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001542 RID: 5442
		private static Action<ITooltipsProvider> OnEnterProvider;

		// Token: 0x04001543 RID: 5443
		private static Action<ITooltipsProvider> OnExitProvider;
	}
}
