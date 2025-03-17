using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsenaFinal.db;
using tsenaFinal.models;

namespace tsenaFinal.models
{
    internal class Marche
    {
        public int IdMarche { get; set; }
        public string NomMarche { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Marche(int idMarche, string nomMarche, int x, int y, int width, int height)
        {
            IdMarche = idMarche;
            NomMarche = nomMarche;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static int Create(Connexion connexion, string nomMarche, int x, int y, int width, int height)
        {
            try
            {
                string queryInsert = $"INSERT INTO MARCHE (nomMarche, x, y, width, height) VALUES ('{nomMarche}', {x}, {y}, {width}, {height})";
                connexion.ExecuteUpdate(queryInsert);

                string queryId = "SELECT MAX(idMarche) FROM MARCHE";
                var rows = connexion.ExecuteQuery(queryId);

                if (rows.Count > 0)
                {
                    int idMarche = Convert.ToInt32(rows[0][0]);
                    Console.WriteLine($"Insertion réussie, ID: {idMarche}");
                    return idMarche;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur d'insertion : {ex.Message}");
                return 0;
            }
        }

        public static List<Marche> GetAll(Connexion connexion)
        {
            List<Marche> marches = new List<Marche>();
            try
            {
                string query = "SELECT * FROM MARCHE";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    int idMarche = Convert.ToInt32(row[0]);
                    string nomMarche = row[1].ToString();
                    int x = Convert.ToInt32(row[2]);
                    int y = Convert.ToInt32(row[3]);
                    int width = Convert.ToInt32(row[4]);
                    int height = Convert.ToInt32(row[5]);
                    marches.Add(new Marche(idMarche, nomMarche, x, y, width, height));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des marchés : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return marches;
        }

        public static decimal GetTarifSpecial(Connexion connexion, int idMarche, int mois, int annee)
        {
            try
            {
                string query = $"SELECT MONTANT FROM TARIF_SPECIAL WHERE IDMARCHE = {idMarche} AND MOIS = {mois} AND ANNEE = {annee}";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    return Convert.ToDecimal(row[0]);
                }
                return 0; // Retourne 0 si aucun tarif spécial n'est trouvé
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du tarif spécial : {ex.Message}");
                return 0;
            }
        }

        public static List<Box> GetAllBox(Connexion connexion, int idMarche)
        {
            List<Box> boxes = new List<Box>();
            try
            {
                string query = $"SELECT idBox FROM BOX WHERE IDMARCHE = {idMarche}";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    int idBox = Convert.ToInt32(row[0]);
                    Box box = Box.GetById(connexion, idBox);
                    if (box != null)
                    {
                        boxes.Add(box);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des boxes du marché : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return boxes;
        }

        public static Marche GetById(Connexion connexion, int idMarche)
        {
            try
            {
                string query = $"SELECT * FROM marche WHERE idMarche = {idMarche}";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count == 0)
                    return null;

                var row = rows[0];
                int idMarcheValue = Convert.ToInt32(row[0]);
                string nomMarche = row[1].ToString();
                int x = Convert.ToInt32(row[2]);
                int y = Convert.ToInt32(row[3]);
                int width = Convert.ToInt32(row[4]);
                int height = Convert.ToInt32(row[5]);
                return new Marche(idMarcheValue, nomMarche, x, y, width, height);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération du marché : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static decimal GetLoyerMarche(Connexion connexion, int idMarche, int mois, int annee)
        {
            try
            {
                int? moisTotal = annee * 12 + mois;
                string query = $@"
                    SELECT MONTANT FROM LOYER 
                    WHERE IDMARCHE = {idMarche} 
                    AND ((YEAR(DATELOYER) * 12 + MONTH(DATELOYER)) <= {moisTotal}) 
                    ORDER BY (YEAR(DATELOYER) * 12 + MONTH(DATELOYER)) DESC";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    return Convert.ToDecimal(rows[0][0]);
                }
                return 0; // Retourne 0 si aucun loyer n'est trouvé
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du loyer : {ex.Message}");
                return 0;
            }
        }

    }
}
