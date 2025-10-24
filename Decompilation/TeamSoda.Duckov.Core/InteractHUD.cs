using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class InteractHUD : MonoBehaviour
{
	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001B7F8 File Offset: 0x000199F8
	private PrefabPool<InteractSelectionHUD> Selections
	{
		get
		{
			if (this._selectionsCache == null)
			{
				this._selectionsCache = new PrefabPool<InteractSelectionHUD>(this.selectionPrefab, null, null, null, null, true, 10, 10000, null);
			}
			return this._selectionsCache;
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0001B831 File Offset: 0x00019A31
	private void Awake()
	{
		this.interactableGroup = new List<InteractableBase>();
		this.selectionsHUD = new List<InteractSelectionHUD>();
		this.selectionPrefab.gameObject.SetActive(false);
		this.master.gameObject.SetActive(false);
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0001B86C File Offset: 0x00019A6C
	private void Update()
	{
		if (this.characterMainControl == null)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl == null)
			{
				return;
			}
		}
		if (this.camera == null)
		{
			this.camera = Camera.main;
			if (this.camera == null)
			{
				return;
			}
		}
		bool flag = false;
		bool flag2 = false;
		this.interactableMaster = this.characterMainControl.interactAction.MasterInteractableAround;
		bool flag3 = InputManager.InputActived && (!this.characterMainControl.CurrentAction || !this.characterMainControl.CurrentAction.Running);
		Shader.SetGlobalFloat(this.interactableHash, flag3 ? 1f : 0f);
		this.interactable = (this.interactableMaster != null && flag3);
		if (this.interactable)
		{
			if (this.interactableMaster != this.interactableMasterTemp)
			{
				this.interactableMasterTemp = this.interactableMaster;
				flag = true;
				flag2 = true;
			}
			if (this.interactableIndexTemp != this.characterMainControl.interactAction.InteractIndexInGroup)
			{
				this.interactableIndexTemp = this.characterMainControl.interactAction.InteractIndexInGroup;
				flag2 = true;
			}
		}
		else
		{
			this.interactableMasterTemp = null;
		}
		if (this.interactable != this.master.gameObject.activeInHierarchy)
		{
			this.master.gameObject.SetActive(this.interactable);
		}
		if (flag)
		{
			this.RefreshContent();
			this.SyncPos();
		}
		if (flag2)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0001B9F3 File Offset: 0x00019BF3
	private void LateUpdate()
	{
		if (this.characterMainControl == null)
		{
			return;
		}
		if (this.camera == null)
		{
			return;
		}
		this.SyncPos();
		this.UpdateInteractLine();
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0001BA20 File Offset: 0x00019C20
	private void SyncPos()
	{
		if (!this.syncPosToTarget)
		{
			return;
		}
		if (!this.interactableMaster)
		{
			return;
		}
		Vector3 position = this.interactableMaster.transform.TransformPoint(this.interactableMaster.interactMarkerOffset);
		Vector3 v = LevelManager.Instance.GameCamera.renderCamera.WorldToScreenPoint(position);
		Vector2 v2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, v, null, out v2);
		base.transform.localPosition = v2;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0001BAA8 File Offset: 0x00019CA8
	private void RefreshContent()
	{
		if (this.interactableMaster == null)
		{
			return;
		}
		this.selectionsHUD.Clear();
		this.interactableGroup.Clear();
		foreach (InteractableBase interactableBase in this.interactableMaster.GetInteractableList())
		{
			if (interactableBase != null)
			{
				this.interactableGroup.Add(interactableBase);
			}
		}
		this.Selections.ReleaseAll();
		foreach (InteractableBase interactableBase2 in this.interactableGroup)
		{
			InteractSelectionHUD interactSelectionHUD = this.Selections.Get(null);
			interactSelectionHUD.transform.SetAsLastSibling();
			interactSelectionHUD.SetInteractable(interactableBase2, this.interactableGroup.Count > 1);
			this.selectionsHUD.Add(interactSelectionHUD);
		}
		this.master.ForceUpdateRectTransforms();
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0001BBC0 File Offset: 0x00019DC0
	private void RefreshSelection()
	{
		InteractableBase interactTarget = this.characterMainControl.interactAction.InteractTarget;
		foreach (InteractSelectionHUD interactSelectionHUD in this.selectionsHUD)
		{
			if (interactSelectionHUD.InteractTarget == interactTarget)
			{
				interactSelectionHUD.SetSelection(true);
			}
			else
			{
				interactSelectionHUD.SetSelection(false);
			}
		}
		this.master.ForceUpdateRectTransforms();
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0001BC48 File Offset: 0x00019E48
	private void UpdateInteractLine()
	{
	}

	// Token: 0x040005CC RID: 1484
	private CharacterMainControl characterMainControl;

	// Token: 0x040005CD RID: 1485
	public RectTransform master;

	// Token: 0x040005CE RID: 1486
	private InteractableBase interactableMaster;

	// Token: 0x040005CF RID: 1487
	private InteractableBase interactableMasterTemp;

	// Token: 0x040005D0 RID: 1488
	private List<InteractableBase> interactableGroup;

	// Token: 0x040005D1 RID: 1489
	private List<InteractSelectionHUD> selectionsHUD;

	// Token: 0x040005D2 RID: 1490
	private int interactableIndexTemp;

	// Token: 0x040005D3 RID: 1491
	private bool interactable;

	// Token: 0x040005D4 RID: 1492
	private Camera camera;

	// Token: 0x040005D5 RID: 1493
	public bool syncPosToTarget;

	// Token: 0x040005D6 RID: 1494
	public InteractSelectionHUD selectionPrefab;

	// Token: 0x040005D7 RID: 1495
	private int interactableHash = Shader.PropertyToID("Interactable");

	// Token: 0x040005D8 RID: 1496
	private PrefabPool<InteractSelectionHUD> _selectionsCache;
}
