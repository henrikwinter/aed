#include <Arduino.h>
#include <EEPROM.h>
#include <Keyboard.h>

int RXLED = 17; // The RX LED has a defined Arduino pin
int TXLED = 30; // The TX LED has a defined Arduino pin

int eoffset = 100; // 10;
int pswoffset[8];
String psw[] = {"psw1", "psw2", "psw3", "psw4", "psw5", "psw6", "0,1,2,3,4,", "0"};
int flag = 127;

bool locked = true;
int sec[40] = {0, 1, 2, 3, -1};
int seccikl = 0;
int secciklMax = 4;

int blinkf = 0;

//int btnPin[8] = {2, 3, 4, 5, 6, 7, 8, 9}; // elso kezi huzalozas
int btnPin[8]={2,5,9,8,7,4,3,6};  //nyakos verziohoz
int btnPrevState[8];

void writeIntArrayIntoEEPROM(int address, int numbers[], int arraySize)
{
  int addressIndex = address;
  for (int i = 0; i < arraySize; i++)
  {
    EEPROM.write(addressIndex, numbers[i] >> 8);
    EEPROM.write(addressIndex + 1, numbers[i] & 0xFF);
    addressIndex += 2;
  }
}
void readIntArrayFromEEPROM(int address, int numbers[], int arraySize)
{
  int addressIndex = address;
  for (int i = 0; i < arraySize; i++)
  {
    numbers[i] = (EEPROM.read(addressIndex) << 8) + EEPROM.read(addressIndex + 1);
    addressIndex += 2;
  }
}

int writeString(int address, String data)
{
  int stringSize = data.length();
  for (int i = 0; i < stringSize; i++)
  {
    EEPROM.write(address + i, data[i]);
  }
  EEPROM.write(address + stringSize, '\0'); // Add termination null character
  return address + stringSize + 1;
}

String readString(int address)
{
  char data[100]; // Max 100 Bytes
  int len = 0;
  unsigned char k;
  k = EEPROM.read(address);
  while (k != '\0' && len < 100) // Read until null character
  {
    k = EEPROM.read(address + len);
    data[len] = k;
    len++;
  }
  data[len] = '\0';
  return String(data);
}

void Getoffsets()
{
  unsigned char k;
  int len = 0;
  int pswidx = 0;
  pswoffset[pswidx] = eoffset + len;
  pswidx++;
  for (int c = 0; c < 1000; c++)
  {
    if (pswidx > 7)
      break;
    k = EEPROM.read(eoffset + len);
    // Serial.print((char)k);
    if (k == '\0')
    {
      len++;
      //  Serial.print(" ->");
      //  Serial.print(pswidx);
      //  Serial.print(" : ");
      //  Serial.print(eoffset + len);

      pswoffset[pswidx] = eoffset + len;
      String psw = readString(pswoffset[pswidx]);
      //   Serial.print(" -> ");
      //   Serial.print(psw);
      //   Serial.println("");
      pswidx++;
    }
    len++;
  }
}

void SavePsw(String pswarr[8])
{
  int offset = eoffset;
  for (int c = 0; c < 8; c++)
  {
    pswoffset[c] = offset;
    offset = writeString(offset, pswarr[c]);
  }
}

void blinker(int mode)
{

  if (blinkf == 0)
  {
    blinkf = 1;
    digitalWrite(RXLED, HIGH);
    if (seccikl > 0)
      digitalWrite(TXLED, HIGH);
  }
  else
  {
    blinkf = 0;
    digitalWrite(RXLED, LOW);
    if (seccikl > 0)
      digitalWrite(TXLED, LOW);
    else
      digitalWrite(TXLED, HIGH);
  }
}

int rb1(int pin)
{
  int retv = -1;
  int buttonState = digitalRead(btnPin[pin]);
  if ((buttonState != btnPrevState[pin]) && (buttonState == LOW))
  {
    retv = pin;
    delay(10);
  }
  btnPrevState[pin] = buttonState;
  return retv;
}

void rb(int pin)
{
  int buttonState = digitalRead(btnPin[pin]);
  if ((buttonState != btnPrevState[pin]) && (buttonState == LOW))
  {
    String psw = readString(pswoffset[pin]);
    Keyboard.print(psw);
    delay(10);
  }
  btnPrevState[pin] = buttonState;
}
// epr 0  = inited flag
// epr 10 = 8 null rerminated string

int scanbutton()
{
  digitalWrite(TXLED, LOW);
  digitalWrite(RXLED, LOW);
  int timeout = 0;
  unsigned int blinkcik = 0;
  int bv = 0;
  while (seccikl < secciklMax)
  {
    // timeout++;
    // if(timeout>1000) return 0;
    for (int i = 0; i < 6; i++)
    {
      bv = rb1(i);
      if (bv != -1)
      {
        // Serial.print("[bv:");
        // Serial.print(bv);
        // Serial.print("-");
        if (bv == sec[seccikl])
        {
          seccikl++;
        }
        else
        {
          seccikl = 0;
        }
        // Serial.print(seccikl);
        // Serial.println("]");
      }
      else
      {
      }
      if (blinkcik > 254)
      {
        blinker(0);
        blinkcik = 0;
      }

      blinkcik++;
      delay(1);
    }
  }
  locked = false;
}

String str[8];
int split(String comdata)
{
  int pos = 0;
  for (int f = 0; f < 7; f++)
  {
    str[f] = "";
  }
  if (comdata.length() > 0)
  {

    int spilt = 0;

    int num[8];
    for (int i = 0; i < comdata.length(); i++) // String psw[] = {"psw1","psw2","psw3","psw4","psw5","psw6","0,1,2,3,4,","0"};
    {

      if (comdata[i] != char(9)) // Comma is the separator
      {
        str[pos] += comdata[i];
      }
      else
      {
        // str[pos] +='\0';
        // Serial.print(str[pos]);
        // Serial.print(" ");
        pos++;
        if (pos >= 8)
          break;
      }
    }
    if (pos < 6)
    {
      for (int y = pos; y < 5; y++)
      {
        str[pos] = "no-psw";
        pos++;
      }

      str[7] = "0,1,2,3,4,";
      str[8] = "0";
    }
  }

  return pos;
}

void test()
{
  int newseclen = 0;
  String msg = readString(pswoffset[6]);
  // Serial.println(msg);
  char buf[msg.length() + 1];
  msg.toCharArray(buf, sizeof(buf));
  char *token;
  char *pter = buf;
  while ((token = strtok_r(pter, ",", &pter)))
  {
    int numb = atoi(token);
    sec[newseclen] = numb;
    newseclen++;
    // Serial.println(numb);
    // push token to the vector
  }
  sec[newseclen] = -1;
  writeIntArrayIntoEEPROM(10, sec, 40);
}
void handleSerial()
{
  // char *StringToParse;
  if (Serial.available())
  {
    String msg = Serial.readStringUntil('\n');
    int len = split(msg);
    SavePsw(str);
    Serial.println("Psws= " + msg + "!");
    // Serial.println(getValue(newpsw,",",0));
    String sec = readString(pswoffset[6]);
    Serial.println("sec= " + sec + "!");
    test();
  }
}

void setup()
{

  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(RXLED, OUTPUT); // Set RX LED as an output
  pinMode(TXLED, OUTPUT); // Set TX LED as an output

  // put your setup code here, to run once:
  for (int i = 0; i < 8; i++)
  {
    pinMode(btnPin[i], INPUT);
  }
  int wcik = 0;

  Serial.begin(9600);
  while (!Serial)
  {
    if (wcik > 100)
      break;
    wcik++;
    delay(1);
  };
  Serial.println("Init..");

  int inited = EEPROM.read(0);
  if (inited != flag)
  {
    Serial.println("Write..");
    SavePsw(psw);
    writeIntArrayIntoEEPROM(10, sec, 40);
    EEPROM.write(0, flag);
  }
  else
  {
    Serial.println("Read..");
    Getoffsets();
    readIntArrayFromEEPROM(10, sec, 40);
  }

  for (int z = 0; z < 40; z++)
  {
    if (sec[z] < 0)
    {
      secciklMax = z;
      break;
    }
  }

  Keyboard.begin();
}

void loop()
{

  if (locked)
  {
    scanbutton();
  }
  else
  {
    for (int u = 0; u < 6; u++)
    {
      rb(u);
    }
    handleSerial();
  }

  delay(100);
}
