
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
 
using System.Windows.Threading; // Voeg toe als timer

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
        bool springen;
        int snelheid = 30;
        int zwaartekracht = 50;
        Random rand = new Random();
        bool gameover = false;
        int score = 0;
        int cointotal = 0;
        bool vis = true;
        bool vis2 = true;

        //integer array die zorgt dat het opstakel een random hoogte heeft
        int[] obstakelPositie = { 320, 310, 300, 305, 315 };
        
        

        public MainWindow()
        {
            InitializeComponent();

            //Zet de focus op mycanvas, anders werkt springen niet
            myCanvas.Focus();

            //Zet het gameEngine event op de game timer tick
            gameTimer.Tick += gameEngine;
            // laat de game timer elke 20ms tikken
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            // start het spel
            StartGame();

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
            if (e.Key == Key.Space && !springen && Canvas.GetTop(player) > 260)
            {
                // zet de integers op de juiste waarden
                springen = true;
                snelheid = 15;
                zwaartekracht = -12;
              
               
            }

        }

        private void StartGame()
        {

            Canvas.SetLeft(background, 0); // Zet de eerste achtergrond op 0
            Canvas.SetLeft(background2, 1262); // Zet de 2de achtergrond op 1262

            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 140);

            Canvas.SetLeft(obstacle, 950);
            Canvas.SetTop(obstacle, 310);

            Canvas.SetLeft(coin, 500);
            Canvas.SetTop(coin, 320);

            Canvas.SetLeft(coin2, 650);
            Canvas.SetTop(coin2, 320);

            // Zorg dat alle bools en ints gereset zijn
            springen = false;
            vis = true;
            vis2 = true;
            gameover = false;
            score = 0;
            cointotal = 0;

            // Zet de score en cointaantal klaar en maak de coins zichtbaar
            scoreText.Content = $"Score: {score}";
            coinsText.Content = $"Coins: {cointotal}";

            coin.Visibility = Visibility.Visible;
            coin2.Visibility = Visibility.Visible;
            // Start de game timer en doe gameover weg
            gameTimer.Start();
            lblGameOver.Visibility = Visibility.Hidden;
        }

  

        private void gameEngine(object sender, EventArgs e)
        {
            // beweeg het karakter naar beneden met de zwaartekracht integer
            Canvas.SetTop(player, Canvas.GetTop(player) + zwaartekracht);
            // Zorg dat de achtergronden met 3 pixels verplaatsen elke tick
            Canvas.SetLeft(background, Canvas.GetLeft(background) - 3);
            Canvas.SetLeft(background2, Canvas.GetLeft(background2) - 3);
            // Zorg dat de rectangles met 12 pixels verplaatsen elke tick
            Canvas.SetLeft(obstacle, Canvas.GetLeft(obstacle) - 12);
            Canvas.SetLeft(coin, Canvas.GetLeft(coin) - 12);
            Canvas.SetLeft(coin2, Canvas.GetLeft(coin2) - 12);
            // Link score text met score integer
            scoreText.Content = $"Score: {score}";
            coinsText.Content = $"Coins: {cointotal}";
           

            // Zorg dat de hitboxen overeenkomen met de wpf elementen
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            grondHitBox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width, ground.Height);
            obstakelHitBox = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);
            coinHitbox = new Rect(Canvas.GetLeft(coin), Canvas.GetTop(coin), coin.Width, coin.Height);
            coinHitbox2 = new Rect(Canvas.GetLeft(coin2), Canvas.GetTop(coin2), coin2.Width, coin2.Height);


            if (playerHitBox.IntersectsWith(grondHitBox))
            {
                //Als de player op de grond is zet je zwaartekracht op 0
                zwaartekracht = 0;
                // Zet de player op de grond
                Canvas.SetTop(player, Canvas.GetTop(ground) - player.Height);
                springen = false;
              
        
            }

            if (playerHitBox.IntersectsWith(obstakelHitBox))
            {
                // Zet gameover als true en stop de gametimer
                gameover = true;
                gameTimer.Stop();

            }

            

            if (playerHitBox.IntersectsWith(coinHitbox) && vis == true)
              {
                 // Voeg toe aan het cointotaal, zet de coin invisible en de variabele als false
                 cointotal++;
                 coin.Visibility = Visibility.Hidden;
                 vis = false;

            }

            if (playerHitBox.IntersectsWith(coinHitbox2) && vis2 == true)
              {
                 // Voeg toe aan het cointotaal, zet de coin invisible en de variabele als false
                 cointotal++;
                 coin2.Visibility = Visibility.Hidden;
                 vis2 = false;
            }
            

            if (springen)
            {
                // Zet zwaartekracht op -9 zodat de player omhoog gaat
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


            if (Canvas.GetLeft(background) < -1262)
            {
                // Als de eerste achtergrond volledig doorlopen is, zet hem achter de 2de
                Canvas.SetLeft(background, Canvas.GetLeft(background2) + background2.Width);
            }

            if (Canvas.GetLeft(background2) < -1262)
            {
                // Hetzelfde voor de 2de achtergrond
                Canvas.SetLeft(background2, Canvas.GetLeft(background) + background.Width);
            }

            if (Canvas.GetLeft(obstacle) < -50)
            {
                Canvas.SetLeft(obstacle, 950);
                // Kies een random positie zodat het opstakel hoger of lager is 
                Canvas.SetTop(obstacle, obstakelPositie[rand.Next(0, obstakelPositie.Length)]);
                // Voeg 1 toe aan de score
                score += 1;
            }

            if (Canvas.GetLeft(coin) < -50)
            {
                // Zet de coin op de juiste positie en maak ze visible
                Canvas.SetLeft(coin, 500);
                coin.Visibility = Visibility.Visible;
                vis = true;
            }

            if (Canvas.GetLeft(coin2) < -50)
            {
                // Zet de coin op de juiste positie en maak ze visible
                Canvas.SetLeft(coin2, 650);
                coin.Visibility = Visibility.Visible;
                vis2 = true;               
            }

            if (gameover)
            {
                // Zet een zwart kader rond het opstakel voor de duidelijkheid
                obstacle.Stroke = Brushes.Black;
                obstacle.StrokeThickness = 1;

                // Idem met de player
                player.Stroke = Brushes.Red;
                player.StrokeThickness = 1;
                // Zorg voor een game over scherm
                scoreText.Content = "   Press Enter to retry";
                coinsText.Content = "   Press Enter to retry";
                lblGameOver.Visibility = Visibility.Visible;
            }
            else
            {
                // Als er geen gameover is zet je alles af
                player.StrokeThickness = 0;
                obstacle.StrokeThickness = 0;
                lblGameOver.Visibility = Visibility.Hidden;
            }
        }

    }
}