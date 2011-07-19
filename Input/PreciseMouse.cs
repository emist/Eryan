using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Input 
{

    /// <summary>
    /// More precise implementation of Mouse class, for interactions that require more accuraccy, at the expense of randomization.
    /// </summary>

    public class PreciseMouse : Mouse
    {
        Mouse m;

        public PreciseMouse()
        {
        /*    this.m = m;
            this.Speed = m.Speed;
            this.pid = m.Pid;
            this.X = m.getX();
            this.Y = m.getY();
            this.appWin = m.APPWIN;
            */
        }


        public void move(int speed, int x, int y, int randX, int randY, int afterOffset)
        {

            int e = 0, f = 0, g = 0, nx = 0, ny = 0, hypo = 0, seg = 0, cx = 0, cy = 0;
            double a = 0, b = 0, c = 0, randSpeed = 0;
            bool miss = false;



            cx = cursorLocation().X;
            cy = cursorLocation().Y;

            if (x != -1 || y != -1)
            {

                screen = fetchScreen(getPid());


                if (screen == null)
                {
                    Console.WriteLine("screen is null");
                    Console.WriteLine("Mouse PID: " + getPid());
                    return;
                }

                miss = random.Next(missChance) == 0;
                a = x - cx;
                b = y - cy;
                c = Math.Pow(a, 2) + Math.Pow(b, 2);
                hypo = (int)Math.Round(Math.Sqrt(c));

                if (hypo == 0)
                    return;
                else if (hypo > 1 && hypo < 225)
                    seg = 1;
                else if (hypo > 225 && hypo < 600)
                    seg = random.Next(2) + 1;
                else if (hypo > 600 && hypo < 1800)
                    seg = random.Next(3) + 2;
                else
                    seg = 5;

                Console.WriteLine("Seg = " + seg);

                f = (int)Math.Round(a / seg);
                g = (int)Math.Round(b / seg);

                while (e != seg)
                {
                    System.Threading.Thread.Sleep(30 + random.Next(50));
                    randSpeed = (random.Next(speed) / 2.0 + speed) / 10.0;
                    if (randSpeed == 0)
                        randSpeed = 0.1;

                    nx = (cursorLocation().X + (f * e)) + random.Next(randX, randX);
                    ny = (cursorLocation().Y + (g * e)) + random.Next(randY, randY);

                    if (miss)
                    {
                        nx = nx + random.Next(randX, randX * 2);
                        ny = ny + random.Next(randY, randY * 2);
                    }
                    WindMouse(cx, cy, nx, ny, 11, 1, 10 / randSpeed, 12 / randSpeed, 10 * randSpeed, 10 * randSpeed);
                    e++;
                }
                cx = cursorLocation().X;
                cy = cursorLocation().Y;

                if (!atPosition(x, y))
                {
                    System.Threading.Thread.Sleep(30 + random.Next(30));
                    WindMouse(cx, cy, (x + random.Next(randX)), (y + random.Next(randY)), 11, 1, 5 / randSpeed, 10 / randSpeed, 10 * randSpeed, 10 * randSpeed);
                }

                //               PostMessage(appWin, (int)WMessages.WM_LBUTTONDOWN, 0, MakeLParam(x, y));
                //               PostMessage(appWin, (int)WMessages.WM_LBUTTONUP, 0, MakeLParam(x, y));

            }
        }


    }
}
