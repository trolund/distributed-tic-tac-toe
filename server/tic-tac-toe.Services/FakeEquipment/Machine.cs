namespace Manufacturing.Services.FakeEquipment;

public class Machine
{
    private MachineState _currentState;
    public string MachineName;
    public string ProductionLine;

    public Machine(string machineName, string productionLine)
    {
        MachineName = machineName;
        ProductionLine = productionLine;
        _currentState = MachineState.Red; // Initial state
        Console.WriteLine("Initial state: Red (Standing still)");
    }

    public void ChangeState(MachineState newState)
    {
        switch (_currentState)
        {
            case MachineState.Red:
                if (newState == MachineState.Yellow)
                {
                    _currentState = newState;
                    Console.WriteLine("Changed state to Yellow (Starting up)");
                }
                else
                {
                    Console.WriteLine("Invalid state transition from Red to " + newState);
                }

                break;

            case MachineState.Yellow:
                if (newState == MachineState.Green)
                {
                    _currentState = newState;
                    Console.WriteLine("Changed state to Green (Producing normally)");
                }
                else if (newState == MachineState.Red)
                {
                    _currentState = newState;
                    Console.WriteLine("Changed state to Red (Winding down to standing still)");
                }
                else
                {
                    Console.WriteLine("Invalid state transition from Yellow to " + newState);
                }

                break;

            case MachineState.Green:
                if (newState == MachineState.Yellow)
                {
                    _currentState = newState;
                    Console.WriteLine("Changed state to Yellow (Winding down)");
                }
                else
                {
                    Console.WriteLine("Invalid state transition from Green to " + newState);
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public MachineState GetCurrentState()
    {
        return _currentState;
    }
}