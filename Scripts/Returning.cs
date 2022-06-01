using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.ComponentModel;
using TMPro;
using Simphony.Simulation;

public class Returning : MonoBehaviour
{
    public GameObject plane;
    double start_Pos_x;
    double start_Pos_z;
    public double goal_Pos_x = 5.0;
    public double goal_Pos_z = 10.0;
    double x_Diameter;
    double z_Diameter;

    private GameObject[] obstacle;
    private double[,] obstacleList;
    private double[] diameter;
    //private float diameter_box;
    private List<Node> path;
    private List<Node> path_Reverse = new List<Node>();
    private List<Node> path_Reverse_1 = new List<Node>();

    private LineRenderer lineRenderer;
    private float speed;
    public GameObject dot;
    private int number;

    private BoxCollider m_Collider;
    private BoxCollider n_Collider;

    private float x_ColliderDiameter;
    private float z_ColliderDiameter;
    private float x_ColliderDiameter_1;
    private float z_ColliderDiameter_1;

    private List<Vector3> dirc_List = new List<Vector3>();

    private List<float> m = new List<float>();
    private List<float> b = new List<float>();

    private List<float> m_1 = new List<float>();
    private List<float> b_1 = new List<float>();

    private Vector3 intersectionPoint;
    private float x_Intersection;
    private float y_Intersection;
    private float angle_Intersection;
    private float angle_new;
    private float angle_1_new;

    private float length;
    private float length_1;
    private float length_new;
    private float length_1_new;

    private Vector3 start_point;
    private Vector3 exit_point;
    private Vector3 start_1_point;
    private Vector3 exit_1_point;


    private Vector3 start_point_first;
    private Vector3 start_point_second;
    private Vector3 exit_point_first;
    private Vector3 exit_point_second;
    private Vector3 start_1_point_first;
    private Vector3 start_1_point_second;
    private Vector3 exit_1_point_first;
    private Vector3 exit_1_point_second;


    private int firstIndex;
    private int endIndex;
    private int firstIndex_1;
    private int endIndex_1;

    private float x_new;
    private float y_new;
    private float x_1_new;
    private float y_1_new;
    private float x_new_second;
    private float y_new_second;
    private float x_1_new_second;
    private float y_1_new_second;
    private float x_new_third;
    private float y_new_third;
    private float x_1_new_third;
    private float y_1_new_third;
    private float x_new_forth;
    private float y_new_forth;
    private float x_1_new_forth;
    private float y_1_new_forth;

    private float t1;
    private float t2;
    private float s1;
    private float s2;

    private float t1_new;
    private float t2_new;
    private float s1_new;
    private float s2_new;

    //private Vector3 e;
    //private List<float> angles = new List<float>();

    private float timer = 0;
    private float timer_1 = 0;
    private float timer_2 = 0;
    private float startTime = 0;
    private bool finish = false;
    private bool showMsg = false;
    private bool showMsg2 = true;
    private bool Warning = false;
    private bool PlusMsg = false;
    private float reachTime;

    private Vector3 direction;
    private Vector3 v;
    private float angle;

    //clash detail
    public GameObject priorityObject;

    private bool clash = false;

    private float distance;
    private float time = 0;
    private List<float> timeLablePath = new List<float>();

    private float angle_new_second;
    private float angle_1_new_second;
    private float angle_new_third;
    private float angle_1_new_third;
    private float angle_new_forth;
    private float angle_1_new_forth;

    private float length_new_second;
    private float length_1_new_second;
    private float length_new_third;
    private float length_1_new_third;
    private float length_new_forth;
    private float length_1_new_forth;

    private bool flag_false = false;
    private bool flag1 = false;
    private bool flag2 = false;
    private bool flag3 = false;
    private bool flag4 = false;
    private bool flag5 = false;

    DiscreteEventEngine MyEngine = new DiscreteEventEngine();
    private float largeTruckDumping;
    private float smallTruckDumping;

    private GameObject canvas;

    void Start()
    {
        speed = GetComponent<Loading>().speed;

        start_Pos_x = GetComponent<Hauling>().goal_Pos_x;
        start_Pos_z = GetComponent<Hauling>().goal_Pos_z;

        x_Diameter = GetComponent<Collider>().bounds.size.x;
        z_Diameter = GetComponent<Collider>().bounds.size.z;
        diameter = new double[] { x_Diameter / GetComponent<BoxCollider>().size.x,
                                  z_Diameter / GetComponent<BoxCollider>().size.z};

        //Creating BoxCollider of +0.5f on each side for safety
        m_Collider = GetComponent<BoxCollider>();
        m_Collider.size = new Vector3(1 + (0.5f / transform.localScale.x), 1f, 1 + (0.5f / transform.localScale.z));
        x_ColliderDiameter = GetComponent<Collider>().bounds.size.x;
        z_ColliderDiameter = GetComponent<Collider>().bounds.size.z;

        //Creating BoxCollider of +0.5f on each side for safety
        n_Collider = priorityObject.GetComponent<BoxCollider>();
        n_Collider.size = new Vector3(1 + (0.5f / priorityObject.GetComponent<Transform>().localScale.x), 1f, 1 + (0.5f / priorityObject.GetComponent<Transform>().localScale.z));
        x_ColliderDiameter_1 = priorityObject.GetComponent<Collider>().bounds.size.x;
        z_ColliderDiameter_1 = priorityObject.GetComponent<Collider>().bounds.size.z;

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

        string str = "Optimal Path from Dumping Point to Starting Point for for BLUE one : ";
        for (int i = path.Count - 1; i >= 0; i--)
        {
            str += $"({path[i].x:F3}, {path[i].y:F3}), ";
        }
        print(str);

        print($"Distance between Dumping Point to Starting Point for BLUE one: {InformedRRTStar.get_path_len(path) * 5:F3} m");


        //Draw Line between nodes
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = path.Count + GetComponent<Hauling>().Getpath_line().Count + GetComponent<Loading>().Getpath_line().Count - 2;

        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(Convert.ToSingle(path[i].x), 0.5f, Convert.ToSingle(path[i].y)));
        }

        for (int i = 1; i < GetComponent<Hauling>().Getpath_line().Count; i++)
        {
            lineRenderer.SetPosition(i + path.Count - 1, new Vector3(Convert.ToSingle(GetComponent<Hauling>().Getpath_line()[i].x), 0.5f, Convert.ToSingle(GetComponent<Hauling>().Getpath_line()[i].y)));
        }

        for (int i = 1; i < GetComponent<Loading>().Getpath_line().Count; i++)
        {
            lineRenderer.SetPosition(i + path.Count + GetComponent<Hauling>().Getpath_line().Count - 2, new Vector3(Convert.ToSingle(GetComponent<Loading>().Getpath_line()[i].x), 0.5f, Convert.ToSingle(GetComponent<Loading>().Getpath_line()[i].y)));
        }

        //Draw Nodes
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


        //clash detection
        timeLablePath.Add(0);

        for (int i = 0; i < path_Reverse.Count - 1; i++)
        {
            distance = Vector3.Distance(new Vector3(Convert.ToSingle(path_Reverse[i].x), transform.position.y, Convert.ToSingle(path_Reverse[i].y)),
                                        new Vector3(Convert.ToSingle(path_Reverse[i + 1].x), transform.position.y, Convert.ToSingle(path_Reverse[i + 1].y)));

            time += distance / speed;

            timeLablePath.Add(time);
        }


        //create normalize direction for each line
        for (int i = 1; i < path_Reverse.Count; i++)
        {
            Vector3 dirc = new Vector3(Convert.ToSingle(path_Reverse[i].x) - Convert.ToSingle(path_Reverse[i - 1].x), 0,
                                       Convert.ToSingle(path_Reverse[i].y) - Convert.ToSingle(path_Reverse[i - 1].y));

            dirc_List.Add(dirc.normalized);
        }

        //it finds slop and width of origin for each line
        for (int i = 1; i < path_Reverse.Count; i++)
        {
            float slop = (Convert.ToSingle(path_Reverse[i].y) - Convert.ToSingle(path_Reverse[i - 1].y)) /
                         (Convert.ToSingle(path_Reverse[i].x) - Convert.ToSingle(path_Reverse[i - 1].x));

            float WidthOfOrigin = Convert.ToSingle(path_Reverse[i].y) - slop * Convert.ToSingle(path_Reverse[i].x);

            m.Add(slop);
            b.Add(WidthOfOrigin);
        }

        path_Reverse_1 = priorityObject.GetComponent<Returning_1>().Getpath();
        b_1 = priorityObject.GetComponent<Returning_1>().Get_b();
        m_1 = priorityObject.GetComponent<Returning_1>().Get_m();


        //Calculate the intersection point of two line
        for (int i = 1; i < path_Reverse.Count; i++)
        {
            for (int j = 1; j < path_Reverse_1.Count; j++)
            {

                float x = (b[i - 1] - b_1[j - 1]) / (m_1[j - 1] - m[i - 1]);

                float y = m[i - 1] * x + b[i - 1];

                if (path_Reverse[i].x > path_Reverse[i - 1].x)
                {
                    if (x <= path_Reverse[i].x && x >= path_Reverse[i - 1].x)
                    {
                        if (path_Reverse_1[j].x > path_Reverse_1[j - 1].x)
                        {

                            if (x <= path_Reverse_1[j].x && x >= path_Reverse_1[j - 1].x)
                            {
                                x_Intersection = x;
                                y_Intersection = y;

                                break;
                            }

                        }

                        else
                        {
                            if (x >= path_Reverse_1[j].x && x <= path_Reverse_1[j - 1].x)
                            {
                                x_Intersection = x;
                                y_Intersection = y;

                                break;
                            }
                        }
                    }
                }

                else
                {
                    if (x >= path_Reverse[i].x && x <= path_Reverse[i - 1].x)
                    {
                        if (path_Reverse_1[j].x > path_Reverse_1[j - 1].x)
                        {

                            if (x <= path_Reverse_1[j].x && x >= path_Reverse_1[j - 1].x)
                            {
                                x_Intersection = x;
                                y_Intersection = y;

                                break;
                            }

                        }

                        else
                        {
                            if (x >= path_Reverse_1[j].x && x <= path_Reverse_1[j - 1].x)
                            {
                                x_Intersection = x;
                                y_Intersection = y;

                                break;
                            }
                        }
                    }
                }
            }
        }

        intersectionPoint = new Vector3(x_Intersection, transform.position.y, y_Intersection);

        if (x_Intersection == 0 && y_Intersection == 0)
        {
            //EditorUtility.DisplayDialog("warning!", "There is no intersection point from Dumping Site to starting Point in the path of two objects.", "ok");

            canvas = FindInActiveObjectByTag("Canvas6");
            canvas.SetActive(true);

            Warning = true;
        }

        angle_Intersection = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

        Vector3 direction_Intersection = FindVector(new Vector3(x_Intersection, 0, y_Intersection)).normalized;
        Vector3 direction_Intersection_1 = priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)).normalized;

        //distance between (intersection point) to (pause point)
        length = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_Intersection * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_Intersection * Mathf.PI / 180))));
        length_1 = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_Intersection * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_Intersection * Mathf.PI / 180))));

        firstIndex = FindFirstIndex(intersectionPoint);
        endIndex = FindEndIndex(intersectionPoint);
        firstIndex_1 = priorityObject.GetComponent<Returning_1>().FindFirstIndex(intersectionPoint);
        endIndex_1 = priorityObject.GetComponent<Returning_1>().FindEndIndex(intersectionPoint);



        if (Warning == false)
        {

            // [1]
            if (length <= Vector3.Distance(intersectionPoint, FindFirstPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindFirstPoint(intersectionPoint)))
            {
                if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("1-5");

                    start_point = intersectionPoint + (direction_Intersection * -1 * length);
                    start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);

                    exit_point = intersectionPoint + (direction_Intersection * length);
                    exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);

                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("1-6");

                    start_point = intersectionPoint + (direction_Intersection * -1 * length);
                    start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);
                    exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);

                    if (endIndex < path_Reverse.Count - 1)
                    {
                        x_new = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                        y_new = m[endIndex] * x_new + b[endIndex];

                        Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                         Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                        angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                        length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));

                        exit_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * length_new);
                    }

                    else if (endIndex == path_Reverse.Count - 1)
                    {
                        exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                        flag1 = true;
                    }


                }

                else if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("1-7");

                    if (angle_Intersection < 90)
                    {
                        start_point = intersectionPoint + (direction_Intersection * -1 * length);
                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);

                        if (endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                        }

                        else if (endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }
                    }

                    else
                    {
                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);
                        exit_point = intersectionPoint + (direction_Intersection * length);

                        if (endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }



                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                        }

                        else if (endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                    }
                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("1-8");

                    if (angle_Intersection > 90)
                    {
                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);

                        if (endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * -1 * length_new_second);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }





                            x_new = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new = m[endIndex] * x_new + b[endIndex];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                             Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));
                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * length_new);

                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);
                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                        }

                        else if (endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * -1 * length_new_second);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);
                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);


                            flag1 = true;
                        }

                        else if (endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);

                            x_new = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new = m[endIndex] * x_new + b[endIndex];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                             Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));
                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * length_new);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));


                            flag2 = true;
                        }

                        else if (endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                    }

                    else
                    {
                        start_point = intersectionPoint + (direction_Intersection * -1 * length);
                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);

                        if (endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            x_new = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new = m[endIndex] * x_new + b[endIndex];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                             Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));
                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * length_new);

                            //or this
                            x_new_second = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(vector_new_second, vector_1_new_second);
                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);
                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                        }

                        else if (endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);
                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag1 = true;
                        }

                        else if (endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            x_new = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new = m[endIndex] * x_new + b[endIndex];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                             Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));
                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * length_new);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }
                    }

                }
            }


            // [2]
            else if (length > Vector3.Distance(intersectionPoint, FindFirstPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindFirstPoint(intersectionPoint)))
            {
                if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("2-5");

                    start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);

                    exit_point = intersectionPoint + (direction_Intersection * length);
                    exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);

                    if (firstIndex > 0)
                    {
                        x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                        y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                        Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                         Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                        angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                        length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                        start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                    }

                    else if (firstIndex == 0)
                    {
                        start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                        flag2 = true;
                    }
                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("2-6");

                    start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);
                    exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);

                    if (firstIndex > 0 && endIndex < path_Reverse.Count - 1)
                    {
                        x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                        y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                        Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                         Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                        angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                        length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                        start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                        x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                        y_new_second = m[endIndex] * x_new_second + b[endIndex];

                        Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                        angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                        length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                        exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);
                    }

                    else if (firstIndex == 0 && endIndex < path_Reverse.Count - 1)
                    {
                        start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                        x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                        y_new_second = m[endIndex] * x_new_second + b[endIndex];

                        Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                        angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                        length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                        exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                        flag2 = true;
                    }

                    else if (firstIndex > 0 && endIndex == path_Reverse.Count - 1)
                    {
                        x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                        y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                        Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                         Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                        angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                        length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                        start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);

                        exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                        flag1 = true;
                    }

                    else if (firstIndex == 0 && endIndex == path_Reverse.Count - 1)
                    {
                        start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                        exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                        flag_false = true;
                    }
                }

                else if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("2-7");


                    if (angle_Intersection < 90)
                    {

                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);

                        if (firstIndex > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                        }

                        else if (firstIndex == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {

                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));


                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);




                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                    }

                    else
                    {

                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);
                        exit_point = intersectionPoint + (direction_Intersection * length);


                        if (firstIndex > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(vector_new, vector_1_new_second);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_second = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new_second = m[firstIndex - 1] * x_new_second + b[firstIndex - 1];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * -1 * length_new_second);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }




                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);


                        }

                        else if (firstIndex == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));


                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }
                    }
                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("2-8");

                    if (angle_Intersection < 90)
                    {

                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);


                        if (firstIndex > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            //or this
                            x_new_third = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_second);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                        }

                        else if (firstIndex == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));



                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            //or this
                            x_new_third = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_second);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }
                    }

                    else
                    {

                        start_1_point = intersectionPoint + (direction_Intersection_1 * -1 * length_1);


                        if (firstIndex > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {

                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_second);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }






                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);



                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                        }

                        else if (firstIndex == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);



                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_second);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new = m[endIndex - 1] * x_1_new + b[endIndex - 1];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * length_1_new);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }
                    }
                }
            }


            // [3]
            else if (length <= Vector3.Distance(intersectionPoint, FindFirstPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindFirstPoint(intersectionPoint)))
            {
                if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("3-5");

                    if (angle_Intersection < 90)
                    {

                        exit_point = intersectionPoint + (direction_Intersection * length);
                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (firstIndex_1 > 0)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);
                        }

                        else if (firstIndex_1 == 0)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            flag1 = true;
                        }

                    }

                    else
                    {

                        start_point = intersectionPoint + (direction_Intersection * -1 * length);
                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (firstIndex_1 > 0)
                        {
                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }

                        }

                        else if (firstIndex_1 == 0)
                        {
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = intersectionPoint + (direction_Intersection * length);

                            flag1 = true;
                        }

                    }

                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("3-6");

                    if (angle_Intersection < 90)
                    {

                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (endIndex < path_Reverse.Count - 1 && firstIndex_1 > 0)
                        {

                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                        }

                        else if (endIndex == path_Reverse.Count - 1 && firstIndex_1 > 0)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                        else if (endIndex < path_Reverse.Count - 1 && firstIndex_1 == 0)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag1 = true;
                        }

                        else if (endIndex == path_Reverse.Count - 1 && firstIndex_1 == 0)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));
                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                    }

                    else
                    {

                        start_point = intersectionPoint + (direction_Intersection * -1 * length);
                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (endIndex < path_Reverse.Count - 1 && firstIndex_1 > 0)
                        {
                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_third = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_second);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }

                        }

                        else if (endIndex == path_Reverse.Count - 1 && firstIndex_1 > 0)
                        {
                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                        else if (endIndex < path_Reverse.Count - 1 && firstIndex_1 == 0)
                        {
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag1 = true;
                        }

                        else if (endIndex == path_Reverse.Count - 1 && firstIndex_1 == 0)
                        {
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));
                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                    }

                }

                else if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("3-7");

                    if (angle_Intersection < 90)
                    {
                        if (firstIndex_1 > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {

                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                        }

                        else if (firstIndex_1 == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                    }

                    else
                    {
                        if (firstIndex_1 > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {

                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }



                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                        }

                        else if (firstIndex_1 == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            exit_point = intersectionPoint + (direction_Intersection * length);

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                    }

                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    print("3-8");

                    if (angle_Intersection < 90)
                    {
                        if (firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_third = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_forth);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                        }

                        else if (firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_third = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_forth);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));



                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                    }

                    else
                    {
                        if (firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            x_new_second = (b[endIndex] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_third = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_forth);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                        }

                        else if (firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));








                            x_new_second = (b[endIndex] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            x_new_second = (b[endIndex] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_third = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_forth);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            start_point_first = intersectionPoint + (direction_Intersection * -1 * length);

                            //or this
                            x_new = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new = m[firstIndex] * x_new + b[firstIndex];

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new, transform.position.y, y_new) + (direction_Intersection * -1 * length_new);

                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            x_new_second = (b[endIndex] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            start_point = intersectionPoint + (direction_Intersection * -1 * length);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }
                    }
                }
            }


            // [4]
            else if (length > Vector3.Distance(intersectionPoint, FindFirstPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindFirstPoint(intersectionPoint)))
            {
                if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    //print("4-5");

                    if (angle_Intersection < 90)
                    {

                        exit_point = intersectionPoint + (direction_Intersection * length);
                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (firstIndex > 0 && firstIndex_1 > 0)
                        {
                            print("4-5-1");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);

                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0)
                        {
                            print("4-5-2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0)
                        {
                            print("4-5-3");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            flag1 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0)
                        {
                            print("4-5-4");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            flag_false = true;
                        }

                    }

                    else
                    {

                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (firstIndex > 0 && firstIndex_1 > 0)
                        {
                            print("4-5_1");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);



                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }

                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0)
                        {
                            print("4-5_2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0)
                        {
                            print("4-5_3");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = intersectionPoint + (direction_Intersection * length);

                            flag1 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0)
                        {
                            print("4-5_4");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = intersectionPoint + (direction_Intersection * length);

                            flag_false = true;
                        }

                    }

                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 <= Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    //print("4-6");

                    if (angle_Intersection < 90)
                    {

                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (firstIndex > 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6-1");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6-2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6-3");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));



                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6-4");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6-5");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6-6");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag_false = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6-7");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6-8");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag_false = true;
                        }

                    }

                    else
                    {

                        exit_1_point = intersectionPoint + (direction_Intersection_1 * length_1);


                        if (firstIndex > 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6_1");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_third = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);


                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }

                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6_2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_third = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_third = m[endIndex] * x_new_third + b[endIndex];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * length_new_third);


                            //find the maximum [t2]
                            float t1_new = PointToTime(exit_point_first);
                            float t2_new = PointToTime(exit_point_second);

                            if (t1_new > t2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6_3");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6_4");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1)
                        {
                            print("4-6_5");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6_6");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag_false = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6_7");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag1 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1)
                        {
                            print("4-6_8");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            flag_false = true;
                        }

                    }

                }

                else if (length <= Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    //print("4-7");

                    if (angle_Intersection < 90)
                    {

                        if (firstIndex > 0 && firstIndex_1 > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7-1");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7-2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7-3");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7-4");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7-5");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_forth);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7-6");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7-7");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7-8");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                    }

                    else
                    {

                        if (firstIndex > 0 && firstIndex_1 > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7_1");
                            x_new = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(vector_new, vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7_2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7_3");
                            x_new = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new = Vector3.Angle(vector_new, vector_1_new_third);

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));





                            exit_point = intersectionPoint + (direction_Intersection * length);

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7_4");
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);



                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-7_5");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = intersectionPoint + (direction_Intersection * length);

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7_6");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point_first = intersectionPoint + (direction_Intersection * length);

                            //or this
                            x_new_second = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_new_second = m[firstIndex] * x_new_second + b[firstIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                    Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_new_second);

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_second, transform.position.y, y_new_second) + (direction_Intersection * length_new_second);

                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7_7");
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-7_8");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = intersectionPoint + (direction_Intersection * length);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                    }

                }

                else if (length > Vector3.Distance(intersectionPoint, FindEndPoint(intersectionPoint)) && length_1 > Vector3.Distance(intersectionPoint, priorityObject.GetComponent<Returning_1>().FindEndPoint(intersectionPoint)))
                {
                    //print("4-8");

                    if (angle_Intersection < 90)
                    {

                        if (firstIndex > 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-1");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-3");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-4");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-5");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);






                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-6");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-7");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-8");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-9");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-10");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));



                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-11");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //or this
                            x_new_third = (b[firstIndex - 1] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-12");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-13");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8-14");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-15");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8-16");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                    }

                    else
                    {
                        if (firstIndex > 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_1");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //ot this
                            x_new_third = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }



                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);







                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_2");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }



                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_3");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //ot this
                            x_new_third = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_4");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //ot this
                            x_new_third = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_5");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_6");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_7");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_8");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);





                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point_first = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);


                            //or this
                            x_new_forth = (b[endIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[endIndex]);
                            y_new_forth = m[endIndex] * x_new_forth + b[endIndex];

                            Vector3 vector_new_forth = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                   Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            Vector3 vector_1_new_forth = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_new_forth = Vector3.Angle(vector_new_forth, vector_1_new_forth);

                            length_new_forth = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_forth * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_forth * Mathf.PI / 180))));
                            exit_point_second = new Vector3(x_new_forth, transform.position.y, y_new_forth) + (vector_new_forth.normalized * length_new_forth);


                            //find the maximum [t2]
                            float s1_new = PointToTime(exit_point_first);
                            float s2_new = PointToTime(exit_point_second);

                            if (s1_new > s2_new)
                            {
                                exit_point = exit_point_first;
                            }

                            else
                            {
                                exit_point = exit_point_second;
                            }


                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag2 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_9");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point_first = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            //ot this
                            x_new_third = (b[firstIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[firstIndex - 1]);
                            y_new_third = m[firstIndex - 1] * x_new_third + b[firstIndex - 1];

                            Vector3 vector_new_third = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                                   Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            Vector3 vector_1_new_third = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                     Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_new_third = Vector3.Angle(vector_new_third, vector_1_new_third);

                            length_new_third = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_third * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_third * Mathf.PI / 180))));
                            start_point_second = new Vector3(x_new_third, transform.position.y, y_new_third) + (vector_new_third.normalized * -1 * length_new_third);


                            //find the minimum [t1]
                            float t1_new = PointToTime(start_point_first);
                            float t2_new = PointToTime(start_point_second);

                            if (t1_new < t2_new)
                            {
                                start_point = start_point_first;
                            }

                            else
                            {
                                start_point = start_point_second;
                            }


                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag1 = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_10");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);

                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));




                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);

                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_11");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);


                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);



                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex > 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_12");
                            x_new = (b[firstIndex - 1] - b_1[firstIndex_1]) / (m_1[firstIndex_1] - m[firstIndex - 1]);
                            y_new = m[firstIndex - 1] * x_new + b[firstIndex - 1];

                            Vector3 vector_new = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x) - Convert.ToSingle(path_Reverse[firstIndex - 1].x), 0,
                                                             Convert.ToSingle(path_Reverse[firstIndex].y) - Convert.ToSingle(path_Reverse[firstIndex - 1].y));

                            angle_new = Vector3.Angle(vector_new, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new * Mathf.PI / 180))));
                            start_point = new Vector3(x_new, transform.position.y, y_new) + (vector_new.normalized * -1 * length_new);
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 > 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_13");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));

                            x_1_new = (b[firstIndex] - b_1[firstIndex_1 - 1]) / (m_1[firstIndex_1 - 1] - m[firstIndex]);
                            y_1_new = m[firstIndex] * x_1_new + b[firstIndex];

                            Vector3 vector_1_new = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].x), 0,
                                                               Convert.ToSingle(path_Reverse_1[firstIndex_1].y) - Convert.ToSingle(path_Reverse_1[firstIndex_1 - 1].y));

                            angle_1_new = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new);

                            length_1_new = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new * Mathf.PI / 180))));
                            start_1_point = new Vector3(x_1_new, transform.position.y, y_1_new) + (vector_1_new.normalized * -1 * length_1_new);


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex < path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_14");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));



                            x_new_second = (b[endIndex] - b_1[endIndex_1 - 1]) / (m_1[endIndex_1 - 1] - m[endIndex]);
                            y_new_second = m[endIndex] * x_new_second + b[endIndex];

                            Vector3 vector_new_second = new Vector3(Convert.ToSingle(path_Reverse[endIndex + 1].x) - Convert.ToSingle(path_Reverse[endIndex].x), 0,
                                                                    Convert.ToSingle(path_Reverse[endIndex + 1].y) - Convert.ToSingle(path_Reverse[endIndex].y));

                            angle_new_second = Vector3.Angle(vector_new_second, priorityObject.GetComponent<Returning_1>().FindVector(new Vector3(x_Intersection, 0, y_Intersection)));

                            length_new_second = (x_ColliderDiameter / 2) + (z_ColliderDiameter_1 / (2 * Mathf.Sin(angle_new_second * Mathf.PI / 180))) + (z_ColliderDiameter / (2 * Mathf.Abs(Mathf.Tan(angle_new_second * Mathf.PI / 180))));
                            exit_point = new Vector3(x_new_second, transform.position.y, y_new_second) + (vector_new_second.normalized * length_new_second);
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 < path_Reverse_1.Count - 1)
                        {
                            print("4-8_15");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));


                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));

                            x_1_new_second = (b[endIndex - 1] - b_1[endIndex_1]) / (m_1[endIndex_1] - m[endIndex - 1]);
                            y_1_new_second = m[endIndex - 1] * x_1_new_second + b[endIndex - 1];

                            Vector3 vector_1_new_second = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].x) - Convert.ToSingle(path_Reverse_1[endIndex_1].x), 0,
                                                                      Convert.ToSingle(path_Reverse_1[endIndex_1 + 1].y) - Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            angle_1_new_second = Vector3.Angle(FindVector(new Vector3(x_Intersection, 0, y_Intersection)), vector_1_new_second);

                            length_1_new_second = (x_ColliderDiameter_1 / 2) + (z_ColliderDiameter / (2 * Mathf.Sin(angle_1_new_second * Mathf.PI / 180))) + (z_ColliderDiameter_1 / (2 * Mathf.Abs(Mathf.Tan(angle_1_new_second * Mathf.PI / 180))));
                            exit_1_point = new Vector3(x_1_new_second, transform.position.y, y_1_new_second) + (vector_1_new_second.normalized * length_1_new_second);

                            flag_false = true;
                        }

                        else if (firstIndex == 0 && firstIndex_1 == 0 && endIndex == path_Reverse.Count - 1 && endIndex_1 == path_Reverse_1.Count - 1)
                        {
                            print("4-8_16");
                            start_point = new Vector3(Convert.ToSingle(path_Reverse[firstIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[firstIndex].y));
                            start_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[firstIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[firstIndex_1].y));

                            exit_point = new Vector3(Convert.ToSingle(path_Reverse[endIndex].x), transform.position.y, Convert.ToSingle(path_Reverse[endIndex].y));
                            exit_1_point = new Vector3(Convert.ToSingle(path_Reverse_1[endIndex_1].x), transform.position.y, Convert.ToSingle(path_Reverse_1[endIndex_1].y));

                            flag_false = true;
                        }
                    }
                }
            }


            t1 = PointToTime(start_point);
            t2 = PointToTime(exit_point);

            s1 = priorityObject.GetComponent<Returning_1>().PointToTime(start_1_point);
            s2 = priorityObject.GetComponent<Returning_1>().PointToTime(exit_1_point);

            //print("t1: " + t1);
            //print("t2: " + t2);
            //print("s1: " + s1);
            //print("s2: " + s2);

        }
    }

    void FixedUpdate()
    {

        if (GetComponent<Hauling>().FinishiMessage() == true)
        {
            timer_2 += Time.deltaTime;

            Model MyModel = new Model(MyEngine);
            largeTruckDumping = Convert.ToSingle(MyModel.MyScenario.GetLargeTruckDumping());
            smallTruckDumping = Convert.ToSingle(MyModel.MyScenario.GetSmallTruckDumping());

            //abi zoodtar reside [1]
            if (priorityObject.GetComponent<Hauling_1>().FinishiMessage() == false && flag3 == false)
            {
                flag5 = true;
            }

            //ghermez zoodtar reside
            else if (priorityObject.GetComponent<Hauling_1>().FinishiMessage() == true && flag3 == false && flag5 == false)
            {

                //ghermez hanuz dar marhale Loading ast [2]
                if (priorityObject.GetComponent<Returning_1>().GetTimer_1() <= smallTruckDumping)
                {
                    flag4 = true;
                }

                //ghermez loading ra tamam karde ast [3]
                else if (priorityObject.GetComponent<Returning_1>().GetTimer_1() > smallTruckDumping && flag4 == false)
                {
                    timer_1 += Time.deltaTime;

                    //sepas abi loading ra tamam karde ast
                    if (timer_1 > largeTruckDumping)
                    {

                        //ghermez as s1 rad nashode ast
                        if (s1 >= priorityObject.GetComponent<Returning_1>().GetTime())
                        {
                            s1_new = s1 - priorityObject.GetComponent<Returning_1>().GetTime();
                            s2_new = s2 - priorityObject.GetComponent<Returning_1>().GetTime();


                            startTime = timer_2;
                            flag3 = true;
                        }

                        //ghermez as s1 rad shode ast
                        else
                        {
                            s1_new = 0;
                            s2_new = s2 - priorityObject.GetComponent<Returning_1>().GetTime();


                            startTime = timer_2;
                            flag1 = true;
                            flag3 = true;
                        }
                    }
                }


                //ghermez loading ra tamam karde ast .....[2]
                if (flag4 == true && priorityObject.GetComponent<Returning_1>().GetTimer_1() > smallTruckDumping && flag3 == false)
                {
                    timer_1 += Time.deltaTime;

                    //sepas abi loading ra tamam karde ast
                    if (timer_1 > largeTruckDumping)
                    {

                        //ghermez az s1 rad nashode ast
                        if (s1 >= priorityObject.GetComponent<Returning_1>().GetTime())
                        {
                            s1_new = s1 - priorityObject.GetComponent<Returning_1>().GetTime();
                            s2_new = s2 - priorityObject.GetComponent<Returning_1>().GetTime();


                            startTime = timer_2;
                            flag3 = true;
                        }

                        //ghermez as s1 rad shode ast
                        else
                        {
                            s1_new = 0;
                            s2_new = s2 - priorityObject.GetComponent<Returning_1>().GetTime();


                            startTime = timer_2;
                            flag1 = true;
                            flag3 = true;
                        }
                    }
                }

            }

            //....[1]
            if (flag5 == true && flag3 == false)
            {
                timer_1 += Time.deltaTime;
                priorityObject.GetComponent<Returning_1>().ChangeStartMessageToFalse();

                //abi loading ra tamam karde ast
                if (timer_1 > largeTruckDumping)
                {
                    //ghermez hanuz be loading point nareside ast
                    if (priorityObject.GetComponent<Hauling_1>().FinishiMessage() == false)
                    {
                        s1_new = s1 + (priorityObject.GetComponent<Hauling_1>().GetTimeLablePath()[priorityObject.GetComponent<Hauling_1>().Getpath().Count - 1] - priorityObject.GetComponent<Hauling_1>().GetTime()) + smallTruckDumping;
                        s2_new = s2 + (priorityObject.GetComponent<Hauling_1>().GetTimeLablePath()[priorityObject.GetComponent<Hauling_1>().Getpath().Count - 1] - priorityObject.GetComponent<Hauling_1>().GetTime()) + smallTruckDumping;

                        priorityObject.GetComponent<Returning_1>().ChangeStartMessageToTrue();

                        startTime = timer_2;
                        flag3 = true;
                    }

                    //ghermez be loading point reside ast
                    else
                    {
                        s1_new = s1 + smallTruckDumping;
                        s2_new = s2 + smallTruckDumping;

                        priorityObject.GetComponent<Returning_1>().ChangeStartMessageToTrue();

                        startTime = timer_2;
                        flag3 = true;

                    }


                }
            }








            //follow path
            if (!finish && flag3 == true)
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

                if (!clash)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Convert.ToSingle(path[number].x),
                                                             transform.position.y, Convert.ToSingle(path[number].y)), Time.deltaTime * speed);
                }


                if (flag_false == true && showMsg2 == true)
                {
                    EditorUtility.DisplayDialog("Warning!", "There is no way to solve this solution, so please change the starting and ending points of objects!", "ok");
                    showMsg2 = false;
                }

                else if (flag1 == true && t2 < s1_new && t2 < s2_new)
                {
                    if (timer <= t1 + 0.02f && timer > t1 - 0.04f)
                    {
                        clash = true;
                        PlusMsg = true;
                    }

                    if (timer >= s2_new && timer < s2_new + 0.04f)
                    {
                        clash = false;
                    }
                }

                else if (flag2 == true && t1 > s1_new && t1 > s2_new && showMsg2 == true)
                {
                    EditorUtility.DisplayDialog("Warning!", "There is no way to solve this solution, so please change the starting and ending points of objects!", "ok");
                    showMsg2 = false;
                }

                else if (t1 > s1_new && t1 < s2_new && t2 > s2_new)
                {
                    if (flag2 == true && showMsg2 == true)
                    {
                        EditorUtility.DisplayDialog("Warning!", "There is no way to solve this solution, so please change the starting and ending points of objects!", "ok");
                        showMsg2 = false;
                    }

                    else
                    {
                        if (timer <= t1 + 0.02f && timer > t1 - 0.04f)
                        {
                            clash = true;
                            PlusMsg = true;
                        }

                        if (timer >= s2_new && timer < s2_new + 0.04f)
                        {
                            clash = false;
                        }

                    }
                }

                else if (t1 < s1_new && t2 > s2_new)
                {
                    if (flag2 == true && showMsg2 == true)
                    {
                        EditorUtility.DisplayDialog("Warning!", "There is no way to solve this solution, so please change the starting and ending points of objects!", "ok");
                        showMsg2 = false;
                    }

                    else
                    {
                        if (timer <= t1 + 0.02f && timer > t1 - 0.04f)
                        {
                            clash = true;
                            PlusMsg = true;
                        }

                        if (timer >= s2_new && timer < s2_new + 0.04f)
                        {
                            clash = false;
                        }
                    }
                }

                else if (t2 < s2_new && t2 > s1_new && t1 < s1_new)
                {
                    if (flag2 == true && showMsg2 == true)
                    {
                        EditorUtility.DisplayDialog("Warning!", "There is no way to solve this solution, so please change the starting and ending points of objects!", "ok");
                        showMsg2 = false;
                    }

                    else
                    {
                        if (timer <= t1 + 0.02f && timer > t1 - 0.04f)
                        {
                            clash = true;
                            PlusMsg = true;
                        }

                        if (timer >= s2_new && timer < s2_new + 0.04f)
                        {
                            clash = false;
                        }
                    }
                }

                else if (t2 < s2_new && t2 > s1_new && t1 > s1_new)
                {
                    if (flag2 == true && showMsg2 == true)
                    {
                        EditorUtility.DisplayDialog("Warning!", "There is no way to solve this solution, so please change the starting and ending points of objects!", "ok");
                        showMsg2 = false;
                    }

                    else
                    {
                        if (timer <= t1 + 0.02f && timer > t1 - 0.04f)
                        {
                            clash = true;
                            PlusMsg = true;
                        }

                        if (timer >= s2_new && timer < s2_new + 0.04f)
                        {
                            clash = false;
                        }
                    }
                }





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

    public float GetStartTime()
    {
        return startTime;
    }

    public bool WarningMessage()
    {
        return Warning;
    }

    public bool PlusMessage()
    {
        return PlusMsg;
    }

    public float Get_t1()
    {
        return t1;
    }

    public float Get_s2_new()
    {
        return s2_new;
    }

    public Vector3 GetIntersectionPoint()
    {
        return intersectionPoint;
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


    //it finds an in-active GameObject by Tag:
    public GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

}
