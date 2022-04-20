using System;
using System.Linq;
using Fjord;
using Fjord.Modules.Debug;
using Fjord.Modules.Game;
using Fjord.Modules.Graphics;
using Fjord.Modules.Input;
using Fjord.Modules.Mathf;
using Fjord.Modules.Sound;
using static SDL2.SDL;

namespace Flappy_Bord {
    // Scene class this is where your game is 

    class pipel {
        public V2f position;
        public bool can_add_p;

        public pipel(V2f pos) {
            can_add_p = true;
            position = pos;
        }
    }

    public class main : scene
    {
        bird player = new bird();

        pipel[] p = new pipel[6];

        texture pipe = new texture("pipe.png");
        V2 pipe_size = new V2(0, 0);
        
        short points = 0;
        short high_score = 0;

        texture background = new texture("background.png");

        public main() {
            pipe_size = pipe.get_size();

            Random rand = new Random();

            for(var i = 0; i < p.Length; i++) {
                p[i] = new pipel(new V2f(500 + 60 * i, rand.Next(30, 170)));
            }
        }

        public override void on_load()
        {
            game.MAX_FPS = 60;
            draw.load_font("Arcadia");

            Sound.load_sound("jump", "jump");
            Sound.load_sound("die", "die");
            Sound.load_sound("point", "point");

            // This is where you load all your scenes 
            // The if statement is so that it doesn't trigger multiple times

            game.set_render_resolution(game.renderer, 360, 203);

            if(!scene_handler.get_scene("game-template")) {

                // Add all scenes
                scene_handler.add_scene("game-template", new main());

                // Load the first scene this can later be called in any file as for example a win condition to switch scene.
                scene_handler.load_scene("game-template");
            }
        }

        // Update method
        // This is where all your gamelogic is

        public override void update()
        {
            player.update();

            for(var i = 0; i < p.Length; i++) {
                if(!(player.get<Transform>().position.y < p[i].position.y + 30 && player.get<Transform>().position.y > p[i].position.y - 35)) {
                    if(p[i].position.x > 92 && p[i].position.x < 102) {
                        restart();
                        Sound.play_sound("die");
                    }
                } else {
                    if(p[i].position.x > 99 && p[i].position.x < 100) {
                        if(p[i].can_add_p) {
                            p[i].can_add_p = false;
                            points++;
                            Sound.play_sound("point");
                        }
                    }
                }
 
                p[i].position.x -= 4f * (float)game.delta_time;
                if(p[i].position.x < 0 - pipe_size.x) {
                    p[i].position.x = 360;
                    p[i].can_add_p = true;
                }

                if(points > high_score)
                    high_score = points;
            }

            if(input.get_key_just_pressed(input.key_r)) {
                restart();
            }

            if(input.get_key_just_pressed(input.key_escape)) {
                game.stop();
            }

            base.update();
        }

        // Render method
        // This is where all your rendering is

        public override void render()
        {
            draw.texture(new V2(0, 0), background);

            player.render();

            for(var i = 0; i < p.Length; i++) {
                pipe.set_fliptype(flip_type.none);
                draw.texture(new V2((int)p[i].position.x, (int)p[i].position.y + 30), pipe);
                pipe.set_fliptype(flip_type.vertical);
                draw.texture(new V2((int)p[i].position.x, (int)p[i].position.y - 35 - pipe_size.y), pipe);
            } 

            draw.text(new V2(10, 5), "Arcadia", 6, "Score: " + points.ToString());       
            draw.text(new V2(10, 11), "Arcadia", 6, "High Score: " + high_score.ToString());     

            base.render();   
        }

        public void restart() {
            player = new bird();

            Random rand = new Random();

            for(var i = 0; i < p.Length; i++) {
                p[i] = new pipel(new V2f(500 + 60 * i, rand.Next(30, 170)));
            }

            points = 0;
        }
    }

    // Main Class

    class Program 
    {
        public static void Main(string[] args) 
        {
            // Function that starts game
            // The parameter should be your start scene
            game.set_resource_folder("resources");
            game.set_asset_pack("main");
            game.run(new main());
        }
    }
}