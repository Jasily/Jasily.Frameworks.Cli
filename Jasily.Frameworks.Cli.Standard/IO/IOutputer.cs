namespace Jasily.Frameworks.Cli.IO
{
    public interface IOutputer
    {
        void Write(OutputLevel level, string value);

        void WriteLine(OutputLevel level, string value);
    }
}
