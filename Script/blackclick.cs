using UnityEngine;
using System.Collections;
using System.Threading;
public class blackclick : MonoBehaviour {


    public UICamera uicam;
    public gamemanager mgamemanager;


    public float Probability;
    public static int FromX = -1, ToX = -1, ToY = -1, FromY = -1;
    public static GameObject ObjBlack = null, ObjRed = null;//红色对象，和黑色对象
    public UILabel Lab;
    public static bool bdfdd = true;//测试
    public static string str = "红方走";
    public static bool ChessMove = true;//true   redMove   false BlackMove
    public static bool TrueOrFalse = true;//判断这个时候输赢状态能否走棋 
    public static string RedName = null, BlackName = null, ItemName;//blackchessname  and   redchessname
    public int bestmove;
    public UIToggle tog;
    public Blackmove.CHESSMOVE chere;

    public static bool posthread = true;//判断线程里面的内容是否执行完毕
    Canmovetishi can = new Canmovetishi();
    // Use this for initialization
    //控制窗口，不让窗口乱动

    public UISprite muiimg;

    private void Awake()
    {

        Probability = 1;

    }


    void Start()
    {

    

        muiimg = GetComponent<UISprite>();

        mgamemanager = GameObject.Find("gamemanager").GetComponent<gamemanager>();


        GameObject obj = GameObject.Find("tishi");
        Lab = obj.GetComponent<UILabel>();

       uicam =GameObject.Find("Camera").GetComponent<UICamera>();
    }


    //计算概率
    public void calculate_qizi_pro()
    {

     Color c  =  muiimg.color;
        c.a = Probability;
        muiimg.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        Lab.text = str;
        calculate_qizi_pro();
    }
    //得到点击的象棋名字
    //判断点击到的是什么？
    //0是空   1 是黑色   2 是红色
    public int IsBlackOrRed(int x, int y)
    {
        int Count = board.chess[y, x];
        if (Count == 0)
            return 0;
        else if (Count > 0 && Count < 8)//是黑色
            return 1;
        else  //是红色 
            return 2;
    }
    public void BlackNameOrRedName(GameObject obj)
    {//得到棋子的名字
        if (obj.name.Substring(0, 1) == "r")
            RedName = obj.name;//得到red名字
   
        else if (obj.name.Substring(0, 1) == "b")
            BlackName = obj.name;//得到black名字
        else
            ItemName = obj.name;//得到item名字
    }



    //移动类
    public void IsMove(string One, GameObject game, int x1, int y1, int x2, int y2)
    {
        GameObject parent1 = GameObject.Find(One);
        Debug.LogError("parent1移动:" + parent1.name);
        Debug.LogError("game移动:" + game.name);


        switch (uicam.mouse_click_left_is_right)
        {
            case UICamera.mouseleft_right.left:

                //正常走
                parent1.transform.parent = game.transform;

                parent1.transform.localPosition = Vector3.zero;

                board.chess[y2, x2] = board.chess[y1, x1];

                board.chess[y1, x1] = 0;

                break;
            case UICamera.mouseleft_right.right:
                //分裂

                if (mgamemanager.First != null)
                {
                    float p = mgamemanager.First.GetComponent<blackclick>().Probability / 2;
                    can.ClickChess_splite(x1, y1, game, y2, y1, x2, x1, p);
                    mgamemanager.First.GetComponent<blackclick>().Probability = p;
                    mgamemanager.First = null;
                }
          

                break;

        }


    }


    //吃子类
    public void IsEat(string Frist, string sconde, int x1, int y1, int x2, int y2)
    {

        //一号吃棋的棋子
        GameObject Onename = GameObject.Find(Frist);//得到第一个


        //获得概率
      float Probability = Onename.GetComponent<blackclick>().Probability;

        if (Probability == 1)
        {
            //成功吃掉
            Debug.LogError("经典吃子");
            Debug.LogError("第一个 =" + Onename.name);

            //二号被吃的棋子
            GameObject Twoname = GameObject.Find(sconde);//得到第二个名字

            Debug.LogError("第二个 =" + Twoname.name);


            GameObject Twofather = Twoname.gameObject.transform.parent.gameObject;//得到第二个的父亲

            Onename.gameObject.transform.parent = Twofather.transform;

            Onename.transform.localPosition = Vector3.zero;

            Destroy(Twoname);


            board.chess[y2, x2] = board.chess[y1, x1];

            board.chess[y1, x1] = 0;
        }
        else
        {

            float measure = UnityEngine.Random.Range(0, 2);

            if (measure < Probability)
            {

                //成功吃掉

                Debug.LogError("第一个 =" + Onename.name);

                //二号被吃的棋子
                GameObject Twoname = GameObject.Find(sconde);//得到第二个名字

                Debug.LogError("第二个 =" + Twoname.name);


                GameObject Twofather = Twoname.gameObject.transform.parent.gameObject;//得到第二个的父亲

                Onename.gameObject.transform.parent = Twofather.transform;

                Onename.transform.localPosition = Vector3.zero;

                Destroy(Twoname);


                board.chess[y2, x2] = board.chess[y1, x1];

                board.chess[y1, x1] = 0;

            }
            else
            {
                //没吃掉,自己消失

                Destroy(Onename);
                board.chess[y1, x1] = 0;
            }
        }
    }



    //汇合销毁方法
    public void Merge(int x1,int y1)
    {


        board.chess[y1, x1] = 0;


        Probability = mgamemanager.First.GetComponent<blackclick>().Probability + Probability;

        Destroy(mgamemanager.First);
    }




    Blackmove mo = new Blackmove();
    public void IsClickCheck()
    {

        if (TrueOrFalse == false)
            return;




        GameObject obj = UICamera.hoveredObject;
        BlackNameOrRedName(obj);//是否点击到棋子  如果是  就得到棋子


        Debug.Log("Red nmae=" + RedName);


        if (obj.name.Substring(0, 1) != "i")
            obj = obj.gameObject.transform.parent.gameObject;//得到他的父容器

        Debug.Log(" obj.name=" + obj.name);


        int x = System.Convert.ToInt32((obj.transform.localPosition.x) / 130);
        int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y) / 128));



        int Result = IsBlackOrRed(x, y);//判断点击到了什么
        switch (Result)
        {
            case 0:
              

                Debug.Log("点击空格");

                //点击到了空  是否要走棋
                //如果点击到了空格  就把对象清空
                for (int i = 1; i <= 90; i++)
                {
                    GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                    Destroy(Clear);
                }
                if (posthread == false)
                    return;
                ToY = y;
                ToX = x;
                if (ChessMove)
                {//红色走
                    if (RedName == null)
                        return;
                    string sssRed = RedName;//记录红色棋子的名字
                    bool ba = rules.IsValidMove(board.chess, FromX, FromY, ToX, ToY);
                    if (!ba)
                    {
                        mgamemanager.First = null;
                        return;
                    }
                    int a = board.chess[FromY, FromX];
                    int b = board.chess[ToY, ToX];

                    IsMove(RedName, obj, FromX, FromY, ToX, ToY);//走了

                    str = "黑方走";

                    KingPosition.GameOverCheck();

                    ChessMove = false;

                    //getString();
                    if (str == "红色棋子胜利")
                        return;//因为没有携程关系  每次进入黑色走棋的时候都判断 棋局是否结束
                    if (yemiantiaozhuang.ChessPeople == 2)
                    {//如果现在是双人对战模式
                        BlackName = null;
                        RedName = null;
                        return;
                    }
                   
               

                }
                else
                {//黑色走

                    Debug.Log("点击空格  黑色走");


                    if (BlackName == null)
                        return;
                    bool ba = rules.IsValidMove(board.chess, FromX, FromY, ToX, ToY);
                    if (!ba)
                    {
                        mgamemanager.First = null;
                        return;
                    }

                    int a = board.chess[FromY, FromX];
                    int b = board.chess[ToY, ToX];

                    //看看是否能播放音乐
                    IsMove(BlackName, obj, FromX, FromY, ToX, ToY);

                    //黑色走棋
                    ChessMove = true;
                    str = "红方走";
                    KingPosition.GameOverCheck();
                }
                break;
            case 1://点击到了黑色  是否选中  还是  红色要吃子


                if (uicam.mouse_click_left_is_right == UICamera.mouseleft_right.right)
                {
                    if (mgamemanager.First == null)
                    {
                        Debug.Log("第一次选择棋子");
                        mgamemanager.First = gameObject;
                        mgamemanager.Firstx = x;
                        mgamemanager.Firsty = y;
                      
                    }
                    else
                    {
                        Debug.Log("第二次选择棋子");
                        //汇合
                        if (mgamemanager.First.GetComponent<blackclick>().Probability < 1) {
                        //将第一次的棋子消失
                        Merge(mgamemanager.Firstx, mgamemanager.Firsty);
                        mgamemanager.First = null;
                        }
                        else
                        {
                            mgamemanager.First = null;
                        }
                        
                    }

                }
                if (posthread == false)
                    return;
                if (!ChessMove)
                {
                    Debug.Log("点击黑色可走");
                    FromX = x;
                    FromY = y;
                    //	Canmovetishi can = new Canmovetishi();

                    for (int i = 1; i <= 90; i++)
                    {
                        GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                        Destroy(Clear);
                    }

                    //点击的棋子
                    can.ClickChess(FromX, FromY);
                }
                else
                {
                    Debug.Log("点击黑色不可走");
                    for (int i = 1; i <= 90; i++)
                    {
                        GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                        Destroy(Clear);
                    }
                    if (RedName == null)
                        return;
                    ToX = x;
                    ToY = y;
                    bool ba = rules.IsValidMove(board.chess, FromX, FromY, ToX, ToY);
                    if (!ba) {
                        mgamemanager.First = null;
                       return;
                    }
                        
                    //ChessChongzhi chzh = new ChessChongzhi();
                    int a = board.chess[FromY, FromX];
                    int b = board.chess[ToY, ToX];

                    //看看是否能播放音乐
                    IsEat(RedName, BlackName, FromX, FromY, ToX, ToY);

                    //print(RedName+"  "+BlackName+" "+FromX+" "+FromY+" "+ToX+" "+ToY);
                    ChessMove = false;
                    //getString();
                    //红色吃子  变黑色走
                    str = "黑方走";
                    KingPosition.GameOverCheck();
                    if (str == "红色棋子胜利")
                        return;//因为没有携程关系  每次进入黑色走棋的时候都判断 棋局是否结束
                    if (yemiantiaozhuang.ChessPeople == 2)
                    {
                        RedName = null;
                        BlackName = null;
                        return;
                    }
                    if (ChessMove == false)
                        StartCoroutine("Robot");
                }
                break;
            case 2://点击到了红色   是否选中  还是黑色要吃子


                if (uicam.mouse_click_left_is_right == UICamera.mouseleft_right.right)
                {
                    if (mgamemanager.First == null)
                    {

                        mgamemanager.First = gameObject;
                        mgamemanager.Firstx = x;
                        mgamemanager.Firsty = y;
       
                    }
                    else
                    {

                        if (mgamemanager.First.GetComponent<blackclick>().Probability < 1)
                        {
                            //将第一次的棋子消失
                            Merge(mgamemanager.Firstx, mgamemanager.Firsty);
                            mgamemanager.First = null;
                        }
                        else
                        {
                            mgamemanager.First = null;
                        }
                    }
          
                }




                if (posthread == false)
                    return;
                if (ChessMove)
                {
                    Debug.Log("点击红色 可以走");
                    FromX = x;
                    FromY = y;
                    //Canmovetishi can = new Canmovetishi();
                    for (int i = 1; i <= 90; i++)
                    {
                        GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                        Destroy(Clear);
                    }

                    //点击棋子
                    can.ClickChess(FromX, FromY);
                }
                else
                {
                    Debug.Log("点击红色 不可以走");

                    for (int i = 1; i <= 90; i++)
                    {
                        GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                        Destroy(Clear);
                    }

                    if (BlackName == null)
                        return;
                    ToX = x;
                    ToY = y;
                    bool ba = rules.IsValidMove(board.chess, FromX, FromY, ToX, ToY);
                    if (!ba)
                    {
                        mgamemanager.First = null;
                        return;
                    }
                        

                    int a = board.chess[FromY, FromX];
                    int b = board.chess[ToY, ToX];

                    //看看是否能播放音乐
                    IsEat(BlackName, RedName, FromX, FromY, ToX, ToY);

                    RedName = null;
                    BlackName = null;
                    ChessMove = true;
                    str = "红方走";
                    KingPosition.GameOverCheck();
                }
                break;

        }

    }


}
