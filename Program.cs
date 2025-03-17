using System;
using tsenaFinal.db;
using tsenaFinal.models;

namespace tsenaFinal
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Allouer une console
            //AllocConsole();

            ApplicationConfiguration.Initialize();

            string connectionString = "DRIVER={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\\Users\\Ny Eja\\Documents\\fianarana ITU\\S4\\PROG\\C#\\tsenaFinal\\Database1.accdb";
            Connexion connexion = new Connexion();
            try
            {
                connexion.OpenConnection(connectionString);
                Console.WriteLine("Connexion à la base de données réussie");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ouverture de la connexion : {ex.Message}");
                return;
            }

            // Insérer les données (décommentez si nécessaire)
            //Insert.InsertDonnees(connexion);

            // Récupérer toutes les personnes et afficher leurs dettes
            /*Console.WriteLine("Calcul des dettes pour janvier 2023...");
            var allPersons = Person.GetAll(connexion); // Suppose que cette méthode existe
            foreach (var person in allPersons)
            {
                decimal dette = Paiement.SommeDetteTokonyNaloany(connexion, person.IdPerson, 12, 2026);
                Console.WriteLine($"Dette de {person.IdPerson} est {dette}");
            }

            //Console.WriteLine($"tyyyyyyy {Paiement.GetNextContrat(connexion,5,12,2022)}");

            // Fermer la connexion
            connexion.CloseConnection();
            Console.WriteLine("Connexion fermée");

            Console.WriteLine("Fin du programme. Appuyez sur une touche pour quitter...");
            Console.ReadKey(); // Garde la console ouverte
            FreeConsole();*/ // Ferme la console proprement

            Application.Run(new Form1(connexion));
        }

        // Importations nécessaires pour AllocConsole et FreeConsole
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();
    }
}