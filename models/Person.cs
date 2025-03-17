using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsenaFinal.db;

namespace tsenaFinal.models
{
    internal class Person
    {
        public int IdPerson { get; set; }
        public string Nom { get; set; }

        public Person(int idPerson, string nom)
        {
            IdPerson = idPerson;
            Nom = nom ?? string.Empty; // Gestion de null
        }

        public static List<Person> GetAll(Connexion connexion)
        {
            List<Person> persons = new List<Person>();
            try
            {
                string query = "SELECT * FROM PERSON";
                var rows = connexion.ExecuteQuery(query);

                for (int i = 0; i < rows.Count; i++) // Remplace Length par Count
                {
                    int idPerson = Convert.ToInt32(rows[i][0]);
                    string nom = rows[i][1]?.ToString() ?? string.Empty;
                    persons.Add(new Person(idPerson, nom));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des personnes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return persons;
        }

        public static Person GetById(Connexion connexion, int idPerson)
        {
            try
            {
                string query = $"SELECT * FROM PERSON WHERE idPerson = {idPerson}";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0) // Remplace Length par Count
                {
                    var row = rows[0];
                    int idPersonValue = Convert.ToInt32(row[0]);
                    string nom = row[1]?.ToString() ?? string.Empty;
                    return new Person(idPersonValue, nom);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération de la personne : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Méthode ajoutée pour GetPlusAncienDette
        public static Object[] GetPlusAncienDette(Connexion connexion, int idPerson)
        {
            try
            {
                string query = $@"
                    SELECT TOP 1 idBox, montant, mois, annee 
                    FROM DETTE 
                    WHERE idPerson = {idPerson} 
                    AND montant > 0 
                    ORDER BY annee ASC, mois ASC";
                var rows = connexion.ExecuteQuery(query);

                if (rows.Count > 0)
                {
                    var row = rows[0];
                    int idBox = Convert.ToInt32(row[0]);
                    decimal montant = Convert.ToDecimal(row[1]);
                    int mois = Convert.ToInt32(row[2]);
                    int annee = Convert.ToInt32(row[3]);
                    return new object[]
                    {
                        idBox,montant,mois,annee
                    };
                }
                return null; // Valeurs par défaut si aucune dette
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération de la dette la plus ancienne : {ex.Message}");
                return null;
            }
        }

        // Méthode ajoutée pour UpdateDette
        public static bool UpdateDette(Connexion connexion, int idBox, int idPerson, int mois, int annee, decimal nouveauReste)
        {
            try
            {
                string query = $"UPDATE DETTE SET montant = {nouveauReste} WHERE idBox = {idBox} AND idPerson = {idPerson} AND mois = {mois} AND annee = {annee}";
                connexion.ExecuteUpdate(query);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de la dette : {ex.Message}");
                return false;
            }
        }
    }
}
