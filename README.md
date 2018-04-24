# 改进飞碟（Hit UFO）游戏：
+ 游戏内容要求：

    1.按 adapter模式 设计图修改飞碟游戏
  
    2.使它同时支持物理运动与运动学（变换）运动
  
+ 改进方法：

    + 留下了上一个项目的所有资源，只在源代码的基础上完善了少量bug以及增加了一个用来确定运行运动学还是物理运动的参数和转换按钮，可以在游戏中进行切换。因此整个项目无需重新设计
    + 在预制体Disk上添加了一个Component:Rigidbody，让飞碟成为一个刚体，以便在物理运动中推动它。
    + 添加了一个触发器，当飞碟碰到触发器时消失。

+ UML类图
![UML](https://github.com/SO4P/Unity5/blob/master/5.1.png)
+ 代码：
    + 物理运动
  
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
                public override void Start ()
                {
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
                        else if (sceneController.disk.findDisk(this.transform.name) == null)
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
    + 物理运动管理

            public class physicsManager : SSActionManager, ISSActionCallback
            {
                public myGameObject sceneController;
                public physics fly;
                private float timer = 5;
                // Use this for initialization
                void Start () {
                    sceneController = (myGameObject)SSDirector.getInstance().currentSceneController;
                    sceneController.actionManager = this;
                }

                // Update is called once per frame
                protected new void Update()
                {
                    if (sceneController.chose)
                    {
                        if (sceneController.count < 10)
                        {
                            if (timer != 0)
                            {
                                timer -= Time.deltaTime;
                                if (timer < 0)
                                {
                                    timer = Random.Range(3 - sceneController.level, 6 - sceneController.level);
                                    if (sceneController.count < 5)
                                    {
                                        fly = physics.GetSSAction();
                                        sceneController.number++;
                                        this.RunAction(sceneController.disk.GetDisk().Disk, fly, this);
                                    }
                                    else
                                    {
                                        int number = Random.Range(1, 3);
                                        for (; number > 0; number--)
                                        {
                                            fly = physics.GetSSAction();
                                            sceneController.number++;
                                            this.RunAction(sceneController.disk.GetDisk().Disk, fly, this);
                                        }
                                    }
                                }
                            }
                        
                            if (Input.GetMouseButtonDown(0))
                            {
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                RaycastHit hit;
                                if (Physics.Raycast(ray, out hit))
                                {
                                    if (hit.transform.tag == "Disk")
                                    {
                                        sceneController.score(hit.transform.name);
                                        sceneController.disk.destroy(hit.transform.name);
                                    }
                                }
                            }
                            base.Update();
                        }
                    }
                }

                public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
                int intParam = 0, string strParam = null, Object objectParam = null)
                {
                    //  
                }
            }
    + 触发器脚本

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
+ [演示视频](https://github.com/SO4P/Unity5/blob/master/%E6%BC%94%E7%A4%BA%E8%A7%86%E9%A2%91.zip)
