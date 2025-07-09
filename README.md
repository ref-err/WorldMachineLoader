# WorldMachineLoader
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

### How to Create a Mod
Before you begin, make sure you have **Visual Studio 2022** installed. You'll also need basic knowledge of **C#** and **.NET Framework 4.6.2**. 

#### 1. Create your project
1. In Visual Studio, File -> New -> Project -> Class Library (.NET Framework).
1. Target **.NET Framework 4.6.2**
2. Give it a unique ID (e.g. `net.referr.samplemod`). This will be your assembly and project name.
#### 2. Add references
- Add a reference to `WML.API.dll` (included in WML release)
- Add game assemblies (for patching) 
  - `OneShotMG.exe`
  - `MonoGame.Framework.dll`
  - These assemblies are located in game's installation folder. (e.g. `C:\Program Files (x86)\Steam\steamapps\common\OneShot World Machine Edition`)
#### 3. Implement `IMod`
```cs
public class SampleMod : IMod
{
    public void OnLoad(ModContext context) 
    {
        context.Logger.Log("Hello from SampleMod!");
    }

    public void OnShutdown()
    {
        context.Logger.Log("Goodbye!");
    }
}
```
#### 4. Create `mod.json`
```json
{
    "name": "Sample Mod",
    "id": "net.referr.samplemod",
    "description": "Lorem Ipsum. Ain't no way this is placeholder.",
    "author": "ref-err",
    "version": "0.1.0",
    "url": "https://github.com/ref-err/WorldMachineLoader",
    "icon": "icon.png",
    "assembly_name": "net.referr.samplemod.dll"
}
```
See full example at [SampleMods/SampleMod](SampleMods/SampleMod).

#### 5. Build and install
1. Build your project - you'll get `net.referr.samplemod.dll`.
2. In your game directory, create `mods\SampleMod\` and copy:
   - `net.referr.samplemod.dll`
   - `mod.json`
   - (Optional) `icon.png`

#### 6. Launch the game
Just run `WorldMachineLoader.exe` and you're done!

## Notes for Developers
- Mod assemblies must have unique `Assembly.FullName`.
- You need to use `ModContext.FileSystem` to store all mod's files (e.g. settings). You should not use `System.IO`.
- You can use `ModInfoProvider` to inspect other loaded mods.

## Contributing
See [CONTRIBUTING.md](.github/CONTRIBUTING.md) and [CODE_OF_CONDUCT.md](.github/CODE_OF_CONDUCT.md) before contributing. Contributions are very welcome!

## License
WorldMachineLoader is licensed under the MIT license, see the [LICENSE](LICENSE) file for details.  
WorldMachineLoader is free and open source software.
