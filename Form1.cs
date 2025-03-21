using tsenaFinal.db;
using tsenaFinal.models;
using System.Drawing;
using System.Windows.Forms;

namespace tsenaFinal
{
    public partial class Form1 : Form
    {
        public Connexion con;
        public int echelle = 10;
        public int nbPourcentage = 5;
        public Form1(Connexion con)
        {
            InitializeComponent();

            this.con = con;
            LoadPersonComboBox(); // Charger les personnes dans le ComboBox

            panel1.Invalidate();
            panel4.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            afficherMarche(e.Graphics);
            afficherBoxes(e.Graphics);
        }

        public void afficherMarche(Graphics g)
        {
            List<Marche> marches = Marche.GetAll(con);
            foreach (Marche marche in marches)
            {
                int x1 = marche.X;
                int y1 = marche.Y;
                int x2 = x1 + marche.Width * echelle;
                int y2 = y1 + marche.Height * echelle;

                g.DrawRectangle(new Pen(Color.Black), x1, y1, x2 - x1, y2 - y1);
                g.FillRectangle(new SolidBrush(Color.LightGray), x1, y1, x2 - x1, y2 - y1);
                g.DrawString(marche.NomMarche, new Font("Arial", 10), new SolidBrush(Color.Black),
                    x1 + (marche.Width * echelle) / 2 - 20, y1 + (marche.Height * echelle) / 2 - 10);
                string infoText = $"x:{marche.X}, y:{marche.Y}\nw:{marche.Width}, h:{marche.Height}";
                g.DrawString(infoText, new Font("Arial", 8), new SolidBrush(Color.Navy), x1 + 10, y1 - 30);
            }
        }

        public void afficherBoxes(Graphics g)
        {
            List<Marche> marches = Marche.GetAll(con);
            foreach (Marche marche in marches)
            {
                List<Box> boxes = Marche.GetAllBox(con, marche.IdMarche);
                foreach (Box box in boxes)
                {
                    int widthScaled = box.Width * echelle;
                    int paidWidth = 0;

                    if (!string.IsNullOrWhiteSpace(moisVerifierText.Text) && !string.IsNullOrWhiteSpace(anneeVerifierText.Text))
                    {
                        int mois = int.Parse(moisVerifierText.Text);
                        int annee = int.Parse(anneeVerifierText.Text);
                        decimal pourcentage = Paiement.GetPourcentageNaloa(con, box.IdBox, mois, annee);
                        paidWidth = (int)((pourcentage / 100) * widthScaled);

                        if (tsenaFinal.models.Location.MisyOlonaVe(con, box.IdBox, mois, annee))
                        {
                            g.DrawRectangle(new Pen(Color.Black), box.X, box.Y, paidWidth, box.Height * echelle);
                            g.FillRectangle(new SolidBrush(Color.Green), box.X, box.Y, paidWidth, box.Height * echelle);
                            g.DrawRectangle(new Pen(Color.Black), box.X + paidWidth, box.Y, widthScaled - paidWidth, box.Height * echelle);
                            g.FillRectangle(new SolidBrush(Color.Red), box.X + paidWidth, box.Y, widthScaled - paidWidth, box.Height * echelle);
                        }
                        else
                        {
                            g.DrawRectangle(new Pen(Color.Black), box.X, box.Y, widthScaled, box.Height * echelle);
                            g.FillRectangle(new SolidBrush(Color.Gray), box.X, box.Y, widthScaled, box.Height * echelle);
                        }
                    }
                    else
                    {
                        g.DrawRectangle(new Pen(Color.Black), box.X, box.Y, widthScaled, box.Height * echelle);
                        g.FillRectangle(new SolidBrush(Color.Red), box.X, box.Y, widthScaled, box.Height * echelle);
                    }

                    string infoText = $"id:{box.IdBox}";
                    g.DrawString(infoText, new Font("Arial", 8), new SolidBrush(Color.Navy), box.X + 10, box.Y);
                }
            }
        }

        public void AfficherDettesPersonnes()
        {
            try
            {
                panel4.Visible = true;

                // Vérifier si les champs mois et année sont remplis
                if (!int.TryParse(moisVerifierText.Text, out int mois) || !int.TryParse(anneeVerifierText.Text, out int annee))
                {
                    MessageBox.Show("Veuillez entrer un mois et une année valides.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<Person> allPerson = Person.GetAll(this.con);
                if (allPerson == null || allPerson.Count == 0)
                {
                    MessageBox.Show("Aucune personne trouvée dans la base de données.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                listBox1.Items.Clear(); // Effacer les éléments précédents
                foreach (var person in allPerson)
                {
                    /*var dette = Paiement.SommeDetteTokonyNaloany(this.con, person.IdPerson, mois, annee,nbPourcentage);
                    listBox1.Items.Add($"Personne ID {person.IdPerson} : {dette} Ar (Mois {mois}/{annee}) ");*/
                    var dette = Paiement.SommeDetteTokonyNaloany(this.con, person.IdPerson, mois, annee, nbPourcentage);
                    int detteEntiere = (int)dette; // Conversion en int, tronque les décimales
                    listBox1.Items.Add($"Personne ID {person.IdPerson} : {detteEntiere} Ar (Mois {mois}/{annee})");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'affichage des dettes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void verificationMois(object sender, EventArgs e)
        {
            AfficherDettesPersonnes(); // Appeler la méthode pour afficher les dettes
            panel1.Invalidate(); // Rafraîchir l'affichage des boîtes
        }

        /*private void paiementBox(object sender, EventArgs e)
        {
            //comboxBoxPerson
            int.TryParse(idPersonText.Text, out int idPerson);
            int.TryParse(idBoxText.Text, out int idBox);
            decimal.TryParse(montantText.Text, out decimal montant);
            DateTime datePaiement = datePaiementPicker.Value;
            bool paiement = Paiement.RealInsertion(this.con, idBox, idPerson, montant, datePaiement);
            if (paiement)
            {
                MessageBox.Show("Paiement effectué avec succès");
                panel1.Invalidate();
                //AfficherDettesPersonnes(); // Rafraîchir les dettes après paiement
            }
        }*/

        private void LoadPersonComboBox()
        {
            List<Person> personnes = Person.GetAll(this.con); // Récupère toutes les personnes depuis la base

            comboBox1.DataSource = personnes; // Lier la liste au ComboBox
            comboBox1.DisplayMember = "Nom"; // Affiche le nom
            comboBox1.ValueMember = "IdPerson"; // Stocke l'ID
        }


        /*private void paiementBox(object sender, EventArgs e)
        {
            try
            {
                //comboxBoxPerson comboBox1
                //int.TryParse(idPersonText.Text, out int idPerson);
                int.TryParse(idPersonText.Text, out int idPerson);
                int.TryParse(idBoxText.Text, out int idBox);
                decimal.TryParse(montantText.Text, out decimal montant);
                DateTime datePaiement = datePaiementPicker.Value;

                bool paiement = Paiement.RealInsertion(this.con, idBox, idPerson, montant, datePaiement);

                if (paiement)
                {
                    MessageBox.Show("Paiement effectué avec succès");
                    panel1.Invalidate();
                    //AfficherDettesPersonnes(); // Rafraîchir les dettes après paiement
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors du paiement : {ex.Message}",
                               "Erreur",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }*/

        private void paiementBox(object sender, EventArgs e)
        {
            try
            {
                // Vérifier qu'un élément est sélectionné
                if (comboBox1.SelectedItem is Person selectedPerson)
                {
                    int idPerson = selectedPerson.IdPerson; // Récupérer l'ID de la personne
                    int.TryParse(idBoxText.Text, out int idBox);
                    decimal.TryParse(montantText.Text, out decimal montant);
                    DateTime datePaiement = datePaiementPicker.Value;
                    // Insérer le paiement
                    bool paiement = Paiement.RealInsertion(this.con, idBox, idPerson, montant, datePaiement,nbPourcentage);

                    if (paiement)
                    {
                        MessageBox.Show($"Paiement effectué avec succès de la personne {idPerson}");
                        panel1.Invalidate();
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner une personne valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors du paiement : {ex.Message}",
                               "Erreur",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }


        private void panel3_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }
    }
}