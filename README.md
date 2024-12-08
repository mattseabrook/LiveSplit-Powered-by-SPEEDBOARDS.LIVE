<h1> <img src="https://raw.githubusercontent.com/LiveSplit/LiveSplit/master/res/Icon.svg" alt="LiveSplit" height="42" align="top"/> LiveSplit - Powered by SPEEDBOARDS.LIVE</h1>

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/LiveSplit/LiveSplit/master/LICENSE)

**LiveSplit - Powered by SPEEDBOARDS.LIVE** is a complete line-by-line optimization & re-write of the popular timer program for speedrunners, that is both easy to use and full of features.

Software will officially release in 2025, but I am looking for Beta Testers right now! 

Please contact us: speedboards.live@gmail.com

# CHANGELOG

## 2024-12-08

Refactored my new `Init` function to wait for `Settings` to actually be available before trying to access it:

```csharp
    private async Task Init(string splitsPath = null, string layoutPath = null)
    {
        InitializeCoreComponents();
        await LoadSettingsAsync(); // Ensure settings are loaded first.
        await Task.WhenAll(
            LoadSplitsAsync(splitsPath),
            LoadLayoutAsync(layoutPath)
        );
        await SetupStateAsync();
        RegisterEventHandlers();
        await Task.WhenAll(
            InitializeHooksAsync(),
            ConfigureServerAsync(),
            ConfigureTimersAsync(),
            ConfigureUIAsync()
        );
    }
```

## 2024-12-01

Today was the first session, so I immediately dived into the Main Entrypoint and Main Form of the Application.

### Program.cs

- Optimized argument parsing loop by replacing if-else with a switch statement and incrementing the loop by 2, making argument handling clearer and more efficient.
- Removed the empty finally block since it served no purpose and was adding unnecessary clutter to the code.
- Removed the try-catch block around Application.Run and instead added a global exception handler (Application.ThreadException) to handle exceptions in a more centralized manner.
- Replaced string.Format with string interpolation ($"...") for better readability and more concise syntax.
- Cleaned up unused using directives to reduce code clutter and improve overall maintainability of the file.

### TimerForm.cs

Refactored main `Init` method by breaking it into smaller functions for better modularity, readability, and maintainability.

#### InitializeCoreComponents

- Moved core component initialization code from Init into this method to simplify the main initialization logic.
- Set up components such as LiveSplitCoreFactory, GlobalCache, and Invalidator, ensuring that essential components are loaded in a consistent and isolated manner.
- Set the window title (SetWindowTitle) and applied UI styles (SetStyle) to enhance the visual quality.
- Lazy-loaded settings asynchronously using LoadSettingsAsync() to prevent blocking the main thread during startup, improving startup performance.

#### LoadSettingsAsync

- Created an asynchronous method (LoadSettingsAsync) to handle settings loading without blocking the UI.
- Used Task.Run() to load settings on a separate thread, ensuring the UI remains responsive during this operation.

#### LoadSplitsAsync

- Extracted the split-loading logic from the Init method into a new asynchronous method (LoadSplitsAsync).
- Handled logic to load splits from either the provided splitsPath or fallback to the most recent split in settings.
- Used LoadRunFromFileAsync to load split files asynchronously, preventing any UI freeze due to file operations.

#### LoadRunFromFileAsync

- Created a method to load a run from a file asynchronously (LoadRunFromFileAsync).
- This minimizes the impact on UI responsiveness, especially during potentially slow file I/O operations by running them on a separate thread.

#### LoadLayoutAsync

- Moved layout-loading logic into LoadLayoutAsync to reduce the responsibilities of the main Init method.
- Added functionality to handle various layout configurations, such as recent layouts, standard layouts, and timer-only layouts.
- Used LoadLayoutFromFileAsync to make the layout-loading process non-blocking, improving the user experience by avoiding delays during application startup.

#### LoadLayoutFromFileAsync

- Created a method to load layouts asynchronously (LoadLayoutFromFileAsync), ensuring that file reading does not hinder the main application thread.
- This prevents blocking the UI, leading to smoother startup and transitions between layouts.

#### SetupStateAsync

- Created an asynchronous method (SetupStateAsync) to set up the LiveSplitState and other components.
- Set up the race provider factories, hotkey profiles, and auto-splitters in a non-blocking way.
- Managed state initialization separately to avoid complex dependencies during the startup process.

#### RegisterEventHandlers

- Moved the registration of event handlers (OnReset, OnStart, OnSplit, etc.) to a dedicated method to improve separation of concerns.
- Updated event handlers to use asynchronous lambdas (Task.Run()) to ensure the UI remains responsive, even during operations triggered by events.
- Reduced redundant operations by ensuring event handlers are registered in a streamlined manner.

#### InitializeHooksAsync

- Moved input hook initialization to InitializeHooksAsync to prevent blocking UI during startup.
- Created an asynchronous method to initialize input hooks (CompositeHook) for gamepad and keyboard events.
- Registered hotkeys in a non-blocking manner to maintain smooth user interaction.

#### ConfigureServerAsync

- Extracted the server configuration logic into ConfigureServerAsync to separate it from the main Init process.
- Configured the command server (CommandServer) and started the named pipe asynchronously to avoid blocking the application launch.

#### ConfigureTimersAsync

- Moved timer setup logic to a new asynchronous method (ConfigureTimersAsync) to reduce startup overhead.
- Configured the System.Timers.Timer asynchronously, and set up the Elapsed event handler to run in a non-blocking task.
- Optimized RefreshTask to run as a background task, ensuring that it does not interfere with the application's responsiveness.

#### ConfigureUIAsync

- Created an asynchronous method (ConfigureUIAsync) to handle UI configurations, reducing the likelihood of blocking the main thread.
- Used lazy initialization for ComponentRenderer to minimize memory usage until the component is necessary.
- Configured layout properties (SetLayout) and UI-related settings (StartPosition, BackColor, etc.) asynchronously, making the UI setup more efficient.
- Used null-conditional operators (?.) to safely access layout settings (TopMost = Layout?.Settings?.AlwaysOnTop ?? false), avoiding potential null reference exceptions.