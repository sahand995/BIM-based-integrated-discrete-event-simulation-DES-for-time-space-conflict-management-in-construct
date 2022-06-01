using Assets.Scripts;
using Simphony.Simulation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Program : MonoBehaviour
{
    DiscreteEventEngine MyEngine = new DiscreteEventEngine();
    public GameObject cube;
    public GameObject cube_1;
    private bool flag1 = true;
    private bool flag2 = true;
    private bool flag3 = true;
    private bool flag4 = true;

    private GameObject canvas1;
    private GameObject canvas2;
    private GameObject canvas3;
    private GameObject canvas7;

    void Start()
    {
        canvas1 = GameObject.FindGameObjectWithTag("Canvas1");
        canvas2 = GameObject.FindGameObjectWithTag("Canvas2");
        canvas3 = GameObject.FindGameObjectWithTag("Canvas3");
        canvas7 = GameObject.FindGameObjectWithTag("Canvas7");

    }


    void Update()
    {
        Model MyModel = new Model(MyEngine);

        if (cube.GetComponent<Loading>().FinishiMessage() == true && cube_1.GetComponent<Loading_1>().FinishiMessage() == true &&
            cube.GetComponent<Hauling>().FinishiMessage() == true && cube_1.GetComponent<Hauling_1>().FinishiMessage() == true &&
            cube.GetComponent<Returning>().FinishiMessage() == true && cube_1.GetComponent<Returning_1>().FinishiMessage() == true && flag1 == true)
        {

            #region [- Time calculating From Start point to Loading point -]
            if (cube.GetComponent<Loading>().WarningMessage() == false && cube.GetComponent<Loading>().PlusMessage() == true)
            {

                canvas1.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Starting point to Loading point:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Loading>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Loading_1>().ReachTime():F3} min\n " +
                                                                     $"\nThe Blue cube must stop at time = {cube.GetComponent<Loading>().Get_t1():F3} min and start moving at time = {cube.GetComponent<Loading>().Get_s2():F3} min \n" +
                                                                     $"\nIntersection point: ({cube.GetComponent<Loading>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().z:F3})";

                EditorUtility.DisplayDialog("From Start point to Loading point", $"Duration for Blue Cube: {cube.GetComponent<Loading>().ReachTime():F3} min\n" +
                                                                                 $"Duration for Red Cube: {cube_1.GetComponent<Loading_1>().ReachTime():F3} min\n " +
                                                                                 $"\nThe Blue cube must stop at time = {cube.GetComponent<Loading>().Get_t1():F3} min and start moving at time = {cube.GetComponent<Loading>().Get_s2():F3} min \n" +
                                                                                 $"\nIntersection point: ({cube.GetComponent<Loading>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().z:F3})", "ok");

            }

            else if (cube.GetComponent<Loading>().WarningMessage() == false && cube.GetComponent<Loading>().PlusMessage() == false)
            {
                canvas1.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Starting point to Loading point:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Loading>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Loading_1>().ReachTime():F3} min\n " +
                                                                     $"\nIntersection point: ({cube.GetComponent<Loading>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().z:F3})";

                EditorUtility.DisplayDialog("From Start point to Loading point", $"Duration for Blue Cube: {cube.GetComponent<Loading>().ReachTime():F3} min\n" +
                                                                                 $"Duration for Red Cube: {cube_1.GetComponent<Loading_1>().ReachTime():F3} min\n " +
                                                                                 $"\nIntersection point: ({cube.GetComponent<Loading>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Loading>().GetIntersectionPoint().z:F3})", "ok");

            }

            else if (cube.GetComponent<Loading>().WarningMessage() == true)
            {
                canvas1.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Starting point to Loading point:</color></b>\n" +
                                                                      $"\nDuration for Blue Cube: {cube.GetComponent<Loading>().ReachTime():F3} min\n" +
                                                                      $"Duration for Red Cube: {cube_1.GetComponent<Loading_1>().ReachTime():F3} min\n";

                EditorUtility.DisplayDialog("From Start point to Loading point", $"Duration for Blue Cube: {cube.GetComponent<Loading>().ReachTime():F3} min\n" +
                                                                                 $"Duration for Red Cube: {cube_1.GetComponent<Loading_1>().ReachTime():F3} min\n ", "ok");

            }


            #endregion

            flag1 = false;
        }


        if (flag1 == false && flag2 == true)
        {

            #region [- Time calculating From Loading point to Dump Site -]
            if (cube.GetComponent<Hauling>().WarningMessage() == false && cube.GetComponent<Hauling>().PlusMessage() == true)
            {
                canvas2.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Loading point to Dump Site:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Hauling>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Hauling_1>().ReachTime():F3} min\n " +
                                                                     $"\nThe Blue cube must stop at time = {cube.GetComponent<Hauling>().Get_t1() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().GetStartTime():F3} min and start moving at time = {cube.GetComponent<Hauling>().Get_s2_new() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().GetStartTime():F3} min \n" +
                                                                     $"\nIntersection point: ({cube.GetComponent<Hauling>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().z:F3})";

                EditorUtility.DisplayDialog("From Loading point to Dump Site", $"Duration for Blue Cube: {cube.GetComponent<Hauling>().ReachTime():F3} min\n" +
                                                                               $"Duration for Red Cube: {cube_1.GetComponent<Hauling_1>().ReachTime():F3} min\n " +
                                                                               $"\nThe Blue cube must stop at time = {cube.GetComponent<Hauling>().Get_t1() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().GetStartTime():F3} min and start moving at time = {cube.GetComponent<Hauling>().Get_s2_new() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().GetStartTime():F3} min \n" +
                                                                               $"\nIntersection point: ({cube.GetComponent<Hauling>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().z:F3})", "ok");

            }

            else if (cube.GetComponent<Hauling>().WarningMessage() == false && cube.GetComponent<Hauling>().PlusMessage() == false)
            {
                canvas2.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Loading point to Dump Site:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Hauling>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Hauling_1>().ReachTime():F3} min\n " +
                                                                     $"\nIntersection point: ({cube.GetComponent<Hauling>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().z:F3})";

                EditorUtility.DisplayDialog("From Loading point to Dump Site ", $"Duration for Blue Cube: {cube.GetComponent<Hauling>().ReachTime():F3} min\n" +
                                                                                $"Duration for Red Cube: {cube_1.GetComponent<Hauling_1>().ReachTime():F3} min\n " +
                                                                                $"\nIntersection point: ({cube.GetComponent<Hauling>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Hauling>().GetIntersectionPoint().z:F3})", "ok");

            }

            else if (cube.GetComponent<Hauling>().WarningMessage() == true)
            {
                canvas2.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Loading point to Dump Site:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Hauling>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Hauling_1>().ReachTime():F3} min\n ";

                EditorUtility.DisplayDialog("From Loading point to Dump Site", $"Duration for Blue Cube: {cube.GetComponent<Hauling>().ReachTime():F3} min\n" +
                                                                               $"Duration for Red Cube: {cube_1.GetComponent<Hauling_1>().ReachTime():F3} min\n ", "ok");
            }

            #endregion

            flag2 = false;
        }

        if (flag2 == false && flag3 == true)
        {

            #region [- Time calculating From Dump Site to Starting Point -]
            if (cube.GetComponent<Returning>().WarningMessage() == false && cube.GetComponent<Returning>().PlusMessage() == true)
            {
                canvas3.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Dump Site to Starting Point:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Returning>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Returning_1>().ReachTime():F3} min\n " +
                                                                     $"\nThe Blue cube must stop at time = {cube.GetComponent<Returning>().Get_t1() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().ReachTime() + cube.GetComponent<Returning>().GetStartTime():F3} min and start moving at time = {cube.GetComponent<Returning>().Get_s2_new() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().ReachTime() + cube.GetComponent<Returning>().GetStartTime():F3} min \n" +
                                                                     $"\nIntersection point: ({cube.GetComponent<Returning>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().z:F3})";

                EditorUtility.DisplayDialog("From Dump Site to Starting Point", $"Duration for Blue Cube: {cube.GetComponent<Returning>().ReachTime():F3} min\n" +
                                                                               $"Duration for Red Cube: {cube_1.GetComponent<Returning_1>().ReachTime():F3} min\n " +
                                                                               $"\nThe Blue cube must stop at time = {cube.GetComponent<Returning>().Get_t1() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().ReachTime() +cube.GetComponent<Returning>().GetStartTime():F3} min and start moving at time = {cube.GetComponent<Returning>().Get_s2_new() + cube.GetComponent<Loading>().ReachTime() + cube.GetComponent<Hauling>().ReachTime() + cube.GetComponent<Returning>().GetStartTime():F3} min \n" +
                                                                               $"\nIntersection point: ({cube.GetComponent<Returning>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().z:F3})", "ok");

            }

            else if (cube.GetComponent<Returning>().WarningMessage() == false && cube.GetComponent<Returning>().PlusMessage() == false)
            {
                canvas3.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Dump Site to Starting Point:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Returning>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Returning_1>().ReachTime():F3} min\n " +
                                                                     $"\nIntersection point: ({cube.GetComponent<Returning>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().z:F3})";

                EditorUtility.DisplayDialog("From Dump Site to Starting Point", $"Duration for Blue Cube: {cube.GetComponent<Returning>().ReachTime():F3} min\n" +
                                                                                $"Duration for Red Cube: {cube_1.GetComponent<Returning_1>().ReachTime():F3} min\n " +
                                                                                $"\nIntersection point: ({cube.GetComponent<Returning>().GetIntersectionPoint().x:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().y:F3}, {cube.GetComponent<Returning>().GetIntersectionPoint().z:F3})", "ok");

            }

            else if (cube.GetComponent<Returning>().WarningMessage() == true)
            {
                canvas3.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>From Dump Site to Starting Point:</color></b>\n" +
                                                                     $"\nDuration for Blue Cube: {cube.GetComponent<Returning>().ReachTime():F3} min\n" +
                                                                     $"Duration for Red Cube: {cube_1.GetComponent<Returning_1>().ReachTime():F3} min\n ";

                EditorUtility.DisplayDialog("From Dump Site to Starting Point", $"Duration for Blue Cube: {cube.GetComponent<Returning>().ReachTime():F3} min\n" +
                                                                               $"Duration for Red Cube: {cube_1.GetComponent<Returning_1>().ReachTime():F3} min\n ", "ok");
            }

            #endregion

            flag3 = false;
        }




        if (flag2 == false && flag3 == false && flag4 == true)
        {
            MyModel.MyScenario.LargeTruckHaulingToLoader = cube.GetComponent<Loading>().ReachTime();
            MyModel.MyScenario.SmallTruckHaulingToLoader = cube_1.GetComponent<Loading_1>().ReachTime();
            MyModel.MyScenario.LargeTruckHaulingToSpotter = cube.GetComponent<Hauling>().ReachTime();
            MyModel.MyScenario.SmallTruckHaulingToSpotter = cube_1.GetComponent<Hauling_1>().ReachTime();
            MyModel.MyScenario.LargeTruckHaulingToStart = cube.GetComponent<Returning>().ReachTime();
            MyModel.MyScenario.SmallTruckHaulingToStart = cube_1.GetComponent<Returning_1>().ReachTime();



            //Initializations
            MyEngine.InitializeEngine();
            MyEngine.Simulate(MyModel);

            canvas7.GetComponent<TMPro.TextMeshProUGUI>().text = "<b><color=yellow>Result:</color></b>\n" +
                                                                 $"\nBlue Cube: \n" +
                                                                 $"1 = { MyModel.MyScenario.ObservationForLargeTrucks[0]:F3} min\n" +
                                                                 $"\nRed Cube: \n" +
                                                                 $"1 = { MyModel.MyScenario.ObservationForSmallTrucks[0]:F3} min\n" +
                                                                 $"\nMean = { MyModel.MyScenario.Mean:F3}";

            EditorUtility.DisplayDialog("Result", $"Blue Truck: \n" +
                                                  $"1 = { MyModel.MyScenario.ObservationForLargeTrucks[0]:F3} min\n" +
                                                  $"\nRed Truck: \n" +
                                                  $"1 = { MyModel.MyScenario.ObservationForSmallTrucks[0]:F3} min\n" +
                                                  $"\nMean = { MyModel.MyScenario.Mean:F3}", "ok");


            flag4 = false;
        }


    }
}
