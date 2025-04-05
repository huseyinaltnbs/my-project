using System;
using System.Drawing;
using System.Windows.Forms;

public class MouseDragHelper
{
    private bool mouseDown;
    private Point lastLocation;
    private Form form;

    public MouseDragHelper(Form form)
    {
        this.form = form;
    }

    public void Attach(Control control)
    {
        control.MouseDown += (sender, e) =>
        {
            mouseDown = true;
            lastLocation = e.Location;
        };

        control.MouseMove += (sender, e) =>
        {
            if (mouseDown)
            {
                form.Location = new Point(
                    (form.Location.X - lastLocation.X) + e.X,
                    (form.Location.Y - lastLocation.Y) + e.Y);

                form.Update();
            }
        };

        control.MouseUp += (sender, e) => { mouseDown = false; };
    }
}
