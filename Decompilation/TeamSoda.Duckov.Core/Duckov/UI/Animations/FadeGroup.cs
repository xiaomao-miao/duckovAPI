using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D6 RID: 982
	public class FadeGroup : MonoBehaviour
	{
		// Token: 0x140000ED RID: 237
		// (add) Token: 0x060023BD RID: 9149 RVA: 0x0007CB54 File Offset: 0x0007AD54
		// (remove) Token: 0x060023BE RID: 9150 RVA: 0x0007CB8C File Offset: 0x0007AD8C
		public event Action<FadeGroup> OnFadeComplete;

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x060023BF RID: 9151 RVA: 0x0007CBC4 File Offset: 0x0007ADC4
		// (remove) Token: 0x060023C0 RID: 9152 RVA: 0x0007CBFC File Offset: 0x0007ADFC
		public event Action<FadeGroup> OnShowComplete;

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x060023C1 RID: 9153 RVA: 0x0007CC34 File Offset: 0x0007AE34
		// (remove) Token: 0x060023C2 RID: 9154 RVA: 0x0007CC6C File Offset: 0x0007AE6C
		public event Action<FadeGroup> OnHideComplete;

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060023C3 RID: 9155 RVA: 0x0007CCA1 File Offset: 0x0007AEA1
		public bool IsHidingInProgress
		{
			get
			{
				return this.isHidingInProgress;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x0007CCA9 File Offset: 0x0007AEA9
		public bool IsShowingInProgress
		{
			get
			{
				return this.isShowingInProgress;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060023C5 RID: 9157 RVA: 0x0007CCB1 File Offset: 0x0007AEB1
		public bool IsShown
		{
			get
			{
				return this.isShown;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x0007CCB9 File Offset: 0x0007AEB9
		public bool IsHidden
		{
			get
			{
				return !this.isShown;
			}
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0007CCC4 File Offset: 0x0007AEC4
		private void Start()
		{
			if (this.skipHideOnStart)
			{
				this.SkipHide();
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x0007CCD4 File Offset: 0x0007AED4
		private void OnEnable()
		{
			if (this.showOnEnable)
			{
				this.Show();
			}
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x0007CCE4 File Offset: 0x0007AEE4
		[ContextMenu("Show")]
		public void Show()
		{
			if (this.debug)
			{
				Debug.Log("Fadegroup SHOW " + base.name);
			}
			this.skipHideOnStart = false;
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
			this.ShowTask().Forget();
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0007CD34 File Offset: 0x0007AF34
		[ContextMenu("Hide")]
		public void Hide()
		{
			if (this.debug)
			{
				Debug.Log("Fadegroup HIDE " + base.name, base.gameObject);
			}
			this.HideTask().Forget();
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0007CD64 File Offset: 0x0007AF64
		public void Toggle()
		{
			if (this.IsShown)
			{
				this.Hide();
				return;
			}
			if (this.IsHidden)
			{
				this.Show();
			}
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0007CD83 File Offset: 0x0007AF83
		public UniTask ShowAndReturnTask()
		{
			if (this.skipHideBeforeShow)
			{
				this.SkipHide();
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
			return this.ShowTask();
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0007CDAD File Offset: 0x0007AFAD
		public UniTask HideAndReturnTask()
		{
			return this.HideTask();
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0007CDB5 File Offset: 0x0007AFB5
		private int CacheNewTaskToken()
		{
			this.activeTaskToken = UnityEngine.Random.Range(0, int.MaxValue);
			return this.activeTaskToken;
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x0007CDD0 File Offset: 0x0007AFD0
		public UniTask ShowTask()
		{
			FadeGroup.<ShowTask>d__35 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<FadeGroup.<ShowTask>d__35>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x0007CE14 File Offset: 0x0007B014
		public UniTask HideTask()
		{
			FadeGroup.<HideTask>d__36 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<FadeGroup.<HideTask>d__36>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0007CE57 File Offset: 0x0007B057
		private void ShowComplete()
		{
			this.isShowingInProgress = false;
			Action<FadeGroup> onFadeComplete = this.OnFadeComplete;
			if (onFadeComplete != null)
			{
				onFadeComplete(this);
			}
			Action<FadeGroup> onShowComplete = this.OnShowComplete;
			if (onShowComplete == null)
			{
				return;
			}
			onShowComplete(this);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0007CE84 File Offset: 0x0007B084
		private void HideComplete()
		{
			this.isHidingInProgress = false;
			Action<FadeGroup> onFadeComplete = this.OnFadeComplete;
			if (onFadeComplete != null)
			{
				onFadeComplete(this);
			}
			Action<FadeGroup> onHideComplete = this.OnHideComplete;
			if (onHideComplete != null)
			{
				onHideComplete(this);
			}
			if (this == null)
			{
				return;
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0007CEDC File Offset: 0x0007B0DC
		public void SkipHide()
		{
			foreach (FadeElement fadeElement in this.fadeElements)
			{
				if (fadeElement == null)
				{
					Debug.LogWarning("Element in fade group " + base.name + " is null");
				}
				else
				{
					fadeElement.SkipHide();
				}
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x0007CF68 File Offset: 0x0007B168
		public bool IsFading
		{
			get
			{
				return this.fadeElements.Any((FadeElement e) => e != null && e.IsFading);
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0007CF94 File Offset: 0x0007B194
		internal void SkipShow()
		{
			foreach (FadeElement fadeElement in this.fadeElements)
			{
				if (fadeElement == null)
				{
					Debug.LogWarning("Element in fade group " + base.name + " is null");
				}
				else
				{
					fadeElement.SkipShow();
				}
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x0400183C RID: 6204
		[SerializeField]
		private List<FadeElement> fadeElements = new List<FadeElement>();

		// Token: 0x0400183D RID: 6205
		[SerializeField]
		private bool skipHideOnStart = true;

		// Token: 0x0400183E RID: 6206
		[SerializeField]
		private bool showOnEnable;

		// Token: 0x0400183F RID: 6207
		[SerializeField]
		private bool skipHideBeforeShow = true;

		// Token: 0x04001843 RID: 6211
		public bool manageGameObjectActive;

		// Token: 0x04001844 RID: 6212
		private bool isHidingInProgress;

		// Token: 0x04001845 RID: 6213
		private bool isShowingInProgress;

		// Token: 0x04001846 RID: 6214
		private bool isShown;

		// Token: 0x04001847 RID: 6215
		public bool debug;

		// Token: 0x04001848 RID: 6216
		private int activeTaskToken;
	}
}
