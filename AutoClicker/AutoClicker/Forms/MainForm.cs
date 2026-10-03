using Gma.System.MouseKeyHook;

namespace AutoClicker;

public partial class MainForm : Form
{
    private readonly IKeyboardMouseEvents _hook = Hook.GlobalEvents();
    private readonly AutoClickerManager _autoClickerManager;
    private readonly AutoClickerSettings _autoClickerSettings;
    private readonly Overlay _overlayForm = new();
    private Point _lastPhysicalMousePosition = Cursor.Position; // not the jittered one
    private readonly HotkeyDetector _hotkey = new();

    public MainForm()
    {
        InitializeComponent();

        _hook.MouseMove += MouseHook_MouseMove;
        _hook.KeyDown += GlobalKeyboardHook_KeyDown;
        _hook.KeyUp += GlobalKeyboardHook_KeyUp;

        _autoClickerSettings = new AutoClickerSettings();
        _autoClickerSettings.Load();
        _autoClickerManager = new AutoClickerManager(_autoClickerSettings);
        _autoClickerManager.ProblemDetected += AutoClickerManager_ProblemDetected;

        currentCursorPositionRadioButton.CheckedChanged += CurrentCursorPositionRadioButton_CheckedChanged;
        xNumericUpDown.ValueChanged += (_, _) => RefreshOverlay();
        yNumericUpDown.ValueChanged += (_, _) => RefreshOverlay();
        jitterRadiusNumericUpDown.ValueChanged += (_, _) => RefreshOverlay();

        // populate before LoadSettings
        PopulateKeysDropdown();
        startStopKeyComboBox.SelectedIndex = 0;
        mouseButtonComboBox.Items.AddRange([MouseButtons.Left, MouseButtons.Middle, MouseButtons.Right]);
        mouseButtonComboBox.SelectedIndex = 0;
    }

    private void PopulateKeysDropdown()
    {
        startStopKeyComboBox.Items.Add(Keys.Escape);

        // not the whole Keys enum, it has aliases and mouse buttons
        AddKeys(Keys.F1, Keys.F24);
        AddKeys(Keys.A, Keys.Z);
        AddKeys(Keys.D0, Keys.D9);

        foreach (Keys key in new[]
        {
            Keys.Pause, Keys.Scroll, Keys.Insert, Keys.Delete,
            Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown
        })
        {
            startStopKeyComboBox.Items.Add(key);
        }
    }

    private void AddKeys(Keys first, Keys last)
    {
        for (Keys key = first; key <= last; key++)
        {
            startStopKeyComboBox.Items.Add(key);
        }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        LoadSettings();
        notifyIcon1.Icon = SystemIcons.Application;
        notifyIcon1.Visible = true;
    }

    private Hotkey? CurrentHotkey() =>
        startStopKeyComboBox.SelectedItem is Keys key ? new Hotkey(key, SelectedModifier()) : null;

    private Keys SelectedModifier() =>
        ctrlRadioButton.Checked ? Keys.Control : altRadioButton.Checked ? Keys.Alt : Keys.Shift;

    private void GlobalKeyboardHook_KeyDown(object? sender, KeyEventArgs e)
    {
        if (CurrentHotkey() is not { } hotkey)
        {
            return;
        }

        HotkeyPress press = _hotkey.OnKeyDown(hotkey, e.KeyCode, e.Modifiers);
        if (press == HotkeyPress.None)
        {
            return;
        }

        // keeps the hotkey away from whichever app has focus
        e.Handled = true;

        if (press == HotkeyPress.Pressed)
        {
            ToggleClicking();
        }
    }

    private void GlobalKeyboardHook_KeyUp(object? sender, KeyEventArgs e)
    {
        if (CurrentHotkey() is { } hotkey && _hotkey.OnKeyUp(hotkey, e.KeyCode))
        {
            e.Handled = true;
        }
    }

    private void ToggleClicking()
    {
        SaveSettings();

        bool currentlyActive = _autoClickerManager.Toggle();
        if (currentlyActive)
        {
            Hide();
        }
        else
        {
            RestoreWindow();
        }

        SetTrayStatus(currentlyActive);
    }

    private void RestoreWindow()
    {
        Show();
        WindowState = FormWindowState.Normal;

        // Activate alone only flashes the taskbar while another app has focus
        TopMost = true;
        TopMost = false;
        Activate();
    }

    private void SetTrayStatus(bool clicking) =>
        notifyIcon1.Text = clicking ? "AutoClicker - clicking" : "AutoClicker - stopped";

    // raised on the worker thread
    private void AutoClickerManager_ProblemDetected(object? sender, ClickerProblem problem)
    {
        if (IsHandleCreated)
        {
            BeginInvoke(() => ShowProblem(problem));
        }
    }

    private void ShowProblem(ClickerProblem problem)
    {
        if (problem.Stopped)
        {
            RestoreWindow();
            SetTrayStatus(clicking: false);
        }

        notifyIcon1.ShowBalloonTip(5000, "AutoClicker", problem.Message, ToolTipIcon.Warning);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        SaveSettings();
        _autoClickerManager.Dispose();
        notifyIcon1.Visible = false;

        _hook.Dispose();
        _overlayForm.Dispose();
    }

    private void LoadSettings()
    {
        var settings = _autoClickerSettings.Settings;

        intervalNumericUpDown.Value = settings.Interval;
        randomDelayNumericUpDown.Value = settings.RandomDelay;
        xNumericUpDown.Value = settings.X;
        yNumericUpDown.Value = settings.Y;
        jitterRadiusNumericUpDown.Value = settings.JitterRadius;
        currentCursorPositionRadioButton.Checked = settings.PositionMode == ClickPositionMode.CurrentCursor;
        specificPositionRadioButton.Checked = settings.PositionMode == ClickPositionMode.SpecificPosition;
        displayOverlayCheckBox.Checked = settings.DisplayOverlay;
        startStopKeyComboBox.SelectedItem = settings.StartStopKey;
        ctrlRadioButton.Checked = settings.StartStopModifiers == Keys.Control;
        altRadioButton.Checked = settings.StartStopModifiers == Keys.Alt;
        shiftRadioButton.Checked = settings.StartStopModifiers == Keys.Shift;
        mouseButtonComboBox.SelectedItem = settings.MouseButton;

        // saved key isn't in the list
        if (startStopKeyComboBox.SelectedItem is null)
        {
            startStopKeyComboBox.SelectedIndex = 0;
        }
    }

    private void SaveSettings()
    {
        _autoClickerSettings.Update(new Settings
        {
            Interval = (int)intervalNumericUpDown.Value,
            RandomDelay = (int)randomDelayNumericUpDown.Value,
            X = (int)xNumericUpDown.Value,
            Y = (int)yNumericUpDown.Value,
            JitterRadius = (int)jitterRadiusNumericUpDown.Value,
            PositionMode = specificPositionRadioButton.Checked
                ? ClickPositionMode.SpecificPosition
                : ClickPositionMode.CurrentCursor,
            DisplayOverlay = displayOverlayCheckBox.Checked,
            MouseButton = mouseButtonComboBox.SelectedItem is MouseButtons selectedButton
                ? selectedButton
                : _autoClickerSettings.Settings.MouseButton,
            StartStopKey = startStopKeyComboBox.SelectedItem is Keys key ? key : Keys.Escape,
            StartStopModifiers = SelectedModifier()
        });

        _autoClickerSettings.Save();
    }

    private void CurrentCursorPositionRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        xNumericUpDown.Enabled = !currentCursorPositionRadioButton.Checked;
        yNumericUpDown.Enabled = !currentCursorPositionRadioButton.Checked;
        RefreshOverlay();
    }

    private void NotifyIcon1_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        RestoreWindow();
    }

    private void MainForm_Resize(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            Hide();
        }
    }

    private void MouseHook_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_autoClickerManager.IsJittering)
        {
            return;
        }

        // the saved X/Y must not change here, only the label and overlay follow the cursor
        _lastPhysicalMousePosition = e.Location;
        if (Visible)
        {
            mousePositionLabel.Text = $"Current Mouse Position: X=[{e.X}] Y=[{e.Y}]";
        }

        RefreshOverlay();
    }

    private void RefreshOverlay()
    {
        if (!displayOverlayCheckBox.Checked)
        {
            return;
        }

        Point target = currentCursorPositionRadioButton.Checked
            ? _lastPhysicalMousePosition
            : new Point((int)xNumericUpDown.Value, (int)yNumericUpDown.Value);
        _overlayForm.SetCircle(target, (int)jitterRadiusNumericUpDown.Value);
    }

    private void DisplayOverlayCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (displayOverlayCheckBox.Checked)
        {
            RefreshOverlay();
            _overlayForm.Show();
        }
        else
        {
            _overlayForm.Hide();
        }
    }
}
