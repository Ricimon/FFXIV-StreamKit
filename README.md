# StreamKit

Plugin for [XIVLauncher/Dalamud](https://goatcorp.github.io/), used to trigger OBS effects with in-game actions.

This plugin is currently a proof-of-concept of the ability to manipulate OBS with in-game actions and events, and as such, only contains a single, simple feature. I hope to receive additional feature requests, and eventually build out a more general and programmable system based on desired features. So, feel free to ask for more features on GitHub issues or by submitting PRs!

## Current Features

- Enable and disable OBS sources depending on if your character is alive

## Demo

<img src="/.github/res/demo.gif">

## Installation

First, make sure your OBS has [obs-websocket](https://github.com/obsproject/obs-websocket) **v4.x** installed. Configure it with a password if you wish.

Since this project is still a work in progress, you'll need to add a custom repo URL to your Dalamud settings to install the plugin and keep it updated. Follow the steps below:

1. Open Dalamud settings by typing `/xlsettings`
2. Click on the Experimental tab
3. Copy and paste the following repo.json link as a new row under URL
    - https://raw.githubusercontent.com/Ricimon/FFXIV-StreamKit/main/repo.json
4. Click on the + button
5. Click on the "Save and Close" button
6. Open the Plugin Installer by typing `/xlplugins`
7. Under "All Plugins", search for the plugin by typing "StreamKit" into the search bar
    - The [3] in the plugin icon indicates it's a third-party plugin.
8. Click install, and the plugin should appear under "Installed Plugins"

You do **not** need download the release zip and manually install it. Doing so will require manual updates later. Any manually installed copies should be removed before using the custom plugin repository method, as they will conflict.

## Usage

In the `/xlplugins` window, hitting the Open Configuration button on the plugin opens the StreamKit configuration window. Typing the command `/streamkit` will also open this window.

<img src="/.github/res/configuration.png" width=400>

If you left default settings after installing obs-websocket, the existing IP:PORT parameter should work (`ws://localhost:4444`).

Set the `Alive image source name` and `Dead image source name` to the name of the corresponding sources on OBS. Note that these sources will only be changed if they are on the current active scene, and only the first source found by this name will be changed. Use a folder to group together multiple sources.

Use `Test Alive` and `Test Dead` to test that the proper OBS sources are toggling on and off.
