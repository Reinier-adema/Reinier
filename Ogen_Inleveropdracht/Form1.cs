using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Timers;

namespace Ogen_Inleveropdracht
{
    public partial class Form1 : Form
    {
        int breedte;
        int Grootteoog;
        int Xpupil; int Ypupil;
        int xMuis; int yMuis;
        bool muisclick = false;
        int Pupilstraal = 10;
        int middenX = 0;
        int middenY = 0;
        int Xverschil = 0;
        int Yverschil = 0;
        int verschiloogmuis = 0;
        Thread test;

        public Form1()
        {
            InitializeComponent();
            breedte = this.Width / 9;
            this.MouseMove += this.Muisbeweegt;
            this.MouseClick += this.Muisklik;
            this.Paint += this.TekenMethode;
            this.DoubleBuffered = true;
            Grootteoog = 3 * breedte;
        }

        private void TekenMethode(object sender, PaintEventArgs gr)
        {
            this.OgenTekenen(gr.Graphics, breedte, 70, Grootteoog / 2);
            this.OgenTekenen(gr.Graphics, 5 * breedte, 70, Grootteoog / 2);
            if (muisclick == true)
            {
                this.OgenTekenen(gr.Graphics, breedte, 70, Grootteoog / 2);
                this.OgenTekenen(gr.Graphics, 5 * breedte, 70, Grootteoog / 2);
            }
        }

        private void Muisbeweegt(object sender, MouseEventArgs muis)
        {
            xMuis = muis.X; yMuis = muis.Y;
            this.Invalidate();
        }
        private void Muisklik(object sender, MouseEventArgs muis)
        {
            if (muisclick == false)
            {
                muisclick = true;
            }
            else
            {
                muisclick = false;
            }
            test = new Thread(this.testen);
            test.Start();
            Invalidate();
        }

        private void testen()
        {
            int T = 0;
            Graphics gr = this.CreateGraphics();
            while (muisclick)
            {
                gr.DrawLine(new Pen(Color.Black), 1, 1, T, T);
                T++;
            }
        }

        private void OgenTekenen(Graphics gr, int x, int y, int Oogstraal)
        {
            middenX = x + Oogstraal;
            middenY = y + Oogstraal;
            Xverschil = xMuis - middenX;
            Yverschil = yMuis - middenY;

            verschiloogmuis = (int)Math.Sqrt(Xverschil * Xverschil + Yverschil * Yverschil);

            if (verschiloogmuis == 0)
            {
                Xpupil = middenX;
                Ypupil = middenY;
            }

            else if(verschiloogmuis > Oogstraal * 2)
            {
                Xpupil = ((Oogstraal * Xverschil) / verschiloogmuis) + middenX - Pupilstraal;
                Ypupil = ((Oogstraal * Yverschil) / verschiloogmuis) + middenY - Pupilstraal;
            }

            else
            {
                Xpupil = ((int)(0.5 * Xverschil)) + middenX - Pupilstraal;
                Ypupil = ((int)(0.5 * Yverschil)) + middenY - Pupilstraal;
            }

            gr.FillEllipse(Brushes.White, x - Pupilstraal, y - Pupilstraal, 2 * Oogstraal + (2 * Pupilstraal), 2 * Oogstraal + (2 * Pupilstraal));
            gr.DrawEllipse(new Pen(Color.Black), x - Pupilstraal, y - Pupilstraal, 2 * Oogstraal + (2 * Pupilstraal), 2 * Oogstraal + (2 * Pupilstraal));
            gr.FillEllipse(Brushes.Black, Xpupil, Ypupil, 2 * Pupilstraal, 2 * Pupilstraal);

            if (muisclick == true)
            {
                this.lazerogen(gr, xMuis, yMuis, middenX, middenY, verschiloogmuis, Xverschil, Yverschil, Oogstraal);
            }
        }
        private void lazerogen(Graphics gr, int x, int y, int middenX, int middenY, int verschiloogmuis, int Xverschil, int Yverschil, int Oogstraal)
        {
            //muisclick = false;
            if (verschiloogmuis == 0)
            {
                Xpupil = middenX;
                Ypupil = middenY;
            }

            else if (verschiloogmuis > Oogstraal * 2)
            {
                Xpupil = ((Oogstraal * Xverschil) / verschiloogmuis) + middenX - Pupilstraal;
                Ypupil = ((Oogstraal * Yverschil) / verschiloogmuis) + middenY - Pupilstraal;
            }

            else
            {
                Xpupil = ((int)(0.5 * Xverschil)) + middenX - Pupilstraal;
                Ypupil = ((int)(0.5 * Yverschil)) + middenY - Pupilstraal;
            }
            gr.DrawLine(new Pen(Color.Red), Xpupil + Pupilstraal, Ypupil + Pupilstraal, x, y);
            
            //Schieten schiet = new Schieten();
            //schiet.Schiet(Xpupil + Pupilstraal, Ypupil + Pupilstraal, x, y);
        }
    }

    /*class Schieten : Form
    {
        float helling = 1;
        int i;
        System.Timers.Timer tmrschiet = new System.Timers.Timer(1000);
        int Xp = 0; int Yp = 0; int xM = 0; int yM = 0;

        public Schieten()
        {
            SetTimer();
            tmrschiet.Enabled = false;
        }

        public void Schiet(int Xpupil, int Ypupil, int xMuis, int yMuis)
        {
            helling = (Xpupil - xMuis) / (Xpupil - yMuis);
            Xp = Xpupil; Yp = Ypupil; xM = xMuis; yM = yMuis;
            i = 0;
            tmrschiet.Enabled = true;
        }

        private void SetTimer()
        {
            tmrschiet.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.DrawLine(new Pen(Color.Red), helling * i + Xp, (1 / helling) * (i - Yp), xM, yM);
            i++;
            Invalidate();
        }
    }*/
}
