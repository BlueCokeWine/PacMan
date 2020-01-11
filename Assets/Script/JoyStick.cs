#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
	// 공개
	[SerializeField] Transform stick;         // 조이스틱.
	[SerializeField] Transform up;
	[SerializeField] Transform down;
	[SerializeField] Transform right;
	[SerializeField] Transform left;

	// 비공개
	Vector3 joyStickOriginPosition;  // 조이스틱의 처음 위치.
	Vector3 joyStickDirection;         // 조이스틱의 벡터(방향)
	float radius;           // 조이스틱 배경의 반 지름.

	void Awake()
	{
		radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
		joyStickOriginPosition = stick.transform.position;

		// 캔버스 크기에대한 반지름 조절.
		float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
		radius *= Can;
	}

	// 드래그
	public void Drag(BaseEventData baseData)
	{
		PointerEventData pointerData = baseData as PointerEventData;
		Vector3 pointerPosition = pointerData.position;

		// 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
		joyStickDirection = (pointerPosition - joyStickOriginPosition).normalized;

		if(Mathf.Abs(joyStickDirection.x) > Mathf.Abs(joyStickDirection.y))
		{
			if(joyStickDirection.x > 0f)
			{
				stick.position = right.position;
				StageManager.Instance.MovePlayer(EDirX.Right, EDirY.None);
			}
			else
			{
				stick.position = left.position;
				StageManager.Instance.MovePlayer(EDirX.Left, EDirY.None);
			}
		}
		else if(Mathf.Abs(joyStickDirection.x) < Mathf.Abs(joyStickDirection.y))
		{
			if (joyStickDirection.y > 0f)
			{
				stick.position = up.position;
				StageManager.Instance.MovePlayer(EDirX.None, EDirY.Up);
			}
			else
			{
				stick.position = down.position;
				StageManager.Instance.MovePlayer(EDirX.None, EDirY.Down);
			}
		}

	}

	// 드래그 끝.
	public void DragEnd()
	{
		stick.localPosition = Vector3.zero; // 스틱을 원래의 위치로.
		joyStickDirection = Vector3.zero;          // 방향을 0으로.
	}

}
