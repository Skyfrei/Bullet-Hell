using System;
using System.Collections.Generic;
using System.Threading;

namespace PixivAPI
{
    class Program
    {
       static void Main()
       {
           Console.WriteLine("Click Esc to close window");
           var window = new SimpleWindow();
           window.Run();

           Console.WriteLine("All done!");
       }
    }
    
    class SimpleWindow
    {
        public Airship.Player player;
        public List<Airship.EnemyAirships> enemyList;
        public List<Airship.Blueprint.Bullets> bulletList;
        public List<Airship.Blueprint.Bullets> enemyBullets;
        public SFML.Graphics.Text game = new SFML.Graphics.Text($"Game Over! Click space to restart!", new SFML.Graphics.Font("myfont.ttf"));
           
        public int count;
        public int bulletCount = 4;
        public Thread changeBulletThread; 
        public void Run()
        {
            var window = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(800, 1000), "SFML!");
            window.KeyPressed += Window_KeyPressed;

            player = new Airship.Player(40, 3);
            player.Position = new SFML.System.Vector2f(window.Size.X / 2 - 40, 900);

            enemyList = new List<Airship.EnemyAirships>();
            bulletList = new List<Airship.Blueprint.Bullets>();
            enemyBullets = new List<Airship.Blueprint.Bullets>();

            
            for (int i = 0; i < 50; i++)
            {
                bulletList.Add(new Airship.Blueprint.Bullets(player.Position));
            }

            enemyList.Add(new Airship.EnemyAirships());
            enemyList.Add(new Airship.EnemyAirships());
            enemyList.Add(new Airship.EnemyAirships());
            enemyList.Add(new Airship.EnemyAirships());

            for (int i = 0; i < 200; i++)
            {
                enemyBullets.Add(new Airship.Blueprint.Bullets(enemyList[0].Position));
            }

            changeBulletThread = new Thread(new ThreadStart(() => changeBullet(enemyBullets)));

            var scoreText = new SFML.Graphics.Text($"Score: {player.Score}", new SFML.Graphics.Font("myfont.ttf"));
            while (window.IsOpen)
            {
                
                window.Clear();
                count += 1;
                render(window, scoreText);
                window.DispatchEvents();
                window.Display();
            }
        }
        public void render(SFML.Graphics.RenderWindow window, SFML.Graphics.Text scoreText)
        {
            player.update(player.Position, window);
            scoreText.DisplayedString = $"Score: {player.Score}";        
            window.Draw(player.airshipShape);

            foreach (Airship.EnemyAirships element in enemyList)
            {
                window.Draw(element.airshipShape);
                    for (int j = 0; j < enemyBullets.Count; j += 4 )
                    {
                        for (int kek = 0; kek < enemyList.Count; kek++)
                        {
                            enemyBullets[kek + j].thrown = true;
                            enemyBullets[kek + j].Position = enemyList[kek].Position;
                        }  
                    }

                if (count == 150)
                {
                    element.update(window);
                    if (enemyList.IndexOf(element) == enemyList.Count - 1)
                    {
                        count = 0;
                    }
                    
                }
            }

            changeBulletThread.Start();

            for (int v = 0; v < enemyBullets.Count; v++)
            {   
                if (enemyBullets[v].thrown == true)
                {
                        window.Draw(enemyBullets[v].bulletShape);    
                }
            }
            
            for (int i = 0; i < bulletList.Count; i++)
            {      
                if (bulletList[i].thrown == true)
                {
                    window.Draw(bulletList[i].bulletShape); 
                    bulletList[i].update(player.Position);
                }
                
                for (int j = enemyList.Count - 1; j > -1; j--)
                {
                    if (bulletList[i].Position.X + 50 >= enemyList[j].Position.X &&
                        bulletList[i].Position.X - 50 <= enemyList[j].Position.X &&
                        bulletList[i].Position.Y == enemyList[j].Position.Y )
                    {
                       enemyList[j].Health = 20; 
                       Console.WriteLine(enemyList[j].Health);
                        if (enemyList[j].Health <= 0)
                        {
                            player.Score++;
                            enemyList.RemoveAt(j);
                        }
                        bulletList[i].update(player.Position);
                        bulletList[i].thrown = false;
                    }
                    if (enemyList.Count == 0)
                    {
                        enemyList.Add(new Airship.EnemyAirships());
                        enemyList.Add(new Airship.EnemyAirships());
                        enemyList.Add(new Airship.EnemyAirships());
                        enemyList.Add(new Airship.EnemyAirships());
                    }
                }
            }
                for (int j = 0; j < enemyBullets.Count; j++)
                {
                    if (enemyBullets[j].Position.X  + 50 >= player.Position.X &&
                        enemyBullets[j].Position.X -  80 <= player.Position.X &&
                        enemyBullets[j].Position.Y == player.Position.Y )
                    {
                        player.Health = 10;
                        enemyBullets[j].update(enemyList[0].Position);
                        enemyBullets[j].thrown = false;
                        Console.WriteLine(player.Health);
                    }
                }
            window.Draw(scoreText);
        } 

        public void changeBullet(List<Airship.Blueprint.Bullets> enemyBulletsArray)
        {
            for (int v = 0; v < enemyBullets.Count; v++)
            {
                while (enemyBullets[v].Position.Y + 10 <= 1000)
                {
                    if (enemyBullets[v].thrown == true)
                    {
                        enemyBullets[v].update(enemyList[0].Position); 
                    }
                }
            }
        }

        /// <summary>
            /// Function called when a key is pressed
        /// </summary>
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
            if(SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Left))
            {
                player.moveShip(0);
            }
            if(SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Right))
            {
                player.moveShip(1);
            }
            if(SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Space))
            {
                foreach ( Airship.Blueprint.Bullets bullet in bulletList)
                {
                    if (bullet.thrown == false) 
                    {
                        bullet.thrown = true;
                        break;
                    }
                }
            }
            
        }
    }

}
