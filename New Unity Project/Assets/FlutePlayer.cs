using UnityEngine;
using System.Collections;

public class FlutePlayer : MonoBehaviour
{
    public GameObject flute;
    public GameObject playingFlutePosition;
    public GameObject[] pipePositions;

    private Vector3 initialFlutePosition;
    private Vector3 initialFluteRotation;
    

    // Use this for initialization
    void Start()
    {
        initialFlutePosition = flute.transform.localPosition;
        initialFluteRotation = flute.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeanTween.moveLocal(flute, playingFlutePosition.transform.localPosition, 0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateLocal(flute, playingFlutePosition.transform.localEulerAngles, 0.5f).setEase(LeanTweenType.easeInOutQuad);
        }

        if (Input.GetMouseButtonUp(0))
        {
            LeanTween.moveLocal(flute, initialFlutePosition, 0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateLocal(flute, initialFluteRotation, 0.5f).setEase(LeanTweenType.easeInOutQuad);
        }
    }
}