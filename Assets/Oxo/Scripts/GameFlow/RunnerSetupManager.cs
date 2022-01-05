using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerSetupManager : MonoBehaviour
{
    public RunnerType type;

    public float levelDuration;

    public float oneSecDistance;

    void Start()
    {

    }

    void Update()
    {

    }

    IEnumerator TranslationSpeedCalculation()
    {
        yield return new WaitForSeconds(1f);

    }

    public enum RunnerType { Classic, Dreamtech }

    public enum FlowType { Type01, Type02 }

    // You should enter the action after 1.5-2.5 second from start (1.2 for so fast game 2.2 for so slow game)

    // Level design need bonus action like boss level that can has short () standard game action 

    // Collectable or action gates


    // 3 type obstacle
    // Type01 is you can escape 
    // Type02 is you sould take damage and slow you (for count master)
    // Type03 interaction with object like button and pick up the obstacle

    // All obstacles if your count so hight you should decrease but if you has a bit count you can escape the obstacles.

    //COUNT MASTER GAMEFLOW

    // all dual collectable has desicion 
    // obstacles can move and collectables can move.


    // they show the new obstacle on the short boss level and use the game flow after that

    // when you see more than 3 consecutive obstacles 3. and 4. one has movement for the prevent stagnation.

    // Bonus Level has bar mechanic that is additional action

    //START
    // after 1.7 sec two collectable (0.5 sec)
    // all other action between 0.8-1.3 sec 

    //Level 1
    // 2c 2o(t1,t2) 2c 3o(t1,t1,t2) 1c 2o(t1,t1) //12

    //Level 2
    // 2c 3o(t1,t1,t2) 1c 2o(t1,t2) 1c 1o(t3) //10

    //Level 3
    // 2c 2o(t1,t2) 1c 1o(t1) 1c 2o(t1,t2) 1c 1o(t3) //11

    //BONUS LEVEL 1
    // 2c 2o(t1,t2) 1c 3o(t1,t1,t2) 1c 1o  //10

    //Level 4 
    // 2c 5o(t1,t1,t1,t1,t2) 1c 1o(t2) 1c 2o(t2,t2) 1c 2o(t2,t2) //15

    //Level 5
    // 2c 2o(t1,t2) 2c 2o(t1,t2) 1c 2o(t1,t1) //11

    //BONUS LEVEL 2
    // 2c 3o(t1,t1,t2) 1c 3o(t1,t1,t2) 1c 1o(t1) //11

    //Level 6
    // 2c 2o(t1,t2) 1c 1o(t1) 2c 2o(t1,t2) 1c 1o(t3) //12

    //Level 7
    //1c 2o(t2,t2) 1c 1o 1c 2o(t2,t2) 1c 1o(t1) //10

    //BONUS LEVEL 3
    // 1c 2o(t1,t2) 1c 2o(t1,t2) 2c 2o(t1,t2) 1c 2o(t1,t1) //13

    //Level 8
    // 2c 2o(t1,t2) 1c 3o(t1,t1,t2) 1c 1o(t1) //10

    //Level 9
    // 2c 2o(t3,t2) 2c 3o(t1,t1,t2) 1c 1o(t1) //11

    //BONUS LEVEL 4
    // 2c 2o(t3,t2) 1c 2o(t1,t1) 1c 3o(t1,t1,t2) 1c 1o(t1) //13

    //Level 10
    // 2c 2o(t1,t2) 2c 3o(t1,t1,t2) 1c 1o(t1) //11

    //Level 11
    //

}