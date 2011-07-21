using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Eryan.Input
{
    /// <summary>
    /// Mouse input device
    /// </summary>
    public class Mouse : InputDevice
    {
        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMoveMouse(IntPtr handle, int x, int y);

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMouseClick(IntPtr handle, bool left, int x, int y);

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMouseButtonDown(IntPtr handle, bool left, int x, int y);

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMouseButtonUp(IntPtr handle, bool left, int x, int y);

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllCalcTest(IntPtr handle);

        protected Utils screen = null;
        ZiadSpace.Util.BitHelper bh = new ZiadSpace.Util.BitHelper();


        //Mouse state
        protected int X = 0, Y = 0;
        protected int speed = 4;
        protected int defRandX = 1;
        protected int defRandY = 1;
        protected int missChance = 10;

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

        public uint Pid
        {
            get
            {
                return pid;
            }
        }

        public void releaseLeftButton()
        {
            dllMouseButtonUp(appWin, true, x, y);
        }

        public void releaseRightButton()
        {
            dllMouseButtonUp(appWin, false, x, y);
        }

        public void holdLeftButton()
        {
            dllMouseButtonDown(appWin, true, x, y);
        }

        public void holdRightButton()
        {
            dllMouseButtonDown(appWin, false, x, y);
        }

        public int MissChance
        {
            get
            {
                return missChance;
            }
            set
            {
                missChance = value;
            }
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }


        public Mouse()
        {
        }

        /// <summary>
        /// Get mouse position on the X axis
        /// </summary>
        /// <returns>The X coordinate of the mouse cursor</returns>
        public int getX()
        {
            return X;
        }


        public IntPtr APPWIN
        {
            get
            {
                return appWin;
            }
        }

        /// <summary>
        /// Get mouse position on the Y axis
        /// </summary>
        /// <returns>The Y coordinate of the mouse cursor</returns>
        public int getY()
        {
            return Y;
        }

           

        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }


        /// <summary>
        /// Gets the mouse speed
        /// </summary>
        /// <returns>The current mouse speed</returns>
        public int getSpeed()
        {
            return speed;
        }

        /// <summary>
        /// The current cursor location
        /// </summary>
        /// <returns>A point relative to the EVE window</returns>
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

        
        
        /// <summary>
        /// Clicks the given coordinates
        /// </summary>
        /// <param name="x">The position in the x axis to click</param>
        /// <param name="y">The position in the y axis to click</param>
        /// <param name="leftClick">Whether its a left click or right click</param>
        /// <param name="move_after">How much it should move after clicking</param>
        public void click(int x, int y, bool leftClick, int move_after)
        {
            Console.WriteLine("Clicking X,Y " + X + "," + Y);
            dllMouseClick(appWin, leftClick, x, y);
            //move mouse after
        }

        /// <summary>
        /// Move the mouse to the given coordinates with randomization
        /// </summary>
        /// <param name="x">The position in the x axis to move to</param>
        /// <param name="y">The position in the y axis to move to</param>
        /// <param name="randX">The random deviation along the X axis</param>
        /// <param name="randY">The random deviation along the Y axis</param>
        public void move(int x, int y, int randX, int randY)
        {
            move(speed, x, y, randX, randY, 0);
        }


        /// <summary>
        /// Move the mouse to a poitn with the given speed
        /// </summary>
        /// <param name="speed">The speed at which to move</param>
        /// <param name="p">The point to move to</param>
        public void move(int speed, Point p)
        {
            move(speed, p.X, p.Y, 0, 0, 0);
        }

        /// <summary>
        /// Mouse the mouse to the point
        /// </summary>
        /// <param name="p">Point to move to</param>
        public void move(Point p)
        {
            move(p.X, p.Y, 0, 0);
        }

        /// <summary>
        /// Move the mouse to the given point with the given randomization
        /// </summary>
        /// <param name="p">The point to move to</param>
        /// <param name="randX">The deviation along the X axis</param>
        /// <param name="randY">The deviation along the Y axis</param>
        public void move(Point p, int randX, int randY)
        {
            move(p.X, p.Y, randX, randY);
        }

        /// <summary>
        /// Move the mouse to the given point with randomization and movement after
        /// </summary>
        /// <param name="p">The point to move</param>
        /// <param name="randX">The deviation along the X axis</param>
        /// <param name="randY">The deviation along the Y axis</param>
        /// <param name="afterOffset">How much to move by after it gets there</param>
        public void move(Point p, int randX, int randY, int afterOffset)
        {
            move(getSpeed(), p.X, p.Y, randX, randY, afterOffset);
        }

        /// <summary>
        /// Gets the hypotenuse of the triangle made by two points
        /// </summary>
        /// <param name="xs">X coordinate of point 1</param>
        /// <param name="xe">X coordinate of point 2</param>
        /// <param name="ys">Y coordinate of point 1</param>
        /// <param name="ye">Y coordinate of point 2</param>
        /// <returns>The hypotenuse</returns>
        public double hypot(double xs, double xe, double ys, double ye)
        {
            double x = Math.Pow(xs - xe, 2);
            double y = Math.Pow(ys - ye, 2);
            return Math.Sqrt(x + y);
        }

        /// <summary>
        /// Insane mouse logic
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="ys"></param>
        /// <param name="xe"></param>
        /// <param name="ye"></param>
        /// <param name="gravity"></param>
        /// <param name="wind"></param>
        /// <param name="minWait"></param>
        /// <param name="maxWait"></param>
        /// <param name="maxStep"></param>
        /// <param name="targetArea"></param>


        

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

        public int x
        {
            get
            {
                return X;
            }
            set
            {
                X = value;
            }
        }

        public int y
        {
            get
            {
                return Y;
            }
            set
            {
                Y = value;
            }
        }


        /// <summary>
        /// Move the mouse to Point p
        /// </summary>
        /// <param name="p">The point to move the mouse to</param>
        public void moveMouse(Point p)
        {
           
            screen = fetchScreen(getPid());
            if (screen == null)
            {
                Console.WriteLine("Screen is null");
                return;
            }

            
            if (appWin == IntPtr.Zero)
            {
                Console.WriteLine("Mouse appWin is null1: " + getPid());
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

        /// <summary>
        /// Check whether the mouse it at the given x and y coordinates
        /// </summary>
        /// <param name="x">X coordinate to check</param>
        /// <param name="y">Y coordinate to check</param>
        /// <returns>Returns true if the mouse is within the 5 pixel square of the given coordinates</returns>
        public bool atPosition(int x, int y)
        {
            if (cursorLocation().X >= x - 5 && cursorLocation().X <= x + 5)
                if (cursorLocation().Y >= y - 5 && cursorLocation().Y <= y + 5)
                    return true;
            return false;
        }

        /// <summary>
        /// Get how far the mouse is from the given point
        /// </summary>
        /// <param name="p">The point to test against</param>
        /// <returns>The distance between the mouse and the given point</returns>
        public int cursorDistance(Point p)
        {
            return (int)hypot(p.X, this.X, p.Y, this.Y);
        }


        /// <summary>
        /// More insane mouse logic
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="randX"></param>
        /// <param name="randY"></param>
        /// <param name="afterOffset"></param>

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
