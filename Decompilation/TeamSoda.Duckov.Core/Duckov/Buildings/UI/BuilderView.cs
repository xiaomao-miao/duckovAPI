using System;
using Cinemachine;
using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000319 RID: 793
	public class BuilderView : View, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x0005ED11 File Offset: 0x0005CF11
		public static BuilderView Instance
		{
			get
			{
				return View.GetViewInstance<BuilderView>();
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0005ED18 File Offset: 0x0005CF18
		public void SetupAndShow(BuildingArea targetArea)
		{
			this.targetArea = targetArea;
			base.Open(null);
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0005ED28 File Offset: 0x0005CF28
		protected override void Awake()
		{
			base.Awake();
			this.input_Rotate.action.actionMap.Enable();
			this.input_MoveCamera.action.actionMap.Enable();
			this.selectionPanel.onButtonSelected += this.OnButtonSelected;
			this.selectionPanel.onRecycleRequested += this.OnRecycleRequested;
			BuildingManager.OnBuildingListChanged += this.OnBuildingListChanged;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0005EDA4 File Offset: 0x0005CFA4
		private void OnRecycleRequested(BuildingBtnEntry entry)
		{
			BuildingManager.ReturnBuildingsOfType(entry.Info.id, null).Forget<int>();
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0005EDBC File Offset: 0x0005CFBC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			BuildingManager.OnBuildingListChanged -= this.OnBuildingListChanged;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0005EDD5 File Offset: 0x0005CFD5
		private void OnBuildingListChanged()
		{
			this.selectionPanel.Refresh();
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0005EDE4 File Offset: 0x0005CFE4
		private void OnButtonSelected(BuildingBtnEntry entry)
		{
			if (!entry.CostEnough)
			{
				this.NotifyCostNotEnough(entry);
				return;
			}
			if (entry.Info.ReachedAmountLimit)
			{
				return;
			}
			this.BeginPlacing(entry.Info);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0005EE20 File Offset: 0x0005D020
		private void NotifyCostNotEnough(BuildingBtnEntry entry)
		{
			Debug.Log("Resource not enough " + entry.Info.DisplayName);
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0005EE4A File Offset: 0x0005D04A
		private void SetMode(BuilderView.Mode mode)
		{
			this.placingModeInputIndicator.SetActive(false);
			this.OnExitMode(this.mode);
			this.mode = mode;
			switch (mode)
			{
			case BuilderView.Mode.None:
			case BuilderView.Mode.Destroying:
				break;
			case BuilderView.Mode.Placing:
				this.placingModeInputIndicator.SetActive(true);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0005EE8A File Offset: 0x0005D08A
		private void OnExitMode(BuilderView.Mode mode)
		{
			this.contextMenu.Hide();
			switch (mode)
			{
			case BuilderView.Mode.None:
			case BuilderView.Mode.Destroying:
				break;
			case BuilderView.Mode.Placing:
				this.OnExitPlacing();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x0005EEB0 File Offset: 0x0005D0B0
		public void BeginPlacing(BuildingInfo info)
		{
			if (this.previewBuilding != null)
			{
				UnityEngine.Object.Destroy(this.previewBuilding.gameObject);
			}
			this.placingBuildingInfo = info;
			this.SetMode(BuilderView.Mode.Placing);
			if (info.Prefab == null)
			{
				Debug.LogError("建筑 " + info.DisplayName + " 没有prefab");
			}
			this.previewBuilding = UnityEngine.Object.Instantiate<Building>(info.Prefab);
			if (this.previewBuilding.ID != info.id)
			{
				Debug.LogError("建筑 " + info.DisplayName + " 的 prefab 上的 ID 设置错误");
			}
			this.SetupPreview(this.previewBuilding);
			this.UpdatePlacing();
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x0005EF6A File Offset: 0x0005D16A
		public void BeginDestroying()
		{
			this.SetMode(BuilderView.Mode.Destroying);
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0005EF73 File Offset: 0x0005D173
		private void SetupPreview(Building previewBuilding)
		{
			if (previewBuilding == null)
			{
				return;
			}
			previewBuilding.SetupPreview();
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0005EF85 File Offset: 0x0005D185
		private void OnExitPlacing()
		{
			if (this.previewBuilding != null)
			{
				UnityEngine.Object.Destroy(this.previewBuilding.gameObject);
			}
			GridDisplay.HidePreview();
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x0005EFAC File Offset: 0x0005D1AC
		private void Update()
		{
			switch (this.mode)
			{
			case BuilderView.Mode.None:
				this.UpdateNone();
				break;
			case BuilderView.Mode.Placing:
				this.UpdatePlacing();
				break;
			case BuilderView.Mode.Destroying:
				this.UpdateDestroying();
				break;
			}
			this.UpdateCamera();
			this.UpdateContextMenuIndicator();
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0005EFF8 File Offset: 0x0005D1F8
		private unsafe void UpdateContextMenuIndicator()
		{
			Vector2Int coord;
			this.TryGetPointingCoord(out coord, null);
			bool flag = this.targetArea.GetBuildingInstanceAt(coord);
			bool isActiveAndEnabled = this.contextMenu.isActiveAndEnabled;
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.followCursorUI.parent as RectTransform, *Mouse.current.position.value, null, out v);
			this.followCursorUI.localPosition = v;
			bool flag2 = flag && !isActiveAndEnabled;
			if (flag2 && !this.hoveringBuildingFadeGroup.IsShown)
			{
				this.hoveringBuildingFadeGroup.Show();
			}
			if (!flag2 && this.hoveringBuildingFadeGroup.IsShown)
			{
				this.hoveringBuildingFadeGroup.Hide();
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0005F0A8 File Offset: 0x0005D2A8
		private void UpdateNone()
		{
			if (this.input_RequestContextMenu.action.WasPressedThisFrame())
			{
				Vector2Int coord;
				if (!this.TryGetPointingCoord(out coord, null))
				{
					return;
				}
				Building buildingInstanceAt = this.targetArea.GetBuildingInstanceAt(coord);
				if (buildingInstanceAt == null)
				{
					this.contextMenu.Hide();
					return;
				}
				this.contextMenu.Setup(buildingInstanceAt);
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0005F104 File Offset: 0x0005D304
		private void UpdateDestroying()
		{
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, null))
			{
				GridDisplay.HidePreview();
				return;
			}
			BuildingManager.BuildingData buildingAt = this.targetArea.AreaData.GetBuildingAt(coord);
			if (buildingAt == null)
			{
				GridDisplay.HidePreview();
				return;
			}
			this.gridDisplay.SetBuildingPreviewCoord(buildingAt.Coord, buildingAt.Dimensions, buildingAt.Rotation, false);
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0005F15C File Offset: 0x0005D35C
		private void ConfirmDestroy()
		{
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, null))
			{
				return;
			}
			BuildingManager.BuildingData buildingAt = this.targetArea.AreaData.GetBuildingAt(coord);
			if (buildingAt == null)
			{
				return;
			}
			BuildingManager.ReturnBuilding(buildingAt.GUID, null).Forget<bool>();
			this.SetMode(BuilderView.Mode.None);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0005F1A4 File Offset: 0x0005D3A4
		private void ConfirmPlacement()
		{
			if (this.previewBuilding == null)
			{
				Debug.Log("No Previewing Building");
				return;
			}
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, this.previewBuilding))
			{
				this.previewBuilding.gameObject.SetActive(false);
				Debug.Log("Mouse Not in Plane!");
				return;
			}
			if (!this.IsValidPlacement(this.previewBuilding.Dimensions, this.previewRotation, coord))
			{
				Debug.Log("Invalid Placement!");
				return;
			}
			BuildingManager.BuyAndPlace(this.targetArea.AreaID, this.previewBuilding.ID, coord, this.previewRotation);
			this.SetMode(BuilderView.Mode.None);
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0005F248 File Offset: 0x0005D448
		private void UpdatePlacing()
		{
			if (this.previewBuilding)
			{
				Vector2Int coord;
				if (!this.TryGetPointingCoord(out coord, this.previewBuilding))
				{
					this.previewBuilding.gameObject.SetActive(false);
					return;
				}
				bool validPlacement = this.IsValidPlacement(this.previewBuilding.Dimensions, this.previewRotation, coord);
				this.gridDisplay.SetBuildingPreviewCoord(coord, this.previewBuilding.Dimensions, this.previewRotation, validPlacement);
				this.ShowPreview(coord);
				if (this.input_Rotate.action.WasPressedThisFrame())
				{
					float num = this.input_Rotate.action.ReadValue<float>();
					this.previewRotation = (BuildingRotation)(((float)this.previewRotation + num + 4f) % 4f);
				}
				if (this.input_RequestContextMenu.action.WasPressedThisFrame())
				{
					this.SetMode(BuilderView.Mode.None);
					return;
				}
			}
			else
			{
				this.SetMode(BuilderView.Mode.None);
			}
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0005F328 File Offset: 0x0005D528
		private void ShowPreview(Vector2Int coord)
		{
			Vector3 position = this.targetArea.CoordToWorldPosition(coord, this.previewBuilding.Dimensions, this.previewRotation);
			this.previewBuilding.transform.position = position;
			this.previewBuilding.gameObject.SetActive(true);
			Quaternion rhs = Quaternion.Euler(new Vector3(0f, (float)((BuildingRotation)90 * this.previewRotation), 0f));
			this.previewBuilding.transform.rotation = this.targetArea.transform.rotation * rhs;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0005F3BC File Offset: 0x0005D5BC
		public bool TryGetPointingCoord(out Vector2Int coord, Building previewBuilding = null)
		{
			coord = default(Vector2Int);
			Ray pointRay = UIInputManager.GetPointRay();
			float distance;
			if (!this.targetArea.Plane.Raycast(pointRay, out distance))
			{
				return false;
			}
			Vector3 point = pointRay.GetPoint(distance);
			if (previewBuilding != null)
			{
				coord = this.targetArea.CursorToCoord(point, previewBuilding.Dimensions, this.previewRotation);
				return true;
			}
			coord = this.targetArea.CursorToCoord(point, Vector2Int.one, BuildingRotation.Zero);
			return true;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0005F440 File Offset: 0x0005D640
		private bool IsValidPlacement(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
		{
			return this.targetArea.IsPlacementWithinRange(dimensions, rotation, coord) && !this.targetArea.AreaData.Collide(dimensions, rotation, coord) && !this.targetArea.PhysicsCollide(dimensions, rotation, coord, 0f, 2f);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x0005F494 File Offset: 0x0005D694
		protected override void OnOpen()
		{
			base.OnOpen();
			this.SetMode(BuilderView.Mode.None);
			this.fadeGroup.Show();
			this.selectionPanel.Setup(this.targetArea);
			this.gridDisplay.Setup(this.targetArea);
			this.cameraCursor = this.targetArea.transform.position;
			this.UpdateCamera();
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x0005F4F7 File Offset: 0x0005D6F7
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			GridDisplay.Close();
			if (this.previewBuilding != null)
			{
				UnityEngine.Object.Destroy(this.previewBuilding.gameObject);
			}
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x0005F530 File Offset: 0x0005D730
		private void UpdateCamera()
		{
			if (this.input_MoveCamera.action.IsPressed())
			{
				Vector2 vector = this.input_MoveCamera.action.ReadValue<Vector2>();
				Transform transform = this.vcam.transform;
				float num = Mathf.Abs(Vector3.Dot(transform.forward, Vector3.up));
				float num2 = Mathf.Abs(Vector3.Dot(transform.up, Vector3.up));
				Vector3 a = ((num > num2) ? transform.up : transform.forward).ProjectOntoPlane(Vector3.up);
				Vector3 a2 = transform.right.ProjectOntoPlane(Vector3.up);
				this.cameraCursor += (a2 * vector.x + a * vector.y) * this.cameraSpeed * Time.unscaledDeltaTime;
				this.cameraCursor.x = Mathf.Clamp(this.cameraCursor.x, this.targetArea.transform.position.x - (float)this.targetArea.Size.x, this.targetArea.transform.position.x + (float)this.targetArea.Size.x);
				this.cameraCursor.z = Mathf.Clamp(this.cameraCursor.z, this.targetArea.transform.position.z - (float)this.targetArea.Size.y, this.targetArea.transform.position.z + (float)this.targetArea.Size.y);
			}
			this.vcam.transform.position = this.cameraCursor + Quaternion.Euler(0f, this.yaw, 0f) * Quaternion.Euler(this.pitch, 0f, 0f) * Vector3.forward * this.cameraDistance;
			this.vcam.transform.LookAt(this.cameraCursor, Vector3.up);
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0005F768 File Offset: 0x0005D968
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.contextMenu.Hide();
				BuilderView.Mode mode = this.mode;
				if (mode == BuilderView.Mode.Placing)
				{
					this.ConfirmPlacement();
					return;
				}
				if (mode != BuilderView.Mode.Destroying)
				{
					return;
				}
				this.ConfirmDestroy();
			}
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0005F7A5 File Offset: 0x0005D9A5
		public static void Show(BuildingArea target)
		{
			BuilderView.Instance.SetupAndShow(target);
		}

		// Token: 0x040012D3 RID: 4819
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040012D4 RID: 4820
		[SerializeField]
		private BuildingSelectionPanel selectionPanel;

		// Token: 0x040012D5 RID: 4821
		[SerializeField]
		private BuildingContextMenu contextMenu;

		// Token: 0x040012D6 RID: 4822
		[SerializeField]
		private GameObject placingModeInputIndicator;

		// Token: 0x040012D7 RID: 4823
		[SerializeField]
		private RectTransform followCursorUI;

		// Token: 0x040012D8 RID: 4824
		[SerializeField]
		private FadeGroup hoveringBuildingFadeGroup;

		// Token: 0x040012D9 RID: 4825
		[SerializeField]
		private CinemachineVirtualCamera vcam;

		// Token: 0x040012DA RID: 4826
		[SerializeField]
		private float cameraSpeed = 10f;

		// Token: 0x040012DB RID: 4827
		[SerializeField]
		private float pitch = 45f;

		// Token: 0x040012DC RID: 4828
		[SerializeField]
		private float cameraDistance = 10f;

		// Token: 0x040012DD RID: 4829
		[SerializeField]
		private float yaw = -45f;

		// Token: 0x040012DE RID: 4830
		[SerializeField]
		private Vector3 cameraCursor;

		// Token: 0x040012DF RID: 4831
		[SerializeField]
		private BuildingInfo placingBuildingInfo;

		// Token: 0x040012E0 RID: 4832
		[SerializeField]
		private InputActionReference input_Rotate;

		// Token: 0x040012E1 RID: 4833
		[SerializeField]
		private InputActionReference input_RequestContextMenu;

		// Token: 0x040012E2 RID: 4834
		[SerializeField]
		private InputActionReference input_MoveCamera;

		// Token: 0x040012E3 RID: 4835
		[SerializeField]
		private GridDisplay gridDisplay;

		// Token: 0x040012E4 RID: 4836
		[SerializeField]
		private BuildingArea targetArea;

		// Token: 0x040012E5 RID: 4837
		[SerializeField]
		private BuilderView.Mode mode;

		// Token: 0x040012E6 RID: 4838
		private Building previewBuilding;

		// Token: 0x040012E7 RID: 4839
		[SerializeField]
		private BuildingRotation previewRotation;

		// Token: 0x020005BA RID: 1466
		private enum Mode
		{
			// Token: 0x04002063 RID: 8291
			None,
			// Token: 0x04002064 RID: 8292
			Placing,
			// Token: 0x04002065 RID: 8293
			Destroying
		}
	}
}
