using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public static RoomManager Instance;
    public bool launchHunter = false;
    public bool launchFromGame = false;
    

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 2 && launchHunter)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "HunterManager"), new Vector3(-7,2,-8), Quaternion.identity);
            Cursor.visible = false;
        }
        else if(scene.buildIndex == 2)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), new Vector3(-7,2,-8), Quaternion.identity);
            Cursor.visible = false;
        }
        else if(scene.buildIndex == 0)
        {
            MenuManager.Instance.OpenMenu("loading");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
         
            
        }
        
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
