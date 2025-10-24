using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000012 RID: 18
	public class Effect : MonoBehaviour, ISelfValidator
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003477 File Offset: 0x00001677
		public Item Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000347F File Offset: 0x0000167F
		public bool Display
		{
			get
			{
				return this.display;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003487 File Offset: 0x00001687
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000348F File Offset: 0x0000168F
		public string GetDisplayString()
		{
			return this.Description;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600007C RID: 124 RVA: 0x00003498 File Offset: 0x00001698
		// (remove) Token: 0x0600007D RID: 125 RVA: 0x000034D0 File Offset: 0x000016D0
		public event Action<Effect, Item> onSetTargetItem;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600007E RID: 126 RVA: 0x00003508 File Offset: 0x00001708
		// (remove) Token: 0x0600007F RID: 127 RVA: 0x00003540 File Offset: 0x00001740
		public event Action<Effect, Item> onItemTreeChanged;

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003575 File Offset: 0x00001775
		public ReadOnlyCollection<EffectTrigger> Triggers
		{
			get
			{
				if (this._Triggers == null)
				{
					this._Triggers = new ReadOnlyCollection<EffectTrigger>(this.triggers);
				}
				return this._Triggers;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003596 File Offset: 0x00001796
		public ReadOnlyCollection<EffectFilter> Filters
		{
			get
			{
				if (this._Filters == null)
				{
					this._Filters = new ReadOnlyCollection<EffectFilter>(this.filters);
				}
				return this._Filters;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000035B7 File Offset: 0x000017B7
		public ReadOnlyCollection<EffectAction> Actions
		{
			get
			{
				if (this._Actions == null)
				{
					this._Actions = new ReadOnlyCollection<EffectAction>(this.actions);
				}
				return this._Actions;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000035D8 File Offset: 0x000017D8
		private bool EvaluateFilters(EffectTriggerEventContext context)
		{
			foreach (EffectFilter effectFilter in this.filters)
			{
				if (effectFilter.enabled && !effectFilter.Evaluate(context))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000363C File Offset: 0x0000183C
		internal void Trigger(EffectTriggerEventContext context)
		{
			if (!base.enabled)
			{
				return;
			}
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!this.EvaluateFilters(context))
			{
				return;
			}
			foreach (EffectAction effectAction in this.actions)
			{
				effectAction.NotifyTriggered(context);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000036B0 File Offset: 0x000018B0
		private void OnValidate()
		{
			if (this.item == null)
			{
				Transform parent = base.transform.parent;
				this.item = ((parent != null) ? parent.GetComponent<Item>() : null);
			}
			base.transform.hideFlags = HideFlags.HideInInspector;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000036EC File Offset: 0x000018EC
		public void SetItem(Item targetItem)
		{
			this.UnregisterItemEvents();
			if (targetItem == null)
			{
				this.item = null;
				base.transform.SetParent(null);
			}
			this.item = targetItem;
			foreach (EffectTrigger effectTrigger in this.triggers)
			{
				effectTrigger.NotifySetItem(this, targetItem);
			}
			Action<Effect, Item> action = this.onSetTargetItem;
			if (action != null)
			{
				action(this, targetItem);
			}
			this.RegisterItemEvents();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003780 File Offset: 0x00001980
		private void RegisterItemEvents()
		{
			if (this.item == null)
			{
				return;
			}
			this.item.onItemTreeChanged += this.OnItemTreeChanged;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000037A8 File Offset: 0x000019A8
		private void UnregisterItemEvents()
		{
			if (this.item == null)
			{
				return;
			}
			this.item.onItemTreeChanged -= this.OnItemTreeChanged;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000037D0 File Offset: 0x000019D0
		private void OnItemTreeChanged(Item item)
		{
			Action<Effect, Item> action = this.onItemTreeChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.item);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000037EC File Offset: 0x000019EC
		public void Validate(SelfValidationResult result)
		{
			Transform parent = base.transform.parent;
			Item y = (parent != null) ? parent.GetComponent<Item>() : null;
			if (this.item != y)
			{
				result.AddError("Item 应为直接父物体").WithFix("将 Item 设为正确的值", delegate()
				{
					Transform parent2 = base.transform.parent;
					this.item = ((parent2 != null) ? parent2.GetComponent<Item>() : null);
				}, true);
			}
			else if (this.item != null && !this.item.Effects.Contains(this))
			{
				result.AddError("Item中未引用本Effect").WithFix("在Item中加入本Effect。", delegate()
				{
					this.item.Effects.Add(this);
				}, true);
			}
			bool flag = false;
			foreach (EffectTrigger component in this.triggers)
			{
				if (!this.<Validate>g__ValidateComponent|35_13(component))
				{
					flag = true;
				}
			}
			if (flag)
			{
				result.AddError("Trigger 列表中包含来自其他 Game Object 的 Trigger。").WithFix("移除来自其他 Game Object 的 Trigger。", delegate()
				{
					this.triggers.RemoveAll((EffectTrigger e) => e.gameObject != base.gameObject);
				}, true);
			}
			flag = false;
			foreach (EffectFilter component2 in this.filters)
			{
				if (!this.<Validate>g__ValidateComponent|35_13(component2))
				{
					flag = true;
				}
			}
			if (flag)
			{
				result.AddError("Filter 列表中包含来自其他 Game Object 的 Filter。").WithFix("移除来自其他 Game Object 的 Filter。", delegate()
				{
					this.filters.RemoveAll((EffectFilter e) => e.gameObject != base.gameObject);
				}, true);
			}
			flag = false;
			foreach (EffectAction component3 in this.actions)
			{
				if (!this.<Validate>g__ValidateComponent|35_13(component3))
				{
					flag = true;
				}
			}
			if (flag)
			{
				result.AddError("Trigger 列表中包含来自其他 Game Object 的 Trigger。").WithFix("移除来自其他 Game Object 的 Trigger。", delegate()
				{
					this.actions.RemoveAll((EffectAction e) => e.gameObject != base.gameObject);
				}, true);
			}
			if (Effect.<Validate>g__AnyDuplicate|35_11<EffectTrigger>(this.triggers))
			{
				result.AddError("Trigger 列表中有重复的元素。").WithFix("移除重复元素。", delegate()
				{
					this.triggers = new List<EffectTrigger>(this.triggers.Distinct<EffectTrigger>());
				}, true);
			}
			if (Effect.<Validate>g__AnyDuplicate|35_11<EffectFilter>(this.filters))
			{
				result.AddError("Filter 列表中有重复的元素。").WithFix("移除重复元素。", delegate()
				{
					this.filters = new List<EffectFilter>(this.filters.Distinct<EffectFilter>());
				}, true);
			}
			if (Effect.<Validate>g__AnyDuplicate|35_11<EffectAction>(this.actions))
			{
				result.AddError("Trigger 列表中有重复的元素。").WithFix("移除重复元素。", delegate()
				{
					this.actions = new List<EffectAction>(this.actions.Distinct<EffectAction>());
				}, true);
			}
			if (Effect.<Validate>g__AnyNull|35_12<EffectTrigger>(this.triggers))
			{
				result.AddError("Trigger 列表中有空元素。").WithFix("移除空元素。", delegate()
				{
					this.triggers.RemoveAll((EffectTrigger e) => e == null);
				}, true);
			}
			if (Effect.<Validate>g__AnyNull|35_12<EffectFilter>(this.filters))
			{
				result.AddError("Filter 列表中有空元素。").WithFix("移除空元素。", delegate()
				{
					this.filters.RemoveAll((EffectFilter e) => e == null);
				}, true);
			}
			if (Effect.<Validate>g__AnyNull|35_12<EffectAction>(this.actions))
			{
				result.AddError("Trigger 列表中有空元素。").WithFix("移除空元素。", delegate()
				{
					this.actions.RemoveAll((EffectAction e) => e == null);
				}, true);
			}
			if (this.triggers.Count < 1)
			{
				result.AddWarning("没有配置任何触发器(Trigger)，将无法触发效果。");
			}
			if (this.actions.Count < 1)
			{
				result.AddWarning("没有配置任何动作(Action),该效果没有任何实际作用。");
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003B30 File Offset: 0x00001D30
		internal void AddEffectComponent(EffectComponent effectComponent)
		{
			if (effectComponent is EffectTrigger)
			{
				this.triggers.Add(effectComponent as EffectTrigger);
				effectComponent.Master = this;
				return;
			}
			if (effectComponent is EffectFilter)
			{
				this.filters.Add(effectComponent as EffectFilter);
				effectComponent.Master = this;
				return;
			}
			if (effectComponent is EffectAction)
			{
				this.actions.Add(effectComponent as EffectAction);
				effectComponent.Master = this;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003B9F File Offset: 0x00001D9F
		private void Awake()
		{
			this.RegisterItemEvents();
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003BA7 File Offset: 0x00001DA7
		private static Color TriggerColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectTrigger;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003BB3 File Offset: 0x00001DB3
		private static Color FilterColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectFilter;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003BBF File Offset: 0x00001DBF
		private static Color ActionColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectAction;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003D87 File Offset: 0x00001F87
		[CompilerGenerated]
		internal static bool <Validate>g__AnyDuplicate|35_11<T>(List<T> list)
		{
			return (from e in list
			group e by e).Any((IGrouping<T, T> g) => g.Count<T>() > 1);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003DB4 File Offset: 0x00001FB4
		[CompilerGenerated]
		internal static bool <Validate>g__AnyNull|35_12<T>(List<T> list)
		{
			return list.Any((T e) => e == null);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003DCC File Offset: 0x00001FCC
		[CompilerGenerated]
		private bool <Validate>g__ValidateComponent|35_13(EffectComponent component)
		{
			return component == null || !(component.gameObject != base.gameObject);
		}

		// Token: 0x0400002D RID: 45
		[SerializeField]
		private Item item;

		// Token: 0x0400002E RID: 46
		[SerializeField]
		private bool display;

		// Token: 0x0400002F RID: 47
		[SerializeField]
		private string description = "未定义描述";

		// Token: 0x04000032 RID: 50
		[SerializeField]
		internal List<EffectTrigger> triggers = new List<EffectTrigger>();

		// Token: 0x04000033 RID: 51
		[SerializeField]
		internal List<EffectFilter> filters = new List<EffectFilter>();

		// Token: 0x04000034 RID: 52
		[SerializeField]
		internal List<EffectAction> actions = new List<EffectAction>();

		// Token: 0x04000035 RID: 53
		private ReadOnlyCollection<EffectTrigger> _Triggers;

		// Token: 0x04000036 RID: 54
		private ReadOnlyCollection<EffectFilter> _Filters;

		// Token: 0x04000037 RID: 55
		private ReadOnlyCollection<EffectAction> _Actions;
	}
}
