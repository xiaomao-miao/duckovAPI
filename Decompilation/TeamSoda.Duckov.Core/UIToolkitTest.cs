using System;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000209 RID: 521
public class UIToolkitTest : MonoBehaviour
{
	// Token: 0x06000F32 RID: 3890 RVA: 0x0003BDD8 File Offset: 0x00039FD8
	private void Awake()
	{
		VisualElement visualElement = this.doc.rootVisualElement.Q("Button", null);
		CallbackEventHandler callbackEventHandler = this.doc.rootVisualElement.Q("Button2", null);
		visualElement.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnButtonClicked), TrickleDown.NoTrickleDown);
		callbackEventHandler.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnButton2Clicked), TrickleDown.NoTrickleDown);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0003BE37 File Offset: 0x0003A037
	private void OnButton2Clicked(ClickEvent evt)
	{
		Debug.Log("Button 2 Clicked");
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0003BE43 File Offset: 0x0003A043
	private void OnButtonClicked(ClickEvent evt)
	{
		Debug.Log("Button Clicked");
	}

	// Token: 0x04000C65 RID: 3173
	[SerializeField]
	private UIDocument doc;
}
