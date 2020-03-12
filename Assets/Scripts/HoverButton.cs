using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour {

    private Image fillImage;
    private float fillSpeed = 1f;
    private float decaySpeed = 100f;

    public bool fillNextUpdate = false;

    // Start is called before the first frame update
    void Start() {
        this.fillImage = this.transform.Find("FillImage").GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update() {

        if (this.fillNextUpdate) {

            this.fillImage.fillAmount += Time.deltaTime * fillSpeed;

            if(fillImage.fillAmount >= 1) {
                //Debug.Log("filled!!!!");
            }

            this.fillNextUpdate = false;
        } else {

            this.fillImage.fillAmount -= Time.deltaTime * decaySpeed;
        }
    }
}
