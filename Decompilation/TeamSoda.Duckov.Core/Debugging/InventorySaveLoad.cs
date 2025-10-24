using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Debugging
{
	// Token: 0x02000221 RID: 545
	public class InventorySaveLoad : MonoBehaviour
	{
		// Token: 0x0600105A RID: 4186 RVA: 0x0003F519 File Offset: 0x0003D719
		public void Save()
		{
			this.inventory.Save(this.key);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0003F52C File Offset: 0x0003D72C
		public UniTask Load()
		{
			InventorySaveLoad.<Load>d__4 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<InventorySaveLoad.<Load>d__4>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0003F56F File Offset: 0x0003D76F
		private void OnLoadFinished()
		{
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0003F571 File Offset: 0x0003D771
		public void BeginLoad()
		{
			this.Load().Forget();
		}

		// Token: 0x04000D05 RID: 3333
		public Inventory inventory;

		// Token: 0x04000D06 RID: 3334
		public string key = "helloInventory";

		// Token: 0x04000D07 RID: 3335
		private bool loading;
	}
}
