using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x0200031B RID: 795
	public class BuildingContextMenu : MonoBehaviour
	{
		// Token: 0x06001A70 RID: 6768 RVA: 0x0005FB0D File Offset: 0x0005DD0D
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.recycleButton.onPointerClick += this.OnRecycleButtonClicked;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0005FB37 File Offset: 0x0005DD37
		private void OnRecycleButtonClicked(BuildingContextMenuEntry entry)
		{
			if (this.Target == null)
			{
				return;
			}
			BuildingManager.ReturnBuilding(this.Target.GUID, null).Forget<bool>();
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x0005FB5E File Offset: 0x0005DD5E
		// (set) Token: 0x06001A73 RID: 6771 RVA: 0x0005FB66 File Offset: 0x0005DD66
		public Building Target { get; private set; }

		// Token: 0x06001A74 RID: 6772 RVA: 0x0005FB6F File Offset: 0x0005DD6F
		public void Setup(Building target)
		{
			this.Target = target;
			if (target == null)
			{
				this.Hide();
				return;
			}
			this.nameText.text = target.DisplayName;
			this.Show();
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0005FBA0 File Offset: 0x0005DDA0
		private void LateUpdate()
		{
			if (this.Target == null)
			{
				this.Hide();
				return;
			}
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(GameCamera.Instance.renderCamera, this.Target.transform.position);
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, screenPoint, null, out v);
			this.rectTransform.localPosition = v;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0005FC0D File Offset: 0x0005DE0D
		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0005FC1B File Offset: 0x0005DE1B
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x040012F8 RID: 4856
		private RectTransform rectTransform;

		// Token: 0x040012F9 RID: 4857
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040012FA RID: 4858
		[SerializeField]
		private BuildingContextMenuEntry recycleButton;
	}
}
