using System;
using Fjord;
using Fjord.Modules.Debug;
using Fjord.Modules.Game;
using Fjord.Modules.Graphics;
using Fjord.Modules.Input;
using Fjord.Modules.Mathf;
using Fjord.Modules.Sound;

namespace Flappy_Bord {
    class bird_controller : component {

        float velocity = 0;
        float gravity = 0.3f;
        float max = 30f;

        public override void on_load()
        {
            parent.get_component<Sprite_Renderer>().texture = texture_handler.load_texture("bird.png");
            parent.get_component<Transform>().position = new V2f(100, 101);
        }

        public override void update() {
            velocity += gravity;

            velocity = Math.Clamp(velocity, -max, max);

            parent.get_component<Transform>().position.y += velocity * game.delta_time;

            if(input.get_key_just_pressed(input.key_space)) {
                velocity = -200;

                Sound.play_sound("jump");
            }
        }

        public override void render()
        {
            base.render();
        }
    }
}