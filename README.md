# SanchetanaXR
## Mobile VR Game with Custom ESP32 Android Gamepads

## Overview

Experience immersive VR gaming on your Android device with our custom-designed VR game and unique ESP32-powered gamepads. This project combines Unity-based VR development with innovative hardware inputs to create an accessible and engaging gaming experience.

## Features:

<li>Immersive VR Gameplay: Leverages the gyroscope and accelerometer of your Android phone for intuitive camera control.

<li>Custom ESP32 Gamepads: Two ESP32 boards (master and slave) provide joystick and button inputs for seamless interaction.

<li>Master ESP32: Equipped with a joystick module and connects to the Android device via Bluetooth.

<li>Slave ESP32: Contains 4-6 customizable buttons, communicating with the master using ESP-NOW.

<li>Lightweight Setup: No expensive VR headsets required. Your phone acts as both the display and the input device.

<li>Open-Source: Fully documented and customizable codebase for both the Unity game and the ESP32 gamepads.

## How It Works

### Game

Built using Unity for Android.

The camera movement is controlled by the phone's gyroscope and accelerometer.

Interactions with the game world are performed using the ESP32 gamepads.

### ESP32 Gamepads

#### Master ESP32: 

Connects to the Android phone via Bluetooth using the Ble-gamepad library.

Sends joystick inputs directly to the phone.

#### Slave ESP32:

Communicates with the master ESP32 via ESP-NOW.

Captures button presses and forwards them to the master ESP32.


### Software Libraries

>Unity: Game engine for VR game development.

>Ble-gamepad: [Bluetooth gamepad library for ESP32](https://github.com/lemmingDev/ESP32-BLE-Gamepad).

>NimBLE-Arduino: [ESP-NOW communication library](https://github.com/h2zero/NimBLE-Arduino).

### Future Enhancements

<li>Support for multiplayer.

<li>Enhanced graphics and optimized performance.

<li>Additional hardware support for more complex inputs.

### **Contributing**

Contributions are welcome! Feel free to fork the repository, submit issues, or create pull requests.

### License

This project is licensed under the MIT License.

### Acknowledgements

>[Ble-gamepad](https://github.com/lemmingDev/ESP32-BLE-Gamepad)

>[NimBLE-Arduino](https://github.com/h2zero/NimBLE-Arduino)

>Unity Asset Store for terrain assets.

### Contact

For questions or feedback, reach out to:

Email: [daujo3036@gmail.com](daujo3036@gmail.com)

GitHub: [My Github Profile](https://github.com/AtharvA20003/)


