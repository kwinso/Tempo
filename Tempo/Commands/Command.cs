namespace Tempo.Commands
{
    public abstract class Command
    {
        
        public abstract bool Match(string[] args);
        
        public abstract void Execute(string[] args);
    }
}