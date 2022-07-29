using System;
using System.Windows;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.Windows.Media.Media3D;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VMS.TPS
{
    public class Script
    {
        /*public double sizeX, sizeY;
        public double resolutionX, resolutionY;
        public double depth;
        public double max;
        private double[,] matrix;

        public int pixelsX;
        public int pixelsY;

        private bool extracted;
        private int planeNumber = 0;*/
        
        public void Execute(ScriptContext context)
        {
            // Your code here.
            run(context.IonPlanSetup, context.Patient);
        }
        public void run(IonPlanSetup plan, Patient patient)
        {
            //test(plan);
            CSI_GUI.Form1 form = new CSI_GUI.Form1(plan);
            form.ShowDialog();







            //csv.WriteToCSV(filePath, strOut);
        }
        public void test(IonPlanSetup plan)
        {
            
            //string filePath = @"\\SKVfile01.skandion.local\Gemensamdata$\Intern\QA Patient\ErikP\" + plan.Course.Patient.FirstName.Substring(0, 2) + plan.Course.Patient.LastName.Substring(0, 2) + plan.Course.Patient.Id.Substring(8, 4) + ".csv";//default Hard coded path
            //List<string> strOut = new List<string>();
            Structure waterPhantom = plan.StructureSet.Structures.FirstOrDefault(s => s.Id.Contains("Water_P"));
            VVector coordinates = new VVector();

            coordinates.x = plan.IonBeams.First().IsocenterPosition.x;
            coordinates.y = plan.IonBeams.First().IsocenterPosition.y;
            coordinates.z = waterPhantom.MeshGeometry.Positions.First().Z;

            foreach (var point in waterPhantom.MeshGeometry.Positions)
            {
                if (coordinates.z < point.Z)
                    coordinates.z = point.Z;
            }

            string strOut = "";
            strOut = "vatten z: " + coordinates.z.ToString() + "\n" + "Userorigin z: " + plan.StructureSet.Image.UserOrigin.x.ToString() + "\n" + "Iso z: " + plan.IonBeams.First().IsocenterPosition.z;
            strOut = strOut + "\n" + "Userorigin x: " + plan.StructureSet.Image.UserOrigin.x.ToString();
            strOut = strOut + "\n" + "Userorigin y: " + plan.StructureSet.Image.UserOrigin.y.ToString();
            strOut = strOut + "\n" + "Userorigin z: " + plan.StructureSet.Image.UserOrigin.z.ToString();
            plan.DoseValuePresentation = DoseValuePresentation.Absolute;
            DoseValue dose = plan.Dose.GetDoseToPoint(plan.StructureSet.Image.UserOrigin);

            MessageBox.Show(strOut);

            plan.Dose.GetDoseToPoint(coordinates);
        }
        /*
        public void Plane(PlanSetup planSetup, IonBeam beam, double requestedSizeX, double requestedSizeY, double resolutionX, double resolutionY, double depth)
        {
            this.resolutionX = resolutionX;
            this.resolutionY = resolutionY;
            this.pixelsX = (int)Math.Ceiling(requestedSizeX / resolutionX) + 1;
            this.pixelsY = (int)Math.Ceiling(requestedSizeY / resolutionY) + 1;
            this.sizeX = resolutionX * (pixelsX - 1);
            this.sizeY = resolutionY * (pixelsY - 1);

            this.depth = depth;
        }

        public void Extract()
        {
            IonBeam beam = plan.IonBeams.First();
            bool extracted = false;
            if (!extracted)
            {
                plan.DoseValuePresentation = DoseValuePresentation.Absolute;
                double scaleFactor = 0.01 * plan.TotalPrescribedDose.Dose * beam.WeightFactor / (0.01 * plan.PlanNormalizationValue);
                double posZ = -18.0 + depth; // User origin shift (surface) - depth
                matrix = new double[pixelsX, pixelsY];
                max = double.NegativeInfinity;
                int nextProgressStep = pixelsY / 10;
                for (int indexY = 0; indexY < pixelsY; indexY++)
                {
                    double posY = -sizeY / 2.0 + indexY * sizeY / ((double)(pixelsY - 1));
                    for (int indexX = 0; indexX < pixelsX; indexX++)
                    {
                        double posX = -sizeX / 2.0 + indexX * sizeX / ((double)(pixelsX - 1));

                        VVector pos = new VVector(posX * 10.0, posZ * 10, -posY * 10.0); // DICOM-coordinates
                        double doseGy;
                        if (!double.IsNaN(pos.x) && !double.IsNaN(pos.y) && !double.IsNaN(posZ))
                        {
                            DoseValue dose = beam.Dose.GetDoseToPoint(pos);
                            doseGy = dose.Dose * scaleFactor;
                        }
                        else
                            doseGy = 0;

                        matrix[indexX, indexY] = doseGy;
                        if (doseGy > max)
                            max = doseGy;
                    }
                    
                }
                extracted = true;
            }
        }*/
    }
}

