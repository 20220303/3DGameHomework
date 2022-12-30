using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PerfabsController;

class Judge:MonoBehaviour
{
    CoastController rightCoast;
    CoastController leftCoast;
    BoatController boat;
    public Judge(CoastController _rightCoast, CoastController _leftCoast, BoatController _boat)
    {
        rightCoast = _rightCoast;
        leftCoast = _leftCoast;
        boat = _boat;
    }

    public int endGame()
    {
        //两边的priest和devil的数量
        int right_priest = rightCoast.getPVNum()[0];
        int right_devil = rightCoast.getPVNum()[1];

        int left_priest = leftCoast.getPVNum()[0];
        int left_devil = leftCoast.getPVNum()[1];

        //胜利的唯一情况
        if (left_devil + left_priest == 6) return 2;


        if (boat.get_which_coast() == 1)
        {
            //在right岸
            right_priest += boat.getPVNum()[0];
            right_devil += boat.getPVNum()[1];

        }
        else
        {
            //在left岸
            left_priest += boat.getPVNum()[0];
            left_devil += boat.getPVNum()[1];
        }

        //输掉的情况
        if (right_priest < right_devil && right_priest > 0) return 1;
        if (left_priest < left_devil && left_priest > 0) return 1;


        //继续游戏
        return 0;
    }
 }

