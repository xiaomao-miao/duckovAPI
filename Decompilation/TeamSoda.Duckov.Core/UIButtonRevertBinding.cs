using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001DE RID: 478
public class UIButtonRevertBinding : MonoBehaviour
{
	// Token: 0x06000E37 RID: 3639 RVA: 0x00039643 File Offset: 0x00037843
	private void Awake()
	{
		if (this.button == null)
		{
			this.button = base.GetComponent<Button>();
		}
		this.button.onClick.AddListener(new UnityAction(this.OnBtnClick));
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0003967B File Offset: 0x0003787B
	public void OnBtnClick()
	{
		InputRebinder.Clear();
		InputRebinder.Save();
	}

	// Token: 0x04000BC1 RID: 3009
	[SerializeField]
	private Button button;
}
