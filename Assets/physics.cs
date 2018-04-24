using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics : SSAction
{
    public myGameObject sceneController;
    public bool start = true;
    private bool move = true;
    Vector3 force;
    Vector3 begin;//0,1,-10

    public static physics GetSSAction()
    {
        physics action = ScriptableObject.CreateInstance<physics>();
        return action;
    }
    // Use this for initialization
    public override void Start () {
        sceneController = (myGameObject)SSDirector.getInstance().currentSceneController;
        int level = (int)sceneController.disk.findDisk(this.transform.name).speed / 5;
        force = new Vector3(1 * Random.Range(-1, 1) * level, 1 * Random.Range(0.5f, 2) * level, sceneController.disk.findDisk(this.transform.name).speed);
        begin = new Vector3(3 * Random.Range(-2, 2), -4, -10);
        this.transform.position = begin;
        sceneController.disk.findDisk(this.transform.name).stop = false;
    }
	
	// Update is called once per frame
	public override void Update () {
        if (move)
        {
            if(this.gameobject == null)
            {
                start = false;
                move = false;
            }
            else if(sceneController.disk.findDisk(this.transform.name) == null)
            {
                start = false;
                move = false;
            }
            else if (sceneController.disk.findDisk(this.transform.name).stop)
            {
                start = false;
                move = false;
            }
            else if (start)
            {
                gameobject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameobject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                start = false;
            }
        }
        else
        {
            if (this.gameobject != null)
                sceneController.disk.destroy(this.transform.name);
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
	}
    
}
