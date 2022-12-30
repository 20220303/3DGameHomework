using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using interfaceApplication;


public class SSAction : ScriptableObject
{
    public bool enable = true;//�Ƿ����
    public bool destory = false;//�Ƿ�ɾ��

    public GameObject gameobject;//��������
    public Transform transform;//���������transformer
    public ISSActionCallback callback;//�ص�����


    protected SSAction() { }

    public virtual void Start()
    {
        //��ָ���޷�ʵ������ķ��������ʱ�������쳣��
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }



}



//���ࣺ�ƶ���ָ��
public class SSMoveToAction : SSAction
{

    public Vector3 target; //desitination
    public float speed; 

    private SSMoveToAction() { }

    public static SSMoveToAction GetSSAction(Vector3 _target, float _speed)
    {
        //����һ������ �����д���һ��������� Ȼ����ܷ���һ��������ʵ��
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
        //����ֻ�ǰѸ�perfas�ӵ�ǰλ���ƶ���Ŀ��λ��
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        //���
        if (this.transform.position == target)
        {
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }

}



//�������
public class SequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence; //�����б�
    public int repeat = -1; //-1��������ѭ��������еĶ���
    public int start = 0; //��ǰ���Ķ���������

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
        //ִ��ÿ������
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this; //ÿһ�������Ļص�����Ϊ�ö������
            action.Start();
        }
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update(); //start�ڻص������еݼ�
        }
    }

    //ʵ�ֽӿ�
    public void SSActionEvent(
        SSAction source, SSActionEventType events = SSActionEventType.competeted,
        int intParam = 0, string strParam = null, Object objectParam = null
    )
    {
        source.destory = false; 
        this.start++; //��һ������
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            { //������Ͻ���
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
        }
    }
}





//�����������
public class SSActionManager : MonoBehaviour, ISSActionCallback {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>(); //�����ֵ�
    private List<SSAction> waitingAdd = new List<SSAction>(); //�ȴ�ִ��
    private List<int> waitingDelete = new List<int>(); //�ȴ�ɾ��             

    protected void Update() {
        foreach (SSAction ac in waitingAdd) {
            actions[ac.GetInstanceID()] = ac;                                       
        }
        waitingAdd.Clear();

        //�����ֵ���ÿһ��pair������ִ�л���ɾ��
        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction ac = kv.Value;
            if (ac.destory) {
                waitingDelete.Add(ac.GetInstanceID());
            }else if (ac.enable) {
                ac.Update();
            }
        }

        //ɾ����������ɵĶ�������մ�ɾ���б�
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

//����������
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

    //�ƶ����Ķ���
    public void moveBoat(GameObject boat, Vector3 endPos, float speed) {
        SSAction action1 = SSMoveToAction.GetSSAction(endPos, speed);
        boatMove = SequenceAction.GetSSAcition(0, 0, new List<SSAction> { action1 });
        this.RunAction(boat, boatMove, this);
    }


    //�ƶ���ɫ
    public void moveRole(GameObject role, Vector3 middlePos, Vector3 endPos, float speed) {
        //�����ƶ�
        SSAction action1 = SSMoveToAction.GetSSAction(middlePos, speed);
        SSAction action2 = SSMoveToAction.GetSSAction(endPos, speed);
        //��������
        roleMove = SequenceAction.GetSSAcition(1, 0, new List<SSAction> { action1, action2 });
        this.RunAction(role, roleMove, this);
    }






}
