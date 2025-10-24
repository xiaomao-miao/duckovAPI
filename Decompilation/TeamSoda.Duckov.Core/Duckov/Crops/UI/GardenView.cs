using System;
using System.Runtime.CompilerServices;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Crops.UI
{
	// Token: 0x020002F0 RID: 752
	public class GardenView : View, IPointerClickHandler, IEventSystemHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, ICursorDataProvider
	{
		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001831 RID: 6193 RVA: 0x000588B9 File Offset: 0x00056AB9
		// (set) Token: 0x06001832 RID: 6194 RVA: 0x000588C0 File Offset: 0x00056AC0
		public static GardenView Instance { get; private set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001833 RID: 6195 RVA: 0x000588C8 File Offset: 0x00056AC8
		// (set) Token: 0x06001834 RID: 6196 RVA: 0x000588D0 File Offset: 0x00056AD0
		public Garden Target { get; private set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001835 RID: 6197 RVA: 0x000588D9 File Offset: 0x00056AD9
		// (set) Token: 0x06001836 RID: 6198 RVA: 0x000588E1 File Offset: 0x00056AE1
		public bool SeedSelected { get; private set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x000588EA File Offset: 0x00056AEA
		// (set) Token: 0x06001838 RID: 6200 RVA: 0x000588F2 File Offset: 0x00056AF2
		public int PlantingSeedTypeID
		{
			get
			{
				return this._plantingSeedTypeID;
			}
			private set
			{
				this._plantingSeedTypeID = value;
				this.SeedMeta = ItemAssetsCollection.GetMetaData(value);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x00058907 File Offset: 0x00056B07
		// (set) Token: 0x0600183A RID: 6202 RVA: 0x0005890F File Offset: 0x00056B0F
		public ItemMetaData SeedMeta { get; private set; }

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600183B RID: 6203 RVA: 0x00058918 File Offset: 0x00056B18
		// (set) Token: 0x0600183C RID: 6204 RVA: 0x00058920 File Offset: 0x00056B20
		public GardenView.ToolType Tool { get; private set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600183D RID: 6205 RVA: 0x00058929 File Offset: 0x00056B29
		// (set) Token: 0x0600183E RID: 6206 RVA: 0x00058931 File Offset: 0x00056B31
		public bool Hovering { get; private set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x0005893A File Offset: 0x00056B3A
		// (set) Token: 0x06001840 RID: 6208 RVA: 0x00058942 File Offset: 0x00056B42
		public Vector2Int HoveringCoord { get; private set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x0005894B File Offset: 0x00056B4B
		// (set) Token: 0x06001842 RID: 6210 RVA: 0x00058953 File Offset: 0x00056B53
		public Crop HoveringCrop { get; private set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x0005895C File Offset: 0x00056B5C
		public string ToolDisplayName
		{
			get
			{
				switch (this.Tool)
				{
				case GardenView.ToolType.None:
					return "...";
				case GardenView.ToolType.Plant:
					return this.textKey_Plant.ToPlainText();
				case GardenView.ToolType.Harvest:
					return this.textKey_Harvest.ToPlainText();
				case GardenView.ToolType.Water:
					return this.textKey_Water.ToPlainText();
				case GardenView.ToolType.Destroy:
					return this.textKey_Destroy.ToPlainText();
				default:
					return "?";
				}
			}
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06001844 RID: 6212 RVA: 0x000589C8 File Offset: 0x00056BC8
		// (remove) Token: 0x06001845 RID: 6213 RVA: 0x00058A00 File Offset: 0x00056C00
		public event Action onContextChanged;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06001846 RID: 6214 RVA: 0x00058A38 File Offset: 0x00056C38
		// (remove) Token: 0x06001847 RID: 6215 RVA: 0x00058A70 File Offset: 0x00056C70
		public event Action onToolChanged;

		// Token: 0x06001848 RID: 6216 RVA: 0x00058AA5 File Offset: 0x00056CA5
		protected override void Awake()
		{
			base.Awake();
			this.btn_ChangePlant.onClick.AddListener(new UnityAction(this.OnBtnChangePlantClicked));
			ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
			GardenView.Instance = this;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00058AE0 File Offset: 0x00056CE0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x00058AF9 File Offset: 0x00056CF9
		private void OnDisable()
		{
			if (this.cellHoveringGizmos)
			{
				this.cellHoveringGizmos.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00058B19 File Offset: 0x00056D19
		private void OnPlayerItemOperation()
		{
			if (base.gameObject.activeSelf && this.SeedSelected)
			{
				this.RefreshSeedAmount();
			}
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00058B36 File Offset: 0x00056D36
		public static void Show(Garden target)
		{
			GardenView.Instance.Target = target;
			GardenView.Instance.Open(null);
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00058B50 File Offset: 0x00056D50
		protected override void OnOpen()
		{
			base.OnOpen();
			if (this.Target == null)
			{
				this.Target = UnityEngine.Object.FindObjectOfType<Garden>();
			}
			if (this.Target == null)
			{
				Debug.Log("No Garden instance found. Aborting..");
				base.Close();
			}
			this.fadeGroup.Show();
			this.RefreshSeedInfoDisplay();
			this.EnableCursor();
			this.SetTool(this.Tool);
			this.CenterCamera();
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00058BC3 File Offset: 0x00056DC3
		protected override void OnClose()
		{
			base.OnClose();
			this.cropSelector.Hide();
			this.fadeGroup.Hide();
			this.ReleaseCursor();
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00058BE7 File Offset: 0x00056DE7
		private void EnableCursor()
		{
			CursorManager.Register(this);
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00058BEF File Offset: 0x00056DEF
		private void ReleaseCursor()
		{
			CursorManager.Unregister(this);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x00058BF8 File Offset: 0x00056DF8
		private void ChangeCursor()
		{
			CursorManager.NotifyRefresh();
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00058BFF File Offset: 0x00056DFF
		private void Update()
		{
			this.UpdateContext();
			this.UpdateCursor3D();
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x00058C0D File Offset: 0x00056E0D
		private void OnBtnChangePlantClicked()
		{
			this.cropSelector.Show();
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x00058C1C File Offset: 0x00056E1C
		private void OnContextChanged()
		{
			Action action = this.onContextChanged;
			if (action != null)
			{
				action();
			}
			this.RefreshHoveringGizmos();
			this.RefreshCursor();
			if (this.dragging && this.Hovering)
			{
				this.ExecuteTool(this.HoveringCoord);
			}
			this.ChangeCursor();
			this.RefreshCursor3DActive();
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x00058C70 File Offset: 0x00056E70
		private void RefreshCursor()
		{
			this.cursorIcon.gameObject.SetActive(false);
			this.cursorAmountDisplay.gameObject.SetActive(false);
			this.cursorItemDisplay.gameObject.SetActive(false);
			switch (this.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				this.cursorAmountDisplay.gameObject.SetActive(this.SeedSelected);
				this.cursorItemDisplay.gameObject.SetActive(this.SeedSelected);
				this.cursorIcon.sprite = this.iconPlant;
				return;
			case GardenView.ToolType.Harvest:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconHarvest;
				return;
			case GardenView.ToolType.Water:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconWater;
				return;
			case GardenView.ToolType.Destroy:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconDestroy;
				break;
			default:
				return;
			}
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00058D78 File Offset: 0x00056F78
		private void RefreshHoveringGizmos()
		{
			if (!this.cellHoveringGizmos)
			{
				return;
			}
			if (!this.Hovering || !base.enabled)
			{
				this.cellHoveringGizmos.gameObject.SetActive(false);
				return;
			}
			this.cellHoveringGizmos.gameObject.SetActive(true);
			this.cellHoveringGizmos.SetParent(null);
			this.cellHoveringGizmos.localScale = Vector3.one;
			this.cellHoveringGizmos.position = this.Target.CoordToWorldPosition(this.HoveringCoord);
			this.cellHoveringGizmos.rotation = Quaternion.LookRotation(-Vector3.up);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x00058E18 File Offset: 0x00057018
		public void SetTool(GardenView.ToolType action)
		{
			this.Tool = action;
			this.OnContextChanged();
			this.plantModePanel.SetActive(action == GardenView.ToolType.Plant);
			Action action2 = this.onToolChanged;
			if (action2 != null)
			{
				action2();
			}
			this.RefreshSeedAmount();
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00058E4D File Offset: 0x0005704D
		private CursorData GetCursorByTool(GardenView.ToolType action)
		{
			return null;
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00058E50 File Offset: 0x00057050
		private void UpdateContext()
		{
			bool hovering = this.Hovering;
			Crop hoveringCrop = this.HoveringCrop;
			Vector2Int hoveringCoord = this.HoveringCoord;
			Vector2Int? pointingCoord = this.GetPointingCoord();
			if (pointingCoord == null)
			{
				this.HoveringCrop = null;
				return;
			}
			this.HoveringCoord = pointingCoord.Value;
			this.HoveringCrop = this.Target[this.HoveringCoord];
			this.Hovering = this.hoveringBG;
			if (!this.HoveringCrop)
			{
				this.Hovering &= this.Target.IsCoordValid(this.HoveringCoord);
			}
			if (hovering != this.HoveringCrop || hoveringCrop != this.HoveringCrop || hoveringCoord != this.HoveringCoord)
			{
				this.OnContextChanged();
			}
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00058F18 File Offset: 0x00057118
		private void UpdateCursor3D()
		{
			Vector3 a;
			bool flag = this.TryPointerOnPlanePoint(UIInputManager.Point, out a);
			this.show3DCursor = (flag && this.Hovering);
			this.cursor3DTransform.gameObject.SetActive(this.show3DCursor);
			if (!flag)
			{
				return;
			}
			Vector3 position = this.cursor3DTransform.position;
			Vector3 vector = a + this.cursor3DOffset;
			Vector3 position2;
			if (this.show3DCursor)
			{
				position2 = Vector3.Lerp(position, vector, 0.25f);
			}
			else
			{
				position2 = vector;
			}
			this.cursor3DTransform.position = position2;
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x00058FA0 File Offset: 0x000571A0
		private void RefreshCursor3DActive()
		{
			this.cursor3D_Plant.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Plant));
			this.cursor3D_Water.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Water));
			this.cursor3D_Harvest.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Harvest));
			this.cursor3D_Destory.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Destroy));
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00058FF5 File Offset: 0x000571F5
		public void SelectSeed(int seedTypeID)
		{
			this.PlantingSeedTypeID = seedTypeID;
			if (seedTypeID > 0)
			{
				this.SeedSelected = true;
			}
			this.RefreshSeedInfoDisplay();
			this.OnContextChanged();
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00059018 File Offset: 0x00057218
		private void RefreshSeedInfoDisplay()
		{
			if (this.SeedSelected)
			{
				this.seedItemDisplay.Setup(this.PlantingSeedTypeID);
				this.cursorItemDisplay.Setup(this.PlantingSeedTypeID);
			}
			this.seedItemDisplay.gameObject.SetActive(this.SeedSelected);
			this.seedItemPlaceHolder.gameObject.SetActive(!this.SeedSelected);
			this.RefreshSeedAmount();
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00059084 File Offset: 0x00057284
		private bool TryPointerOnPlanePoint(Vector2 pointerPos, out Vector3 planePoint)
		{
			planePoint = default(Vector3);
			if (this.Target == null)
			{
				return false;
			}
			Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, pointerPos);
			Plane plane = new Plane(this.Target.transform.up, this.Target.transform.position);
			float distance;
			if (!plane.Raycast(ray, out distance))
			{
				return false;
			}
			planePoint = ray.GetPoint(distance);
			return true;
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x000590F8 File Offset: 0x000572F8
		private bool TryPointerPosToCoord(Vector2 pointerPos, out Vector2Int result)
		{
			result = default(Vector2Int);
			if (this.Target == null)
			{
				return false;
			}
			Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, pointerPos);
			Plane plane = new Plane(this.Target.transform.up, this.Target.transform.position);
			float distance;
			if (!plane.Raycast(ray, out distance))
			{
				return false;
			}
			Vector3 point = ray.GetPoint(distance);
			result = this.Target.WorldPositionToCoord(point);
			return true;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0005917C File Offset: 0x0005737C
		private Vector2Int? GetPointingCoord()
		{
			Vector2Int value;
			if (!this.TryPointerPosToCoord(UIInputManager.Point, out value))
			{
				return null;
			}
			return new Vector2Int?(value);
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x000591A8 File Offset: 0x000573A8
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2Int coord;
			if (!this.TryPointerPosToCoord(eventData.position, out coord))
			{
				return;
			}
			this.ExecuteTool(coord);
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x000591D0 File Offset: 0x000573D0
		private void ExecuteTool(Vector2Int coord)
		{
			switch (this.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				this.CropActionPlant(coord);
				return;
			case GardenView.ToolType.Harvest:
				this.CropActionHarvest(coord);
				return;
			case GardenView.ToolType.Water:
				this.CropActionWater(coord);
				return;
			case GardenView.ToolType.Destroy:
				this.CropActionDestroy(coord);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00059220 File Offset: 0x00057420
		private void CropActionDestroy(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			if (crop.Ripen)
			{
				crop.Harvest();
				return;
			}
			crop.DestroyCrop();
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0005925C File Offset: 0x0005745C
		private void CropActionWater(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			crop.Water();
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00059288 File Offset: 0x00057488
		private void CropActionHarvest(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			crop.Harvest();
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x000592B4 File Offset: 0x000574B4
		private void CropActionPlant(Vector2Int coord)
		{
			if (!this.Target.IsCoordValid(coord))
			{
				return;
			}
			if (this.Target[coord] != null)
			{
				return;
			}
			CropInfo? cropInfoFromSeedType = this.GetCropInfoFromSeedType(this.PlantingSeedTypeID);
			if (cropInfoFromSeedType == null)
			{
				return;
			}
			Cost cost = new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(this.PlantingSeedTypeID, 1L)
			});
			if (!cost.Pay(true, true))
			{
				return;
			}
			this.Target.Plant(coord, cropInfoFromSeedType.Value.id);
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00059344 File Offset: 0x00057544
		private CropInfo? GetCropInfoFromSeedType(int plantingSeedTypeID)
		{
			SeedInfo seedInfo = CropDatabase.GetSeedInfo(plantingSeedTypeID);
			if (seedInfo.cropIDs == null)
			{
				return null;
			}
			if (seedInfo.cropIDs.Count <= 0)
			{
				return null;
			}
			return CropDatabase.GetCropInfo(seedInfo.GetRandomCropID());
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00059390 File Offset: 0x00057590
		public void OnPointerMove(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == this.mainEventReceiver)
			{
				this.hoveringBG = true;
				return;
			}
			this.hoveringBG = false;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x000593C8 File Offset: 0x000575C8
		private void RefreshSeedAmount()
		{
			if (this.SeedSelected)
			{
				int itemCount = ItemUtilities.GetItemCount(this.PlantingSeedTypeID);
				this.seedAmount = itemCount;
				string text = string.Format("x{0}", itemCount);
				this.seedAmountText.text = text;
				this.cursorAmountDisplay.text = text;
				return;
			}
			this.seedAmountText.text = "";
			this.cursorAmountDisplay.text = "";
			this.seedAmount = 0;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00059441 File Offset: 0x00057641
		public void OnPointerDown(PointerEventData eventData)
		{
			this.dragging = true;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0005944A File Offset: 0x0005764A
		public void OnPointerUp(PointerEventData eventData)
		{
			this.dragging = false;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00059453 File Offset: 0x00057653
		public void OnPointerExit(PointerEventData eventData)
		{
			this.dragging = false;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0005945C File Offset: 0x0005765C
		private void UpdateCamera()
		{
			this.cameraRig.transform.position = this.camFocusPos;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00059474 File Offset: 0x00057674
		private void CenterCamera()
		{
			if (this.Target == null)
			{
				return;
			}
			this.camFocusPos = this.Target.transform.TransformPoint(this.Target.cameraRigCenter);
			this.UpdateCamera();
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x000594AC File Offset: 0x000576AC
		public CursorData GetCursorData()
		{
			return this.GetCursorByTool(this.Tool);
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00059514 File Offset: 0x00057714
		[CompilerGenerated]
		private bool <RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType toolType)
		{
			if (this.Tool != toolType)
			{
				return false;
			}
			if (!this.Hovering)
			{
				return false;
			}
			switch (toolType)
			{
			case GardenView.ToolType.None:
				return false;
			case GardenView.ToolType.Plant:
				return this.SeedSelected && this.seedAmount > 0 && !this.HoveringCrop;
			case GardenView.ToolType.Harvest:
				return this.HoveringCrop && this.HoveringCrop.Ripen;
			case GardenView.ToolType.Water:
				return this.HoveringCrop;
			case GardenView.ToolType.Destroy:
				return this.HoveringCrop;
			default:
				return false;
			}
		}

		// Token: 0x040011A2 RID: 4514
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040011A3 RID: 4515
		[SerializeField]
		private GameObject mainEventReceiver;

		// Token: 0x040011A4 RID: 4516
		[SerializeField]
		private Button btn_ChangePlant;

		// Token: 0x040011A5 RID: 4517
		[SerializeField]
		private GameObject plantModePanel;

		// Token: 0x040011A6 RID: 4518
		[SerializeField]
		private ItemMetaDisplay seedItemDisplay;

		// Token: 0x040011A7 RID: 4519
		[SerializeField]
		private GameObject seedItemPlaceHolder;

		// Token: 0x040011A8 RID: 4520
		[SerializeField]
		private TextMeshProUGUI seedAmountText;

		// Token: 0x040011A9 RID: 4521
		[SerializeField]
		private GardenViewCropSelector cropSelector;

		// Token: 0x040011AA RID: 4522
		[SerializeField]
		private Transform cellHoveringGizmos;

		// Token: 0x040011AB RID: 4523
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Plant = "Garden_Plant";

		// Token: 0x040011AC RID: 4524
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Harvest = "Garden_Harvest";

		// Token: 0x040011AD RID: 4525
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Destroy = "Garden_Destroy";

		// Token: 0x040011AE RID: 4526
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Water = "Garden_Water";

		// Token: 0x040011AF RID: 4527
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_TargetOccupied = "Garden_TargetOccupied";

		// Token: 0x040011B0 RID: 4528
		[SerializeField]
		private Transform cameraRig;

		// Token: 0x040011B1 RID: 4529
		[SerializeField]
		private Image cursorIcon;

		// Token: 0x040011B2 RID: 4530
		[SerializeField]
		private TextMeshProUGUI cursorAmountDisplay;

		// Token: 0x040011B3 RID: 4531
		[SerializeField]
		private ItemMetaDisplay cursorItemDisplay;

		// Token: 0x040011B4 RID: 4532
		[SerializeField]
		private Sprite iconPlant;

		// Token: 0x040011B5 RID: 4533
		[SerializeField]
		private Sprite iconHarvest;

		// Token: 0x040011B6 RID: 4534
		[SerializeField]
		private Sprite iconWater;

		// Token: 0x040011B7 RID: 4535
		[SerializeField]
		private Sprite iconDestroy;

		// Token: 0x040011B8 RID: 4536
		[SerializeField]
		private CursorData cursorPlant;

		// Token: 0x040011B9 RID: 4537
		[SerializeField]
		private CursorData cursorHarvest;

		// Token: 0x040011BA RID: 4538
		[SerializeField]
		private CursorData cursorWater;

		// Token: 0x040011BB RID: 4539
		[SerializeField]
		private CursorData cursorDestroy;

		// Token: 0x040011BC RID: 4540
		[SerializeField]
		private Transform cursor3DTransform;

		// Token: 0x040011BD RID: 4541
		[SerializeField]
		private Vector3 cursor3DOffset = Vector3.up;

		// Token: 0x040011BE RID: 4542
		[SerializeField]
		private GameObject cursor3D_Plant;

		// Token: 0x040011BF RID: 4543
		[SerializeField]
		private GameObject cursor3D_Harvest;

		// Token: 0x040011C0 RID: 4544
		[SerializeField]
		private GameObject cursor3D_Water;

		// Token: 0x040011C1 RID: 4545
		[SerializeField]
		private GameObject cursor3D_Destory;

		// Token: 0x040011C2 RID: 4546
		private Vector3 camFocusPos;

		// Token: 0x040011C5 RID: 4549
		private int _plantingSeedTypeID;

		// Token: 0x040011CD RID: 4557
		private bool enabledCursor;

		// Token: 0x040011CE RID: 4558
		private bool show3DCursor;

		// Token: 0x040011CF RID: 4559
		private bool hoveringBG;

		// Token: 0x040011D0 RID: 4560
		private int seedAmount;

		// Token: 0x040011D1 RID: 4561
		private bool dragging;

		// Token: 0x02000585 RID: 1413
		public enum ToolType
		{
			// Token: 0x04001FB7 RID: 8119
			None,
			// Token: 0x04001FB8 RID: 8120
			Plant,
			// Token: 0x04001FB9 RID: 8121
			Harvest,
			// Token: 0x04001FBA RID: 8122
			Water,
			// Token: 0x04001FBB RID: 8123
			Destroy
		}
	}
}
