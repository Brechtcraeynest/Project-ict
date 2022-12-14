using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;

using System.Windows.Threading; // Voeg toe als timer
using System.Diagnostics.Metrics;
using System.Threading;

namespace ProjectICT
{
   
    public partial class MainWindow : Window
    {
        
        // Maak een nieuwe dispatchertimer aan die gametimer noemt
        DispatcherTimer gameTimer = new DispatcherTimer(); 
        // Maak Rectangles aan die als hitboxen werken voor de elementen in het project
        Rect playerHitBox;
        Rect grondHitBox;
        Rect obstakelHitBox;
        Rect coinHitbox;
        Rect coinHitbox2;

        // Maak alle variabelen aan die in het project gebruikt worden
        SerialPort serialPort = new SerialPort();
        bool springen;
        int snelheid = 30;
        int zwaartekracht = 50;
        Random rand = new Random();
        bool gameover = false;
        int score = 0;
        int cointotal = 0;
        


        //integer array die zorgt dat het opstakel een random hoogte heeft
        int[] obstakelPositie = { 320, 310, 300, 305, 315 };

        
        public MainWindow()
        {
            InitializeComponent();
                //Voeg een extra optie toe aan de combobox
                cbxPortName.Items.Add("None");
                //Zet de compoorts erin
                foreach (string s in SerialPort.GetPortNames())
                cbxPortName.Items.Add(s);

                //Zet de focus op mycanvas, anders werkt springen niet
                myCanvas.Focus();
                //Zet het gameEngine event op de game timer tick
                gameTimer.Tick += gameEngine;
                // laat de game timer elke 20ms tikken
                gameTimer.Interval = TimeSpan.FromMilliseconds(20);
                // start het spel
                
            
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            // Als het game over is en enter word ingeduwt, e is een parameter die de key aangeeft
            if (e.Key == Key.Enter && gameover)
            {
                // start het spel
                StartGame();
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            // Als spatie is ingedrukt en springen is true en de y positie is boven 260
            if (e.Key == Key.Space && !springen && Canvas.GetTop(rectPlayer) > 260)
            {
                // zet de integers op de juiste waarden
                springen = true;
                snelheid = 15;
                zwaartekracht = -12;
                
               
            }

        }

        private void StartGame()
        {
                rectAchtergrond.Visibility = Visibility.Visible;
                rectAchtergrond2.Visibility = Visibility.Visible;
                rectCoin.Visibility = Visibility.Visible;
                rectCoin2.Visibility = Visibility.Visible;
                rectGrond.Visibility = Visibility.Visible;
                rectObstakel.Visibility = Visibility.Visible;
                rectPlayer.Visibility = Visibility.Visible;
                lblCoinsText.Visibility = Visibility.Visible;
                lblScoreText.Visibility = Visibility.Visible;

                lblGameOver.Visibility = Visibility.Hidden;

                Canvas.SetLeft(rectAchtergrond, 0); // Zet de eerste achtergrond op 0
                Canvas.SetLeft(rectAchtergrond2, 1262); // Zet de 2de achtergrond op 1262

                Canvas.SetLeft(rectPlayer, 110);
                Canvas.SetTop(rectPlayer, 140);

                Canvas.SetLeft(rectObstakel, 950);
                Canvas.SetTop(rectObstakel, 310);

                Canvas.SetLeft(rectCoin, 500);
                Canvas.SetTop(rectCoin, 320);

                Canvas.SetLeft(rectCoin2, 650);
                Canvas.SetTop(rectCoin2, 320);

                // Zorg dat alle bools en ints gereset zijn
                springen = false;
               
                gameover = false;
                score = 0;
                cointotal = 0;

                // Zet de score en cointaantal klaar en maak de coins zichtbaar
                lblScoreText.Content = $"Score: {score}";
                lblCoinsText.Content = $"Coins: {cointotal}";

                
                // Start de game timer en doe gameover weg
                gameTimer.Start();
            
        }

  

        private void gameEngine(object sender, EventArgs e)
        {   
            //vermijd Magic numbers
            int bewegenNaarRechts = 12;
            int bewegenAchtergrond = 3;

            // beweeg het karakter naar beneden met de zwaartekracht integer
            Canvas.SetTop(rectPlayer, Canvas.GetTop(rectPlayer) + zwaartekracht);
            // Zorg dat de achtergronden met 3 pixels verplaatsen elke tick
            Canvas.SetLeft(rectAchtergrond, Canvas.GetLeft(rectAchtergrond) - bewegenAchtergrond);
            Canvas.SetLeft(rectAchtergrond2, Canvas.GetLeft(rectAchtergrond2) - bewegenAchtergrond);
            // Zorg dat de rectangles met 12 pixels verplaatsen elke tick
            Canvas.SetLeft(rectObstakel, Canvas.GetLeft(rectObstakel) - bewegenNaarRechts);
            Canvas.SetLeft(rectCoin, Canvas.GetLeft(rectCoin) - bewegenNaarRechts);
            Canvas.SetLeft(rectCoin2, Canvas.GetLeft(rectCoin2) - bewegenNaarRechts);
            // Link score text met score integer
            lblScoreText.Content = $"Score: {score}";
            lblCoinsText.Content = $"Coins: {cointotal}";
           

            // Zorg dat de hitboxen overeenkomen met de wpf elementen
            playerHitBox = new Rect(Canvas.GetLeft(rectPlayer), Canvas.GetTop(rectPlayer), rectPlayer.Width, rectPlayer.Height);
            grondHitBox = new Rect(Canvas.GetLeft(rectGrond), Canvas.GetTop(rectGrond), rectGrond.Width, rectGrond.Height);
            obstakelHitBox = new Rect(Canvas.GetLeft(rectObstakel), Canvas.GetTop(rectObstakel), rectObstakel.Width, rectObstakel.Height);
            coinHitbox = new Rect(Canvas.GetLeft(rectCoin), Canvas.GetTop(rectCoin), rectCoin.Width, rectCoin.Height);
            coinHitbox2 = new Rect(Canvas.GetLeft(rectCoin2), Canvas.GetTop(rectCoin2), rectCoin2.Width, rectCoin2.Height);


            if (playerHitBox.IntersectsWith(grondHitBox))
            {
                //Als de rectPlayer op de grond is zet je zwaartekracht op 0
                zwaartekracht = 0;
                // Zet de rectPlayer op de grond
                Canvas.SetTop(rectPlayer, Canvas.GetTop(rectGrond) - rectPlayer.Height);
                springen = false;
              
        
            }
            
            if (playerHitBox.IntersectsWith(obstakelHitBox))
            {
                // Zet gameover als true en stop de gametimer
                gameover = true;
                gameTimer.Stop();

            }

            

            if (playerHitBox.IntersectsWith(coinHitbox) && rectCoin.Visibility == Visibility.Visible)
              {
                 // Voeg toe aan het cointotaal, zet de rectCoin invisible en de variabele als false
                 cointotal++;
                 rectCoin.Visibility = Visibility.Hidden;
                 serialPort.WriteLine($" S:{score} C:{cointotal}");
            }

            if (playerHitBox.IntersectsWith(coinHitbox2) && rectCoin2.Visibility == Visibility.Visible)
              {
                 // Voeg toe aan het cointotaal, zet de rectCoin invisible en de variabele als false
                 cointotal++;
                 serialPort.WriteLine($" S:{score} C:{cointotal}");
                 rectCoin2.Visibility = Visibility.Hidden;
            }
            

            if (springen)
            {
                // Zet zwaartekracht op -9 zodat de rectPlayer omhoog gaat
                zwaartekracht = -9;
                // zet de snelheid wat lager
                snelheid--;
            }
            else
            {
                // Zet anders zwaartekracht op 12
                zwaartekracht = 12;
            }

            if (snelheid < 0)
            {
                springen = false;
            }


            if (Canvas.GetLeft(rectAchtergrond) < -1262)
            {
                // Als de eerste achtergrond volledig doorlopen is, zet hem achter de 2de
                Canvas.SetLeft(rectAchtergrond, Canvas.GetLeft(rectAchtergrond2) + rectAchtergrond2.Width);
            }

            if (Canvas.GetLeft(rectAchtergrond2) < -1262)
            {
                // Hetzelfde voor de 2de achtergrond
                Canvas.SetLeft(rectAchtergrond2, Canvas.GetLeft(rectAchtergrond) + rectAchtergrond.Width);
            }

            if (Canvas.GetLeft(rectObstakel) < -50)
            {
                Canvas.SetLeft(rectObstakel, 950);
                // Kies een random positie zodat het opstakel hoger of lager is 
                Canvas.SetTop(rectObstakel, obstakelPositie[rand.Next(0, obstakelPositie.Length)]);
                // Voeg 1 toe aan de score
                score += 1;
                serialPort.WriteLine($" S:{score} C:{cointotal}");
            }

            if (Canvas.GetLeft(rectCoin) < -40)
            {
                // Zet de rectCoin op de juiste positie en maak ze visible
                Canvas.SetLeft(rectCoin, 500);
                rectCoin.Visibility = Visibility.Visible;
            }

            if (Canvas.GetLeft(rectCoin2) < -40)
            {
                // Zet de rectCoin op de juiste positie en maak ze visible
                Canvas.SetLeft(rectCoin2, 650);
                rectCoin.Visibility = Visibility.Visible;            
            }

            if (gameover)
            {
                // Zet een zwart kader rond het opstakel voor de duidelijkheid
                rectObstakel.Stroke = Brushes.Black;
                rectObstakel.StrokeThickness = 1;

                // Idem met de rectPlayer
                rectPlayer.Stroke = Brushes.Red;
                rectPlayer.StrokeThickness = 1;
                // Zorg voor een game over scherm
                lblScoreText.Content = "   Press Enter to retry";
                lblCoinsText.Content = "   Press Enter to retry";
                lblGameOver.Visibility = Visibility.Visible;
            }
            else
            {
                // Als er geen gameover is zet je alles af
                rectPlayer.StrokeThickness = 0;
                rectObstakel.StrokeThickness = 0;
                lblGameOver.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            serialPort.WriteLine("Daaaag");
            Thread.Sleep(200);
            serialPort.WriteLine("");
            serialPort.Dispose();
        }

        private void cbxPortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                    serialPort.Close();

                if (cbxPortName.SelectedItem.ToString() != "None")
                {
                    serialPort.PortName = cbxPortName.SelectedItem.ToString();
                    serialPort.Open();
                }
                else
                {
                    MessageBox.Show("Kies een COM-poort.", "Fout",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Startgame_Click(object sender, RoutedEventArgs e)
        {
            try {

                    if ((serialPort != null) && (serialPort.IsOpen))
                    {
                  
                    gbxPoort.Visibility = Visibility.Hidden;
                    gbxStart.Visibility = Visibility.Hidden;
                    lblcontrols.Visibility = Visibility.Hidden;
                    StartGame();
                    
                    }
                }
            catch (Exception ex)
            { MessageBox.Show($"Fout met het selecteren van een compoort {ex}"); }
        }

        private void btnTestSerial_Click(object sender, RoutedEventArgs e)
        {
            serialPort.WriteLine("Test");
            var milliseconds = 200;
            Thread.Sleep(milliseconds);
            serialPort.WriteLine("est");
            Thread.Sleep(milliseconds);
            serialPort.WriteLine("st");
            Thread.Sleep(milliseconds);
            serialPort.WriteLine("t");
            Thread.Sleep(milliseconds);
            serialPort.WriteLine("");
        }
    }
}