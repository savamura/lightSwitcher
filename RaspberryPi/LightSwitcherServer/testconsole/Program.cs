﻿using System;
using Serilog;
using ArduinoLightswitcherGateway;

namespace testconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Get a list of serial port names.             
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            using (var arduinoGateway = new ArduinoGateway(
                new ArduinoGatewayConfig("ttyACM", 9600)))
            using (var lightswitcher = new LightSwitcherGateway(
                arduinoGateway,
                new LightSwitcherConfig(48, 97, 14)))
            {
                arduinoGateway.Send(0);

                var command = "";
                Console.WriteLine("Print '1|0 <PIN>' or x for exit");
            
                while (true)
                {
                    command = Console.ReadLine();
                    if (command == "x")
                    {
                        break;
                    }

                    var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var turnOn = false;
                    if (parts.Length == 0)
                    {
                        continue;
                    }
                    
                    if (parts[0] == "1")
                    {
                        turnOn = true;
                    } 
                    else if (parts[0] == "0")
                    {
                        turnOn = false;
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized command");
                        continue;
                    }

                    if (!byte.TryParse(parts[1], out byte pin))
                    {
                        Console.WriteLine("Can't parse pin");
                        continue;
                    }

                    var result = lightswitcher.ChangeSwitchState(pin, turnOn);
                    Console.WriteLine($"result: {result}");
                }
            }
        }
    }
}