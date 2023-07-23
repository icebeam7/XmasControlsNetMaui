using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace XmasControlsNetMaui.Controls;

public partial class XmasTreeControl : ContentView
{
    public XmasTreeControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty LeavesBrushProperty =
                    BindableProperty.Create(nameof(LeavesBrush),
                        typeof(Brush),
                        typeof(XmasTreeControl),
                        Brush.Transparent,
                        BindingMode.TwoWay,
                        propertyChanged: OnLeavesBrushChanged);

    public Brush LeavesBrush
    {
        get => (Brush)GetValue(LeavesBrushProperty);
        set { SetValue(LeavesBrushProperty, value); }
    }

    public static readonly BindableProperty TrunkBrushProperty =
        BindableProperty.Create(nameof(TrunkBrush),
            typeof(Brush),
            typeof(XmasTreeControl),
            Brush.Transparent,
            BindingMode.TwoWay,
            propertyChanged: OnTrunkBrushChanged);

    public Brush TrunkBrush
    {
        get => (Brush)GetValue(TrunkBrushProperty);
        set { SetValue(TrunkBrushProperty, value); }
    }

    private static void OnLeavesBrushChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var xmasTreeControl = bindable as XmasTreeControl;
        xmasTreeControl.FillBrush(xmasTreeControl.Content as AbsoluteLayout, newValue as Brush, 3);
    }

    private static void OnTrunkBrushChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var xmasTreeControl = bindable as XmasTreeControl;
        xmasTreeControl.FillBrush(xmasTreeControl.Content as AbsoluteLayout, newValue as Brush, 4);
    }

    void FillBrush(AbsoluteLayout layout, Brush brush, int pointsCount)
    {
        foreach (var item in layout.Children)
        {
            var control = item as Polygon;

            if (control != null)
            {
                if (control.Points.Count == pointsCount)
                {
                    control.Fill = brush;
                    control.Stroke = brush;
                }
            }
        }
    }

    List<Tuple<int, int, bool>> positions = new List<Tuple<int, int, bool>>()
        {
            new Tuple<int, int, bool>(0, 75, false),
            new Tuple<int, int, bool>(75, 25, false),
            new Tuple<int, int, bool>(100, 100, false),
            new Tuple<int, int, bool>(0, 175, false),
            new Tuple<int, int, bool>(150, 175, false),
        };

    private void OnDrop(object sender, DropEventArgs e)
    {

        var properties = e.Data.Properties;

        if (properties.ContainsKey("Sphere"))
        {

            var sphere = (Ellipse)properties["Sphere"];

            var xmasSphere = new XmasSphereControl()
            {
                SphereBrush = sphere.Fill
            };

            var layout = this.Content as AbsoluteLayout;

            var position = positions.FirstOrDefault(x => !x.Item3);

            if (position != null)
            {

                layout.SetLayoutBounds((IView)xmasSphere,
                    new Rect(new Point(position.Item1, position.Item2), new Size(xmasSphere.Width, xmasSphere.Height)));

                layout.Children.Add(xmasSphere);
                positions[positions.IndexOf(position)] = new Tuple<int, int, bool>(position.Item1, position.Item2, true);
            }

        }
        else if (properties.ContainsKey("Star"))
        {

            var star = (Path)properties["Star"];

            var xmasStar = new XmasStarControl()
            {
                StarStroke = star.Stroke
            };

            var layout = this.Content as AbsoluteLayout;

            layout.SetLayoutBounds((IView)xmasStar,
                new Rect(new Point(90, -5), new Size(xmasStar.Width, xmasStar.Height)));

            layout.Children.Add(xmasStar);

        }
    }
}