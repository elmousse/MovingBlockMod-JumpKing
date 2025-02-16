# Moving Block Mod for Jump King

## Getting Started

This mod introduces **moving platforms** to *Jump King*,
allowing map creators to create dynamic, moving obstacles. You can control
the movement, speed, and even the behavior of these platforms through
Levers.

A moving platform is essentially a set of solid blocks that move together as one.

### Mod structure in a map

To use the mod in your map, create a folder named
`moving_block_mod` in the root of your map folder. If you are
using JK Worldsmith, make sure to place it inside the bin folder.

> It's strongly recommended to keep your main working copy of the mod folder
outside the level folder. When you need to test, copy it into the level
folder. This prevents Worldsmith from accidentally deleting it when loading
your level.

Here’s a quick overview of the folder structure inside the mod:

``` python
📁 moving_block_mod/
├── 📂 moving_platform/ 
│   ├── 📂 textures/ # .xnb files for moving platform textures
│   ├── 📂 hitboxes/ # .xnb files for moving platform hitboxes
│   └── 📂 definitions/ # .xml files defining moving platforms
└── 📂 lever/
    ├── 📂 textures/ # .xnb files for lever textures
    └── 📂 definitions/ # .xml files defining levers
```