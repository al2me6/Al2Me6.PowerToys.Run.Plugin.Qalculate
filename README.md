# (Unofficial) Qalculate plugin for PowerToys Run

This repo supplies an alternative calculator/unit converter plugin powered by [Qalculate](https://qalculate.github.io).

The following computations are triggered globally:

- 'basic' math expressions
- a selection of functions
- base conversions (bin, oct, hex)
- some common mathematical/physical constants
- queries involving units

Full access to Qalculate syntax requires use of its trigger keyword (default `==`). It is recommended that the build-in Calculator and Unit Converter plugins be disabled and for the `=` trigger keyword to be re-bound to Qalculate.

## Build & Install

First, clone the repo and run `git submodule update --init --recursive`.

Once compiled in Release mode in Visual Studio, the plugin will be found in `<repo>\x64\Release\modules\launcher\Plugins`. Copy/symlink the `Qalculate` folder to `%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins`.

For this plugin to function, Qalculate must be installed and the included `qalc.exe` must be manually added to your PATH.

## License

MIT.
