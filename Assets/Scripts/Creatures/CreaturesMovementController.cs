﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CreaturesMovementController : MonoBehaviour {

	private bool mouseDown;
	private Vector3 mouseStartPos;
	public GameObject arrow;
	public GameObject xIcon;
	public List<GameObject> toDisable;
	public CreaturesSplitJoinController creatureController;
	public CreaturesPool pool;

	public void ActivateArrowMode() {
		Invoke ("ActivateArrowModeHelper", 0.05f); 
		InvokeRepeating ("ArrowUpdate", 0.1f, Time.unscaledDeltaTime);
	}

	private void ActivateArrowModeHelper() {
		foreach(GameObject go in toDisable)
			go.SetActive (false);
		arrow.SetActive (true);
		xIcon.SetActive (true);
	}

	public void DisableArrowMode() {
		arrow.SetActive (false);
		xIcon.SetActive (false);
		foreach(GameObject go in toDisable)
			go.SetActive (true);
		CancelInvoke ("ArrowUpdate");
	}

	private void UpdateCreaturesTarget(Vector3 direction) {
		foreach(GameObject go in pool.GetActive ()){
			Creature creature = go.GetComponent<Creature> ();
			creature.movement.AddDirectionalVector((Vector2)direction);
		}
	}


	// Update is called once per frame
	void ArrowUpdate () {
		if (!creatureController.ModeIsOn ()) {
			Vector3 currentMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (!mouseDown && Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ()) {
				mouseDown = true;
				arrow.SetActive (true);
				mouseStartPos = currentMousePos;
				arrow.transform.position = mouseStartPos;
			} else if (Input.GetMouseButtonUp (0) && mouseDown) {
				UpdateCreaturesTarget (currentMousePos - mouseStartPos);
				mouseDown = false;
				arrow.SetActive (false);
				arrow.transform.localScale = Vector3.zero;
				arrow.transform.localPosition = Vector3.zero;
				arrow.transform.position = Vector3.zero;
			} else if (mouseDown) {
				Vector3 v3 = currentMousePos - mouseStartPos;
				arrow.transform.position = new Vector3 (mouseStartPos.x + v3.x / 2.0f,
					mouseStartPos.y + v3.y / 2.0f, 0);
				arrow.transform.localScale = new Vector3 (v3.magnitude / 4.0f, v3.magnitude / 2.0f, transform.localScale.z);
				arrow.transform.localRotation = Quaternion.FromToRotation (Vector3.up, v3);
			}
		}
	}


}
