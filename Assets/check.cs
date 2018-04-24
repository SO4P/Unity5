using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check : MonoBehaviour
{
    public int num;
    public myGameObject sceneController;
    void Start()
    {
        sceneController = (myGameObject)SSDirector.getInstance().currentSceneController;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Disk")
        {
            sceneController.disk.findDisk(other.gameObject.name).stop = true;
        }
    }
}
