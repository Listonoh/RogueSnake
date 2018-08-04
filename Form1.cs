using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rogsnake
{
    public partial class Form1 : Form
    {
        int celkoveSkore;
        int lvl;
        Bitmap DrawArea;
        public Form1()
        {
            InitializeComponent();
            DrawArea = new Bitmap(pictureBox2.Size.Width, pictureBox2.Size.Height);
            pictureBox2.Image = DrawArea;
            label2.Text = celkoveSkore.ToString();
            timer1.Interval = 60;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Mapa mapa;
        Graphics g;


        private void button1_Click(object sender, EventArgs e)
        {
            lvl = 1;
            g = Graphics.FromImage(DrawArea);
            CteckaPokoju.CtecPokoju("MyTest2.txt", "mapaPokoju.png"); //vytvoří soubor z mapa0.2.png
            mapa = new Mapa(lvl, "dot.png", barva, pocetOcasu);

            
 
            
            pocetOcasu = 4;
            barva = 0;
            timer1.Enabled = true;
        }

        int tic = 0;
        int barva = 0;
        int pocetOcasu = 4;

        
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            switch (mapa.stav)
            {
                case Stav.nezacala:
                    
                    pictureBox1.Visible = true;
                    button1.Visible = true;
                    stisknutaSipka = StisknutaSipka.zadna;
                    pictureBox2.Visible = false;
                    
                    break;


                case Stav.zacala:
                    pictureBox2.Image = DrawArea;
                    mapa.vykresliSe(g, ClientSize.Width, ClientSize.Height , 3);
                    pictureBox2.Visible = true;
                    pictureBox1.Visible = false;
                    button1.Visible = false;
                    mapa.stav = Stav.bezi;
                    break;  

                case Stav.bezi:
                    tic++;
                    
                    timer1.Enabled = false;
                    if (tic % 4 == 0)
                    {
                    mapa.PohniVsemiPrvky(stisknutaSipka);

                    }
                    mapa.vykresliSe(g, ClientSize.Width, ClientSize.Height, tic % 4);
                    if (mapa.stav != Stav.prohra)
                    {
                        Refresh();
                    }
                    
                    timer1.Enabled = true;


                    this.Text ="skore   -    " + mapa.skore ;
                    break;
                case Stav.vyhra:
                    timer1.Enabled = false;
                    if (lvl < 6)
                    {
                        lvl++;
                        pocetOcasu = mapa.had.pocetOcasu;
                        barva = mapa.had.barva;
                        MessageBox.Show("pokračujete do úrovně: " + lvl);
                        celkoveSkore += mapa.skore;
                        label2.Text = celkoveSkore.ToString();
                        mapa = new Mapa(lvl, "dot.png", barva, pocetOcasu);
                        mapa.vykresliSe(g, ClientSize.Width, ClientSize.Height,3);
                        mapa.stav = Stav.bezi;
                        stisknutaSipka = StisknutaSipka.zadna;
                        timer1.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Vyhra!");
                        mapa.stav = Stav.nezacala;
                        timer1.Enabled = true;
                    }
                    
                    break;
                case Stav.prohra:
                    timer1.Enabled = false;
                    celkoveSkore += mapa.skore;
                    label2.Text = celkoveSkore.ToString();
                    mapa.vykresliSe(g, ClientSize.Width, ClientSize.Height, 3);
                    Refresh();
                    MessageBox.Show("Prohra!");
                    
                    mapa.stav = Stav.nezacala;
                    timer1.Enabled = true;
                    break;
                default:
                    break;
            }
            
        }
        

        
        
        StisknutaSipka stisknutaSipka = StisknutaSipka.zadna;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                stisknutaSipka = StisknutaSipka.nahoru;
                return true;
            }
            if (keyData == Keys.Down)
            {
                stisknutaSipka = StisknutaSipka.dolu;
                return true;
            }
            if (keyData == Keys.Left)
            {
                stisknutaSipka = StisknutaSipka.doleva;
                return true;
            }
            if (keyData == Keys.Right)
            {
                stisknutaSipka = StisknutaSipka.doprava;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 40;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Interval = 60;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Interval = 80;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Interval = 100;
        }
    }
}
