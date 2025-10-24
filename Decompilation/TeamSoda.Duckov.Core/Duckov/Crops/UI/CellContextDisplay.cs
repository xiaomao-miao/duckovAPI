using System;
using Duckov.Economy;
using TMPro;
using UnityEngine;

namespace Duckov.Crops.UI
{
	// Token: 0x020002EF RID: 751
	public class CellContextDisplay : MonoBehaviour
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x000584EB File Offset: 0x000566EB
		private Garden Garden
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.Target;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001820 RID: 6176 RVA: 0x00058508 File Offset: 0x00056708
		private Vector2Int HoveringCoord
		{
			get
			{
				if (this.master == null)
				{
					return default(Vector2Int);
				}
				return this.master.HoveringCoord;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001821 RID: 6177 RVA: 0x00058538 File Offset: 0x00056738
		private Crop HoveringCrop
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.HoveringCrop;
			}
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x00058555 File Offset: 0x00056755
		private void Show()
		{
			this.canvasGroup.alpha = 1f;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00058567 File Offset: 0x00056767
		private void Hide()
		{
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00058579 File Offset: 0x00056779
		private void Awake()
		{
			this.master.onContextChanged += this.OnContextChanged;
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00058592 File Offset: 0x00056792
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x0005859A File Offset: 0x0005679A
		private bool AnyContent
		{
			get
			{
				return this.plantInfo.activeSelf || this.currentCropInfo.activeSelf || this.operationInfo.activeSelf;
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x000585C3 File Offset: 0x000567C3
		private void Update()
		{
			if (this.master.Hovering && this.AnyContent)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
			if (this.HoveringCrop)
			{
				this.UpdateCurrentCropInfo();
			}
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x000585FC File Offset: 0x000567FC
		private void LateUpdate()
		{
			Vector3 worldPoint = this.Garden.CoordToWorldPosition(this.HoveringCoord) + Vector3.up * 2f;
			Vector2 v = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPoint);
			base.transform.position = v;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0005864C File Offset: 0x0005684C
		private void OnContextChanged()
		{
			this.Refresh();
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x00058654 File Offset: 0x00056854
		private void Refresh()
		{
			this.HideAll();
			switch (this.master.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				if (this.HoveringCrop)
				{
					this.SetupCurrentCropInfo();
					return;
				}
				this.SetupPlantInfo();
				if (this.master.PlantingSeedTypeID > 0)
				{
					this.SetupOperationInfo();
					return;
				}
				break;
			case GardenView.ToolType.Harvest:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				if (this.HoveringCrop.Ripen)
				{
					this.SetupOperationInfo();
					return;
				}
				break;
			case GardenView.ToolType.Water:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				this.SetupOperationInfo();
				return;
			case GardenView.ToolType.Destroy:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				this.SetupOperationInfo();
				break;
			default:
				return;
			}
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0005871D File Offset: 0x0005691D
		private void SetupCurrentCropInfo()
		{
			this.currentCropInfo.SetActive(true);
			this.cropNameText.text = this.HoveringCrop.DisplayName;
			this.UpdateCurrentCropInfo();
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00058748 File Offset: 0x00056948
		private void UpdateCurrentCropInfo()
		{
			if (this.HoveringCrop == null)
			{
				return;
			}
			this.cropCountdownText.text = this.HoveringCrop.RemainingTime.ToString("hh\\:mm\\:ss");
			this.cropCountdownText.gameObject.SetActive(!this.HoveringCrop.Ripen && this.HoveringCrop.Data.watered);
			this.noWaterIndicator.SetActive(!this.HoveringCrop.Data.watered);
			this.ripenIndicator.SetActive(this.HoveringCrop.Ripen);
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x000587EB File Offset: 0x000569EB
		private void SetupOperationInfo()
		{
			this.operationInfo.SetActive(true);
			this.operationNameText.text = this.master.ToolDisplayName;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00058810 File Offset: 0x00056A10
		private void SetupPlantInfo()
		{
			if (!this.master.SeedSelected)
			{
				return;
			}
			this.plantInfo.SetActive(true);
			this.plantingCropNameText.text = this.master.SeedMeta.DisplayName;
			this.plantCostDisplay.Setup(new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(this.master.PlantingSeedTypeID, 1L)
			}), 1);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x00058885 File Offset: 0x00056A85
		private void HideAll()
		{
			this.plantInfo.SetActive(false);
			this.currentCropInfo.SetActive(false);
			this.operationInfo.SetActive(false);
			this.Hide();
		}

		// Token: 0x04001195 RID: 4501
		[SerializeField]
		private GardenView master;

		// Token: 0x04001196 RID: 4502
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04001197 RID: 4503
		[SerializeField]
		private GameObject plantInfo;

		// Token: 0x04001198 RID: 4504
		[SerializeField]
		private TextMeshProUGUI plantingCropNameText;

		// Token: 0x04001199 RID: 4505
		[SerializeField]
		private CostDisplay plantCostDisplay;

		// Token: 0x0400119A RID: 4506
		[SerializeField]
		private GameObject currentCropInfo;

		// Token: 0x0400119B RID: 4507
		[SerializeField]
		private TextMeshProUGUI cropNameText;

		// Token: 0x0400119C RID: 4508
		[SerializeField]
		private TextMeshProUGUI cropCountdownText;

		// Token: 0x0400119D RID: 4509
		[SerializeField]
		private GameObject noWaterIndicator;

		// Token: 0x0400119E RID: 4510
		[SerializeField]
		private GameObject ripenIndicator;

		// Token: 0x0400119F RID: 4511
		[SerializeField]
		private GameObject operationInfo;

		// Token: 0x040011A0 RID: 4512
		[SerializeField]
		private TextMeshProUGUI operationNameText;
	}
}
