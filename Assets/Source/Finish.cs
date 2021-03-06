﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour {
    
    [SerializeField] private string sceneName = string.Empty;

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            SceneManager.LoadScene (sceneName);
        }
    }

    public void Replay () {
        SceneManager.LoadScene ("Game");
    }

}