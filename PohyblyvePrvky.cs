using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rogsnake
{
    abstract class PohyblivyPrvek : Prvek
    {
        public int smer;
        public abstract void UdelejKrok();
        public PohyblivyPrvek otec;
        public ocas tail = null;
    }

    class ohen : PohyblivyPrvek
    {
        int tim = 5;
        public ohen(Mapa mapa, int x, int y)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
        }
        public override void UdelejKrok()
        {
            if (tim < 0 )
            {
                mapa.ZrusPohyblivyPrvek(x, y);
            }
            tim--;
        }
    }
    class bomba : PohyblivyPrvek
    {
        bool narazila = false;
        public bomba(Mapa mapa, int x, int y)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
            smer = -1;
        }
        public override void UdelejKrok()
        {
            if (narazila ||smer == -2)
            {
                booom();
            }
            else if (smer != -1) //pohybuje se
            {
                int mx = x + mapa.smerx(smer);
                int my = y + mapa.smery(smer);
                if (mapa.JeVolno(mx,my))
                {
                    mapa.Presun(x, y, mx, my);
                }
                else
                {
                    narazila = true;
                }
            }
        }

        private void booom()
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (j == 0 && i ==0)
                    {
                        
                    }
                    else
                    {
                        mapa.exploze(x + i, y + j);
                        mapa.ZrusPohyblivyPrvek(x, y);
                    }
                }
            }
        }
    }

    class kanon : PohyblivyPrvek //c
    {
        int tim;
        public kanon(Mapa mapa, int x, int y)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
            smer = 3;
        }

        public override void UdelejKrok()
        {
            tim++;
            if (tim % 25 == 0)
            {
                int nax = mapa.smerx(smer); //random směr když se nemůže pohnout dál
                int nay = mapa.smery(smer);
                vystrelKouly(nax + x, nay + y, smer);
            }
        }
        void vystrelKouly(int mx, int my, int smerKoule)
        {
            Koule k = new Koule(mapa, mx, my, smerKoule);
            mapa.pridejKouly(mx, my);
            mapa.PohyblivePrvky.Add(k);
        }

    }

    class BombPad : PohyblivyPrvek //c
    {
        int tim;
        public BombPad(Mapa mapa, int x, int y)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
            smer = 2; //pod sebe
        }

        public override void UdelejKrok()
        {
            int nax = mapa.smerx(smer) +x; 
            int nay = mapa.smery(smer) + y;

            if (mapa.JeVolno(nax,nay))
            {
                tim++;
            }
            else
            {
                tim = 1;
            }

            if (tim % 11 == 0)
            {
                polozBonbu(nax, nay);
                tim = 1;
            }
        }
        void polozBonbu(int mx, int my)
        {
            bomba B = new bomba(mapa, mx, my);
            mapa.pridejBonbu(mx, my);
            mapa.PohyblivePrvky.Add(B);
        }

    }

    class Koule : PohyblivyPrvek //b
    {
        int tim;
        public Koule(Mapa mapa, int x, int y, int smer)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
            this.smer = smer;
            
        }

        public override void UdelejKrok()
        {
            tim++;
            if (tim % 3 == 0)
            {
                int nax = mapa.smerx(smer); //random směr když se nemůže pohnout dál
                int nay = mapa.smery(smer);
                if (mapa.JeVolno(x + nax, y + nay))
                {
                    mapa.Presun(x, y, x + nax, y + nay);
                }
                else if (mapa.JeHad(x + nax, nay + y) || mapa.JeOcas(x + nax, nay + y))
                {
                    mapa.had.zasah();
                    mapa.ZrusPohyblivyPrvek(x, y);
                }
                else
                {
                    mapa.ZrusPohyblivyPrvek(x, y);
                }
            }
        }
    }


    class ohnivaKoule : PohyblivyPrvek
    {
        int tim;
        public ohnivaKoule(Mapa mapa, int x, int y)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
            smer = 2;
        }

        public override void UdelejKrok()
        {
            tim++;
            if (tim % 3 == 0)
            {
                int nax = mapa.smerx(smer); //random směr když se nemůže pohnout dál
                int nay = mapa.smery(smer);
                if (mapa.JeVolno(x + nax, y + nay))
                {
                    mapa.Presun(x, y, x + nax, y + nay);
                }
                else if (mapa.JeHad(x + nax, nay + y) || mapa.JeOcas(x + nax, nay + y))
                {
                    mapa.had.zasah();
                    mapa.ZrusPohyblivyPrvek(x, y);
                }
                else
                {
                    smer = (smer + 2 ) % 4;
                    tim += 1;
                }
            }
        }
    }

    class duch : PohyblivyPrvek
    {
        int tim;
        public duch(Mapa mapa, int x, int y)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
            smer = mapa.rnd.Next(4);
        }

        public override void UdelejKrok()
        {
            tim++;
            if (tim % 3 == 0)
            {

                int nax = mapa.smerx(smer); //random směr když se nemůže pohnout dál
                int nay = mapa.smery(smer);
                if (mapa.JeVolno(x + nax, y + nay))
                {
                    mapa.Presun(x, y, x + nax, y + nay);
                }
                else if (mapa.JeHad(x + nax, nay + y) || mapa.JeOcas(x + nax, nay + y))
                {
                    mapa.had.zasah();
                    mapa.ZrusPohyblivyPrvek(x, y);
                    //mapa.PohyblivePrvky.Remove(this);
                    
                }
                else
                {
                    smer = mapa.rnd.Next(4);
                    tim += 2;
                    UdelejKrok();
                }

            }
        }
    }

    class ocas : PohyblivyPrvek
    {
        int zX;
        int zY;
        public ocas(Mapa mapa, PohyblivyPrvek otec, int x, int y)
        {
            this.mapa = mapa;
            this.x = x;
            this.y = y;
            mapa.VytvorOcas(x, y);
            this.otec = otec;
            mapa.PohyblivePrvky.Add(this);

        }
        public void pridejOcas()
        {
            if (tail == null)
            {
                tail = new ocas(this.mapa, this, zX, zY);

            }
            else tail.pridejOcas();
        }

        public void OcasePohyb(int naX, int naY)
        {
            zX = x;
            zY = y;
            mapa.Presun(x, y, naX, naY);
            x = naX;
            y = naY;
            if (tail != null)
            {
                tail.OcasePohyb(zX, zY);
            }
        }

        public override void UdelejKrok()
        {

        }
    }

    class Had : PohyblivyPrvek
    {
        public int kolikPridatOcasu = 2;
        public int pocetOcasu = 0;
        public int barva = 1; //zivoty
        public Had(Mapa mapa, int kdex, int kdey)
        {
            this.mapa = mapa;
            this.x = kdex;
            this.y = kdey;
            smer = -1;
        }

        public void zasah()
        {
            barva--;
            if (barva < 0)
            {
                barva = 0;
                mapa.stav = Stav.prohra;
            }
        }
        public override void UdelejKrok()
        {
            int sx = 0;
            int sy = 0;

            switch (mapa.stisknutaSipka)
            {
                case StisknutaSipka.zadna:
                    break;
                case StisknutaSipka.doleva:
                    if (smer != 1)
                    {
                        sx--;
                        smer = 3;
                    }
                    else sx++;
                    break;
                case StisknutaSipka.nahoru:
                    if (smer != 2)
                    {
                        sy--;
                        smer = 0;
                    }
                    else sy++;
                    break;
                case StisknutaSipka.doprava:
                    if (smer != 3)
                    {
                        sx++;
                        smer = 1;
                    }
                    else sx--;
                    break;
                case StisknutaSipka.dolu:
                    if (smer != 0)
                    {
                        sy++;
                        smer = 2;
                    }
                    else sy--;
                    break;
                default:
                    break;
            }

            int mx = x + sx;
            int my = y + sy;

            if (mapa.JeVolno(mx, my))
            {
                mapa.Presun(x, y, mx, my);
                posunOcas(x - sx, y - sy);
            }
            else
            if (mapa.JePickables(mx, my))
            {
                mapa.SeberPickable(mx, my, this);
                mapa.Presun(x, y, mx, my);
                posunOcas(x - sx, y - sy); //x a y už je aktualyzovaný / posunutý
                
            }
            else if (mapa.JeOtevrenyVychod(mx, my))
            {
                mapa.stav = Stav.vyhra;
            }
            else if (mapa.JeBomba(mx , my) && mapa.JeVolno(mx +sx , my +sy))
            {
                mapa.pohniBonbou(mx, my, smer);
            }
            else if (sx + sy != 0)
            {
                mapa.stav = Stav.prohra;
            }


            if (kolikPridatOcasu > 0 && sx + sy != 0) //musí se pohybovat pro přidávání ocasů
            {
                kolikPridatOcasu--;
                pocetOcasu++;
                pridejOcas(x - sx, y - sy);
            }

        }

        private void posunOcas(int naX, int naY)
        {
            if (tail != null)
                tail.OcasePohyb(naX, naY);
        }

        void pridejOcas(int x, int y)
        {
            if (tail == null)
            {
                tail = new ocas(this.mapa, this, x, y);
            }
            else tail.pridejOcas();
        }

        public void pridejOcas()
        {
            kolikPridatOcasu++;
        }
    }

}
