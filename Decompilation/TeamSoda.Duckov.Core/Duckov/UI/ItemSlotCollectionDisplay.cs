using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039B RID: 923
	public class ItemSlotCollectionDisplay : MonoBehaviour
	{
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x0007263B File Offset: 0x0007083B
		// (set) Token: 0x060020C3 RID: 8387 RVA: 0x00072643 File Offset: 0x00070843
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			internal set
			{
				this.editable = value;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060020C4 RID: 8388 RVA: 0x0007264C File Offset: 0x0007084C
		// (set) Token: 0x060020C5 RID: 8389 RVA: 0x00072654 File Offset: 0x00070854
		public bool ContentSelectable
		{
			get
			{
				return this.contentSelectable;
			}
			set
			{
				this.contentSelectable = value;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060020C6 RID: 8390 RVA: 0x0007265D File Offset: 0x0007085D
		public bool ShowOperationMenu
		{
			get
			{
				return this.showOperationMenu;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x00072665 File Offset: 0x00070865
		// (set) Token: 0x060020C8 RID: 8392 RVA: 0x0007266D File Offset: 0x0007086D
		public bool Movable { get; private set; }

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x00072676 File Offset: 0x00070876
		// (set) Token: 0x060020CA RID: 8394 RVA: 0x0007267E File Offset: 0x0007087E
		public Item Target { get; private set; }

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x060020CB RID: 8395 RVA: 0x00072688 File Offset: 0x00070888
		// (remove) Token: 0x060020CC RID: 8396 RVA: 0x000726C0 File Offset: 0x000708C0
		public event Action<ItemSlotCollectionDisplay, SlotDisplay> onElementClicked;

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x060020CD RID: 8397 RVA: 0x000726F8 File Offset: 0x000708F8
		// (remove) Token: 0x060020CE RID: 8398 RVA: 0x00072730 File Offset: 0x00070930
		public event Action<ItemSlotCollectionDisplay, SlotDisplay> onElementDoubleClicked;

		// Token: 0x060020CF RID: 8399 RVA: 0x00072768 File Offset: 0x00070968
		public void Setup(Item target, bool movable = false)
		{
			this.Target = target;
			this.Clear();
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Slots == null)
			{
				return;
			}
			this.Movable = movable;
			for (int i = 0; i < this.Target.Slots.Count; i++)
			{
				Slot slot = this.Target.Slots[i];
				if (slot != null)
				{
					SlotDisplay slotDisplay = SlotDisplay.Get();
					slotDisplay.onSlotDisplayClicked += this.OnSlotDisplayClicked;
					slotDisplay.onSlotDisplayDoubleClicked += this.OnSlotDisplayDoubleClicked;
					slotDisplay.ShowOperationMenu = this.ShowOperationMenu;
					slotDisplay.Setup(slot);
					slotDisplay.Editable = this.editable;
					slotDisplay.ContentSelectable = this.contentSelectable;
					slotDisplay.transform.SetParent(this.entriesParent, false);
					slotDisplay.Movable = this.Movable;
					this.slots.Add(slotDisplay);
				}
			}
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00072861 File Offset: 0x00070A61
		private void OnSlotDisplayDoubleClicked(SlotDisplay display)
		{
			Action<ItemSlotCollectionDisplay, SlotDisplay> action = this.onElementDoubleClicked;
			if (action == null)
			{
				return;
			}
			action(this, display);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00072878 File Offset: 0x00070A78
		private void Clear()
		{
			foreach (SlotDisplay slotDisplay in this.slots)
			{
				slotDisplay.onSlotDisplayClicked -= this.OnSlotDisplayClicked;
				SlotDisplay.Release(slotDisplay);
			}
			this.slots.Clear();
			this.entriesParent.DestroyAllChildren();
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000728F0 File Offset: 0x00070AF0
		private void OnSlotDisplayClicked(SlotDisplay display)
		{
			Action<ItemSlotCollectionDisplay, SlotDisplay> action = this.onElementClicked;
			if (action != null)
			{
				action(this, display);
			}
			if (!this.editable && this.notifyNotEditable)
			{
				this.ShowNotEditableIndicator().Forget();
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00072920 File Offset: 0x00070B20
		private UniTask ShowNotEditableIndicator()
		{
			ItemSlotCollectionDisplay.<ShowNotEditableIndicator>d__36 <ShowNotEditableIndicator>d__;
			<ShowNotEditableIndicator>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowNotEditableIndicator>d__.<>4__this = this;
			<ShowNotEditableIndicator>d__.<>1__state = -1;
			<ShowNotEditableIndicator>d__.<>t__builder.Start<ItemSlotCollectionDisplay.<ShowNotEditableIndicator>d__36>(ref <ShowNotEditableIndicator>d__);
			return <ShowNotEditableIndicator>d__.<>t__builder.Task;
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000729A1 File Offset: 0x00070BA1
		[CompilerGenerated]
		private bool <ShowNotEditableIndicator>g__TokenChanged|36_0(ref ItemSlotCollectionDisplay.<>c__DisplayClass36_0 A_1)
		{
			return A_1.token != this.currentToken;
		}

		// Token: 0x0400164B RID: 5707
		[SerializeField]
		private Transform entriesParent;

		// Token: 0x0400164C RID: 5708
		[SerializeField]
		private CanvasGroup notEditableIndicator;

		// Token: 0x0400164D RID: 5709
		[SerializeField]
		private bool editable = true;

		// Token: 0x0400164E RID: 5710
		[SerializeField]
		private bool contentSelectable = true;

		// Token: 0x0400164F RID: 5711
		[SerializeField]
		private bool showOperationMenu = true;

		// Token: 0x04001650 RID: 5712
		[SerializeField]
		private bool notifyNotEditable;

		// Token: 0x04001651 RID: 5713
		[SerializeField]
		private float fadeDuration = 1f;

		// Token: 0x04001652 RID: 5714
		[SerializeField]
		private float sustainDuration = 1f;

		// Token: 0x04001655 RID: 5717
		private List<SlotDisplay> slots = new List<SlotDisplay>();

		// Token: 0x04001658 RID: 5720
		private int currentToken;
	}
}
