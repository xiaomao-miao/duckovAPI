using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000166 RID: 358
public class FadeGroupButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000ADF RID: 2783 RVA: 0x0002EB5B File Offset: 0x0002CD5B
	private void OnEnable()
	{
		UIInputManager.OnCancel += this.OnCancel;
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0002EB6E File Offset: 0x0002CD6E
	private void OnDisable()
	{
		UIInputManager.OnCancel -= this.OnCancel;
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0002EB81 File Offset: 0x0002CD81
	private void OnCancel(UIInputEventData data)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (data.Used)
		{
			return;
		}
		if (!this.triggerWhenCancel)
		{
			return;
		}
		this.Execute();
		data.Use();
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0002EBAA File Offset: 0x0002CDAA
	public void OnPointerClick(PointerEventData eventData)
	{
		this.Execute();
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0002EBB2 File Offset: 0x0002CDB2
	private void Execute()
	{
		if (this.closeOnClick)
		{
			this.closeOnClick.Hide();
		}
		if (this.openOnClick)
		{
			this.openOnClick.Show();
		}
	}

	// Token: 0x0400095E RID: 2398
	[SerializeField]
	private FadeGroup closeOnClick;

	// Token: 0x0400095F RID: 2399
	[SerializeField]
	private FadeGroup openOnClick;

	// Token: 0x04000960 RID: 2400
	[SerializeField]
	private bool triggerWhenCancel;
}
