using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Eryan
{
    public class Mouse : InputDevice
    {
        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMoveMouse(IntPtr handle, int x, int y);

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMouseClick(IntPtr handle, int x, int y);

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllCalcTest(IntPtr handle);

        Utils screen = null;
        ZiadSpace.Util.BitHelper bh = new ZiadSpace.Util.BitHelper();


        //Mouse state
        int X = 0, Y = 0;
        int speed = 4;
        int defRandX = 1;
        int defRandY = 1;
        int missChance = 10;

        //Windows API messages
        public enum WMessages : uint
        {
            WM_MOUSEMOVE = 	0x200,  //Mouse Move
            WM_LBUTTONDOWN = 0x201, //Left mousebutton down
            WM_LBUTTONUP = 0x202,  //Left mousebutton up
            WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
            WM_RBUTTONDOWN = 0x204, //Right mousebutton down
            WM_RBUTTONUP = 0x205,   //Right mousebutton up
            WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
            WM_KEYDOWN = 0x100,  //Key down
            WM_KEYUP = 0x101,   //Key up
            WM_NCHITTEST = 0x84,
            WM_SETCURSOR = 0x20,
        }

        public Mouse()
        {
        }

        public int getX()
        {
            return X;
        }

       

        public int getY()
        {
            return Y;
        }

        public int getSpeed()
        {
            return speed;
        }

        public Point cursorLocation()
        {
            return new Point(X, Y);
        }

        //Randomness variables
        const int DEFAULT_MAX_MOVE_AFTER = 5;

        public void click(bool leftClick)
        {
            click(leftClick, DEFAULT_MAX_MOVE_AFTER);
        }

        public void click(bool leftClick, int move_after)
        {
            click(X, Y, leftClick, move_after);
        }

        //Implementation method

        public void click(int x, int y, bool leftClick, int move_after)
        {
            //SendEvent stuff goes here
        }

        public void move(int x, int y, int randX, int randY)
        {
            move(speed, x, y, randX, randY, 0);
        }

        public void move(int speed, Point p)
        {
            move(speed, p.X, p.Y, 0, 0, 0);
        }

        public void move(Point p)
        {
            move(p.X, p.Y, 0, 0);
        }

        public void move(Point p, int randX, int randY)
        {
            move(p.X, p.Y, randX, randY);
        }

        public void move(Point p, int randX, int randY, int afterOffset)
        {
            move(getSpeed(), p.X, p.Y, randX, randY, afterOffset);
        }


        public double hypot(double xs, double xe, double ys, double ye)
        {
            double x = Math.Pow(xs - xe, 2);
            double y = Math.Pow(ys - ye, 2);
            return Math.Sqrt(x + y);
        }

        public void WindMouse(int xs, int ys, int xe, int ye, double gravity, double wind, double minWait, double maxWait, double maxStep, double targetArea)
        {
            double veloX = 0, veloY = 0, windX = 1, windY = 1, veloMag, dist, randomDist, lastDist, step;
            int lastX, lastY;
            double sqrt2, sqrt3, sqrt5;

            sqrt2 = Math.Sqrt(2);
            sqrt3 = Math.Sqrt(3);
            sqrt5 = Math.Sqrt(5);
            
            while(hypot(xs, xe, ys, ye) > 1)
            {
                dist = hypot(xs, xe, ys, ye);
                wind = Math.Min(wind, dist);
                if(dist >= targetArea)
                {
                    int tempWindX = (int)(Math.Round( (decimal)wind) * 2 + 1) - (int)wind;
                    int tempWindY = (int)(Math.Round((decimal)wind) * 2 + 1) - (int)wind;
                    windX = windX / sqrt3 + (random.Next( (int)(tempWindX) ) / sqrt5);
                    windY = windY / sqrt3 + (random.Next( (int)(tempWindY) ) / sqrt5);
                }
                else
                {
                    windX = windX / sqrt2;
                    windY = windY / sqrt2;
                    if(maxStep < 3)
                    {
                        maxStep = random.Next(3) + 3.0;
                    }
                    else
                    {
                        maxStep = maxStep / sqrt5;
                    }
                }
                veloX = veloX + windX;
                veloY = veloY + windY;
                veloX = veloX + gravity * (xe - xs) / dist;
                veloY = veloY + gravity * (ye - ys) / dist;

                if(hypot(veloX, 0, veloY, 0) > maxStep)
                {
                    randomDist = maxStep / 2.0 + random.Next( (int)Math.Round(maxStep) / 2);
                    veloMag = Math.Sqrt(veloX * veloX + veloY * veloY);
                    veloX = (veloX / veloMag) * randomDist;
                    veloY = (veloY / veloMag) * randomDist;
                }
                lastX = (int)Math.Round((decimal)xs);
                lastY = (int)Math.Round((decimal)ys);

                xs = (int)(xs + veloX);
                ys = (int)(ys + veloY);

                if(lastX != (int)Math.Round((decimal)xs) || lastY != (int)Math.Round((decimal)ys))
                {
                    moveMouse( new Point((int)Math.Round((decimal)xs), (int)Math.Round((decimal)ys)));
                }
                step = hypot(xs, lastX, ys, lastY);
                System.Threading.Thread.Sleep( (int)Math.Round(maxWait - minWait) * (int)(step /maxStep) + (int)minWait);
                lastDist = dist;
            }
            if(Math.Round((decimal)xe) != Math.Round((decimal)xs) || Math.Round((decimal)ye) != Math.Round((decimal)ye))
                moveMouse(new Point( (int)Math.Round((decimal)xe), (int)Math.Round((decimal)ye)));
            

        }


        public void moveMouse(Point p)
        {
           
            screen = fetchScreen(getPid());
            if (screen == null)
            {
               // Console.WriteLine("screen is null");
               // Console.WriteLine("Mouse PID: " + getPid());
                return;
            }

            
            if (appWin == IntPtr.Zero)
            {
                //Console.WriteLine("Mouse appWin is null1: " + getPid());
                return ;
            }
            

            this.X = p.X;
            this.Y = p.Y;

       

            //PostMessage(appWin, (int)WMessages.WM_NCHITTEST, 0, 0x009803CE);
            //SendMessage(appWin, (int)WMessages.WM_SETCURSOR, (long)appWin, ZiadSpace.Util.BitHelper.MakeLong(300, 200));
            //PostMessage(hWndCalc, (int)WMessages.WM_MOUSEMOVE, 0, ZiadSpace.Util.BitHelper.MakeDword(300, 200));

            //PostMessage(hWndCalc, (int)WMessages.WM_LBUTTONDOWN, 0, ZiadSpace.Util.BitHelper.MakeDword(300, 200));
            //PostMessage(hWndCalc, (int)WMessages.WM_LBUTTONUP, 0, ZiadSpace.Util.BitHelper.MakeDword(300,200));

            screen.drawLine(Pens.BurlyWood, p, new Point(p.X + 5, p.Y));
            //dllMoveMouse(appWin, X, Y);
            //dllMouseClick(appWin, X, Y);

            dllMoveMouse(appWin, p.X, p.Y);
            

        }


        public bool atPosition(int x, int y)
        {
            if (cursorLocation().X >= x - 5 && cursorLocation().X <= x + 5)
                if (cursorLocation().Y >= y - 5 && cursorLocation().Y <= y + 5)
                    return true;
            return false;
        }

        public int cursorDistance(Point p)
        {
            return (int)hypot(p.X, this.X, p.Y, this.Y);
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
                 //   Console.WriteLine("screen is null");
                 //   Console.WriteLine("Mouse PID: " + getPid());
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

                f = (int)Math.Round(a / seg);
                g = (int)Math.Round(b / seg);

                while (e != seg)
                {
                    System.Threading.Thread.Sleep(30 + random.Next(50));
                    randSpeed = (random.Next(speed) / 2.0 + speed) / 10.0;
                    if (randSpeed == 0)
                        randSpeed = 0.1;

                    nx = (cursorLocation().X + (f * e)) + random.Next(randX);
                    ny = (cursorLocation().Y + (g * e)) + random.Next(randY);

                    if (miss)
                    {
                        nx = nx + random.Next(randX, randX * 2);
                        ny = ny + random.Next(randY, randY * 2);
                    }
                    WindMouse(cx, cy, nx, ny, 11, 8, 10 / randSpeed, 12 / randSpeed, 10 * randSpeed, 10 * randSpeed);
                    e++;
                }
                cx = cursorLocation().X;
                cy = cursorLocation().Y;

                if(!atPosition(x,y))
                {
                    System.Threading.Thread.Sleep(30 + random.Next(30));
                    WindMouse(cx, cy, (x+random.Next(randX)), (y + random.Next(randY)), 11, 6, 10/randSpeed, 15/randSpeed, 10*randSpeed, 10*randSpeed);
                }

 //               PostMessage(appWin, (int)WMessages.WM_LBUTTONDOWN, 0, MakeLParam(x, y));
 //               PostMessage(appWin, (int)WMessages.WM_LBUTTONUP, 0, MakeLParam(x, y));

            }
        }
    
    }
}
