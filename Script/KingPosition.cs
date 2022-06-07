using UnityEngine;
using System.Collections;

public class KingPosition : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void GameOverCheck()
    {
        int JiangNum = 0, ShuaiNum = 0;
        for(int i=0;i<10;i++)
            for(int j = 0; j < 9; j++)
            {
                if (board.chess[i, j] == 1)
                    JiangNum++;
                if (board.chess[i, j] == 8)
                    ShuaiNum++;
            }
        if (JiangNum == 0&&ShuaiNum!=0)
        {
            blackclick.str = "红色棋子胜利";
            blackclick.TrueOrFalse = false;
            return;
        }else if(JiangNum != 0 && ShuaiNum == 0)
        {
            blackclick.str = "黑色棋子胜利";
            blackclick.TrueOrFalse = false;
            return;
        }else if (JiangNum == 0 && ShuaiNum == 0)
        {
            blackclick.str = "平局";
            blackclick.TrueOrFalse = false;
            return;
        }

    }
    

}
