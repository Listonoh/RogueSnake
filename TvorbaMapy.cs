using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogsnake
{
    class TvorbaMapy
    {

        public planMapy[,] listPlanuMapy;
        public int sirka, vyska, sx, sy, cx, cy;
        string path;
        int pocetPrekazek;
        public bool[,] mapa;
        Random random = new Random();
        public char[,,] prekazky;
        public char[,] planekFinal = new char[8, 8];

        public List<int> levoDolu = new List<int>();
        public List<int> vodorovne = new List<int>();
        public List<int> svisle = new List<int>();
        public List<int> T = new List<int>();
        public List<int> prevracenyT = new List<int>();
        public List<int> kriz = new List<int>();
        public List<int> levo = new List<int>();
        public List<int> dole = new List<int>();

        public TvorbaMapy(int x, int y, string cesta)
        {
            path = cesta;
            nactiPrekazky();
            this.sirka = x;
            this.vyska = y;
            listPlanuMapy = new planMapy[x, y];
            
            mapa = new bool[sirka, vyska];
            sx = random.Next(sirka); //začátek
            sy = random.Next(vyska);
            cx = random.Next(sirka); //cíl
            cy = random.Next(vyska);
            while (Math.Abs(cx - sx) + Math.Abs(cy - sy) < Math.Max(sirka, vyska) / 2 - 1)
            {
                cx = random.Next(sirka); //cíl
                cy = random.Next(vyska);
            }
            mapa[sx, sy] = true;
            mapa[cx, cy] = true;
            vytvorPlan();
        }

        void nactiPrekazky()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            pocetPrekazek = int.Parse(sr.ReadLine());
            int sirkaPrekazek = 17;
            int vyskaPrekazek = 14;
            prekazky = new char[pocetPrekazek, sirkaPrekazek, vyskaPrekazek];

            for (int i = 0; i < pocetPrekazek; i++)
            {
                for (int y = 0; y < vyskaPrekazek; y++)
                {
                    string radek = sr.ReadLine();
                    for (int x = 0; x < sirkaPrekazek; x++)
                    {
                        char znak = radek[x];
                        prekazky[i, x, y] = znak;
                    }
                }
                sr.ReadLine();
            }
            sr.Close();
            nalistujPrekazky();
            Console.WriteLine("ahoj");
        }

        void nalistujPrekazky()
        {
            for (int i = 0; i < pocetPrekazek; i++)
            {
                int vychody = 0;
                if (prekazky[i, 0, 0] == 'X') //levo
                {
                    vychody += 1;
                }
                if (prekazky[i, 16, 0] == 'X') //nahore
                {
                    vychody += 2;
                }
                if (prekazky[i, 0, 13] == 'X')// dole
                {
                    vychody += 4;
                }
                if (prekazky[i, 16, 13] == 'X') //pravo
                {
                    vychody += 8;
                }

                switch (vychody)
                {
                    case 5:
                        levoDolu.Add(i);
                        break;
                    case 1:
                        levo.Add(i);
                        break;
                    case 4:
                        dole.Add(i);
                        break;
                    case 9:
                        vodorovne.Add(i);
                        break;
                    case 13:
                        T.Add(i);
                        break;
                    case 6:
                        svisle.Add(i);
                        break;
                    case 7:
                        prevracenyT.Add(i);
                        break;
                    case 15:
                        kriz.Add(i);
                        break;
                    default:
                        break;
                }

            }

        }



        private void vytvorPlan()
        {
            int vx = sx;
            int vy = sy;
            int difx, dify, r, or;
            bool propojeno = false;
            while (!propojeno)
            {
                r = random.Next(2);
                or = (r + 1) % 2;
                difx = vx - cx;
                dify = vy - cy;
                if (difx == 0) //pokud ne tak musí být rozdíl v y jinak je identita
                {
                    vy = -Math.Sign(dify) + vy;
                }
                else if (dify == 0)
                {
                    vx = -Math.Sign(difx) + vx;
                }
                else
                {
                    vx = -Math.Sign(difx) * r + vx;
                    vy = -Math.Sign(dify) * or + vy;
                }

                if (vx == cx && vy == cy)
                {
                    propojeno = true;
                }
                else
                {
                    mapa[vx, vy] = true;
                }
            }
            pridejDalsiPole();
            planMapy k;

            for (int i = 0; i < sirka; i++)
            {
                for (int j = 0; j < vyska; j++)
                {
                    if (mapa[i, j])
                    {
                        k = new planMapy(this, i, j, false);
                    }
                    else
                    {
                        k = new planMapy(this, i, j, true);
                    }
                    listPlanuMapy[i, j] = k;

                }
            }
        }

        void pridejDalsiPole()
        {
            int pocet = random.Next(5, 8);
            while (pocet > 0)
            {
                int rx = random.Next(sirka);
                int ry = random.Next(vyska);

                if (maSouseda(rx, ry))
                {
                    pocet--;
                    mapa[rx, ry] = true;
                }
            }
        }

        bool maSouseda(int x, int y)
        {
            if (x - 1 >= 0 && mapa[x - 1, y])
            {
                return true;
            }
            else if (x + 1 < sirka && mapa[x + 1, y])
            {
                return true;
            }
            if (y - 1 >= 0 && mapa[x, y - 1])
            {
                return true;
            }
            if (y + 1 < vyska && mapa[x, y + 1])
            {
                return true;
            }
            else return false;
        }

        private void vypisMapu()
        {
            char r;
            //FileStream fs = File.Create(path)
            for (int i = 0; i < vyska; i++)
            {
                for (int j = 0; j < sirka; j++)
                {
                    if (mapa[j, i])
                    {
                        r = '0';
                    }
                    else r = 'X';
                    Console.Write(r);
                }
                Console.WriteLine();
            }
        }
    }





    class planMapy
    {
        bool levo, pravo, nahore, dole;
        int x, y;
        int vyska = 16;
        int sirka = 19;
        public char[,] plan;
        TvorbaMapy planMap;

        public planMapy(TvorbaMapy t, int x, int y, bool prazdna)
        {
            planMap = t;
            plan = new char[sirka, vyska];
            this.x = x;
            this.y = y;
            inic();
            if (!prazdna)
            {
                ziskejSousedy();
                ohranicPlan();
                if (x == planMap.sx && y == planMap.sy)
                {
                    plan[sirka / 2, vyska / 2] = 'H';
                }
                else if (x == planMap.cx && y == planMap.cy)
                {
                    plan[sirka / 2, vyska / 2] = 'e';
                }
                else
                {
                    pridejPrekazky();
                }


            }

        }

        void pridejPrekazky()
        {
            int vychody = 0;
            int cisloPrekazky = 0;
            bool prekVodorovne = false;
            bool prekSvisle = false;
            Random r = new Random(x*y);

            if (levo)
            {
                vychody++;
            }
            if (nahore)
            {
                vychody += 2;
            }
            if (dole)
            {
                vychody += 4;
            }
            if (pravo)
            {
                vychody += 8;
            }

            int volnePole = 63;
            switch (vychody)
            {
                case 1: //levo
                    if (planMap.levo.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.levo[r.Next(planMap.levo.Count - 1)];
                    break;
                case 2: //nahore
                    if (planMap.dole.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.dole[r.Next(planMap.dole.Count - 1)];
                    prekVodorovne = true;
                    break;
                case 3:
                    if (planMap.levoDolu.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.levoDolu[r.Next(planMap.levoDolu.Count - 1)];
                    prekVodorovne = true;
                    break;
                case 4:
                    if (planMap.dole.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.dole[r.Next(planMap.dole.Count - 1)];
                    break;
                case 5:
                    if (planMap.levoDolu.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.levoDolu[r.Next(planMap.levoDolu.Count - 1)];
                    break;
                case 6:
                    if (planMap.svisle.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        //cisloPrekazky = planMap.svisle[6];
                    cisloPrekazky = planMap.svisle[r.Next(planMap.svisle.Count - 1)];
                    break;
                case 7:
                    if (planMap.prevracenyT.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.prevracenyT[r.Next(planMap.prevracenyT.Count - 1)];
                    break;
                case 8:
                    if (planMap.levo.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.levo[r.Next(planMap.levo.Count - 1)];
                    prekSvisle = true;
                    break;
                case 9:
                    if (planMap.vodorovne.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.vodorovne[r.Next(planMap.vodorovne.Count -1)];
                    break;
                case 10:
                    if (planMap.levoDolu.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.levoDolu[r.Next(planMap.levoDolu.Count - 1)];
                    prekVodorovne = true;
                    prekSvisle = true;
                    break;
                case 11:
                    if (planMap.T.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.T[r.Next(planMap.T.Count - 1)];
                    prekVodorovne = true;
                    break;
                case 12:
                    if (planMap.levoDolu.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.levoDolu[r.Next(planMap.levoDolu.Count - 1)];
                    prekSvisle = true;
                    break;
                case 13:
                    if (planMap.T.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.T[r.Next(planMap.T.Count - 1)];
                    break;
                case 14:
                    if (planMap.prevracenyT.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.prevracenyT[r.Next(planMap.prevracenyT.Count - 1)];
                    prekSvisle = true;
                    break;
                case 15:
                    if (planMap.kriz.Count == 0)
                    {
                        cisloPrekazky = volnePole;
                    }
                    else
                        cisloPrekazky = planMap.kriz[r.Next(planMap.kriz.Count - 1)];
                    break;
                default:
                    break;
            }

            if (prekSvisle && prekVodorovne)
            {
                for (int y = 1; y < vyska - 1; y++)
                {
                    for (int x = 1; x < sirka - 1; x++)
                    {
                        plan[x, y] = planMap.prekazky[cisloPrekazky, sirka - 2 - x, vyska - 2 - y];
                    }
                }
            }
            else if (prekSvisle)
            {
                for (int y = 1; y < vyska - 1; y++)
                {
                    for (int x = 1; x < sirka - 1; x++)
                    {
                        plan[x, y] = planMap.prekazky[cisloPrekazky, sirka - 2 - x, y - 1];
                    }
                }
            }
            else if (prekVodorovne)
            {
                for (int y = 1; y < vyska - 1; y++)
                {
                    for (int x = 1; x < sirka - 1; x++)
                    {
                        plan[x, y] = planMap.prekazky[cisloPrekazky, x - 1, vyska - 2 - y];
                    }
                }
            }
            else
            {
                for (int y = 1; y < vyska - 1; y++)
                {
                    for (int x = 1; x < sirka - 1; x++)
                    {
                        plan[x, y] = planMap.prekazky[cisloPrekazky, x - 1, y - 1];
                    }
                }
            }
        }


        void ziskejSousedy()
        {
            if (x - 1 >= 0 && planMap.mapa[x - 1, y])
            {
                levo = true;
            }
            if (x + 1 < planMap.sirka && planMap.mapa[x + 1, y])
            {
                pravo = true;
            }
            if (y - 1 >= 0 && planMap.mapa[x, y - 1])
            {
                nahore = true;
            }
            if (y + 1 < planMap.vyska && planMap.mapa[x, y + 1])
            {
                dole = true;
            }
        }

        void inic()
        {
            for (int i = 0; i < vyska; i++)
            {
                for (int j = 0; j < sirka; j++)
                {
                    plan[j, i] = '.';
                }
            }
        }

        public void vypis()
        {
            for (int i = 0; i < vyska; i++)
            {
                for (int j = 0; j < sirka; j++)
                {
                    Console.Write(plan[j, i]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        void ohranicPlan()
        {
            for (int x = 0; x < sirka; x++)
            {
                for (int y = 0; y < vyska; y++)
                {
                    if (x == 0 || x == sirka - 1 || y == 0 || y == vyska - 1) //ohraničení
                    {
                        plan[x, y] = 'X';
                    }

                    if (levo) //místa beze zdí 
                    {
                        if (x == 0)
                        {
                            if ((y >= (vyska / 2) - 2) && (y <= (vyska / 2) + 2))
                            {

                                plan[x, y] = '.';

                            }
                        }
                    }

                    if (pravo)
                    {
                        if (x == sirka - 1)
                        {
                            if ((y >= (vyska / 2) - 2) && (y <= (vyska / 2) + 2))
                            {

                                plan[x, y] = '.';
                            }
                        }
                    }

                    if (nahore)
                    {
                        if (y == 0)
                        {
                            if ((x >= (sirka / 2) - 2 && x <= (sirka / 2) + 2))
                            {
                                plan[x, y] = '.';
                            }
                        }

                    }

                    if (dole)
                    {
                        if (y == vyska - 1)
                        {
                            if (!(x < (sirka / 2) - 2 || x > (sirka / 2) + 2))
                            {
                                plan[x, y] = '.';
                            }
                        }
                    }

                }
            }
        }
    }
}
