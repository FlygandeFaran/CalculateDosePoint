using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI_GUI
{
    public class Point
    {
        private string planID;
        private double _x;
        private double _y;
        private double _z;
        private double pointDose;
        private List<double> listOfFieldPointDose;
        private List<string> listOfFieldIDs;

        public string PlanID
        {
            get { return planID; }
            set { planID = value; }
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }
        public double PointDose
        {
            get { return pointDose; }
            set { pointDose = value; }
        }
        public List<double> ListOfFieldPointDose
        {
            get { return listOfFieldPointDose; }
            set { listOfFieldPointDose = value; }
        }
        public List<string> ListOfFieldIDs
        {
            get { return listOfFieldIDs; }
            set { listOfFieldIDs = value; }
        }
        public Point()
        {
            listOfFieldPointDose = new List<double>();
            listOfFieldIDs = new List<string>();
        }
        public void AddFieldPointDose(double point)
        {
            listOfFieldPointDose.Add(point);
        }
        public void AddFieldIDs(string fieldID)
        {
            listOfFieldIDs.Add(fieldID);
        }

        public string Output(int i)
        {
            string strOut = string.Format("{0, -13} {1, 18} {2, 9} {3, 5} {4, 5} {5, 13} {6, 12}", planID, listOfFieldIDs.ElementAt(i), _x, _y, _z, pointDose.ToString("0.00"), listOfFieldPointDose.ElementAt(i).ToString("0.00"));
            //string strOut = string.Format("{0, 5} {1, 35} {2, 30} {3, 30}", "1", "+1", "-2", "±3");
            return strOut;
        }
    }
}
