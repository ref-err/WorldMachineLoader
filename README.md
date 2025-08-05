![Logo](.github/logo.png)
---
_This is a continuation/fork of the original [WorldMachineLoader](https://github.com/thehatkid/WorldMachineLoader) by hat_kid._

A mod loader for [OneShot: World Machine Edition](https://store.steampowered.com/app/2915460/OneShot_World_Machine_Edition/)!  

## Features
- Load mods as `.dll` files
- Easy-to-use modding API
- Built-in event system for game integration
- Patch game methods with `[GamePatch]` attribute
- Built-in mod list UI

## Getting Started

### How to Install
1. Download the latest release from the [releases page](https://github.com/ref-err/WorldMachineLoader/releases).
2. Extract the contents into your OneShot: World Machine Edition game directory.
3. Run `WorldMachineLoader.exe`!

### Documentation
Documentation is now available at https://docs.wml.ref-err.ru/

## Notes for Developers
- Mod assemblies must have unique `Assembly.FullName`.
- You need to use `ModContext.FileSystem` to store all mod's files (e.g. settings). You should not use `System.IO`.
- You can use `ModInfoProvider` to inspect other loaded mods.

## Contributing
See [CONTRIBUTING.md](.github/CONTRIBUTING.md) and [CODE_OF_CONDUCT.md](.github/CODE_OF_CONDUCT.md) before contributing. Contributions are very welcome!

## License
WorldMachineLoader is licensed under the MIT license, see the [LICENSE](LICENSE) file for details.  
WorldMachineLoader is free and open source software.
