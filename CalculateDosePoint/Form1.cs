using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace CSI_GUI
{
    public partial class Form1 : Form
    {
        //private IonPlanSetup plan;
        private Point m_point;
        private PointManager pm;
        private CSVwriter csv;
        private List<string> output;
        public Form1(IonPlanSetup _plan)
        {

            this.plan = _plan;
            InitializeComponent();
            InitializeGUI();
        }
        private void InitializeGUI()
        {
            pm = new PointManager();
            m_point = new Point();
            csv = new CSVwriter();
            output = new List<string>();
            txtFileName.Text = plan.Id;
        }
        private void UpdateGUI()
        {

        }

        private void OutputFromField()
        {
            foreach (IonPlanSetup ionplan in lsbPlans.SelectedItems)
            {
                if (!string.IsNullOrEmpty(txtShiftX.Text))
                    if (!string.IsNullOrEmpty(txtShiftY.Text))
                        if (!string.IsNullOrEmpty(txtShiftZ.Text))
                        {
                            m_point.X = double.Parse(txtShiftX.Text);
                            m_point.Y = double.Parse(txtShiftY.Text);
                            m_point.Z = double.Parse(txtShiftZ.Text);
                            VVector coordinate = new VVector(m_point.X, m_point.Z + ionplan.StructureSet.Image.UserOrigin.y, m_point.Y);

                            ionplan.DoseValuePresentation = DoseValuePresentation.Absolute;
                            m_point.PointDose = ionplan.Dose.GetDoseToPoint(coordinate).Dose;
                            foreach (IonBeam beam in ionplan.IonBeams)
                            {
                                m_point.AddFieldPointDose(beam.Dose.GetDoseToPoint(coordinate).Dose / 100 * ionplan.TotalDose.Dose);
                                m_point.AddFieldIDs(beam.Id);
                            }

                            m_point.PlanID = ionplan.Id;
                            pm.Add(m_point);
                            m_point = new Point();
                        }
            }
            
        }

        private void btnAddField_Click(object sender, EventArgs e)
        {
            OutputFromField();
            UpdateList();
        }

        private void UpdateList()
        {
            lbListOfFields.Items.Clear();
            foreach (var field in pm.ListOfFields)
                for (int i = 0; i < field.ListOfFieldIDs.Count; i++) 
                    lbListOfFields.Items.Add(field.Output(i));
        }
        private void ExtraData()
        {
            foreach (IonPlanSetup ionplan in lsbPlans.SelectedItems)
            {
                output.Add("Plan ID" + ";" + ionplan.Id);
                output.Add("Total dose" + ";" + ionplan.TotalDose.ValueAsString);
                output.Add("");
                foreach (IonBeam beam in ionplan.IonBeams)
                {
                    output.Add("Field ID" + ";" + beam.Id);
                    output.Add("MU" + ";" + beam.Meterset.Value.ToString("0.00"));
                    bool checkRS = beam.RangeShifters.Any();
                    if (checkRS)
                        output.Add("Range shifter" + ";" + "Yes");
                    else
                        output.Add("Range shifter" + ";" + "No");
                    output.Add("Snout position (cm)" + ";" + (beam.IonControlPoints.First().SnoutPosition / 10).ToString("0.00"));
                    output.Add("SSD (cm)" + ";" + (beam.SSD / 10).ToString("0.00"));
                    output.Add("Nominal Range (cm)" + ";" + (beam.NominalRange / 10).ToString("0.00"));
                    output.Add("Nominal Modulation Width (cm)" + ";" + (beam.NominalSOBPWidth / 10).ToString("0.00"));
                    output.Add("");
                }
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            pm.Delete(lbListOfFields.SelectedIndex);
            UpdateList();
        }

        private void btnRensa_Click(object sender, EventArgs e)
        {
            pm.Clear();
            UpdateList();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string filename = @"\\SKVfile01.skandion.local\Gemensamdata$\Gemensam\01 Commissioning\02_Room_Matching_Project\09_Validation\Eclipse output\" + txtFileName.Text + ".csv";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            ExtraData();
            string strOutput = "X(mm)" + ";" + "Y (mm)" + ";" + "Z(mm)" + ";" + "Point dose (Gy)";
            foreach (IonBeam beam in plan.IonBeams)
                strOutput = strOutput + ";" + beam.Id;
            output.Add(strOutput);
            foreach (var point in pm.ListOfFields)
            {
                string strOut = point.X.ToString() + ";" + point.Y.ToString() + ";" + point.Z.ToString() + ";" + point.PointDose.ToString();
                for (int i = 0; i < point.ListOfFieldIDs.Count; i++)
                {
                    strOut = strOut + ";" + point.ListOfFieldPointDose.ElementAt(i).ToString();
                }
                output.Add(strOut);
            }
            csv.WriteToCSV(filename, output);
            MessageBox.Show("Done!");
        }
    }
}
