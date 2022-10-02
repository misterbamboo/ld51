using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const string Tag = "Player";
    private List<IPickable> pickables = new List<IPickable>();
    private IBoss bossNear;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject textGiveCoffe;
    [SerializeField] Transform pickedLocation;
    [SerializeField] GameObject pickedItem;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckActionTrigger();
        PickedItemFollowPosition();
        UpdateAnimator();
    }

    private void CheckActionTrigger()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerAction();
        }
    }

    private void TriggerAction()
    {
        if (HandIsFull())
        {
            DropItem();
        }
        else
        {
            TryPickItem();
        }
    }

    private bool HandIsFull() => pickedItem != null;

    private void DropItem()
    {
        if (IsBossNear())
        {
            GiveItemToBoss();
        }
        else
        {
            DropItemNowere();
        }
    }

    private void GiveItemToBoss()
    {
        bossNear.GiveCoffee(pickedItem);
    }

    private bool IsBossNear() => bossNear != null;

    private void DropItemNowere()
    {
        var pickedItemBody = pickedItem.GetComponent<Rigidbody>();
        if (pickedItemBody != null)
        {
            pickedItemBody.isKinematic = false;
        }

        pickedItem.transform.SetParent(null);
        pickedItem = null;
    }

    private void TryPickItem()
    {
        var pickable = GetCleanFirst();
        if (pickable != null)
        {
            GetPickable(pickable);
        }
    }

    private IPickable GetCleanFirst()
    {
        while (pickables.Count > 0)
        {
            if (pickables[0] == null)
            {
                pickables.RemoveAt(0);
            }
            else
            {
                return pickables[0];
            }
        }

        return null;
    }

    private void GetPickable(IPickable pickable)
    {
        pickedItem = pickable.PickObject();
        pickedItem.transform.SetParent(transform);
        pickedItem.transform.position = pickedLocation.position;

        var pickedItemBody = pickedItem.GetComponent<Rigidbody>();
        if (pickedItemBody != null)
        {
            pickedItemBody.isKinematic = true;
        }
    }

    private void PickedItemFollowPosition()
    {
        if (pickedItem != null)
        {
            pickedItem.transform.position = pickedLocation.position;
        }
    }

    private void UpdateAnimator()
    {
        AnimateHoldItem(pickedItem != null);
        AnimateIsWalking(playerMovement.IsAccelerating);
    }

    private void AnimateIsWalking(bool state)
    {
        animator.SetBool("IsWalking", state);
    }

    private void AnimateHoldItem(bool state)
    {
        animator.SetBool("HoldItem", state);
    }

    private void OnTriggerEnter(Collider other)
    {
        var pickable = other.GetComponent<IPickable>();
        if (pickable != null)
        {
            pickables.Add(pickable);
        }

        var boss = other.GetComponent<IBoss>();
        if (boss != null)
        {
            if (pickedItem != null)
            {
                textGiveCoffe.SetActive(true);
            }

            bossNear = boss;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var pickable = other.GetComponent<IPickable>();
        if (pickables.Contains(pickable))
        {
            pickables.Remove(pickable);
        }

        var boss = other.GetComponent<IBoss>();
        if (boss != null)
        {
            textGiveCoffe.SetActive(false);
            bossNear = null;
        }
    }
}
