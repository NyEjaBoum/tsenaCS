using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Windows.Forms; // Pour MessageBox
using tsenaFinal.db; // Namespace pour Connexion
using tsenaFinal.models; // Pour accéder à la classe Box

namespace tsenaFinal.models
{
    internal class Location
    {
        public int IdLocation { get; set; }
        public int IdBox { get; set; }
        public int IdPerson { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; } // Nullable pour gérer NULL dans la base

        public Location(int idLocation, int idBox, int idPerson, DateTime dateDebut, DateTime dateFin)
        {
            IdLocation = idLocation;
            IdBox = idBox;
            IdPerson = idPerson;
            DateDebut = dateDebut;
            DateFin = dateFin;
        }

        public static void Create(Connexion connexion, int idBox, int idPerson, DateTime dateDebut, DateTime dateFin)
        {
            try
            {
                string dateDebutContrat = dateDebut.ToString("yyyy-MM-dd");
                string query;
                /*if (dateFin == null)
                {
                    //query = $"INSERT INTO LOCATION (IDBOX, IDPERSON, dateDebut, dateFin) VALUES ({idBox}, {idPerson}, '{dateDebutContrat}', NULL)";
                }*/
                
                
                    string dateFinContrat = dateFin.ToString("yyyy-MM-dd");
                    query = $"INSERT INTO LOCATION (IDBOX, IDPERSON, dateDebut, dateFin) VALUES ({idBox}, {idPerson}, '{dateDebutContrat}', '{dateFinContrat}')";
                
                Console.WriteLine("eto lesy eh");
                connexion.ExecuteUpdate(query);
                Console.WriteLine("Insertion Location réussie");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        public static Object[] GetDebutContrat(Connexion connexion, int idBox, int idPerson)
        {
            try
            {
                string query = $"SELECT TOP 1 MONTH(DATEDEBUT), YEAR(DATEDEBUT) FROM LOCATION WHERE IDBOX = {idBox} AND IDPERSON = {idPerson} ORDER BY DATEDEBUT DESC";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    var row = rows[0];
                    int mois = Convert.ToInt32(row[0]);
                    int annee = Convert.ToInt32(row[1]);
                    return new object[] {
                           mois,
                           annee
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération de la date de début : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null; // Retourner une liste vide en cas d'erreur
        }


        public static Object[] GetFinContrat(Connexion connexion, int idBox, int idPerson)
        {
            try
            {
                string query = $"SELECT TOP 1 MONTH(DATEFIN), YEAR(DATEFIN) FROM LOCATION WHERE IDBOX = {idBox} AND IDPERSON = {idPerson} ORDER BY DATEDEBUT DESC";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    var row = rows[0];
                    int? mois = row[0] != DBNull.Value ? Convert.ToInt32(row[0]) : (int?)null;
                    int? annee = row[1] != DBNull.Value ? Convert.ToInt32(row[1]) : (int?)null;
                    return new object[] {
                           mois,
                           annee
                    };
                }
                return null; // Aucun contrat trouvé
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération de la date de fin : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static void LouerBox(Connexion connexion, int idBox, int idPerson, string dateStr, string dateStr2)
        {
            try
            {
                DateTime date = DateTime.Parse(dateStr);
                DateTime dateFin = DateTime.Parse(dateStr2);

                Create(connexion, idBox, idPerson, date, dateFin);

                int moisDebut = date.Month;
                int anneeDebut = date.Year;

                decimal loyerApayer = Box.GetLoyerBox(connexion, idBox, moisDebut, anneeDebut);
                // Box.UpdatePerson(connexion, idBox, idPerson); // Commenté dans votre Python, donc ici aussi
                Box.InsertDette(connexion, idPerson, idBox, loyerApayer, moisDebut, anneeDebut);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la location de la box : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuitterBox(Connexion connexion, int idBox, int idPerson, string dateStr)
        {
            try
            {
                DateTime date = DateTime.Parse(dateStr);
                string dateAccess = $"#{date.ToString("yyyy-MM-dd")}#";

                string query = $@"
                    UPDATE LOCATION 
                    SET DATEFIN = {dateAccess}
                    WHERE IDBOX = {idBox}
                    AND IDPERSON = {idPerson}
                    AND DATEFIN IS NULL";
                connexion.ExecuteUpdate(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la fin de location : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static List<Location> GetAllContratPerson(Connexion connexion, int idPerson)
        {
            List<Location> liste = new List<Location>();
            try
            {
                string query = $"SELECT * FROM LOCATION WHERE IDPERSON = {idPerson}";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    int idLocation = Convert.ToInt32(row[0]);
                    int idBox = Convert.ToInt32(row[1]);
                    int idPersonRow = Convert.ToInt32(row[2]);
                    DateTime dateDebut = Convert.ToDateTime(row[3]);
                    DateTime dateFin = Convert.ToDateTime(row[4]);

                    Location location = new Location(idLocation, idBox, idPersonRow, dateDebut, dateFin);
                    liste.Add(location);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des contrats : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return liste;
        }

        public static DateTime? GetMaxDateFinContratPerson(Connexion connexion, int idPerson)
        {
            try
            {
                string query = $"SELECT MAX(dateFin) FROM LOCATION WHERE IDPERSON = {idPerson}";
                var result = connexion.ExecuteQuery(query);

                if (result.Count > 0 && result[0][0] != DBNull.Value)
                {
                    return Convert.ToDateTime(result[0][0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération de la date maximale : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool MisyOlonaVe(Connexion connexion, int idBox, int mois, int annee)
        {
            try
            {
                int moisTotal = annee * 12 + mois;
                Console.WriteLine($"mois total {moisTotal}");

                string query = $@"
                    SELECT idPerson 
                    FROM LOCATION 
                    WHERE idBox = {idBox} 
                    AND (
                        dateFin IS NULL 
                        OR (YEAR(dateFin) * 12 + MONTH(dateFin)) >= {moisTotal}
                    )
                    AND (YEAR(dateDebut) * 12 + MONTH(dateDebut)) <= {moisTotal}
                    ORDER BY (YEAR(dateDebut) * 12 + MONTH(dateDebut)) DESC";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    int idPerson = Convert.ToInt32(rows[0][0]);
                    Console.WriteLine($"id Box {idBox} mois {mois} annee {annee} idPerson {idPerson}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return false;
            }
        }

        public static bool IsHisBoxMoisAnnee(Connexion connexion, int idBox, int idPerson, int mois, int annee)
        {
            try
            {
                int moisTotal = annee * 12 + mois;
                Console.WriteLine($"mois total {moisTotal}");

                // Note : Votre table LOCATION n'a pas de champ "type" dans le schéma actuel, je suppose qu'il faut ajuster
                string query = $@"
                    SELECT idPerson 
                    FROM LOCATION 
                    WHERE IDBOX = {idBox} 
                    AND idPerson = {idPerson} 
                    AND ((YEAR(dateDebut) * 12 + MONTH(dateDebut)) <= {moisTotal})
                    AND (dateFin IS NULL OR (YEAR(dateFin) * 12 + MONTH(dateFin)) >= {moisTotal})
                    ORDER BY (YEAR(dateDebut) * 12 + MONTH(dateDebut)) DESC";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    int foundIdPerson = Convert.ToInt32(rows[0][0]);
                    Console.WriteLine($"id Box {idBox} idPerson {foundIdPerson} mois {mois} annee {annee}");
                    return true; // Si le contrat existe pour cet idPerson
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return false;
            }
        }

        public static object[] IsDetteInContrat(Connexion connexion, int idBox, int idPerson, int moisDette, int anneeDette)
        {
            try
            {
                int moisDetteTotal = anneeDette * 12 + moisDette;
                string query = $@"
                    SELECT MONTH(dateDebut), YEAR(dateDebut), MONTH(dateFin), YEAR(dateFin)
                    FROM LOCATION 
                    WHERE idBox = {idBox} AND idPerson = {idPerson}
                    AND (YEAR(dateDebut) * 12 + MONTH(dateDebut)) <= {moisDetteTotal}
                    AND (dateFin IS NULL OR (YEAR(dateFin) * 12 + MONTH(dateFin)) >= {moisDetteTotal})";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    var row = rows[0];
                    return new object[] {
                        Convert.ToInt32(row[0]), // mois_debut
                        Convert.ToInt32(row[1]), // annee_debut
                        row[2] != DBNull.Value ? Convert.ToInt32(row[2]) : (int?)null, // mois_fin
                        row[3] != DBNull.Value ? Convert.ToInt32(row[3]) : (int?)null  // annee_fin
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la vérification de la dette : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }



        public static Object[] GetProchainContrat(Connexion connexion, int idBox, int idPerson, int moisFin, int anneeFin)
        {
            try
            {
                string dateFinStr = $"{anneeFin}-{moisFin:02d}-01";
                string query = $@"
                    SELECT TOP 1 MONTH(dateDebut), YEAR(dateDebut)
                    FROM LOCATION 
                    WHERE idBox = {idBox} AND idPerson = {idPerson} 
                    AND dateDebut > #{dateFinStr}#
                    ORDER BY dateDebut ASC";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    var row = rows[0];
                    return new object[] {
                           Convert.ToInt32(row[0]),
                           Convert.ToInt32(row[1])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération du prochain contrat : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}