<h1> <img src="https://raw.githubusercontent.com/LiveSplit/LiveSplit/master/res/Icon.svg" alt="LiveSplit" height="42" align="top"/> LiveSplit - Powered by SPEEDBOARDS.LIVE</h1>

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/LiveSplit/LiveSplit/master/LICENSE)

LiveSplit - Powered by SPEEDBOARDS.LIVE is a timer program for speedrunners that is both easy to use and full of features.

# CHANGELOG

## 2024-12-01

### Program.cs

- Optimized argument parsing loop by replacing if-else with a switch statement and incrementing the loop by 2, making argument handling clearer and more efficient.
- Removed the empty finally block since it served no purpose and was adding unnecessary clutter to the code.
- Removed the try-catch block around Application.Run and instead added a global exception handler (Application.ThreadException) to handle exceptions in a more centralized manner.
- Replaced string.Format with string interpolation ($"...") for better readability and more concise syntax.
Cleaned up unused using directives to reduce code clutter and improve overall maintainability of the file.