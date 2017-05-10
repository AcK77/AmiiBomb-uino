# **![](http://i64.tinypic.com/qpqm45.png) AmiiBomb - Amiibo Cloning**

AmiiBomb is Windows tools, with cheap component used, for create Amiibo Tag (NTAG215) and much more...

## What's you need?

- *1x* Windows PC
- *1x* Arduino Uno R3 *(around $4)*
- *1x* USB Cable Type AB *(pretty sure you already have it)*
- *1x* RFID Module RC522 *(around $2)*
- *7x* Pin Wire Male-Female *(around $1 for x20)*
- *1x* Soldering Iron Kit *(and a little soldering skills)*
- Many NTAG215 as you want Amiibo Tag *(around $34 for x100)*

## What you have to do?

You have to solder the pins on the RC522 Module and connect them following this schematics. Connect the Arduino Uno (or Nano) to the PC by USB and That's all! (Guys with soldering skills already know that but I prefer explain for anyone!)

![Arduino / RC255 PinOut](http://i66.tinypic.com/2ng8zv9.jpg)

Signal    | RC522 Pin | Arduino Pin
--------- | --------- | -----------
RST/Reset | RST       | 9
SPI SS    | SDA(SS)   | 10
SPI MOSI  | MOSI      | 11 / ICSP-4 
SPI MISO  | MISO      | 12 / ICSP-1 
SPI SCK   | SCK       | 13 / ICSP-3 
VCC       | 3.3V      | 3.3V
GND       | GND       | GND

## And when the hardware is ready?

You have to run AmiiBomb, Set an Amiibo folder (*.bin files), select the Amiibo Keys, flash the AmiiBombuino Firmware to the Arduino, and you are ready to Read and Write Amiibo Tag.

 - ***.bin folder**

It's the folder where you can put your Amiibo dumps (previously dumped with AmiiBomb or found on internet, Google is your friend for that).
You can save the readed Amiibo here too, to write it back to another NTAG. You don't have to re-execute AmiiBomb, it's looking for folder changes automatically.

 - **Amiibo Keys**

You probably already have them if you know a little how Amiibo cloning works. If not, a little helper is here in AmiiBomb. Due to Copyright reason, we can't distribute them, but there is many place to found them. AmiiBomb send you to the right website, you just have to highlight the keys chars and copy them to your Clipboard, AmiiBomb check if the valid keys was inside, and ask you if you want to save them. If you have already them, you just have to select the keys file.

 - **AmiiBombuino Firmware**

Just an Arduino program who communicate with AmiiBomb, You can flash it through avrdude by yourself or using the Internal Flasher in AmiiBomb or using XLoader.

## Overview

![](http://i68.tinypic.com/dwe7g6.png) 
![](http://i67.tinypic.com/4sjvd3.png) 
![](http://i66.tinypic.com/21mtao8.jpg) 
![](http://i66.tinypic.com/1z6z0o1.png) 

## What is done?
- Grab info of an Amiibo Dump via http://Amiibo.life website.
- Cache system for Amiibo Dump informations in a file.
- Enable/Disable and Reset Informations Caching files.
- Reconize encrypted/decrypted Amiibo Dump.
- Decrypt/Encrypt Amiibo Dump.
- Able to fix the incorrect size of one type of Amiibo Dump (Power Saves or N2? I don't know:/)
- Dump and Write AppData of an Amiibo Dump.
- Help to found Amiibo Keys and Autodetect them in Clipboard for save them in file.
- Multilanguages.
- Read a NTAG215 and save it to an Amiibo Dump file.
- Write an Amiibo Dump file to an NTAG215.
- Flash AmiiBombuino Firmware inside AmiiBomb.
- Get Amiibo Tag Basic Informations.
- and more...

## What's Next?
- Many little things to do AmiiBomb more User Friendly.
- Clean the code.
- It's work really fine with an NTAG215 but I don't know if it's work with an Amiibo ^^'!
- AppData editor (for SSB Amiibo or any others who have interesting things inside).
- Support PN532 NFC Module in AmiiBombuino.
- Improve the docs.

In a future, I would own an N2 and a PowerSaves to add many functionality as possible in AmiiBomb. If you have any kind of ideas or suggestions, just let me know :)! You feel free to donate me at paypal adress: ackeedy@gmail.com

Thanks & Enjoy!