using DoAnCuoiKiCSDL.Admin;
using DoAnCuoiKiCSDL.Analyst;
using DoAnCuoiKiCSDL.Dishes;
using DoAnCuoiKiCSDL.EmployeeFolder;
using DoAnCuoiKiCSDL.Order;
using DoAnCuoiKiCSDL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login loginForm = new Login();
            AnalystForm form1= new AnalystForm();
            Application.Run(new Login());
        }
    }
}
