using ArduinoLightswitcherGateway;

namespace ArduinoLightswitcherGateway
{
    public static class LightSwitcherGatewayFactory
    {
        public static ILightSwitcherGateway CreateLightswitcherGateway(string arduinoGatewayType)
        {
            switch (arduinoGatewayType)
            {
                case "fake":
                    return new LightSwitcherGateway(
                        new DummyArduinoGateway(),
                        new LightSwitcherConfig(48, 97, 14));

                default:
                    return new LightSwitcherGateway(
                        new ArduinoGateway(
                            new ArduinoGatewayConfig("ttyACM", 9600)),
                        new LightSwitcherConfig(48, 97, 14));
            }
        }
    }
}