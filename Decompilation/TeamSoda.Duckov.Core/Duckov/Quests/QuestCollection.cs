using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000334 RID: 820
	[CreateAssetMenu(menuName = "Quest Collection")]
	public class QuestCollection : ScriptableObject, IList<Quest>, ICollection<Quest>, IEnumerable<Quest>, IEnumerable, ISelfValidator
	{
		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x000655AE File Offset: 0x000637AE
		public static QuestCollection Instance
		{
			get
			{
				return GameplayDataSettings.QuestCollection;
			}
		}

		// Token: 0x1700052F RID: 1327
		public Quest this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x000655D2 File Offset: 0x000637D2
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001BFA RID: 7162 RVA: 0x000655DF File Offset: 0x000637DF
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000655E2 File Offset: 0x000637E2
		public void Add(Quest item)
		{
			this.list.Add(item);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000655F0 File Offset: 0x000637F0
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000655FD File Offset: 0x000637FD
		public bool Contains(Quest item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x0006560B File Offset: 0x0006380B
		public void CopyTo(Quest[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x0006561A File Offset: 0x0006381A
		public IEnumerator<Quest> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x0006562C File Offset: 0x0006382C
		public int IndexOf(Quest item)
		{
			return this.list.IndexOf(item);
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x0006563A File Offset: 0x0006383A
		public void Insert(int index, Quest item)
		{
			this.list.Insert(index, item);
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00065649 File Offset: 0x00063849
		public bool Remove(Quest item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x00065657 File Offset: 0x00063857
		public void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x00065665 File Offset: 0x00063865
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x0006566D File Offset: 0x0006386D
		public void Collect()
		{
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x00065670 File Offset: 0x00063870
		public void Validate(SelfValidationResult result)
		{
			this.list.GroupBy(delegate(Quest e)
			{
				if (e == null)
				{
					return -1;
				}
				return e.ID;
			});
			if (this.list.GroupBy(delegate(Quest e)
			{
				if (e == null)
				{
					return -1;
				}
				return e.ID;
			}).Any((IGrouping<int, Quest> g) => g.Count<Quest>() > 1))
			{
				result.AddError("存在冲突的QuestID。").WithFix("自动重新分配ID", new Action(this.AutoFixID), true);
			}
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x0006571C File Offset: 0x0006391C
		private void AutoFixID()
		{
			int num = this.list.Max((Quest e) => e.ID) + 1;
			foreach (IEnumerable<Quest> enumerable in from e in this.list
			group e by e.ID into g
			where g.Count<Quest>() > 1
			select g)
			{
				int num2 = 0;
				foreach (Quest quest in enumerable)
				{
					if (!(quest == null) && num2++ != 0)
					{
						quest.ID = num++;
					}
				}
			}
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x00065828 File Offset: 0x00063A28
		public Quest Get(int id)
		{
			return this.list.FirstOrDefault((Quest q) => q != null && q.ID == id);
		}

		// Token: 0x040013B1 RID: 5041
		[SerializeField]
		private List<Quest> list;
	}
}
