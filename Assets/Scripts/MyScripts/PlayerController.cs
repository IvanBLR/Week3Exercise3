using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform _pointForHolding;

    private InteractableItem _lastStuff;
    private InteractableItem _lastItem;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var door = SelectedObject.GetSelectedObject<Door>();
            if (door != null)
                door.SwitchDoorState();

            PickUpTheStuff();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            PushTheStuff(_lastStuff);

        SetAndRemoveLightning();
    }

    private void PickUpTheStuff()
    {
        var interactableStuff = SelectedObject.GetSelectedObject<InteractableItem>();

        if (_lastStuff != null)
        {
            Drop(_lastStuff);
        }

        if (interactableStuff != null)
        {

            interactableStuff.GetComponent<Rigidbody>().isKinematic = true;

            interactableStuff.transform.SetParent(_pointForHolding);

            interactableStuff.transform.localPosition = new Vector3(0, 0, 2);
            interactableStuff.transform.localRotation = Quaternion.identity;


        }
        _lastStuff = interactableStuff;
    }
    private void PushTheStuff(InteractableItem stuff)
    {
        if (stuff != null)
        {
            var rb = stuff.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            stuff.transform.SetParent(null);

            rb.AddForce(transform.forward * 1000);
        }
    }
    private void Drop(InteractableItem stuff)
    {
        stuff.GetComponent<Rigidbody>().isKinematic = false;
        stuff.transform.SetParent(null);
    }
    private void SetAndRemoveLightning()
    {
        var interactableObject = SelectedObject.GetSelectedObject<InteractableItem>();
        if (interactableObject != _lastItem)
        {
            if (_lastItem != null)
                _lastItem.RemoveFocus();
        }
        if (interactableObject != null)
            interactableObject.SetFocus();

        _lastItem = interactableObject;
    }
}

public static class SelectedObject
{
    public static T GetSelectedObject<T>() where T : MonoBehaviour
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            return hitInfo.collider.gameObject.GetComponent<T>();
        return null;
    }
}