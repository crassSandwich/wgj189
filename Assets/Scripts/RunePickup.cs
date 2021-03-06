﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunePickup : MonoBehaviour
{
    public RuneType Type;

    void Start ()
    {
        DeathLoopManager.Instance.OnPlayerDied.AddListener(() => Destroy(gameObject));
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            RuneManager.Instance.CollectRune(Type);
            Destroy(gameObject);
        }
    }
}
