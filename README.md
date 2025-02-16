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
📁 moving_block_mod
├── 📂 moving_platforms
│   ├── 📂 textures # .xnb files for moving platform textures
│   ├── 📂 hitboxes # .xnb files for moving platform hitboxes
│   └── 📂 definitions # .xml files defining moving platforms
└── 📂 levers
    ├── 📂 textures # .xnb files for lever textures
    └── 📂 definitions # .xml files defining levers
```

In the Getting Started section, we will focus on how to setup a simple
moving platform, more advanced topics about them are available in the 
moving platform section. Same for the levers.

### Create your first moving platform

Once you have the mod folder set up, you can start creating your first
moving platform. To create one, you will need 3 files: a definition, a
texture and a hitbox.

#### Setup the definition file

Inside the `/moving_platforms/definitions` folder, create a new
XML file and name it as you like. In this file, you can define multiple
platforms. Use the `<MovingPlatforms>` tag to create a
list, and each platform should be defined inside a
`<MovingPlatform>` tag.

```xml
<MovingPlatforms>
    <!-- a moving platform -->
    <MovingPlatform>
    </MovingPlatform>

    <!-- other moving platforms -->
</MovingPlatforms>
```

Let's start by providing the mod at which screen the platform will be
placed. This is done by adding the `<screen>` tag.

```xml
<MovingPlatforms>
    <MovingPlatform>
        <screen>1</screen> <!-- the platform will be placed at screen 1 -->
    </MovingPlatform>
</MovingPlatforms>
```

#### Add a hitbox

A moving platform hitbox is an image made up of blocks, similar to the
`level.xnb` file in your map. The image's height and width should match
the hitbox size.

Lets do a 6x2 blocks platform (only base block available for now), the image
should be 6x2 pixels.

Convert it to a .xnb file, place it inside the `/moving_platforms/hitboxes` folder.

Then, reference it in the definition file using the
`<hitboxName>` tag to specify the hitbox file name.

```xml
<MovingPlatforms>
    <MovingPlatform>
        <screen>1</screen>
        <hitboxName>platform-6-2-hitbox</hitboxName> <!-- your hitbox file name -->
    </MovingPlatform>
</MovingPlatforms>
```