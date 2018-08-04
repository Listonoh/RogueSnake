using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rogsnake
{
    class Mapa
    {
        public Random rnd = new Random();
        public int skore = 0;
        public Had had;
        private char[,] plan;
        int sirka;
        int vyska;
        int sx; // velikost políčka
        public Stav stav = Stav.nezacala;
        Bitmap[] ikonky;
        public StisknutaSipka stisknutaSipka;
        public List<PohyblivyPrvek> PohyblivePrvky;
        public List<Prvek> Prvky;
        public List<Pickable> pickables;


        public void ZrusPohyblivyPrvek(int zX, int zY)
        {
            for (int i = 0; i < PohyblivePrvky.Count; i++)
            {
                if ((PohyblivePrvky[i].x == zX) && (PohyblivePrvky[i].y == zY))
                {
                    plan[zX, zY] = '.';

                    PohyblivePrvky.RemoveAt(i);

                    break;
                }
            }
        }


        public Mapa(int lvl, string cestaIkonky, int barva, int pocetOcasu)
        {
            NactiIkonky(cestaIkonky);
            NactiMapu(lvl);
            stav = Stav.zacala;
            had.barva = barva;
            had.kolikPridatOcasu = pocetOcasu;
        }

        public void Presun(int zX, int zY, int naX, int naY)
        {
            char c = plan[zX, zY];
            plan[zX, zY] = '.';
            plan[naX, naY] = c;

            // přepíše objektům jejich souřadnice uvnitř
            //if ((zX == hrdina.x) && (zY == hrdina.y))
            if (c == 'H')
            {
                had.x = naX;
                had.y = naY;
                return;
            }

            // najit pripadny pohyblivyPrvek a zmenit mu polohu :
            foreach (PohyblivyPrvek po in PohyblivePrvky)
            {
                if ((po.x == zX) && (po.y == zY))
                {
                    po.x = naX;
                    po.y = naY;
                    break; // jakmile tam stoji jeden, tak uz tam nikro jiny nestoji
                }
            }

        }

        public bool JeVolno(int x, int y)
        {
            if (JePlocha(x, y))
                return (plan[x, y] == '.');
            return false;
        }
        public void znicOcas(int x, int y)
        {
            ZrusPohyblivyPrvek(x, y);
        }
        public bool JeHad(int x, int y)
        {
            return (plan[x, y] == 'H');
        }
        public bool JeOcas(int x, int y)
        {
            return (plan[x, y] == 'o');
        }
        public bool JeBomba(int x, int y)
        {
            return (plan[x, y] == 'B');
        }
        public int smerx(int s)
        {
            if (s == 1)
            {
                return 1;
            }
            else if (s == 3)
            {
                return -1;
            }
            else return 0;
        }
        public int smery(int s)
        {
            if (s == 2)
            {
                return 1;
            }
            else if (s == 0)
            {
                return -1;
            }
            else return 0;
        }

        public void exploze(int x , int y)
        {
            char c = plan[x, y];
            switch (c)
            {
                case 'H':
                case 'o':
                    had.zasah();
                    break;
                case '.':
                    plan[x, y] = 'P';
                    ohen ohen = new ohen(this, x, y);
                    PohyblivePrvky.Add(ohen);
                    break;
                case 'B':
                    for (int i = 0; i < PohyblivePrvky.Count; i++)
                    {
                        if (PohyblivePrvky[i].x == x && PohyblivePrvky[i].y == y)
                        {
                            PohyblivePrvky[i].smer = -2;
                        }
                    }
                    break;
                default:
                    break;
            }

        }

        bool JePlocha(int x, int y)
        {
            if (x > sirka - 1 || x < 0 || y > vyska - 1 || y < 0)
            {
                stav = Stav.prohra;
                return false;
            }
            else return true;
        }
        public void SeberPickable(int mx, int my, Had had)
        {
            for (int i = 0; i < pickables.Count; i++)
            {
                if (pickables[i].x == mx && pickables[i].y == my)
                {
                    pickables[i].Pick(had);
                }
            } 
        }
        public void pohniBonbou(int x, int  y , int smer)
        {
            for (int i = 0; i < PohyblivePrvky.Count; i++)
            {
                if (PohyblivePrvky[i].x == x && PohyblivePrvky[i].y == y)
                {
                    PohyblivePrvky[i].smer = smer;
                    PohyblivePrvky[i].UdelejKrok();
                }
            }
        }
        public void pridejKouly(int x, int y)
        {
            if (JeVolno(x,y))
            {
                plan[x, y] = 'b';
            }
        }

        public void pridejBonbu(int x, int y)
        {
            if (JeVolno(x, y))
            {
                plan[x, y] = 'B';
            }
        }

        public bool JeJablko(int x, int y)
        {
            if (JePlocha(x, y))
                return (plan[x, y] == 'j');
            return false;
        }
        public bool JePickables(int x, int y)
        {
            if (JePlocha(x, y))
                return ((plan[x, y] == 'j') || (plan[x,y] == 'k') || (plan[x, y] == 'A') || (plan[x, y] == 'D')); //určitě by to šlo lépe :(
            return false;
        }
        public bool JeOtevrenyVychod(int x, int y)
        {
            return plan[x, y] == 'e';
        }
        public void OtevriVychod()
        {
            for (int x = 0; x < sirka; x++)
            {
                for (int y = 0; y < vyska; y++)
                {
                    if (plan[x, y] == 'E')
                        plan[x, y] = 'e';
                }
            }

        }
        public void VytvorJablko()
        {
            int x = rnd.Next(sirka - 1);
            int y = rnd.Next(vyska - 1);
            if (plan[x, y] == '.')
            {
                Jablko jablko = new Jablko(this, x, y);
                pickables.Add(jablko);
                plan[x, y] = 'j';
            }
            else VytvorJablko();
        }
        public void VytvorOcas(int x, int y)
        {
            plan[x, y] = 'o';
        }


        public void NactiMapu(int lvl)
        {
            PohyblivePrvky = new List<PohyblivyPrvek>();
            Prvky = new List<Prvek>();
            pickables = new List<Pickable>();
            int s = 19;
            int v = 16;
            if (lvl % 3  == 0)
            {
                //načítání z předem vybrané mapy "boss"
                System.IO.StreamReader sr = new System.IO.StreamReader("mapa" + lvl + ".txt");
                int sirkaVyrezu = int.Parse(sr.ReadLine()); //v mapách
                int vyskaVyrezu = int.Parse(sr.ReadLine());



                sirka = sirkaVyrezu * s;
                vyska = vyskaVyrezu * v;

                int my, mx;
                plan = new char[sirka, vyska];

                for (int i = 0; i < sirkaVyrezu; i++)
                {
                    for (int j = 0; j < vyskaVyrezu; j++)
                    { //jednotlivé prvky
                        for (int y = 0; y < v; y++)
                        {
                            string radek = sr.ReadLine();
                            for (int x = 0; x < s; x++)
                            {
                                char znak = radek[x];
                                mx = x + i * s;
                                my = y + j * v;
                                nactiZnak(mx, my, znak);
                            }
                        }
                        sr.ReadLine();

                    }
                }


                sr.Close();
            }
            else
            {
                //random generovaná mapa
                TvorbaMapy t = new TvorbaMapy(8, 8, "MyTest2.txt");
                int sirkaMapy = 8;
                int vyskaMapy = 8;

                sirka = sirkaMapy * s;
                vyska = vyskaMapy * v;

                int my, mx;
                plan = new char[sirka, vyska];

                for (int i = 0; i < sirkaMapy; i++)
                {
                    for (int j = 0; j < vyskaMapy; j++)
                    { //jednotlivé prvky
                        planMapy k = t.listPlanuMapy[i, j];

                        for (int y = 0; y < v; y++)
                        {
                            for (int x = 0; x < s; x++)
                            {
                                char znak = k.plan[x, y];
                                mx = x + i * s;
                                my = y + j * v;
                                nactiZnak(mx, my, znak);
                            }
                        }
                    }
                }
            }
        }

        void nactiZnak(int mx, int my ,char znak)
        {
            plan[mx, my] = znak;

            // vytvorit pripadne pohyblive objekty:
            switch (znak)
            {
                case 'H':
                    this.had = new Had(this, mx, my);
                    break;
                case 'j':
                    Jablko jablko = new Jablko(this, mx, my);
                    pickables.Add(jablko);
                    break;
                case 'D':
                    Diamant diamant = new Diamant(this, mx, my);
                    pickables.Add(diamant);
                    break;
                case 'A':
                    armor armor = new armor(this, mx, my);
                    pickables.Add(armor);
                    break;
                case 'X':
                    Sutr balvan = new Sutr(this, mx, my);
                    break;
                case '1':
                    duch d = new duch(this, mx, my);
                    PohyblivePrvky.Add(d);
                    break;
                case '2':
                    ohnivaKoule ohnivaKoule = new ohnivaKoule(this, mx, my);
                    PohyblivePrvky.Add(ohnivaKoule);
                    break;
                case 'c':
                    kanon kanon = new kanon(this, mx, my);
                    PohyblivePrvky.Add(kanon);
                    break;
                case 'B':
                    bomba bomb = new bomba(this, mx, my);
                    PohyblivePrvky.Add(bomb);
                    break;
                case 'k':
                    Klic klic = new Klic(this, mx, my);
                    pickables.Add(klic);
                    break;
                case '_':
                    BombPad bombPad = new BombPad(this, mx, my);
                    PohyblivePrvky.Add(bombPad);
                    break;
                default:
                    break;
            }
        }


        public void NactiIkonky(string cesta)
        {
            Bitmap bmp = new Bitmap(cesta);
            this.sx = 32;
            int pocetW = (bmp.Width / sx); // v rade a ve čtverci
            int pocetH = (bmp.Height / sx);
            ikonky = new Bitmap[pocetH * pocetW];
            for (int j = 0; j < pocetH; j++)
            {
                for (int i = 0; i < pocetW; i++)
                {
                    Rectangle rect = new Rectangle(i * sx, j * sx, sx, sx);
                    
                    ikonky[i + j * pocetW] = bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);
                }
            }

        }

        public void PohniVsemiPrvky(StisknutaSipka stisknutaSipka)
        {
            this.stisknutaSipka = stisknutaSipka;
            for (int i = 0; i < PohyblivePrvky.Count; i++)
            {
                PohyblivePrvky[i].UdelejKrok();
            }

            had.UdelejKrok();
        }

        int minSmerOcasu = -1;
        int minIkonkaOcasu = -1;

        public void vykresliSe(Graphics g, int sirkaVyrezuPixely, int vyskaVyrezuPixely, int frame)
        {
            int sirkaVyrezu = 19;
            int vyskaVyrezu = 16;
            int hlavaX = 0;
            int hlavaY = 0;
            int indexHlavy = 0;
            int ocasX = 0;
            int ocasY = 0;
            int indexOcasu = 0;
            int smerOcasu = 0;
            
            


            // urcit LHR vyrezu:
            int dx = had.x / 19;
            int dy = had.y / 16;



            for (int x = 0; x < sirkaVyrezu; x++)
            {
                for (int y = 0; y < vyskaVyrezu; y++)
                {
                    char c = plan[dx * sirkaVyrezu + x, dy * vyskaVyrezu + y];
                    int indexObrazku = "H.jXoeE12D4cbkBA_P".IndexOf(c); // 0..
                    if (indexObrazku == 0) //když hlava
                    {
                        if (had.smer < 0)
                        {
                            indexObrazku++;
                        }
                        indexObrazku += had.smer + 18 + 16*had.barva;
                        hlavaX = x;
                        hlavaY = y;
                        indexHlavy = indexObrazku;
                    }
                    else
                    if (indexObrazku == 4) //když ocas, tak vrátí správnou texturu ocasu
                    {
                        int ind = vratIndex(dx * sirkaVyrezu + x, dy * vyskaVyrezu + y);
                        if (ind != 3) //neboly index který není chybový
                        {

                            if (ind >= 14 && ind <= 17) //neboli je to ocas
                            {
                                ocasX = x;
                                ocasY = y;
                                smerOcasu = ind - 14;
                                if (smerOcasu == 1)
                                {
                                    smerOcasu = 3;
                                }
                                else if (smerOcasu == 3)
                                {
                                    smerOcasu = 1;
                                }

                                indexOcasu = ind + 14 + 16 * had.barva;
                            }
                            ind += 14 + 16 * had.barva; //odsazení
                            
                        }
                        indexObrazku = ind;
                    }

                    if (indexObrazku != 1) //podklad (nemusí mít texturu pozadí a je děravej)
                    {
                        g.DrawImage(ikonky[1], x * sx, y * sx);
                    }
                    g.DrawImage(ikonky[indexObrazku], x * sx, y * sx);

                }
            }
            //hlava animace
            g.DrawImage(ikonky[1], hlavaX * sx, hlavaY * sx);
            g.DrawImage(ikonky[indexHlavy], hlavaX * sx - smerx(had.smer) * (3-frame) * 8, hlavaY * sx - smery(had.smer) * (3-frame) * 8);


            //ocas animace 
            //g.DrawImage(ikonky[1], ocasX * sx, ocasY * sx);
            if (minIkonkaOcasu == -1 && indexOcasu !=0)
            {
                minIkonkaOcasu = indexOcasu;
            }

            if ((ocasX != 0 && ocasY != 0) && minSmerOcasu != -1 && minIkonkaOcasu != 0)
            {
                if (frame % 4 < 3)
                {
                    g.DrawImage(ikonky[minIkonkaOcasu], ocasX * sx - smerx(minSmerOcasu) * (3 - frame) * 8, ocasY * sx - smery(minSmerOcasu) * (3 - frame) * 8);

                }
            }

            if (frame % 4 == 3)
            {
                minSmerOcasu = smerOcasu;
                minIkonkaOcasu = indexOcasu;
            }
            
            
        }

        int vratIndex(int x, int y)
        {
            for (int i = 0; i < PohyblivePrvky.Count; i++)
            {
                if ((PohyblivePrvky[i].x == x) && (PohyblivePrvky[i].y == y))
                {
                    if (PohyblivePrvky[i].otec != null)
                    {
                        if (PohyblivePrvky[i].tail != null) //hodně zmatená magie -.- ale funguje :)
                        {
                            int tx = PohyblivePrvky[i].tail.x - x;
                            int ty = PohyblivePrvky[i].tail.y - y;
                            int ox = PohyblivePrvky[i].otec.x - x;
                            int oy = PohyblivePrvky[i].otec.y - y;
                            int tk = tx * 10 + ty;
                            int ok = ox * 10 + oy;
                            int k = Math.Max(tk, ok) * 10 + Math.Min(ok, tk);
                            switch (k)
                            {
                                case 90:
                                    return 8;
                                case 99:
                                    return 13;
                                case 101:
                                    return 9;
                                case 0:
                                    return 12;
                                case 9:
                                    return 10;
                                case -20:
                                    return 11;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            int ox = PohyblivePrvky[i].otec.x - x;
                            int oy = PohyblivePrvky[i].otec.y - y;
                            int k = ox * 10 + oy;
                            switch (k)
                            {
                                case 1:
                                    return 16;

                                case -1:
                                    return 14;
                                case 10:
                                    return 17;
                                case -10:
                                    return 15;
                                default:
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
            return 3;
        }
    }
}
