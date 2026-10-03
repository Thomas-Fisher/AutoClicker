# AutoClicker

A small Windows auto clicker built with .NET 8 and WinForms. It clicks at the current cursor position or at a fixed screen position, with optional random delay and positional jitter.

## Features

- Left, middle or right mouse button
- Configurable click interval (1-10000 ms) plus an optional random extra delay
- Click at the live cursor position, or at a fixed X / Y coordinate
- Optional jitter: each click lands at a random point within a radius, then the cursor snaps back
- Optional on-screen overlay showing the click position and jitter radius
- Global start/stop hotkey with a choice of Ctrl, Alt or Shift as the modifier, which works while the window is hidden and is not passed on to the app in focus
- Tray icon to restore the window; settings are saved between runs

## Requirements

- Windows 10 or 11
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) to build (the .NET 8 Desktop Runtime is enough to run a published build)

## Build, run and test

```bash
dotnet build AutoClicker/AutoClicker.sln
dotnet run --project AutoClicker/AutoClicker/AutoClicker.csproj
dotnet test AutoClicker/AutoClicker.sln
```

## Usage

1. Choose the mouse button, interval, position mode and (optionally) jitter.
2. Pick a hotkey and one modifier (Ctrl, Alt or Shift).
3. Press the hotkey to start clicking. The window hides while clicking.
4. Press it again to stop. The window reappears.

The default hotkey is **Shift + Esc**. Always keep the stop hotkey in mind before starting: while clicking, the window is hidden and the mouse is busy. You can also double-click the tray icon to bring the window back. If Windows blocks the clicks (for example the target window runs as administrator) or the clicker stops unexpectedly, a notification appears from the tray icon.

## Settings

Settings are stored as JSON in `%APPDATA%\AutoClicker\settings.json`. Values outside the supported ranges are clamped when loaded. If the file is invalid it is renamed to `settings.json.bak` and replaced with defaults.

## Project layout

| Path | Purpose |
|------|---------|
| `AutoClicker/AutoClicker` | The WinForms app |
| `AutoClicker/AutoClickerTest` | xUnit tests (settings, click logic, hotkey handling) |
| `.github/workflows/ci.yml` | Builds and runs the tests on every push and pull request |

## Responsible use

Use this for accessibility, testing and other non-competitive purposes. Using automation in online games or services that forbid it may breach their terms of service and can get your account banned.
