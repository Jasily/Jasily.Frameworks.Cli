using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Converters
{
    internal static class ValueConverterExtensions
    {
        public static void InstallValueConverter([NotNull] this IServiceCollection isc)
        {
            if (isc == null) throw new ArgumentNullException(nameof(isc));

            isc.AddSingleton<IValueConverter<bool>, BooleanConverter>()
                .AddSingleton<IValueConverter<char>, CharConverter>()
                .AddSingleton<IValueConverter<sbyte>, SByteConverter>()
                .AddSingleton<IValueConverter<byte>, ByteConverter>()
                .AddSingleton<IValueConverter<short>, Int16Converter>()
                .AddSingleton<IValueConverter<ushort>, UInt16Converter>()
                .AddSingleton<IValueConverter<char>, CharConverter>()
                .AddSingleton<IValueConverter<int>, Int32Converter>()
                .AddSingleton<IValueConverter<uint>, UInt32Converter>()
                .AddSingleton<IValueConverter<long>, Int64Converter>()
                .AddSingleton<IValueConverter<ulong>, UInt64Converter>()
                .AddSingleton<IValueConverter<float>, SingleConverter>()
                .AddSingleton<IValueConverter<double>, DoubleConverter>()
                .AddSingleton<IValueConverter<decimal>, DecimalConverter>()
                .AddSingleton<IValueConverter<DateTime>, DateTimeConverter>()
                .AddSingleton<IValueConverter<string>, StringConverter>();
        }
    }
}