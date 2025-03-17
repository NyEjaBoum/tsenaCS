using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Windows.Forms; // Pour MessageBox
using tsenaFinal.db;

namespace tsenaFinal.models
{
    internal class tarif_special
    {
        public int IdTarifSpecial { get; set; }
        public int IdMarche { get; set; }
        public int Mois { get; set; }
        public int Annee { get; set; }
        public decimal Montant { get; set; }

        public tarif_special(int idTarifSpecial, int idMarche, int mois, int annee, decimal montant)
        {
            IdTarifSpecial = idTarifSpecial;
            IdMarche = idMarche;
            Mois = mois;
            Annee = annee;
            Montant = montant;
        }

        public static List<tarif_special> GetAllTarifSpecial(Connexion connexion)
        {
            List<tarif_special> tariffs = new List<tarif_special>();
            try
            {
                string query = "SELECT * FROM TARIF_SPECIAL";
                var rows = connexion.ExecuteQuery(query);

                foreach (var row in rows)
                {
                    int idTarif = Convert.ToInt32(row[0]);
                    int idMarche = Convert.ToInt32(row[1]);
                    int mois = Convert.ToInt32(row[2]);
                    int annee = Convert.ToInt32(row[3]);
                    decimal montant = Convert.ToDecimal(row[4]);
                    tariffs.Add(new tarif_special(idTarif, idMarche, mois, annee, montant));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des tarifs spéciaux : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tariffs;
        }

        public static decimal GetTarifSpecialMoisAnnee(Connexion connexion, int mois, int annee)
        {
            try
            {
                string query = $"SELECT montant FROM TARIF_SPECIAL WHERE MOIS = {mois} AND ANNEE = {annee}";
                var rows = connexion.ExecuteQuery(query);
                decimal tarif = 0;

                foreach (var row in rows)
                {
                    tarif = Convert.ToDecimal(row[0]);
                }
                return tarif;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du tarif spécial : {ex.Message}");
                return 0;
            }
        }
    }
}