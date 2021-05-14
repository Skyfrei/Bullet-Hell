using System;
using System.Collections.Generic;

namespace Airship
{
    abstract class Blueprint : SFML.Graphics.CircleShape
    {
        internal int width = 800;
        internal int height = 1000;
        protected int health;
        protected int attack;
        public SFML.Graphics.CircleShape airshipShape;
        public class Bullets
        {
            public SFML.System.Vector2f Position{get; set;}
            public SFML.Graphics.CircleShape bulletShape;
            public bool thrown;
            public int yChange;
            public SFML.System.Vector2f startingPosition{get; set;}
            public Bullets(SFML.System.Vector2f shootAngle)
            {
                this.bulletShape = new SFML.Graphics.CircleShape(5)
                {
                    FillColor = SFML.Graphics.Color.White
                };
                this.thrown = false;
                this.Position = new SFML.System.Vector2f(shootAngle.X + 35, shootAngle.Y - 20);
                this.bulletShape.Position = this.Position;
                this.startingPosition = this.Position;
                
            }
            public void update(SFML.System.Vector2f vectorPosition)
            {
                if (vectorPosition.Y > 700)
                {
                    if (this.Position.Y - 10 >= 0)
                    {
                        this.Position = new SFML.System.Vector2f( vectorPosition.X + 35, this.Position.Y - 10f);
                        this.bulletShape.Position = this.Position;
                    }
                    else if (this.Position.Y <= 0 || this.Position.X >= 800 || this.Position.X <= 0)
                    {
                        this.thrown = false;
                        this.Position = new SFML.System.Vector2f(vectorPosition.X + 35, vectorPosition.Y - 20f);   
                    }
                }
                else
                {
                    if (this.Position.Y + 10 <= 1000)
                    {
                        this.Position = new SFML.System.Vector2f( this.Position.X, this.Position.Y + 0.5f);
                        this.bulletShape.Position = this.Position;
                    }
                    else if (this.Position.Y >= 900 || this.Position.X >= 800 || this.Position.X <= 0)
                    {
                        this.thrown = false;
                        this.Position = new SFML.System.Vector2f(vectorPosition.X - 35, vectorPosition.Y + 20);   
                    }
                }
                   
            }
        }
                public interface Skills{
            int skill1();
        }
    }

    class EnemyAirships : Blueprint, Blueprint.Skills
    {
        private System.Random random = new System.Random(); 
        public int Health
        {
            get{return this.health;}
            set{this.health -= value;}
        }
        public EnemyAirships()
        {
             this.airshipShape = new SFML.Graphics.CircleShape(40, 3)
            {
                FillColor = SFML.Graphics.Color.Blue
            };
            this.Position = new SFML.System.Vector2f( width / 2 - 40, 100);
            this.airshipShape.Position = this.Position;
            this.airshipShape.Rotation = 180.0f;
            this.Rotation = 180.0f;
            this.health = 100;
            this.attack = 20;
        }
        public void update(SFML.Graphics.RenderWindow window)
        {
            int decider = random.Next(2);
            
            if (decider == 0)
            {
                if (this.Position.X - 100 > 0)
                {
                    this.Position = new SFML.System.Vector2f( this.Position.X - 100, 100);
                    this.airshipShape.Position = this.Position;
                }
                else if (this.Position.X - 100 <= 0)
                {
                    this.Position = new SFML.System.Vector2f(80, 100);
                    this.airshipShape.Position = this.Position;
                }
            }
            else if (decider == 1)
            {
                if (this.Position.X + 100 < 720)
                {
                    this.Position = new SFML.System.Vector2f( this.Position.X + 100, 100);
                    this.airshipShape.Position = this.Position;
                } 
                else if (this.Position.X + 100 >= 720)
                {
                    this.Position = new SFML.System.Vector2f(780, 100);
                    this.airshipShape.Position = this.Position;
                } 
                
            }
            
        }

        public int skill1(){

            return 0;
        }
        
    }
    class Player : Blueprint, Blueprint.Skills, Update, MoveShip
    {
        private SFML.System.Vector2f movingDirection;
        public int Score{get; set;}
        public int Health {
            get {return this.health;}
            set {this.health -= value;}
        }
        public Player(float radius, uint pointCount)
        {       
            this.airshipShape = new SFML.Graphics.CircleShape(radius, pointCount)
            {
                FillColor = SFML.Graphics.Color.Blue
            };
            this.health = 100;
            this.attack = 20;
        }
        public void update(SFML.System.Vector2f vector2D, SFML.Graphics.RenderWindow window) {
                this.Position = vector2D;
                this.airshipShape.Position = this.Position;       
        }
        public void moveShip(int direction){
            if(direction == 0 )
            {
                movingDirection = new SFML.System.Vector2f(this.Position.X - 100.0f, this.Position.Y);
                if (this.Position.X - 100 >= 0)
                {
                    this.Position = movingDirection;
                }
                else if (this.Position.X - 100 < 0)
                {
                    this.Position = new SFML.System.Vector2f(0, this.Position.Y);
                }
            }
            else if (direction == 1)
            {
                movingDirection = new SFML.System.Vector2f(this.Position.X + 100.0f, this.Position.Y);
                if (this.Position.X + 100<= 720)
                {
                    this.Position = movingDirection;
                } 
                else if (this.Position.X + 100 > 720)
                {
                    this.Position = new SFML.System.Vector2f(700, this.Position.Y);
                } 
            }
        }
        public int skill1()
        {
            return 20 * this.attack;
        }
    }

    /// <summary>
         /// Interfaces
    /// </summary>
    interface MoveShip{
        void moveShip(int direction);
    }
    interface Update{
        void update(SFML.System.Vector2f vector2D, SFML.Graphics.RenderWindow window);
    }

    interface blockPowerUp{
        int attackUp();
        int healthUp();
    }
}