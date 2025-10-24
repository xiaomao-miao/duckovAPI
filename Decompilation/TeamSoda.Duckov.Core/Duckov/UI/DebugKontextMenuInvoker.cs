using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000382 RID: 898
	public class DebugKontextMenuInvoker : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001F1F RID: 7967 RVA: 0x0006D05D File Offset: 0x0006B25D
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Show(eventData.position);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0006D06C File Offset: 0x0006B26C
		public void Show(Vector2 point)
		{
			KontextMenu kontextMenu = this.kontextMenu;
			KontextMenuDataEntry[] array = new KontextMenuDataEntry[5];
			int num = 0;
			KontextMenuDataEntry kontextMenuDataEntry = new KontextMenuDataEntry();
			kontextMenuDataEntry.text = "你好";
			kontextMenuDataEntry.action = delegate()
			{
				Debug.Log("好");
			};
			array[num] = kontextMenuDataEntry;
			int num2 = 1;
			KontextMenuDataEntry kontextMenuDataEntry2 = new KontextMenuDataEntry();
			kontextMenuDataEntry2.text = "你好2";
			kontextMenuDataEntry2.action = delegate()
			{
				Debug.Log("好好");
			};
			array[num2] = kontextMenuDataEntry2;
			int num3 = 2;
			KontextMenuDataEntry kontextMenuDataEntry3 = new KontextMenuDataEntry();
			kontextMenuDataEntry3.text = "你好3";
			kontextMenuDataEntry3.action = delegate()
			{
				Debug.Log("好好好");
			};
			array[num3] = kontextMenuDataEntry3;
			int num4 = 3;
			KontextMenuDataEntry kontextMenuDataEntry4 = new KontextMenuDataEntry();
			kontextMenuDataEntry4.text = "你好4";
			kontextMenuDataEntry4.action = delegate()
			{
				Debug.Log("好好好好");
			};
			array[num4] = kontextMenuDataEntry4;
			int num5 = 4;
			KontextMenuDataEntry kontextMenuDataEntry5 = new KontextMenuDataEntry();
			kontextMenuDataEntry5.text = "你好5";
			kontextMenuDataEntry5.action = delegate()
			{
				Debug.Log("好好好好好");
			};
			array[num5] = kontextMenuDataEntry5;
			kontextMenu.InstanceShow(this, point, array);
		}

		// Token: 0x04001545 RID: 5445
		[SerializeField]
		private KontextMenu kontextMenu;
	}
}
