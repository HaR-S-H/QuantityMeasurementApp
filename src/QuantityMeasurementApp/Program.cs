using System;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.UI;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Console application entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main application entry point.
        /// </summary>
        private static void Main()
        {
            try
            {
                IConsoleMenu menuApp = new ConsoleMenu();
                menuApp.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }
    }
}
