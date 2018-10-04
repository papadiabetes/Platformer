﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump_boi
{
    class Collision
    {
        public Game1 game;

        public bool IsColliding(Sprite hero, Sprite otherSprite)
        {
            if (hero.rightEdge < otherSprite.leftEdge ||
                hero.leftEdge > otherSprite.rightEdge ||
                hero.bottomEdge < otherSprite.topEdge ||
                hero.topEdge > otherSprite.bottomEdge)
            {
                return false;
            }
            return true;
        }

        bool CheckForTile( Vector2 coordinates) // Checks if there is a tile at the specified coodinates 
        {
            int column = (int)coordinates.X;
            int row = (int)coordinates.Y;
            
            if (column < 0 || column > game.levelTileWidth - 1)
            {
                return false;
            }
            if (row < 0 || row > game.levelTileHeight - 1)
            {
                return true;
            }
            Sprite tileFound = game.levelGrid[column, row];

            if (tileFound != null)
            {
                return true;
            }
            return false;


        }
        Sprite CollideLeft( Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X < 0)
            {
                hero.position.X = tile.rightEdge + hero.offset.X;
                hero.velocity.X = 0;
            }


            return hero;
        }

        Sprite CollideRight(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X > 0)
            {
                hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                hero.velocity.Y = 0;
            }
            return hero;
        }


        Sprite CollideBottomDiagonals(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int topEdgeDistance = Math.Abs(tile.topEdge - playerPrediction.bottomEdge);
            

            if (IsColliding(playerPrediction, tile) == true)
            {
                if (topEdgeDistance < rightEdgeDistance && topEdgeDistance < leftEdgeDistance)
                {
                    //if the top edge is closest, collision is happening to the right of the platform 
                    hero.position.Y = tile.topEdge - hero.height + hero.offset.Y;
                    hero.velocity.Y = 0;
                }
                else if (rightEdgeDistance < leftEdgeDistance)
                {
                    // if the right edge is closest, the collision is happening to the right of the platform
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    hero.velocity.X = 0;
                }
                else
                {
                    //if the left is the closest, the collision is happening to the left of the platform
                    hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                    hero.velocity.X = 0;
                }

                
            }
            return hero;
        }
        Sprite CollideAboveDiagonals(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int rightEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int bottomEdgeDistance = Math.Abs(tile.bottomEdge - playerPrediction.topEdge);
            if (IsColliding(playerPrediction, tile) == true)
            {
                if (bottomEdgeDistance < leftEdgeDistance && bottomEdgeDistance < rightEdgeDistance)
                {
                    hero.position.Y = tile.bottomEdge + hero.offset.Y;
                    hero.velocity.Y = 0;
                }
                else if (leftEdgeDistance < rightEdgeDistance)
                {
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    hero.velocity.X = 0;
                }
                else
                {
                    hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                    hero.velocity.X = 0;
                }
            }
            return hero;
        }

        Sprite CollideBelow(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int rightEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int bottomEdgeDistance = Math.Abs(tile.bottomEdge = playerPrediction.topEdge);
            if (IsColliding(playerPrediction, tile) == true)
            {
                if (bottomEdgeDistance < leftEdgeDistance && bottomEdgeDistance < rightEdgeDistance)
                {
                    hero.position.Y = tile.bottomEdge + hero.offset.Y;
                    hero.velocity.Y = 0;
                }
                else if (leftEdgeDistance < rightEdgeDistance)
                {
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    hero.velocity.X = 0;
                }
                else 
                {
                    hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                    hero.velocity.X = 0;
                }
                
            }
            return hero;
        }
        public Sprite CollideWithPlatforms(Sprite hero, float deltatime)
        {
            // Create a copy of the hero that will move to where the hero will be in the next frame.
            Sprite playerPrediction = new Sprite();
            playerPrediction.position = hero.position;
            playerPrediction.width = hero.width;
            playerPrediction.height = hero.height;
            playerPrediction.offset = hero.offset;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.velocity * deltatime;

            int playerColum = (int)Math.Round(playerPrediction.position.X / game.tileHeight);
            int playerRow = (int)Math.Round(playerPrediction.position.Y / game.tileHeight);

            Vector2 playerTile = new Vector2(playerColum, playerRow);

            Vector2 leftTile = new Vector2(playerTile.X - 1, playerTile.Y);
            Vector2 rightTile = new Vector2(playerTile.X + 1, playerTile.Y);
            Vector2 topTile = new Vector2(playerTile.X, playerTile.Y - 1);
            Vector2 bottomTile = new Vector2(playerTile.X, playerTile.Y = 1);

            Vector2 bottomLeftTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            Vector2 bottomRightTile = new Vector2(playerTile.X + 1, playerTile.Y + 1);
            Vector2 topRightTile = new Vector2(playerTile.X - 1, playerTile.Y - 1);
            Vector2 topLefttTile = new Vector2(playerTile.X + 1, playerTile.Y - 1);
            // .. This allows us to predict if the hero will be overlapping an obstacle in the next frame

            bool leftCheck = CheckForTile(leftTile);
            bool rightCheck = CheckForTile(rightTile);
            bool topCheck = CheckForTile(topTile);
            bool bottomCheck = CheckForTile(bottomTile);

            bool bottomLeftCheck = CheckForTile(bottomLeftTile);
            bool bottomRightCheck = CheckForTile(bottomRightTile);
            bool topLeftCheck = CheckForTile(topLefttTile);
            bool topRightCheck = CheckForTile(topRightTile);

            if (leftCheck == true) //Check for collisions with 
            {
                hero = CollideLeft(hero, leftTile, playerPrediction);
            }

            if (leftCheck == false && bottomCheck == false && bottomLeftCheck == true)
            {
                hero = CollideBottomDiagonals(hero, bottomLeftTile, playerPrediction);
            }

            if (rightCheck == false && bottomCheck == false && bottomRightCheck == true)
            
            {
                hero = CollideBottomDiagonals(hero, bottomRightTile, playerPrediction);
            }


            if (leftCheck == false && topCheck == false && topLeftCheck)
            {
                hero = CollideAboveDiagonals(hero, topLefttTile, playerPrediction);
            }

            return hero;
        }
    }
}
