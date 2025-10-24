using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class ItemPickerDebug : MonoBehaviour
{
	// Token: 0x06000AC3 RID: 2755 RVA: 0x0002E823 File Offset: 0x0002CA23
	public void PickPlayerInventoryAndLog()
	{
		this.Pick().Forget();
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0002E830 File Offset: 0x0002CA30
	private UniTask Pick()
	{
		ItemPickerDebug.<Pick>d__1 <Pick>d__;
		<Pick>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Pick>d__.<>1__state = -1;
		<Pick>d__.<>t__builder.Start<ItemPickerDebug.<Pick>d__1>(ref <Pick>d__);
		return <Pick>d__.<>t__builder.Task;
	}
}
