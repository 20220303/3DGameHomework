using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using interfaceApplication;


public class SSAction : ScriptableObject
{
    public bool enable = true;//是否存在
    public bool destory = false;//是否删除

    public GameObject gameobject;//动作对象
    public Transform transform;//动作对象的transformer
    public ISSActionCallback callback;//回调函数


    protected SSAction() { }

    public virtual void Start()
    {
        //是指在无法实现请求的方法或操作时引发的异常。
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }



}



//子类：移动到指定
public class SSMoveToAction : SSAction
{

    public Vector3 target; //desitination
    public float speed; 

    private SSMoveToAction() { }

    public static SSMoveToAction GetSSAction(Vector3 _target, float _speed)
    {
        //创建一个方法 方法中传入一个类的名称 然后就能返回一个这个类的实例
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
        action.target = _target;
        action.speed = _speed;
        return action;
    }

    public override void Start()
    {
    }

    public override void Update()
    {
        //这里只是把该perfas从当前位置移动到目标位置
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        //完成
        if (this.transform.position == target)
        {
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }

}



//动作组合
public class SequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence; //动作列表
    public int repeat = -1; //-1就是无限循环做组合中的动作
    public int start = 0; //当前做的动作的索引

    public static SequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence)
    {
        
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.sequence = sequence;
        action.repeat = repeat;
        action.start = start;
        return action;
    }

    public override void Start()
    {
        //执行每个动作
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this; //每一个动作的回调函数为该动作组合
            action.Start();
        }
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update(); //start在回调函数中递加
        }
    }

    //实现接口
    public void SSActionEvent(
        SSAction source, SSActionEventType events = SSActionEventType.competeted,
        int intParam = 0, string strParam = null, Object objectParam = null
    )
    {
        source.destory = false; 
        this.start++; //下一个动作
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            { //动作组合结束
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
        }
    }
}





//动作管理基类
public class SSActionManager : MonoBehaviour, ISSActionCallback {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>(); //动作字典
    private List<SSAction> waitingAdd = new List<SSAction>(); //等待执行
    private List<int> waitingDelete = new List<int>(); //等待删除             

    protected void Update() {
        foreach (SSAction ac in waitingAdd) {
            actions[ac.GetInstanceID()] = ac;                                       
        }
        waitingAdd.Clear();

        //对于字典中每一个pair，看是执行还是删除
        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction ac = kv.Value;
            if (ac.destory) {
                waitingDelete.Add(ac.GetInstanceID());
            }else if (ac.enable) {
                ac.Update();
            }
        }

        //删除所有已完成的动作并清空待删除列表
        foreach (int key in waitingDelete) {
            SSAction ac = actions[key];
            actions.Remove(key);
            Object.Destroy(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager) {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void SSActionEvent(
        SSAction source, SSActionEventType events = SSActionEventType.competeted,
        int intParam = 0, string strParam = null, Object objectParam = null) {

    }
}

//动作管理类
public class ActionManager : SSActionManager {
    public GameController sceneController;
    private SequenceAction boatMove;
    private SequenceAction roleMove;

    protected void Start() {
        sceneController = (GameController)Director.getInstance().currentSceneController;
        sceneController.actionManager = this;
    }

    protected new void Update() {
        base.Update();
    }

    //移动船的动作
    public void moveBoat(GameObject boat, Vector3 endPos, float speed) {
        SSAction action1 = SSMoveToAction.GetSSAction(endPos, speed);
        boatMove = SequenceAction.GetSSAcition(0, 0, new List<SSAction> { action1 });
        this.RunAction(boat, boatMove, this);
    }


    //移动角色
    public void moveRole(GameObject role, Vector3 middlePos, Vector3 endPos, float speed) {
        //两段移动
        SSAction action1 = SSMoveToAction.GetSSAction(middlePos, speed);
        SSAction action2 = SSMoveToAction.GetSSAction(endPos, speed);
        //两个动作
        roleMove = SequenceAction.GetSSAcition(1, 0, new List<SSAction> { action1, action2 });
        this.RunAction(role, roleMove, this);
    }






}
