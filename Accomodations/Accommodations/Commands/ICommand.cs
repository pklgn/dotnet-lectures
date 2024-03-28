namespace Accomodations.Commands;

public interface ICommand
{
    void Execute();
    void Undo();
}
