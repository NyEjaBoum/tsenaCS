using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsenaFinal.db;
using tsenaFinal.models;

namespace tsenaFinal
{
    internal class Insert
    {
        public Insert()
        {
            // Constructeur vide, comme dans le code Python
        }

        public static void InsertBox(Connexion connexion)
        {
            // Boîtes pour Nosy Be (grille 5x4)
            int startX = 30;  // X de départ (selon la table Marche)
            int startY = 100; // Y de départ (selon la table Marche)
            int width = 30;   // Largeur selon la table Marche
            int height = 50;  // Hauteur selon la table Marche
            int spacingX = 40; // Espacement horizontal (30 + 10)
            int spacingY = 60; // Espacement vertical (50 + 10)

            for (int row = 0; row < 4; row++) // 4 lignes
            {
                for (int col = 0; col < 5; col++) // 5 colonnes
                {
                    int index = row * 5 + col + 1; // Index de 1 à 20
                    if (index <= 20) // Limite à 20 boîtes
                    {
                        int x = startX + (col * spacingX); // X augmente dans la ligne
                        int y = startY + (row * spacingY); // Y descend pour chaque ligne
                        Box.Create(connexion, 1, x, y, 2, 5, index);
                    }
                }
            }

            // Boîtes pour Andravohangy (grille 5x3)
            startX = 400; // X de départ (selon la table Marche)
            width = 60;   // Largeur selon la table Marche
            height = 50;  // Hauteur selon la table Marche
            spacingX = 70; // Espacement horizontal (60 + 10)

            for (int row = 0; row < 3; row++) // 3 lignes
            {
                for (int col = 0; col < 5; col++) // 5 colonnes
                {
                    int index = row * 5 + col + 1; // Index de 1 à 15
                    if (index <= 15) // Limite à 15 boîtes
                    {
                        int x = startX + (col * spacingX); // X augmente dans la ligne
                        int y = startY + (row * spacingY); // Y descend pour chaque ligne
                        Box.Create(connexion, 2, x, y, 3, 5, index);
                    }
                }
            }
        }

        public static void InsertDonnees(Connexion connexion)
        {
            /*Marche.Create(connexion, "Nosy Be", 30, 100, 30, 50);
            Marche.Create(connexion, "Andravoahangy", 400, 100, 40, 50);*/

            Insert.InsertBox(connexion);

            // Locations pour Nosy Be
            Location.LouerBox(connexion, 1, 1, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 2, 1, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 3, 1, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 4, 1, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 5, 1, "2022-01-01", "2022-12-31");

            Location.LouerBox(connexion, 6, 2, "2022-01-01", "3000-12-31"); // DateFin = null
            Location.LouerBox(connexion, 7, 2, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 8, 2, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 9, 2, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 10, 2, "2022-01-01", "3000-12-31");

            Location.LouerBox(connexion, 11, 3, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 12, 3, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 13, 3, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 14, 3, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 15, 3, "2022-01-01", "3000-12-31");

            Location.LouerBox(connexion, 16, 4, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 17, 4, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 18, 4, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 19, 4, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 20, 4, "2022-01-01", "3000-12-31");

            // Locations pour Andravoahangy
            Location.LouerBox(connexion, 21, 4, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 22, 4, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 23, 4, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 24, 4, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 25, 4, "2022-01-01", "2022-12-31");

            Location.LouerBox(connexion, 26, 4, "2022-01-01","3000-12-31"); //null tsisy fin de contrat
            Location.LouerBox(connexion, 27, 4, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 28, 4, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 29, 4, "2022-01-01", "3000-12-31");
            Location.LouerBox(connexion, 30, 4, "2022-01-01", "3000-12-31");

            Location.LouerBox(connexion, 31, 5, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 32, 5, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 33, 5, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 34, 5, "2022-01-01", "2022-12-31");
            Location.LouerBox(connexion, 35, 5, "2022-01-01", "2022-12-31");

            // Changements Nosy Be (2023)
            Location.LouerBox(connexion, 1, 6, "2023-01-01", "3000-12-31"); //null tsisy fin de contrat
            Location.LouerBox(connexion, 2, 6, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 3, 6, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 4, 6, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 5, 6, "2023-01-01", "3000-12-31");

            // Changements Andravoahangy (2023)
            Location.LouerBox(connexion, 21, 7, "2023-01-01", "3000-12-31"); //null tsisy fin de contrat
            Location.LouerBox(connexion, 22, 7, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 23, 7, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 24, 7, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 25, 7, "2023-01-01", "3000-12-31");

            Location.LouerBox(connexion, 31, 8, "2023-01-01", "3000-12-31"); // null tsisy fin de contrat
            Location.LouerBox(connexion, 32, 8, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 33, 8, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 34, 8, "2023-01-01", "3000-12-31");
            Location.LouerBox(connexion, 35, 8, "2023-01-01", "3000-12-31");

            /* // Paiements 2022
             Paiement.RealInsertion(connexion, 1, 1, 12, 2022, 4000000, DateTime.Parse("2022-12-12"));
             Paiement.RealInsertion(connexion, 1, 2, 12, 2022, 6000000, DateTime.Parse("2022-12-12"));
             Paiement.RealInsertion(connexion, 1, 3, 12, 2022, 6000000, DateTime.Parse("2022-12-12"));
             Paiement.RealInsertion(connexion, 1, 4, 12, 2022, 23600000, DateTime.Parse("2022-12-12"));
             Paiement.RealInsertion(connexion, 1, 5, 12, 2022, 11700000, DateTime.Parse("2022-12-12"));

             // Paiements 2023
             Paiement.RealInsertion(connexion, 1, 6, 1, 2023, 400000, DateTime.Parse("2023-01-01"));
             Paiement.RealInsertion(connexion, 1, 2, 1, 2023, 600000, DateTime.Parse("2023-01-01"));
             Paiement.RealInsertion(connexion, 1, 3, 1, 2023, 600000, DateTime.Parse("2023-01-01"));
             Paiement.RealInsertion(connexion, 1, 4, 1, 2023, 1050000, DateTime.Parse("2023-01-01"));
             Paiement.RealInsertion(connexion, 1, 7, 1, 2023, 1050000, DateTime.Parse("2023-01-01"));
             Paiement.RealInsertion(connexion, 1, 8, 1, 2023, 1050000, DateTime.Parse("2023-01-01"));*/

            // Paiements 2022
            Paiement.RealInsertion(connexion, 1, 1, 4000000, DateTime.Parse("2022-12-12"),5);
            Paiement.RealInsertion(connexion, 1, 2, 6000000, DateTime.Parse("2022-12-12"),5);
            Paiement.RealInsertion(connexion, 1, 3, 6000000, DateTime.Parse("2022-12-12"),5);
            Paiement.RealInsertion(connexion, 1, 4, 23600000, DateTime.Parse("2022-12-12"),5);
            Paiement.RealInsertion(connexion, 1, 5, 11700000, DateTime.Parse("2022-12-12"),5);

            // Paiements 2023
            Paiement.RealInsertion(connexion, 1, 6, 400000, DateTime.Parse("2023-01-01"),5);
            Paiement.RealInsertion(connexion, 1, 2, 600000, DateTime.Parse("2023-01-01"),5);
            Paiement.RealInsertion(connexion, 1, 3, 600000, DateTime.Parse("2023-01-01"),5);
            Paiement.RealInsertion(connexion, 1, 4, 1050000, DateTime.Parse("2023-01-01"),5);
            Paiement.RealInsertion(connexion, 1, 7, 1050000, DateTime.Parse("2023-01-01"),5);
            Paiement.RealInsertion(connexion, 1, 8, 1050000, DateTime.Parse("2023-01-01"),5);
        }
    }
}
