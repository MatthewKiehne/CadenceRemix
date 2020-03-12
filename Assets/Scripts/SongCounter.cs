using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SongCounter : MonoBehaviour
{

    // Eventually add a public slot for a song or something
    private float timestart = 0;
    public Slider slider;

    // Update is called once per frame
    void Update() {
        float progress = 0;

        // Slider values go from 0 to 1 so while progress is less that one continue
        // Should be a while loop here, this is just proof of concept
        if (progress < 1) {
            timestart += Time.deltaTime;
            progress = timestart / 50; //This will need to be changed
            this.slider.value = progress;
        }

        // After while loop there would be a completion screen or something

    }
}
