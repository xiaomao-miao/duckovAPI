using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E7 RID: 487
public class Soda_Joysticks : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000E5E RID: 3678 RVA: 0x00039A8C File Offset: 0x00037C8C
	public bool Holding
	{
		get
		{
			return this.holding;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00039A94 File Offset: 0x00037C94
	public Vector2 InputValue
	{
		get
		{
			return this.inputValue;
		}
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x00039A9C File Offset: 0x00037C9C
	private void Start()
	{
		this.joyImage.gameObject.SetActive(false);
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 0f;
		}
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x00039AEF File Offset: 0x00037CEF
	private void Update()
	{
		if (this.holding && !this.usable)
		{
			this.Revert();
		}
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x00039B07 File Offset: 0x00037D07
	private void OnEnable()
	{
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
		this.triggeringCancle = false;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00039B30 File Offset: 0x00037D30
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.usable)
		{
			return;
		}
		if (this.holding)
		{
			return;
		}
		this.holding = true;
		this.currentPointerID = eventData.pointerId;
		this.downPoint = eventData.position;
		this.verticalRes = Screen.height;
		this.joystickRangePixel = (float)this.verticalRes * this.joystickRangePercent;
		this.cancleRangePixel = (float)this.verticalRes * this.cancleRangePercent;
		if (!this.fixedPositon)
		{
			this.backGround.transform.position = this.downPoint;
		}
		this.joyImage.transform.position = this.backGround.transform.position;
		this.backGround.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.joyImage.gameObject.SetActive(true);
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, true);
		}
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 1f;
		}
		if (this.canCancle && this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0.12f;
		}
		this.triggeringCancle = false;
		UnityEvent onTouchEvent = this.OnTouchEvent;
		if (onTouchEvent == null)
		{
			return;
		}
		onTouchEvent.Invoke();
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x00039C7C File Offset: 0x00037E7C
	public void OnPointerUp(PointerEventData eventData)
	{
		if (!this.usable)
		{
			return;
		}
		UnityEvent<bool> onUpEvent = this.OnUpEvent;
		if (onUpEvent != null)
		{
			onUpEvent.Invoke(!this.triggeringCancle);
		}
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, false);
		}
		if (this.holding && this.currentPointerID == eventData.pointerId)
		{
			this.Revert();
		}
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00039CE0 File Offset: 0x00037EE0
	private void Revert()
	{
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, false);
		}
		if (this.holding)
		{
			UnityEvent<bool> onUpEvent = this.OnUpEvent;
			if (onUpEvent != null)
			{
				onUpEvent.Invoke(false);
			}
		}
		if (!this.usable)
		{
			return;
		}
		this.joyImage.transform.position = this.backGround.transform.position;
		this.inputValue = Vector2.zero;
		this.holding = false;
		this.backGround.transform.rotation = Quaternion.Euler(Vector3.zero);
		if (this.joyImage.gameObject.activeSelf)
		{
			this.joyImage.gameObject.SetActive(false);
		}
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 0f;
		}
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x00039DCB File Offset: 0x00037FCB
	public void CancleTouch()
	{
		this.Revert();
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x00039DD3 File Offset: 0x00037FD3
	public void OnDisable()
	{
		this.Revert();
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x00039DDC File Offset: 0x00037FDC
	public void OnDrag(PointerEventData eventData)
	{
		if (this.holding && eventData.pointerId == this.currentPointerID)
		{
			Vector2 vector = eventData.position;
			if (vector == this.downPoint)
			{
				this.inputValue = Vector2.zero;
				return;
			}
			float num = Vector2.Distance(vector, this.downPoint);
			float d = num;
			Vector2 normalized = (vector - this.downPoint).normalized;
			if (num > this.joystickRangePixel)
			{
				if (this.followFinger)
				{
					this.downPoint += (num - this.joystickRangePixel) * normalized;
				}
				if (!this.fixedPositon && this.followFinger)
				{
					this.backGround.transform.position = this.downPoint;
				}
				d = this.joystickRangePixel;
			}
			vector = this.downPoint + normalized * d;
			Vector2 vector2 = Vector2.zero;
			if (this.joystickRangePixel > 0f)
			{
				vector2 = normalized * d / this.joystickRangePixel;
			}
			this.joyImage.transform.position = this.backGround.transform.position + normalized * d;
			Vector3 vector3 = Vector3.zero;
			vector3.y = -vector2.x;
			vector3.x = vector2.y;
			vector3 *= this.rotValue;
			this.backGround.transform.rotation = Quaternion.Euler(vector3);
			float num2 = vector2.magnitude;
			num2 = Mathf.InverseLerp(this.deadZone, this.fullZone, num2);
			this.inputValue = num2 * normalized;
			UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
			if (updateValueEvent != null)
			{
				updateValueEvent.Invoke(this.inputValue, true);
			}
			if (this.canCancle && this.cancleRangeCanvasGroup)
			{
				if (num >= this.cancleRangePixel)
				{
					this.cancleRangeCanvasGroup.alpha = 1f;
					this.triggeringCancle = true;
					return;
				}
				this.cancleRangeCanvasGroup.alpha = 0.12f;
				this.triggeringCancle = false;
			}
		}
	}

	// Token: 0x04000BD7 RID: 3031
	public bool usable = true;

	// Token: 0x04000BD8 RID: 3032
	private int verticalRes;

	// Token: 0x04000BD9 RID: 3033
	[Range(0f, 0.5f)]
	public float joystickRangePercent = 0.3f;

	// Token: 0x04000BDA RID: 3034
	[Range(0f, 0.5f)]
	public float cancleRangePercent = 0.4f;

	// Token: 0x04000BDB RID: 3035
	public bool fixedPositon = true;

	// Token: 0x04000BDC RID: 3036
	public bool followFinger;

	// Token: 0x04000BDD RID: 3037
	public bool canCancle;

	// Token: 0x04000BDE RID: 3038
	private float joystickRangePixel;

	// Token: 0x04000BDF RID: 3039
	private float cancleRangePixel;

	// Token: 0x04000BE0 RID: 3040
	[SerializeField]
	private Transform backGround;

	// Token: 0x04000BE1 RID: 3041
	[SerializeField]
	private Image joyImage;

	// Token: 0x04000BE2 RID: 3042
	[SerializeField]
	private CanvasGroup cancleRangeCanvasGroup;

	// Token: 0x04000BE3 RID: 3043
	private bool holding;

	// Token: 0x04000BE4 RID: 3044
	private Vector2 downPoint;

	// Token: 0x04000BE5 RID: 3045
	private int currentPointerID;

	// Token: 0x04000BE6 RID: 3046
	private Vector2 inputValue;

	// Token: 0x04000BE7 RID: 3047
	[SerializeField]
	private float rotValue = 10f;

	// Token: 0x04000BE8 RID: 3048
	[Range(0f, 1f)]
	public float deadZone;

	// Token: 0x04000BE9 RID: 3049
	[Range(0f, 1f)]
	public float fullZone = 1f;

	// Token: 0x04000BEA RID: 3050
	public bool hideWhenNotTouch;

	// Token: 0x04000BEB RID: 3051
	public CanvasGroup canvasGroup;

	// Token: 0x04000BEC RID: 3052
	private bool triggeringCancle;

	// Token: 0x04000BED RID: 3053
	public UnityEvent<Vector2, bool> UpdateValueEvent;

	// Token: 0x04000BEE RID: 3054
	public UnityEvent OnTouchEvent;

	// Token: 0x04000BEF RID: 3055
	public UnityEvent<bool> OnUpEvent;
}
