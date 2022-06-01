using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
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

    private LineRenderer lineRenderer;
    public float speed = 5.0f;
    public GameObject dot;
    private int number;

    private bool finish = false;
    private float timer = 0;

    //clash detail
    public GameObject priorityObject;

    private bool clash = false;

    private float x_ColliderDiameter;
    private float z_ColliderDiameter;
    private float x_ColliderDiameter_1;
    private float z_ColliderDiameter_1;

    private Vector3 direction;
    private Vector3 v;
    private float angle;
    private Vector3 vector_base;
    private Vector3 normalizeee_base;
    private Vector3 Cross;

    private Transform trans;
    private Transform trans_1;
    private Vector3 min;
    private Vector3 max;
    private Vector3 min_1;
    private Vector3 max_1;
    private Vector3 boxCollider;
    private Vector3 boxCollider_1;

    private Vector3 vector;
    private Vector3 normalize;
    private float gamma;
    private float beta;
    private float alpha;
    private float a;
    private float b;
    private float c;

    private float t1;
    private float t2;
    private float s1;
    private float s2;

    private bool rightToLeft = false;
    private bool leftToRight = false;

    void Start()
    {
        start_Pos_x = GetComponent<Transform>().position.x;
        start_Pos_z = GetComponent<Transform>().position.z;

        x_Diameter = GetComponent<Collider>().bounds.size.x;
        z_Diameter = GetComponent<Collider>().bounds.size.z;
        diameter = new double[] { x_Diameter / GetComponent<BoxCollider>().size.x,
                                  z_Diameter / GetComponent<BoxCollider>().size.z};

        x_ColliderDiameter = GetComponent<Collider>().bounds.size.x;
        z_ColliderDiameter = GetComponent<Collider>().bounds.size.z;
        x_ColliderDiameter_1 = priorityObject.GetComponent<Collider>().bounds.size.x;
        z_ColliderDiameter_1 = priorityObject.GetComponent<Collider>().bounds.size.z;

        obstacle = GameObject.FindGameObjectsWithTag("Obstacle");

        obstacleList = new double[,] { { obstacle[0].transform.position.x, obstacle[0].transform.position.z, obstacle[0].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[1].transform.position.x, obstacle[1].transform.position.z, obstacle[1].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[2].transform.position.x, obstacle[2].transform.position.z, obstacle[2].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[3].transform.position.x, obstacle[3].transform.position.z, obstacle[3].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[4].transform.position.x, obstacle[4].transform.position.z, obstacle[4].GetComponent<Collider>().bounds.size.x / 2.0 },
                                       { obstacle[5].transform.position.x, obstacle[5].transform.position.z, obstacle[5].GetComponent<Collider>().bounds.size.x / 2.0 }
        };


        InformedRRTStar rrt = new InformedRRTStar(new double[] { start_Pos_x, start_Pos_z },
                     new double[] { goal_Pos_x, goal_Pos_z }, diameter, obstacleList,
                     new double[] { plane.transform.position.x - (plane.GetComponent<Collider>().bounds.size.x / 2.0), plane.transform.position.z + (plane.GetComponent<Collider>().bounds.size.z / 2.0) });

        path = rrt.informed_rrt_star_search();

        string str = "Path for BLUE one : ";
        for (int i = path.Count - 1; i >= 0; i--)
        {
            str += $"({path[i].x:F3}, {path[i].y:F3}), ";
        }
        print(str);

        print($"Distance between start to end for BLUE one: {InformedRRTStar.get_path_len(path):F3}");


        // Draw Line between nodes
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(Convert.ToSingle(path[i].x), 0.5f, Convert.ToSingle(path[i].y)));
        }

        // Draw Nodes
        for (int i = 0; i < path.Count; i++)
        {
            Instantiate(dot, new Vector3(Convert.ToSingle(path[i].x), 0.5f, Convert.ToSingle(path[i].y)), Quaternion.identity);
        }

        //vertices = GetComponent<MeshFilter>().mesh.vertices;
        //vertices_1 = priorityObject.GetComponent<MeshFilter>().mesh.vertices;

        trans = GetComponent<BoxCollider>().transform;
        min = GetComponent<BoxCollider>().center - GetComponent<BoxCollider>().size * 0.5f;
        max = GetComponent<BoxCollider>().center + GetComponent<BoxCollider>().size * 0.5f;

        trans_1 = priorityObject.GetComponent<BoxCollider>().transform;
        min_1 = priorityObject.GetComponent<BoxCollider>().center - priorityObject.GetComponent<BoxCollider>().size * 0.5f;
        max_1 = priorityObject.GetComponent<BoxCollider>().center + priorityObject.GetComponent<BoxCollider>().size * 0.5f;

        number = path.Count - 2;

    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        //follow path
        if (!finish)
        {
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


            //Clash detection
            vector_base = new Vector3(priorityObject.transform.position.x - transform.position.x,
                                      priorityObject.transform.position.y - transform.position.y,
                                      priorityObject.transform.position.z - transform.position.z);

            normalizeee_base = Vector3.Normalize(vector_base);

            Cross = Vector3.Cross(normalizeee_base, direction);
            //print(Cross.y);

            if (Cross.y < 0) //from Right to Left
            {
                rightToLeft = true;

                boxCollider = trans.TransformPoint(new Vector3(max.x, max.y, min.z));            //P110
                boxCollider_1 = trans_1.TransformPoint(new Vector3(max_1.x, max_1.y, max_1.z));  //P111

                vector = new Vector3(boxCollider_1.x - boxCollider.x,
                                     boxCollider_1.y - boxCollider.y,
                                     boxCollider_1.z - boxCollider.z);

                normalize = Vector3.Normalize(vector);

                gamma = Mathf.Abs(Vector3.Angle(normalize, direction));
                beta = Mathf.Abs(Vector3.Angle(normalize * -1, priorityObject.GetComponent<GameManager_1>().GetDirection()));
                alpha = 180 - (gamma + beta);

                a = vector.magnitude;
                b = (Mathf.Sin(beta * Mathf.PI / 180) / Mathf.Sin(alpha * Mathf.PI / 180)) * a;
                c = (Mathf.Sin(gamma * Mathf.PI / 180) / Mathf.Sin(alpha * Mathf.PI / 180)) * a;

                //Debug.DrawRay(trans.TransformPoint(new Vector3(max.x, max.y, min.z)), Vector3.Normalize(vector) * a, Color.red);
                //Debug.DrawRay(trans.TransformPoint(new Vector3(max.x, max.y, min.z)), Vector3.Normalize(GetDirection()) * b, Color.blue);
                //Debug.DrawRay(trans_1.TransformPoint(new Vector3(max_1.x, max_1.y, max_1.z)), Vector3.Normalize(priorityObject.GetComponent<GameManager_1>().GetDirection()) * c, Color.white);

                if (alpha > 90)
                {
                    Vector3 boxCollider_new = trans.TransformPoint(new Vector3(max.x, max.y, max.z));  //P111
                    boxCollider_1 = trans_1.TransformPoint(new Vector3(max_1.x, max_1.y, max_1.z));    //P111

                    Vector3 vector_new = new Vector3(boxCollider_1.x - boxCollider_new.x,
                                                     boxCollider_1.y - boxCollider_new.y,
                                                     boxCollider_1.z - boxCollider_new.z);

                    Vector3 normalize_new = Vector3.Normalize(vector_new);
                    float gamma_new = Mathf.Abs(Vector3.Angle(normalize_new, direction));
                    float beta_new = Mathf.Abs(Vector3.Angle(normalize_new * -1, priorityObject.GetComponent<GameManager_1>().GetDirection()));
                    float alpha_new = 180 - (gamma_new + beta_new);

                    float a_new = vector_new.magnitude;
                    float b_new = (Mathf.Sin(beta_new * Mathf.PI / 180) / Mathf.Sin(alpha_new * Mathf.PI / 180)) * a_new;
                    float c_new = (Mathf.Sin(gamma_new * Mathf.PI / 180) / Mathf.Sin(alpha_new * Mathf.PI / 180)) * a_new;

                    if ((Mathf.Sin(gamma_new * Mathf.PI / 180) / Mathf.Sin(beta_new * Mathf.PI / 180)) > (priorityObject.GetComponent<GameManager_1>().speed / speed) && timer > 0.1f)
                    {
                        if (b_new > 0 && b_new < 0.1f)
                        {
                            s1 = (c - (Mathf.Tan((alpha - 90) * Mathf.PI / 180) * z_ColliderDiameter_1)) / priorityObject.GetComponent<GameManager_1>().speed;
                            s2 = (x_ColliderDiameter + b + (z_ColliderDiameter_1 / Mathf.Cos((alpha - 90) * Mathf.PI / 180))) / speed;
                        }

                        if (s1 < s2)
                        {
                            clash = true;
                        }

                        else
                        {
                            clash = false;
                        }

                    }
                }

                else if (alpha < 90)
                {
                    if ((Mathf.Sin(gamma * Mathf.PI / 180) / Mathf.Sin(beta * Mathf.PI / 180)) > (priorityObject.GetComponent<GameManager_1>().speed / speed) && timer > 0.1f)
                    {
                        if (b > 0 && b < 0.1f)
                        {
                            t1 = (c + (z_ColliderDiameter_1 / Mathf.Tan(alpha * Mathf.PI / 180))) / priorityObject.GetComponent<GameManager_1>().speed;
                            t2 = (x_ColliderDiameter + b + (z_ColliderDiameter_1 / Mathf.Sin(alpha * Mathf.PI / 180))) / speed;
                        }

                        if (s1 < s2)
                        {
                            clash = true;
                        }

                        else
                        {
                            clash = false;
                        }

                    }


                }

                else if (alpha == 90)
                {
                    if ((Mathf.Sin(gamma * Mathf.PI / 180) / Mathf.Sin(beta * Mathf.PI / 180)) > (priorityObject.GetComponent<GameManager_1>().speed / speed) && timer > 0.1f)
                    {
                        if (b > 0 && b < 0.1f)
                        {
                            s1 = (c + (z_ColliderDiameter_1 / Mathf.Tan(alpha * Mathf.PI / 180))) / priorityObject.GetComponent<GameManager_1>().speed;
                            s2 = (x_ColliderDiameter + b + (z_ColliderDiameter_1 / Mathf.Sin(alpha * Mathf.PI / 180))) / speed;
                        }

                        if (s1 < s2)
                        {
                            clash = true;
                        }

                        else
                        {
                            clash = false;
                        }

                    }

                }


                if (c < 0 && leftToRight == true)
                {
                    clash = false;
                }
            }


            else if (Cross.y >= 0) //from Left to Right
            {
                leftToRight = true;

                boxCollider = trans.TransformPoint(new Vector3(max.x, max.y, max.z));              //P111
                boxCollider_1 = trans_1.TransformPoint(new Vector3(max_1.x, max_1.y, min_1.z));    //P110    

                vector = new Vector3(boxCollider_1.x - boxCollider.x,
                                     boxCollider_1.y - boxCollider.y,
                                     boxCollider_1.z - boxCollider.z);

                normalize = Vector3.Normalize(vector);
                gamma = Mathf.Abs(Vector3.Angle(normalize, direction));
                beta = Mathf.Abs(Vector3.Angle(normalize * -1, priorityObject.GetComponent<GameManager_1>().GetDirection()));
                alpha = 180 - (gamma + beta);

                a = vector.magnitude;
                b = (Mathf.Sin(beta * Mathf.PI / 180) / Mathf.Sin(alpha * Mathf.PI / 180)) * a;
                c = (Mathf.Sin(gamma * Mathf.PI / 180) / Mathf.Sin(alpha * Mathf.PI / 180)) * a;

                Debug.DrawRay(trans.TransformPoint(new Vector3(max.x, max.y, max.z)), Vector3.Normalize(vector) * a, Color.red);
                Debug.DrawRay(trans.TransformPoint(new Vector3(max.x, max.y, max.z)), Vector3.Normalize(GetDirection()) * b, Color.blue);
                Debug.DrawRay(trans_1.TransformPoint(new Vector3(max_1.x, max_1.y, min_1.z)), Vector3.Normalize(priorityObject.GetComponent<GameManager_1>().GetDirection()) * c, Color.green);


                if (alpha > 90)
                {
                    Vector3 boxCollider_new = trans.TransformPoint(new Vector3(max.x, max.y, min.z));  //P110
                    boxCollider_1 = trans_1.TransformPoint(new Vector3(max_1.x, max_1.y, min_1.z));    //P110    

                    Vector3 vector_new = new Vector3(boxCollider_1.x - boxCollider_new.x,
                                                     boxCollider_1.y - boxCollider_new.y,
                                                     boxCollider_1.z - boxCollider_new.z);

                    Vector3 normalize_new = Vector3.Normalize(vector_new);
                    float gamma_new = Mathf.Abs(Vector3.Angle(normalize_new, direction));
                    float beta_new = Mathf.Abs(Vector3.Angle(normalize_new * -1, priorityObject.GetComponent<GameManager_1>().GetDirection()));
                    float alpha_new = 180 - (gamma_new + beta_new);

                    float a_new = vector_new.magnitude;
                    float b_new = (Mathf.Sin(beta_new * Mathf.PI / 180) / Mathf.Sin(alpha_new * Mathf.PI / 180)) * a_new;
                    float c_new = (Mathf.Sin(gamma_new * Mathf.PI / 180) / Mathf.Sin(alpha_new * Mathf.PI / 180)) * a_new;

                    if ((Mathf.Sin(gamma_new * Mathf.PI / 180) / Mathf.Sin(beta_new * Mathf.PI / 180)) > (priorityObject.GetComponent<GameManager_1>().speed / speed) && timer > 0.1f)
                    {
                        if (b_new > 0 && b_new < 0.1f)
                        {
                            t1 = (c - (Mathf.Tan((alpha - 90) * Mathf.PI / 180) * z_ColliderDiameter_1)) / priorityObject.GetComponent<GameManager_1>().speed;
                            t2 = (x_ColliderDiameter + b + (z_ColliderDiameter_1 / Mathf.Cos((alpha - 90) * Mathf.PI / 180))) / speed;
                        }

                        if (t1 < t2)
                        {
                            clash = true;
                        }

                        else
                        {
                            clash = false;
                        }

                    }
                }

                else if (alpha < 90)
                {
                    if ((Mathf.Sin(gamma * Mathf.PI / 180) / Mathf.Sin(beta * Mathf.PI / 180)) > (priorityObject.GetComponent<GameManager_1>().speed / speed) && timer > 0.1f)
                    {
                        if (b > 0 && b < 0.1f)
                        {
                            t1 = (c + (z_ColliderDiameter_1 / Mathf.Tan(alpha * Mathf.PI / 180))) / priorityObject.GetComponent<GameManager_1>().speed;
                            t2 = (x_ColliderDiameter + b + (z_ColliderDiameter_1 / Mathf.Sin(alpha * Mathf.PI / 180))) / speed;
                        }

                        if (t1 < t2)
                        {
                            clash = true;
                        }

                        else
                        {
                            clash = false;
                        }

                    }


                }

                else if (alpha == 90)
                {
                    if ((Mathf.Sin(gamma * Mathf.PI / 180) / Mathf.Sin(beta * Mathf.PI / 180)) > (priorityObject.GetComponent<GameManager_1>().speed / speed) && timer > 0.1f)
                    {
                        if (b > 0 && b < 0.1f)
                        {
                            t1 = (c + (z_ColliderDiameter_1 / Mathf.Tan(alpha * Mathf.PI / 180))) / priorityObject.GetComponent<GameManager_1>().speed;
                            t2 = (x_ColliderDiameter + b + (z_ColliderDiameter_1 / Mathf.Sin(alpha * Mathf.PI / 180))) / speed;
                        }

                        if (t1 < t2)
                        {
                            clash = true;
                        }

                        else
                        {
                            clash = false;
                        }

                    }

                }


                if (c < 0 && rightToLeft == true)
                {
                    clash = false;
                }


            }

            if (dist <= 0.1f && number > 0)
            {
                number--;
            }

            else if (number == 0)
            {
                EditorUtility.DisplayDialog("Result (BLUE Cube)", $"Time is: {timer:F3}", "ok");
                finish = true;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            clash = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            clash = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            clash = false;
        }
    }

    public Vector3 GetDirection()
    {
        return direction;
    }
}
