#include <Wire.h>
#include <MPU6050.h>

// C++ code
//
int EqualTensionThreshold = 0;

float AccelerationSteadinessThreshold = 0;

bool IsTensionEven = false;

bool IsTensionApplied = false;

bool IsAimSteady = false;

int i = 0;

MPU6050 mpu;

void setup()
{
  pinMode(A0, INPUT);
  pinMode(A1, INPUT);
  pinMode(A4, INPUT);
  pinMode(A5, INPUT);
  pinMode(6, OUTPUT);
  pinMode(5, OUTPUT);

  EqualTensionThreshold = 16;
  AccelerationSteadinessThreshold = 0.5;
  IsTensionEven = false;
  IsTensionApplied = false;
  IsAimSteady = false;

  Serial.begin(115200);

  Serial.println("Initialize MPU6050");
  while(!mpu.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G))
  {
    Serial.println("Could not find a valid MPU6050 sensor, check wiring!");
    delay(500);
  }

  mpu.setAccelOffsetZ(-8.5);

  mpu.calibrateGyro();

  mpu.setThreshold(3);

  checkSettings();

  digitalWrite(5, LOW);

  digitalWrite(6, LOW);
}

void checkSettings()
{
  Serial.println();
  
  Serial.print(" * Sleep Mode:            ");
  Serial.println(mpu.getSleepEnabled() ? "Enabled" : "Disabled");
  
  Serial.print(" * Clock Source:          ");
  switch(mpu.getClockSource())
  {
    case MPU6050_CLOCK_KEEP_RESET:     Serial.println("Stops the clock and keeps the timing generator in reset"); break;
    case MPU6050_CLOCK_EXTERNAL_19MHZ: Serial.println("PLL with external 19.2MHz reference"); break;
    case MPU6050_CLOCK_EXTERNAL_32KHZ: Serial.println("PLL with external 32.768kHz reference"); break;
    case MPU6050_CLOCK_PLL_ZGYRO:      Serial.println("PLL with Z axis gyroscope reference"); break;
    case MPU6050_CLOCK_PLL_YGYRO:      Serial.println("PLL with Y axis gyroscope reference"); break;
    case MPU6050_CLOCK_PLL_XGYRO:      Serial.println("PLL with X axis gyroscope reference"); break;
    case MPU6050_CLOCK_INTERNAL_8MHZ:  Serial.println("Internal 8MHz oscillator"); break;
  }
  
  Serial.print(" * Accelerometer:         ");
  switch(mpu.getRange())
  {
    case MPU6050_RANGE_16G:            Serial.println("+/- 16 g"); break;
    case MPU6050_RANGE_8G:             Serial.println("+/- 8 g"); break;
    case MPU6050_RANGE_4G:             Serial.println("+/- 4 g"); break;
    case MPU6050_RANGE_2G:             Serial.println("+/- 2 g"); break;
  }  

  Serial.print(" * Accelerometer offsets: ");
  Serial.print(mpu.getAccelOffsetX());
  Serial.print(" / ");
  Serial.print(mpu.getAccelOffsetY());
  Serial.print(" / ");
  Serial.println(mpu.getAccelOffsetZ());
  
  Serial.println();

//  Serial.println();
//  
//  Serial.print(" * Sleep Mode:        ");
//  Serial.println(mpu.getSleepEnabled() ? "Enabled" : "Disabled");
//  
//  Serial.print(" * Clock Source:      ");
//  switch(mpu.getClockSource())
//  {
//    case MPU6050_CLOCK_KEEP_RESET:     Serial.println("Stops the clock and keeps the timing generator in reset"); break;
//    case MPU6050_CLOCK_EXTERNAL_19MHZ: Serial.println("PLL with external 19.2MHz reference"); break;
//    case MPU6050_CLOCK_EXTERNAL_32KHZ: Serial.println("PLL with external 32.768kHz reference"); break;
//    case MPU6050_CLOCK_PLL_ZGYRO:      Serial.println("PLL with Z axis gyroscope reference"); break;
//    case MPU6050_CLOCK_PLL_YGYRO:      Serial.println("PLL with Y axis gyroscope reference"); break;
//    case MPU6050_CLOCK_PLL_XGYRO:      Serial.println("PLL with X axis gyroscope reference"); break;
//    case MPU6050_CLOCK_INTERNAL_8MHZ:  Serial.println("Internal 8MHz oscillator"); break;
//  }
//  
//  Serial.print(" * Gyroscope:         ");
//  switch(mpu.getScale())
//  {
//    case MPU6050_SCALE_2000DPS:        Serial.println("2000 dps"); break;
//    case MPU6050_SCALE_1000DPS:        Serial.println("1000 dps"); break;
//    case MPU6050_SCALE_500DPS:         Serial.println("500 dps"); break;
//    case MPU6050_SCALE_250DPS:         Serial.println("250 dps"); break;
//  } 
//  
//  Serial.print(" * Gyroscope offsets: ");
//  Serial.print(mpu.getGyroOffsetX());
//  Serial.print(" / ");
//  Serial.print(mpu.getGyroOffsetY());
//  Serial.print(" / ");
//  Serial.println(mpu.getGyroOffsetZ());
//  
//  Serial.println();
}

void loop()
{ 
  Vector rawAccel = mpu.readRawAccel();
  Vector normAccel = mpu.readNormalizeAccel();
  //Vector rawGyro = mpu.readRawGyro();
  //Vector normGyro = mpu.readNormalizeGyro();
  
  switch(Serial.read())
  {
    case '4':

    Serial.print(" Xraw = ");
    Serial.print(rawAccel.XAxis);
    Serial.print(" Yraw = ");
    Serial.print(rawAccel.YAxis);
    Serial.print(" Zraw = ");
    Serial.println(rawAccel.ZAxis);

    break;

    case '5':
    
//    Serial.print(" Xnorm = ");
//    Serial.print(normAccel.XAxis);
//    Serial.print(" Ynorm = ");
//    Serial.print(normAccel.YAxis);
//    Serial.print(" Znorm = ");
//    Serial.println(normAccel.ZAxis);

Serial.println(sqrt(pow (normAccel.XAxis, 2) + pow (normAccel.YAxis, 2) + pow (normAccel.ZAxis, 2))); 

    break;
    
  }
  
  // Detects if tension is being applied
  if (analogRead(A0) >= 20 ||  analogRead(A1) >= 20)
  { 
    IsTensionApplied = true;
  } 
  else
  {
    IsTensionApplied = false;
  }
  
  // Detects equal tension from both force sensors
  if (analogRead(A0) <= analogRead(A1) && analogRead(A0) >= analogRead(A1) - EqualTensionThreshold) 
  {
    IsTensionEven = true;
  } 
  else if (analogRead(A1) <= analogRead(A0) && analogRead(A1) >= analogRead(A0) - EqualTensionThreshold)
  {
    IsTensionEven = true;
  }
  
  if (analogRead(A0) < analogRead(A1) - EqualTensionThreshold || analogRead(A1) < analogRead(A0) - EqualTensionThreshold)
  {
    IsTensionEven = false;
  } 

  float accelMagnitude = sqrt(pow (normAccel.XAxis, 2) + pow (normAccel.YAxis, 2) + pow (normAccel.ZAxis, 2)); 
  
  // Detects Steadiness of Aim (just gets acceleration for now)
  if (accelMagnitude < AccelerationSteadinessThreshold)
  {
    IsAimSteady = true;
  } 
  else
  {
    IsAimSteady = false;
  }
  
  // Turns on lights based on aim steadiness and tension
  if (IsTensionEven && IsTensionApplied)
  {
    digitalWrite(6, HIGH);
    
    Serial.println("Tension Applied and even");
  } 
  else
  {
    digitalWrite(6, LOW);
  }
  if (IsAimSteady)
  {
    digitalWrite(5, HIGH);
  } 
  else
  {
    digitalWrite(5, LOW);
  }
  
  delay(10); // Delay a little bit to improve simulation performance
}
