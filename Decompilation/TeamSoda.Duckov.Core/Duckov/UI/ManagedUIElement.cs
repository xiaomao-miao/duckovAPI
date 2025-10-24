using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A6 RID: 934
	public abstract class ManagedUIElement : MonoBehaviour
	{
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x00074E64 File Offset: 0x00073064
		// (set) Token: 0x06002171 RID: 8561 RVA: 0x00074E6C File Offset: 0x0007306C
		public bool open { get; private set; }

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06002172 RID: 8562 RVA: 0x00074E78 File Offset: 0x00073078
		// (remove) Token: 0x06002173 RID: 8563 RVA: 0x00074EAC File Offset: 0x000730AC
		public static event Action<ManagedUIElement> onOpen;

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06002174 RID: 8564 RVA: 0x00074EE0 File Offset: 0x000730E0
		// (remove) Token: 0x06002175 RID: 8565 RVA: 0x00074F14 File Offset: 0x00073114
		public static event Action<ManagedUIElement> onClose;

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x00074F47 File Offset: 0x00073147
		protected virtual bool ShowOpenCloseButtons
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x00074F4A File Offset: 0x0007314A
		protected virtual void Awake()
		{
			this.RegisterEvents();
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x00074F52 File Offset: 0x00073152
		protected virtual void OnDestroy()
		{
			this.UnregisterEvents();
			if (this.open)
			{
				this.Close();
			}
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x00074F68 File Offset: 0x00073168
		public void Open(ManagedUIElement parent = null)
		{
			this.open = true;
			this.parent = parent;
			Action<ManagedUIElement> action = ManagedUIElement.onOpen;
			if (action != null)
			{
				action(this);
			}
			this.OnOpen();
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00074F8F File Offset: 0x0007318F
		public void Close()
		{
			this.open = false;
			this.parent = null;
			Action<ManagedUIElement> action = ManagedUIElement.onClose;
			if (action != null)
			{
				action(this);
			}
			this.OnClose();
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x00074FB6 File Offset: 0x000731B6
		private void RegisterEvents()
		{
			ManagedUIElement.onOpen += this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose += this.OnManagedUIBehaviorClose;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x00074FDA File Offset: 0x000731DA
		private void UnregisterEvents()
		{
			ManagedUIElement.onOpen -= this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose -= this.OnManagedUIBehaviorClose;
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00074FFE File Offset: 0x000731FE
		private void OnManagedUIBehaviorClose(ManagedUIElement obj)
		{
			if (obj != null && obj == this.parent)
			{
				this.Close();
			}
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x0007501D File Offset: 0x0007321D
		private void OnManagedUIBehaviorOpen(ManagedUIElement obj)
		{
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x0007501F File Offset: 0x0007321F
		protected virtual void OnOpen()
		{
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x00075021 File Offset: 0x00073221
		protected virtual void OnClose()
		{
		}

		// Token: 0x040016AC RID: 5804
		[SerializeField]
		private ManagedUIElement parent;
	}
}
