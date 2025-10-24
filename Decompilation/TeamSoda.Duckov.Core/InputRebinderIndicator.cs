using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001BC RID: 444
public class InputRebinderIndicator : MonoBehaviour
{
	// Token: 0x06000D34 RID: 3380 RVA: 0x00036E28 File Offset: 0x00035028
	private void Awake()
	{
		InputRebinder.OnRebindBegin = (Action<InputAction>)Delegate.Combine(InputRebinder.OnRebindBegin, new Action<InputAction>(this.OnRebindBegin));
		InputRebinder.OnRebindComplete = (Action<InputAction>)Delegate.Combine(InputRebinder.OnRebindComplete, new Action<InputAction>(this.OnRebindComplete));
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x00036E80 File Offset: 0x00035080
	private void OnRebindComplete(InputAction action)
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x00036E8D File Offset: 0x0003508D
	private void OnRebindBegin(InputAction action)
	{
		this.fadeGroup.Show();
	}

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	private FadeGroup fadeGroup;
}
