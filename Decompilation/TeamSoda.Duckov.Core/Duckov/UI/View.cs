using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C0 RID: 960
	public abstract class View : ManagedUIElement
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0007A405 File Offset: 0x00078605
		// (set) Token: 0x060022E8 RID: 8936 RVA: 0x0007A40C File Offset: 0x0007860C
		public static View ActiveView
		{
			get
			{
				return View._activeView;
			}
			private set
			{
				UnityEngine.Object activeView = View._activeView;
				View._activeView = value;
				if (activeView != View._activeView)
				{
					Action onActiveViewChanged = View.OnActiveViewChanged;
					if (onActiveViewChanged == null)
					{
						return;
					}
					onActiveViewChanged();
				}
			}
		}

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x060022E9 RID: 8937 RVA: 0x0007A434 File Offset: 0x00078634
		// (remove) Token: 0x060022EA RID: 8938 RVA: 0x0007A468 File Offset: 0x00078668
		public static event Action OnActiveViewChanged;

		// Token: 0x060022EB RID: 8939 RVA: 0x0007A49C File Offset: 0x0007869C
		protected override void Awake()
		{
			base.Awake();
			if (this.exitButton != null)
			{
				this.exitButton.onClick.AddListener(new UnityAction(base.Close));
			}
			UIInputManager.OnNavigate += this.OnNavigate;
			UIInputManager.OnConfirm += this.OnConfirm;
			UIInputManager.OnCancel += this.OnCancel;
			this.viewTabs = base.transform.parent.parent.GetComponent<ViewTabs>();
			if (this.autoClose)
			{
				base.Close();
			}
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0007A535 File Offset: 0x00078735
		protected override void OnDestroy()
		{
			base.OnDestroy();
			UIInputManager.OnNavigate -= this.OnNavigate;
			UIInputManager.OnConfirm -= this.OnConfirm;
			UIInputManager.OnCancel -= this.OnCancel;
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x0007A570 File Offset: 0x00078770
		protected override void OnOpen()
		{
			this.autoClose = false;
			if (View.ActiveView != null && View.ActiveView != this)
			{
				View.ActiveView.Close();
			}
			View.ActiveView = this;
			ItemUIUtilities.Select(null);
			if (this.viewTabs != null)
			{
				this.viewTabs.Show();
			}
			if (base.gameObject == null)
			{
				Debug.LogError("GameObject不存在", base.gameObject);
			}
			InputManager.DisableInput(base.gameObject);
			AudioManager.Post(this.sfx_Open);
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x0007A602 File Offset: 0x00078802
		protected override void OnClose()
		{
			if (View.ActiveView == this)
			{
				View.ActiveView = null;
			}
			InputManager.ActiveInput(base.gameObject);
			AudioManager.Post(this.sfx_Close);
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x0007A62E File Offset: 0x0007882E
		internal virtual void TryQuit()
		{
			base.Close();
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x0007A636 File Offset: 0x00078836
		public void OnNavigate(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView != this)
			{
				return;
			}
			this.OnNavigate(eventData.vector);
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x0007A65B File Offset: 0x0007885B
		public void OnConfirm(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView != this)
			{
				return;
			}
			this.OnConfirm();
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x0007A67A File Offset: 0x0007887A
		public void OnCancel(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView == null || View.ActiveView != this)
			{
				return;
			}
			this.OnCancel();
			if (!eventData.Used)
			{
				this.TryQuit();
				eventData.Use();
			}
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0007A6BA File Offset: 0x000788BA
		protected virtual void OnNavigate(Vector2 vector)
		{
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0007A6BC File Offset: 0x000788BC
		protected virtual void OnConfirm()
		{
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x0007A6BE File Offset: 0x000788BE
		protected virtual void OnCancel()
		{
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0007A6C0 File Offset: 0x000788C0
		protected static T GetViewInstance<T>() where T : View
		{
			return GameplayUIManager.GetViewInstance<T>();
		}

		// Token: 0x040017BA RID: 6074
		[HideInInspector]
		private static View _activeView;

		// Token: 0x040017BC RID: 6076
		[SerializeField]
		private ViewTabs viewTabs;

		// Token: 0x040017BD RID: 6077
		[SerializeField]
		private Button exitButton;

		// Token: 0x040017BE RID: 6078
		[SerializeField]
		private string sfx_Open;

		// Token: 0x040017BF RID: 6079
		[SerializeField]
		private string sfx_Close;

		// Token: 0x040017C0 RID: 6080
		private bool autoClose = true;
	}
}
