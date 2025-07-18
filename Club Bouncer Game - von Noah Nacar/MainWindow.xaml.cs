using System.Windows;
using System.Windows.Threading;

namespace Club_Bouncer_Game___von_Noah_Nacar
{

    public partial class MainWindow : Window
    {

        private int leben = 3; // leben 
        private int punkte = 0; // punkte 
        private bool istGastBetrunken; // check ob gast betrunken ist 
        private Random zufall = new Random(); // generiert zufällig texte die betrunken oder nicht betrunken sein können 
        private DispatcherTimer zeitZumEntscheiden; // check wie viel zeit man über hat (( habe ich neu gelernt kenne mich jetzt nicht zu 100% mit aus )) 
        private bool sichEntschieden; // check ob der user sich bereits entschieden hat 

        private List<string> worteNichtBetrunken = new List<string> // eine liste an strings mit worten die nicht betrunken sind 
        {
            "Guten Abend, Darf man rein ?" , "hey habe reserviert, kann ich durch?" , "Ich stehe auf der liste darf man durch ?",
        };

        private List<string> wortetBetrunken = new List<string> // eine liste an strings mit worten die betrunken sind 
        {
            "daRF icH Reeehin ?" , "guude n arbEn ka... kkann ich rein ?" , "LaSsch micH D.D..dUrch"
        };

        private void do_timerStarten()
        {
            zeitZumEntscheiden = new DispatcherTimer();
            zeitZumEntscheiden.Interval = TimeSpan.FromSeconds(3); // setzt die zeit auf 3 sekunden 
            zeitZumEntscheiden.Tick += zeitZumEntscheiden_Tick;
        }

        private void zeitZumEntscheiden_Tick(object sender, EventArgs e)
        {
            zeitZumEntscheiden.Stop();
            
            if (!sichEntschieden)
            {
                if(leben < 3)
                {
                    leben--;
                }
                do_checkSpielbar();
                if (leben == 0)
                    do_neuenGastHinzufuegen();
            }
        }

        private void do_updateLebenUndPunkteAnzeige() // Fertig
        {
            txtLeben.Text = "Leben: " + leben;
            txtPunkte.Text = "Punkte: " + punkte;
        }

        private void do_neuenGastHinzufuegen()
        {
            sichEntschieden = false;
            txtStatus.Text = "";

            istGastBetrunken = zufall.Next(2 ) == 0; // bestimmt zufällig ob der gast betrunken ist oder nicht 
            if ( istGastBetrunken )
            {
                txtGast.Text = wortetBetrunken[zufall.Next(wortetBetrunken.Count)];
            }
            else
            {
                txtGast.Text = worteNichtBetrunken[zufall.Next(worteNichtBetrunken.Count)];
            }

            do_updateLebenUndPunkteAnzeige();
            zeitZumEntscheiden.Start();
        }

        private void do_checkEntscheidung(bool reinlassen) // hier wird geschaut ob die antwort falsch oder richtig ist  und ob überhaupt neue gäste hinzugefügt werden sollen 
        {
            zeitZumEntscheiden.Stop();
            sichEntschieden = true;

            if ((istGastBetrunken && reinlassen) || ((!istGastBetrunken) && !reinlassen))
            {
                leben--;
                txtStatus.Text = "Falsch";
            }
            else
            {
                punkte++;
                txtStatus.Text = "Richtig";
            }
            do_checkSpielbar();
            if (leben >0)
                do_neuenGastHinzufuegen();
        }

        private void do_checkSpielbar()
        {
            do_updateLebenUndPunkteAnzeige(); // Checkt ob man weiter spielen kann wenn nicht game over / game vorbei
            if (leben <= 0)
            {
                txtGast.Text = "Game Over";
                txtStatus.Text = "DU HAST VERLOREN";
                btnReinlassen.IsEnabled = false;
                btnAblehnen.IsEnabled = false;
            }
        }

        private void btnReinlassen_Click(object sender, RoutedEventArgs e)
        {
            do_checkEntscheidung(true);
        }

        private void btnAblehnen_Click(object sender, RoutedEventArgs e)
        {
            do_checkEntscheidung(false);
        }


        public MainWindow()
        {
            InitializeComponent();
            do_timerStarten();
            do_neuenGastHinzufuegen();

        }
    }
}