using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Simphony.Simulation;

public class Hauling_1 : MonoBehaviour
{

    public GameObject plane;
    double start_Pos_x;
    double start_Pos_z;
    public double goal_Pos_x = 5.0;
    public double goal_Pos_z = 10.0;
    double x_Diameter;
    double z_Diameter;

    private GameObject[] obstacle;
    double[,] obstacleList;
    double[] diameter;
    List<Node> path;

    //private LineRenderer lineRenderer;
    private float speed;
    public GameObject dot;
    private int number;

    private float timer = 0;
    private float timer_1 = 0;
    private bool finish = false;
    private bool showMsg = false;
    private float reachTime;

    private Vector3 direction;
    private Vector3 v;
    private float angle;

    private List<Node> path_Reverse = new List<Node>();
    private List<float> m = new List<float>();
    private List<float> b = new List<float>();

    private float distance;
    private float time = 0;
    private List<float> timeLablePath = new List<float>();

    DiscreteEventEngine MyEngine = new DiscreteEventEngine();
    private float smallTruckLoading;

    private bool startMessage = true;

    void Awake()
    {
        speed = GetComponent<Loading_1>().speed;

        start_Pos_x = GetComponent<Loading_1>().goal_Pos_x;
        start_Pos_z = GetComponent<Loading_1>().goal_Pos_z;

        x_Diameter = GetComponent<Collider>().bounds.size.x;
        z_Diameter = GetComponent<Collider>().bounds.size.z;
        diameter = new double[] { x_Diameter / GetComponent<BoxCollider>().size.x,
                                  z_Diameter / GetComponent<BoxCollider>().size.z};

        obstacle = GameObject.FindGameObjectsWithTag("Obstacle");

        obstacleList = new double[,] { { obstacle[0].transform.position.x, obstacle[0].transform.position.z, obstacle[0].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[1].transform.position.x, obstacle[1].transform.position.z, obstacle[1].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[2].transform.position.x, obstacle[2].transform.position.z, obstacle[2].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[3].transform.position.x, obstacle[3].transform.position.z, obstacle[3].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[4].transform.position.x, obstacle[4].transform.position.z, obstacle[4].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[5].transform.position.x, obstacle[5].transform.position.z, obstacle[5].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[6].transform.position.x, obstacle[6].transform.position.z, obstacle[6].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[7].transform.position.x, obstacle[7].transform.position.z, obstacle[7].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[8].transform.position.x, obstacle[8].transform.position.z, obstacle[8].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[9].transform.position.x, obstacle[9].transform.position.z, obstacle[9].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[10].transform.position.x, obstacle[10].transform.position.z, obstacle[10].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[11].transform.position.x, obstacle[11].transform.position.z, obstacle[11].GetComponent<Collider>().bounds.size.x / 2.0 }
        };


        InformedRRTStar rrt = new InformedRRTStar(new double[] { start_Pos_x, start_Pos_z },
                     new double[] { goal_Pos_x, goal_Pos_z }, diameter, obstacleList,
                     new double[] { plane.transform.position.x - (plane.GetComponent<Collider>().bounds.size.x / 2.0), plane.transform.position.z + (plane.GetComponent<Collider>().bounds.size.z / 2.0) });

        path = rrt.informed_rrt_star_search();

        string str = "Optimal Path from Loading Point to Dumping Point for RED one : ";
        for (int i = path.Count - 1; i >= 0; i--)
        {
            str += $"({path[i].x:F3}, {path[i].y:F3}), ";
        }
        print(str);

        print($"Distance between Loading Point to Dumping Point for RED one: {InformedRRTStar.get_path_len(path) * 5:F3} m");
        

        // Draw Nodes
        for (int i = 0; i < path.Count; i++)
        {
            Instantiate(dot, new Vector3(Convert.ToSingle(path[i].x), 0.5f, Convert.ToSingle(path[i].y)), Quaternion.identity);
        }

        number = path.Count - 2;


        //create reverse path
        for (int i = path.Count - 1; i >= 0; i--)
        {
            path_Reverse.Add(path[i]);
        }

        //find slop and width of origin for each line
        for (int i = 1; i < path_Reverse.Count; i++)
        {
            float slop = (Convert.ToSingle(path_Reverse[i].y) - Convert.ToSingle(path_Reverse[i - 1].y)) /
                         (Convert.ToSingle(path_Reverse[i].x) - Convert.ToSingle(path_Reverse[i - 1].x));

            float WidthOfOrigin = Convert.ToSingle(path_Reverse[i].y) - slop * Convert.ToSingle(path_Reverse[i].x);

            m.Add(slop);
            b.Add(WidthOfOrigin);
        }

        //clash detection
        timeLablePath.Add(0);

        for (int i = 0; i < path_Reverse.Count - 1; i++)
        {
            distance = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)),
                                        new Vector3(Convert.ToSingle(path_Reverse[i + 1].x), transform.position.y, Convert.ToSingle(path_Reverse[i + 1].y)));

            time += distance / speed;

            timeLablePath.Add(time);

        }


    }



    void FixedUpdate()
    {
        if (GetComponent<Loading_1>().FinishiMessage() == true)
        {
            Model MyModel = new Model(MyEngine);
            smallTruckLoading = Convert.ToSingle(MyModel.MyScenario.GetSmallTruckLoading());

            if (startMessage == true)
            {
                timer_1 += Time.deltaTime;
            }

            //follow path
            if (!finish && timer_1 > smallTruckLoading)
            {
                timer += Time.deltaTime;

                float dist = Vector3.Distance(new Vector3(Convert.ToSingle(path[number].x), transform.position.y, Convert.ToSingle(path[number].y)),
                      transform.position);

                direction = new Vector3(Convert.ToSingle(path[number].x) - transform.position.x, 0,
                                        Convert.ToSingle(path[number].y - transform.position.z));

                v = new Vector3(Convert.ToSingle(path[number].x) - Convert.ToSingle(path[number + 1].x), 0,
                                        Convert.ToSingle(path[number].y) - Convert.ToSingle(path[number + 1].y));

                angle = Vector3.SignedAngle(Vector3.right, v, Vector3.up);

                transform.localRotation = Quaternion.Euler(0, angle, 0);

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Convert.ToSingle(path[number].x),
                            transform.position.y, Convert.ToSingle(path[number].y)), Time.deltaTime * speed);

                if (dist <= 0.1f && number > 0)
                {
                    number--;
                }
                else if (number == 0)
                {
                    showMsg = true;
                    finish = true;
                    reachTime = timer;
                }
            }

        }








    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public bool ShownMessage()
    {
        return showMsg;
    }

    public bool FinishiMessage()
    {
        return finish;
    }

    public float ReachTime()
    {
        return reachTime;
    }

    public List<Node> Getpath()
    {
        return path_Reverse;
    }

    public List<Node> Getpath_line()
    {
        return path;
    }

    public List<float> Get_m()
    {
        return m;
    }

    public List<float> Get_b()
    {
        return b;
    }

    public float GetTime()
    {
        return timer;
    }

    public List<float> GetTimeLablePath()
    {
        return timeLablePath;
    }

    public float GetTimer_1()
    {
        return timer_1;
    }

    public void ChangeStartMessageToFalse()
    {
        startMessage = false;
    }

    public void ChangeStartMessageToTrue()
    {
        startMessage = true;
    }


    //how long does it take to go to the specific point
    public float PointToTime(Vector3 point)
    {
        List<float> distanceList = new List<float>();

        for (int i = 1; i < path_Reverse.Count - 1; i++)
        {
            float dist = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)), point);

            distanceList.Add(dist);
        }

        float c = distanceList.Min();
        int index = distanceList.IndexOf(c) + 1; //3

        float a = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[index - 1].x), transform.position.y, Convert.ToSingle(path_Reverse[index - 1].y)), point);
        float b = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[index + 1].x), transform.position.y, Convert.ToSingle(path_Reverse[index + 1].y)), point);

        float m1 = (Convert.ToSingle(path_Reverse[index].y) - point.z) / (Convert.ToSingle(path_Reverse[index].x) - point.x);

        if (m1 <= m[index - 1] + 0.001 && m1 >= m[index - 1] - 0.001)
        {
            return timeLablePath[index - 1] + (a / speed);
        }

        else if (m1 <= m[index] + 0.001 && m1 >= m[index] - 0.001)
        {
            return timeLablePath[index] + (c / speed);
        }

        else
        {
            return timeLablePath[index - 1] + (a / speed);
        }
    }


    //it finds the specific vector that point is on it
    public Vector3 FindVector(Vector3 point)
    {
        List<float> distanceList = new List<float>();

        for (int i = 1; i < path_Reverse.Count - 1; i++)
        {
            float dist = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)), point);

            distanceList.Add(dist);
        }

        float c = distanceList.Min();
        int index = distanceList.IndexOf(c) + 1; //3

        float m1 = (Convert.ToSingle(path_Reverse[index].y) - point.z) / (Convert.ToSingle(path_Reverse[index].x) - point.x);

        if (m1 <= m[index - 1] + 0.001 && m1 >= m[index - 1] - 0.001)
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index].x) - Convert.ToSingle(path_Reverse[index - 1].x), 0,
                               Convert.ToSingle(path_Reverse[index].y) - Convert.ToSingle(path_Reverse[index - 1].y));
        }

        else if (m1 <= m[index] + 0.001 && m1 >= m[index] - 0.001)
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index + 1].x) - Convert.ToSingle(path_Reverse[index].x), 0,
                               Convert.ToSingle(path_Reverse[index + 1].y) - Convert.ToSingle(path_Reverse[index].y));
        }

        else
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index].x) - Convert.ToSingle(path_Reverse[index - 1].x), 0,
                   Convert.ToSingle(path_Reverse[index].y) - Convert.ToSingle(path_Reverse[index - 1].y));
        }
    }


    //it finds the nearest path point before intersection point
    public Vector3 FindFirstPoint(Vector3 point)
    {
        List<float> distanceList = new List<float>();

        for (int i = 1; i < path_Reverse.Count - 1; i++)
        {
            float dist = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)), point);

            distanceList.Add(dist);
        }

        float c = distanceList.Min();
        int index = distanceList.IndexOf(c) + 1; //3

        float m1 = (Convert.ToSingle(path_Reverse[index].y) - point.z) / (Convert.ToSingle(path_Reverse[index].x) - point.x);

        if (m1 <= m[index - 1] + 0.001 && m1 >= m[index - 1] - 0.001)
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index - 1].x), transform.position.y, Convert.ToSingle(path_Reverse[index - 1].y));
        }

        else if (m1 <= m[index] + 0.001 && m1 >= m[index] - 0.001)
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index].x), transform.position.y, Convert.ToSingle(path_Reverse[index].y));
        }

        else
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index - 1].x), transform.position.y, Convert.ToSingle(path_Reverse[index - 1].y));
        }
    }


    //it finds the nearest point after intersection point
    public Vector3 FindEndPoint(Vector3 point)
    {
        List<float> distanceList = new List<float>();

        for (int i = 1; i < path_Reverse.Count - 1; i++)
        {
            float dist = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)), point);

            distanceList.Add(dist);
        }

        float c = distanceList.Min();
        int index = distanceList.IndexOf(c) + 1; //3

        float m1 = (Convert.ToSingle(path_Reverse[index].y) - point.z) / (Convert.ToSingle(path_Reverse[index].x) - point.x);

        if (m1 <= m[index - 1] + 0.001 && m1 >= m[index - 1] - 0.001)
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index].x), transform.position.y, Convert.ToSingle(path_Reverse[index].y));
        }

        else if (m1 <= m[index] + 0.001 && m1 >= m[index] - 0.001)
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index + 1].x), transform.position.y, Convert.ToSingle(path_Reverse[index + 1].y));
        }

        else
        {
            return new Vector3(Convert.ToSingle(path_Reverse[index].x), transform.position.y, Convert.ToSingle(path_Reverse[index].y));
        }
    }


    //it finds the nearest index before intersection point
    public int FindFirstIndex(Vector3 point)
    {
        List<float> distanceList = new List<float>();

        for (int i = 1; i < path_Reverse.Count - 1; i++)
        {
            float dist = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)), point);

            distanceList.Add(dist);
        }

        float c = distanceList.Min();
        int index = distanceList.IndexOf(c) + 1; //3

        float m1 = (Convert.ToSingle(path_Reverse[index].y) - point.z) / (Convert.ToSingle(path_Reverse[index].x) - point.x);

        if (m1 <= m[index - 1] + 0.001 && m1 >= m[index - 1] - 0.001)
        {
            return index - 1;
        }

        else if (m1 <= m[index] + 0.001 && m1 >= m[index] - 0.001)
        {
            return index;
        }

        else
        {
            return 3;
        }
    }


    //it finds the nearest index after intersection point
    public int FindEndIndex(Vector3 point)
    {
        List<float> distanceList = new List<float>();

        for (int i = 1; i < path_Reverse.Count - 1; i++)
        {
            float dist = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)), point);

            distanceList.Add(dist);
        }

        float c = distanceList.Min();
        int index = distanceList.IndexOf(c) + 1; //3

        float m1 = (Convert.ToSingle(path_Reverse[index].y) - point.z) / (Convert.ToSingle(path_Reverse[index].x) - point.x);

        if (m1 <= m[index - 1] + 0.001 && m1 >= m[index - 1] - 0.001)
        {
            return index;
        }

        else if (m1 <= m[index] + 0.001 && m1 >= m[index] - 0.001)
        {
            return index + 1;
        }

        else
        {
            return 3;
        }
    }


}
