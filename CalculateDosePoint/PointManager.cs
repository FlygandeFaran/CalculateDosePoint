using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSI_GUI
{
    public class PointManager
    {
        private List<Point> listOfFields;

        public List<Point> ListOfFields
        {
            get { return listOfFields; }
        }
        
        public void Add(Point _point)
        {
            listOfFields.Add(_point);
        }
        public void Delete(int index)
        {
            listOfFields.RemoveAt(index);
        }
        public void Clear()
        {
            listOfFields.Clear();
        }
        public PointManager()
        {
            listOfFields = new List<Point>();
        }
    }
}
