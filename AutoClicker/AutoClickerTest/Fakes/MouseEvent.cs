using System.Windows.Forms;

namespace AutoClickerTest.Fakes;

internal sealed record MouseEvent(string Kind, int X, int Y, MouseButtons Button = MouseButtons.None);
