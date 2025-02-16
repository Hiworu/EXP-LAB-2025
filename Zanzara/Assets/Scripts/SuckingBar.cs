using UnityEngine;
using UnityEngine.UI;

public class SuckingBar : MonoBehaviour
{
    public Slider suckingBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetMaxSucking(float sucking)
    {
        suckingBar.maxValue = sucking;
        suckingBar.value = sucking;
    }

    public void SetSucking(float sucking)
    {
        suckingBar.value = sucking;
    }
}
