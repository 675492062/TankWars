using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public float rotateSpeedMultiplier = 1;
	float speed = 0;

	public float angle = 0;

	bool isMouseDown = false;

	Vector2 mouseDownPos = new Vector2();


	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			isMouseDown = true;
			mouseDownPos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			isMouseDown = false;
		}

		if (isMouseDown) {
			float deltaX = Input.mousePosition.x - mouseDownPos.x;
			speed = deltaX;
			mouseDownPos = Input.mousePosition;
		} else {
			speed = 0;
		}


		angle += Time.deltaTime * speed * rotateSpeedMultiplier;

		transform.localRotation = Quaternion.AngleAxis (angle, new Vector3 (0, 1, 0));
		//transform.rotation = new Quaternion(transform.rotation.x, y, transform.rotation.z, transform.rotation.w);


	}
}
