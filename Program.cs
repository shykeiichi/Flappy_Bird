using System.Linq;
using Fjord;
using Fjord.Modules.Debug;
using Fjord.Modules.Game;
using Fjord.Modules.Graphics;
using Fjord.Modules.Input;
using Fjord.Modules.Mathf;
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
        IntPtr pipe = texture_handler.load_texture("pipe.png");
        V2 pipe_size = new V2(0, 0);
        
        short points = 0;
        short high_score = 0;

        IntPtr background = texture_handler.load_texture("background.png");

        public main() {

            uint f; int a;
            SDL_QueryTexture(pipe, out f, out a, out pipe_size.x, out pipe_size.y);

            Random rand = new Random();

            for(var i = 0; i < p.Length; i++) {
                p[i] = new pipel(new V2f(500 + 60 * i, rand.Next(30, 170)));
            }
        }

        public override void on_load()
        {

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
                if(!(player.get_component<Transform>().position.y < p[i].position.y + 30 && player.get_component<Transform>().position.y > p[i].position.y - 35)) {
                    if(p[i].position.x > 92 && p[i].position.x < 102) {
                        restart();
                    }
                } else {
                    if(p[i].position.x > 99 && p[i].position.x < 100) {
                        if(p[i].can_add_p) {
                            p[i].can_add_p = false;
                            points++;
                        }
                    }
                }
 
                p[i].position.x -= 4f * (float)game.delta_time;
                if(p[i].position.x < 0 - pipe_size.x) {
                    p[i].position.x = 360;
                }

                if(points > high_score)
                    high_score = points;
            }

            if(input.get_key_just_pressed(input.key_escape)) {
                restart();
            }
        }

        // Render method
        // This is where all your rendering is

        public override void render()
        {
            draw.texture(background, 0, 0, 0);

            player.render();

            for(var i = 0; i < p.Length; i++) {
                draw.texture_ext(pipe, (int)p[i].position.x, (int)p[i].position.y + 30, 0, 1, 1, false);
                draw.texture_ext(pipe, (int)p[i].position.x, (int)p[i].position.y - 35 - pipe_size.y, 0, 1, 1, false, draw_origin.TOP_LEFT, flip_type.vertical);
            } 

            draw.text(10, 5, "default", 6, "Score: " + points.ToString());       
            draw.text(10, 11, "default", 6, "High Score: " + high_score.ToString());        
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
            game.run(new main());
        }
    }
}