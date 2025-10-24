using System;
using System.Collections.Generic;
using Duckov.MiniMaps;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C2 RID: 450
public class MapMarkerSettingsPanel : MonoBehaviour
{
	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000D65 RID: 3429 RVA: 0x0003748F File Offset: 0x0003568F
	private List<Sprite> Icons
	{
		get
		{
			return MapMarkerManager.Icons;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00037498 File Offset: 0x00035698
	private PrefabPool<MapMarkerPanelButton> IconBtnPool
	{
		get
		{
			if (this._iconBtnPool == null)
			{
				this._iconBtnPool = new PrefabPool<MapMarkerPanelButton>(this.iconBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._iconBtnPool;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000D67 RID: 3431 RVA: 0x000374D4 File Offset: 0x000356D4
	private PrefabPool<MapMarkerPanelButton> ColorBtnPool
	{
		get
		{
			if (this._colorBtnPool == null)
			{
				this._colorBtnPool = new PrefabPool<MapMarkerPanelButton>(this.colorBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._colorBtnPool;
		}
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00037510 File Offset: 0x00035710
	private void OnEnable()
	{
		this.Setup();
		MapMarkerManager.OnColorChanged = (Action<Color>)Delegate.Combine(MapMarkerManager.OnColorChanged, new Action<Color>(this.OnColorChanged));
		MapMarkerManager.OnIconChanged = (Action<int>)Delegate.Combine(MapMarkerManager.OnIconChanged, new Action<int>(this.OnIconChanged));
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x00037564 File Offset: 0x00035764
	private void OnDisable()
	{
		MapMarkerManager.OnColorChanged = (Action<Color>)Delegate.Remove(MapMarkerManager.OnColorChanged, new Action<Color>(this.OnColorChanged));
		MapMarkerManager.OnIconChanged = (Action<int>)Delegate.Remove(MapMarkerManager.OnIconChanged, new Action<int>(this.OnIconChanged));
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x000375B1 File Offset: 0x000357B1
	private void OnIconChanged(int obj)
	{
		this.Setup();
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x000375B9 File Offset: 0x000357B9
	private void OnColorChanged(Color color)
	{
		this.Setup();
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x000375C4 File Offset: 0x000357C4
	private void Setup()
	{
		if (MapMarkerManager.Instance == null)
		{
			return;
		}
		this.IconBtnPool.ReleaseAll();
		this.ColorBtnPool.ReleaseAll();
		Color[] array = this.colors;
		for (int i = 0; i < array.Length; i++)
		{
			Color cur = array[i];
			MapMarkerPanelButton mapMarkerPanelButton = this.ColorBtnPool.Get(null);
			mapMarkerPanelButton.Image.color = cur;
			mapMarkerPanelButton.Setup(delegate
			{
				MapMarkerManager.SelectColor(cur);
			}, cur == MapMarkerManager.SelectedColor);
		}
		for (int j = 0; j < this.Icons.Count; j++)
		{
			Sprite sprite = this.Icons[j];
			if (!(sprite == null))
			{
				MapMarkerPanelButton mapMarkerPanelButton2 = this.IconBtnPool.Get(null);
				Image image = mapMarkerPanelButton2.Image;
				image.sprite = sprite;
				image.color = MapMarkerManager.SelectedColor;
				int index = j;
				mapMarkerPanelButton2.Setup(delegate
				{
					MapMarkerManager.SelectIcon(index);
				}, index == MapMarkerManager.SelectedIconIndex);
			}
		}
	}

	// Token: 0x04000B64 RID: 2916
	[SerializeField]
	private Color[] colors;

	// Token: 0x04000B65 RID: 2917
	[SerializeField]
	private MapMarkerPanelButton iconBtnTemplate;

	// Token: 0x04000B66 RID: 2918
	[SerializeField]
	private MapMarkerPanelButton colorBtnTemplate;

	// Token: 0x04000B67 RID: 2919
	private PrefabPool<MapMarkerPanelButton> _iconBtnPool;

	// Token: 0x04000B68 RID: 2920
	private PrefabPool<MapMarkerPanelButton> _colorBtnPool;
}
