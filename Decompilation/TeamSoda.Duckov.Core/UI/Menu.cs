using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x02000210 RID: 528
	public class Menu : MonoBehaviour
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0003DA87 File Offset: 0x0003BC87
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x0003DA8F File Offset: 0x0003BC8F
		public bool Focused
		{
			get
			{
				return this.focused;
			}
			set
			{
				this.SetFocused(value);
			}
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06000FB3 RID: 4019 RVA: 0x0003DA98 File Offset: 0x0003BC98
		// (remove) Token: 0x06000FB4 RID: 4020 RVA: 0x0003DAD0 File Offset: 0x0003BCD0
		public event Action<Menu, MenuItem> onSelectionChanged;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06000FB5 RID: 4021 RVA: 0x0003DB08 File Offset: 0x0003BD08
		// (remove) Token: 0x06000FB6 RID: 4022 RVA: 0x0003DB40 File Offset: 0x0003BD40
		public event Action<Menu, MenuItem> onConfirmed;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06000FB7 RID: 4023 RVA: 0x0003DB78 File Offset: 0x0003BD78
		// (remove) Token: 0x06000FB8 RID: 4024 RVA: 0x0003DBB0 File Offset: 0x0003BDB0
		public event Action<Menu, MenuItem> onCanceled;

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0003DBE5 File Offset: 0x0003BDE5
		private void SetFocused(bool value)
		{
			this.focused = value;
			if (this.focused && this.cursor == null)
			{
				this.SelectDefault();
			}
			MenuItem menuItem = this.cursor;
			if (menuItem == null)
			{
				return;
			}
			menuItem.NotifyMasterFocusStatusChanged();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0003DC1A File Offset: 0x0003BE1A
		public MenuItem GetSelected()
		{
			return this.cursor;
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0003DC24 File Offset: 0x0003BE24
		public T GetSelected<T>() where T : Component
		{
			if (this.cursor == null)
			{
				return default(T);
			}
			return this.cursor.GetComponent<T>();
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0003DC54 File Offset: 0x0003BE54
		public void Select(MenuItem toSelect)
		{
			if (toSelect.transform.parent != base.transform)
			{
				Debug.LogError("正在尝试选中不属于此菜单的项目。已取消。");
				return;
			}
			if (!this.items.Contains(toSelect))
			{
				this.items.Add(toSelect);
			}
			if (!toSelect.Selectable)
			{
				return;
			}
			if (this.cursor != null)
			{
				this.DeselectCurrent();
			}
			this.cursor = toSelect;
			this.cursor.NotifySelected();
			this.OnSelectionChanged();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0003DCD4 File Offset: 0x0003BED4
		public void SelectDefault()
		{
			MenuItem[] componentsInChildren = base.GetComponentsInChildren<MenuItem>(false);
			if (componentsInChildren == null)
			{
				return;
			}
			foreach (MenuItem menuItem in componentsInChildren)
			{
				if (!(menuItem == null) && menuItem.Selectable)
				{
					this.Select(menuItem);
				}
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0003DD17 File Offset: 0x0003BF17
		public void Confirm()
		{
			if (this.cursor != null)
			{
				this.cursor.NotifyConfirmed();
			}
			Action<Menu, MenuItem> action = this.onConfirmed;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0003DD49 File Offset: 0x0003BF49
		public void Cancel()
		{
			if (this.cursor != null)
			{
				this.cursor.NotifyCanceled();
			}
			Action<Menu, MenuItem> action = this.onCanceled;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0003DD7B File Offset: 0x0003BF7B
		private void DeselectCurrent()
		{
			this.cursor.NotifyDeselected();
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0003DD88 File Offset: 0x0003BF88
		private void OnSelectionChanged()
		{
			Action<Menu, MenuItem> action = this.onSelectionChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0003DDA4 File Offset: 0x0003BFA4
		public void Navigate(Vector2 direction)
		{
			if (this.cursor == null)
			{
				this.SelectDefault();
			}
			if (this.cursor == null)
			{
				return;
			}
			if (Mathf.Approximately(direction.sqrMagnitude, 0f))
			{
				return;
			}
			MenuItem menuItem = this.FindClosestEntryInDirection(this.cursor, direction);
			if (menuItem == null)
			{
				return;
			}
			this.Select(menuItem);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0003DE08 File Offset: 0x0003C008
		private MenuItem FindClosestEntryInDirection(MenuItem cursor, Vector2 direction)
		{
			if (cursor == null)
			{
				return null;
			}
			direction = direction.normalized;
			float num = Mathf.Cos(0.7853982f);
			Menu.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.bestMatch = null;
			CS$<>8__locals1.bestSqrDist = float.MaxValue;
			CS$<>8__locals1.bestDot = num;
			foreach (MenuItem cur in this.items)
			{
				Menu.<>c__DisplayClass26_1 CS$<>8__locals2;
				CS$<>8__locals2.cur = cur;
				if (!(CS$<>8__locals2.cur == null) && !(CS$<>8__locals2.cur == cursor) && CS$<>8__locals2.cur.Selectable)
				{
					Vector3 vector = CS$<>8__locals2.cur.transform.localPosition - cursor.transform.localPosition;
					Vector3 normalized = vector.normalized;
					Menu.<>c__DisplayClass26_2 CS$<>8__locals3;
					CS$<>8__locals3.dot = Vector3.Dot(normalized, direction);
					if (CS$<>8__locals3.dot >= num)
					{
						CS$<>8__locals3.sqrDist = vector.magnitude;
						if (CS$<>8__locals3.sqrDist <= CS$<>8__locals1.bestSqrDist)
						{
							if (CS$<>8__locals3.sqrDist < CS$<>8__locals1.bestSqrDist)
							{
								Menu.<FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							}
							else if (CS$<>8__locals3.sqrDist == CS$<>8__locals1.bestSqrDist && CS$<>8__locals3.dot > CS$<>8__locals1.bestDot)
							{
								Menu.<FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							}
						}
					}
				}
			}
			return CS$<>8__locals1.bestMatch;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0003DF80 File Offset: 0x0003C180
		internal void Register(MenuItem menuItem)
		{
			this.items.Add(menuItem);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0003DF8F File Offset: 0x0003C18F
		internal void Unegister(MenuItem menuItem)
		{
			this.items.Remove(menuItem);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0003DFB1 File Offset: 0x0003C1B1
		[CompilerGenerated]
		internal static void <FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref Menu.<>c__DisplayClass26_0 A_0, ref Menu.<>c__DisplayClass26_1 A_1, ref Menu.<>c__DisplayClass26_2 A_2)
		{
			A_0.bestMatch = A_1.cur;
			A_0.bestSqrDist = A_2.sqrDist;
			A_0.bestDot = A_2.dot;
		}

		// Token: 0x04000CA7 RID: 3239
		[SerializeField]
		private bool focused;

		// Token: 0x04000CA8 RID: 3240
		[SerializeField]
		private MenuItem cursor;

		// Token: 0x04000CA9 RID: 3241
		[SerializeField]
		private LayoutGroup layout;

		// Token: 0x04000CAD RID: 3245
		private HashSet<MenuItem> items = new HashSet<MenuItem>();
	}
}
