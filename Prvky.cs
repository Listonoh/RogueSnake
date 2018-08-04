using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogsnake
{
    abstract class Prvek
    {
        public Mapa mapa;
        public int x;
        public int y;
    }

    abstract class Pickable : Prvek
    {
        public abstract void Pick(Had had);
    }

    class Sutr : Prvek
    {
        public Sutr(Mapa mapa, int kdex, int kdey)
        {
            this.mapa = mapa;
            this.x = kdex;
            this.y = kdey;
        }
    }

    class Jablko : Pickable
    {
        public Jablko(Mapa mapa, int kdex, int kdey)
        {
            this.mapa = mapa;
            this.x = kdex;
            this.y = kdey;
        }

        public override void Pick(Had had)
        {
            had.pridejOcas();
            //mapa.VytvorJablko();
            mapa.skore += 100;
        }
    }

    class armor : Pickable
    {
        public armor(Mapa mapa, int kdex, int kdey)
        {
            this.mapa = mapa;
            this.x = kdex;
            this.y = kdey;
        }

        public override void Pick(Had had)
        {
            if (had.barva < 2)
            {
            had.barva++;
            }
            mapa.skore += 100;
        }
    }

    class Diamant : Pickable
    {
        public Diamant(Mapa mapa, int kdex, int kdey)
        {
            this.mapa = mapa;
            this.x = kdex;
            this.y = kdey;
        }

        public override void Pick(Had had)
        {

            mapa.skore += 500;
        }
    }

    class Klic : Pickable
    {
        public Klic(Mapa mapa, int kdex, int kdey)
        {
            this.mapa = mapa;
            this.x = kdex;
            this.y = kdey;
        }

        public override void Pick(Had had)
        {
            mapa.OtevriVychod();
            mapa.skore += 50;
            mapa.pickables.Remove(this);
        }
    }

    class OdpalPlosina : Prvek
    {
        public OdpalPlosina(Mapa mapa, int kdex, int kdey)
        {
            this.mapa = mapa;
            this.x = kdex;
            this.y = kdey;
        }
    }



}
