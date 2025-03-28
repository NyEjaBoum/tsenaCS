﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsenaFinal.db; // Namespace pour Connexion

namespace tsenaFinal.models
{
    internal class Paiement
    {
        public int IdPayement { get; set; }
        public int IdBox { get; set; }
        public int IdPerson { get; set; }
        public int Mois { get; set; }
        public int Annee { get; set; }
        public decimal Montant { get; set; }
        public DateTime Date { get; set; }

        public Paiement(int idPayement, int idBox, int idPerson, int mois, int annee, decimal montant, DateTime date)
        {
            IdPayement = idPayement;
            IdBox = idBox;
            IdPerson = idPerson;
            Mois = mois;
            Annee = annee;
            Montant = montant;
            Date = date;
        }

        public static object[] GetActiveContract(Connexion connexion, int idBox, int idPerson, int moisApayer, int anneeApayer)
        {
            try
            {
                string queryContrat = $@"
                    SELECT MONTH(dateDebut), YEAR(dateDebut), MONTH(dateFin), YEAR(dateFin)
                    FROM LOCATION 
                    WHERE idBox = {idBox} AND idPerson = {idPerson}
                    AND (YEAR(dateDebut) * 12 + MONTH(dateDebut)) <= ({anneeApayer} * 12 + {moisApayer})
                    AND (dateFin IS NULL OR (YEAR(dateFin) * 12 + MONTH(dateFin)) >= ({anneeApayer} * 12 + {moisApayer}))";
                var contrat = connexion.ExecuteQuery(queryContrat);
                return contrat.Count > 0 ? contrat[0] : null; // Retourne mois_debut, annee_debut, mois_fin, annee_fin
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération du contrat actif : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static decimal SumLoyerMoisAnneeBox(Connexion connexion, int idBox, int mois, int annee)
        {
            try
            {
                string query = $"SELECT SUM(MONTANT) FROM PAIEMENT WHERE MOIS = {mois} AND ANNEE = {annee} AND IDBOX = {idBox}";
                var rows = connexion.ExecuteQuery(query);
                decimal total = 0;

                if (rows.Count > 0 && rows[0][0] != DBNull.Value)
                {
                    total = Convert.ToDecimal(rows[0][0]);
                }
                return total;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du calcul de la somme des loyers pour la box : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        public static decimal SumLoyerMoisAnneeBoxPerson(Connexion connexion, int idBox, int idPerson, int? mois, int? annee)
        {
            try
            {
                string query = $"SELECT SUM(MONTANT) FROM PAIEMENT WHERE MOIS = {mois} AND ANNEE = {annee} AND IDBOX = {idBox} AND IDPERSON = {idPerson}";
                var rows = connexion.ExecuteQuery(query);
                decimal total = 0;

                if (rows.Count > 0 && rows[0][0] != DBNull.Value)
                {
                    total = Convert.ToDecimal(rows[0][0]);
                }
                return total;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du calcul de la somme des loyers pour la box et la personne : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        public static decimal GetResteApayer(Connexion connexion, int idBox, int idPerson, int mois, int annee)
        {
            var contrat = GetActiveContract(connexion, idBox, idPerson, mois, annee);
            if (contrat != null)
            {
                decimal sumLoyer = SumLoyerMoisAnneeBoxPerson(connexion, idBox, idPerson, mois, annee);
                decimal loyerApayer = Box.GetLoyerBox(connexion, idBox, mois, annee);
                return loyerApayer > sumLoyer ? loyerApayer - sumLoyer : 0;
            }
            return 0;
        }

        public static decimal sumNaloanyPersonByDate(Connexion connexion, int idPerson, int mois, int annee)
        {
            try
            {
                string query = $"SELECT SUM(MONTANT) FROM PAIEMENT WHERE IDPERSON = {idPerson} AND YEAR(DATEPAIEMENT) * 12 + MONTH(DATEPAIEMENT) <= {annee} *12 + {mois}";
                var rows = connexion.ExecuteQuery(query);
                decimal total = 0;

                if (rows.Count > 0 && rows[0][0] != DBNull.Value)
                {
                    total = Convert.ToDecimal(rows[0][0]);
                }
                return total;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du calcul de la somme payée par personne : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        // Méthode pour calculer le montant avec pénalité de retard
        public static decimal CalculerMontantAvecRetard(decimal montantInitial, int moisRetard, decimal pourcentagePenalite)
        {
            try
            {
                if (moisRetard <= 0)
                {
                    return montantInitial;
                }

                if (pourcentagePenalite < 0)
                {
                    Console.WriteLine("Le pourcentage de pénalité ne peut pas être négatif. Utilisation de 0.");
                    pourcentagePenalite = 0;
                }

                decimal tauxPenalite = pourcentagePenalite / 100m;
                decimal montantAvecRetard = montantInitial;
                decimal montantBase = montantInitial;

                for (int i = 0; i < moisRetard; i++)
                {
                    montantAvecRetard = montantBase + (montantBase * tauxPenalite);
                    montantBase = montantAvecRetard;
                }

                Console.WriteLine($"Calcul retard: Montant initial={montantInitial}, " +
                                 $"Mois de retard={moisRetard}, Pourcentage pénalité={pourcentagePenalite}%, " +
                                 $"Montant avec retard={montantAvecRetard}");
                return montantAvecRetard;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du calcul du montant avec retard : {ex.Message}");
                return montantInitial;
            }
        }

        public static decimal SommeTokonyNaloanyByDate(Connexion connexion, int idPerson, int mois, int annee, decimal pourcentagePenalite)
        {
            var allContrat = Location.GetAllContratPerson(connexion, idPerson);
            DateTime? maxDate = Location.GetMaxDateFinContratPerson(connexion, idPerson);

            int maxDateMois = maxDate?.Month ?? mois;
            int maxDateAnnee = maxDate?.Year ?? annee;

            int maxDateT = maxDateAnnee * 12 + maxDateMois;
            int moisT = annee * 12 + mois;
            int moisTRef = moisT; // On utilise la date cible (mois/annee) comme référence

            decimal totalDettes = 0;

            foreach (var contrat in allContrat)
            {
                int moisDeb = contrat.DateDebut.Month;
                int anneeDeb = contrat.DateDebut.Year;
                int moisDebT = anneeDeb * 12 + moisDeb;

                while (moisDebT <= moisTRef)
                {
                    var misyContrat = Paiement.GetActiveContract(connexion, contrat.IdBox, idPerson, moisDeb, anneeDeb);
                    decimal resteBase;
                    if (misyContrat != null)
                    {
                        resteBase = Box.GetLoyerBox(connexion, contrat.IdBox, moisDeb, anneeDeb);
                    }
                    else
                    {
                        resteBase = 0;
                    }

                    // Calculer le retard par rapport à mois et annee fournis
                    int moisRetard = (annee * 12 + mois) - (anneeDeb * 12 + moisDeb);

                    // Appliquer la pénalité si nécessaire
                    decimal resteAvecRetard = CalculerMontantAvecRetard(resteBase, moisRetard, pourcentagePenalite);

                    Console.WriteLine($"Reste pour idBox={contrat.IdBox}, mois={moisDeb}, année={anneeDeb} : " +
                                    $"base={resteBase}, retard={moisRetard} mois, avec pénalité={resteAvecRetard}");

                    totalDettes += resteAvecRetard;

                    moisDeb++;
                    if (moisDeb > 12)
                    {
                        moisDeb = 1;
                        anneeDeb++;
                    }
                    moisDebT = anneeDeb * 12 + moisDeb;
                }
            }

            Console.WriteLine($"Somme totale tokony aloany pour idPerson={idPerson} jusqu'à {mois}/{annee} " +
                            $"avec pénalités ({pourcentagePenalite}%) : {totalDettes}");
            return totalDettes;
        }

        public static decimal SommeDetteTokonyNaloany(Connexion connexion, int idPerson, int mois, int annee, decimal pourcentagePenalite)
        {
            decimal efaVoaloa = sumNaloanyPersonByDate(connexion, idPerson, mois, annee);
            decimal tokonyAloa = SommeTokonyNaloanyByDate(connexion, idPerson, mois, annee, pourcentagePenalite);
            decimal totalDette = tokonyAloa - efaVoaloa;

            Console.WriteLine($"Dette totale pour idPerson={idPerson} jusqu'à {mois}/{annee} : " +
                            $"tokony aloa={tokonyAloa}, efa voaloa={efaVoaloa}, total dette={totalDette}");
            return totalDette;
        }

        public static List<int> GetAllBoxApayerPerson(Connexion connexion, int idPerson)
        {
            List<int> boxApayer = new List<int>();
            try
            {
                var allBoxPerson = Box.GetAllBoxPerson(connexion, idPerson);
                foreach (var box in allBoxPerson)
                {
                    var debutContrat = Location.GetDebutContrat(connexion, box.IdBox, idPerson);

                    int mois = Convert.ToInt32(debutContrat[0]);
                    int annee = Convert.ToInt32(debutContrat[1]);

                    decimal reste = GetResteApayer(connexion, box.IdBox, idPerson, mois, annee);
                    if (reste > 0)
                    {
                        boxApayer.Add(box.IdBox);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des boxes à payer : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return boxApayer;
        }

        public static bool CheckFinContrat(Connexion connexion,int idBox,int idPerson,int moisApayer, int anneeApayer, int mois_fin,int annee_fin)
        {
            int moisFinTotal = annee_fin * 12 + mois_fin;
            int moisCourantTotal = anneeApayer * 12 + moisApayer;
            if(moisCourantTotal > moisFinTotal)
            {
                return true;
            }
            return false;
        }

        public static Object[] GetNextContrat(Connexion connexion,int idPerson,int moisFin,int anneeFin)
        {
            try
            {
                string queryNextContract = $@"
                    SELECT TOP 1 MONTH(dateDebut), YEAR(dateDebut)
                    FROM LOCATION 
                    WHERE idPerson = {idPerson} 
                    AND YEAR(dateDebut) * 12 + MONTH(DATEDEBUT) > {anneeFin} * 12 + {moisFin}
                    ORDER BY dateDebut ASC";
                var rows = connexion.ExecuteQuery(queryNextContract);

                if (rows.Count > 0)
                {
                    int moisApayer = Convert.ToInt32(rows[0][0]);
                    int anneeApayer = Convert.ToInt32(rows[0][1]);
                    Console.WriteLine($"Nouveau contrat détecté à partir de {moisApayer}/{anneeApayer}");
                    return new object[]
                    {
                        moisApayer,
                        anneeApayer
                    };
                }
                Console.WriteLine("Aucun nouveau contrat trouvé, arrêt");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du prochain contrat : {ex.Message}");
                return null;
            }
        }

        public static bool RealInsertion(Connexion connexion, int idBox, int idPerson, decimal montant, DateTime date, decimal pourcentagePenalite)
        {
            try
            {
                // Formater la date pour la requête SQL (format Access : yyyy-MM-dd)
                string datePaiement = date.ToString("yyyy-MM-dd");
                decimal montantRestant = montant;
                int currentIdBox = idBox;

                while (montantRestant > 0)
                {
                    // Récupérer la dette la plus ancienne
                    var dettePlusAncienne = Person.GetPlusAncienDette(connexion, idPerson);
                    if (dettePlusAncienne == null)
                    {
                        Console.WriteLine("Aucune dette à payer");
                        break;
                    }

                    int idBoxAncien = Convert.ToInt32(dettePlusAncienne[0]);
                    decimal montantDette = Convert.ToDecimal(dettePlusAncienne[1]);
                    int moisApayer = Convert.ToInt32(dettePlusAncienne[2]);
                    int anneeApayer = Convert.ToInt32(dettePlusAncienne[3]);

                    Console.WriteLine($"Dette la plus ancienne : idBox={idBoxAncien}, mois={moisApayer}, année={anneeApayer}, montant_dette={montantDette}");

                    // Vérifier les autres dettes pour le même mois et année
                    string queryCheckOtherDebts = $@"
                SELECT DETTE.idBox, DETTE.montant, BOX.numeroBox 
                FROM DETTE, BOX 
                WHERE DETTE.idBox = BOX.idBox 
                AND DETTE.idPerson = {idPerson}
                AND DETTE.mois = {moisApayer}
                AND DETTE.annee = {anneeApayer}
                AND DETTE.montant > 0 
                ORDER BY BOX.numeroBox ASC";
                    var otherDebts = connexion.ExecuteQuery(queryCheckOtherDebts);

                    if (otherDebts != null && otherDebts.Count > 0)
                    {
                        // S'il y a plusieurs dettes, prendre celle avec le numeroBox le plus bas qui a encore un reste à payer
                        foreach (var debt in otherDebts)
                        {
                            int idBoxTemp = Convert.ToInt32(debt[0]);
                            decimal montantTemp = Convert.ToDecimal(debt[1]);
                            int numeroBox = Convert.ToInt32(debt[2]);
                            decimal resteIdBox = Paiement.GetResteApayer(connexion, idBoxTemp, idPerson, moisApayer, anneeApayer);

                            if (resteIdBox > 0)
                            {
                                currentIdBox = idBoxTemp;
                                Console.WriteLine($"Priorisation de idBox={currentIdBox} (Box n°{numeroBox}) pour mois={moisApayer}, année={anneeApayer}");
                                break;
                            }
                        }
                        if (currentIdBox == idBox) // Si aucune priorisation n'a été faite
                        {
                            currentIdBox = Convert.ToInt32(otherDebts[0][0]);
                            Console.WriteLine($"Choix par défaut de idBox={currentIdBox} pour mois={moisApayer}, année={anneeApayer}");
                        }
                    }
                    else
                    {
                        currentIdBox = idBoxAncien != 0 ? idBoxAncien : idBox;
                        Console.WriteLine($"Une seule dette ou aucune autre dette, utilisation de idBox={currentIdBox}");
                    }

                    Console.WriteLine($"Box courante sélectionnée : idBox={currentIdBox}");

                    // Calculer le reste à payer avec pénalités
                    decimal loyerBase = Box.GetLoyerBox(connexion, currentIdBox, moisApayer, anneeApayer);
                    int moisPaiement = date.Month;
                    int anneePaiement = date.Year;
                    int moisRetard = (anneePaiement * 12 + moisPaiement) - (anneeApayer * 12 + moisApayer);
                    if (moisRetard < 0) moisRetard = 0; // Pas de retard négatif
                    decimal loyerAvecRetard = Paiement.CalculerMontantAvecRetard(loyerBase, moisRetard, pourcentagePenalite);
                    decimal dejaPaye = Paiement.SumLoyerMoisAnneeBoxPerson(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                    decimal reste = loyerAvecRetard - dejaPaye;
                    if (reste < 0) reste = 0; // Pas de reste négatif
                    Console.WriteLine($"Reste à payer pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer} : " +
                                     $"loyer base={loyerBase}, retard={moisRetard} mois, avec pénalité={loyerAvecRetard}, déjà payé={dejaPaye}, reste={reste}");

                    // Vérifier l'existence d'un contrat actif
                    var contrat = Paiement.GetActiveContract(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                    if (contrat == null)
                    {
                        Console.WriteLine($"Aucun contrat actif pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}, dette ignorée");
                        string queryDeleteDette = $"DELETE FROM DETTE WHERE idPerson = {idPerson} AND idBox = {currentIdBox} AND mois = {moisApayer} AND annee = {anneeApayer}";
                        connexion.ExecuteUpdate(queryDeleteDette);
                        continue;
                    }

                    int moisDebut = Convert.ToInt32(contrat[0]);
                    int anneeDebut = Convert.ToInt32(contrat[1]);
                    int moisFin = Convert.ToInt32(contrat[2]);
                    int anneeFin = Convert.ToInt32(contrat[3]);

                    if (reste > 0)
                    {
                        // Vérifier si la période dépasse la fin du contrat
                        if (Paiement.CheckFinContrat(connexion, currentIdBox, idPerson, moisApayer, anneeApayer, moisFin, anneeFin))
                        {
                            var nextMoisApayer = Paiement.GetNextContrat(connexion, idPerson, moisFin, anneeFin);
                            if (nextMoisApayer == null || nextMoisApayer[0] == null || nextMoisApayer[1] == null)
                            {
                                break;
                            }
                            moisApayer = Convert.ToInt32(nextMoisApayer[0]);
                            anneeApayer = Convert.ToInt32(nextMoisApayer[1]);
                            continue;
                        }

                        // Effectuer le paiement
                        /*decimal paiementActuel = Math.Min(montantRestant, reste);
                        Console.WriteLine($"Paiement actuel : {paiementActuel}");
                        // Forcer le format décimal avec point pour SQL
                        string paiementActuelStr = paiementActuel.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        string queryPaiement = $"INSERT INTO PAIEMENT (idBox, idPerson, mois, annee, montant, datePaiement) " +
                                               $"VALUES ({currentIdBox}, {idPerson}, {moisApayer}, {anneeApayer}, {paiementActuelStr}, '{datePaiement}')";*/


                        // Effectuer le paiement
                        decimal paiementActuel = Math.Min(montantRestant, reste); // Reste en decimal
                        Console.WriteLine($"Paiement actuel : {paiementActuel}");
                        // Forcer en double avec 2 décimales et point pour SQL
                        string paiementActuelStr = ((double)paiementActuel).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                        string queryPaiement = $"INSERT INTO PAIEMENT (idBox, idPerson, mois, annee, montant, datePaiement) " +
                                               $"VALUES ({currentIdBox}, {idPerson}, {moisApayer}, {anneeApayer}, {paiementActuelStr}, '{datePaiement}')";

                        connexion.ExecuteUpdate(queryPaiement);
                        Console.WriteLine($"Paiement de {paiementActuel} inséré pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}");

                        montantRestant -= paiementActuel;
                        Console.WriteLine($"Montant restant après paiement : {montantRestant}");

                        // Mettre à jour la dette
                        decimal nouveauReste = Paiement.GetResteApayer(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                        Console.WriteLine($"Nouveau reste après paiement : {nouveauReste}");
                        Person.UpdateDette(connexion, currentIdBox, idPerson, moisApayer, anneeApayer, nouveauReste);
                    }

                    // Passer au mois suivant
                    moisApayer++;
                    if (moisApayer > 12)
                    {
                        moisApayer = 1;
                        anneeApayer++;
                    }

                    // Vérifier si le nouveau mois dépasse la fin du contrat
                    if (Paiement.CheckFinContrat(connexion, currentIdBox, idPerson, moisApayer, anneeApayer, moisFin, anneeFin))
                    {
                        var nextMoisApayer = Paiement.GetNextContrat(connexion, idPerson, moisFin, anneeFin);
                        if (nextMoisApayer == null || nextMoisApayer[0] == null || nextMoisApayer[1] == null)
                        {
                            continue;
                        }
                        moisApayer = Convert.ToInt32(nextMoisApayer[0]);
                        anneeApayer = Convert.ToInt32(nextMoisApayer[1]);
                        Console.WriteLine($"Nouveau contrat trouvé : mois={moisApayer}, année={anneeApayer}");
                    }

                    Console.WriteLine($"Passage au mois suivant pour idBox={currentIdBox} : {moisApayer}/{anneeApayer}");
                    decimal montantDetteSuivante = Paiement.GetResteApayer(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                    Console.WriteLine($"Reste à payer pour le nouveau mois {moisApayer}/{anneeApayer} : {montantDetteSuivante}");

                    if (montantDetteSuivante >= 0)
                    {
                        string queryNouvelleDette = $"INSERT INTO DETTE (idBox, idPerson, mois, annee, montant) VALUES ({currentIdBox}, {idPerson}, {moisApayer}, {anneeApayer}, {montantDetteSuivante})";
                        connexion.ExecuteUpdate(queryNouvelleDette);
                        Console.WriteLine($"Nouvelle dette de {montantDetteSuivante} insérée pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}");
                    }
                    else
                    {
                        Console.WriteLine($"Aucune dette à insérer pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}");
                    }
                }

                Console.WriteLine("Paiement réussi");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return false;
            }
        }

        /*public static bool RealInsertion(Connexion connexion, int idBox, int idPerson, decimal montant, DateTime date)
        {
            try
            {
                // Formater la date pour la requête SQL (format Access : yyyy-MM-dd)
                string datePaiement = date.ToString("yyyy-MM-dd");
                decimal montantRestant = montant;
                int currentIdBox = idBox;

                while (montantRestant > 0)
                {
                    // Récupérer la dette la plus ancienne
                    var dettePlusAncienne = Person.GetPlusAncienDette(connexion, idPerson);
                    if (dettePlusAncienne == null)
                    {
                        Console.WriteLine("Aucune dette à payer");
                        break;
                    }

                    int idBoxAncien = Convert.ToInt32(dettePlusAncienne[0]);
                    decimal montantDette = Convert.ToDecimal(dettePlusAncienne[1]);
                    int moisApayer = Convert.ToInt32(dettePlusAncienne[2]);
                    int anneeApayer = Convert.ToInt32(dettePlusAncienne[3]);

                    Console.WriteLine($"Dette la plus ancienne : idBox={idBoxAncien}, mois={moisApayer}, année={anneeApayer}, montant_dette={montantDette}");

                    // Vérifier les autres dettes pour le même mois et année
                    string queryCheckOtherDebts = $@"
                        SELECT DETTE.idBox, DETTE.montant, BOX.numeroBox 
                        FROM DETTE, BOX 
                        WHERE DETTE.idBox = BOX.idBox 
                        AND DETTE.idPerson = {idPerson}
                        AND DETTE.mois = {moisApayer}
                        AND DETTE.annee = {anneeApayer}
                        AND DETTE.montant > 0 
                        ORDER BY BOX.numeroBox ASC";
                    var otherDebts = connexion.ExecuteQuery(queryCheckOtherDebts);

                    if (otherDebts != null && otherDebts.Count > 0)
                    {
                        // S'il y a plusieurs dettes, prendre celle avec le numeroBox le plus bas qui a encore un reste à payer
                        foreach (var debt in otherDebts)
                        {
                            int idBoxTemp = Convert.ToInt32(debt[0]);
                            decimal montantTemp = Convert.ToDecimal(debt[1]);
                            int numeroBox = Convert.ToInt32(debt[2]);
                            decimal resteIdBox = Paiement.GetResteApayer(connexion, idBoxTemp, idPerson, moisApayer, anneeApayer);

                            if (resteIdBox > 0)
                            {
                                currentIdBox = idBoxTemp;
                                Console.WriteLine($"Priorisation de idBox={currentIdBox} (Box n°{numeroBox}) pour mois={moisApayer}, année={anneeApayer}");
                                break;
                            }
                        }
                        if (currentIdBox == idBox) // Si aucune priorisation n'a été faite
                        {
                            currentIdBox = Convert.ToInt32(otherDebts[0][0]);
                            Console.WriteLine($"Choix par défaut de idBox={currentIdBox} pour mois={moisApayer}, année={anneeApayer}");
                        }
                    }
                    else
                    {
                        currentIdBox = idBoxAncien != 0 ? idBoxAncien : idBox;
                        Console.WriteLine($"Une seule dette ou aucune autre dette, utilisation de idBox={currentIdBox}");
                    }

                    Console.WriteLine($"Box courante sélectionnée : idBox={currentIdBox}");

                    // Calculer le reste à payer
                    decimal reste = Paiement.GetResteApayer(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                    Console.WriteLine($"Reste à payer pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer} : {reste}");

                    // Vérifier l'existence d'un contrat actif
                    var contrat = Paiement.GetActiveContract(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                    if (contrat == null)
                    {
                        Console.WriteLine($"Aucun contrat actif pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}, dette ignorée");
                        string queryDeleteDette = $"DELETE FROM DETTE WHERE idPerson = {idPerson} AND idBox = {currentIdBox} AND mois = {moisApayer} AND annee = {anneeApayer}";
                        connexion.ExecuteUpdate(queryDeleteDette);
                        continue;
                    }

                    int moisDebut = Convert.ToInt32(contrat[0]);
                    int anneeDebut =  Convert.ToInt32(contrat[1]);
                    int moisFin =  Convert.ToInt32(contrat[2]);
                    int anneeFin = Convert.ToInt32(contrat[3]);

                    if (reste > 0)
                    {
                        // Vérifier si la période dépasse la fin du contrat
                        if (Paiement.CheckFinContrat(connexion, currentIdBox, idPerson, moisApayer, anneeApayer, moisFin, anneeFin))
                        {
                            var nextMoisApayer = Paiement.GetNextContrat(connexion, idPerson, moisFin, anneeFin);
                            if (nextMoisApayer == null || nextMoisApayer[0] == null || nextMoisApayer[1] == null)
                            {
                                break;
                            }
                            moisApayer = Convert.ToInt32(nextMoisApayer[0]);
                            anneeApayer = Convert.ToInt32(nextMoisApayer[1]);
                            continue;
                        }

                        // Effectuer le paiement
                        decimal paiementActuel = Math.Min(montantRestant, reste);
                        Console.WriteLine($"Paiement actuel : {paiementActuel}");
                        string queryPaiement = $"INSERT INTO PAIEMENT (idBox, idPerson, mois, annee, montant, datePaiement) VALUES ({currentIdBox}, {idPerson}, {moisApayer}, {anneeApayer}, {paiementActuel}, '{datePaiement}')";
                        connexion.ExecuteUpdate(queryPaiement);
                        Console.WriteLine($"Paiement de {paiementActuel} inséré pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}");

                        montantRestant -= paiementActuel;
                        Console.WriteLine($"Montant restant après paiement : {montantRestant}");

                        // Mettre à jour la dette
                        decimal nouveauReste = Paiement.GetResteApayer(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                        Console.WriteLine($"Nouveau reste après paiement : {nouveauReste}");
                        Person.UpdateDette(connexion, currentIdBox, idPerson, moisApayer, anneeApayer, nouveauReste);
                    }

                    // Passer au mois suivant
                    moisApayer++;
                    if (moisApayer > 12)
                    {
                        moisApayer = 1;
                        anneeApayer++;
                    }

                    // Vérifier si le nouveau mois dépasse la fin du contrat
                    if (Paiement.CheckFinContrat(connexion, currentIdBox, idPerson, moisApayer, anneeApayer, moisFin, anneeFin))
                    {
                        var nextMoisApayer = Paiement.GetNextContrat(connexion, idPerson, moisFin, anneeFin);
                        if (nextMoisApayer == null || nextMoisApayer[0] == null || nextMoisApayer[1] == null)
                        {
                            continue;
                        }
                        moisApayer = Convert.ToInt32(nextMoisApayer[0]);
                        anneeApayer = Convert.ToInt32(nextMoisApayer[1]);
                        Console.WriteLine($"Nouveau contrat trouvé : mois={moisApayer}, année={anneeApayer}");
                    }

                    Console.WriteLine($"Passage au mois suivant pour idBox={currentIdBox} : {moisApayer}/{anneeApayer}");
                    decimal montantDetteSuivante = Paiement.GetResteApayer(connexion, currentIdBox, idPerson, moisApayer, anneeApayer);
                    Console.WriteLine($"Reste à payer pour le nouveau mois {moisApayer}/{anneeApayer} : {montantDetteSuivante}");

                    if (montantDetteSuivante >= 0)
                    {
                        string queryNouvelleDette = $"INSERT INTO DETTE (idBox, idPerson, mois, annee, montant) VALUES ({currentIdBox}, {idPerson}, {moisApayer}, {anneeApayer}, {montantDetteSuivante})";
                        connexion.ExecuteUpdate(queryNouvelleDette);
                        Console.WriteLine($"Nouvelle dette de {montantDetteSuivante} insérée pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}");
                    }
                    else
                    {
                        Console.WriteLine($"Aucune dette à insérer pour idBox={currentIdBox}, mois={moisApayer}, année={anneeApayer}");
                    }
                }

                Console.WriteLine("Paiement réussi");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return false;
            }
        }*/

        public static decimal GetPourcentageNaloa(Connexion connexion, int idBox, int moisV, int anneeV)
        {
            try
            {
                Console.WriteLine("eto");
                int mois = moisV;
                int annee = anneeV;
                decimal dejaPayer = Paiement.SumLoyerMoisAnneeBox(connexion, idBox, mois, annee);
                decimal loyerApayer = Box.GetLoyerBox(connexion, idBox, mois, annee);
                if (loyerApayer == 0) return 0; // Éviter une division par zéro
                decimal resultat = (dejaPayer * 100) / loyerApayer;
                return resultat;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du calcul du pourcentage payé : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
    }
}
