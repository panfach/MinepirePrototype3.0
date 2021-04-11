using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	public static float XAngle, YAngle, Height;

	public float stickMinZoom, stickMaxZoom;
	public float swivelMinZoom, swivelMaxZoom;
	public float moveSpeedMinZoom, moveSpeedMaxZoom;
	public float scrollSpeed, rotationSpeed, smoothSpeed;
	public float panBorderThickness = 10f;
	public CellGrid cellGrid;

	float zoomDelta, rotationDelta, xDelta, zDelta;
	float rotationAngle = 0f;
	float zoom = 1f;
	Vector3 position;
	Transform swivel, stick;

	void Awake()
	{
		swivel = transform.GetChild(0);
		stick = swivel.GetChild(0);
		position = transform.localPosition;
	}

	void Update()
	{
		if (!StateManager.CameraIsFreezed)
		{
			zoomDelta = Input.GetAxis("Mouse ScrollWheel");
			if (zoomDelta != 0f)
			{
				AdjustZoom(zoomDelta);
			}

			//rotationDelta = Input.GetAxis("Rotation");
			if (Input.GetKey(KeyCode.Q)) rotationDelta = -1f;
			else if (Input.GetKeyDown(KeyCode.E)) rotationDelta = 1f;
			else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E)) rotationDelta = 0f;
			if (rotationDelta != 0f)
			{
				AdjustRotation(rotationDelta);
			}

			xDelta = Input.GetAxis("Horizontal");
			zDelta = Input.GetAxis("Vertical");
			if (Input.mousePosition.y >= Screen.height - panBorderThickness) zDelta = 1f;
			if (Input.mousePosition.y <= panBorderThickness) zDelta = -1f;
			if (Input.mousePosition.x >= Screen.width - panBorderThickness) xDelta = 1f;
			if (Input.mousePosition.x <= panBorderThickness) xDelta = -1f;
			if (xDelta != 0f || zDelta != 0f)
			{
				AdjustPosition(xDelta, zDelta);
			}

			transform.localPosition = Vector3.Lerp(transform.localPosition, position, (smoothSpeed * Time.unscaledDeltaTime));           // Передача камере сглаженной координаты
		}
	}



	void AdjustZoom(float delta)
	{
		zoom = Mathf.Clamp01(zoom + delta * scrollSpeed * Time.unscaledDeltaTime);
		Height = 1f - zoom;

		float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
		stick.localPosition = new Vector3(0f, 0f, distance);

		float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
		swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);

		XAngle = angle;
		//BuildingInfo.cameraHeightDifference = true;
	}

	void AdjustRotation(float delta)
    {
		rotationAngle += delta * rotationSpeed * Time.unscaledDeltaTime;
		if (rotationAngle < 0f)
		{
			rotationAngle += 360f;
		}
		else if (rotationAngle >= 360f)
		{
			rotationAngle -= 360f;
		}
		transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);

		YAngle = rotationAngle;
		//BuildingInfo.cameraAngleDifference = true;
	}

	void AdjustPosition(float xDelta, float zDelta)
	{
		Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
		float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
		float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) * damping * Time.unscaledDeltaTime;

		position += direction * distance;                                                                               // Расчет новой координаты
		position = ClampPosition(position);
	}

	Vector3 ClampPosition(Vector3 position)
	{
		position.x = Mathf.Clamp(position.x, CellGrid.xMin, CellGrid.xMax);

		position.z = Mathf.Clamp(position.z, CellGrid.zMin, CellGrid.zMax);

		return position;
	}
}
