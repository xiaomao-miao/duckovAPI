using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A8 RID: 936
	public class GameplayUIManager : MonoBehaviour
	{
		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x00075033 File Offset: 0x00073233
		public static GameplayUIManager Instance
		{
			get
			{
				return GameplayUIManager.instance;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x0007503A File Offset: 0x0007323A
		public View ActiveView
		{
			get
			{
				return View.ActiveView;
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00075044 File Offset: 0x00073244
		public static T GetViewInstance<T>() where T : View
		{
			if (GameplayUIManager.Instance == null)
			{
				return default(T);
			}
			View view;
			if (GameplayUIManager.Instance.viewDic.TryGetValue(typeof(T), out view))
			{
				return view as T;
			}
			View view2 = GameplayUIManager.Instance.views.Find((View e) => e is T);
			if (view2 == null)
			{
				return default(T);
			}
			GameplayUIManager.Instance.viewDic[typeof(T)] = view2;
			return view2 as T;
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000750F8 File Offset: 0x000732F8
		private void Awake()
		{
			if (GameplayUIManager.instance == null)
			{
				GameplayUIManager.instance = this;
			}
			else
			{
				Debug.LogWarning("Duplicate Gameplay UI Manager detected!");
			}
			foreach (View view in this.views)
			{
				view.gameObject.SetActive(true);
			}
			foreach (GameObject gameObject in this.setActiveOnAwake)
			{
				if (!(gameObject == null))
				{
					gameObject.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000751C0 File Offset: 0x000733C0
		public PrefabPool<ItemDisplay> ItemDisplayPool
		{
			get
			{
				if (this.itemDisplayPool == null)
				{
					this.itemDisplayPool = new PrefabPool<ItemDisplay>(GameplayDataSettings.UIPrefabs.ItemDisplay, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.itemDisplayPool;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x00075204 File Offset: 0x00073404
		public PrefabPool<SlotDisplay> SlotDisplayPool
		{
			get
			{
				if (this.slotDisplayPool == null)
				{
					this.slotDisplayPool = new PrefabPool<SlotDisplay>(GameplayDataSettings.UIPrefabs.SlotDisplay, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.slotDisplayPool;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x00075248 File Offset: 0x00073448
		public PrefabPool<InventoryEntry> InventoryEntryPool
		{
			get
			{
				if (this.inventoryEntryPool == null)
				{
					this.inventoryEntryPool = new PrefabPool<InventoryEntry>(GameplayDataSettings.UIPrefabs.InventoryEntry, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.inventoryEntryPool;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x0007528A File Offset: 0x0007348A
		public SplitDialogue SplitDialogue
		{
			get
			{
				return this._splitDialogue;
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x00075292 File Offset: 0x00073492
		public static UniTask TemporaryHide()
		{
			if (GameplayUIManager.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			GameplayUIManager.Instance.canvasGroup.blocksRaycasts = false;
			return GameplayUIManager.Instance.fadeGroup.HideAndReturnTask();
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000752C6 File Offset: 0x000734C6
		public static UniTask ReverseTemporaryHide()
		{
			if (GameplayUIManager.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			GameplayUIManager.Instance.canvasGroup.blocksRaycasts = true;
			return GameplayUIManager.Instance.fadeGroup.ShowAndReturnTask();
		}

		// Token: 0x040016B0 RID: 5808
		private static GameplayUIManager instance;

		// Token: 0x040016B1 RID: 5809
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040016B2 RID: 5810
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040016B3 RID: 5811
		[SerializeField]
		private List<View> views = new List<View>();

		// Token: 0x040016B4 RID: 5812
		[SerializeField]
		private List<GameObject> setActiveOnAwake;

		// Token: 0x040016B5 RID: 5813
		private Dictionary<Type, View> viewDic = new Dictionary<Type, View>();

		// Token: 0x040016B6 RID: 5814
		private PrefabPool<ItemDisplay> itemDisplayPool;

		// Token: 0x040016B7 RID: 5815
		private PrefabPool<SlotDisplay> slotDisplayPool;

		// Token: 0x040016B8 RID: 5816
		private PrefabPool<InventoryEntry> inventoryEntryPool;

		// Token: 0x040016B9 RID: 5817
		[SerializeField]
		private SplitDialogue _splitDialogue;
	}
}
