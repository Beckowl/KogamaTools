# KogamaTools
A plugin that adds quality of life features to KoGaMa.

<div style="display: flex; justify-content: space-around;">
  <img src="https://cdn.discordapp.com/attachments/1304111895531356180/1317955280276951070/Q7QPDyi.png?ex=67609101&is=675f3f81&hm=8b66e1cd134ffb653e3d136f8c9a2a3422fef4b21c2573f4b6135d350e381f7f&" alt="Left: Demonstration of destructibles unlock and single side painting being used. Right: Resized model of santa's cabin">
</div>

# **Features**

## 🛠️ **Build Features**

- **Model Tools**:
  - No build limit
  - Custom model scale
  - Single-side painting
  - Copy/paste
  - Export/import
  - Blue mode toggle
  - Destructible materials in model editing
  - Material picking with the mouse wheel
- **Editor**:
  - Free movement in avatar editor
  - Speed modifiers
  - Rotation step size modifier
  - Grid size modifier
  - Unlimited config values (input fields, sliders)
  - Multi-selection
  - Object grouping & Group editor
  - Force object links (e.g., connections to doors, hovercrafts, jetpacks)
  - Force interaction flags (e.g., rotation in all axes, force cloning)

---

## 🎮 **PvP Features**

- Instant respawn
- Anti-AFK
- Camera focus (scoping)
- Custom FOV
- Keybind modifiers
- Custom crosshair:
  - Color
  - Texture

---

## 🎨 **Graphics Features**

- UI toggle
- Reflective water toggle
- Resolution modifier
- Window mode settings (fullscreen, borderless, etc.)
- Shadow distance
- Draw distance
- Camera distance
- Ortographic camera
- Theme toggle
- Theme preview creator
- Screenshot functionality

---

## 📊 **Info Menu**

- World object counter
- Logic object counter
- Link counter
- Object link counter
- Model counters
- Ping (latency) display
- FPS display

---
## 🌀 **Other**
- Anti ban
- Cheat engine support
- Older versions support (should work with any version since the transparency logic update)

---

# **Requirements**

- [KoGaMa Standalone Client](https://www-gamelauncher.kogstatic.com/www/KogamaLauncher.msi)  
- [BepInEx 6.0.0-be](https://builds.bepinex.dev/projects/bepinex_be)  
- [.NET 6.0 Runtime](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0)  

---

# **Installation**


## 🚀 **Easy Method (Installer)**

TODO

---

## 🛠️ **Manual Installation**

1. **Locate Your Installation Folder**  
   Depending on the server you play on, navigate to the corresponding folder:
   - **KoGaMa-WWW**:  
     ```
     %localappdata%/KogamaLauncher-WWW\Launcher\Standalone
     ```
   - **KoGaMa-BR**:  
     ```
     %localappdata%/KogamaLauncher-BR\Launcher\Standalone
     ```
   - **KoGaMa-Friends**:  
     ```
     %localappdata%/KogamaLauncher-Friends\Launcher\Standalone
     ```

2. **Install BepInEx**  
   - Download the BepInEx ZIP file.  
   - Extract the contents into the folder you located in Step 1.
   - Run `Kogama.exe`.  
   - If a console window appears, BepInEx has been successfully installed.
   - Wait for the interop assemblies to be generated.

4. **Install KogamaTools**  
   - Download the latest version of [KogamaTools](https://github.com/pipocalio/KogamaTools).  
   - Extract the contents of the ZIP file into the `Plugins` folder created by BepInEx.  

5. **Start KoGaMa**  
   - Join any project or game.  
   - The KogamaTools overlay should appear after the loading screen.

---

## **Used libraries**
- [BepInEx](https://github.com/BepInEx/BepInEx) - *Licensed under the [LGPL-2.1 license](https://opensource.org/licenses/LGPL-2.1)*
- [ClickableTransparentOverlay](https://github.com/zaafar/ClickableTransparentOverlay) - *Licensed under the [Apache-2.0 License](https://opensource.org/licenses/Apache-2.0)*
- [ImGui](https://github.com/ocornut/imgui) - *Licensed under the [MIT license](https://opensource.org/licenses/MIT)*
- [NativeFileDialogSharp](https://github.com/milleniumbug/NativeFileDialogSharp) - *Licensed under the [Zlib license](https://opensource.org/licenses/Zlib)*
- [ParsingHelper](https://github.com/SoftCircuits/ParsingHelper) - *Licensed under the [MIT license](https://opensource.org/licenses/MIT)*

### **Need Help?**  
Join our [Discord Server](https://discord.gg/zgQxurzcEB) for support & discussions.

