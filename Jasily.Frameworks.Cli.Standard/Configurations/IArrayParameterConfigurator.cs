namespace Jasily.Frameworks.Cli.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public interface IArrayParameterConfigurator
    {
        /// <summary>
        /// get or set array parameter
        /// </summary>
        int MinLength { get; set; }

        /// <summary>
        /// get or set array parameter
        /// </summary>
        int MaxLength { get; set; }
    }
}