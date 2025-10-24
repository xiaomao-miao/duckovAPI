using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Economy;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000202 RID: 514
public class ItemRepair_RepairAllPanel : MonoBehaviour
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0003B8E0 File Offset: 0x00039AE0
	private PrefabPool<ItemDisplay> Pool
	{
		get
		{
			if (this._pool == null)
			{
				this._pool = new PrefabPool<ItemDisplay>(this.itemDisplayTemplate, null, null, null, null, true, 10, 10000, delegate(ItemDisplay e)
				{
					e.onPointerClick += this.OnPointerClickEntry;
				});
			}
			return this._pool;
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0003B924 File Offset: 0x00039B24
	private void OnPointerClickEntry(ItemDisplay display, PointerEventData data)
	{
		data.Use();
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0003B92C File Offset: 0x00039B2C
	private void Awake()
	{
		this.itemDisplayTemplate.gameObject.SetActive(false);
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x0003B95C File Offset: 0x00039B5C
	private void OnButtonClicked()
	{
		if (this.master == null)
		{
			return;
		}
		List<Item> allEquippedItems = this.master.GetAllEquippedItems();
		this.master.RepairItems(allEquippedItems);
		this.needsRefresh = true;
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0003B997 File Offset: 0x00039B97
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		ItemRepairView.OnRepaireOptionDone += this.OnRepairOptionDone;
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0003B9BB File Offset: 0x00039BBB
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
		ItemRepairView.OnRepaireOptionDone -= this.OnRepairOptionDone;
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0003B9DF File Offset: 0x00039BDF
	public void Setup(ItemRepairView master)
	{
		this.master = master;
		this.Refresh();
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x0003B9EE File Offset: 0x00039BEE
	private void OnPlayerItemOperation()
	{
		this.needsRefresh = true;
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x0003B9F7 File Offset: 0x00039BF7
	private void OnRepairOptionDone()
	{
		this.needsRefresh = true;
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0003BA00 File Offset: 0x00039C00
	private void Refresh()
	{
		this.needsRefresh = false;
		this.Pool.ReleaseAll();
		List<Item> list = (from e in this.master.GetAllEquippedItems()
		where e.Durability < e.MaxDurabilityWithLoss
		select e).ToList<Item>();
		int num = 0;
		if (list != null && list.Count > 0)
		{
			foreach (Item target in list)
			{
				this.Pool.Get(null).Setup(target);
			}
			num = this.master.CalculateRepairPrice(list);
			this.placeholder.SetActive(false);
			Cost cost = new Cost((long)num);
			bool enough = cost.Enough;
			this.button.interactable = enough;
		}
		else
		{
			this.placeholder.SetActive(true);
			this.button.interactable = false;
		}
		this.priceDisplay.text = num.ToString();
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0003BB18 File Offset: 0x00039D18
	private void Update()
	{
		if (this.needsRefresh)
		{
			this.Refresh();
		}
	}

	// Token: 0x04000C55 RID: 3157
	[SerializeField]
	private ItemRepairView master;

	// Token: 0x04000C56 RID: 3158
	[SerializeField]
	private TextMeshProUGUI priceDisplay;

	// Token: 0x04000C57 RID: 3159
	[SerializeField]
	private ItemDisplay itemDisplayTemplate;

	// Token: 0x04000C58 RID: 3160
	[SerializeField]
	private Button button;

	// Token: 0x04000C59 RID: 3161
	[SerializeField]
	private GameObject placeholder;

	// Token: 0x04000C5A RID: 3162
	private PrefabPool<ItemDisplay> _pool;

	// Token: 0x04000C5B RID: 3163
	private bool needsRefresh;
}
