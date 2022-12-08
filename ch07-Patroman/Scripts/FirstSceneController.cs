using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController
{
    public PatorlFactory _PatorlFactory;                             //巡逻者工厂
    public ScoreRecorder _ScoreRecorder;                             //记录员
    public PatrolActionManager _PatrolActionManager;                 //运动管理器
    public int wall_sign = -1;                                       //当前玩家所处哪个格子
    public GameObject player;                                        //玩家
    private List<GameObject> patrols;                                //场景中巡逻者列表
    private bool game_over = false;                                  //游戏结束

    void Update()
    {
        for (int i = 0; i < patrols.Count; i++)
        {
            
            patrols[i].gameObject.GetComponent<PatrolData>().wall_sign = wall_sign;
        }
        //20分结束游戏
        if(_ScoreRecorder.score == 20)
        {
            Gameover();
        }
       
    }
    void Start()
    {
        
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        _PatorlFactory = Singleton<PatorlFactory>.Instance;//巡逻者工厂
        _PatrolActionManager = Singleton<PatrolActionManager>.Instance;//巡逻者的动作管理器

        //ISceneController接口中函数的实现
        LoadResources();
       
        _ScoreRecorder = Singleton<ScoreRecorder>.Instance;//拿到计分的单实例对象
    }
    //加载资源
    public void LoadResources()
    {
        player = _PatorlFactory.LoadPlayer();//player

        //导入资源的时候为了巡逻兵有动作
        patrols = _PatorlFactory.LoadPatrol();
        for (int i = 0; i < patrols.Count; i++)
        {
            _PatrolActionManager.GoPatrol(patrols[i]);//让巡逻兵有动作
        }
    }

    public void Attack()
    {
        if (!game_over)
        {
            //Fire1对应鼠标左键
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire1");
                player.GetComponent<Animator>().SetTrigger("yep");
            }
            
        }
    }


    //获得分数
    public int GetScore()
    {
        return _ScoreRecorder.score;
    }
    //判断是不是game_over的函数
    public bool GetGameover()
    {
        return game_over;
    }
    //重新开始，这个用的是重新加载场景
    public void Restart()
    {
        Debug.Log("游戏重新开始！");

        SceneManager.LoadScene("Scenes/SampleScene");

    }

    //发布与订阅模式
    void OnEnable()
    {
       
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameoverChange += Gameover;
    }
    
    void OnDisable()
    {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameoverChange -= Gameover;
    }
    
    void AddScore()
    {
        _ScoreRecorder.AddScore();
    }
    //游戏失败
    void Gameover()
    {
        game_over = true;
        _PatorlFactory.StopPatrol();
        _PatrolActionManager.DestroyAllAction();
    }
}
