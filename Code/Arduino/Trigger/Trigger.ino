//Constant variables
const int portasyncPin = 5; //Number of portasync pin
const int footswitchPin = 6; //Number of footswitch trigger pin
const int eegPin = 7; //Number of EEG pin
const int redLed = 52; //Number of red LED pin
const int greenLed = 50; //Number of blue LED pin
const int blueLed = 53; //Number of green LED pin
int currentMicros; //Variable 1 for how long program has been running
int currentMicros1; //Variable 2 for how long program has been running 
String timer; //Variable to store a timer

void setup() {
 pinMode(redLed, OUTPUT); //Define led as output
 pinMode(greenLed, OUTPUT); //Define led as output
 pinMode(blueLed, OUTPUT); //Define led as output
 pinMode(portasyncPin, OUTPUT); //Define portasync as output
 pinMode(footswitchPin, OUTPUT); //Define footswitch as output
 pinMode(eegPin, OUTPUT); //Define EEG as output
 Serial.begin(9600); //Starts serial connection and sets speed to 9600 bps
}

void loop() {
  
      char inputTrigger; //Variable for input trigger
      
      if (Serial.available() > 0) { //Sends data only when data is available
        inputTrigger = Serial.read(); //Read from the seral port
        currentMicros = micros(); //Get time 1
        if (inputTrigger == 'C') { //If serial port is successfully connected, turn on green led
          digitalWrite(greenLed, HIGH);
        }
        if (inputTrigger == 'U') { //If serial port is successfully unconnected, turn off green led
          digitalWrite(greenLed, LOW);
        }
        if (inputTrigger == '0') { //If a trigger is received
            analogWrite(portasyncPin, 175); //Sends pwm signal of 175 to portasync
            analogWrite(footswitchPin,175); //Sends pwm signal of 175 to footswitch
            currentMicros1 = micros(); //Get time 2
            delay(220); //Waits 220 ms
            analogWrite(footswitchPin,0); //Sends pwm signal of 0 to footswitch
            analogWrite(portasyncPin, 0); //Sends pwm signal of 0 to portasync
            timer = String(currentMicros1-currentMicros); //Calculate time between time 1 and time 2
            Serial.println(timer); //Printer the timer to the serial port
        } 
      }
}
