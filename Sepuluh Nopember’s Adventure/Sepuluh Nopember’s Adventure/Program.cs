using System;
using System.Windows.Forms;

namespace Sepuluh_Nopember_s_Adventure
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); 
            Application.Run(new Second());
        }
    }
}