using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPlayer : MonoBehaviour {

    [SerializeField]
    private Camera mainCamera;

    private void Update() {

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = this.mainCamera.WorldToScreenPoint(this.transform.position);

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if(raycastResults.Count != 0) {

            if (raycastResults[0].gameObject.tag.Equals("GuiButton")) {
                raycastResults[0].gameObject.transform.parent.GetComponent<HoverButton>().fillNextUpdate = true;
            }
        }
    }

}