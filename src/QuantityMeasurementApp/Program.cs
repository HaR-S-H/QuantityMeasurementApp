using System;
using QuantityMeasurementApp.Business;

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
                UI.ConsoleMenu.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }
    }
}
