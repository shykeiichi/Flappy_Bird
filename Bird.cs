using Fjord.Modules.Game;

namespace Flappy_Bord {
    class bird : entity {
        public bird() {
            add_component(new Transform(), this);
            add_component(new Sprite_Renderer(), this);
            add_component(new bird_controller(), this);
        }
    }
}