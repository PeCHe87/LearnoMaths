using System.Collections;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    public delegate void OnCloseComplete(); // This defines what type of method you're going to call.
    public delegate void OnOpenComplete();

    [SerializeField] private RectTransform _leftDoor, _rightDoor;
    [SerializeField] private float _speedDoorsOpen, _speedDoorsClose;
    [SerializeField] private float _offsetToOpenDoorL, _offsetToOpenDoorR;

    private Vector3 doorOriginL = Vector3.zero;
    private Vector3 doorOriginR = Vector3.zero;
    private bool openningDoors = false;

    private void Awake()
    {
        doorOriginL = _leftDoor.anchoredPosition;
        doorOriginR = _rightDoor.anchoredPosition;
    }

    public void OpenDoors(bool automatically = false, OnOpenComplete callback = null)
    {
        Debug.Log("OpenDoors::Automatically: " + automatically);

        if (automatically)
        {
            _leftDoor.anchoredPosition = doorOriginL - new Vector3(_offsetToOpenDoorL, 0, 0);
            _rightDoor.anchoredPosition = doorOriginR + new Vector3(_offsetToOpenDoorR, 0, 0);
        }
        else
        {
            StartCoroutine(OpenDoorsSmooth(callback));
        }
    }

    private IEnumerator OpenDoorsSmooth(OnOpenComplete callback = null)
    {
        if (openningDoors)
        {
            yield return null;
        }
        else
        {
            openningDoors = true;

            bool areOpened = false;

            Vector3 destL = doorOriginL - new Vector3(_offsetToOpenDoorL, 0, 0);
            Vector3 destR = doorOriginR + new Vector3(_offsetToOpenDoorR, 0, 0);

            while (!areOpened && openningDoors)
            {
                //Move doors
                _leftDoor.anchoredPosition = Vector3.Lerp(_leftDoor.anchoredPosition, destL, Time.deltaTime * _speedDoorsOpen);
                _rightDoor.anchoredPosition = Vector3.Lerp(_rightDoor.anchoredPosition, destR, Time.deltaTime * _speedDoorsOpen);

                //Check if they are near to be opened
                if (Mathf.Abs(_leftDoor.anchoredPosition.x - destL.x) < 0.1f && Mathf.Abs(_rightDoor.anchoredPosition.x - destR.x) < 0.1f)
                    areOpened = true;

                yield return null;
            }

            Debug.Log("OpenDoors::OnComplete");

            if (areOpened)
            {
                if (callback != null)
                    callback();
            }

            openningDoors = false;
        }
    }

    public void CloseDoors(OnCloseComplete callback)
    {
        openningDoors = false;

        StartCoroutine(CloseDoorsSmooth(callback));
    }

    private IEnumerator CloseDoorsSmooth(OnCloseComplete callback)
    {
        Debug.Log("CloseDoors");

        bool areClosed = false;

        while (!areClosed)
        {
            //Move doors
            _leftDoor.anchoredPosition = Vector3.Lerp(_leftDoor.anchoredPosition, doorOriginL, Time.deltaTime * _speedDoorsClose);
            _rightDoor.anchoredPosition = Vector3.Lerp(_rightDoor.anchoredPosition, doorOriginR, Time.deltaTime * _speedDoorsClose);

            //Debug.Log("CloseDoors::Moving doors");

            //check if they are near to be closed
            if (Mathf.Abs(_leftDoor.anchoredPosition.x - doorOriginL.x) < 0.05f && Mathf.Abs(_rightDoor.anchoredPosition.x - doorOriginR.x) < 0.05f)
                areClosed = true;

            yield return null;
        }

        Debug.Log("CloseDoors::OnComplete");

        callback();
    }

}
