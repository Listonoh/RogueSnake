using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;


namespace Rogsnake
{
    static class CteckaPokoju
    {
        public static void CtecPokoju(string cesta , string cestaPNG)
        {
            Bitmap mapa = new Bitmap(cestaPNG);
            FileStream fs = File.Create(cesta);
            Byte[] info;
            int vyska = mapa.Height / 14;
            int sirka = mapa.Width / 17;
            string s = "64";
            s += "\r\n";
            info = new UTF8Encoding(true).GetBytes(s);
            fs.Write(info, 0, info.Length);

            s = "";
            for (int i = 0; i < vyska; i++)
            {

                for (int j = 0; j < sirka; j++)
                {

                    for (int y = 0; y < 14; y++)
                    {
                        s = "";
                        for (int x = 0; x < 17; x++)
                        {
                            Color k = mapa.GetPixel(j * 17 + x, i * 14 + y);
                            int barva = k.ToArgb();
                            switch (k.A)
                            {
                                case 255:
                                    if (barva == Color.Black.ToArgb())
                                    {
                                        s += 'X';
                                    }
                                    else if (barva == Color.Purple.ToArgb())
                                    {
                                        s += 'B';
                                    }
                                    else if (barva == Color.Red.ToArgb())
                                    {
                                        s += '1';
                                    }
                                    else if (barva == Color.Blue.ToArgb())
                                    {
                                        s += 'A';
                                    }
                                    else if (barva == Color.Orange.ToArgb())
                                    {
                                        s += '2';
                                    }
                                    else if (barva == Color.Green.ToArgb())
                                    {
                                        s += 'j';
                                    }
                                    else if (barva == Color.Aqua.ToArgb())
                                    {
                                        s += 'D';
                                    }
                                    else if (barva == Color.GreenYellow.ToArgb())
                                    {
                                        s += 'c';
                                    }
                                    else
                                    {
                                        s += '?';
                                    }
                                    break;

                                default:
                                    s += '.';
                                    break;
                            }
                        }
                        s += "\r\n";
                        //s += '\n';
                        info = new UTF8Encoding(true).GetBytes(s);
                        fs.Write(info, 0, info.Length);
                    }

                    s = "\r\n";
                    info = new UTF8Encoding(true).GetBytes(s);
                    fs.Write(info, 0, info.Length);

                }
            }
            fs.Close();
        }
    }
}
