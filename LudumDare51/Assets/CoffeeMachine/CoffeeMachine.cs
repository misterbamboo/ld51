using UnityEngine;

public class CoffeeMachine : MonoBehaviour, IPickable
{
    [SerializeField] GameObject coffeePrefab;
    [SerializeField] GameObject[] itemsToShowWhenPlayerNearby;
    public bool isPlayerNear = false;

    private void Start()
    {
        UpdateItemToShow();
    }

    private void OnTriggerEnter(Collider other)
    {
        SetPlayerNear(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        SetPlayerNear(other, false);
    }

    private void SetPlayerNear(Collider other, bool state)
    {
        if (other.tag == Player.Tag)
        {
            isPlayerNear = state;
            UpdateItemToShow();
        }
    }

    private void UpdateItemToShow()
    {
        if (itemsToShowWhenPlayerNearby == null) return;

        for (int i = 0; i < itemsToShowWhenPlayerNearby.Length; i++)
        {
            itemsToShowWhenPlayerNearby[i].SetActive(isPlayerNear);
        }
    }

    public GameObject PickObject()
    {
        return Instantiate(coffeePrefab);
    }
}
