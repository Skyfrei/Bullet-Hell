﻿using System;
using System.Collections.Generic;

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

                
                if (count == 150){
                    enemyBullets[0].thrown = true;
                    for (int i = 0; i < enemyBullets.Count; i++)
                    {
                        if (enemyBullets[i].thrown == true)
                        {
                            window.Draw(enemyBullets[i].bulletShape);
                            enemyBullets[i].update(element.Position);
                        }
                    }
                    element.update(window);
                    if (enemyList.IndexOf(element) == enemyList.Count - 1)
                    {
                        count = 0;
                    }
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
                }
            }

            window.Draw(scoreText);
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
            if(e.Code == SFML.Window.Keyboard.Key.Left)
            {
                player.moveShip(0);
            }
            if(e.Code == SFML.Window.Keyboard.Key.Right)
            {
                player.moveShip(1);
            }
            if(e.Code == SFML.Window.Keyboard.Key.Space)
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
