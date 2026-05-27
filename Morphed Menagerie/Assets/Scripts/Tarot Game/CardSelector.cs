using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public LayerMask layerSelect;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layerSelect))
            {
                SetCard setCard = hit.collider.gameObject.GetComponent<SetCard>();

                if (setCard != null)
                {
                    setCard.ShowInfo();
                }
            }
        }
    }
}
