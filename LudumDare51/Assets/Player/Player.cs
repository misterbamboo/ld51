﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const string Tag = "Player";
    private List<IPickable> pickables = new List<IPickable>();

    [SerializeField] Transform pickedLocation;
    [SerializeField] GameObject pickedItem;

    private void Update()
    {
        CheckActionTrigger();
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

    private void OnTriggerEnter(Collider other)
    {
        var pickable = other.GetComponent<IPickable>();
        pickables.Add(pickable);
    }

    private void OnTriggerExit(Collider other)
    {
        var pickable = other.GetComponent<IPickable>();
        if (pickables.Contains(pickable))
        {
            pickables.Remove(pickable);
        }
    }
}
