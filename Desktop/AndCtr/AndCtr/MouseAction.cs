using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndCtr
{
    public class MouseAction
    {
        WndCursorController CurCtr = null;
        delegate void MouseEventDelegate(String Event);
        public MouseAction()
        {
            CurCtr = new WndCursorController();
        }
        private void onMouseAction(String Event)
        {
            String[] CmdStr = Event.Split('\n');
            String[] str = CmdStr[0].Split(' ');
            switch (str[0])
            {
                case "Set":
                    int x0 = 0, y0 = 0;
                    WndCursorController.GetCursorPos(ref x0, ref y0);

                    int x = int.Parse(str[1]);
                    int y = int.Parse(str[2]);
                    x = x0 + x;
                    y = y0 + y;
                    CurCtr.SetCursorPos(x, y);
                    break;
                case "LDown": CurCtr.onLeftMouseDown(); break;
                case "LUp": CurCtr.onLeftMouseUp(); break;
                case "LClick": CurCtr.onLeftMouseClick(); break;
                case "RDown": CurCtr.onRightMouseDown(); break;
                case "RUp": CurCtr.onRightMouseUp(); break;
                case "RClick": CurCtr.onRightMouseClick(); break;
                default: Console.WriteLine("Wrong Event String"); break;
            }
        }

        public void OnEvent(String Event)
        {
            MouseEventDelegate Dl = new MouseEventDelegate(onMouseAction);
            Dl(Event);
        }
    }
}
