namespace AutoClicker;

public class Overlay : Form
{
    private const int WsExTransparent = 0x20;
    private const int WsExToolWindow = 0x80;
    private const int WsExNoActivate = 0x08000000;

    private Point _center;
    private int _radius;

    public Overlay()
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        Bounds = SystemInformation.VirtualScreen; // all monitors
        TopMost = true;
        ShowInTaskbar = false;
        TransparencyKey = Color.Magenta;
        BackColor = Color.Magenta;
        DoubleBuffered = true;
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            // click-through, no focus, not in Alt+Tab
            cp.ExStyle |= WsExTransparent | WsExToolWindow | WsExNoActivate;
            return cp;
        }
    }

    public void SetCircle(Point center, int radius)
    {
        if (center == _center && radius == _radius)
        {
            return;
        }

        InvalidateCircle();
        _center = center;
        _radius = radius;
        InvalidateCircle();
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        if (Visible)
        {
            Bounds = SystemInformation.VirtualScreen;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.DrawEllipse(Pens.Red, CircleBounds());
    }

    private void InvalidateCircle()
    {
        Rectangle bounds = CircleBounds();
        bounds.Inflate(2, 2);
        Invalidate(bounds);
    }

    // screen position minus the window origin
    private Rectangle CircleBounds() =>
        new(_center.X - Left - _radius, _center.Y - Top - _radius, 2 * _radius, 2 * _radius);
}
