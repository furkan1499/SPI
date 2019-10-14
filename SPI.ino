#define trigPin 9
#define echoPin 8
 
void setup() {
Serial.begin (9600);
pinMode(trigPin, OUTPUT);
pinMode(echoPin, INPUT);
}
void loop() {
int sure, mesafe;
digitalWrite(trigPin, LOW);
delayMicroseconds(2);
digitalWrite(trigPin, HIGH);
delayMicroseconds(10);
digitalWrite(trigPin, LOW);
sure = pulseIn(echoPin, HIGH);
mesafe = (sure/2) / 29.1;
Serial.println(mesafe);

  
delay(1000);
 
}
