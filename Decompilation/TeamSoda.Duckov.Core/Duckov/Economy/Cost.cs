using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x02000325 RID: 805
	[Serializable]
	public struct Cost
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x0006151D File Offset: 0x0005F71D
		public bool Enough
		{
			get
			{
				return EconomyManager.IsEnough(this, true, true);
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x0006152C File Offset: 0x0005F72C
		public bool IsFree
		{
			get
			{
				return this.money <= 0L && (this.items == null || this.items.Length == 0);
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0006154E File Offset: 0x0005F74E
		public bool Pay(bool accountAvaliable = true, bool cashAvaliable = true)
		{
			return EconomyManager.Pay(this, accountAvaliable, cashAvaliable);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00061560 File Offset: 0x0005F760
		public static Cost FromString(string costDescription)
		{
			int num = 0;
			List<Cost.ItemEntry> list = new List<Cost.ItemEntry>();
			foreach (string text in costDescription.Split(',', StringSplitOptions.None))
			{
				string[] array2 = text.Split(":", StringSplitOptions.None);
				if (array2.Length != 2)
				{
					Debug.LogError("Invalid cost description: " + text + "\n" + costDescription);
				}
				else
				{
					string text2 = array2[0].Trim();
					int num2;
					if (!int.TryParse(array2[1].Trim(), out num2))
					{
						Debug.LogError("Invalid cost description: " + text);
					}
					else if (text2 == "money")
					{
						num = num2;
					}
					else
					{
						int num3 = ItemAssetsCollection.TryGetIDByName(text2);
						if (num3 <= 0)
						{
							Debug.LogError("Invalid item name " + text2);
						}
						else
						{
							list.Add(new Cost.ItemEntry
							{
								id = num3,
								amount = (long)num2
							});
						}
					}
				}
			}
			return new Cost
			{
				money = (long)num,
				items = list.ToArray()
			};
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x00061671 File Offset: 0x0005F871
		public static bool TaskPending
		{
			get
			{
				return Cost.ReturnTaskLocks.Count > 0;
			}
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00061680 File Offset: 0x0005F880
		internal UniTask Return(bool directToBuffer = false, bool toPlayerInventory = false, int amountFactor = 1, List<Item> generatedItemsBuffer = null)
		{
			Cost.<Return>d__12 <Return>d__;
			<Return>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Return>d__.<>4__this = this;
			<Return>d__.directToBuffer = directToBuffer;
			<Return>d__.toPlayerInventory = toPlayerInventory;
			<Return>d__.amountFactor = amountFactor;
			<Return>d__.generatedItemsBuffer = generatedItemsBuffer;
			<Return>d__.<>1__state = -1;
			<Return>d__.<>t__builder.Start<Cost.<Return>d__12>(ref <Return>d__);
			return <Return>d__.<>t__builder.Task;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000616EC File Offset: 0x0005F8EC
		public Cost(long money, [TupleElementNames(new string[]
		{
			"id",
			"amount"
		})] ValueTuple<int, long>[] items)
		{
			this.money = money;
			this.items = new Cost.ItemEntry[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				ValueTuple<int, long> valueTuple = items[i];
				this.items[i] = new Cost.ItemEntry
				{
					id = valueTuple.Item1,
					amount = valueTuple.Item2
				};
			}
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x00061753 File Offset: 0x0005F953
		public Cost(long money)
		{
			this.money = money;
			this.items = new Cost.ItemEntry[0];
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x00061768 File Offset: 0x0005F968
		public Cost([TupleElementNames(new string[]
		{
			"id",
			"amount"
		})] params ValueTuple<int, long>[] items)
		{
			this.money = 0L;
			this.items = new Cost.ItemEntry[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				ValueTuple<int, long> valueTuple = items[i];
				this.items[i] = new Cost.ItemEntry
				{
					id = valueTuple.Item1,
					amount = valueTuple.Item2
				};
			}
		}

		// Token: 0x04001329 RID: 4905
		public long money;

		// Token: 0x0400132A RID: 4906
		public Cost.ItemEntry[] items;

		// Token: 0x0400132B RID: 4907
		private static List<object> ReturnTaskLocks = new List<object>();

		// Token: 0x020005C3 RID: 1475
		[Serializable]
		public struct ItemEntry
		{
			// Token: 0x04002080 RID: 8320
			[ItemTypeID]
			public int id;

			// Token: 0x04002081 RID: 8321
			public long amount;
		}
	}
}
