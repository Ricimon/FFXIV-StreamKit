# StreamKit

Plugin for XIVLauncher/Dalamud, used to trigger OBS effects with in-game actions.

This plugin is currently a proof-of-concept of the ability to manipulate OBS with in-game actions and events, and as such, only contains a single, simple feature. I hope to receive additional feature requests, and eventually build out a more general and programmable system based on desired features. So, feel free to ask for more features on GitHub issues or by submitting PRs!

## Current Features

- Enable and disable OBS sources depending on if your character is alive

## Demo

<img src="/.github/res/demo.gif">

## Installation

First, make sure your OBS has [obs-websocket](https://github.com/obsproject/obs-websocket) **v4.x** installed. Configure it with a password if you wish.

Next, grab the latest release in the [Releases](https://github.com/Ricimon/FFXIV-StreamKit/releases) section. Download the `StreamKitDalamud.zip` file, and extract its contents (should be a single folder called `StreamKit`) into XIVLauncher's installedPlugins folder, which is typically located at `%APPDATA%/XIVLauncher/installedPlugins`.

Then in FFXIV, run `/xlplugins`, go to Installed Plugins, and Load the plugin.

## Usage

In the `/xlplugins` window, hitting the Open Configuration button on the plugin opens the StreamKit configuration window. Typing the command `/streamkit` will also open this window.

<img src="/.github/res/configuration.png" width=400>

If you left default settings after installing obs-websocket, the existing IP:PORT parameter should work (`ws://localhost:4444`).

Set the `Alive image source name` and `Dead image source name` to the name of the corresponding sources on OBS. Note that these sources will only be changed if they are on the current active scene, and only the first source found by this name will be changed. Use a folder to group together multiple sources.

Use `Test Alive` and `Test Dead` to test that the proper OBS sources are toggling on and off.
