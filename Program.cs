using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAnMonHocNT106;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace DoAnMonHocNT106
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("firebase-key.json")
            });

            Console.WriteLine("Firebase Initialized!");
        }
    }
}
