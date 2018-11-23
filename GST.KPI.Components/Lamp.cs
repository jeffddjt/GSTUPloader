using GST.KPI.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GST.KPI.Components
{  
    public class Lamp:PictureBox
    {
        public event EventHandler StatusChanged;
        public Lamp()
        {
            this.Width = 25;
            this.Height = 25;
        }

        private Color color = Color.Red;

        public KPIStatus Status
        {
            get
            {
                return this.color == Color.Red ? KPIStatus.Stoped : this.color == Color.DarkGreen ? KPIStatus.Running : KPIStatus.Unknown;
            }
            set
            {
                switch (value)
                {
                    case KPIStatus.Running:
                        this.color = Color.DarkGreen;
                        break;
                    case KPIStatus.Stoped:
                        this.color = Color.Red;
                        break;
                    default:
                        this.color = Color.Gray;
                        break;
                }
                this.Refresh();
                this.StatusChanged?.Invoke(this,new EventArgs());
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;
            Brush brush = new SolidBrush(this.color);
            g.FillEllipse(brush, new RectangleF(0, 0, this.Width, this.Height));
            base.OnPaint(pe);
        }
    }
}
