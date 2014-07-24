using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AndCtr
{
    public class WndCursor
    {
        [Flags]
        public enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("User32")]
        public extern static bool GetCursorPos(ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);
    }

    public class WndCursorController
    { 
        /// <summary>
        /// 设置鼠标于屏幕的绝对位置，以左上角为(0,0)点。
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public void SetCursorPos(int x, int y)
        {
            WndCursor.SetCursorPos(x, y);
        }
        /// <summary>
        /// 获取鼠标在屏幕的绝对位置，左上角为(0,0)点
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public static void GetCursorPos(ref int x, ref int y)
        {
            WndCursor.POINT p = new WndCursor.POINT(0,0);
            WndCursor.GetCursorPos(ref p);
            x = p.X;
            y = p.Y;
        }
        /// <summary>
        /// 触发鼠标（左键）按下事件
        /// </summary>
        public void onLeftMouseDown()
        {
            WndCursor.mouse_event(WndCursor.MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 触发鼠标（左键）弹起事件
        /// </summary>
        public void onLeftMouseUp()
        {
            WndCursor.mouse_event(WndCursor.MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 触发鼠标（右键）按下事件
        /// </summary>
        public void onRightMouseDown()
        {
            WndCursor.mouse_event(WndCursor.MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 触发鼠标（右键）弹起事件
        /// </summary>
        public void onRightMouseUp()
        {
            WndCursor.mouse_event(WndCursor.MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 触发鼠标（左键）单击事件
        /// </summary>
        public void onLeftMouseClick()
        {
            WndCursor.mouse_event(WndCursor.MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            WndCursor.mouse_event(WndCursor.MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 触发鼠标（右键）单击事件
        /// </summary>
        public void onRightMouseClick()
        {
            WndCursor.mouse_event(WndCursor.MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
            WndCursor.mouse_event(WndCursor.MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
        }
    }
}
