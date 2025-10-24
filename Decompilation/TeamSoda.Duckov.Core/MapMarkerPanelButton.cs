using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001C1 RID: 449
public class MapMarkerPanelButton : MonoBehaviour
{
	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000D62 RID: 3426 RVA: 0x0003744B File Offset: 0x0003564B
	public Image Image
	{
		get
		{
			return this.image;
		}
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00037453 File Offset: 0x00035653
	public void Setup(UnityAction action, bool selected)
	{
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(action);
		this.selectionIndicator.gameObject.SetActive(selected);
	}

	// Token: 0x04000B61 RID: 2913
	[SerializeField]
	private GameObject selectionIndicator;

	// Token: 0x04000B62 RID: 2914
	[SerializeField]
	private Image image;

	// Token: 0x04000B63 RID: 2915
	[SerializeField]
	private Button button;
}
