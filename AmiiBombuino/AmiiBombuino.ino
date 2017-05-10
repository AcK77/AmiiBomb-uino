#include <SPI.h>
#include "MFRC522.h"
#include "SerialCommand.h"

#define RST_PIN         9
#define SS_PIN          10
#define Begin_of_Message "\x02"
#define End_of_Message "\x03"

MFRC522 mfrc522(SS_PIN, RST_PIN);
SerialCommand SCmd; 

void setup()
{
  Serial.begin(115200);
  SPI.begin();
  mfrc522.PCD_Init();
  SCmd.addCommand("/AMII", PingPong);
  SCmd.addCommand("/NTAG_HERE", NTAG_Here);
  SCmd.addCommand("/GET_NTAG_UID", NTAG_UID);
  SCmd.addCommand("/NTAG_HALT", NTAG_Halt);
  SCmd.addCommand("/READ_AMIIBO", Read_Amiibo);
  SCmd.addCommand("/WRITE_AMIIBO", Write_Amiibo);
}

void loop()
{
  SCmd.readSerial();
}

void PingPong()
{
  Serial.print(Begin_of_Message);
  Serial.print("BOMB");
  Serial.print(End_of_Message);
}

void NTAG_Halt()
{
  mfrc522.PICC_HaltA();
  mfrc522.PCD_StopCrypto1();

  Serial.print(Begin_of_Message);
  Serial.print("HALT");
  Serial.print(End_of_Message);
}

void NTAG_Here()
{
  Serial.print(Begin_of_Message);
  
  if(!mfrc522.PICC_IsNewCardPresent() || !mfrc522.PICC_ReadCardSerial())
    Serial.print("NO");
  else
    Serial.print("YES");

  Serial.print(End_of_Message);
}

void NTAG_UID()
{
  Serial.print(Begin_of_Message);
  for (byte i = 0; i < mfrc522.uid.size; i++)
  {
    Serial.print(mfrc522.uid.uidByte[i] < 0x10 ? "0" : "");
    Serial.print(mfrc522.uid.uidByte[i], HEX);
  } 
  Serial.print(End_of_Message);
}

void Read_Amiibo()
{
  char *arg;
  arg = SCmd.next(); 

  MFRC522::StatusCode status;
  byte buffer[18];
  byte size = sizeof(buffer);

  status = (MFRC522::StatusCode) mfrc522.MIFARE_Read(atoi(arg), buffer, &size);
  if (status != MFRC522::STATUS_OK) 
  {
      Serial.print(Begin_of_Message);
      Serial.print("/ERROR Data: ");
      Serial.print(mfrc522.GetStatusCodeName(status));
      Serial.print(End_of_Message);
  }
    
  Serial.print(Begin_of_Message);
  for (byte i = 0; i < 4; i++)
  {
    Serial.print(buffer[i] < 0x10 ? "0" : "");
    Serial.print(buffer[i], HEX);
  }
  Serial.print(End_of_Message);
}

void Write_Amiibo()
{
  char *arg;
  arg = SCmd.next(); 

  byte buffer[0x21C];

  Serial.print(Begin_of_Message);
  Serial.print("/WAIT");
  Serial.print(End_of_Message);

  while(Serial.available() == 0){}

  Serial.readBytes(buffer, 0x21C); 

  MFRC522::StatusCode status;

  // Write Data
  for (byte page = 3; page < 135; page++)
  {
    status = (MFRC522::StatusCode) mfrc522.MIFARE_Ultralight_Write(page, buffer + (page * 4), 4);
    if (status != MFRC522::STATUS_OK)
    {
      Serial.print(Begin_of_Message);
      Serial.print("/ERROR Data: ");
      Serial.print(mfrc522.GetStatusCodeName(status));
      Serial.print(End_of_Message);
      break;
    }
  }

  if (atoi(arg) == 1)
  {
    if(status == MFRC522::STATUS_OK)
    {
      // Write Dynamic Lock Bytes
      byte Dynamic_Lock_Bytes[] = { 0x01, 0x00, 0x0F, 0xBD };
      status = (MFRC522::StatusCode) mfrc522.MIFARE_Ultralight_Write(130, Dynamic_Lock_Bytes, 4);
      if (status != MFRC522::STATUS_OK)
      {
        Serial.print(Begin_of_Message);
        Serial.print("/ERROR DynLock: ");
        Serial.print(mfrc522.GetStatusCodeName(status));
        Serial.print(End_of_Message);
      }
    }

    if(status == MFRC522::STATUS_OK)
    {
      // Write Static Lock Bytes
      byte Static_Lock_Bytes[] = { 0x0F, 0xE0, 0x0F, 0xE0 };
      status = (MFRC522::StatusCode) mfrc522.MIFARE_Ultralight_Write(2, Static_Lock_Bytes, 4);
      if (status != MFRC522::STATUS_OK)
      {
        Serial.print(Begin_of_Message);
        Serial.print("/ERROR StaticLock: ");
        Serial.print(mfrc522.GetStatusCodeName(status));
        Serial.print(End_of_Message);
      }
    }
  }

  if(status == MFRC522::STATUS_OK)
  {
    Serial.print(Begin_of_Message);
    Serial.print("/END_WRITE");
    Serial.print(End_of_Message);
  }
}

