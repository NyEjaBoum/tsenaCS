using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tsenaFinal.db;
using tsenaFinal.models;

namespace tsenaFinal.models
{
    internal class Box
    {
        public int IdBox { get; set; }
        public int IdMarche { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumeroBox { get; set; }

        public Box(int idBox, int idMarche, int x, int y, int width, int height, int numeroBox)
        {
            IdBox = idBox;
            IdMarche = idMarche;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            NumeroBox = numeroBox;
        }

        public static decimal GetSuperficie(int width,int height)
        {
            return width * height;
        }

        public static void InsertDette(Connexion connexion,int idPerson,int idBox,decimal montant,int mois,int annee)
        {
            try
            {
                string query = $"INSERT INTO DETTE (IDPERSON, IDBOX, MONTANT, MOIS, ANNEE) VALUES ({idPerson}, {idBox}, {montant}, {mois}, {annee})";
                connexion.ExecuteUpdate(query);
                Console.WriteLine("insert dette reussie");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        public static void Create(Connexion connexion, int idMarche, int x, int y, int width, int height, int numeroBox)
        {
            try
            {
                string query = $"INSERT INTO BOX (idMarche,x, y, width, height,numeroBox) VALUES ({idMarche},{x}, {y}, {width}, {height},{numeroBox})";
                connexion.ExecuteUpdate(query);
                Console.WriteLine("Insertion Box réussie");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        public static List<Box> GetAll(Connexion connexion)
        {
            List<Box> boxes = new List<Box>();
            try
            {
                string query = "SELECT * FROM box";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    int idBox = Convert.ToInt32(row[0]);
                    int idMarche = Convert.ToInt32(row[1]);
                    int x = Convert.ToInt32(row[2]);
                    int y = Convert.ToInt32(row[3]);
                    int width = Convert.ToInt32(row[4]);
                    int height = Convert.ToInt32(row[5]);
                    int numeroBox = Convert.ToInt32(row[6]);
                    boxes.Add(new Box(idBox, idMarche, x, y, width, height, numeroBox));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des boxes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return boxes;
        }

        public static int GetIdMarche(Connexion connexion, int idBox)
        {
            try
            {
                string query = $"SELECT idMarche FROM box WHERE idBox = {idBox}";
                var rows = connexion.ExecuteQuery(query);
                return rows.Count > 0 ? Convert.ToInt32(rows[0][0]) : 0; // Remplace Length par Count
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return 0;
            }
        }

        public static Box GetById(Connexion connexion, int idBox)
        {
            try
            {
                string query = $"SELECT * FROM box WHERE idBox = {idBox}";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    int idBoxValue = Convert.ToInt32(row[0]);
                    int idMarche = Convert.ToInt32(row[1]);
                    int x = Convert.ToInt32(row[2]);
                    int y = Convert.ToInt32(row[3]);
                    int width = Convert.ToInt32(row[4]);
                    int height = Convert.ToInt32(row[5]);
                    int numeroBox = Convert.ToInt32(row[6]);
                    return new Box(idBoxValue, idMarche, x, y, width, height, numeroBox);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération de la Box : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static List<Box> GetAllBoxPerson(Connexion connexion, int idPerson)
        {
            List<Box> boxes = new List<Box>();
            try
            {
                string query = $"SELECT * FROM BOX WHERE IDPERSON = {idPerson}";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    int idBox = Convert.ToInt32(row[0]);
                    int idMarche = Convert.ToInt32(row[1]);
                    int x = Convert.ToInt32(row[2]);
                    int y = Convert.ToInt32(row[3]);
                    int width = Convert.ToInt32(row[4]);
                    int height = Convert.ToInt32(row[5]);
                    int numeroBox = Convert.ToInt32(row[6]);
                    boxes.Add(new Box(idBox, idMarche, x, y, width, height, numeroBox));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des boxes pour la personne : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return boxes;
        }

        public static decimal GetLoyerBox(Connexion connexion, int idBox, int mois, int annee)
        {
            try
            {
                Box box = GetById(connexion, idBox);
                if (box == null)
                {
                    Console.WriteLine($"Aucune Box trouvée pour idBox = {idBox}");
                    return 0;
                }

                int idMarche = box.IdMarche;
                decimal loyerApayer = 0;
                decimal tarifSpe = tarif_special.GetTarifSpecialMoisAnnee(connexion, mois, annee); // Assumes tarif_special class exists

                if (tarifSpe == 0)
                {
                    loyerApayer = Marche.GetLoyerMarche(connexion, idMarche, mois, annee); // Adjusted to include mois, annee
                }
                else
                {
                    loyerApayer = tarifSpe;
                }

                decimal superficie = GetSuperficie(box.Width, box.Height);
                return superficie * loyerApayer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return 0;
            }
        }
    }
}
