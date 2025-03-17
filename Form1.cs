using tsenaFinal.db;
using tsenaFinal.models;

namespace tsenaFinal
{
    public partial class Form1 : Form
    {
        public Connexion con;
        public Form1(Connexion con)
        {
            InitializeComponent();
            this.con = con;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void verificationMois(object sender, EventArgs e)
        {
            int.TryParse(moisVerifierText.Text,out int mois);
            int.TryParse(anneeVerifierText.Text,out int annee);

            List<Person> allPerson = Person.GetAll(this.con);
            foreach(var person in allPerson)
            {
                var dette = Paiement.SommeDetteTokonyNaloany(this.con,person.IdPerson,mois,annee);
                MessageBox.Show($"Dette de {person.IdPerson} est {dette}");

            }


        }
    }
}
