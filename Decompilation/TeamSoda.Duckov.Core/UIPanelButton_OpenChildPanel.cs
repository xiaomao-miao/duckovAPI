using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000155 RID: 341
public class UIPanelButton_OpenChildPanel : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000A7C RID: 2684 RVA: 0x0002DDAB File Offset: 0x0002BFAB
	private void Awake()
	{
		if (this.master == null)
		{
			this.master = base.GetComponentInParent<UIPanel>();
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0002DDC7 File Offset: 0x0002BFC7
	public void OnPointerClick(PointerEventData eventData)
	{
		UIPanel uipanel = this.master;
		if (uipanel != null)
		{
			uipanel.OpenChild(this.target);
		}
		eventData.Use();
	}

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	private UIPanel master;

	// Token: 0x04000922 RID: 2338
	[SerializeField]
	private UIPanel target;
}
