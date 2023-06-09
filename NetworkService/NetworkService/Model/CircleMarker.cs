using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NetworkService.Model
{
    public class CircleMarker : BindableBase
    {
        private int cmId;
        private int cmValue;
        private string cmDate;
        private string cmTime;
        private double cmDiameter;
        private Brush cmColor;

        public CircleMarker()
        {

        }

        public CircleMarker(int cmId, int cmValue, string cmDate, string cmTime)
        {
            CmId = cmId;
            CmValue = cmValue;
            CmDate = cmDate;
            CmTime = cmTime;
        }

        public int CmId
        {
            get
            {
                return cmId;
            }
            set
            {
                cmId = value;
                OnPropertyChanged("CmId");
            }
        }

        public int CmValue
        {
            get
            {
                return cmValue;
            }
            set
            {
                cmValue = value;
                CmDiameter = (((CmValue - 32) * 5) / 9) * 0.2;
                if (CmValue < 670 || CmValue > 735)
                {
                    cmColor = Brushes.Red;
                }
                else
                {
                    cmColor = Brushes.CadetBlue;
                }
                OnPropertyChanged("CmColor");
                OnPropertyChanged("CmValue");
            }
        }

        public string CmDate
        {
            get
            {
                return cmDate;
            }
            set
            {
                cmDate = value;
                OnPropertyChanged("CmDate");
            }
        }

        public string CmTime
        {
            get
            {
                return cmTime;
            }
            set
            {
                cmTime = value;
                OnPropertyChanged("CmTime");
            }
        }

        public double CmDiameter
        {
            get
            {
                return cmDiameter;
            }
            set
            {
                cmDiameter = value;
                OnPropertyChanged("CmDiameter");
            }
        }

        public Brush CmColor
        {
            get
            {
                return cmColor;
            }
            set
            {
                cmColor = value;
                OnPropertyChanged("CmColor");
            }
        }
    }
}
