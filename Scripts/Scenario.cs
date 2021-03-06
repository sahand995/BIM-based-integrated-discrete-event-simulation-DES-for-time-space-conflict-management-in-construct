using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simphony.Simulation;


namespace Assets.Scripts
{
    public class Scenario : DiscreteEventScenario
    {
        DiscreteEventEngine MyEngine;


        //Resource
        private Resource Loader;
        private Resource Spotter;

        private int LoaderCount;
        private int SpotterCount;

        private WaitingFile FileLoader = new WaitingFile("FileLoader", false);
        private WaitingFile FileSpotter = new WaitingFile("FileSpotter", false);


        private NumericStatistic LargeNumericStatistic;
        private NumericStatistic SmallNumericStatistic;
        public IReadOnlyList<double> ObservationForLargeTrucks;
        public IReadOnlyList<double> ObservationForSmallTrucks;
        public double Mean;


        //Duration
        public double LargeTruckHaulingToLoader;
        public double SmallTruckHaulingToLoader;
        private double LargeTruckLoading = 3;
        private double SmallTruckLoading = 2;
        public double LargeTruckHaulingToSpotter;
        public double SmallTruckHaulingToSpotter;
        private double LargeTruckDumping = 5;
        private double SmallTruckDumping = 3;
        public double LargeTruckHaulingToStart;
        public double SmallTruckHaulingToStart;



        #region [- Ctor -]
        public Scenario(DiscreteEventEngine myEngine, int numLoaders, int numSpotters)
        {
            MyEngine = myEngine;
            LoaderCount = numLoaders;
            SpotterCount = numSpotters;
        }
        #endregion

        public override int InitializeScenario()
        {
            Loader = new Resource("Loader", LoaderCount);
            Spotter = new Resource("Spotter", SpotterCount);
            Loader.WaitingFiles.Add(FileLoader);
            Spotter.WaitingFiles.Add(FileSpotter);
            LargeNumericStatistic = new NumericStatistic("LargeCT", false);
            SmallNumericStatistic = new NumericStatistic("SmallCT", false);

            return 1;
        }


        public override double InitializeRun(int runIndex)
        {
            Loader.InitializeRun(runIndex);
            Spotter.InitializeRun(runIndex);
            FileLoader.InitializeRun(runIndex);
            FileSpotter.InitializeRun(runIndex);
            LargeNumericStatistic.InitializeRun(runIndex);
            SmallNumericStatistic.InitializeRun(runIndex);

            for (int i = 0; i < 2; i++)
            {
                Truck MyTruck = new Truck();

                if (i % 2 == 0)
                {
                    MyTruck.Type = "Large";
                    MyEngine.ScheduleEvent(MyTruck, RequestLoader, LargeTruckHaulingToLoader);
                }

                else
                {
                    MyTruck.Type = "Small";
                    MyEngine.ScheduleEvent(MyTruck, RequestLoader, SmallTruckHaulingToLoader);
                }

                MyTruck.StartTime = MyEngine.TimeNow;

            }


            return double.PositiveInfinity;
        }


        private void RequestLoader(Truck myTruck)
        {
            MyEngine.RequestResource(myTruck, Loader, 1, Loading, FileLoader);
        }

        private void Loading(Truck myTruck)
        {
            if (myTruck.Type == "Large")
            {
                MyEngine.ScheduleEvent(myTruck, ReleaseLoaderAndHauling, LargeTruckLoading);
            }

            else
            {
                MyEngine.ScheduleEvent(myTruck, ReleaseLoaderAndHauling, SmallTruckLoading);
            }
        }


        private void ReleaseLoaderAndHauling(Truck myTruck)
        {
            MyEngine.ReleaseResource(myTruck, Loader, 1);


            //Hauling
            if (myTruck.Type == "Large")
            {
                MyEngine.ScheduleEvent(myTruck, RequestSpotter, LargeTruckHaulingToSpotter);
            }

            else
            {
                MyEngine.ScheduleEvent(myTruck, RequestSpotter, SmallTruckHaulingToSpotter);
            }
        }


        private void RequestSpotter(Truck myTruck)
        {
            MyEngine.RequestResource(myTruck, Spotter, 1, Dumping, FileSpotter);
        }


        private void Dumping(Truck myTruck)
        {
            if (myTruck.Type == "Large")
            {
                MyEngine.ScheduleEvent(myTruck, ReleaseSpotter, LargeTruckDumping);
            }

            else
            {
                MyEngine.ScheduleEvent(myTruck, ReleaseSpotter, SmallTruckDumping);
            }
        }

        private void ReleaseSpotter(Truck myTruck)
        {
            MyEngine.ReleaseResource(myTruck, Spotter, 1);

            //Returning
            if (myTruck.Type == "Large")
            {
                MyEngine.ScheduleEvent(myTruck, ReturnToStart, LargeTruckHaulingToStart);
            }

            else
            {
                MyEngine.ScheduleEvent(myTruck, ReturnToStart, SmallTruckHaulingToStart);
            }

        }

        private void ReturnToStart (Truck myTruck)
        {
            if (myTruck.Type == "Large")
            {
                MyEngine.CollectStatistic(LargeNumericStatistic, MyEngine.TimeNow - myTruck.StartTime);
            }

            else
            {
                MyEngine.CollectStatistic(SmallNumericStatistic, MyEngine.TimeNow - myTruck.StartTime);
            }
        }


        public override void FinalizeRun(int runIndex)
        {


        }

        public override void FinalizeScenario()
        {
            ObservationForLargeTrucks = LargeNumericStatistic.Times;
            ObservationForSmallTrucks = SmallNumericStatistic.Times;

            Mean = (LargeNumericStatistic.Sum + SmallNumericStatistic.Sum) / (LargeNumericStatistic.Count + SmallNumericStatistic.Count);
        }


        public double GetLargeTruckLoading()
        {
            return LargeTruckLoading;
        }

        public double GetSmallTruckLoading()
        {
            return SmallTruckLoading;
        }

        public double GetLargeTruckDumping()
        {
            return LargeTruckDumping;
        }

        public double GetSmallTruckDumping()
        {
            return SmallTruckDumping;
        }

    }
}
