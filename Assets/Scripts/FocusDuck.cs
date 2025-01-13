using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusDuck : MonoBehaviour
{
    Vector3 defaultPos;
    float defaultCamSize;
    Vector3 targetPos;

    [Header("Zoom Settings")]
    [SerializeField] float[] clampX;
    [SerializeField] float[] clampY;
    [SerializeField] float zoomOrthographicSize = 3;

    [Header("CameraSmoothing")]
    Vector3 velocity = Vector3.zero;
    [SerializeField] float smoothTime = 0.25f;

    [Header("References")]
    [SerializeField] Button zoomButton;
    GameObject focusObject;
    Camera mCam;
    Settings settings;
 
    void Start()
    {
        //find references
        mCam = Camera.main;
        settings = FindObjectOfType<Settings>();

        //set variables
        defaultPos = transform.position;
        defaultCamSize = mCam.orthographicSize;
    }

    void Update()
    {
        if (focusObject != null)
        {
            targetPos = new Vector3(
                    Mathf.Clamp(focusObject.transform.position.x, clampX[0], clampX[1]),
                    Mathf.Clamp(focusObject.transform.position.y, clampY[0], clampY[1]),
                    transform.position.z);

            if (Input.touchCount <= 0) { SmoothCamera(); }
        }
    }
    void SmoothCamera()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    public void Focus(GameObject touchObject)
    {
        if (settings.allowZoom)
        {
            focusObject = touchObject;
            mCam.orthographicSize = zoomOrthographicSize;
            ZoomButtonVisible(true);
        }
    }

    public void ExitFocus()
    {
        focusObject = null;
        transform.position = defaultPos;
        mCam.orthographicSize = defaultCamSize;

        ZoomButtonVisible(false);
    }

    void ZoomButtonVisible(bool visibilty)
    {
        zoomButton.gameObject.SetActive(visibilty);
    }
}
