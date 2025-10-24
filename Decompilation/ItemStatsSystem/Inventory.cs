using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem.Items;
using Sirenix.OdinInspector;
using SodaCraft.Localizations;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200001A RID: 26
	public class Inventory : MonoBehaviour, ISelfValidator, IEnumerable<Item>, IEnumerable
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004038 File Offset: 0x00002238
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00004040 File Offset: 0x00002240
		public bool Loading
		{
			get
			{
				return this.loading;
			}
			set
			{
				this.loading = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004049 File Offset: 0x00002249
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00004064 File Offset: 0x00002264
		public string DisplayNameKey
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.displayNameKey))
				{
					return "UI_InventoryDefault";
				}
				return this.displayNameKey;
			}
			set
			{
				this.displayNameKey = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000406D File Offset: 0x0000226D
		public string DisplayName
		{
			get
			{
				return this.DisplayNameKey.ToPlainText();
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000407A File Offset: 0x0000227A
		public List<Item> Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004082 File Offset: 0x00002282
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000408A File Offset: 0x0000228A
		public bool AcceptSticky
		{
			get
			{
				return this.acceptSticky;
			}
			set
			{
				this.acceptSticky = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000409C File Offset: 0x0000229C
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00004093 File Offset: 0x00002293
		public bool NeedInspection
		{
			get
			{
				return this.needInspection;
			}
			set
			{
				this.needInspection = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000040A4 File Offset: 0x000022A4
		public int Capacity
		{
			get
			{
				return this.defaultCapacity;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000040AC File Offset: 0x000022AC
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000040B4 File Offset: 0x000022B4
		public Item AttachedToItem
		{
			get
			{
				return this.attachedToItem;
			}
			internal set
			{
				this.attachedToItem = value;
			}
		}

		// Token: 0x17000033 RID: 51
		public Item this[int index]
		{
			get
			{
				return this.GetItemAt(index);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000C7 RID: 199 RVA: 0x000040C8 File Offset: 0x000022C8
		// (remove) Token: 0x060000C8 RID: 200 RVA: 0x00004100 File Offset: 0x00002300
		public event Action<Inventory, int> onContentChanged;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000C9 RID: 201 RVA: 0x00004138 File Offset: 0x00002338
		// (remove) Token: 0x060000CA RID: 202 RVA: 0x00004170 File Offset: 0x00002370
		public event Action<Inventory> onInventorySorted;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000CB RID: 203 RVA: 0x000041A8 File Offset: 0x000023A8
		// (remove) Token: 0x060000CC RID: 204 RVA: 0x000041E0 File Offset: 0x000023E0
		public event Action<Inventory> onCapacityChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000CD RID: 205 RVA: 0x00004218 File Offset: 0x00002418
		// (remove) Token: 0x060000CE RID: 206 RVA: 0x00004250 File Offset: 0x00002450
		public event Action<Inventory, int> onSetIndexLock;

		// Token: 0x060000CF RID: 207 RVA: 0x00004285 File Offset: 0x00002485
		public void LockIndex(int index)
		{
			if (this.lockedIndexes.Contains(index))
			{
				return;
			}
			this.lockedIndexes.Add(index);
			Action<Inventory, int> action = this.onSetIndexLock;
			if (action == null)
			{
				return;
			}
			action(this, index);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000042B4 File Offset: 0x000024B4
		public void UnlockIndex(int index)
		{
			this.lockedIndexes.RemoveAll((int e) => e == index);
			Action<Inventory, int> action = this.onSetIndexLock;
			if (action == null)
			{
				return;
			}
			action(this, index);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000042FD File Offset: 0x000024FD
		public bool IsIndexLocked(int index)
		{
			return this.lockedIndexes.Contains(index);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000430B File Offset: 0x0000250B
		public void ToggleLockIndex(int index)
		{
			if (this.IsIndexLocked(index))
			{
				this.UnlockIndex(index);
				return;
			}
			this.LockIndex(index);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004325 File Offset: 0x00002525
		public float CachedWeight
		{
			get
			{
				if (this.cachedWeight == null)
				{
					this.RecalculateWeight();
				}
				return this.cachedWeight.Value;
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004348 File Offset: 0x00002548
		private void Start()
		{
			foreach (Item item in this)
			{
				if (!(item == null) && item.ParentItem != this)
				{
					item.NotifyAddedToInventory(this);
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000043A8 File Offset: 0x000025A8
		public bool IsEmpty()
		{
			using (List<Item>.Enumerator enumerator = this.content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004404 File Offset: 0x00002604
		public void Sort(Comparison<Item> comparison)
		{
			this.content.Sort(comparison);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004414 File Offset: 0x00002614
		[ContextMenu("Sort")]
		public void Sort()
		{
			if (this.Loading)
			{
				return;
			}
			this.Loading = true;
			List<Item> list = new List<Item>();
			for (int i = 0; i < this.content.Count; i++)
			{
				if (!this.IsIndexLocked(i))
				{
					Item item3 = this.content[i];
					if (!(item3 == null))
					{
						item3.Detach();
						list.Add(item3);
					}
				}
			}
			List<IGrouping<Tag, Item>> list2 = (from e in list
			where e != null
			select e into item
			group item by Inventory.<Sort>g__GetFirstTag|56_3(item)).ToList<IGrouping<Tag, Item>>();
			list2.Sort(delegate(IGrouping<Tag, Item> g1, IGrouping<Tag, Item> g2)
			{
				Tag key = g1.Key;
				Tag key2 = g2.Key;
				int num = (key != null) ? key.Priority : -1;
				int num2 = (key2 != null) ? key2.Priority : -1;
				if (num != num2)
				{
					return num - num2;
				}
				return string.Compare(key.name, key2.name, StringComparison.OrdinalIgnoreCase);
			});
			List<Item> list3 = new List<Item>();
			using (List<IGrouping<Tag, Item>>.Enumerator enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<IGrouping<int, Item>> list4 = (from item in enumerator.Current
					group item by item.TypeID).ToList<IGrouping<int, Item>>();
					list4.Sort(delegate(IGrouping<int, Item> a, IGrouping<int, Item> b)
					{
						Item item4 = a.First<Item>();
						Item item5 = b.First<Item>();
						if (item4.Order == item5.Order)
						{
							return a.Key - b.Key;
						}
						return item4.Order - item5.Order;
					});
					foreach (IGrouping<int, Item> grouping in list4)
					{
						List<Item> collection;
						if (grouping.First<Item>().Stackable && Inventory.TryMerge(grouping, out collection))
						{
							list3.AddRange(collection);
						}
						else
						{
							list3.AddRange(grouping);
						}
					}
				}
			}
			int count = this.content.Count;
			foreach (Item item2 in list3)
			{
				this.AddItem(item2);
			}
			this.Loading = false;
			Action<Inventory> action = this.onInventorySorted;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004648 File Offset: 0x00002848
		private static bool TryMerge(IEnumerable<Item> itemsOfSameTypeID, out List<Item> result)
		{
			result = null;
			List<Item> list = itemsOfSameTypeID.ToList<Item>();
			list.RemoveAll((Item e) => e == null);
			if (list.Count <= 0)
			{
				return false;
			}
			int typeID = list[0].TypeID;
			foreach (Item item in list)
			{
				if (typeID != item.TypeID)
				{
					Debug.LogError("尝试融合的Item具有不同的TypeID,已取消");
					return false;
				}
			}
			if (!list[0].Stackable)
			{
				Debug.LogError("此类物品不可堆叠，已取消");
				return false;
			}
			result = new List<Item>();
			Stack<Item> stack = new Stack<Item>(list);
			Item item2 = null;
			while (stack.Count > 0)
			{
				if (item2 == null)
				{
					item2 = stack.Pop();
				}
				if (stack.Count <= 0)
				{
					result.Add(item2);
					break;
				}
				item2.Detach();
				Item item3 = null;
				while (item2.StackCount < item2.MaxStackCount && stack.Count > 0)
				{
					item3 = stack.Pop();
					item3.Detach();
					item2.Combine(item3);
				}
				result.Add(item2);
				if (item3 != null && item3.StackCount > 0)
				{
					if (stack.Count <= 0)
					{
						result.Add(item3);
						break;
					}
					item2 = item3;
				}
				else
				{
					item2 = null;
				}
			}
			return true;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000047C8 File Offset: 0x000029C8
		public int GetFirstEmptyPosition(int preferedFirstPosition = 0)
		{
			if (preferedFirstPosition < 0)
			{
				preferedFirstPosition = 0;
			}
			if (this.content.Count <= preferedFirstPosition)
			{
				return preferedFirstPosition;
			}
			for (int i = preferedFirstPosition; i < this.content.Count; i++)
			{
				if (this.content[i] == null)
				{
					return i;
				}
			}
			if (this.content.Count < this.Capacity)
			{
				return this.content.Count;
			}
			for (int j = 0; j < preferedFirstPosition; j++)
			{
				if (this.content[j] == null)
				{
					return j;
				}
			}
			return -1;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000485C File Offset: 0x00002A5C
		public int GetLastItemPosition()
		{
			for (int i = this.content.Count - 1; i >= 0; i--)
			{
				if (this.content[i] != null)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004898 File Offset: 0x00002A98
		public bool AddAt(Item item, int atPosition)
		{
			if (item == null)
			{
				Debug.LogError("尝试添加的物体为空");
				return false;
			}
			if (this.Capacity <= atPosition)
			{
				Debug.LogError(string.Format("向 Inventory {0} 加入物品时位置 {1} 超出最大容量 {2}。", base.name, atPosition, this.Capacity));
				return false;
			}
			if (item.ParentObject != null)
			{
				string format = "{0} \nParent: {1} \nInventory: {2} \nPlug: {3}";
				object[] array = new object[4];
				array[0] = item.name;
				array[1] = item.ParentItem;
				int num = 2;
				Inventory inInventory = item.InInventory;
				array[num] = ((inInventory != null) ? inInventory.name : null);
				int num2 = 3;
				Slot pluggedIntoSlot = item.PluggedIntoSlot;
				array[num2] = ((pluggedIntoSlot != null) ? pluggedIntoSlot.DisplayName : null);
				Debug.Log(string.Format(format, array));
				Debug.LogError(string.Concat(new string[]
				{
					"正在尝试将一个有父物体的物品 ",
					item.DisplayName,
					" 放入Inventory。请先使其脱离其父物体 ",
					item.ParentObject.name,
					" 再进行此操作。"
				}));
				return false;
			}
			Item itemAt = this.GetItemAt(atPosition);
			if (itemAt != null)
			{
				Debug.LogError(string.Format("正在尝试将物品 {0} 放入 Inventory {1} 的 {2} 位置。但此位置已经存在另一物体 {3}。", new object[]
				{
					item.DisplayName,
					base.name,
					atPosition,
					itemAt.DisplayName
				}));
			}
			while (this.content.Count <= atPosition)
			{
				this.content.Add(null);
			}
			this.content[atPosition] = item;
			item.transform.SetParent(base.transform);
			item.NotifyAddedToInventory(this);
			item.InitiateNotifyItemTreeChanged();
			this.RecalculateWeight();
			Action<Inventory, int> action = this.onContentChanged;
			if (action != null)
			{
				action(this, atPosition);
			}
			return true;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004A3C File Offset: 0x00002C3C
		public bool RemoveAt(int position, out Item removedItem)
		{
			removedItem = null;
			if (this.Capacity <= position && position >= this.content.Count)
			{
				Debug.LogError("位置超出Inventory容量。");
				return false;
			}
			Item itemAt = this.GetItemAt(position);
			if (itemAt != null)
			{
				this.content[position] = null;
				removedItem = itemAt;
				removedItem.NotifyRemovedFromInventory(this);
				removedItem.InitiateNotifyItemTreeChanged();
				Item item = this.AttachedToItem;
				if (item != null)
				{
					item.InitiateNotifyItemTreeChanged();
				}
				int num = this.content.Count - 1;
				while (num >= 0 && this.content[num] == null)
				{
					this.content.RemoveAt(num);
					num--;
				}
				this.RecalculateWeight();
				Action<Inventory, int> action = this.onContentChanged;
				if (action != null)
				{
					action(this, position);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004B08 File Offset: 0x00002D08
		public bool AddItem(Item item)
		{
			int firstEmptyPosition = this.GetFirstEmptyPosition(0);
			if (firstEmptyPosition < 0)
			{
				Debug.Log("添加物品失败，Inventory " + base.name + " 已满。");
				return false;
			}
			return this.AddAt(item, firstEmptyPosition);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004B48 File Offset: 0x00002D48
		public bool RemoveItem(Item item)
		{
			int num = this.content.IndexOf(item);
			if (num < 0)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"正在尝试从Inventory ",
					base.name,
					" 中删除 ",
					item.DisplayName,
					"，但它并不在这个Inventory中。"
				}));
				return false;
			}
			Item item2;
			return this.RemoveAt(num, out item2);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004BAC File Offset: 0x00002DAC
		public Item GetItemAt(int position)
		{
			if (position >= this.Capacity && position >= this.content.Count)
			{
				Debug.LogError("访问的位置超出Inventory容量。");
				return null;
			}
			if (this.content.Count <= position)
			{
				return null;
			}
			return this.content[position];
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004BF8 File Offset: 0x00002DF8
		public void Validate(SelfValidationResult result)
		{
			if (this.AttachedToItem != null)
			{
				if (this.AttachedToItem.gameObject != base.gameObject)
				{
					result.AddError("AttachedItem引用了另一个Game Object上的Item。").WithFix("引用本物体上的Item。", delegate()
					{
						this.attachedToItem = base.GetComponent<Item>();
					}, true);
				}
				if (this.AttachedToItem.Inventory != this)
				{
					if (this.AttachedToItem.Inventory != null)
					{
						result.AddError("AttachedItem引用了其他的Inventory。请检查Item内的配置。");
					}
					else
					{
						result.AddError("AttachedItem没有引用此Inventory。").WithFix("使AttachedItem引用此Inventory。", delegate()
						{
							this.AttachedToItem.Inventory = this;
						}, true);
					}
				}
			}
			if (this.AttachedToItem == null)
			{
				Item gotItem = base.GetComponent<Item>();
				if (gotItem != null)
				{
					result.AddError("同一GameObject上存在Item，但AttachedToItem变量留空。").WithFix("设为本物体上的Item。", delegate()
					{
						this.attachedToItem = gotItem;
					}, true);
				}
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004D00 File Offset: 0x00002F00
		public IEnumerator<Item> GetEnumerator()
		{
			foreach (Item item in this.content)
			{
				if (!(item == null))
				{
					yield return item;
				}
			}
			List<Item>.Enumerator enumerator = default(List<Item>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004D0F File Offset: 0x00002F0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004D18 File Offset: 0x00002F18
		public void DestroyAllContent()
		{
			for (int i = 0; i < this.content.Count; i++)
			{
				Item item = this.content[i];
				if (!(item == null))
				{
					this.RemoveItem(item);
					item.DestroyTree();
				}
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004D5F File Offset: 0x00002F5F
		public List<Item> FindAll(Predicate<Item> match)
		{
			return this.content.FindAll(match);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004D70 File Offset: 0x00002F70
		public void RecalculateWeight()
		{
			float num = 0f;
			foreach (Item item in this.content)
			{
				if (!(item == null))
				{
					float num2 = item.RecalculateTotalWeight();
					num += num2;
				}
			}
			this.cachedWeight = new float?(num);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004DE4 File Offset: 0x00002FE4
		public void SetCapacity(int capacity)
		{
			this.defaultCapacity = capacity;
			Action<Inventory> action = this.onCapacityChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004E00 File Offset: 0x00003000
		public int GetItemCount()
		{
			int num = 0;
			using (List<Item>.Enumerator enumerator = this.content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current == null))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004E5C File Offset: 0x0000305C
		internal void NotifyContentChanged(Item item)
		{
			if (!item)
			{
				return;
			}
			Action<Inventory, int> action = this.onContentChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.content.IndexOf(item));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004E84 File Offset: 0x00003084
		public int GetIndex(Item item)
		{
			if (item == null)
			{
				return -1;
			}
			return this.content.IndexOf(item);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004EA0 File Offset: 0x000030A0
		public Item Find(int typeID)
		{
			return this.content.Find((Item e) => e != null && e.TypeID == typeID);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004F02 File Offset: 0x00003102
		[CompilerGenerated]
		internal static Tag <Sort>g__GetFirstTag|56_3(Item item)
		{
			if (item == null)
			{
				return null;
			}
			if (item.Tags == null || item.Tags.Count == 0)
			{
				return null;
			}
			return item.Tags.Get(0);
		}

		// Token: 0x04000040 RID: 64
		private bool loading;

		// Token: 0x04000041 RID: 65
		[LocalizationKey("Default")]
		[SerializeField]
		private string displayNameKey = "";

		// Token: 0x04000042 RID: 66
		private const string defaultDisplayNameKey = "UI_InventoryDefault";

		// Token: 0x04000043 RID: 67
		[SerializeField]
		private int defaultCapacity = 64;

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private Item attachedToItem;

		// Token: 0x04000045 RID: 69
		[SerializeField]
		private List<Item> content = new List<Item>();

		// Token: 0x04000046 RID: 70
		[SerializeField]
		private bool needInspection;

		// Token: 0x04000047 RID: 71
		[SerializeField]
		private bool acceptSticky;

		// Token: 0x04000048 RID: 72
		private const bool TrimListWhenRemovingItem = true;

		// Token: 0x0400004D RID: 77
		public bool hasBeenInspectedInLootBox;

		// Token: 0x0400004E RID: 78
		[SerializeField]
		public List<int> lockedIndexes = new List<int>();

		// Token: 0x0400004F RID: 79
		private float? cachedWeight;
	}
}
