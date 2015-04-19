using UnityEngine;
using System.Collections;

public class FlutePlayer : MonoBehaviour
{
    public GameObject flute;
    public GameObject playingFlutePosition;
    public GameObject[] pipePositions;

    private Vector3 initialFlutePosition;
    private Vector3 initialFluteRotation;

    public KeyCode[] keycodes;
    public AudioClip[] clips;

    public AudioSource panfluteSource;

    public Material highlightedMaterial;
    private Material preHighlightMaterial;
    
    // Use this for initialization
    void Start()
    {
        initialFlutePosition = flute.transform.localPosition;
        initialFluteRotation = flute.transform.localEulerAngles;
    }

    private LTDescr fluteDescr;

    private GameObject selectedObject;
    //private GameObject pHolder;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (fluteDescr != null)
            {
                LeanTween.cancel(flute, fluteDescr.uniqueId);
            }

            LeanTween.moveLocal(flute, playingFlutePosition.transform.localPosition, 0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateLocal(flute, playingFlutePosition.transform.localEulerAngles, 0.5f).setEase(LeanTweenType.easeInOutQuad);
        }

        if (Input.GetMouseButtonUp(0))
        {
            panfluteSource.Stop();

            if (fluteDescr != null)
            {
                LeanTween.cancel(flute, fluteDescr.uniqueId);
            }

            LeanTween.moveLocal(flute, initialFlutePosition, 0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateLocal(flute, initialFluteRotation, 0.5f).setEase(LeanTweenType.easeInOutQuad);
        }

        if (Input.GetMouseButton(0))
        {
            for (int i = 0; i < keycodes.Length; i++)
            {
                if (Input.GetKeyDown(keycodes[i]))
                {
                    panfluteSource.Stop();
                    panfluteSource.PlayOneShot(clips[i]);
                }

                if (Input.GetKeyUp(keycodes[i]))
                {
                    panfluteSource.Stop();
                }
            }
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                DisposeOfSelectedObjectHighlight();

                selectedObject = hit.transform.gameObject;
                preHighlightMaterial = selectedObject.GetComponent<MeshRenderer>().material;
                highlightedMaterial.color = preHighlightMaterial.color;
                selectedObject.GetComponent<MeshRenderer>().material = new Material(highlightedMaterial);
            }
            else
            {
                DisposeOfSelectedObjectHighlight();
            }
        }
        else
        {
            DisposeOfSelectedObjectHighlight();
        }
    }

    private void DisposeOfSelectedObjectHighlight()
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material = preHighlightMaterial;
            preHighlightMaterial = null;
            selectedObject = null;
        }
    }
}